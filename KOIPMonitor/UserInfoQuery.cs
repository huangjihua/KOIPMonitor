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
    class UserInfoQuery
    {
        public UserInfoQuery() { }
        ~UserInfoQuery() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            int ID = 0;//记录自动增量ID编号
            string USERID = "";//用户ID
            int PageCount = 0;//单页记录数
            int CurrPage = 0;//当前页

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.UserInfoQuery;
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
                                                       "KOIPMonitor>>UserInfoQuery>>process>>", "消息体内容有误");
                        return;
                    }

                    if (dt.Columns["ID"] != null)
                    {
                        try
                        {
                            ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                        }
                        catch
                        {
                            ID = 0;
                        }
                    }
                    else
                    {
                        ID = 0;
                    }

 

                    if (dt.Columns["USERID"] != null)
                    {

                        USERID = dt.Rows[0]["USERID"].ToString();

                    }
                    else
                    {
                        USERID = null;
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
                    UserInfo_Query(ID,USERID, PageCount, CurrPage, ref dtRet, ref dtRetInfo);

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
                        //string strSend = System.Text.Encoding.UTF8.GetString(ByteResult);
                        //_hashtable_Package.Add("2", strSend);
                        _hashtable_Package.Add("2", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatasPage), _hashtable_Package);

                    }
                    else
                    {
                        cmd2 = -8023;
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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoQuery>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoQuery>>process>>", ex.Message);

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
        /// 用户查询
        /// </summary>
        /// <param name="ID">自动增量ID编号</param>
        /// <param name="USERID">用户编号</param>
        /// <param name="PageCount">单页记录数</param>
        /// <param name="CurrPage">当前页</param>
        /// <param name="dtRet">查询内容记录表</param>
        /// <param name="dtRetInfo">查询辅助信息记录表{总记录数，总页数}</param>
        public static void UserInfo_Query(int ID,string USERID,
                                        int PageCount, int CurrPage, ref DataTable dtRet, ref DataTable dtRetInfo)
        {

            try
            {

                string strsql = "";


                if (USERID != null)
                {
                    strsql = " userid like" + "\'%" + USERID + "%\' ";
                }


                if (ID > 0)
                {
                    strsql = " id=" + ID.ToString();
                }
                DALPages.SQLPages sp = new DALPages.SQLPages();

                sp.FIELDS = "id as ID,userid as USERID, name as USERNAME,email as EMAIL,isemail as ISEMAIL,mobile as MOBILE, ismobile as ISMOBILE,uroles as ROLES,password  as USERPWD";
                sp.CURRENTPAGE = CurrPage;
                sp.ORDERTYPE = 0;
                sp.PAGESIZE = PageCount;
                sp.SQLWHERE = strsql;// "";
                sp.STRORDERBY = "id";// " ID DESC "; 
                sp.TABLENAME = "t_userinfo";
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
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>UserInfoQuery>>UserInfo_Query>>", ex.Message);

            }

        }

       
    }
        
    
}
