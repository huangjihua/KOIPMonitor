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
    class AlarmLogClear
    {
        public AlarmLogClear() { }
        ~AlarmLogClear() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.AlarmLogClear;
                    cmd2 = ErrCommon.Success;
                    byte[] ByteResult = null;
                    //DataTable dt = new DataTable();
                    //Commonality.CommClass.ReadXML(request.receiveFileTemporarily, ref dt);

                    //if (dt.Rows.Count <= 0)
                    //{
                    //    cmd2 = -101;//解包失败
                    //    //哈希表存放包体内容
                    //    Hashtable _hashtable_Package_Temp = new Hashtable();
                    //    _hashtable_Package_Temp.Add("1", request);//...連結位置            
                    //    _hashtable_Package_Temp.Add("2", cmd1);
                    //    _hashtable_Package_Temp.Add("3", cmd2);
                    //    ByteResult = null;
                    //    _hashtable_Package_Temp.Add("4", ByteResult);
                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);
                    //    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                    //                                   "KOIPMonitor>>AlarmLogClear>>process>>", "消息体内容有误");
                    //    return;
                    //}


                    int Ret = -1;

                    AlarmLog_Clear(ref Ret);
                    switch (Ret)
                    {
                        case -1:
                            cmd2 = -8026;
                            break;
                        case 0:
                            cmd2 = ErrCommon.Success;
                            break;
                    }


                    #endregion

                    Hashtable _hashtable_PackageArry = new Hashtable();
                    _hashtable_PackageArry.Add("1", request);//...連結位置    
                    _hashtable_PackageArry.Add("2", cmd1);
                    _hashtable_PackageArry.Add("3", cmd2);
                    _hashtable_PackageArry.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);


                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogClear>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogClear>>process>>", ex.Message);

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
        /// 日志清除
        /// </summary>
        /// <param name="Ret">操作返回值</param>
        /// <returns></returns>
        public static void AlarmLog_Clear(ref int Ret)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);

                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_alarmlog_clear", parm);

                Ret = Convert.ToInt32(parm[0].Value);


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogClear>>AlarmLog_Clear>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }

        }

       
    }
        
    
}
