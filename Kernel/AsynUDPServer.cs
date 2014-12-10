using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Kernel
{
    public class AsynUDPServer
    {
        private ManualResetEvent _allDone = new ManualResetEvent(false);
        public event AsynUDPServerReceiveEvent ReceiveEvent;
        private Socket _socket;
        private IPEndPoint _ipLocalEndPoint;//...設定傾聽的IP及Port 
        private int _receiveBuffer;        

        public int SetReceiveBuffer
        {
            set
            {
                _receiveBuffer = value;
            }
        }

        //...判斷Listen Socket 是否已經綁定端口位置
        public bool IsBound
        {
            get
            {
                if (_socket != null)
                {
                    return _socket.IsBound;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Stop()
        {
            _socket.Close();
        }

        public void Start()
        {          
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);           
            _socket.Bind(_ipLocalEndPoint);        
            KConsole.Write(ErrorLevel.Debug, "AsynUDPServer>>Start>>Listen:", ((EndPoint)_ipLocalEndPoint).ToString());
            Thread listen = new Thread(new ThreadStart(Run));
            listen.Start();
        }

        private void Run()
        {
            //...建立接收的位置, 所有
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //...返回對方的位置
            EndPoint remoteEP = (EndPoint)(sender);
            try
            {
                while (true)
                {
                    _allDone.Reset();
                    UDPServerStateObject stateObject = new UDPServerStateObject();
                    stateObject.workSocket = _socket;
                    stateObject.receiveBuffer = new byte[_receiveBuffer];
                    _socket.BeginReceiveFrom(stateObject.receiveBuffer, 0, _receiveBuffer, 0, ref remoteEP, new AsyncCallback(ReceiveFromCallback), stateObject);
                    _allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                _allDone.Set();
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynUDPServer>>Run", ex.Message);
            }
        }

        private void ReceiveFromCallback(IAsyncResult ar)
        {
            try
            {
                UDPServerStateObject so = (UDPServerStateObject)ar.AsyncState;
                Socket socket = so.workSocket;

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint tempRemoteEP = (EndPoint)sender;
                int receivedDataLength = socket.EndReceiveFrom(ar, ref tempRemoteEP);
                so.tempRemoteEP = tempRemoteEP;

                ReceiveEvent(so);//...交由外部處理
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>AsynUDPServer>>ReceiveFromCallback", ex.Message);
            }
            finally
            {
                _allDone.Set();
            }
        }

        public AsynUDPServer()
        {
            _ipLocalEndPoint = new IPEndPoint(IPAddress.Any,523);
        }

        public AsynUDPServer(int listenPort)
        {
            //...傾聽此服務器所有網卡上的 指定端口封包
            _ipLocalEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
        }

        public AsynUDPServer(string listenIP, int listenPort)
        {
            //...傾聽此服務器指定的IP 及 端口
            _ipLocalEndPoint = new IPEndPoint(System.Net.IPAddress.Parse(listenIP), listenPort);
        }

        ~AsynUDPServer()
        {
            _allDone = null;
            _socket = null;           
            _ipLocalEndPoint = null;
            KConsole.Write(ErrorLevel.Debug, "Kernel>>~AsynUDPServer>>", "AsynUDPServer Dispose");
        }
    }
}
