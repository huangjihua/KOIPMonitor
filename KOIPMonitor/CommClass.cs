using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Kernel;
namespace KOIPMonitor
{
    /// <summary>
    /// 公共类，存放处理函数
    /// </summary>
    class CommClass
    {

        #region 客户端连接服务管理
        /// <summary>
        /// 客户端连接服务列表
        /// </summary>
        public static Dictionary<string, Kernel.StateObject> ClientConnList = new Dictionary<string, Kernel.StateObject>();

        /// <summary>
        /// 移除客户端连接列表
        /// </summary>
        /// <param name="Request">连接对像</param>
        public static void RemoveClientConnList(Kernel.StateObject Request)
        {
            try
            {
                foreach (KeyValuePair<string, Kernel.StateObject> a in ClientConnList)
                {
                    if (a.Value != null)
                    {
                        if (a.Value.ConnectionSession == Request.ConnectionSession)
                        {
                            ClientConnList.Remove(a.Key);
                            break;
                        }
                    }
                }
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
"KOIPMonitor>>CommClass>>RevDevObjList>>", "User is Down!~~");    
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>RemoveClientConnList>>",
                     ex.Message);
                return;
            }

        }
        /// <summary>
        /// 添加客户端用户列表
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Request">连接对像</param>
        public static void AddClientConnList(string UserId,Kernel.StateObject Request)
        {
            try
            {
                if (!ClientConnList.ContainsKey(UserId))
                {
                    ClientConnList.Add(UserId, Request);
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>AddClientConnList>>",
                     ex.Message);
                return;
            }

        }

        /// <summary>
        /// 获取用户是否登录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns>0:未登录，-1:已登录</returns>
        public static int GetUserIsLogin(string UserID)
        {
            int ret = 0;
            try
            {
                foreach (KeyValuePair<string, Kernel.StateObject> a in ClientConnList)
                {
                    if (a.Key == UserID)
                    {
                        ret = -1;
                        break;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>RemoveClientConnList>>",
                     ex.Message);
                return ret;
            }


        }

        #endregion

        public  struct DevObj
        {
            //public string DevID;
            public Kernel.StateObject request;
            public int IsLocal;
        }

        #region 设备对像列表管理
        /// <summary>
        /// 设备对像列表
        /// </summary>
        public static Dictionary<string, DevObj> DevObjList = new Dictionary<string, DevObj>();

        //public static List<DevObj> DevObjList = new List<DevObj>();

        /// <summary>
        /// 移除设备连接列表
        /// </summary>
        /// <param name="DevID">设备编号</param>
        public static void RevDevObjList(string DevID)
        {
            try
            {
                foreach (KeyValuePair<string, DevObj> a in DevObjList)
                {
                    if (a.Key == DevID)
                    {
                        //a.Value.States.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                        DevObjList.Remove(a.Key);
                        break;
                    }
                }
                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if (DevObjList[i].DevID==DevID)
                //    {
                //        DevObjList.RemoveAt(i);
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>RevDevObjList>>",
                     ex.Message);
                return;
            }
        }

        /// <summary>
        /// 获取下线类型
        /// </summary>
        /// <param name="Request">连接对像</param>
        /// <returns>0:未知;1:用户;2:下级服务器</returns>
        public static int GetDownLineType(StateObject Request)
        {

            int ret = 0;
            try
            {

                foreach (KeyValuePair<string, DevObj> a in DevObjList)
                {
                    if (a.Value.request != null)
                    {
                        if (a.Value.request.ConnectionSession == Request.ConnectionSession)
                        {
                            ret = 2;
                            break;
                        }
                    }

                }

                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if (DevObjList[i].request!=null)
                //    {
                //        if (DevObjList[i].request.ConnectionSession==Request.ConnectionSession)
                //        {
                //            ret = 2;
                //            break;
                //        }
                //    }
                //}

                foreach (KeyValuePair<string, Kernel.StateObject> a in ClientConnList)
                {
                    if (a.Value != null)
                    {
                        if (a.Value.ConnectionSession == Request.ConnectionSession)
                        {
                            ret = 1;
                            break;
                        }
                    }
                }


                return ret;
            }
            catch(Exception ex) 
            {
                                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
"KOIPMonitor>>CommClass>>GetDownLineType>>", ex.Message);  
                return ret;
            }

        }


        /// <summary>
        /// 移除设备连接列表
        /// </summary>
        /// <param name="DevID">设备编号</param>
        public static void RevDevObjList(StateObject Request)
        {
            try
            {
                string[] DevObjKey = new string[DevObjList.Count];
                int i=0;
                foreach (KeyValuePair<string, DevObj> a in DevObjList)
                {
                    
                    if (a.Value.request != null)
                    {
                        if (a.Value.request.ConnectionSession == Request.ConnectionSession)
                        {
                            //a.Value.States.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                            //DevObjList.Remove(a.Key);
                            //DevObj _DevObj = new DevObj();
                            //_DevObj.IsLocal = a.Value.IsLocal;
                            //_DevObj.request = null;
                            //DevObjList[a.Key] = _DevObj;
                            DevObjKey[i]=a.Key;
                            ServMonitor_AlarmHandlingEvent.UpdateServState(a.Key, "3");
                            i++;
                        }
                    }
                }
                for (int j = 0; j < DevObjKey.Length; j++)
                {
                    //if (String.IsNullOrEmpty( DevObjKey[j]))
                    //{
                        DevObj _DevObj = new DevObj();
                        _DevObj.IsLocal = -1;
                        _DevObj.request = null;
                        DevObjList[DevObjKey[j]] = _DevObj;
                    //}

                }
                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if (DevObjList[i].request!=null)
                //    {
                //        if (DevObjList[i].request.ConnectionSession==Request.ConnectionSession)
                //        {
                //            //DevObjList[i].request = null;
                //            ServMonitor_AlarmHandlingEvent.UpdateServState(DevObjList[i].DevID, "3");
                //            DevObjList.RemoveAt(i);
     
                //        }
                //    }
                //}

                ServMonitor_AlarmHandlingEvent.NoticeDevState();

                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                "KOIPMonitor>>CommClass>>RevDevObjList>>", "Node Server is Down!~~");       

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>RevDevObjList>>",
                     ex.Message);
                return;
            }
        }

        /// <summary>
        /// 查看设备连接对像是否存在
        /// </summary>
        /// <param name="DevID">设备编号</param>
        /// <returns>false:不存在;true:存在</returns>
        private static bool IsDevObjList(string DevID)
        {
            bool ret=false;
            //for (int i = 0; i < DevObjList.Count; i++)
            //{
            //    if ((DevObjList[i].DevID == DevID))
            //    {
            //        ret = true;
            //        break;
            //    }

            //}
            return ret;
        }

        /// <summary>
        /// 添加设备对像
        /// </summary>
        /// <param name="DevID"></param>
        /// <param name="_DevObj"></param>
        public static void AddDevObjList(string DevID, DevObj _DevObj)
        {
            try
            {
                if (!DevObjList.ContainsKey(DevID))
                {
                    DevObjList.Add(DevID, _DevObj);
                }
                else
                {
                    //KoIp.BusinessDAL.CommonList.KnsUserList[usersession.UID] = usersession
                    DevObjList[DevID] = _DevObj;
                }

                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if ((DevObjList[i].DevID == DevID))
                //    {
                //        //DevObjList.Add(DevID, _DevObj);
                //        DevObjList.RemoveAt(i);
                //        DevObjList.Add(_DevObj);
                //    }

                //}

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>AddDevObjList>>",
                     ex.Message);
                return;
            }
        }

        /// <summary>
        /// 添加设备对像
        /// </summary>
        /// <param name="_DevObj"></param>
        public static void AddDevObjList(DevObj _DevObj)
        {
            try
            {
                //if (!DevObjList.ContainsKey(DevID))
                //{
                //    DevObjList.Add(DevID, _DevObj);
                //}
                //else
                //{
                //    //KoIp.BusinessDAL.CommonList.KnsUserList[usersession.UID] = usersession
                //    DevObjList[DevID] = _DevObj;
                //}


                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if ((DevObjList[i].DevID == _DevObj.DevID))
                //    {
                //        //DevObjList.Add(DevID, _DevObj);
                //        DevObjList.RemoveAt(i);

                //    }

                //}
                //DevObjList.Add(_DevObj);

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>AddDevObjList>>",
                     ex.Message);
                return;
            }
        }

        /// <summary>
        /// 获取服务器对像管理连接地址
        /// </summary>
        /// <param name="DevID">设备编号</param>
        /// <param name="Request">连接对像</param>
        public static void GetDevMagObj(string DevID, ref DevObj _DevObj)
        {
            try
            {
                foreach (KeyValuePair<string, DevObj> a in DevObjList)
                {
                    if (a.Key.ToString() == DevID.ToString())
                    {
                        //a.Value.States.workSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                        _DevObj = DevObjList[DevID];
                        return;
                    }
                }
                //for (int i = 0; i < DevObjList.Count; i++)
                //{
                //    if (DevObjList[i].DevID==DevID)
                //    {
                //        _DevObj = DevObjList[i];
                //        return;
                //    }
                //}
                if (CommClass.ISCONUP=="0")
                {
                    _DevObj.IsLocal = -1;
                    _DevObj.request = null;
                }


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                     "KOIPMonitor>>CommClass>>GetDevMagObj>>",
                     ex.Message);
                return;
            }
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
                    //OutputBody(dt);
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


        public static string ServListPath = "/" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/ServList.xml";

        /// <summary>
        /// 服务器类型值{0:Linux；1:Windows}默认为Linux
        /// </summary>
        public static int OStype = 0;


        /// <summary>
        /// 本地服务器ID
        /// </summary>
        public static string ID = "";

        /// <summary>
        /// 本地服务IP地址
        /// </summary>
        public static string IP = "";

        /// <summary>
        /// 本地服务端口号
        /// </summary>
        public static int PORT = 0;
        
        /// <summary>
        /// 上级服务ID
        /// </summary>
        public static string UPID = "";

        /// <summary>
        /// 上级服务IP地址
        /// </summary>
        public static string UPIP = "";

        /// <summary>
        /// 上级服务端口号
        /// </summary>
        public static int UPPORT = 0;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBCONN = "";

        /// <summary>
        /// 是否连接上级服务{0:不连接;1:连接}
        /// </summary>
        public static string ISCONUP="";

        /// <summary>
        /// 是否启用自动启动
        /// </summary>
        public static string ISAUTOSTART = "1";

        /// <summary>
        /// 服务器列表信息
        /// </summary>
        public static DataTable DtServList = new DataTable();

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
                ServListPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\ServList.xml";

            }

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
        /// 创建服务列表
        /// </summary>
        public static void CreateServListTable()
        {
            try
            {
                DataColumn dc = null;
                dc = dt.Columns.Add("ID", Type.GetType("System.String"));
                dc = dt.Columns.Add("TYPE", Type.GetType("System.String"));
                dc = dt.Columns.Add("IP", Type.GetType("System.String"));
                dc = dt.Columns.Add("PORT", Type.GetType("System.String"));
                dc = dt.Columns.Add("NAME", Type.GetType("System.String"));
                dc = dt.Columns.Add("UPID", Type.GetType("System.String"));
                dc = dt.Columns.Add("STATE", Type.GetType("System.String"));


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                                     "KOIPMonitor>>CommClass>>CreateServListTable>>",
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
        /// <param name="StrID"></param>
        /// <param name="NAME"></param>
        /// <param name="TYPE"></param>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="UPID"></param>
        /// <param name="STATE"></param>
        public static void AddToServList(string ID,
                                         string NAME,
                                         string TYPE,string IP,string PORT,string UPID,string STATE)

        {
            try
            {

                DataRow newRow;
                newRow = dt.NewRow();
                newRow["ID"] = ID;
                newRow["NAME"] = NAME;
                newRow["TYPE"] = TYPE;
                newRow["IP"] = IP;
                newRow["PORT"] = PORT;
                newRow["UPID"] = UPID;
                if (String.IsNullOrEmpty(STATE))
                {
                    STATE = "";
                    newRow["STATE"] = STATE;
                }


                dt.Rows.Add(newRow);

            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious,
                                     "CommClass>>AddToServList>>",
                                     ex.Message);
            }
            finally
            {
                ////GC.Collect();
            }
        }




        #region 创建ServList文件
        /// <summary>
        /// 创建ServList.xml文件
        /// </summary>
        public static void CreateServList()
        {
            try
            {
                FileInfo file = new FileInfo(ServListPath);
                if (file.Exists)
                {
                    return;
                }

                AddToServList("1", "KMS", "1", "192.168.8.90", "20001", "0", null);
                AddToServList("1", "KIS", "2", "192.168.8.90", "30001", "0", null);
                AddToServList("1", "KNS", "3", "192.168.8.90", "40001", "0", null);
                AddToServList("1", "KDS", "4", "192.168.8.90", "45001", "0", null);

                CreateXML(ServListPath, dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

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
                string[] aryField = {
                                  "ID",
                                  "IP", 
                                  "PORT", 
                                  "UPID",
                                  "UPIP",
                                  "UPPORT",
                                  "DBCONN",
                                  "ISCONUP",
                                  "ISAUTOSTART"
                                };
                string[] aryVale = {
                                  "1",
                                  "192.168.8.90", 
                                  "45001", 
                                   "2",
                                  "192.168.8.90", 
                                  "20001", 
                                  "Server=192.168.8.119;Database=db_newktvbms;Uid=root;Pwd=ising99;",
                                   "1",
                                   "1"
                                   };

                //string[] aryField = {
                //                  "IsConKms",
                //                  "kmsip", 
                //                  "kmsport", 
                //                  "IsConUpkds",
                //                  "upkdsip",
                //                  "upkdsport",
                //                  "ip", 
                //                  "port", 
                //                  "MICPATH", 
                //                  "servermode", 
                //                  "DBCONN", 
                //                  /*"DATATYPE",*/ 
                //                  /*"DATASOURCEPATH"*/
                //                };
                //string[] aryVale = {
                //                  "0",
                //                  "192.168.8.90", 
                //                  "20001", 
                //                   "1",
                //                  "192.168.8.90", 
                //                  "20001", 
                //                  "192.168.8.90", 
                //                  "45001" ,
                //                  MicPath,
                //                  "kds",
                //                  "Server=192.168.8.119;Database=db_newktvbms;Uid=root;Pwd=ising99;",
                //                  /*"1",//1=MySQL;0=SQL SERVER*/
                //                  /*"E:\\MyWork\\KOIP相关\\点歌系统\\Code\\DataSource\\"*/
                //               };
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
