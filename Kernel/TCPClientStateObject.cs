using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Net;

namespace Kernel
{
    public class TCPClientStateObject
    {
        //private ManualResetEvent sendDone = new ManualResetEvent(false);
        //private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private Object thisSendLock = new Object();
        private byte[] _receiveBuffer;
        private bool _isReadHeader;
        private string _receiveFileTemporarily;
        private string _sentResultFileTemporarily;


        /// <summary>
        /// 第一個指令
        /// </summary>
        private byte _ver;
        private short _cmd1;
        private short _cmd2;
        private int _receivePackageTotalSize;
        private int _headerBufferSize;
        private string _connectionSession;
        private Socket _workSocket;
        public List<byte> ListHeadBuffer = new List<byte>();
        public TCPClientStateObject()
        {

        }

        ~TCPClientStateObject()
        {
            try
            {
                _receiveBuffer = null;
                thisSendLock = null;
                if (_workSocket != null)
                {
                    _workSocket.Close();
                }
                //GC.Collect();
            }
            catch { }
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
                try
                {
                    return ((IPEndPoint)workSocket.RemoteEndPoint).Address.ToString();
                }
                catch(Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 客戶端過來聯外網路端口
        /// </summary>
        public int wanPort
        {
            get
            {
                 try
                {
                return Convert.ToInt32(((IPEndPoint)workSocket.RemoteEndPoint).Port.ToString());
                }
                 catch (Exception ex)
                 {
                     return 0;
                 }
            }
        }

        //public string userLoginID
        //{
        //    get
        //    {
        //        return this._userLoginID;
        //    }
        //    set
        //    {
        //        this._userLoginID = value;
        //    }
        //}

        //public SortedList userTable
        //{
        //    get
        //    {
        //        return this._userTable;
        //    }
        //    set
        //    {
        //        this._userTable = value;
        //    }
        //}

        //public string roomID
        //{
        //    get
        //    {
        //        return this._roomID;
        //    }
        //    set
        //    {
        //        this._roomID = value;
        //    }
        //}

        //public SortedList roomTable
        //{
        //    get
        //    {
        //        return this._roomTable;
        //    }
        //    set
        //    {
        //        this._roomTable = value;
        //    }
        //}

        //public string roomLineName
        //{
        //    get
        //    {
        //        return this._roomLineName;
        //    }
        //    set
        //    {
        //        this._roomLineName = value;
        //    }
        //}

        public void Send(byte[] data)
        {
            try
            {
                if (_workSocket == null) return;
                if (!_workSocket.Connected) return;
                
                this.workSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), null);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>Send", ex.Message);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            int bytesSent = 0;
            try
            {
                if (_workSocket == null) return;
                if (!_workSocket.Connected) return;
                bytesSent = _workSocket.EndSend(ar);
            }
            catch (ObjectDisposedException ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>StateObject>>SendCallback", ex.Message);
            }

        }
    }
}
