using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
namespace ServMonitor
{
    /// <summary>
    /// 公共类，存放处理函数
    /// </summary>
    class CommClass
    {
        #region 获取操作系统
        /// <summary>
        /// 获取操作系统
        /// </summary>
        /// <param name="OS_Type">操作系统类型</param>
        /// <returns>true:Windows;flase:Linux</returns>
        public static bool GetOS(byte OS_Type)
        {
            int OsType = 0;
            OsType = Convert.ToInt32(OS_Type);
            if ((OsType % 2) == 0)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 读XML文件内容
        /// <summary>
        /// 读XML文件内容
        /// </summary>
        /// <param name="FilePath">XML文件路径</param>
        /// <param name="dt">XML内容记录集</param>
        public static void ReadXML(string FilePath,ref DataTable dt)
        {
            if (FilePath=="")
            {
                return;
            }
            try
            {
                FileInfo file = new FileInfo(FilePath);
                if (file.Exists)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(FilePath);
                    dt = ds.Tables[0];
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region 全局变量

        /// <summary>
        /// 配置文件路径
        /// </summary>        
        public static string ConfigFilePath = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/ServInfo.xml";

        public static string DataSourcePath = "";

        /// <summary>
        /// 服务器类型值{0:Linux；1:Windows}默认为Linux
        /// </summary>
        public static int OStype = 0;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBConnect = "";

        /// <summary>
        /// 上级KDS服务IP地址
        /// </summary>
        public static string UpKdsIp = "";
        
        /// <summary>
        /// 上组KDS服务端口号
        /// </summary>
        public static int UpKdsPort=0;

        /// <summary>
        /// 是否连接KMS,纳入KMS服务管理,1连接，0不连接
        /// </summary>
        public static int IsConKms = 0;

        /// <summary>
        /// 是否连接上级KDS服务,实现远程数据同步功能,1连接，0不连接
        /// </summary>
        public static int IsConUpkds = 0;

        /// <summary>
        /// KMS服务IP地址
        /// </summary>
        public static string KmsIp = "";
        /// <summary>
        /// KMS服务端口号
        /// </summary>
        public static int KmsPort = 0;

        /// <summary>
        /// MIC文件路径
        /// </summary>
        public static string PathMIC = "";

        /// <summary>
        /// 曲库文件版本号
        /// </summary>
        public static string SongDataVer = "";

        /// <summary>
        /// 实时获取MIC数据状态{true：开启；false：关闭}
        /// </summary>
        public static bool RealGetMICState = false;
        #endregion

        #region 设置全局变量路径
        /// <summary>
        /// 设置全局变量路径
        /// </summary>
        public static void SetPubPath()
        {
            if (OStype!=0)
            {
                ConfigFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\ServInfo.xml";
            }

            if (OStype != 0)
            {
                PathMIC = "./MIC/";
            }
            else
            {
                PathMIC = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\MIC\\";
            }

        }
        #endregion
        /// <summary>
        /// 程序入口函数{0:Linux运行；1:WIN运行}
        /// </summary>
        /// <param name="OStype"></param>
        public static string getuploadpath()
        {
            if (OStype != 0)
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\upload\\";
            }
            else
            {
                return "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/upload/";
            }
        }
        #region 计算日期时间差
        /// <summary>
        /// 计算日期时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static  string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天"
                + ts.Hours.ToString() + "小时"
                + ts.Minutes.ToString() + "分钟"
                + ts.Seconds.ToString() + "秒"
                +ts.Milliseconds.ToString()+"毫秒";

            return dateDiff;
        }
        #endregion

        #region 获取数据库连接字符串
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns>数据连接字符串</returns>
        public static string GetDBConnectString()
        {
            string StrConn="";
            DataTable dt = new DataTable();
            ReadXML(ConfigFilePath, ref dt);
            if (dt.Rows.Count>0)
            {
                
                        StrConn = dt.Rows[0]["DBCONN"].ToString();                  
                
                
            }
            return StrConn;
        }
        #endregion

        #region 创建XML文件
        /// <summary>
        /// 创建XML文件
        /// </summary>
        /// <param name="strXMLPath"></param>
        /// <param name="dt"></param>
        public static void CreateXML(string strXMLPath, DataTable dt)
        {
            dt.WriteXml(strXMLPath);
        }
        #endregion


        private static DataTable dt = new DataTable("ServInfo");

        /// <summary>
        /// 创建监听服务列表
        /// </summary>
        public static void CreateServInfo()
        {
            try
            {
                DataColumn dc = null;
                dc = dt.Columns.Add("ServName", Type.GetType("System.String"));
                dc = dt.Columns.Add("ServPath", Type.GetType("System.String"));
                dc = dt.Columns.Add("ID", Type.GetType("System.String"));
                dc = dt.Columns.Add("TYPE", Type.GetType("System.String"));
                dc = dt.Columns.Add("APPTYPE", Type.GetType("System.String"));


            }
            catch (Exception ex)
            {
                ConsoleManage.Write(ErrorLevel.Serious,
                                     "IsSongServ>>CommonFunction>>CreateSingListTable>>",
                                     ex.Message);
            }
            finally
            {
                ////GC.Collect();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServName"></param>
        /// <param name="ServPath"></param>
        public static void AddToServInfo(string ServName,
                                         string ServPath,string ID,string TYPE
)
        {
            try
            {
                /// <summary>
                /// 添加信息至排麦列表_歌曲属性完整信息
                /// </summary>
                /// 
                DataRow newRow;
                newRow = dt.NewRow();
                newRow["ServName"] = ServName;
                newRow["ServPath"] = ServPath;
                newRow["ID"] = ID;
                newRow["TYPE"] = TYPE; 
                dt.Rows.Add(newRow);

            }
            catch (Exception ex)
            {
                ConsoleManage.Write(ErrorLevel.Serious,
                                     "IsSongServ>>CommonFunction>>AddToServInfo>>",
                                     ex.Message);
            }
            finally
            {
                ////GC.Collect();
            }
        }




        #region 创建ServInfo文件
        /// <summary>
        /// 创建ServInfo.xml文件
        /// </summary>
        public static void CreateConfig()
        {
            try
            {
                FileInfo file = new FileInfo(ConfigFilePath);
                if (file.Exists)
                {
                    return;
                }
                string KMSPath = "";//默认KMS路径
                string KISPath = "";//默认KIS路径
                string KNSPath = "";//默认KNS路径
                string KDSPath = "";//默认KDS路径
                if (OStype != 0)
                {
                     KMSPath = "D:\\KMS\\KMS.exe";//默认KMS路径
                     KISPath = "D:\\KIS\\KIS.exe";//默认KIS路径
                     KNSPath = "D:\\KNS\\KNS.exe";//默认KNS路径
                     KDSPath = "D:\\KDS\\KDS.exe";//默认KDS路径
                }
                else
                {
                    KMSPath = "/usr/KMS/KMS_Start.sh";//默认KMS路径
                    KISPath = "/usr/KIS/KIS_Start.sh";//默认KIS路径
                    KNSPath = "/usr/KNS/KNS_Start.sh";//默认KNS路径
                    KDSPath = "/usr/KDS/KDS_Start.sh";//默认KDS路径                
                }

                AddToServInfo("KMS", KMSPath,"1","1");
                AddToServInfo("KIS", KISPath,"1","2");
                AddToServInfo("KNS", KNSPath,"1","3");
                AddToServInfo("KDS", KDSPath,"1","4");

                CreateXML(ConfigFilePath, dt);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }
        #endregion

        #region 获取数据文件准确名称
        /// <summary>
        /// 获取数据文件准确名称
        /// </summary>
        /// <param name="DataFileName">数据文件模糊名称</param>
        /// <param name="TotalPage">总页数</param>
        /// <returns></returns>
        public static string GetDataSourceFile(string DataFileName, ref int TotalPage)
        {
            string RetFileName = "";
            DirectoryInfo selDir = new DirectoryInfo(DataSourcePath);
            FileInfo[] listFile;
            listFile = selDir.GetFiles(DataFileName, SearchOption.AllDirectories);
            string TmpFileName = "";
            foreach (FileInfo d in listFile)
            {
                RetFileName = DataSourcePath + d.Name.ToString();
                TmpFileName = d.Name.ToString().Replace(".", "&");

            }
            if (TmpFileName != "")
            {
                Regex r = new Regex("&");

                string[] sArray = r.Split(TmpFileName);
                try
                {
                    TotalPage = Convert.ToInt32(sArray[1]);
                }
                catch
                {
                    TotalPage = 0;
                }
            }
            return RetFileName;
        }
        #endregion


        #region XML数据创建至Byte数组
        /// <summary>
        /// 二维表字段结构体
        /// </summary>
        public struct TTable
        {
            public string FieldName;
            public string FieldValue;
        }

        /// <summary>
        /// 根据传入表列表获取字段数量
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>字段数据</returns>
        private static int GetFieldCount(List<TTable> Table)
        {
            if (Table.Count > 0)
            {
                string FristField = "";//首字段名称
                FristField = Table[0].FieldName;//首字段赋值

                for (int i = 0; i < Table.Count; i++)
                {
                    if ((FristField == Table[i].FieldName) && (i > 0))
                    {
                        return i;
                    }
                }
            }
            else
            {
                return 0;
            }
            return Table.Count;

        }

        /// <summary>
        /// 根据DataTable创建XML字节数组
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="ByteResult">返回字节内容</param>
        public static void DataTableToByteArry(DataTable dt, ref byte[] ByteResult)
        {
            DataSet ds = new DataSet("ROOT");
            dt.TableName = "ITEM";
            ds.Tables.Add(dt);

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                {
                    ds.Tables[0].WriteXml(xmlWriter);
                    ByteResult = ms.ToArray();
                }
            }

        }

        /// <summary>
        /// 根据LIST表创建XML字节数组
        /// </summary>
        /// <param name="Table">TTable表列表</param>
        /// <param name="ByteResult">返回字节内容</param>
        /// <param name="Root">根标识</param>
        /// <param name="item">条目标识</param>
        public static void TableToByteArry(List<TTable> Table, ref byte[] ByteResult, string Root, string item)
        {
            DataSet ds = new DataSet(Root);
            DataTable tblDatas = new DataTable(item);
            DataColumn dc = null;
            DataRow newRow = null;
            newRow = tblDatas.NewRow();
            int FieldCount = 0;
            FieldCount = GetFieldCount(Table);

            for (int i = 0; i < FieldCount; i++)
            {
                dc = tblDatas.Columns.Add(Table[i].FieldName, Type.GetType("System.String"));
            }


            for (int i = 0; i < Table.Count; i++)
            {
                newRow[Table[i].FieldName] = Table[i].FieldValue;

                if ((((i + 1) % FieldCount) == 0))
                {
                    tblDatas.Rows.Add(newRow);
                    newRow = tblDatas.NewRow();
                }
            }

            ds.Tables.Add(tblDatas);
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                {
                    ds.Tables[0].WriteXml(xmlWriter);
                    ByteResult = ms.ToArray();
                }
            }
        }


        /// <summary>
        /// 根据LIST表创建XML字节数组
        /// </summary>
        /// <param name="Table">TTable表列表</param>
        /// <param name="ByteResult">返回字节内容</param>
        public static void TableToByteArry(List<TTable> Table, ref byte[] ByteResult)
        {
            DataSet ds = new DataSet("ROOT");
            DataTable tblDatas = new DataTable("ITEM");
            DataColumn dc = null;
            DataRow newRow = null;
            newRow = tblDatas.NewRow();
            int FieldCount = 0;
            FieldCount = GetFieldCount(Table);

            for (int i = 0; i < FieldCount; i++)
            {
                dc = tblDatas.Columns.Add(Table[i].FieldName, Type.GetType("System.String"));
            }


            for (int i = 0; i < Table.Count; i++)
            {
                newRow[Table[i].FieldName] = Table[i].FieldValue;

                if ((((i + 1) % FieldCount) == 0))
                {
                    tblDatas.Rows.Add(newRow);
                    newRow = tblDatas.NewRow();
                }
            }

            ds.Tables.Add(tblDatas);

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                {
                    ds.Tables[0].WriteXml(xmlWriter);
                    ByteResult = ms.ToArray();
                }
            }
        }

        #endregion

        #region 输入包体内容
        public static void OutputBody(DataTable dt)
        {
            try
            {
                string StrBody = "";
                int RowsCount = 0;
                int ColumnsCount = 0;

                ColumnsCount = dt.Columns.Count;
                RowsCount = dt.Rows.Count;
                if (RowsCount > 0)
                {
                    for (int i = 0; i < RowsCount; i++)
                    {
                        for (int j = 0; j < ColumnsCount; j++)
                        {

                            StrBody += dt.Columns[j].ToString() + ":" + dt.Rows[i][j].ToString() + "\n";
                        }

                    }
                }
                ConsoleManage.Write(ErrorLevel.Serious, "KNS>>CommClass>>OutputBody", StrBody);

            }
            catch (Exception ex)
            {
                ConsoleManage.Write(ErrorLevel.Serious, "KNS>>CommClass>>OutputBody", ex.Message);

            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

        /// <summary>
        /// 丢失MIC列表
        /// </summary>
        public static Dictionary<string, string> LoseMICList = new Dictionary<string, string>();

    }
}
