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
     class RobotByServMonitor
    {
        private System.Timers.Timer _timer_job1;
        //public static event ServMonitorAlarmHandlingEvent AlarmEvent;
        public int Interval;
        /// <summary>
        /// 服务器名称
        /// </summary>
        private string _ServName = "";

        /// <summary>
        /// 服务器执行路径
        /// </summary>
        private string _ServPath = "";

        /// <summary>
        /// 服务器ID编号
        /// </summary>
        private string _ID = "";

        /// <summary>
        /// 服务器类型
        /// </summary>
        private string _TYPE = "";

        /// <summary>
        /// 程序类型
        /// 0:控制台程序
        /// 1:服务程序
        /// </summary>
        private string _APPTYPE="";


        /// <summary>
        /// 程序类型
        /// 0:控制台程序
        /// 1:服务程序
        /// </summary>
        public string APPTYPE
        {
            set { _APPTYPE = value; }
            get { return _APPTYPE; }
        }

        /// <summary>
        /// 服务器类型{1：KMS 2：KIS 3：KNS 4：KDS}
        /// </summary>
        public string TYPE
        {
            set { _TYPE = value; }
            get { return _TYPE; }
        }

        /// <summary>
        /// 服务器ID编号
        /// </summary>
        public string ID
        {
            set { _ID = value; }
            get { return _ID; }
        }


        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServName
        {
            set { _ServName = value; }
            get { return _ServName; }
        }

        /// <summary>
        /// 服务器路径
        /// </summary>
        public string ServPath
        {
            set { _ServPath = value; }
            get { return _ServPath; }
        }

        public void Start()
        {
            _timer_job1 = new System.Timers.Timer();
            _timer_job1.Interval = this.Interval;
            _timer_job1.AutoReset = true;
            _timer_job1.Elapsed += new ElapsedEventHandler(do_Job1);
            _timer_job1.Start();
        }
        public void Stop()
        {
            _timer_job1.Stop();
            _timer_job1 = null;
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

        private bool IsMsgShow = true;
        private void do_Job1(object source, ElapsedEventArgs e)
        {
            if (ExistServ(_ServName))
            {
                ServInfoState SerInfoState = new ServInfoState();//服务器状态信息
                if (APPTYPE == "1")
                {

                    ServiceController myController = new System.ServiceProcess.ServiceController(_ServName);
                    myController.Start();
                    //myController.Pause();
                    //myController.Continue();
                    //myController.Stop();
                    myController.Refresh();
                    IsMsgShow = true;
                    ConsoleManage.Write(ErrorLevel.Serious,
    "RobotByServMonitor>>do_Job1>>" + _ServName,
    _ServName + " start Sourcess!");
                    SerInfoState.ID = _ID;
                    SerInfoState.TYPE = _TYPE;
                    SerInfoState.STATE = "1";
                    SerInfoState.NAME = ServName + "[" + SerInfoState.ID + "]";
                    AlarmEvent(SerInfoState);

                    Common.RemoveServStartList(_ServName);

                }
                else if (APPTYPE == "0")
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = _ServPath;    // 要启动的程序
                    ConsoleManage.Write(ErrorLevel.Serious,
    "RobotByServMonitor>>do_Job1>>" + _ServPath,
    " Test Show!");
                    if (CommClass.OStype != 0)
                    {
                        info.Arguments = CommClass.OStype.ToString();//传递给程序的参数
                    }
                    else
                    {
                        //info.UserName = "root";
                        //info.Arguments = "&";//传递给程序的参数
                    }
                    info.WindowStyle = ProcessWindowStyle.Hidden;   //隐藏窗口
                    //info.UseShellExecute = false;


                    try
                    {
                        Process pro = Process.Start(info); //启动程序
                        IsMsgShow = true;
                        ConsoleManage.Write(ErrorLevel.Serious,
    "RobotByServMonitor>>do_Job1>>" + _ServName,
    _ServName + " start Sourcess!");
                        /*ServInfoState*/
                        SerInfoState.ID = _ID;
                        SerInfoState.TYPE = _TYPE;
                        SerInfoState.STATE = "1";
                        SerInfoState.NAME = ServName + "[" + SerInfoState.ID + "]";
                        AlarmEvent(SerInfoState);
                        Common.RemoveServStartList(_ServName);
                    }
                    catch (Exception ex)
                    {
                        if (IsMsgShow)
                        {
                            ConsoleManage.Write(ErrorLevel.Serious,
      "RobotByServMonitor>>do_Job1>>" + _ServName,
      _ServName + "  start faile!" + ex.ToString());
                            //Console.WriteLine(ex.ToString());
                        }
                        IsMsgShow = false;

                        //Stop();
                        return;
                    }
                }

            }
        }

        /// <summary>
        /// 监测服务进程是否存在
        /// </summary>
        /// <param name="ServName">服务名称</param>
        /// <returns>false存在;true不存在</returns>
        private static bool ExistServ(string ServName)
        {
            bool ret = false;

            foreach (KeyValuePair<string, string> a in Common.NoStartServList)
            {
                if (a.Key == ServName.ToString())
                {
                    //a.Value.States.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    ret = true;
                    break;
                }
            }
            
            return ret;


            //foreach (Process thisproc in System.Diagnostics.Process.GetProcessesByName(ServName))
            //{
            //    if (thisproc.ProcessName == ServName)
            //    {
            //        ret = true;
            //        break;

            //    }

            //}

            //Process[] myProcess;
            //myProcess = System.Diagnostics.Process.GetProcessesByName(ServName);
            //if (myProcess.Length>0)
            //{
            //     ret = true;
            //}




            //foreach (Process thisproc in System.Diagnostics.Process.GetProcesses())
            //{
            //    if (thisproc.StartInfo.FileName == ServName)
            //    {
            //        ret = true;
            //        break;

            //    }

            //}
        }
    }
}
