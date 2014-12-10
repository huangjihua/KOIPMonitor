using System;
using System.Collections.Generic;
using System.Text;
using MySqlHeader;
using System.Data;
using MySql.Data.MySqlClient;

namespace KoIP.MySqlDAL
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class ising99_user_info
    {
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="loginname">登录名</param>
        /// <param name="passwrod">登录密码</param>
        /// <param name="kisaddress">用户所属地区</param>
        /// <param name="Return">返回值：0成功</param>
        /*
        1.		-10001	用户名错误	登入KIS
        2.		-10002	密码错误	登入KIS
        3.		-10003	登录验证koip服务器异常	登入KIS
        */

        public void UserLogin(string loginname, string passwrod,string kisaddress, ref short Return,ref string SessionString,ref string strUid)
        {
            Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance; 
            try
            {                               
                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlCmdHeader.Parameter("p_loginname", MySqlDbType.VarChar, loginname);
                MySqlDataReader Reads = MCMD.ExtcuteReader(ConnString.GetMySqlConnStr("db_koipConnectionString"), CommandType.StoredProcedure, "spu_userlogin", parm);
                if (Reads.HasRows)
                {
                    while (Reads.Read())
                    {
                        if (Reads["PASSWORD"].ToString().Trim() != passwrod)
                        {
                            Return = -10002;
                        }
                        else
                        {
                            SessionString = Reads["ID"].ToString().Trim() + "|" + loginname.Trim();
                            strUid = Reads["ID"].ToString().Trim();
                        }


                    }
                }
                else
                {
                    Return = -10001;
                }
            }
            catch (Exception ex)
            {
                Return = -10003;
            }
            finally
            {
                MCMD.Dispose();
            }            
        }
    }
}