using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using System.Threading;

namespace KOIPMonitor
{
    /// <summary>
    /// 连接上级服务
    /// </summary>
    class ConnectUpServ
    {
        public static AsynTCPClient tcpClient;
        static System.Timers.Timer HeartSocketTimer = null;


        /// <summary>
        /// 初始化Timer对象(心跳包检测)
        /// </summary>
        public static void HeartSocketobjectTime(object _obj)
        {
            //StrServerMode = ServerMode;
            HeartSocketTimer = new System.Timers.Timer();
            HeartSocketTimer.Elapsed += new System.Timers.ElapsedEventHandler(HeartSocket);
            HeartSocketTimer.Interval = 30000;//相隔多长时间跑一次3600000
            HeartSocketTimer.AutoReset = true;
            HeartSocketTimer.Enabled = true;
        }


        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void HeartSocket(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ConnectUpServ>>HeartSocket>>", "HeartSocket");

                if (tcpClient.Connected)
                {
                    CommonFunction.SendHandshake(tcpClient);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Start), null);//连接上级服务器
                }

            }
            catch
            {
            }
            finally
            {
            }
        }
        /// <summary>
        /// 连接上级服务启动
        /// </summary>
        /// <param name="obj">参数，用做线程处理</param>
        public static void Start(object obj)
        {
            try
            {
                //HeartSocketobjectTime();
                tcpClient = new AsynTCPClient(CommClass.OStype.ToString());
                tcpClient.targetAddress = CommClass.UPIP.ToString();// SocketInfo.KMSIP;
                tcpClient.targetPort = CommClass.UPPORT;// SocketInfo.KMSPORT;
                tcpClient.BeginConnect(tcpClient.targetAddress, tcpClient.targetPort);
                tcpClient.ReceiveEvent += new AsynTCPClientReceiveEvent(tcpClient_ReceiveEvent.ReceiveEvent);
                tcpClient.ExceptionHandlingEvent += new AsynClientExceptionHandlingEvent(tcpClient_ExceptionHandlingEvent.ReceiveEvent);

                if (tcpClient.Connected)
                {

                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ConnectUpServ>>Start>>", "Connect UpServer is success!\n UpServIP=" + CommClass.UPIP.ToString() + " UpServPort=" + CommClass.UPPORT.ToString());
                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ConnectUpServ>>Start>>", "Connect UpServer is fail!");

                }
                

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>ConnectUpServ>>Start>>", ex.Message);
            }
        }
    }
}
