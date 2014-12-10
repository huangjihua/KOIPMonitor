using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Threading;

namespace ServMonitor
{
    class ConsoleManage
    {
        static ManualResetEvent _WriteLogDone = new ManualResetEvent(false);
        static Queue myWriteQ = new Queue();
        static System.Timers.Timer aTimer = null;

        /// <summary>
        /// 將訊息寫到 Console, Level 表示: 0=錯誤 ,1=警示, 2=普通訊息
        /// </summary>
        static public void Write(ErrorLevel _level, string _Source, string _Description)
        {
            try
            {
                Hashtable _data = new Hashtable();
                _data.Add("_Level", (int)_level);
                _data.Add("_Source", _Source);
                _data.Add("_Description", _Description);
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadWriteToConsole), _data);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(threadWriteToLog), _data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ConsoleManage>>WriteToConsole>>WriteToConsole", ex.Message);
                //PrintLog(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 初始化Timer对象
        /// </summary>
        public static void objectTime()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(Execute);
            aTimer.Interval = 1000;//相隔多长时间跑一次3600000
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }


        public static void Execute(Object source, System.Timers.ElapsedEventArgs e)
        {
            //aTimer.Dispose();
            PrintValues(myWriteQ);//fa you jian 
            //继续循环判断
            //objectTime();
        }

        public static void PrintValues(IEnumerable myCollection)
        {
            try
            {
                foreach (Object data in myCollection)
                {
                    PrintLog(data.ToString());
                    myWriteQ.Dequeue();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Console.Write("    {0}", obj);
            //Console.WriteLine();
            //MessageBox.Show(obj.ToString());
        }

        static private void threadWriteToLog(object _obj)
        {
            _WriteLogDone.Reset();
            try
            {
                Hashtable _data = (Hashtable)_obj;
                string Level = _data["_Level"].ToString();
                string Source = _data["_Source"].ToString();
                string Description = _data["_Description"].ToString();
                string _DateTime = DateTime.Now.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(" 日期：[" + _DateTime + "]");
                sb.AppendLine(" Level : " + Level);
                sb.AppendLine(" Source : " + Source);
                sb.AppendLine(" Description : " + Description);
                sb.AppendLine(" ================================================================");
                //sb.AppendLine();              
                //Console.Write(sb.ToString());
                //myWriteQ.Enqueue(sb.ToString());
                PrintLog(sb.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("ConsoleManage>>threadWriteToConsole>>threadWriteToConsole", ex.Message);
                //PrintLog(ex.Message.ToString());
            }
            _WriteLogDone.WaitOne();
        }

        static private void threadWriteToConsole(object _obj)
        {
            try
            {
                Hashtable _data = (Hashtable)_obj;
                string Level = _data["_Level"].ToString();
                string Source = _data["_Source"].ToString();
                string Description = _data["_Description"].ToString();
                string _DateTime = DateTime.Now.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(" [ " + _DateTime + " ]");
                sb.AppendLine(" Level : " + Level);
                sb.AppendLine(" Source : " + Source);
                sb.AppendLine(" Description : " + Description);
                sb.AppendLine();
                Console.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ConsoleManage>>threadWriteToConsole>>threadWriteToConsole", ex.Message);
                //PrintLog(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 错误信息输出日志
        /// </summary>
        /// <param name="data"></param>
        private static void PrintLog(string data)
        {
            Object thisLock = new Object();
            //lock (thisLock)
            //{
            #region aaa

            string LogFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Log\\"; //System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + System.Configuration.ConfigurationSettings.AppSettings["LogFilePath"].ToString() + "\\";
            if (CommClass.OStype==0)
            {
                LogFilePath = "./Log/"; 
            }

            
            if (string.IsNullOrEmpty(LogFilePath))
            {
                //ConsoleManage.Write(ErrorLevel.Warn, "Kernel>>ConsoleManage>>Save", "filePath is null or empty");
                //throw new ArgumentNullException();
            }
            if (!System.IO.Directory.Exists(LogFilePath))
                Directory.CreateDirectory(LogFilePath);
            using (StreamWriter sw = new StreamWriter(LogFilePath + DateTime.Now.ToString("yyyyMMddHH") + "log" + ".txt", true, System.Text.Encoding.GetEncoding("gb2312")))
            {
                sw.WriteLine(data.ToString());
                //sw.WriteLine("");
            }

            #endregion
            //}
        }
    }
     enum ErrorLevel
    {
        /// <summary>
        /// 嚴重
        /// </summary>
        Serious = 0,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 1,
        /// <summary>
        /// 安全
        /// </summary>
        Safe = 2,
        Response = 3,
    }
}
