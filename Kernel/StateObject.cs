using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Net;

namespace Kernel
{
    [Serializable]
    public class StateObject
    {
        private ManualResetEvent _allDone = new ManualResetEvent(false);
 
        private Object thisSendLock = new Object();
        private byte[] _receiveBuffer = null;
        private bool _isReadHeader = false;
        private string _receiveFileTemporarily = null;
        private string _sentResultFileTemporarily = null;


        /// <summary>
        /// 第一個指令
        /// </summary>
        private byte _ver = 0;
        private short _cmd1 = 0;
        private short _cmd2 = 0;
        private int _receivePackageTotalSize = 0;
        private int _headerBufferSize = 0;
        private string _connectionSession = null;
        private Socket _workSocket = null;
        public List<byte> ListHeadBuffer = new List<byte>();

        public StateObject()
        {

        }

        ~StateObject()
        {
            _receiveBuffer = null;
            thisSendLock = null;
            if (_workSocket != null)
            {
                _workSocket.Close(3000);
            }
            //GC.Collect();
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

        /// <summary>
        /// 標記,判斷是否讀過封包頭
        /// </summary>
        public bool isReadHeader
        {
            get
            {
                return this._isReadHeader;
            }
            set
            {
                this._isReadHeader = value;
            }
        }

        /// <summary>
        /// 封包收進來後 暫存在哪個路徑
        /// </summary>
        public string receiveFileTemporarily
        {
            get
            {
                return this._receiveFileTemporarily;

            }
            set
            {
                this._receiveFileTemporarily = value;
            }
        }


        /// <summary>
        /// 封包header 
        /// </summary>
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

        /// <summary>
        /// 封包header 
        /// </summary>
        public short cmd1
        {
            get
            {
                return this._cmd1;
            }
            set
            {
                this._cmd1 = value;
            }
        }

        /// <summary>
        /// 封包header 
        /// </summary>
        public short cmd2
        {
            get
            {
                return this._cmd2;
            }
            set
            {
                this._cmd2 = value;
            }
        }

        /// <summary>
        /// 封包header 
        /// </summary>
        public int receivePackageTotalSize
        {
            get
            {
                return this._receivePackageTotalSize;

            }
            set
            {
                this._receivePackageTotalSize = value;
            }
        }

        public string sentResultFileTemporarily
        {
            get
            {
                return this._sentResultFileTemporarily;
            }
            set
            {
                this._sentResultFileTemporarily = value;
            }
        }

        public string ConnectionSession
        {
            get
            {
                return this._connectionSession;
            }
            set
            {
                this._connectionSession = value;
            }
        }

        /// <summary>
        /// 設定封包 header 接收長度
        /// </summary>
        public int headerBufferSize
        {
            get
            {
                return _headerBufferSize;
            }
            set
            {
                _headerBufferSize = value;
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

        /// <summary>
        /// 客戶端過來聯外網路位置
        /// </summary>
        public string wanIP
        {
            get
            {
                return ((IPEndPoint)workSocket.RemoteEndPoint).Address.ToString();
            }
        }

        /// <summary>
        /// 客戶端過來聯外網路端口
        /// </summary>
        public int wanPort
        {
            get
            {
                return Convert.ToInt32(((IPEndPoint)workSocket.RemoteEndPoint).Port.ToString());
            }
        }       

        public void Send(byte[] data)
        {
            try
            {
                if (!_workSocket.Connected) return;
                if (_workSocket == null) return;
                _allDone.Set();
                this.workSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), null);
                _allDone.WaitOne();
            }
            catch (Exception ex)
            {
                //_workSocket.Shutdown(SocketShutdown.Both);
                KConsole.Write(ErrorLevel.Response, "", "KoIPRouter>>StateObject>>Send>>Exception:" + ex.Message);
               // KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>Send", ex.Message);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            int bytesSent = 0;
            try
            {
                if (!_workSocket.Connected) return;
                if (_workSocket == null) return;
                bytesSent = _workSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Response, "", "KoIPRouter>>StateObject>>SendCallback>>Exception:" + ex.Message);
                //KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>SendCallback", ex.Message);
            }

        }
    }
}
