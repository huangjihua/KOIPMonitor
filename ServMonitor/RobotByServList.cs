using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.ServiceProcess;

namespace ServMonitor
{
    /// <summary>
    /// 智能监听服务
    /// </summary>
     class RobotByServList
    {
        private  static System.Timers.Timer _timer_job1;
        public  static int Interval;
        //public event ServMonitorAlarmHandlingEvent AlarmEvent;

        public  static void Start()
        {
            _timer_job1 = new System.Timers.Timer();
            _timer_job1.Interval = Interval;
            _timer_job1.AutoReset = true;
            _timer_job1.Elapsed += new ElapsedEventHandler(do_Job1);
            _timer_job1.Start();
            //MonitorInterface.AlarmEvent = AlarmEvent;
        }
        public static void Stop()
        {
            _timer_job1.Stop();
            _timer_job1 = null;
        }

        private static void do_Job1(object source, ElapsedEventArgs e)
        {
            try
            {
                AddServStartList();

            }
            catch (Exception ex)
            {

                    ConsoleManage.Write(ErrorLevel.Serious,
"RobotByServList>>do_Job1>>", ex.Message);

                return;
            }

        }

        public static void AlarmEvent(ServInfoState sender)
        {
            try
            {
                //Console.WriteLine(sender.ToString());
                MonitorInterface.MonitorAlarmEvent(sender);
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
        /// 初始化服务器状态信息
        /// </summary>
        private static bool IsInit = true;//
        //private  event ServMonitorAlarmHandlingEvent AlarmEvent;
        /// <summary>
        /// 添加启动服务
        /// </summary>
        /// <param name="ServName">服务名称</param>
        /// <param name="ServPath">服务路径</param>
        public static void AddServStartList()
        {
            string ServName="";
            string ServPath = "";
            string _ID = "";
            string _TYPE = "";
            string _APPTYPE = "";
            try
            {
                if ((Common.DtServInfo.Columns["ID"] == null)||
                    (Common.DtServInfo.Columns["TYPE"] == null)||
                    (Common.DtServInfo.Columns["ServName"] == null)||
                    (Common.DtServInfo.Columns["ServPath"] == null)||
                    (Common.DtServInfo.Columns["APPTYPE"] == null))
                {
                    return;
                }
                for (int i = 0; i < Common.DtServInfo.Rows.Count; i++)
                {
                    ServName = Common.DtServInfo.Rows[i]["ServName"].ToString();
                    ServPath = Common.DtServInfo.Rows[i]["ServPath"].ToString();
                    _APPTYPE = Common.DtServInfo.Rows[i]["APPTYPE"].ToString();
                    if (Common.DtServInfo.Columns["ID"] != null)
                    {
                        _ID = Common.DtServInfo.Rows[i]["ID"].ToString();
                    }
                    if (Common.DtServInfo.Columns["TYPE"] != null)
                    {
                        _TYPE = Common.DtServInfo.Rows[i]["TYPE"].ToString();
                    }


                    if (_APPTYPE == "1")
                    {
                        ServiceController myController = new System.ServiceProcess.ServiceController(ServName);
                        if (myController.Status == ServiceControllerStatus.Stopped)
                        {
                            if (!Common.NoStartServList.ContainsKey(ServName))
                            {

                                Common.NoStartServList.Add(ServName, ServPath);
                                ServInfoState SerInfoState = new ServInfoState();//服务器状态信息
                                SerInfoState.ID = _ID;
                                SerInfoState.TYPE = _TYPE;
                                SerInfoState.STATE = "2";
                                SerInfoState.NAME = ServName + "[" + _ID + "]";
                                AlarmEvent(SerInfoState);
                            }
                            else
                            {
                                Common.RemoveServStartList(ServName);
                                //if (IsInit)
                                //{
                                ServInfoState SerInfoState = new ServInfoState();//服务器状态信息
                                SerInfoState.ID = _ID;
                                SerInfoState.TYPE = _TYPE;
                                SerInfoState.STATE = "1";
                                SerInfoState.NAME = ServName + "[" + SerInfoState.ID + "]";
                                AlarmEvent(SerInfoState);
                            }
                        }
                     }
                    else if (_APPTYPE == "0")
                    {
                        Process[] myProcess;
                        myProcess = System.Diagnostics.Process.GetProcessesByName(ServName);
                        if (myProcess.Length == 0)
                        {
                            if (!Common.NoStartServList.ContainsKey(ServName))
                            {

                                Common.NoStartServList.Add(ServName, ServPath);
                                ServInfoState SerInfoState = new ServInfoState();//服务器状态信息
                                SerInfoState.ID = _ID;
                                SerInfoState.TYPE = _TYPE;
                                SerInfoState.STATE = "2";
                                SerInfoState.NAME = ServName + "[" + _ID + "]";
                                AlarmEvent(SerInfoState);
                            }
                        }
                        else
                        {

                            Common.RemoveServStartList(ServName);
                            //if (IsInit)
                            //{
                            ServInfoState SerInfoState = new ServInfoState();//服务器状态信息
                            SerInfoState.ID = _ID;
                            SerInfoState.TYPE = _TYPE;
                            SerInfoState.STATE = "1";
                            SerInfoState.NAME = ServName + "[" + SerInfoState.ID + "]";
                            AlarmEvent(SerInfoState);
                            //}

                        }
                    }
                }
                IsInit = false;
            }
            catch(Exception ex)
            {
                ConsoleManage.Write(ErrorLevel.Serious, "ServMonitor>>RobotByServList>>AddServStartList>>", ex.Message);
                return;
            }


            
        }

        /// <summary>
        /// 添加启动服务
        /// </summary>
        /// <param name="ServName">服务名称</param>
        /// <param name="ServPath">服务路径</param>
        public static void AddServStartList(string ServName)
        {
            string ServPath = "";
            try
            {
                if ((Common.DtServInfo.Columns["ID"] == null) ||
                    (Common.DtServInfo.Columns["TYPE"] == null) ||
                    (Common.DtServInfo.Columns["ServName"] == null) ||
                    (Common.DtServInfo.Columns["ServPath"] == null))
                {
                    return;
                }
                for (int i = 0; i < Common.DtServInfo.Rows.Count; i++)
                {
                    ServPath = Common.DtServInfo.Rows[i]["ServPath"].ToString();
                    if (!Common.NoStartServList.ContainsKey(ServName))
                    {
                        Common.NoStartServList.Add(ServName, ServPath);
                        break;
                    }
                }
            }
            catch
            {
                return;
            }
        }

    }
}
