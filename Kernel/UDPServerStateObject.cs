using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Kernel
{
    public class UDPServerStateObject
    {
        private ManualResetEvent _sendToDone = new ManualResetEvent(false);
        private Socket _workSocket;
        private byte[] _receiveBuffer = null;
        private EndPoint _tempRemoteEP = null;
        private byte _ver;
        private short _cmd;
        private int _packageLength;

        public int packageLength
        {
            get
            {
                return this._packageLength;

            }
            set
            {
                this._packageLength = value;
            }
        }

        public short cmd
        {
            get
            {
                return this._cmd;

            }
            set
            {
                this._cmd = value;
            }
        }

        public byte ver
        {
            get
            {
                return this._ver;

            }
            set
            {
                this._ver = value;
            }
        }

        public Socket workSocket
        {
            get
            {
                return _workSocket;
            }
            set
            {
                this._workSocket = value;
            }
        }

        public byte[] receiveBuffer
        {
            get
            {
                return this._receiveBuffer;

            }
            set
            {
                this._receiveBuffer = value;
            }
        }

        public EndPoint tempRemoteEP
        {
            get
            {
                return _tempRemoteEP;
            }
            set
            {
                this._tempRemoteEP = value;
            }
        }

        private void SendToCallback(IAsyncResult ar)
        {
            int send = _workSocket.EndSendTo(ar);
            _sendToDone.Set();
        }

        public void BeginSendTo(byte[] buffer)
        {
            _sendToDone.Reset();
            try
            {
                _workSocket.BeginSendTo(buffer, 0, buffer.Length, 0, _tempRemoteEP, new AsyncCallback(SendToCallback), null);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>UDPServerStateObject>>BeginSendTo>>", ex.Message);
            }
            _sendToDone.WaitOne();
        }

        public UDPServerStateObject()
        {
           
        }

        ~UDPServerStateObject()
        {
            _receiveBuffer = null;
            _tempRemoteEP = null;
            _sendToDone = null;
        }
    }
}
