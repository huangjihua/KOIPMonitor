using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using System.Threading;
using Commonality;
namespace KOIPMonitor
{
    class tcpClient_ReceiveEvent
    {
        public static void ReceiveEvent(TCPClientStateObject request)
        {
            try
            {
                if (request == null)
                    return;

                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response,
                //    "TCPCLIENT RECEIVE", "CMD1=" + request.cmd1.ToString() + " "
                //    + "CMD2=" + request.cmd2.ToString() + " from " +
                //    request.wanIP.ToString() + ":" + request.wanPort.ToString());



                switch (request.cmd1)
                {
                    case OMSCmd.ReqServOpt://启动服务请求
                        ReqServOpt.process(request);//启动服务回应
                        break;
                    //case OMSCmd.ReqGetServState://获取服务器状态请求
                    //    RspGetServState.process(request);//获取服务器状态回应
                        //break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "BusinessDAL.KNS>>tcpClient_ReceiveEvent>>ReceiveEvent>>", ex.Message);

            }
            finally
            {
                //if (!string.IsNullOrEmpty(request.receiveFileTemporarily))
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), request.receiveFileTemporarily);
            }
        }
    }
}


