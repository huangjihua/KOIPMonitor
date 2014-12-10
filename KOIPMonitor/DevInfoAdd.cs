using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using Kernel;
using System.IO;
using System.Data;
using MySqlHeader;
using MySql.Data.MySqlClient;

namespace KOIPMonitor
{
    class DevInfoAdd
    {
        public DevInfoAdd() { }
        ~DevInfoAdd() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

//            <ROOT>
//    <ITEM>
//<DEVID>1</ DEVID >
//<DEVNAME></ DEVNAME >
//<DEVTYPE></ DEVTYPE >
//<IP></ IP >
//<PORT></ PORT >
//<UPDEVID></ UPDEVID >
//<DESCR></ DESCR >
//    </ITEM>
//</ROOT>


            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            string DevID = "";      //设备编号
            string DevName = "";    //设备名称
            int DevTypeID = 0;      //设备类型编号
            string IP = "";         //IP地址
            string PORT = "";       //端口号
            string UPDEVID = "";    //上级设备编号
            string DESCR = "";       //说明

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.DevInfoAdd;
                    cmd2 = ErrCommon.Success;
                    byte[] ByteResult = null;
                    DataTable dt = new DataTable();
                    Commonality.CommClass.ReadXML(request.receiveFileTemporarily, ref dt);

                    if (dt.Rows.Count <= 0)
                    {
                        cmd2 = -101;//解包失败
                        //哈希表存放包体内容
                        Hashtable _hashtable_Package_Temp = new Hashtable();
                        _hashtable_Package_Temp.Add("1", request);//...連結位置            
                        _hashtable_Package_Temp.Add("2", cmd1);
                        _hashtable_Package_Temp.Add("3", cmd2);
                        ByteResult = null;
                        _hashtable_Package_Temp.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);
                        Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                                                       "KOIPMonitor>>DevInfoAdd>>process>>", "消息体内容有误");
                        return;
                    }


 
  
                    DevID = dt.Rows[0]["DEVID"].ToString();
                    DevName = dt.Rows[0]["DEVNAME"].ToString();
                    try
                    {
                        DevTypeID =Convert.ToInt32(dt.Rows[0]["DEVTYPE"].ToString());
                    }
                    catch
                    {
                        DevTypeID = 0;
                    }
                    IP = dt.Rows[0]["IP"].ToString();
                    PORT = dt.Rows[0]["PORT"].ToString();
                    UPDEVID = dt.Rows[0]["UPDEVID"].ToString();
                    DESCR = dt.Rows[0]["DESCR"].ToString();


                    int Ret = -1;
                    int RetID = 0;

                    DevInfo_Add(DevID,
                                 DevName,
                                 DevTypeID,
                                 IP,
                                 PORT,
                                 UPDEVID,
                                 DESCR, ref  Ret, ref RetID);
                    switch (Ret)
                    {
                        case -1:
                            cmd2 = -8016;
                            break;
                        case 0:
                            cmd2 = ErrCommon.Success;
                            ByteResult = BitConverter.GetBytes(RetID);
                            CommonFunction.GetServerList();
                            break;
                    }


                    #endregion

                    Hashtable _hashtable_PackageArry = new Hashtable();
                    _hashtable_PackageArry.Add("1", request);//...連結位置    
                    _hashtable_PackageArry.Add("2", cmd1);
                    _hashtable_PackageArry.Add("3", cmd2);
                    _hashtable_PackageArry.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);
                    //CommonFunction.NoticeServerList();

                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>DevInfoAdd>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>DevInfoAdd>>process>>", ex.Message);

            }
            finally
            {
                //删除文件
                //if (!string.IsNullOrEmpty(request.receiveFileTemporarily))
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), request.receiveFileTemporarily);

                //GC.Collect();
            }


        }


        /// <summary>
        /// 设备信息添加
        /// </summary>
        /// <param name="DevID">设备编号</param>
        /// <param name="DevID">设备编号</param>
        /// <param name="DevName">设备名称</param>
        /// <param name="DevTypeID">设备类型</param>
        /// <param name="IP">IP地址</param>
        /// <param name="PORT">端口号</param>
        /// <param name="UPDEVID">上级服务编号</param>
        /// <param name="DESCR">描述</param>
        /// <param name="Ret">操作返回值</param>
        /// <param name="RetID">编号返回值</param>
        public static void DevInfo_Add(string DevID,
            string DevName,
            int DevTypeID,
            string IP,
            string PORT,
            string UPDEVID,
            string DESCR, ref int Ret,ref int RetID)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[9];
                parm[0] = MySqlCmdHeader.Parameter("p_devno", MySqlDbType.VarChar, DevID);
                parm[1] = MySqlCmdHeader.Parameter("p_name", MySqlDbType.VarChar, DevName);
                parm[2] = MySqlCmdHeader.Parameter("p_devid", MySqlDbType.Int32, DevTypeID);
                parm[3] = MySqlCmdHeader.Parameter("p_ip", MySqlDbType.VarChar, IP);

                parm[4] = MySqlCmdHeader.Parameter("p_port", MySqlDbType.VarChar, PORT);
                parm[5] = MySqlCmdHeader.Parameter("p_remark", MySqlDbType.VarChar, DESCR);
                parm[6] = MySqlHeader.MySqlCmdHeader.Parameter("p_updevid", MySqlDbType.VarChar, UPDEVID);              
                
                parm[7] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);
                parm[8] = MySqlHeader.MySqlCmdHeader.Parameter("RetID", MySqlDbType.Int32, ParameterDirection.Output);

                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_t_devinfo_add", parm);

                Ret = Convert.ToInt32(parm[7].Value);
                RetID = Convert.ToInt32(parm[8].Value);



            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>DevInfoAdd>>DevInfo_Add>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }
        }

       
    }
        
    
}
