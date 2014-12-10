using System;
using System.Collections.Generic;
using System.Text;
using ServMonitor;
using System.Data;
using System.Collections;
using System.Threading;
namespace KOIPMonitor
{
    class ServMonitor_OptHandlingEvent
    {
        public static void OptEvent(ServOptState sender)
        {
            try
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,"sender", "\n"+sender.ToString());


                short cmd1 = 0;//主命令字
                short cmd2 = 0;//子命令字
                cmd1 = OMSCmd.RspServOpt;
                cmd2 = ErrCommon.Success;
                byte[] ByteResult = null;
                int OptResult = 0;//服务器操作结果
                try
                {
                    OptResult = Convert.ToInt32(sender.OPTSTATE.ToString());
                }
                catch
                {
                    OptResult = -1;
                }

                switch (OptResult)

                {
                    case 0:
                        cmd2 =0;
                        break;
                    case -1:
                        cmd2 = -8029;
                        break;
                    case -2:
                        cmd2 = -8030;
                        break;
                    default:
                        cmd2 = -8031;
                        break;
                }

                if (CommClass.ISCONUP == "0")
                {
                    foreach (KeyValuePair<string, Kernel.StateObject> a in CommClass.ClientConnList)
                    {
                        Hashtable _hashtable_Package = new Hashtable();
                        _hashtable_Package.Add("1", a.Value);//...連結位置 
                        _hashtable_Package.Add("2", cmd1);
                        _hashtable_Package.Add("3", cmd2);
                        _hashtable_Package.Add("4", ByteResult);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package);
                    }
                }
                else
                {
                    Hashtable _hashtable_Package = new Hashtable();
                    _hashtable_Package.Add("1", ConnectUpServ.tcpClient);//...連結位置 
                    _hashtable_Package.Add("2", cmd1);
                    _hashtable_Package.Add("3", cmd2);
                    _hashtable_Package.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.UnGzipTcpClientSend), _hashtable_Package);

                }



                
                
            }
            catch (Exception ex)
            {
                //Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "BusinessDAL.KNS>>tcpClient_ExceptionHandlingEvent>>ReceiveEvent>>", ex.Message);
            }
            finally
            {

            }
        }

    }
}
