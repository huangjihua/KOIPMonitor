using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using Kernel;
using System.IO;
using System.Data;

namespace KOIPMonitor
{
    class RspServOpt
    {
        public RspServOpt() { }
        ~RspServOpt() { }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void process(StateObject request)
        {
            try
            {
                if (request != null)
                {

                    short cmd1 = request.cmd1;//主命令字
                    short cmd2 = request.cmd2;//子命令字
                    byte[] ByteResult = null;
                    //if (request.cmd2)
                    //{
                        
                    //}
                    //ByteResult = File.ReadAllBytes(request.receiveFileTemporarily);

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
                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", "StateObject request==null");
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "RspGetServState>>process>>", ex.Message);

            }
            finally
            {
                //删除文件
                //if (!string.IsNullOrEmpty(request.receiveFileTemporarily))
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), request.receiveFileTemporarily);

                //GC.Collect();
            }
            
        }
     
       
    }
        
    
}
