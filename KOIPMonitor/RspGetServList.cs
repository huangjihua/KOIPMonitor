using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using Kernel;
using System.IO;
using System.Data;

namespace KOIPMonitor
{
    class RspGetServList
    {
        public RspGetServList() { }
        ~RspGetServList() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
             Hashtable _hashtable_Package = new Hashtable();

            try
            {
                if (request != null)
                {

                        #region 包体解析
                        short cmd1 = 0;//主命令字
                        short cmd2 = 0;//子命令字
                        cmd1 = OMSCmd.RspGetServList;
                        cmd2 = ErrCommon.Success;
                        byte[] ByteResult = null;
                        //DataTable dt = new DataTable();
                        //Commonality.CommClass.ReadXML(request.receiveFileTemporarily, ref dt);

                        //if (dt.Rows.Count <= 0)
                        //{
                        //    cmd2 = -101;//解包失败
                        //    //哈希表存放包体内容
                        //    Hashtable _hashtable_Package_Temp = new Hashtable();
                        //    _hashtable_Package_Temp.Add("1", request);//...連結位置            
                        //    _hashtable_Package_Temp.Add("2", cmd1);
                        //    _hashtable_Package_Temp.Add("3", cmd2);
                        //    ByteResult = null;
                        //    _hashtable_Package_Temp.Add("4", ByteResult);
                        //    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);
                        //    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                        //                                   "BusinessDAL.KNS.server>>SuperaddSingSongList>>process>>", "消息体内容有误");
                        //    return;
                        //}

                        //sessionid = dt.Rows[0]["SESSIONID"].ToString();
                        //RoomId = dt.Rows[0]["ROOMID"].ToString();
                        //UserID = sessionid;

                        #endregion

                        ////List表内容
                        //List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                        ////Table属性内容
                        //Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();

                        //Ttable.FieldName = "MYLANAUDIO";
                        //Ttable.FieldValue = "";
                        //ListTtable.Add(Ttable);

                        //Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);
                        DataTable dt = new DataTable();
                        dt = CommClass.DtServList.Copy();
                        Commonality.CommClass.OutputBody(dt);
                        CommClass.DataTableToByteArry(dt,ref ByteResult);

                        Hashtable _hashtable_PackageArry = new Hashtable();


                        _hashtable_PackageArry.Add("1", request);//...連結位置    
                        _hashtable_PackageArry.Add("2", cmd1);
                        _hashtable_PackageArry.Add("3", cmd2);
                        _hashtable_PackageArry.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);


               }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServList>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServList>>process>>", ex.Message);

            }
            finally
            {
                //删除文件
                //if (!string.IsNullOrEmpty(request.receiveFileTemporarily))
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), request.receiveFileTemporarily);

                //GC.Collect();
            }

            
        }

       
    }
        
    
}
