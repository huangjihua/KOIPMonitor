using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using System.Threading;
using Commonality;
namespace KOIPMonitor
{
    public class tcpServer_ReceiveEvent
    {
        public static void ReceiveEvent(StateObject OmsState)
        {
            try
            {

                if (OmsState == null)
                    return;

                switch (OmsState.cmd1)
                {

                    case  OMSCmd.RspServOpt:
                        RspServOpt.process(OmsState);
                        break;
                    case  OMSCmd.UpDevState:
                        UpDevState.process(OmsState);
                        break;
                    case OMSCmd.ReqGetServList://启动服务请求
                        if (CommClass.ISCONUP == "0")
                        {
                            RspGetServList.process(OmsState);//启动服务回应
                        }
                        break;
                    case OMSCmd.ReqServOpt://启动服务请求
                        ReqServOpt.process(OmsState);//启动服务回应
                        break;
                    case OMSCmd.ReqGetServState://获取服务器状态请求
                        RspGetServState.process(OmsState);//获取服务器状态回应
                        break;
                    case OMSCmd.ReqUserLogin://获取服务器状态请求
                        if (CommClass.ISCONUP == "0")
                        {
                            RspUserLogin.process(OmsState);//获取服务器状态回应
                        }
                        break;
                    case CommCmd.Handshake://握手
                        //byte[] ResultByte=null;
                        //CommonFunction.HandshakeByte(ref ResultByte);
                        //OmsState.Send(ResultByte);
                        CommonFunction.SendHandshake(OmsState);
                        break;
                    case OMSCmd.DevTypeAdd://设备类型添加
                        if (CommClass.ISCONUP == "0")
                        {
                            DevTypeAdd.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevTypeDelete://设备类型删除
                        if (CommClass.ISCONUP == "0")
                        {
                            DevTypeDelete.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevTypeModify://设备类型修改
                        if (CommClass.ISCONUP == "0")
                        {
                            DevTypeModify.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevTypeQuery://设备类型查询
                        if (CommClass.ISCONUP == "0")
                        {
                            DevTypeQuery.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevInfoAdd://设备信息添加
                        if (CommClass.ISCONUP == "0")
                        {
                            DevInfoAdd.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevInfoDelete://设备信息删除
                        if (CommClass.ISCONUP == "0")
                        {
                            DevInfoDelete.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevInfoModify://设备信息修改
                        if (CommClass.ISCONUP == "0")
                        {
                            DevInfoModify.process(OmsState);
                        }
                        break;
                    case OMSCmd.DevInfoQuery://设备信息查询
                        if (CommClass.ISCONUP == "0")
                        {
                            DevInfoQuery.process(OmsState);
                        }
                        break;
                    case OMSCmd.UserInfoAdd://用户添加
                        if (CommClass.ISCONUP == "0")
                        {
                            UserInfoAdd.process(OmsState);
                        }
                        break;
                    case OMSCmd.UserInfoDelete://用户删除
                        if (CommClass.ISCONUP == "0")
                        {
                            UserInfoDelete.process(OmsState);
                        }
                        break;
                    case OMSCmd.UserInfoModify://用户修改
                        if (CommClass.ISCONUP == "0")
                        {
                            UserInfoModify.process(OmsState);
                        }
                        break;
                    case OMSCmd.UserInfoQuery://用户查询
                        if (CommClass.ISCONUP == "0")
                        {
                            UserInfoQuery.process(OmsState);
                        }
                        break;
                    case OMSCmd.ChangePwd://用户密码修改
                        if (CommClass.ISCONUP == "0")
                        {
                            ChangePwd.process(OmsState);
                        }
                        break;
                    case OMSCmd.AlarmLogQuery://故障日志查询
                        if (CommClass.ISCONUP == "0")
                        {
                            AlarmLogQuery.process(OmsState);
                        }
                        break;
                    case OMSCmd.AlarmLogClear://故障日志清除
                        if (CommClass.ISCONUP == "0")
                        {
                            AlarmLogClear.process(OmsState);
                        }
                        break;
                    case OMSCmd.KOIPOnline://KOIP挂线查询
                        if (CommClass.ISCONUP == "0")
                        {
                            KOIPOnline.process(OmsState);
                        }
                        break;
                    default:
                        break;
                }
  
            }
            catch (Exception ex)
            {

                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>tcpServer_ReceiveEvent>>KOIPMonitor  TCP 連接接收處理>>", ex.Message);
            }
            finally
            {
                if (!string.IsNullOrEmpty(OmsState.receiveFileTemporarily))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), OmsState.receiveFileTemporarily);
                    //KdsState.workSocket.Close();
                    //KdsState.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                }
                
            }
        }
    }
}
