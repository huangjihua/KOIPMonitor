using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;

namespace Kernel
{
    public class AsynTCPClient
    {
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private string _targetAddress;
        private int _targetPort;
        private int _pendingConnectionsQueue = 1000;
        private Socket tcpClient;
        private IPEndPoint remoteEP;
        private bool _connected;

        /// <summary>
        /// 接收到封包後續處理
        /// </summary>
        public event AsynTCPClientReceiveEvent ReceiveEvent;
        public event AsynClientExceptionHandlingEvent ExceptionHandlingEvent;



        /// <summary>
        /// 操作系统类型值
        /// </summary>
        private int OSType = 0;
        /// <summary>
        /// 传入操作系统类型值
        /// </summary>
        /// <param name="OsType"></param>
        public AsynTCPClient(string OsType)
        {
            try
            {
                OSType = Convert.ToInt32(OsType);
            }
            catch
            {
                OSType = 0;
            }
        }
        public AsynTCPClient()
        {

        }
        ~AsynTCPClient()
        {
            connectDone = null;
            sendDone = null;
            receiveDone = null;
            tcpClient = null;
            remoteEP = null;
            ReceiveEvent = null;
            ExceptionHandlingEvent = null;
        }

        //...判斷Socket 是否連接
        public bool Connected
        {
            set
            {
                this._connected = value;
            }
            get
            {
                return this._connected;
            }
        }

        /// <summary>
        /// 輸入目標位置
        /// </summary>        
        public string targetAddress
        {
            set
            {
                this._targetAddress = value;
            }
            get
            {
                return this._targetAddress;
            }
        }

        /// <summary>
        /// 設定傾聽端口
        /// </summary>
        public int targetPort
        {
            set
            {
                this._targetPort = value;
            }
            get
            {
                return this._targetPort;
            }
        }

        /// <summary>
        /// 設定TCP 等待連接佇列大小
        /// </summary>
        public int pendingConnectionsQueue
        {
            set
            {
                this._pendingConnectionsQueue = value;
            }
            get
            {
                return this._pendingConnectionsQueue;
            }
        }

        public void BeginConnect(string Address, int Port)
        {
            try
            {
                _targetAddress = Address;
                _targetPort = Port;
                remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(_targetAddress), _targetPort);
                tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Thread listen = new Thread(new ThreadStart(Run));
                listen.Start();

                //...等待連線成功
                while (!this._connected)
                {
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                //connectDone.Set();
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>Start", ex.Message);
                GC.Collect();
                //throw new ArgumentNullException();
            }
        }

