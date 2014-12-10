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
    class ReqServOpt
    {
        public ReqServOpt() { }
        ~ReqServOpt() { }
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
            string _OPTTYPE = "";

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.RspServOpt;
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
                                                       "BusinessDAL.KNS.server>>SuperaddSingSongList>>process>>", "消息体内容有误");
                        return;
                    }

                    _ID = dt.Rows[0]["ID"].ToString();
                    _TYPE = dt.Rows[0]["TYPE"].ToString();
                    _OPTTYPE = dt.Rows[0]["OPTTYPE"].ToString();

                    ByteResult = File.ReadAllBytes(request.receiveFileTemporarily);
                    CommClass.DevObj _DevObj = new CommClass.DevObj();

                    CommClass.GetDevMagObj(_ID, ref _DevObj);

                    if (_DevObj.IsLocal == 0)
                    {
                        switch (_OPTTYPE)
                        {
                            case "1":
                                ServMonitor.MonitorInterface.ServStop(_ID, _TYPE);
                                break;
                            case "2":
                                ServMonitor.MonitorInterface.ServStart(_ID, _TYPE);
                                break;
                            case "3":
                                ServMonitor.MonitorInterface.ServRestart(_ID, _TYPE);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if ((_DevObj.request == null) || (_DevObj.IsLocal==-1))
                        {
                            cmd2 = -8033;
                            cmd1 = OMSCmd.RspServOpt;
                            //哈希表存放包体内容
                            Hashtable _hashtable_Package_Temp = new Hashtable();
                            _hashtable_Package_Temp.Add("1", request);//...連結位置            
                            _hashtable_Package_Temp.Add("2", cmd1);
                            _hashtable_Package_Temp.Add("3", cmd2);
                            _hashtable_Package_Temp.Add("4", ByteResult);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);
                            return;


                        }
                        else
                        {
                            cmd1 = OMSCmd.ReqServOpt;
                            //哈希表存放包体内容
                            Hashtable _hashtable_Package_Temp = new Hashtable();
                            _hashtable_Package_Temp.Add("1", _DevObj.request);//...連結位置            
                            _hashtable_Package_Temp.Add("2", cmd1);
                            _hashtable_Package_Temp.Add("3", cmd2);
                            _hashtable_Package_Temp.Add("4", ByteResult);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);

                        }
                    }

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if ((dt.Rows[i]["ID"] == _ID) && (dt.Rows[i]["TYPE"] == _TYPE))
                    //    {
                    //        _STATE = dt.Rows[i]["STATE"].ToString();
                    //        break;
                    //    }
                    //}


                    #endregion

                    //short cmd1 = 0;//主命令字
                    //short cmd2 = 0;//子命令字
                    //cmd1 = OMSCmd.RspServOpt;
                    //cmd2 = ErrCommon.Success;
                    //byte[] ByteResult = null;
                    //Hashtable _hashtable_PackageArry = new Hashtable();
                    //_hashtable_PackageArry.Add("1", request);//...連結位置    
                    //_hashtable_PackageArry.Add("2", cmd1);
                    //_hashtable_PackageArry.Add("3", cmd2);
                    //_hashtable_PackageArry.Add("4", ByteResult);
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);



                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", ex.Message);

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
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(TCPClientStateObject request)
        {

            //哈希表存放包体内容
            Hashtable _hashtable_Package = new Hashtable();
            string _ID = "";
            string _TYPE = "";
            string _OPTTYPE = "";

            try
            {
                if (request != null)
                {

                    #region 包体解析
                    short cmd1 = 0;//主命令字
                    short cmd2 = 0;//子命令字
                    cmd1 = OMSCmd.RspServOpt;
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
                                                       "BusinessDAL.KNS.server>>SuperaddSingSongList>>process>>", "消息体内容有误");
                        return;
                    }

                    _ID = dt.Rows[0]["ID"].ToString();
                    _TYPE = dt.Rows[0]["TYPE"].ToString();
                    _OPTTYPE = dt.Rows[0]["OPTTYPE"].ToString();

                    CommClass.DevObj _DevObj = new CommClass.DevObj();
                    CommClass.GetDevMagObj(_ID, ref _DevObj);

                    if (_DevObj.IsLocal == 0)
                    {
                        switch (_OPTTYPE)
                        {
                            case "1":
                                ServMonitor.MonitorInterface.ServStop(_ID, _TYPE);
                                break;
                            case "2":
                                ServMonitor.MonitorInterface.ServStart(_ID, _TYPE);
                                break;
                            case "3":
                                ServMonitor.MonitorInterface.ServRestart(_ID, _TYPE);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if ((_DevObj.request == null) || (_DevObj.IsLocal == -1))
                        {
                            cmd2 = -8033;
                            cmd1 = OMSCmd.RspServOpt;
                            //哈希表存放包体内容
                            Hashtable _hashtable_Package_Temp = new Hashtable();
                            _hashtable_Package_Temp.Add("1", request);//...連結位置            
                            _hashtable_Package_Temp.Add("2", cmd1);
                            _hashtable_Package_Temp.Add("3", cmd2);
                            _hashtable_Package_Temp.Add("4", ByteResult);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);
                            return;


                        }
                        else
                        {
                            cmd1 = OMSCmd.ReqServOpt;
                            //哈希表存放包体内容
                            Hashtable _hashtable_Package_Temp = new Hashtable();
                            _hashtable_Package_Temp.Add("1", _DevObj.request);//...連結位置            
                            _hashtable_Package_Temp.Add("2", cmd1);
                            _hashtable_Package_Temp.Add("3", cmd2);
                            _hashtable_Package_Temp.Add("4", ByteResult);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package_Temp);

                        }
                    }

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if ((dt.Rows[i]["ID"] == _ID) && (dt.Rows[i]["TYPE"] == _TYPE))
                    //    {
                    //        _STATE = dt.Rows[i]["STATE"].ToString();
                    //        break;
                    //    }
                    //}


                    #endregion

                    //short cmd1 = 0;//主命令字
                    //short cmd2 = 0;//子命令字
                    //cmd1 = OMSCmd.RspServOpt;
                    //cmd2 = ErrCommon.Success;
                    //byte[] ByteResult = null;
                    //Hashtable _hashtable_PackageArry = new Hashtable();
                    //_hashtable_PackageArry.Add("1", request);//...連結位置    
                    //_hashtable_PackageArry.Add("2", cmd1);
                    //_hashtable_PackageArry.Add("3", cmd2);
                    //_hashtable_PackageArry.Add("4", ByteResult);
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_PackageArry);



                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", ex.Message);

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
