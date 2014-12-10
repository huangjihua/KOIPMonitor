using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace MySqlHeader
{
  
    [Serializable]
    public class MySqlCmdHeader : IDisposable
    {
        private static MySqlCmdHeader _instance = null;
        private bool m_disposed;
        //将构造函数设为private，防止客户代码通过new实例化对象 
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public MySqlCmdHeader()
        { }
       
        public static MySqlCmdHeader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MySqlCmdHeader();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 设置MySqlParameter参数
        /// </summary>
        /// <returns></returns>
        public static MySqlParameter Parameter(string ParamName, MySqlDbType type, Object Value)
        {
            
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.MySqlDbType = type;
            parameter.Value = Value;
            return parameter;
            

        }
        public static MySqlParameter Parameter(string ParamName, DbType type, Object Value)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.DbType = type;
            parameter.Value = Value;
            return parameter;

        }

        public static MySqlParameter Parameter(string ParamName, MySqlDbType type, Object Value, int size)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.MySqlDbType = type;
            parameter.Value = Value;
            parameter.Size = size;
            return parameter;

        }
        public static MySqlParameter Parameter(string ParamName, DbType type, Object Value, int size)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.DbType = type;
            parameter.Value = Value;
            parameter.Size = size;
            return parameter;
        }
        ///// <summary>
        ///// 设置MySqlParameter参数
        ///// </summary>
        ///// <returns></returns>
        public static MySqlParameter Parameter(string ParamName, MySqlDbType type, Object Value, ParameterDirection pd, int size)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.MySqlDbType = type;
            parameter.Value = Value;
            parameter.Direction = pd;
            parameter.Size = size;
            //ParameterDirection pd = ParameterDirection.Input;
            return parameter;

        }

        /// <summary>
        /// 设置SqlParameter参数
        /// </summary>
        /// <returns></returns>
        public static MySqlParameter Parameter(string ParamName, DbType type, Object Value, ParameterDirection pd, int size)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.DbType = type;
            parameter.Value = Value;
            parameter.Direction = pd;
            parameter.Size = size;
            //ParameterDirection pd = ParameterDirection.Input;
            return parameter;

        }


        ///// <summary>
        ///// 设置SqlParameter参数
        ///// </summary>
        ///// <returns></returns>
        public static MySqlParameter Parameter(string ParamName, MySqlDbType type, Object Value, ParameterDirection pd)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.MySqlDbType = type;
            parameter.Value = Value;
            parameter.Direction = pd;
            //ParameterDirection pd = ParameterDirection.Input;
            return parameter;

        }

        /// <summary>
        /// 设置SqlParameter参数
        /// </summary>
        /// <returns></returns>
        public static MySqlParameter Parameter(string ParamName, DbType type, Object Value, ParameterDirection pd)
        {
            MySqlParameter parameter = new MySqlParameter();
            parameter.ParameterName = ParamName;
            parameter.DbType = type;
            parameter.Value = Value;
            parameter.Direction = pd;
            //ParameterDirection pd = ParameterDirection.Input;
            return parameter;
            //ParameterDirection.Output
            //ParameterDirection.ReturnValueOrOutput
        }

        /// <summary>
        /// 设置Command对象的参数
        /// </summary>
        /// <param name="Cmd">SqlCommand 对象</param>
        /// <param name="Conn">SqlConnection对象</param>
        /// <param name="CmdType">指定SqlCommand如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">SqlParameter参数列表</param>
        private static void CmdParameter(MySqlCommand Cmd, MySqlConnection Conn, MySqlTransaction Transaction, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
            Cmd.Connection = Conn;
            Cmd.CommandType = CmdType;
            Cmd.CommandText = CmdText;
            if (Transaction != null)
                Cmd.Transaction = Transaction;
            if (Parameter != null)
            {
                foreach (MySqlParameter parameter in Parameter)
                    Cmd.Parameters.Add(parameter);
            }
        }


        /// <summary>
        /// 执行ExecuteNonQuery操作，返回受影响的行数
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            int Sin;
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand Comm = new MySqlCommand())
                {
                    
                    MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                    Sin = Comm.ExecuteNonQuery();
                }
            }
            return Sin;
        }
        public int ExecuteNonQuery(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter, bool isTran)
        {
            int Sin = 0;
            MySqlTransaction transaction;
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                transaction = Conn.BeginTransaction();
                using (MySqlCommand Comm = new MySqlCommand())
                {

                    MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                    try
                    {
                        Sin = Comm.ExecuteNonQuery();
                        transaction.Commit();//提交事务
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();//回滚事务
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                   
                }
            }
            return Sin;
        }
        /// <summary>
        /// 执行无返回ExecuteNonQuery操作
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>
        public void ExecuteNonQueryNull(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand Comm = new MySqlCommand())
                {
                    MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                    Comm.ExecuteNonQuery();
                    Comm.Dispose();
                }
            }
        }
        public void ExecuteNonQueryNull(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter, bool isTran)
        {
            MySqlTransaction transaction;
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                transaction = Conn.BeginTransaction();
                using (MySqlCommand Comm = new MySqlCommand())
                {
                    MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                    Comm.ExecuteNonQuery();
                    try
                    {
                        Comm.ExecuteNonQuery();
                        transaction.Commit();//提交事务
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();//回滚事务
                        }
                        catch (Exception ex2)
                        {
                            //Response.Write("回滚错误类型:" + ex2.GetType());
                            //Response.Write("回滚错误信息:" + ex2.Message);
                        }
                    }
                    Comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行ExecuteScalar操作，返回第一行第一列
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>
        public string ExecuteScalar(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            string Sin = string.Empty;
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                MySqlCommand Comm = new MySqlCommand();
                MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                Sin = Convert.ToString(Comm.ExecuteScalar());

                Comm.Dispose();
            }
            return Sin;
        }
        public string ExecuteScalar(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter, bool isTran)
        {
            string Sin = string.Empty;
            MySqlTransaction transaction;
            using (MySqlConnection Conn = new MySqlConnection(ConnString))
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                transaction = Conn.BeginTransaction();
                MySqlCommand Comm = new MySqlCommand();
                MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
                //Sin = Convert.ToString(Comm.ExecuteScalar());
                try
                {
                    Sin = Convert.ToString(Comm.ExecuteScalar());
                    transaction.Commit();//提交事务
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();//回滚事务
                    }
                    catch (Exception ex2)
                    {
                      
                    }
                }
                Comm.Dispose();
            }
            return Sin;
        }
        /// <summary>
        /// 执行ExecuteReader操作，返回SqlDataReader读取的数据集
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>
        public MySqlDataReader ExtcuteReader(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            MySqlConnection Conn = new MySqlConnection(ConnString);
            MySqlCommand Comm = new MySqlCommand();
            MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
            MySqlDataReader Dr = Comm.ExecuteReader(CommandBehavior.CloseConnection);
            Comm.Dispose();
            //Conn.Close();
            //Conn.Dispose();
            return Dr;
        }
        /// <summary>
        /// 执行ExecuteReader操作，返回DataTable读取的数据集
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>
        public DataTable ExtcuteDataTable(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter,string dbname)
        {
            DataSet Ds = new DataSet();
            MySqlConnection Conn = new MySqlConnection(ConnString);
            MySqlCommand Comm = new MySqlCommand();
            MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
            MySqlDataAdapter Ap = new MySqlDataAdapter();
            Ap.SelectCommand = Comm;
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    Ap.Fill(Ds, dbname);
                }
                else
                {
                    Ap.Fill(Ds, "db");
                }
            }
            catch
            { }
            finally
            {
                Comm.Dispose();
                Conn.Dispose();
                Conn.Close();
            }
            
            if (!string.IsNullOrEmpty(dbname))
            {
                return Ds.Tables[dbname];
            }
            else
            {
                return Ds.Tables["db"];
            }
        }
        public DataTable ExtcuteDataTable(string ConnString, CommandType CmdType, string CmdText, MySqlParameter[] Parameter, string dbname, bool isTran)
        {
            DataSet Ds = new DataSet();
            MySqlTransaction transaction;
            MySqlConnection Conn = new MySqlConnection(ConnString);
            transaction = Conn.BeginTransaction();
            MySqlCommand Comm = new MySqlCommand();
            MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
            MySqlDataAdapter Ap = new MySqlDataAdapter();
            
            Ap.SelectCommand = Comm;
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    Ap.Fill(Ds, dbname);
                }
                else
                {
                    Ap.Fill(Ds, "db");
                }
                transaction.Commit();//提交事务
            }
            catch
            {
                try
                {
                    transaction.Rollback();//回滚事务
                }
                catch (Exception ex2)
                {

                }
            }
            finally
            {
                Comm.Dispose();
                Conn.Dispose();
                Conn.Close();
            }
            
            if (!string.IsNullOrEmpty(dbname))
            {
                return Ds.Tables[dbname];
            }
            else
            {
                return Ds.Tables["db"];
            }
        }
     
        
        /// <summary>
        /// 执行ExecuteReader操作，返回DataTable读取的数据集
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="dbname">表名</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>yjyygyowxf207154

        public DataSet ExtcuteDataSet(string ConnString, string dbname, CommandType CmdType, string CmdText, MySqlParameter[] Parameter)
        {
            DataSet Ds = new DataSet();
            MySqlConnection Conn = new MySqlConnection(ConnString);
            MySqlCommand Comm = new MySqlCommand();
            //Comm.CommandTimeout = CommandTimeout;
            MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
            MySqlDataAdapter Ap = new MySqlDataAdapter();
            Ap.SelectCommand = Comm;
            try
            {
                //Ap.Fill(Ds);
                if (!string.IsNullOrEmpty(dbname))
                {
                    Ap.Fill(Ds, dbname);
                }
                else
                {
                    Ap.Fill(Ds, "db");
                }
            }
            catch
            { }
            finally
            {
                Comm.Dispose();
                Conn.Close();
                Conn.Dispose();
            }
            return Ds;
        }

        /// <summary>
        /// 执行ExecuteReader操作，返回DataTable读取的数据集
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="dbname">表名</param>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">命令字符串</param>
        /// <param name="Parameter">参数列表</param>
        /// <returns></returns>yjyygyowxf207154

        
        public DataSet ExtcuteDataSet(string ConnString,string dbname, CommandType CmdType, string CmdText, MySqlParameter[] Parameter, int CommandTimeout)
        {
            DataSet Ds = new DataSet();
            MySqlConnection Conn = new MySqlConnection(ConnString);
            MySqlCommand Comm = new MySqlCommand();
            if (CommandTimeout > 0)
            {
                Comm.CommandTimeout = CommandTimeout;
            }
            MySqlCmdHeader.CmdParameter(Comm, Conn, null, CmdType, CmdText, Parameter);
            MySqlDataAdapter Ap = new MySqlDataAdapter();
            Ap.SelectCommand = Comm;
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    Ap.Fill(Ds, dbname);
                }
                else
                {
                    Ap.Fill(Ds,"db");
                }
            }
            catch
            { }
            finally
            {
                Comm.Dispose();
                Conn.Close();
                Conn.Dispose();
            }
            return Ds;
        }

        /**/
        /// <summary>
        /// 析构函数
        /// </summary>
        ~MySqlCmdHeader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    if (_instance != null)
                        _instance = null;
                    //if (dt != null)
                    //    this.dt.Dispose();
                    //this._CurrentPosition = null;
                    //this._Department = null;
                    //this._EmployeeCode = null;
                }

                // Release unmanaged resources
                m_disposed = true;
            }
        }
       

    }
}
