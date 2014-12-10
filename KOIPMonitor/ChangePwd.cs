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
    class ChangePwd
    {
        public ChangePwd() { }
        ~ChangePwd() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

//<?xml version="1.0" encoding="utf-8"?>
//<ROOT>
//    <ITEM>
//<USERID>1</ USERID >
//<USERPWD></ USERPWD >
//<USERNEWPWD></ USERNEWPWD >
//    </ITEM>
//</ROOT>


            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            string USERPWD = "";    //设备名称
            string USERNEWPWD = "";         //IP地址
            int intID = 0;//记录索引编号


            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.ChangePwd;
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
                                                       "KOIPMonitor>>ChangePwd>>process>>", "消息体内容有误");
                        return;
                    }


                    USERPWD = dt.Rows[0]["USERPWD"].ToString();
                    USERNEWPWD = dt.Rows[0]["USERNEWPWD"].ToString();
                    try
                    {
                        intID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    }
                    catch
                    {
                        intID = 0;
                    }

                    int Ret = -1;

                    Change_Pwd(intID,USERPWD,USERNEWPWD, ref  Ret);
                    switch (Ret)
                    {
                        case -1:
                            cmd2 = -8024;
                            break;
                        case 0:
                            cmd2 = ErrCommon.Success;
                            break;
                        default:
                            cmd2 = -8024;
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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ChangePwd>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ChangePwd>>process>>", ex.Message);

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
        /// 用户密码修改
        /// </summary>
        /// 
        /// <param name="USERID">用户ID</param>
        /// <param name="USERPWD">用户密码</param>
        /// <param name="USERNEWPWD">用户密码</param>
        /// <param name="Ret">操作返回值</param>
        public static void Change_Pwd(int intID,string USERPWD ,
                                      string USERNEWPWD,ref int Ret)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[4];
                parm[0] = MySqlCmdHeader.Parameter("p_id", MySqlDbType.Int32, intID);
                parm[1] = MySqlCmdHeader.Parameter("p_oldpwd", MySqlDbType.VarChar, USERPWD);
                parm[2] = MySqlCmdHeader.Parameter("p_newpwd", MySqlDbType.VarChar, USERNEWPWD);
                parm[3] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);

                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_changepwd", parm);

                Ret = Convert.ToInt32(parm[3].Value);


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ChangePwd>>Change_Pwd>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }
        }


       
    }
        
    
}
