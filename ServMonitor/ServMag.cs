using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace ServMonitor
{

    /// <summary>
    /// 智能监听服务
    /// </summary>
    class ServMag
    {
        private System.Timers.Timer _timer_job1;
        //public static event ServMonitorAlarmHandlingEvent AlarmEvent;
        public static void OptEvent(ServOptState sender)
        {
            try
            {
                //Console.WriteLine(sender.ToString());
                MonitorInterface.MonitorOptEvent(sender);
            }
            catch (Exception ex)
            {
                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "BusinessDAL.KNS>>tcpClient_ExceptionHandlingEvent>>ReceiveEvent>>", ex.Message);
            }
            finally
            {

            }
        }


        #region 不带连接对像操作
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        /// <param name="ServName">服务名称</param>
        public static void  ServStop(string ID,string TYPE,string ServName)
        {
            try
            {
                Process[] myProcess;
                myProcess = System.Diagnostics.Process.GetProcessesByName(ServName);
                Console.WriteLine(ServName + "    " + "ServStop myProcess.Length=" + myProcess.Length.ToString());
                if (myProcess.Length > 0)
                {
                    Console.WriteLine(myProcess[0].ProcessName.ToString());
                    //myProcess[0].Close();
                    myProcess[0].Kill();
                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
                    SerInfoState.ID = ID;
                    SerInfoState.TYPE = TYPE;
                    SerInfoState.OPTSTATE = "0";
                    SerInfoState.NAME = ServName;
                    OptEvent(SerInfoState);

                    ServInfoState SerAlareState = new ServInfoState();//服务器状态信息
                    SerAlareState.ID = ID;
                    SerAlareState.TYPE = TYPE;
                    SerAlareState.STATE = "2";
                    SerAlareState.NAME = ServName + "[" + ID + "]";
                    RobotByServList.AlarmEvent(SerAlareState);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadAddServList), ServName);

                }
                else
                {
                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
                    SerInfoState.ID = ID;
                    SerInfoState.TYPE = TYPE;
                    SerInfoState.OPTSTATE = "-2";
                    SerInfoState.NAME = ServName;
                    OptEvent(SerInfoState);


                    ServInfoState SerAlareState = new ServInfoState();//服务器状态信息
                    SerAlareState.ID = ID;
                    SerAlareState.TYPE = TYPE;
                    SerAlareState.STATE = "2";
                    SerAlareState.NAME = ServName + "[" + ID + "]";
                    RobotByServList.AlarmEvent(SerAlareState);
                }

            }
            catch
            {
                return;
            }
        }



        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        /// <param name="ServName">服务名称</param>
        /// <param name="ServPath">服务执行路径</param>
        public static void ServRestart(string ID, string TYPE, string ServName,string ServPath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = ServPath;    // 要启动的程序

            if (CommClass.OStype != 0)
            {
                info.Arguments = CommClass.OStype.ToString();//传递给程序的参数
            }
            else
            {
                //info.UserName = "root";
            }
            info.WindowStyle = ProcessWindowStyle.Hidden;   //隐藏窗口

            //info.UseShellExecute = false;
            if (ExistServ(ServName))
            {

                try
                {
                    Process pro = Process.Start(info); //启动程序
                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
                    SerInfoState.ID = ID;
                    SerInfoState.TYPE = TYPE;
                    SerInfoState.OPTSTATE = "0";
                    SerInfoState.NAME = ServName;
                    OptEvent(SerInfoState);

                    Common.RemoveServStartList(ServName);
                }
                catch (Exception ex)
                {

                    ConsoleManage.Write(ErrorLevel.Serious,
"RobotByServMonitor>>do_Job1>>" + ServName,
ServName + "  start faile!" + ex.Message);
                    //Console.WriteLine(ex.ToString());
                    return;
                }

            }
            else
            {
                ServStop(ID, TYPE, ServName);
                Process pro = Process.Start(info); //启动程序
                ServOptState SerInfoState = new ServOptState();//服务器状态信息
                SerInfoState.ID = ID;
                SerInfoState.TYPE = TYPE;
                SerInfoState.OPTSTATE = "0";
                SerInfoState.NAME = ServName;
                OptEvent(SerInfoState);
                Common.RemoveServStartList(ServName);

            }

            ServInfoState SerAlareState = new ServInfoState();//服务器状态信息
            SerAlareState.ID = ID;
            SerAlareState.TYPE = TYPE;
            SerAlareState.STATE = "1";
            SerAlareState.NAME = ServName + "[" + ID + "]";
            RobotByServList.AlarmEvent(SerAlareState);
        }



        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        /// <param name="ServName">服务名称</param>
        /// <param name="ServPath">服务执行路径</param>
        public static void ServStart(string ID, string TYPE, string ServName, string ServPath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = ServPath;    // 要启动的程序
            if (CommClass.OStype != 0)
            {
                info.Arguments = CommClass.OStype.ToString();//传递给程序的参数
            }
            else
            {
                //info.UserName = "root";
            }
            info.WindowStyle = ProcessWindowStyle.Hidden;   //隐藏窗口
            //info.UseShellExecute = false;
            if (ExistServ(ServName))
            {

                try
                {
                    Process pro = Process.Start(info); //启动程序
                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
                    SerInfoState.ID = ID;
                    SerInfoState.TYPE = TYPE;
                    SerInfoState.OPTSTATE = "0";
                    SerInfoState.NAME = ServName;
                    OptEvent(SerInfoState);

                    Common.RemoveServStartList(ServName);
                }
                catch (Exception ex)
                {

                    ConsoleManage.Write(ErrorLevel.Serious,
"RobotByServMonitor>>do_Job1>>" + ServName,
ServName + "  start faile!" + ex.Message);
                    //Console.WriteLine(ex.ToString());
                    return;
                }

            }
            else
            {
                ServOptState SerInfoState = new ServOptState();//服务器状态信息
                SerInfoState.ID = ID;
                SerInfoState.TYPE = TYPE;
                SerInfoState.OPTSTATE = "-1";
                SerInfoState.NAME = ServName;
                OptEvent(SerInfoState);

                Common.RemoveServStartList(ServName);

            }

            ServInfoState SerAlareState = new ServInfoState();//服务器状态信息
            SerAlareState.ID = ID;
            SerAlareState.TYPE = TYPE;
            SerAlareState.STATE = "1";
            SerAlareState.NAME = ServName + "[" + ID + "]";
            RobotByServList.AlarmEvent(SerAlareState);
        }
        #endregion

