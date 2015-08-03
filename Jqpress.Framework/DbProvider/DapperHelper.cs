using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Jqpress.Framework.Configuration;
using System.Data.SQLite;
using Mono.Data.Sqlite;

namespace Jqpress.Framework.DbProvider
{
    public class DapperHelper
    {
        //private static string _mdbpath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);
       // public static string ConnectionString = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", _mdbpath);//for windows

       // public static string ConnectionString = "URI=file:" + _mdbpath + ",version=3";//for mono & linux

        //连接数据库字符串。
       // private readonly string sqlconnection = ConfigurationManager.ConnectionStrings["Lee_Creek"].ConnectionString;
        private readonly string sqlconnection = "";
       // public readonly string mysqlconnectionString = @"server=127.0.0.1;database=test;uid=renfb;pwd=123456;charset='gbk'";
        //获取Sql Server的连接数据库对象。SqlConnection

        //public OleDbConnection OpenConnection()
        //{
        //     OleDbConnection conn = new OleDbConnection(ConnectionString);
        //     conn.Open();
        //     return conn;
        //}

        //for windows
        public SQLiteConnection OpenConnection()
        {
            SQLiteConnection conn = new SQLiteConnection("");
            conn.Open();
            return conn;
        }
        public SQLiteConnection OpenConnection(string connectionString)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            conn.Open();
            return conn;
        }

        //for mono
        //public SqliteConnection OpenConnection()// for sqlite & liunx
        //{
        //    // System.Web.HttpContext.Current.Response.Write(ConnectionString);
        //    // System.Web.HttpContext.Current.Response.End();
        //    SqliteConnection conn = new SqliteConnection(ConnectionString);
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
