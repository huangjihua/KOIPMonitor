using System;
using System.Collections.Generic;
using System.Text;
using ServMonitor;
using System.Data;
using System.Collections;
using System.Threading;
namespace KOIPMonitor
{
    class ServMonitor_AlarmHandlingEvent
    {
        public static void AlarmEvent(ServInfoState sender)
        {
            try
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "sender", "\n" + sender.ToString());

                short cmd1 = 0;//主命令字
                short cmd2 = 0;//子命令字
                cmd1 = OMSCmd.RspGetServState;
                cmd2 = ErrCommon.Success;
                byte[] ByteResult = null;

                //List表内容
                List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                //Table属性内容
                Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();

                Ttable.FieldName = "ID";
                Ttable.FieldValue = sender.ID;
                ListTtable.Add(Ttable);

                Ttable.FieldName = "TYPE";
                Ttable.FieldValue = sender.TYPE;
                ListTtable.Add(Ttable);

                Ttable.FieldName = "STATE";
                Ttable.FieldValue = sender.STATE;
                ListTtable.Add(Ttable);
                Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);

                if (CommClass.ISCONUP == "0")
                {
                    UpdateServState(sender.ID, sender.TYPE, sender.STATE);
                    string strDescr = "";
                    switch (sender.STATE)
                    {
                        case "1":
                            strDescr = "设备启动";
                            break;
                        case "2":
                            strDescr = "设备故障";
                            break;
                        case "3":
                            strDescr = "状态未知";
                            break;
                        default:
                            strDescr = "状态未知";
                            break;
                    }
                    int Ret = -1;
                    if (sender.STATE != "1")
                    {
                        CommonFunction.Alarm_Add(sender.ID, sender.TYPE, strDescr, ref Ret);
                    }
                    CommClass.DevObj _DevObj = new CommClass.DevObj();
                    //_DevObj.DevID = sender.ID;
                    _DevObj.IsLocal = 0;
                    _DevObj.request=null;
                    CommClass.AddDevObjList(sender.ID,_DevObj);
                    foreach (KeyValuePair<string, Kernel.StateObject> a in CommClass.ClientConnList)
                    {
                        Hashtable _hashtable_Package = new Hashtable();
                        _hashtable_Package.Add("1", a.Value);//...連結位置 
                        _hashtable_Package.Add("2", cmd1);
                        _hashtable_Package.Add("3", cmd2);
                        _hashtable_Package.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package);
                    }
                }
                else
                {
                    cmd1 = OMSCmd.UpDevState;
                    cmd2 = ErrCommon.Success;
                    Hashtable _hashtable_Package = new Hashtable();
                    _hashtable_Package.Add("1", ConnectUpServ.tcpClient);//...連結位置 
                    _hashtable_Package.Add("2", cmd1);
                    _hashtable_Package.Add("3", cmd2);
                    _hashtable_Package.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.UnGzipTcpClientSend), _hashtable_Package);

                }                
                
            }
            catch (Exception ex)
            {
                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "BusinessDAL.KNS>>tcpClient_ExceptionHandlingEvent>>ReceiveEvent>>", ex.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 设备状态通知
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="TYPE"></param>
        /// <param name="STATE"></param>
        public static void NoticeDevState(string ID, string TYPE, string STATE)
        {
            try
            {
                short cmd1 = 0;//主命令字
                short cmd2 = 0;//子命令字
                cmd1 = OMSCmd.RspGetServState;
                cmd2 = ErrCommon.Success;
                byte[] ByteResult = null;

                //List表内容
                List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                //Table属性内容
                Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();

                Ttable.FieldName = "ID";
                Ttable.FieldValue = ID;
                ListTtable.Add(Ttable);

                Ttable.FieldName = "TYPE";
                Ttable.FieldValue = TYPE;
                ListTtable.Add(Ttable);

                Ttable.FieldName = "STATE";
                Ttable.FieldValue = STATE;
                ListTtable.Add(Ttable);
                Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);
                foreach (KeyValuePair<string, Kernel.StateObject> a in CommClass.ClientConnList)
                {
                    Hashtable _hashtable_Package = new Hashtable();
                    _hashtable_Package.Add("1", a.Value);//...連結位置 
                    _hashtable_Package.Add("2", cmd1);
                    _hashtable_Package.Add("3", cmd2);
                    _hashtable_Package.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package);
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>NoticeDevState>>", ex.Message);

            }

        }

        /// <summary>
        /// 设备状态通知
        /// </summary>
        public static void NoticeDevState()
        {
            try
            {
                short cmd1 = 0;//主命令字
                short cmd2 = 0;//子命令字
                cmd1 = OMSCmd.RspGetServState;
                cmd2 = ErrCommon.Success;


                for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                {
                    byte[] ByteResult = null;

                    //List表内容
                    List<Commonality.CommClass.TTable> ListTtable = new List<Commonality.CommClass.TTable>();
                    //Table属性内容
                    Commonality.CommClass.TTable Ttable = new Commonality.CommClass.TTable();
                    Ttable.FieldName = "ID";
                    Ttable.FieldValue = CommClass.DtServList.Rows[i]["ID"].ToString();
                    ListTtable.Add(Ttable);

                    Ttable.FieldName = "TYPE";
                    Ttable.FieldValue = CommClass.DtServList.Rows[i]["TYPE"].ToString();
                    ListTtable.Add(Ttable);

                    Ttable.FieldName = "STATE";
                    Ttable.FieldValue = CommClass.DtServList.Rows[i]["STATE"].ToString();
                    ListTtable.Add(Ttable);
                    Commonality.CommClass.TableToByteArry(ListTtable, ref ByteResult);
                    foreach (KeyValuePair<string, Kernel.StateObject> a in CommClass.ClientConnList)
                    {
                        Hashtable _hashtable_Package = new Hashtable();
                        _hashtable_Package.Add("1", a.Value);//...連結位置 
                        _hashtable_Package.Add("2", cmd1);
                        _hashtable_Package.Add("3", cmd2);
                        _hashtable_Package.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package);
                    }
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>NoticeDevState>>", ex.Message);

            }

        }


        /// <summary>
        /// 更新服务器状态
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="TYPE">服务器类型</param>
        /// <param name="STATE">服务器状态{1:正常;2:未启动;3:未知}</param>
        public static void UpdateServState(string ID, string TYPE, string STATE)
        {
            try
            {

                if (CommClass.DtServList == null)
                {
                    return;
                }
                for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                {
                    if ((CommClass.DtServList.Rows[i]["ID"].ToString() == ID) && (CommClass.DtServList.Rows[i]["TYPE"].ToString() == TYPE))
                    {
                        CommClass.DtServList.Rows[i]["STATE"] = STATE;
                    }
                }

                //for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                //{

                //    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "CommClass.DtServList", CommClass.DtServList.Rows[i]["STATE"].ToString());

                //}

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                                     "KoIp.BusinessDAL.KNS>>CommonFunction>>UpdateServState>>",
                                     ex.Message);
            }
            finally
            {
                ////GC.Collect();
            }
        }

        /// <summary>
        /// 更新服务器状态
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="STATE">服务器状态{1:正常;2:未启动;3:未知}</param>
        public static void UpdateServState(string ID,string STATE)
        {
            try
            {

                if (CommClass.DtServList == null)
                {
                    return;
                }
                for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                {
                    if ((CommClass.DtServList.Rows[i]["ID"].ToString() == ID))
                    {
                        CommClass.DtServList.Rows[i]["STATE"] = STATE;
                    }
                }

                //for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                //{

                //    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "CommClass.DtServList", CommClass.DtServList.Rows[i]["STATE"].ToString());

                //}

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                                     "KoIp.BusinessDAL.KNS>>CommonFunction>>UpdateServState>>",
                                     ex.Message);
            }
            finally
            {
                ////GC.Collect();
            }
        }
    }
}