//        #region 带连接对像操作
//        /// <summary>
//        /// 停止服务
//        /// </summary>
//        /// <param name="ID">服务器ID编号</param>
//        /// <param name="TYPE">服务器类型</param>
//        /// <param name="ServName">服务名称</param>
//        public static void ServStop(string ID, string TYPE, string ServName,StateObject request)
//        {
//            try
//            {
//                Process[] myProcess;
//                myProcess = System.Diagnostics.Process.GetProcessesByName(ServName);
//                if (myProcess.Length > 0)
//                {
//                    //myProcess[0].Close();
//                    myProcess[0].Kill();

//                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadAddServList), ServName);

//                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
//                    SerInfoState.ID = ID;
//                    SerInfoState.TYPE = TYPE;
//                    SerInfoState.OPTSTATE = "0";
//                    SerInfoState.NAME = ServName;
//                    SerInfoState.Request = request;
//                    OptEvent(SerInfoState);

//                }

//            }
//            catch
//            {
//                return;
//            }
//        }



//        /// <summary>
//        /// 重启服务
//        /// </summary>
//        /// <param name="ID">服务器ID编号</param>
//        /// <param name="TYPE">服务器类型</param>
//        /// <param name="ServName">服务名称</param>
//        /// <param name="ServPath">服务执行路径</param>
//        public static void ServRestart(string ID, string TYPE, string ServName, string ServPath, StateObject request)
//        {
//            ProcessStartInfo info = new ProcessStartInfo();
//            info.FileName = ServPath;    // 要启动的程序

//            if (CommClass.OStype != 0)
//            {
//                info.Arguments = CommClass.OStype.ToString();//传递给程序的参数
//            }
//            else
//            {
//                //info.UserName = "root";
//            }
//            info.WindowStyle = ProcessWindowStyle.Hidden;   //隐藏窗口

