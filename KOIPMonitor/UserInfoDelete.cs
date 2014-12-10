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
    class UserInfoDelete
    {
        public UserInfoDelete() { }
        ~UserInfoDelete() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            int ID = 0;             //自动增量编号
            string USERID = "";     //用户ID

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.UserInfoDelete;
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
                                                       "KOIPMonitor>>UserInfoDelete>>process>>", "消息体内容有误");
                        return;
                    }

                    try
                    {
                        ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    }
                    catch
                    {
                        ID = 0;
                    }
                    USERID = dt.Rows[0]["USERID"].ToString();



                    int Ret = -1;

                    UserInfo_Delete(USERID, ref Ret);
                    switch (Ret)
                    {
                        case -1:
                            cmd2 = -8021;
                            break;
                        case 0:
                            cmd2 = ErrCommon.Success;
                            ByteResult = BitConverter.GetBytes(ID);
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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoDelete>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoDelete>>process>>", ex.Message);

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
        /// 用户删除
        /// </summary>
        /// <param name="USERID">用户ID</param>
        /// <param name="Ret">操作返回值</param>
        public static void UserInfo_Delete(string USERID, ref int Ret)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlHeader.MySqlCmdHeader.Parameter("p_userid", MySqlDbType.VarChar, USERID);
                parm[1] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);

                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_t_userinfo_delete", parm);

                Ret = Convert.ToInt32(parm[1].Value);


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoDelete>>UserInfo_Delete>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }

        }

       
    }
        
    
}
