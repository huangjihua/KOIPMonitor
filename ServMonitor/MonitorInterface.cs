using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;using System.Data;



namespace ServMonitor
{
    
    public class MonitorInterface
    {
        private static ManualResetEvent _allDone = new ManualResetEvent(false);

        public  static event ServMonitorAlarmHandlingEvent AlarmEvent;
        public static event ServMonitorOptHandlingEvent OptEvent;

        /// <summary>
        /// 获取服务操作信息
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        /// <param name="ServName">服务名</param>
        /// <param name="ServPath">服务执行路径</param>
        private static void GetServOptInfo(string ID,string TYPE,ref string ServName, ref string ServPath)
        {
            try
            {
                if ((Common.DtServInfo.Columns["ID"] == null) ||
                    (Common.DtServInfo.Columns["TYPE"] == null) ||
                    (Common.DtServInfo.Columns["ServName"] == null) ||
                    (Common.DtServInfo.Columns["ServPath"] == null))
                {
                    return;
                }
                for (int i = 0; i < Common.DtServInfo.Rows.Count; i++)
                {
                    if ((Common.DtServInfo.Rows[i]["ID"].ToString().Trim() == ID) && (Common.DtServInfo.Rows[i]["TYPE"].ToString().Trim() == TYPE))
                    {
                        ServName = Common.DtServInfo.Rows[i]["ServName"].ToString();
                        ServPath = Common.DtServInfo.Rows[i]["ServPath"].ToString();
                        break;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        public static void ServStart(string ID, string TYPE)
        {
            try
            {
                string ServName = "";
                string ServPath = "";
                GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
                ServMag.ServStart(ID, TYPE, ServName, ServPath);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        public static void ServStop(string ID, string TYPE)
        {
            try
            {
                string ServName = "";
                string ServPath = "";
                Console.WriteLine("ID="+ID+"  "+"TYPE="+TYPE);
                GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
                ServMag.ServStop(ID, TYPE, ServName);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="ID">服务器ID编号</param>
        /// <param name="TYPE">服务器类型</param>
        public static void ServRestart(string ID, string TYPE)
        {
            try
            {
                string ServName = "";
                string ServPath = "";
                GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
                ServMag.ServRestart(ID, TYPE, ServName, ServPath);
            }
            catch
            {
                return;
            }
        }



        ///// <summary>
        ///// 开启服务
        ///// </summary>
        ///// <param name="ID">服务器ID编号</param>
        ///// <param name="TYPE">服务器类型</param>
        ///// <param name="resqust">连接对像返回给谁</param>
        //public static void ServStart(string ID, string TYPE,StateObject resqust)
        //{
        //    try
        //    {
        //        string ServName = "";
        //        string ServPath = "";
        //        GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
        //        ServMag.ServStart(ID, TYPE, ServName, ServPath, resqust);
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        ///// <summary>
        ///// 停止服务
        ///// </summary>
        ///// <param name="ID">服务器ID编号</param>
        ///// <param name="TYPE">服务器类型</param>
        ///// <param name="resqust">连接对像返回给谁</param>        
        //public static void ServStop(string ID, string TYPE, StateObject resqust)
        //{
        //    try
        //    {
        //        string ServName = "";
        //        string ServPath = "";
        //        GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
        //        ServMag.ServStop(ID, TYPE, ServName, resqust);
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        ///// <summary>
        ///// 重启服务
        ///// </summary>
        ///// <param name="ID">服务器ID编号</param>
        ///// <param name="TYPE">服务器类型</param>
        ///// <param name="resqust">连接对像返回给谁</param>
        //public static void ServRestart(string ID, string TYPE, StateObject resqust)
        //{
        //    try
        //    {
        //        string ServName = "";
        //        string ServPath = "";
        //        GetServOptInfo(ID, TYPE, ref ServName, ref ServPath);
        //        ServMag.ServRestart(ID, TYPE, ServName, ServPath, resqust);
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}




        public static void MonitorAlarmEvent(ServInfoState sender)
        {
            try
            {
                if (AlarmEvent!=null)
                {
                    AlarmEvent(sender);
                }

            }
            catch
            {
                return;
            }

        }


        public static void MonitorOptEvent(ServOptState sender)
        {
            try
            {
                if (OptEvent != null)
                {
                    OptEvent(sender);
                }

            }
            catch
            {
                return;
            }

        }


        /// <summary>
        /// 组件启动入口
        /// </summary>
        public void Start()
        {

            try
            {

                try
                {
                   RobotByServLoad();

                }
                catch
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


        /// <summary>
        /// 组件启动入口
        /// </summary>
        /// <param name="OSType">操作系统类型</param>
        public void Start(string OSType)
        {

            try
            {
                _allDone.Reset();
                try
                {

                    CommClass.OStype = Convert.ToInt32(OSType.ToString());
                }
                catch
                {
                    CommClass.OStype = 0;
                }

                try
                {

                    //CommClass.OStype = 1;

                    #region 系统配置文件初始化
                    CommClass.SetPubPath();
                    CommClass.CreateServInfo();
                    CommClass.CreateConfig();
                    CommClass.ReadXML(CommClass.ConfigFilePath, ref Common.DtServInfo);
                    #endregion
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RobotByServListStart), null);
                    RobotByServLoad();

                    _allDone.WaitOne();
                }
                catch
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// 组件启动入口
        /// </summary>
        /// <param name="OSType">操作系统类型</param>
        /// <param name="IsAutoStart">是否启用自动启动</param>
        public  void Start(string OSType,string IsAutoStart)
        {

            try
            {
                _allDone.Reset();
                try
                {

                    CommClass.OStype = Convert.ToInt32(OSType.ToString());
                }
                catch
                {
                    CommClass.OStype = 0;
                }

                try
                {

                    //CommClass.OStype = 1;

                    #region 系统配置文件初始化
                    CommClass.SetPubPath();
                    CommClass.CreateServInfo();
                    CommClass.CreateConfig();
                    CommClass.ReadXML(CommClass.ConfigFilePath, ref Common.DtServInfo);
                    #endregion
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RobotByServListStart), null);
                    if (IsAutoStart!="0")
                    {
                        RobotByServLoad();
                    }


                    _allDone.WaitOne();
                }
                catch
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        #region 智能守护

        /// <summary>
        /// 守护服务加载
        /// </summary>
        private static void RobotByServLoad()
        {

            if ((Common.DtServInfo.Columns["ID"] == null) ||
                (Common.DtServInfo.Columns["TYPE"] == null) ||
                (Common.DtServInfo.Columns["ServName"] == null) ||
                (Common.DtServInfo.Columns["ServPath"] == null)||
                (Common.DtServInfo.Columns["APPTYPE"]==null))
            {
                return;
            }
            for (int i = 0; i < Common.DtServInfo.Rows.Count; i++)
            {
                RobotByServMonitor RbSm = new RobotByServMonitor();
                RbSm.Interval = 3000;
                RbSm.ServName = Common.DtServInfo.Rows[i]["ServName"].ToString();
                RbSm.ServPath = Common.DtServInfo.Rows[i]["ServPath"].ToString();
                RbSm.APPTYPE = Common.DtServInfo.Rows[i]["APPTYPE"].ToString();
                
                if ( Common.DtServInfo.Rows[i]["ID"]!=null)
                {
                    RbSm.ID = Common.DtServInfo.Rows[i]["ID"].ToString(); 
                }
                if (Common.DtServInfo.Rows[i]["TYPE"] != null)
                {
                    RbSm.TYPE = Common.DtServInfo.Rows[i]["TYPE"].ToString();
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(RobotByServStart), RbSm);

            }
        }



        /// <summary>
        /// 守护服务启动
        /// </summary>
        /// <param name="obj">启动对像</param>
        private static void RobotByServStart(object obj)
        {
            RobotByServMonitor RbSm = (RobotByServMonitor)obj;
            RbSm.Start();
        }


        private static void RobotByServListStart(object obj)
        {
            //RobotByServList.AddServStartList();
            //RobotByServList RbSl = new RobotByServList();
            //RbSl.AddServStartList();
            //RbSl.Interval = 20000;
            //RbSl.Start();

            RobotByServList.AddServStartList();
            RobotByServList.Interval = 3000;
            RobotByServList.Start();



        }

        #endregion
    }
}
