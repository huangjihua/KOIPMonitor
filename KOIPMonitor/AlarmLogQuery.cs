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
    class AlarmLogQuery
    {
        public AlarmLogQuery() { }
        ~AlarmLogQuery() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            string STARTDATE = "";//设备信息编号
            string ENDDATE = "";//设备名称
            int PageCount = 0;//单页记录数
            int CurrPage = 0;//当前页
            int ID = 0;//自动增量编号

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.AlarmLogQuery;
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
                                                       "KOIPMonitor>>DevInfoQuery>>process>>", "消息体内容有误");
                        return;
                    }



                    if (dt.Columns["STARTDATE"] != null)
                    {
                        STARTDATE = dt.Rows[0]["STARTDATE"].ToString();
                    }
                    else
                    {
                        STARTDATE = null;
                    }


                    if (dt.Columns["ENDDATE"] != null)
                    {

                        ENDDATE = dt.Rows[0]["ENDDATE"].ToString();

                    }
                    else
                    {
                        ENDDATE = null;
                    }

                    try
                    {
                        ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    }
                    catch
                    {
                        ID = 0;
                    }

                    try
                    {
                        PageCount = Convert.ToInt32(dt.Rows[0]["PAGECOUNT"].ToString());
                    }
                    catch
                    {
                        PageCount = 0;
                    }
                    try
                    {
                        CurrPage = Convert.ToInt32(dt.Rows[0]["CURRPAGE"].ToString());
                    }
                    catch
                    {
                        CurrPage = 0;
                    }



                    #endregion
                    DataTable dtRet = null;
                    DataTable dtRetInfo = null;
                    AlarmLog_Query(STARTDATE, ENDDATE, PageCount, CurrPage, ref dtRet, ref dtRetInfo);

                    if (dtRet != null)
                    {
                        int TotalRecords = Convert.ToInt32(dtRetInfo.Rows[0][0].ToString());
                        int TotalPage = Convert.ToInt32(dtRetInfo.Rows[0][1].ToString()); ;//总记录数
                        _hashtable_Package.Add("1", request);
                        _hashtable_Package.Add("3", TotalPage);
                        _hashtable_Package.Add("4", CurrPage);
                        _hashtable_Package.Add("5", TotalRecords);
                        _hashtable_Package.Add("6", cmd1);
                        CommClass.DataTableToByteArry(dtRet, ref ByteResult);
                        _hashtable_Package.Add("2", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatasPage), _hashtable_Package);

                    }
                    else
                    {
                        cmd2 = -8026;
                        Hashtable _hashtable_PackageArry = new Hashtable();
                        _hashtable_PackageArry.Add("1", request);//...連結位置    
                        _hashtable_PackageArry.Add("2", cmd1);
                        _hashtable_PackageArry.Add("3", cmd2);
                        _hashtable_PackageArry.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);
                    }


                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogQuery>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogQuery>>process>>", ex.Message);

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
        /// 设备类型查询
        /// </summary>
        /// <param name="STARTDATE">开始时间</param>
        /// <param name="ENDDATE">结束时间</param>
        /// <param name="PageCount">单页记录数</param>
        /// <param name="CurrPage">当前页</param>
        /// <param name="dtRet">查询内容记录表</param>
        /// <param name="dtRetInfo">查询辅助信息记录表{总记录数，总页数}</param>
        public static void AlarmLog_Query(string STARTDATE, string ENDDATE,
                                        int PageCount, int CurrPage, ref DataTable dtRet, ref DataTable dtRetInfo)
        {

            try
            {

                string strsql = "";

                if ((STARTDATE != null) && (ENDDATE != null))
                {
                    strsql = "alarmdate >" + "\'"+STARTDATE.ToString()+"\'" + " and " + "alarmdate < " +"\'"+ ENDDATE.ToString()+"\'";
                }
                else
                {
                    if (STARTDATE != null)
                    {
                        strsql = "alarmdate >" + "\'" + STARTDATE.ToString() + "\'"; 
                    }
                    if (ENDDATE != null)
                    {
                        strsql = "alarmdate < " + "\'" + ENDDATE.ToString() + "\'";
                    }
                }

                DALPages.SQLPages sp = new DALPages.SQLPages();
                sp.FIELDS = " id as ID,alarmdate as ALARMDATE,devid as DEVID,descr as DESCR";
                sp.CURRENTPAGE = CurrPage;
                sp.ORDERTYPE = 0;
                sp.PAGESIZE = PageCount;
                sp.SQLWHERE = strsql;// "";
                sp.STRORDERBY = "id";// " ID DESC "; 
                sp.TABLENAME = "t_alarmlog";
                sp.ORDERBYNAME = "id";
                DataSet ds = sp.ds(CommClass.DBCONN, "sp_PageCommand");

                if (ds.Tables.Count <= 0)
                {
                    dtRet = null;
                    dtRetInfo = null;
                }
                else
                {
                    dtRet = ds.Tables[0].Copy();
                    dtRetInfo = ds.Tables[1].Copy();
                }


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>AlarmLogQuery>>AlarmLog_Query>>", ex.Message);

            }

        }

       
    }
        
    
}
