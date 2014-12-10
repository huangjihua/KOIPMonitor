using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Kernel;
using System.IO;
using System.Threading;
using MySqlHeader;
using MySql.Data.MySqlClient;

namespace KOIPMonitor
{
    class CommonFunction
    {
        public CommonFunction() { }
        ~CommonFunction() { }

        #region 发送数据分页
        /// <summary>
        /// 发送数据分页
        /// </summary>
        /// <param name="_obj"></param>
        public static void SendDatasPage(object _obj)
        {
            try
            {
                Hashtable _hashtable_Package = (Hashtable)_obj;
                StateObject state = (StateObject)_hashtable_Package["1"];
                short cmd1 = 0;
                cmd1 = (short)_hashtable_Package["6"];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        //byte[] GzipResultByte = null;   //Gzip压缩结果
                        //Tools.GzipCompress(ref ResultByte, ref GzipResultByte);

                        int sendLength = 0;
                        //byte[] GzipResultByte = null;
                        byte[] ResultByte = null;
                        ResultByte = (byte[])_hashtable_Package["2"];
                        if (ResultByte!=null)
                        {
                           sendLength= ResultByte.Length;
                           //ResultByte = System.Text.Encoding.UTF8.GetBytes(StrSend);
                           //Gzip压缩结果
                           //Tools.GzipCompress(ref ResultByte, ref GzipResultByte);
                           //sendLength = GzipResultByte.Length;
                        }

                        int TotalPage = 0;
                        try
                        {
                            TotalPage = Convert.ToInt32(_hashtable_Package["3"].ToString().Trim());
                        }
                        catch
                        {
                            TotalPage = 0;
                        }
                        int Currentpage = 0;
                        try
                        {
                            Currentpage = Convert.ToInt32(_hashtable_Package["4"].ToString().Trim());//总记录数
                        }
                        catch
                        {
                            Currentpage = 0;
                        }

                        int TotalRecords = 0;
                        try
                        {
                            TotalRecords = Convert.ToInt32(_hashtable_Package["5"].ToString().Trim());//总记录数

                        }
                        catch
                        {
                            TotalRecords = 0;
                        }
                        byte ver = 1;
                        //short cmd1 = state.cmd1;
                        short cmd2 = state.cmd2;// (short)_hashtable_Package["2"]; 
                        int packagelength = sendLength;// +8;
                        bw.Write(ver);
                        bw.Write(cmd1);
                        bw.Write(cmd2);
                        if (sendLength != 0)
                        {
                            bw.Write(packagelength + 12);
                            bw.Write(TotalPage);
                            bw.Write(Currentpage);
                            bw.Write(TotalRecords);
                            bw.Write(ResultByte);
                        }
                        else
                        {
                            bw.Write(packagelength);
                        }
                        bw.Flush();
                        state.Send(ms.ToArray());
                        if (!string.IsNullOrEmpty(state.receiveFileTemporarily))
                            ThreadPool.QueueUserWorkItem(new WaitCallback(DiskIO.Del), state.receiveFileTemporarily);
                    }
                }
                _hashtable_Package.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        #endregion

        #region 发送数据

