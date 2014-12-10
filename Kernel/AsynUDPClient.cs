using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Kernel
{
    public class AsynUDPClient
    {
        private ManualResetEvent _sendToDone = new ManualResetEvent(false);
        private ManualResetEvent _receiveDone = new ManualResetEvent(false);

        public event AsynUDPClientReceiveEvent ReceiveEvent;
        private string _listenIP;
        private int _listenPort;
        private Socket _server;
        private EndPoint RemoteEP;
        private int _receiveBuffer;


        public int SetReceiveBuffer
        {
            set
            {
                _receiveBuffer = value;
            }
        }


        public AsynUDPClient()
        {
          
        }

        ~AsynUDPClient()
        {
            _server = null;
            RemoteEP = null;
            _sendToDone = null;
            _receiveDone = null;
        }

        public void Connect(string listenIP, int listenPort)
        {
            _listenIP = listenIP;
            _listenPort = listenPort;       
   
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(_listenIP), _listenPort);
            RemoteEP = (EndPoint)ip;
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void BeginSend(byte[] buffer)
        {
            _sendToDone.Reset();
            _server.BeginSendTo(buffer, 0, buffer.Length, 0, RemoteEP, new AsyncCallback(SendToCallback), null);
            _sendToDone.WaitOne();
        }


        private void SendToCallback(IAsyncResult ar)
        {
            int send = _server.EndSendTo(ar);
            _sendToDone.Set();
        }

        public void BeginReceive()
        {
            //_receiveDone.Reset();
            //...建立接收的位置, 所有
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //...返回對方的位置
            EndPoint remoteEP = (EndPoint)(sender);

            UDPClientStateObject stateObject = new UDPClientStateObject();
            stateObject.workSocket = _server;
            stateObject.receiveBuffer = new byte[_receiveBuffer];
            _server.BeginReceiveFrom(stateObject.receiveBuffer, 0, _receiveBuffer, 0, ref remoteEP, new AsyncCallback(ReceiveFromCallback), stateObject);
            //_receiveDone.WaitOne();
        
        }


        private void ReceiveFromCallback(IAsyncResult ar)
        {

            UDPClientStateObject so = (UDPClientStateObject)ar.AsyncState;
            Socket socket = so.workSocket;

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tempRemoteEP = (EndPoint)sender;

            int receivedDataLength = socket.EndReceiveFrom(ar, ref tempRemoteEP);
            so.tempRemoteEP = tempRemoteEP;
            ReceiveEvent(so);//...交由外部處理
            //_receiveDone.Set();
        }



        public void Close()
        {
            _server.Shutdown(SocketShutdown.Both);
            _server.Close();            
        }
    }
}
