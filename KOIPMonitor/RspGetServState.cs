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
    class RspGetServState
    {
        public RspGetServState() { }
        ~RspGetServState() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {

            //哈希表存放包体内容
             Hashtable _hashtable_Package = new Hashtable();
             string _ID = "";
             string _TYPE = "";
             string _STATE = "";

            try
            {
                if (request != null)
                {

                        #region 包体解析
                        short cmd1 = 0;//主命令字
                        short cmd2 = 0;//子命令字
                        cmd1 = OMSCmd.RspGetServState;
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
                                                           "KOIPMonitor>>RspGetServState>>process>>", "消息体内容有误");
                            return;
                        }

                        _ID = dt.Rows[0]["ID"].ToString();
                        _TYPE = dt.Rows[0]["TYPE"].ToString();


                        for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                        {
                            if ((CommClass.DtServList.Rows[i]["ID"].ToString() == _ID) && (CommClass.DtServList.Rows[i]["TYPE"].ToString() == _TYPE))
                            {
                                _STATE = CommClass.DtServList.Rows[i]["STATE"].ToString();
                                break;
                            }
                        }

                        //List表内容
                        List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                        //Table属性内容
                        Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();

                        Ttable.FieldName = "ID";
                        Ttable.FieldValue = _ID;
                        ListTtable.Add(Ttable);

                        Ttable.FieldName = "TYPE";
                        Ttable.FieldValue = _TYPE;
                        ListTtable.Add(Ttable);

                        Ttable.FieldName = "STATE";
                        Ttable.FieldValue = _STATE;
                        ListTtable.Add(Ttable);

                        Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);

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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>RspGetServState>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>RspGetServState>>process>>", ex.Message);

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
