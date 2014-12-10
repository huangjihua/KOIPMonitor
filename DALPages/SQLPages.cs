#define WinX
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DALPages
{
    public class SQLPages
    {
        private string tablename;//表名
        private string fields;//查询返回的字段
        private string orderbyname;//排序的字段名
        private int pagesize;//每页显示记录数
        private int currentpage = 1;//当前页码
        private int ordertype;//设置排序类型,非 0 值则降序
        private string sqlwhere;//查询条件(注意: 不要加 where)
        private string strorderby;//自定义排序字符串如   字段名 asc
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SQLPages()
        {

        }
        //public SQLPages(CodeMode _codemode)
        //{
        //    this.codemode = _codemode;
        //}
        public string TABLENAME
        {
            get { return tablename; }
            set { tablename = value; }
        }
        public string FIELDS
        {
            get { return fields; }
            set { fields = value; }
        }
        public string ORDERBYNAME
        {
            get { return orderbyname; }
            set { orderbyname = value; }
        }
        public int PAGESIZE
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
        public int CURRENTPAGE
        {
            get { return currentpage; }
            set { currentpage = value; }
        }
        public int ORDERTYPE
        {
            get { return ordertype; }
            set { ordertype = value; }
        }
        public string SQLWHERE
        {
            get { return sqlwhere; }
            set { sqlwhere = value; }
        }
        public string STRORDERBY
        {
            get { return strorderby; }
            set { strorderby = value; }
        }
        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="_tablename">表名</param>
        /// <param name="_fields">查询返回的字段</param>
        /// <param name="_orderbyname">排序的字段名</param>
        /// <param name="_pagesize">每页显示记录数</param>
        /// <param name="_currentpage">当前页码</param>
        /// <param name="_ordertype">设置排序类型,非 0 值则降序</param>
        /// <param name="_sqlwhere">查询条件(注意: 不要加 where)</param>
        /// <param name="_strorderby">自定义排序字符串如   字段名 asc</param>
        public SQLPages(string _tablename, string _fields, string _orderbyname, int _pagesize, int _currentpage, int _ordertype, string _sqlwhere, string _strorderby)
        {

            this.tablename = _tablename;
            this.fields = _fields;
            this.orderbyname = _orderbyname;
            this.pagesize = _pagesize;
            this.currentpage = _currentpage;
            this.ordertype = _ordertype;
            this.sqlwhere = _sqlwhere;
            this.strorderby = _strorderby;
        }
        public System.Data.DataSet ds(string strconn, string StoredProcedureName)
        {
            //Console.WriteLine("------------System.Data.DataSet ds--------------");
            //Console.WriteLine("step1");
            System.Data.DataSet ds = new System.Data.DataSet();
            //Console.WriteLine("step2");
            try
            {
                //Console.WriteLine("step4");
                MySqlHeader.MySqlCmdHeader MSH = MySqlHeader.MySqlCmdHeader.Instance;
                //Console.WriteLine("step5");
                string aa = this.fields;
                //Console.WriteLine("step6");
                this.fields = this.fields.Replace("isnull", "ifnull").Replace(",'')", ",'''')");
                //Console.WriteLine("step7");
                this.fields = this.fields.Replace("[", "");
                //Console.WriteLine("step8");
                this.fields = this.fields.Replace("]", "");
                //Console.WriteLine("step9");
                this.sqlwhere = this.sqlwhere.Replace("[", "");
                //Console.WriteLine("step10");
                this.sqlwhere = this.sqlwhere.Replace("]", "");
                //Console.WriteLine("step11");
                MySql.Data.MySqlClient.MySqlParameter[] parm1 = new MySql.Data.MySqlClient.MySqlParameter[8];
                //Console.WriteLine("step12");
                parm1[0] = MySqlHeader.MySqlCmdHeader.Parameter("columnss", DbType.String, this.fields);
                //Console.WriteLine("step13");
                parm1[1] = MySqlHeader.MySqlCmdHeader.Parameter("tablename", DbType.String, this.tablename);
                //Console.WriteLine("step14");
                parm1[2] = MySqlHeader.MySqlCmdHeader.Parameter("PageSize", DbType.Int32, this.pagesize);
                //Console.WriteLine("step15");
                parm1[3] = MySqlHeader.MySqlCmdHeader.Parameter("StrWhere", DbType.String, this.sqlwhere);
                //Console.WriteLine("step16");
                parm1[4] = MySqlHeader.MySqlCmdHeader.Parameter("Order_Filed", DbType.String, this.orderbyname);
                //Console.WriteLine("step17");
                parm1[5] = MySqlHeader.MySqlCmdHeader.Parameter("OrderType", DbType.Int32, this.ordertype);
                //Console.WriteLine("step18");
                parm1[6] = MySqlHeader.MySqlCmdHeader.Parameter("CurrentPageCount", DbType.Int32, this.currentpage);
                //Console.WriteLine("step19");
                parm1[7] = MySqlHeader.MySqlCmdHeader.Parameter("OrderString", DbType.String, this.strorderby);
                //Console.WriteLine("step20");
                ds = MSH.ExtcuteDataSet(strconn, this.tablename, CommandType.StoredProcedure, StoredProcedureName, parm1, 30);
                //Console.WriteLine("step21");
                //string strSql = "select count(*) as Total,";
                //strSql = strSql + "(if(count(*) % " + this.pagesize + "=0,count(*)/" + this.pagesize + ",count(*)/" + this.pagesize + "+1) as ";

                //DataTable dt = MSH.ExtcuteDataTable(strconn, CommandType.Text, strSql, null, "dbCount");
                //ds.Tables.Add(dt);
                MSH.Dispose();
                //Console.WriteLine("step22");
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                //SH.Dispose();
            }
            return ds;
        }
    }
}