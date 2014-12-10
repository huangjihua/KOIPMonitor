using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;
using System.Reflection;

namespace Kernel
{
    public enum ErrorLevel
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
        /// 除錯
        /// </summary>
        Debug = 2,
        /// <summary>
        /// 應答
        /// </summary>
        Response = 3,
    }
  //static Queue queue_SeriousMessage= new Queue();
    public class KConsole
    {
        public static string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Log\\";

        static Queue queue_SeriousMessage= new Queue();
        static Queue queue_WarnMessage = new Queue();
        static Queue queue_DebugMessage = new Queue();

        static System.Timers.Timer timer_SeriousLogWorker = null;
        static System.Timers.Timer timer_WarnLogWorker = null;
        static System.Timers.Timer timer_DebugLogWorker = null;

        static object DebugLock = new object();

        /// <summary>
        /// 將訊息寫到對應的地方, Level 表示: 0=嚴重 ,1=警告, 2=除錯,3=應答
        /// </summary>
        public static void Write(ErrorLevel _level, string _Source, string _Description)
        {
            try
            {
                Hashtable _data = new Hashtable();
                _data.Add("_Level", (int)_level);
                _data.Add("_Source", _Source);
                _data.Add("_Description", _Description);
                switch ((int)_level)
                {
                    case 0:
                        ThreadPool.QueueUserWorkItem(new WaitCallback(thread_WriteToSeriousLog), _data);
                        break;
                    case 1:
                        ThreadPool.QueueUserWorkItem(new WaitCallback(thread_WriteToWarnLog), _data);
                        break;
                    case 2:
                        ThreadPool.QueueUserWorkItem(new WaitCallback(thread_WriteToDebugLog), _data);
                        break;
                    case 3:
                        thread_WriteToConsole(_data);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>Write>>" + ex.Message);                
            }
        }

        /// <summary>
        /// 啟動系統日誌
        /// </summary>
        public static void StartWorker()
        {
            DirectoryInfo Createdir = new DirectoryInfo(directoryName);
            if (!Createdir.Exists)
            {
                Createdir.Create();
            }

            timer_SeriousLogWorker = new System.Timers.Timer();
            timer_SeriousLogWorker.Elapsed += new System.Timers.ElapsedEventHandler(RunSeriousLogWorker);
            timer_SeriousLogWorker.Interval = 1000;
            timer_SeriousLogWorker.AutoReset = true;
            timer_SeriousLogWorker.Enabled = true;


            timer_WarnLogWorker = new System.Timers.Timer();
            timer_WarnLogWorker.Elapsed += new System.Timers.ElapsedEventHandler(RunWarnLogWorker);
            timer_WarnLogWorker.Interval = 1000;
            timer_WarnLogWorker.AutoReset = true;
            timer_WarnLogWorker.Enabled = true;


            timer_DebugLogWorker = new System.Timers.Timer();
            timer_DebugLogWorker.Elapsed += new System.Timers.ElapsedEventHandler(RunDebugLogWorker);
            timer_DebugLogWorker.Interval = 1000;
            timer_DebugLogWorker.AutoReset = true;
            timer_DebugLogWorker.Enabled = true;
            
        }


        private static void RunSeriousLogWorker(Object source, System.Timers.ElapsedEventArgs e)
        {           
            try
            {
                if (queue_SeriousMessage.Count > 0)
                {
                    lock (queue_SeriousMessage.SyncRoot)
                    {
                        if (queue_SeriousMessage.Count <= 0) return;
                        StringBuilder sb = (StringBuilder)queue_SeriousMessage.Dequeue();

                        string FullnameLogFile = directoryName + "Serious." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

                        FileInfo findlogfile = new FileInfo(FullnameLogFile);
                        StringBuilder sb_olddata = null;
                        if (findlogfile.Exists)
                        {
                            sb_olddata = new StringBuilder();
                            using (FileStream fs_open = new FileStream(FullnameLogFile, FileMode.Open, FileAccess.ReadWrite))
                            {
                                using (StreamReader sr = new StreamReader(fs_open, Encoding.UTF8))
                                {
                                    sb.Append(sr.ReadToEnd());
                                }
                            }
                        }
                        findlogfile = null; 

                        //using (FileStream fs_save = new FileStream(FullnameLogFile, FileMode.Create, FileAccess.ReadWrite))
                        //{ 
                        //    using (StreamWriter sw = new StreamWriter(fs_save, Encoding.UTF8))
                        //    {
                        //        if (sb_olddata != null)
                        //        {
                        //            sb_olddata.Insert(0, Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8)));
                        //            sw.WriteLine(sb_olddata.ToString());
                        //        }
                        //        else
                        //        {
                        //            string strUTF8 = Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8));
                        //            sw.WriteLine(strUTF8);
                        //        }
                        //        sw.Flush();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>RunSeriousLogWorker>>" + ex.Message);
            }
        }

        private static void RunWarnLogWorker(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (queue_WarnMessage.Count > 0)
                {
                    lock (queue_WarnMessage.SyncRoot)
                    {
                        if (queue_WarnMessage.Count <= 0) return;
                        StringBuilder sb = (StringBuilder)queue_WarnMessage.Dequeue();
                      
                        string FullnameLogFile = directoryName + "Warn." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

                        FileInfo findlogfile = new FileInfo(FullnameLogFile);
                        StringBuilder sb_olddata = null;
                        if (findlogfile.Exists)
                        {
                            sb_olddata = new StringBuilder();
                            using (FileStream fs_open = new FileStream(FullnameLogFile, FileMode.Open, FileAccess.ReadWrite))
                            {
                                using (StreamReader sr = new StreamReader(fs_open, Encoding.UTF8))
                                {
                                    sb.Append(sr.ReadToEnd());
                                }
                            }
                        }
                        findlogfile = null; 

                        //using (FileStream fs_save = new FileStream(FullnameLogFile, FileMode.Create, FileAccess.ReadWrite))
                        //{
                        //    using (StreamWriter sw = new StreamWriter(fs_save, Encoding.UTF8))
                        //    {
                        //        if (sb_olddata != null)
                        //        {
                        //            sb_olddata.Insert(0, Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8)));
                        //            sw.WriteLine(sb_olddata.ToString());
                        //        }
                        //        else
                        //        {
                        //            string strUTF8 = Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8));
                        //            sw.WriteLine(strUTF8);
                        //        }
                        //        sw.Flush();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>RunWarnLogWorker>>" + ex.Message);
            }
        }

        private static void RunDebugLogWorker(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (queue_DebugMessage.Count > 0)
                {
                    lock (queue_DebugMessage.SyncRoot)
                    {
                        if (queue_DebugMessage.Count <= 0) return;
                        StringBuilder sb = (StringBuilder)queue_DebugMessage.Dequeue();

                      
                        //lock (DebugLock)
                        //{

                            string FullnameLogFile = directoryName + "Debug." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

                           
                            //FileInfo findlogfile = new FileInfo(FullnameLogFile);
                            StringBuilder sb_olddata = null;
                            if (File.Exists(FullnameLogFile))
                            {
                                //...檔案存在
                                sb_olddata = new StringBuilder();
                                using (FileStream fs_open = new FileStream(FullnameLogFile, FileMode.Open, FileAccess.ReadWrite))
                                {
                                    using (StreamReader sr = new StreamReader(fs_open, Encoding.UTF8))
                                    {
                                        sb.Append(sr.ReadToEnd());
                                    }
                                }
                            }
                            //findlogfile = null;

                            //using (FileStream fs_save = new FileStream(FullnameLogFile, FileMode.Create, FileAccess.ReadWrite))
                            //{
                            //    using (StreamWriter sw = new StreamWriter(fs_save, Encoding.UTF8))
                            //    {
                            //        if (sb_olddata != null)
                            //        {
                            //            sb_olddata.Insert(0, Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8)));
                            //            sw.WriteLine(sb_olddata.ToString());
                            //        }
                            //        else
                            //        {
                            //            string strUTF8 = Encoding.UTF8.GetString(Tools.StringEncodingConver(sb.ToString(), Encoding.Default, Encoding.UTF8));
                            //            sw.WriteLine(strUTF8);
                            //        }
                            //        sw.Flush();
                            //    }
                            //}
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>RunDebugLogWorker>>" + ex.Message);
            }
        }

        private static void thread_WriteToSeriousLog(object _obj)
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
                //lock (queue_SeriousMessage.SyncRoot)
                //{
                    queue_SeriousMessage.Enqueue(sb);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>thread_WriteToSeriousLog>>" + ex.Message);
            }
        }

        private static void thread_WriteToWarnLog(object _obj)
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
                //lock (queue_WarnMessage.SyncRoot)
                //{
                    queue_WarnMessage.Enqueue(sb);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>thread_WriteToWarnLog>>" + ex.Message);
            }
        }

        private static void thread_WriteToDebugLog(object _obj)
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
                //lock (queue_DebugMessage.SyncRoot)
                //{
                    queue_DebugMessage.Enqueue(sb);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>thread_WriteToDebugLog>>" + ex.Message);
            }
        }

      

        private static void thread_WriteToConsole(object _obj)
        {
            try
            {
                Hashtable _data = (Hashtable)_obj;              
                string Description = _data["_Description"].ToString();
                Console.WriteLine(Encoding.UTF8.GetString(Tools.StringEncodingConver(Description.ToString(), Encoding.Default, Encoding.UTF8)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kernel>>KConsole>>thread_WriteToConsole" + ex.Message);
            }
        }

   
    }
}