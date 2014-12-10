using System;
using System.Collections.Generic;
using System.Text;
using MySqlHeader;
using MySql.Data.MySqlClient;
using System.Data;

namespace KoIP.MySqlDAL
{
    public class T_STOPUSER
    {
        public static void stopuser(string strmyid, string strSTOPID, ref int Return)
        {
            //string strmyid = Readdt.Rows[0]["MYID"].ToString().Trim();
            //string strSTOPID = Readdt.Rows[0]["STOPID"].ToString().Trim();

            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {
                MySqlParameter[] parm = new MySqlParameter[2];
                parm[0] = MySqlCmdHeader.Parameter("P_MYUID", MySqlDbType.VarChar, strmyid);
                parm[1] = MySqlCmdHeader.Parameter("P_STOPID", MySqlDbType.VarChar, strSTOPID);
                //MySqlDataReader Reads = MCMD.ExtcuteReader(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "spu_userlogin", parm);
                //DataTable dt = MCMD.ExtcuteDataTable(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_T_STOPUSER_ADD", parm, "item");
                int c = Convert.ToInt32(MCMD.ExecuteNonQuery(KoIP.MySqlDAL.getdbconn.dbconn, CommandType.StoredProcedure, "SPU_T_STOPUSER_ADD", parm));
                Return = c;
               
            }
            catch (Exception ex)
            {
                Return = -500;
            }
            finally
            {
                MCMD.Dispose();
            }
        }
    }
}
