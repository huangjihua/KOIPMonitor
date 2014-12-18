using Kernel;
using ServMonitor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace KOIPMonitor
{
    partial class ServiceKOIPM : ServiceBase
    {
        public ServiceKOIPM(string[] args)
        {
            InitializeComponent();


            try
            {

                CommClass.OStype = Convert.ToInt32(args[0].ToString());
            }
            catch
            {
                CommClass.OStype = 0;
            }
            CommClass.OStype = 1;


            CommClass.SetPubPath();
            Commonality.ConsoleManage.objectTime();

            CommClass.CreateConfig();

            //CommClass.CreateServListTable();
            //CommClass.CreateServList();

            //CommClass.ReadXML(CommClass.ServListPath, ref CommClass.DtServList);

            DataTable dt = new DataTable();
            CommClass.ReadXML(CommClass.ConfigFilePath, ref dt);

            if (dt.Rows.Count != 0)
            {


                #region 读取KMS配置相关信息
                //读取数据库连接
                CommClass.DBCONN = dt.Rows[0]["DBCONN"].ToString();
                //本地服务器ID
                CommClass.ID = dt.Rows[0]["ID"].ToString();
                //本地服务IP地址
                CommClass.IP = dt.Rows[0]["IP"].ToString();
                //是否启用自动启动
                CommClass.ISAUTOSTART = dt.Rows[0]["ISAUTOSTART"].ToString();

                //本地服务端口号

                try
                {
                    CommClass.PORT = Convert.ToInt32(dt.Rows[0]["PORT"].ToString());
                }
                catch
                {
                    CommClass.PORT = 0;
                }


                //上级服务ID
                CommClass.UPID = dt.Rows[0]["UPID"].ToString();

                //上级服务IP地址
                CommClass.UPIP = dt.Rows[0]["UPIP"].ToString();

                //上级服务端口号
                try
                {
                    CommClass.UPPORT = Convert.ToInt32(dt.Rows[0]["UPPORT"].ToString());
                }
                catch
                {
                    CommClass.UPPORT = 0;
                }

                //数据库连接字符串
                CommClass.DBCONN = dt.Rows[0]["DBCONN"].ToString();

                //是否连接上级服务{0:不连接;1:连接}
                CommClass.ISCONUP = dt.Rows[0]["ISCONUP"].ToString();

                #endregion

                if (CommClass.ISCONUP == "0")
                {
                    CommonFunction.GetServerList();
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectUpServ.HeartSocketobjectTime), null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectUpServ.Start), null);//连接上级服务器
                }

                AsynTCPSocket listener = new AsynTCPSocket(CommClass.OStype.ToString());
                listener.listenerAddress = CommClass.IP;
                listener.listenerPort = CommClass.PORT;
                listener.ReceiveEvent += new AsynchronousSocketListenerReceiveEvent(tcpServer_ReceiveEvent.ReceiveEvent);
                listener.ExceptionHandlingEvent += new AsynchronousServerExceptionHandlingEvent(tcpServer_ExceptionHandlingEvent.ReceiveEvent);
                listener.Start();
                while (!listener.IsBound)
                {
                    Thread.Sleep(500);
                }

                if (listener.IsBound)
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor：SOCKET=" + CommClass.IP + ":" + CommClass.PORT.ToString() + "  TCP服務啟動成功", "");
                }
                else
                {
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor：SOCKET=" + CommClass.IP + ":" + CommClass.PORT.ToString() + "   TCP服務啟動失敗", "");

                }

                MonitorInterface.AlarmEvent += new ServMonitorAlarmHandlingEvent(ServMonitor_AlarmHandlingEvent.AlarmEvent);
                MonitorInterface.OptEvent += new ServMonitorOptHandlingEvent(ServMonitor_OptHandlingEvent.OptEvent);
                MonitorInterface MonInterface = new MonitorInterface();
                MonInterface.Start(CommClass.OStype.ToString(), CommClass.ISAUTOSTART);



            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            ServiceKOIPM sk = new ServiceKOIPM(args);
            
            
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
