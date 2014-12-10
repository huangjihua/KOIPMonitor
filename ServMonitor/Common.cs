using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ServMonitor
{
    class Common
    {
        /// <summary>
        /// 未启动服务列表
        /// </summary>
        public static Dictionary<string, string> NoStartServList = new Dictionary<string, string>();

        /// <summary>
        /// 被监测服务列表
        /// </summary>
        public static DataTable DtServInfo=new DataTable();

        public static void RemoveServStartList(string ServName)
        {

            foreach (KeyValuePair<string, string> a in NoStartServList)
            {
                if (a.Key == ServName.ToString())
                {
                    //a.Value.States.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    NoStartServList.Remove(a.Key);
                    break;
                }
            }

        }
    }
}
