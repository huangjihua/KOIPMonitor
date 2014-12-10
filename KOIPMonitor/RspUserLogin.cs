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
    class RspUserLogin
    {
        public RspUserLogin() { }
        ~RspUserLogin() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
             Hashtable _hashtable_Package = new Hashtable();
             string UserId = "";//用户ID
             string UserPwd = "";//用户密码


            try
            {
                if (request != null)
                {

                        #region 包体解析
                        short cmd1 = 0;//主命令字
                        short cmd2 = 0;//子命令字
                        cmd1 = OMSCmd.RspUserLogin;
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
                                                           "KOIPMonitor>>RspUserLogin>>process>>", "消息体内容有误");
                            return;
                        }

                        UserId = dt.Rows[0]["USERID"].ToString();
                        UserPwd = dt.Rows[0]["USERPWD"].ToString();
                        //UserId = Guid.NewGuid().ToString();
                        int Ret = -1;
                        string Roles = "";
                        int RetID = 0; ;
                        RspUserLogin.LoginVerification(UserId, UserPwd,ref Ret,ref Roles,ref RetID);
                        switch (Ret)
                        {
                            case -1:
                                cmd2 = -8010;
                                break;
                            case -2:
                                cmd2 = -8011;
                                break;
                            case 0:
                                if (CommClass.GetUserIsLogin(UserId) == 0)
                                {
                                    cmd2 = ErrCommon.Success;
                                }
                                else
                                {
                                    cmd2 = -8032;
                                }
                                CommClass.AddClientConnList(UserId, request);
                                //List表内容
                                List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                                //Table属性内容
                                Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();
                                Ttable.FieldName = "ROLES";
                                Ttable.FieldValue = Roles;
                                ListTtable.Add(Ttable);
                                Ttable.FieldName = "ID";
                                Ttable.FieldValue = RetID.ToString();
                                ListTtable.Add(Ttable);
                                Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);
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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>RspUserLogin>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>RspUserLogin>>process>>", ex.Message);

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
        /// 用户登录验证
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <param name="UserPwd">用户密码</param>
        /// <param name="Ret">验证返回值</param>
        /// <param name="Roles">角色权限值</param>
        /// <param name="Roles">用户信息索引编号</param>
        /// <returns></returns>
        public static void LoginVerification(string Userid,string UserPwd,ref int Ret,ref string Roles,ref int RetID)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {

                MySqlParameter[] parm = new MySqlParameter[5];
                parm[0] = MySqlCmdHeader.Parameter("p_userid", MySqlDbType.VarChar, Userid);
                parm[1] = MySqlHeader.MySqlCmdHeader.Parameter("p_userpwd", MySqlDbType.VarChar, UserPwd);
                parm[2] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);
                parm[3] = MySqlHeader.MySqlCmdHeader.Parameter("p_Roles", MySqlDbType.VarChar, ParameterDirection.Output);
                parm[4] = MySqlHeader.MySqlCmdHeader.Parameter("RetID", MySqlDbType.Int32, ParameterDirection.Output);
                int i=MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_userlogin", parm);

                Ret = Convert.ToInt32(parm[2].Value);
                Roles = parm[3].Value.ToString();
                RetID = Convert.ToInt32(parm[4].Value);
                
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>LoginVerification>>process>>", ex.Message);
            }
            finally
            {
                MCMD.Dispose();
            }
        }

       
    }
        
    
}
