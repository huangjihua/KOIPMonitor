using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using System.Threading;
using Commonality;
namespace KOIPMonitor
{
    public class tcpServer_ExceptionHandlingEvent
    {
        public static void ReceiveEvent(StateObject requset)
        {
            try
            {

                if (requset == null)
                {
                    ConsoleManage.Write(Commonality.ErrorLevel.Response, "", "requset is null");
                    return;
                }
                int ret = 0;
                ret=CommClass.GetDownLineType(requset);
                switch (ret)
                {
                    case 1:
                        CommClass.RemoveClientConnList(requset);
                        break;
                    case 2:
                        CommClass.RevDevObjList(requset);
                        break;

                }

            }
            catch(Exception ex)
            {
                ConsoleManage.Write(Commonality.ErrorLevel.Response, "tcpServer_ExceptionHandlingEvent>>ReceiveEvent>>", ex.Message);
                return;
            }
            
       }
    }

}
