using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using System.Threading;
using Commonality;
namespace KOIPMonitor
{
    class tcpClient_ExceptionHandlingEvent
    {
        public static void ReceiveEvent(TCPClientStateObject sender)
        {
            try
            {
                if (sender == null)
                    return;

                /////KMS端指令以1000起始
                switch (sender.cmd1)
                {
                    ///处理KIS连到到KMS将IPS存储在KMS
                    case 11:
                       //KoIpServerLibrary.KMS_S._1000.Procedure(KmsState);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
               //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "BusinessDAL.KNS>>tcpClient_ExceptionHandlingEvent>>ReceiveEvent>>", ex.Message);
            }
            finally
            {
                if (!string.IsNullOrEmpty(sender.receiveFileTemporarily))
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), sender.receiveFileTemporarily);
            }
        }
    }
}

