using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlHeader;
using MySql.Data.MySqlClient;

namespace KoIP.MySqlDAL
{
    public class T_FRIENDSTYPE
    {
        /// <summary>
        /// 501
        /// </summary>
        /// <param name="MACHINENO"></param>
        /// <returns></returns>
        public DataTable SPU_GET_T_FRIENDSTYPE(string USERID)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {
                MySqlParameter[] parm = new MySqlParameter[1];
                parm[0] = MySqlCmdHeader.Parameter("P_USERID", MySqlDbType.VarChar, USERID);
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_GET_T_FRIENDSTYPE", parm, "ITEM");
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
        public DataTable SPU_RAND_TWO(string strUidList, string SEX)
        {
            //short Return = 0;
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            DataTable dt = new DataTable();
            try
            {
                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlCmdHeader.Parameter("P_STRUIDLIST", MySqlDbType.VarChar, strUidList);
                parm[1] = MySqlHeader.MySqlCmdHeader.Parameter("P_SEX", MySqlDbType.Int32, Convert.ToInt32(SEX));
                dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_RAND_TWO", parm, "ITEM");
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