//            //info.UseShellExecute = false;
//            if (ExistServ(ServName))
//            {

//                try
//                {
//                    Process pro = Process.Start(info); //启动程序
//                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
//                    SerInfoState.ID = ID;
//                    SerInfoState.TYPE = TYPE;
//                    SerInfoState.OPTSTATE = "0";
//                    SerInfoState.NAME = ServName;
//                    SerInfoState.Request = request;
//                    OptEvent(SerInfoState);

//                    Common.RemoveServStartList(ServName);
//                }
//                catch (Exception ex)
//                {

//                    ConsoleManage.Write(ErrorLevel.Serious,
//"RobotByServMonitor>>do_Job1>>" + ServName,
//ServName + "  start faile!" + ex.Message);
//                    //Console.WriteLine(ex.ToString());
//                    return;
//                }

//            }
//            else
//            {
//                ServStop(ID, TYPE, ServName);
//                Process pro = Process.Start(info); //启动程序
//                ServOptState SerInfoState = new ServOptState();//服务器状态信息
//                SerInfoState.ID = ID;
//                SerInfoState.TYPE = TYPE;
//                SerInfoState.OPTSTATE = "0";
//                SerInfoState.NAME = ServName;
//                SerInfoState.Request = request;
//                OptEvent(SerInfoState);
//                Common.RemoveServStartList(ServName);

//            }
//        }



//        /// <summary>
//        /// 开启服务
//        /// </summary>
//        /// <param name="ID">服务器ID编号</param>
//        /// <param name="TYPE">服务器类型</param>
//        /// <param name="ServName">服务名称</param>
//        /// <param name="ServPath">服务执行路径</param>
//        public static void ServStart(string ID, string TYPE, string ServName, string ServPath, StateObject request)
//        {
//            ProcessStartInfo info = new ProcessStartInfo();
//            info.FileName = ServPath;    // 要启动的程序
//            if (CommClass.OStype != 0)
//            {
//                info.Arguments = CommClass.OStype.ToString();//传递给程序的参数
//            }
//            else
//            {
//                //info.UserName = "root";
//            }
//            info.WindowStyle = ProcessWindowStyle.Hidden;   //隐藏窗口
//            //info.UseShellExecute = false;
//            if (ExistServ(ServName))
//            {

//                try
//                {
//                    Process pro = Process.Start(info); //启动程序
//                    ServOptState SerInfoState = new ServOptState();//服务器状态信息
//                    SerInfoState.ID = ID;
//                    SerInfoState.TYPE = TYPE;
//                    SerInfoState.OPTSTATE = "0";
//                    SerInfoState.NAME = ServName;
//                    SerInfoState.Request = request;
//                    OptEvent(SerInfoState);

//                    Common.RemoveServStartList(ServName);
//                }
//                catch (Exception ex)
//                {

//                    ConsoleManage.Write(ErrorLevel.Serious,
//"RobotByServMonitor>>do_Job1>>" + ServName,
//ServName + "  start faile!" + ex.Message);
//                    //Console.WriteLine(ex.ToString());
//                    return;
//                }

//            }
//            else
//            {
//                ServOptState SerInfoState = new ServOptState();//服务器状态信息
//                SerInfoState.ID = ID;
//                SerInfoState.TYPE = TYPE;
//                SerInfoState.OPTSTATE = "-1";
//                SerInfoState.NAME = ServName;
//                SerInfoState.Request = request;
//                OptEvent(SerInfoState);

//                Common.RemoveServStartList(ServName);

//            }
//        }

//        #endregion

        #region 监测服务进程是否存在

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
        }

        #endregion

        #region 进程操作添加服务列表
        /// <summary>
        /// 进程操作添加服务列表
        /// </summary>
        /// <param name="obj"></param>
        private static void ThreadAddServList(object obj)
        {
            try
            {
                string ServName = (string)obj;
                //Common.RemoveServStartList(ServName);
                RobotByServList.AddServStartList(ServName);
            }
            catch
            {
                return;
            }
        }
        #endregion
    }
}
