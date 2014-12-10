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
    public class T_USERINFO
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

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="USERID"></param>
        /// <param name="PASSWORD"></param>
        /// <param name="SessionString"></param>
        /// <returns></returns>
        public short UserLogin(string USERID, string PASSWORD, ref string SessionString)
        {
            short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance; 
            try
            {                               
                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlCmdHeader.Parameter("p_loginname", MySqlDbType.VarChar, USERID);
                //MySqlDataReader Reads = MCMD.ExtcuteReader(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_userlogin", parm);
                DataTable dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_userlogin", parm,"item");
                if (dt.Rows.Count != 1)
                {
                    Return = -2205;//没有此用户
                }
                else
                {
                    if (dt.Rows[0]["PASSWORD"].ToString().Trim() != PASSWORD)
                    {
                        Return = -2204;//密码错误
                    }
                    else
                    {
                        SessionString = dt.Rows[0]["USERID"].ToString().Trim() + "|" + dt.Rows[0]["ID"].ToString().Trim();
                    }
 
                }
                dt.Clear();
                //if (Reads.HasRows)
                //{
                //    while (Reads.Read())
                //    {
                //        if (Reads["PASSWORD"].ToString().Trim() != PASSWORD)
                //        {
                //            Return = -15014;//密码错误
                //        }
                //        else
                //        {
                //            SessionString = Reads["USERID"].ToString().Trim() + "|" + Reads["ID"].ToString().Trim();
                //            //strUid = Reads["ID"].ToString().Trim();
                //            //Return = 0;
                //        }
                //    }
                //}
                //else
                //{
                //    Return = -15015;//没有此用户
                //}
            }
            catch (Exception ex)
            {
                Return = -500;
            }
            finally
            {
                MCMD.Dispose();
            }
            return Return;
        }
      
        /// <summary>
        /// 快速注册
        /// </summary>
        /// <param name="MACHINENO"></param>
        /// <returns></returns>
        public DataTable T_USERINFO_AddNew(string MACHINENO,string straddress)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {

                //Console.WriteLine("IP="+straddress.Trim());
                //straddress = "222.231.05.46";
                string pname = straddress.Trim();// getprovince(straddress.Trim()).Trim();
                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlCmdHeader.Parameter("P_MACHINENO", MySqlDbType.VarChar, MACHINENO);
                parm[1] = MySqlCmdHeader.Parameter("P_ADDRESS", MySqlDbType.VarChar, pname.Trim());
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_T_USERINFO_ADD_Q", parm, "db");                
            }
            catch (Exception ex)
            {
                //Return = -500;
                dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return dt;
        }
        public string getprovince(string straddress)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            string pname = "";
            try
            {
                if (string.IsNullOrEmpty(straddress))
                {
                    pname = "未知";
                }
                else
                {
                    string[] Str = straddress.Split('.');
                    Int64 ipnum1 = Convert.ToInt64(Str[0].Trim()) * 256 * 256 * 256;
                    Int64 ipnum2 = Convert.ToInt64(Str[0].Trim()) * 256 * 256;
                    Int64 ipnum3 = Convert.ToInt64(Str[0].Trim()) * 256;
                    Int64 ipnum4 = Convert.ToInt64(Str[0].Trim());
                    Int64 toipnum = ipnum1 + ipnum2 + ipnum3 + ipnum4;

                    /*
                     
                     if exists(select StateName  from T_IP where P_TOTAL>=FromNum and P_TOTAL<=ToNum) then
select StateName into P_StateName from T_IP where P_TOTAL>=FromNum and P_TOTAL<=ToNum limit 1;
else
set P_StateName='未知';
end if;
 return P_StateName;
                     */
                    string strsql = "select StateName  from T_IP where " + toipnum + ">=FromNum and " + toipnum + "<=ToNum limit 1";
                    dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.Text, strsql, null, "db");
                    if (dt == null)
                    {
                        pname = "未知";
                    }
                    if (dt.Rows.Count != 1)
                    {
                        pname = "未知";
                    }
                    else
                    {
                        pname = dt.Rows[0][0].ToString().Trim();
                    }
                    //return toipnum.ToString();
                }
            }
            catch (Exception ex)
            {
                pname = "未知";
            }
            finally
            {
                dt.Clear();
                MCMD.Dispose();

            }
            return pname;

        }

        public DataTable T_USERINFO_SEARCH(string strwhere)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {

                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlCmdHeader.Parameter("P_StrWhere", MySqlDbType.VarChar, strwhere);
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_SEARCHUSER", parm, "ITEM");
            }
            catch (Exception ex)
            {
                //Return = -500;
                dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return dt;
        }
        public DataTable spu_singUser(string uid)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {

                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlCmdHeader.Parameter("P_UID", MySqlDbType.VarChar, uid);
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_singUser", parm, "ITEM");
            }
            catch (Exception ex)
            {
                //Return = -500;
                dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return dt;
        }

        public short updateuser(string strSql)
        {
            short Return = -1;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            //DataTable dt = new DataTable();
            try
            {
                //MySqlParameter[] parm = new MySqlParameter[1];
                //parm[0] = MySqlCmdHeader.Parameter("P_UID", MySqlDbType.VarChar, uid);
                //dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_singUser", parm, "ITEM");
                Return = (short)MCMD.ExecuteNonQuery(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.Text, strSql, null);
            }
            catch (Exception ex)
            {
                Return = -500;
                //dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return Return;
        }

        public static int getuid(string uid)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            int returni = 0;
            try
            {                
                //MySqlParameter[] parm = new MySqlParameter[1];
                //parm[0] = MySqlCmdHeader.Parameter("P_UID", MySqlDbType.VarChar, uid);
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.Text, "SELECT ID FROM T_USERINFO WHERE USERID='" + uid + "'", null, "ITEM");
                if (dt.Rows.Count == 1)
                {
                    returni = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());
                }
                else
                {
 
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                dt.Clear();
                MCMD.Dispose();
            }
            return returni;
        }

    }
}