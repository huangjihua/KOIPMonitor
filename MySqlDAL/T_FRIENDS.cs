using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlHeader;
using MySql.Data.MySqlClient;

namespace KoIP.MySqlDAL
{
    public class T_FRIENDS
    {
        /// <summary>
        /// 502
        /// </summary>
        /// <param name="USERID"></param>
        /// <param name="TYPEID"></param>
        /// <returns></returns>
        public DataTable SPU_GET_MY_T_FRIENDS(string USERID,string TYPEID)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {
                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlCmdHeader.Parameter("P_USERID", MySqlDbType.VarChar, USERID);
                parm[1] = MySqlCmdHeader.Parameter("P_TYPEID", MySqlDbType.Int32, Convert.ToInt32(TYPEID));
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_SELECT_T_FRIENDSBYTYPEID", parm, "ITEM");
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return dt;
        }

        public DataTable GetMyOnlineFriends(string USERID,string onlinelist)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {
                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlCmdHeader.Parameter("P_USERID", MySqlDbType.VarChar, USERID);
                parm[1] = MySqlCmdHeader.Parameter("P_ONLINELIST", MySqlDbType.VarChar, onlinelist);
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_GetMyOnlineFriends", parm, "ITEM");
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                MCMD.Dispose();
            }
            return dt;
        }
    }
}