        private void Run()
        {
            //connectDone.Reset();
            tcpClient.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), null);
            //connectDone.WaitOne();
        }

        public void Stop()
        {
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>Stop", ex.Message);
            }
            finally
            {
                tcpClient = null;
                GC.Collect();
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                KConsole.Write(ErrorLevel.Debug, "Kernel>>AsynTCPClient>>Run>>", "連接>>" + tcpClient.RemoteEndPoint.ToString());
                //connectDone.Set();
                this._connected = true;
            }
            catch (Exception ex)
            {
                //connectDone.Set();
                this._connected = false;
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>Run>>", ex.Message);
            }
        }

        public void BeginSend(byte[] buffer)
        {
            //sendDone.Reset();
            //tcpClient.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), null);
            //sendDone.WaitOne();

            try
            {
                if (!tcpClient.Connected) return;
                if (tcpClient == null) return;
                this.tcpClient.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), null);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Response, "", "KoIPRouter>>AsynTCPClient>>Send>>Exception:" + ex.Message);
                // KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>Send", ex.Message);
            }
        }


        private void SendCallback(IAsyncResult ar)
        {
            //try
            //{
            //    int bytesSent = tcpClient.EndSend(ar);
            //    sendDone.Set();
            //}
            //catch (Exception ex)
            //{
            //    sendDone.Set();
            //    this._connected = false;
            //    KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>SendCallback>>", ex.Message);
            //}

            int bytesSent = 0;
            try
            {
                if (!tcpClient.Connected) return;
                if (tcpClient == null) return;
                bytesSent = tcpClient.EndSend(ar);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Response, "", "KoIPRouter>>StateObject>>SendCallback>>Exception:" + ex.Message);
                //KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>SendCallback", ex.Message);
            }
        }

        public void BeginReceive()
        {
            try
            {
                //receiveDone.Reset();
                TCPClientStateObject state = new TCPClientStateObject();
                state.headerBufferSize = 9;
                state.receiveBuffer = new byte[state.headerBufferSize];
                state.workSocket = tcpClient;
                tcpClient.BeginReceive(state.receiveBuffer, 0, state.headerBufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                //receiveDone.WaitOne();
            }
            catch (Exception ex)
            {
                this._connected = false;
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>BeginReceive>>", ex.Message);
            }

        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            TCPClientStateObject state = null;
            bool closeSocket = false;

            try
            {
                state = (TCPClientStateObject)ar.AsyncState;
                Socket userClient = state.workSocket;
                int bytesRead = 0;

                //...判斷socket連線是否斷掉
                if (userClient == null || !userClient.Connected)
                {
                    closeSocket = true;
                    return;
                }

                bytesRead = userClient.EndReceive(ar);



                if (bytesRead > 0)
                {
                    #region//...是否讀過 Header
                    if (!state.isReadHeader)
                    {
                        //...建立接收封包的存放位置
                        string directoryName = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/Process/";

                        if (OSType != 0)
                        {
                            directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Process\\";
                        }
                        DirectoryInfo Createdir = new DirectoryInfo(directoryName);
                        if (!Createdir.Exists)
                        {
                            Createdir.Create();
                        }
                        state.receiveFileTemporarily = directoryName + RandomSession.createSession(64);

                        using (MemoryStream ms_readheader = new MemoryStream(state.receiveBuffer, 0, bytesRead))
                        {
                            BinaryReader br = new BinaryReader(ms_readheader);
                            state.ver = br.ReadByte();//...封包版本號
                            state.cmd1 = br.ReadInt16();//...命令1
                            state.cmd2 = br.ReadInt16();//...命令2
                            state.receivePackageTotalSize = br.ReadInt32();//...封包內容總長度

                            if (state.receivePackageTotalSize <= 0)
                            {
                                //...沒有夾帶內容
                                state.isReadHeader = false; //...讀取完畢將標記改為false
                                state.receiveBuffer = null;
                                state.receiveBuffer = new byte[state.headerBufferSize]; //...Buffer 清空
                                state.receiveFileTemporarily = null;
                                ReceiveEvent(state);//...接收完畢號 處理事件   
                                //receiveDone.Set();
                            }
                            else
                            {
                                //...當有夾帶內容時
                                state.receiveBuffer = null;
                                state.receiveBuffer = new byte[state.receivePackageTotalSize];//... 080609 by randy
                                state.isReadHeader = true;//,,,將標記設定為讀過標頭,可以繼續接收資料                               
                            }
                        }

                        //...接收buffer的資料
                        userClient.BeginReceive(state.receiveBuffer, 0, state.receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallback), state);
                        return;

                    }
                    #endregion

                    #region//...儲存數據

                    DiskIO.Save(state.receiveFileTemporarily, state.receiveBuffer, bytesRead);

                    state.receivePackageTotalSize = state.receivePackageTotalSize - bytesRead;//--randy,2008.06.09                    
                    if (state.receivePackageTotalSize == 0)
                    {
                        //...接收完畢
                        state.isReadHeader = false;//...讀取Header reset
                        state.receiveBuffer = new byte[state.headerBufferSize]; //...recevice buffer reset
                        ReceiveEvent(state);//...接收完畢號 處理事件
                        //receiveDone.Set();
                    }
                    else if (state.receivePackageTotalSize > 0)
                    {
                        //...繼續接收
                        state.receiveBuffer = new byte[state.receivePackageTotalSize];//--randy,2008.06.09                     
                    }
                    #endregion

                    #region//...接收buffer的資料
                    //...接收buffer的資料
                    userClient.BeginReceive(state.receiveBuffer, 0, state.receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallback), state);

                    #endregion
                }
                else
                {
                    KConsole.Write(ErrorLevel.Warn, "Kernel>>AsynTCPClient>>ReadCallback>>", "接收到的數據長度<=0");
                    closeSocket = true;
                }
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynTCPClient>>ReceiveCallback>>", ex.Message);
                this._connected = false;
                closeSocket = true;
            }
            finally
            {
                if (closeSocket)
                {
                    //ExceptionHandlingEvent(state);
                    ////receiveDone.Set();
                    //state = null;
                    //GC.Collect();
                    if (ExceptionHandlingEvent != null)
                    {
                        ExceptionHandlingEvent(state);
                    }
                    state = null;
                    GC.Collect();  
                }


            }
        }







    }
}