        public static void SendDatas(object _obj)
        {
            Hashtable _hashtable_Package = (Hashtable)_obj;

            try
            {
                StateObject state = (StateObject)_hashtable_Package["1"];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        byte[] ResultByte = null;
                        ResultByte = (byte[])_hashtable_Package["4"];
                        //byte[] GzipResultByte = null;   //Gzip压缩结果
                        //Tools.GzipCompress(ref ResultByte, ref GzipResultByte);
                        byte ver = 1;

                        short cmd1 = (short)_hashtable_Package["2"];
                        short cmd2 = (short)_hashtable_Package["3"];

                        int packagelength = 0;
                        //if (GzipResultByte != null)
                        //{
                        //    packagelength = GzipResultByte.Length;
                        //}
                        if (ResultByte != null)
                        {
                            packagelength = ResultByte.Length;
                        }
                        bw.Write(ver);
                        bw.Write(cmd1);
                        bw.Write(cmd2);
                        bw.Write(packagelength);
                        if (packagelength > 0)
                        {
                            bw.Write(/*GzipResultByte*/ResultByte);
                        }
                        bw.Flush();

                        if (state != null)
                        {
                            if (state.workSocket.Connected)
                            {
                                state.Send(ms.ToArray());
                                if (cmd1 != CommCmd.Handshake)
                                {
                                    Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "SendDatas", "CMD=" + cmd1.ToString() + " " + "BACKCODE=" + cmd2.ToString() + ">>IPaddress:" + state.wanIP.ToString() + ">>Port:" + state.wanPort.ToString());
                                }
                            }
                            else
                            {
                                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "SendDatas", "state.workSocket is not Connected");

                            }

                        }
                        else
                        {
                            Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "SendDatas", "StateObject is null");

                        }


                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                _hashtable_Package.Clear();
                //if (!string.IsNullOrEmpty(state.receiveFileTemporarily))
                //    DiskIO.Del(state.receiveFileTemporarily);
                ////GC.Collect();
            }
        }

        #endregion

        #region TCP客户端发送数据
        /// <summary>
        /// TCP客户端发送数据
        /// </summary>
        /// <param name="_obj"></param>
        public static void TcpClientSend(object _obj)
        {
            Hashtable _hashtable_Package = (Hashtable)_obj;


            try
            {
                Kernel.AsynTCPClient tcpClient = (Kernel.AsynTCPClient)_hashtable_Package["1"];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        byte[] ResultByte = null;
                        ResultByte=(byte[])_hashtable_Package["4"];
                        byte[] GzipResultByte = null;   //Gzip压缩结果
                        Tools.GzipCompress(ref ResultByte, ref GzipResultByte);
                        byte ver = 1;

                        short cmd1 = (short)_hashtable_Package["2"];
                        short cmd2 = (short)_hashtable_Package["3"];

                        int packagelength = 0;
                        if (GzipResultByte != null)
                        {
                            packagelength = GzipResultByte.Length;
                        }
                        bw.Write(ver);
                        bw.Write(cmd1);
                        bw.Write(cmd2);
                        bw.Write(packagelength);

                        if (packagelength > 0)
                        {
                            bw.Write(GzipResultByte);
                        }
                        bw.Flush();

                        if (tcpClient.Connected)
                        {
                            tcpClient.BeginSend(ms.ToArray());
                            if (cmd1 != CommCmd.Handshake)
                            {
                                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "Client Send Data Success", "CMD=" + cmd1.ToString() + " " + "BACKCODE=" + cmd2.ToString());
                            }
                        }
                        else
                        {
                            Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "CommonFunction>>TcpClientSend>>", "TCP Client Disconnect");

                        }

                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                _hashtable_Package.Clear();
                ////GC.Collect();
            }
            
        }

        /// <summary>
        /// TCP客户端发送数据Gzip不压缩
        /// </summary>
        /// <param name="_obj"></param>
        public static void UnGzipTcpClientSend(object _obj)
        {
            Hashtable _hashtable_Package = (Hashtable)_obj;

            try
            {
                Kernel.AsynTCPClient tcpClient = (Kernel.AsynTCPClient)_hashtable_Package["1"];

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        byte[] ResultByte = null;
                        ResultByte = (byte[])_hashtable_Package["4"];
                        byte ver = 1;

                        short cmd1 = (short)_hashtable_Package["2"];
                        short cmd2 = (short)_hashtable_Package["3"];

                        int packagelength = 0;
                        if (ResultByte != null)
                        {
                            packagelength = ResultByte.Length;
                        }
                        bw.Write(ver);
                        bw.Write(cmd1);
                        bw.Write(cmd2);
                        bw.Write(packagelength);

                        if (packagelength > 0)
                        {
                            bw.Write(ResultByte);
                        }
                        bw.Flush();

                        if (tcpClient.Connected)
                        {
                            tcpClient.BeginSend(ms.ToArray());
                            if (cmd1 != CommCmd.Handshake)
                            {
                                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "Client Send Data Success", "CMD=" + cmd1.ToString() + " " + "BACKCODE=" + cmd2.ToString());
                            }
                        }
                        else
                        {
                            Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "CommonFunction>>TcpClientSend>>", "TCP Client Disconnect");

                        }

                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                _hashtable_Package.Clear();
                ////GC.Collect();
            }

        }

        #endregion

        #region 发送握手
        /// <summary>
        /// 发送数据(握手)
        /// </summary>
        /// <param name="ResultByte">生成握手字节数组</param>
        public static void HandshakeByte(ref byte[] ResultByte)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    byte ver = 1;
                    short cmd1 = CommCmd.Handshake;
                    short cmd2 = ErrCommon.Success; //0;
                    int packagelength = 0;
                    bw.Write(ver);
                    bw.Write(cmd1);
                    bw.Write(cmd2);
                    bw.Write(packagelength);
                    bw.Flush();
                    ResultByte = ms.ToArray();

                }
            }

        }
        /// <summary>
        /// 发送数据(握手)
        /// </summary>
        /// <param name="state">发送对像</param>
        public static void SendHandshake(Kernel.StateObject state)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    byte ver = 1;
                    short cmd1 = CommCmd.Handshake;
                    short cmd2 = ErrCommon.Success; //0;
                    int packagelength = 0;
                    bw.Write(ver);
                    bw.Write(cmd1);
                    bw.Write(cmd2);
                    bw.Write(packagelength);
                    bw.Flush();
                    if (state.workSocket.Connected)
                    {
                        state.Send(ms.ToArray());
                        if (cmd1 != CommCmd.Handshake)
                        {
                            Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "SendDatas", "CMD=" + cmd1.ToString() + " " + "BACKCODE=" + cmd2.ToString() + ">>IPaddress:" + state.wanIP.ToString() + ">>Port:" + state.wanPort.ToString());
                        }
                    }
                    else
                    {
                        Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "SendDatas", "state.workSocket is not Connected");

                    }

                }
            }

        }

        /// <summary>
        /// 发送数据(握手)
        /// </summary>
        /// <param name="state">发送对像</param>
        public static void SendHandshake(Kernel.AsynTCPClient state)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    byte ver = 1;
                    short cmd1 = CommCmd.Handshake;
                    short cmd2 = ErrCommon.Success; //0;
                    int packagelength = 0;
                    bw.Write(ver);
                    bw.Write(cmd1);
                    bw.Write(cmd2);
                    bw.Write(packagelength);
                    bw.Flush();
                    if (state.Connected)
                    {
                        state.BeginSend(ms.ToArray());
                        if (cmd1 != CommCmd.Handshake)
                        {
                            Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "Client Send Data Success", "CMD=" + cmd1.ToString() + " " + "BACKCODE=" + cmd2.ToString());
                        }
                    }
                    else
                    {
                        Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Response, "CommonFunction>>TcpClientSend>>", "TCP Client Disconnect");

                    }
                }
            }

        }
        #endregion

        #region 获取服务器列表
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        public static void GetServerList()
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {
                DataTable dt = new DataTable();
                if (CommClass.DtServList.Rows.Count>0)
                {
                    dt = CommClass.DtServList.Copy();
                    CommClass.DtServList = MCMD.ExtcuteDataTable(CommClass.DBCONN, CommandType.StoredProcedure, "spu_getserverlist", null, "ServList");
                    for (int i = 0; i < CommClass.DtServList.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (CommClass.DtServList.Rows[i]["ID"].ToString() == dt.Rows[j]["ID"].ToString())
                            {
                                CommClass.DtServList.Rows[i]["STATE"] = dt.Rows[j]["STATE"].ToString();
                            }
                        }
                    }


                    NoticeServerList();
                    return;
                }
                CommClass.DtServList = MCMD.ExtcuteDataTable(CommClass.DBCONN, CommandType.StoredProcedure, "spu_getserverlist", null, "ServList");

            }

            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>CommonFunction>>GetServerList>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }
        }
        #endregion        

        #region 故障告警添加
        /// <summary>
        /// 故障告警添加
        /// </summary>
        /// <param name="DevID">设备编号</param>
        /// <param name="DevType">设备类型</param>
        /// <param name="Descr">设备描述</param>
        /// <param name="Ret">操作返回值</param>
        public static void Alarm_Add(string DevID,string DevType, string Descr, ref int Ret)
        {
            MySqlCmdHeader MCMD = MySqlCmdHeader.Instance;
            try
            {

                MySqlParameter[] parm = new MySqlParameter[4];
                parm[0] = MySqlCmdHeader.Parameter("p_devid", MySqlDbType.VarChar, DevID);
                parm[1] = MySqlCmdHeader.Parameter("p_devtype", MySqlDbType.VarChar, DevType);
                parm[2] = MySqlHeader.MySqlCmdHeader.Parameter("p_descr", MySqlDbType.VarChar, Descr);
                parm[3] = MySqlHeader.MySqlCmdHeader.Parameter("Ret", MySqlDbType.Int32, ParameterDirection.Output);
                int i = MCMD.ExecuteNonQuery(CommClass.DBCONN, CommandType.StoredProcedure, "spu_t_alarmlog_add", parm);

                Ret = Convert.ToInt32(parm[3].Value);


            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>Alarm_Add>>", ex.Message);

            }
            finally
            {
                MCMD.Dispose();
            }
        }
        #endregion

        #region 服务器列表更新通知
        /// <summary>
        /// 服务器列表更新通知
        /// </summary>
        public static void NoticeServerList()
        {
            

            try
            {
                short cmd1 = 0;//主命令字
                short cmd2 = 0;//子命令字
                cmd1 = OMSCmd.RspGetServList;
                cmd2 = ErrCommon.Success;
                byte[] ByteResult = null;
                DataTable dt = new DataTable();
                dt = CommClass.DtServList.Copy();
                Commonality.CommClass.OutputBody(dt);
                CommClass.DataTableToByteArry(dt, ref ByteResult);


                foreach (KeyValuePair<string, Kernel.StateObject> a in CommClass.ClientConnList)
                {
                    Hashtable _hashtable_Package = new Hashtable();
                    _hashtable_Package.Add("1", a.Value);//...連結位置 
                    _hashtable_Package.Add("2", cmd1);
                    _hashtable_Package.Add("3", cmd2);
                    _hashtable_Package.Add("4", ByteResult);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CommonFunction.SendDatas), _hashtable_Package);
                }
            }
            catch (Exception ex)
            {
                Commonality.ConsoleManage.Write(Commonality.ErrorLevel.Serious, "KOIPMonitor>>NoticeServerList>>", ex.Message);

            }

        }
        #endregion

    }
}
