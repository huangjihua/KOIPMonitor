using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
namespace Commonality
{
    /// <summary>
    /// 公共类，存放处理函数
    /// </summary>
    public class CommClass
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
                    OutputBody(dt);
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
        public static string ConfigFilePath = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/server.xml";

        public static string DataSourcePath = "";

        /// <summary>
        /// 服务器类型值{0:Linux；1:Windows}默认为Linux
        /// </summary>
        public static int OStype = 0;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBConnect = "";

        #endregion

        #region 设置全局变量路径
        /// <summary>
        /// 设置全局变量路径
        /// </summary>
        public static void SetPubPath()
        {
            if (OStype!=0)
            {
                ConfigFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\server.xml";
            }
        }
        #endregion

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

        #region 创建Server.xml文件
        /// <summary>
        /// 创建Server.xml文件
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
                DataTable dt = new DataTable();
                string[] aryField = { "kmsip", 
                                  "kmsport", 
                                  "ip", 
                                  "port", 
                                  "MICPATH", 
                                  "servermode", 
                                  "DBCONN", 
                                  "DATATYPE", 
                                  "DATASOURCEPATH"
                                };
                string[] aryVale = { "192.168.8.43", 
                                  "20001", 
                                  "192.168.8.90", 
                                  "45001" ,
                                  "E:\\MyWork\\KOIP相关\\点歌系统\\Code\\MIC\\",
                                  "kds",
                                  "server=192.168.8.156;uid=newsa;pwd=ising;database=DB_KTVBMS;",
                                  "1",//1=MySQL;0=SQL SERVER
                                  "E:\\MyWork\\KOIP相关\\点歌系统\\Code\\DataSource\\"
                               };
                for (int i = 0; i < aryField.Length; i++)
                {
                    dt.Columns.Add(new DataColumn(aryField[i], typeof(string)));//设置DataTable的ColumnName，根据不同的字段类型需要设计不同的typeof,最好分开写不要用for循环。
                }
                DataRow dr = dt.NewRow();
                for (int i = 0; i < aryField.Length; i++)
                {
                    dr[aryField[i]] = aryVale[i];//设置DataTable的行内容
                }
                dt.TableName = "Server";
                dt.Rows.Add(dr);
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
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KNS>>CommClass>>OutputBody", StrBody);

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KNS>>CommClass>>OutputBody", ex.Message);

            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

    }
}
