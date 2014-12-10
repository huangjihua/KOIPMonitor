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
    class UserInfoModify
    {
        public UserInfoModify() { }
        ~UserInfoModify() { }
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
            string USERNAME = "";   //用户名称
            string EMAIL = "";      //邮箱地址
            int ISEMAIL = 0;        //是否接收邮件
            string MOBILE = "";     //手机
            int ISMOBILE = 0;       //是否接收短信
            string ROLES = "";      //权限值
            string USERPWD = "";    //用户密码


            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.UserInfoModify;
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
                                                       "KOIPMonitor>>UserInfoModify>>process>>", "消息体内容有误");
                        return;
                    }



                    try
                    {
                        ISEMAIL = Convert.ToInt32(dt.Rows[0]["ISEMAIL"].ToString());
                    }
                    catch
                    {
                        ISEMAIL = 0;
                    }
                    try
                    {
                        ISMOBILE = Convert.ToInt32(dt.Rows[0]["ISMOBILE"].ToString());
                    }
                    catch
                    {
                        ISMOBILE = 0;
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
                    USERNAME = dt.Rows[0]["USERNAME"].ToString();

                    EMAIL = dt.Rows[0]["EMAIL"].ToString();
                    MOBILE = dt.Rows[0]["MOBILE"].ToString();
                    ROLES = dt.Rows[0]["ROLES"].ToString();
                    USERPWD = dt.Rows[0]["USERPWD"].ToString();

                    int Ret = -1;
   
                    UserInfo_Modify(
                                 USERID,
                                 USERNAME,
                                 EMAIL,
                                 ISEMAIL,
                                 MOBILE,
                                 ISMOBILE,
                                 ROLES,USERPWD,ID, ref  Ret);
                    switch (Ret)
                    {
                        case -1:
                            cmd2 = -8022;
                            break;
                        case -2:
                            cmd2 = -8028;
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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoModify>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoModify>>process>>", ex.Message);

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
        /// 用户修改
        /// </summary>
        /// <param name="USERID">用户ID</param>
        /// <param name="USERNAME">用户名称</param>
        /// <param name="EMAIL">邮箱地址</param>
        /// <param name="ISEMAIL">是否接收邮件</param>
        /// <param name="MOBILE">手机</param>
        /// <param name="ISMOBILE">是否接收短信</param>
        /// <param name="ROLES">权限值</param>
        /// <param name="USERPWD">用户密码</param>
        /// <param name="ID">记录自增变量</param>
        /// <param name="Ret">操作返回值</param>
        public static void UserInfo_Modify(
            string USERID,
            string USERNAME,
            string EMAIL,
            int ISEMAIL,
            string MOBILE,
            int ISMOBILE,
            string ROLES,string USERPWD,int ID, ref int Ret)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[10];
                parm[0] = MySqlCmdHeader.Parameter("p_name", MySqlDbType.VarChar, USERNAME);
                parm[1] = MySqlCmdHeader.Parameter("p_userid", MySqlDbType.VarChar, USERID);
                parm[2] = MySqlCmdHeader.Parameter("p_isemail", MySqlDbType.Int32, ISEMAIL);
                parm[3] = MySqlCmdHeader.Parameter("p_mobile", MySqlDbType.VarChar, MOBILE);
                parm[4] = MySqlHeader.MySqlCmdHeader.Parameter("p_ismobile", MySqlDbType.Int32, ISMOBILE);
                parm[5] = MySqlHeader.MySqlCmdHeader.Parameter("p_uroles", MySqlDbType.VarChar, ROLES);
                parm[6] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);
                parm[7] = MySqlCmdHeader.Parameter("p_email", MySqlDbType.VarChar, EMAIL);
                parm[8] = MySqlCmdHeader.Parameter("p_password", MySqlDbType.VarChar, USERPWD);
                parm[9] = MySqlCmdHeader.Parameter("p_id", MySqlDbType.Int32, ID);
                
                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_t_userinfo_modify", parm);

                Ret = Convert.ToInt32(parm[6].Value);   


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoModify>>UserInfo_Modify>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }
        }
       
    }
        
    
}
