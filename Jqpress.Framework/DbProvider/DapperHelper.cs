using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Jqpress.Framework.Configuration;
//using System.Data.SQLite;

namespace Jqpress.Framework.DbProvider
{
    public class DapperHelper
    {
        private static string _mdbpath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);
        public static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _mdbpath;

        //连接数据库字符串。
       // private readonly string sqlconnection = ConfigurationManager.ConnectionStrings["Lee_Creek"].ConnectionString;
        private readonly string sqlconnection = "";
       // public readonly string mysqlconnectionString = @"server=127.0.0.1;database=test;uid=renfb;pwd=123456;charset='gbk'";
        //获取Sql Server的连接数据库对象。SqlConnection

        public OleDbConnection OpenConnection()
        {
             OleDbConnection conn = new OleDbConnection(ConnectionString);
             conn.Open();
             return conn;
        }

        //public OleDbConnection OpenSqliteConnection()
        //{
        //    SQLiteConnection conn = new SQLiteConnection(ConnectionString);
        //    conn.Open();
        //    return conn;
        //}
      
        public SqlConnection OpenConnectionSql()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }
    }
}
