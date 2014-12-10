#define win
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Commonality
{
    public class FilePath
    {
        /// <summary>
        /// 目录
        /// </summary>
        /// 
//        private static string StrbigPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
//#if LINUX
//        private static string strDataPath = StrbigPath + "/".Trim();
//#else
//        private static string strDataPath = StrbigPath + "\\".Trim();
//#endif
        /// <summary>
        /// 服务器类型值{0:Linux；1:Windows}默认为Linux
        /// </summary>
        public static int OStype = 0;
        /// <summary>
        /// 配置文件路径
        /// </summary>        
        public static string ConfigFilePath = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/server.xml";
        public static string wirespeedpath = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/21788.mic";

        /// <summary>
        /// 设置全局变量路径
        /// </summary>
        public static void SetPubPath()
        {
            if (OStype != 0)
            {
                ConfigFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\server.xml";
                wirespeedpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\21788.mic";
            }
        }
        //public static string kmsPath = ConfigFilePath + "kmsinfo.xml";
        //public static string kisPath = ConfigFilePath + "kisinfo.xml";
        //public static string knsPath = ConfigFilePath + "knsinfo.xml";
        //public static string ServerInfo = ConfigFilePath + "server.xml";
        //public static string ServerModePath = ConfigFilePath + "ServerMode.xml";
        //public static string wirespeedpath = ConfigFilePath + "21788.mic";

       


























        /// <summary>
        /// 目录路径
        /// </summary>
        /// <returns></returns>
        private static string getdirpath()
        {
            try
            {
                //static string strDataPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\";
                string strDataPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string[] temp = strDataPath.Split('\\');
                string strReturnPath = string.Empty;
                strReturnPath = strDataPath.Replace(temp[temp.Length - 2], "").Replace("\\\\", "");//+ "\\KoIpService\\" +KoIP.AppUtility.Encrypt.MD5Encrypt("kms.dll");
                return strReturnPath;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
