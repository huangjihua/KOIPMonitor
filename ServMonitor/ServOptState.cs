using System;
using System.Collections.Generic;
using System.Text;


namespace ServMonitor
{
    public class ServOptState
    {

        /// <summary>
        /// 本地服务器ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 本地服务器名称
        /// </summary>                        
        public string NAME;
        /// <summary>
        /// 本地服务器类型
        /// </summary>
        public string TYPE;
        /// <summary>
        /// 操作结果状态
        /// </summary>
        public string OPTSTATE;

        ///// <summary>
        /////返回连接
        ///// </summary>
        //public StateObject Request=null;

        public override string ToString()
        {
            return "ID=" + ID + "\n" + "NAME=" + NAME + "\n" + "TYPE=" + TYPE + "\n" + "STATE=" + OPTSTATE + "\n";
        }
    }
}
