using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Kernel
{
    public class UDPClientStateObject
    {
        private Socket _workSocket;
        private byte[] _receiveBuffer = null;
        private EndPoint _tempRemoteEP = null;

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

        public UDPClientStateObject()
        {

        }

        ~UDPClientStateObject()
        {
            _receiveBuffer = null;
            _tempRemoteEP = null;
        }

    }
}
