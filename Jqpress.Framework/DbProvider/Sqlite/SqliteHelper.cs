using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.Reflection;
using Jqpress.Framework.Configuration;

namespace Jqpress.Framework.DbProvider.Sqlite
{
    public sealed class SqliteHelper
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        static string ConnectionString
        {
            get;
            set;
        }
        static SqliteHelper()
        {
            //数据库文件路径
            var _mdbpath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);//for windows
            //var filePath = System.Environment.CurrentDirectory + @"\App_Data\Jqpress.db";
            var connStr =  "URI=file:" + _mdbpath + ",version=3";//for sqlite & linux
            ConnectionString = connStr;
        }

        #region 参数
        /// <summary>
        /// 创建一个传入参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public static SqliteParameter MakeInParam(string name, object value, DbType type, int size = 50)
        {
            var para = new SqliteParameter(name, type, size);
            para.Direction = ParameterDirection.Input;
            para.Value = value;
            return para;
        }
        /// <summary>
        /// 创建一个传入参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public static SqliteParameter MakeInParam(string name, DbType type, int size, object value)
        {
            var para = new SqliteParameter(name, type, size);
            para.Direction = ParameterDirection.Input;
            para.Value = value;
            return para;
        }

        /// <summary>
        /// 创建一个传出参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public static SqliteParameter MakeOutParam(string name, DbType type = DbType.String, int size = 50)
        {
            var para = new SqliteParameter(name, type, size);
            para.Direction = ParameterDirection.Output;
            return para;
        }
        #endregion

        #region Connection
        static SqliteConnection CreateConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            return conn;
        }
        static SqliteCommand CreateCommand(string commandText, CommandType commandType, SqliteConnection connection, params SqliteParameter[] paramList)
        {
            var cmd = new SqliteCommand(commandText, connection);
            cmd.CommandType = commandType;
            if (paramList != null)
            {
                foreach (var para in paramList)
                {
                    cmd.Parameters.Add(para);
                }
            }
            return cmd;
        }
        #endregion

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText,null);
        }

        public static int ExecuteNonQuery(CommandType cmdType, string commandText, params SqliteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, cmdType, conn, paramList))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static int ExecuteNonQuery(string commandText, params SqliteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Query
        /// <summary>
        /// 执行SQL语句,得到DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        //public static DataSet ExecuteDataSet(string commandText, params SqliteParameter[] paramList)
        //{
        //    using (var conn = CreateConnection())
        //    {
        //        conn.Open();
        //        using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
        //        {
        //            var ada = new SqliteDataAdapter(cmd);
        //            var ds = new DataSet();
        //            ada.Fill(ds);
        //            return ds;
        //        }
        //    }
        //}
        /// <summary>
        /// 执行SQL语句,得到第一个DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        //public static DataTable ExecuteDataTable(string commandText, params SqliteParameter[] paramList)
        //{
        //    var ds = ExecuteDataSet(commandText, paramList);
        //    return ds.Tables[0];
        //}
        /// <summary>
        /// 执行SQL语句,返回数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        //public static List<T> ExecuteList<T>(string commandText, params SqliteParameter[] paramList)
        //{
        //    var dt = ExecuteDataTable(commandText, paramList);
        //    return CollectionHelper.ConvertTo<T>(dt);
        //}
        /// <summary>
        /// 执行SQL语句,返回第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        //public static T ExecuteItem<T>(string commandText, params SqliteParameter[] paraList)
        //{
        //    var dt = ExecuteDataTable(commandText, paraList);
        //    if (dt.Rows.Count == 0) return default(T);
        //    var dr = dt.Rows[0];
        //    return CollectionHelper.CreateItem<T>(dr);
        //}
        #endregion

        #region ExecuteReader

        public static SqliteDataReader ExecuteReader(string commandText) 
        {
            return ExecuteReader(CommandType.Text,commandText,null);
        }

        public static SqliteDataReader ExecuteReader(string commandText, SqliteParameter[] paramList)
        {
            return ExecuteReader(CommandType.Text, commandText, paramList);
        }
        /// <summary>
        /// 返回 SqliteDataReader
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数组</param>
        /// <returns></returns>
        public static SqliteDataReader ExecuteReader(CommandType cmdType, string commandText, SqliteParameter[] paramList)
        {
            try
            {

                var conn = CreateConnection();
                    conn.Open();
                    var cmd = CreateCommand(commandText, cmdType, conn, paramList);
                        SqliteDataReader read = cmd.ExecuteReader();
                        cmd.Parameters.Clear();
                        return read;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(CommandType type,string commandText, params SqliteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, type, conn, paramList))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object ExecuteScalar(string commandText, params SqliteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion


        /// <summary>
        /// 获取分页Sql for sqlite
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="colList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetPageSql(string tableName, string colName, string colList, int pageSize, int pageIndex, int orderBy, string condition)
        {
            string temp = string.Empty;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(condition))
            {
                condition = " 1=1 ";
            }


            temp = "select {1} from {2} where {3} order by {4} {5} limit {0} OFFSET {6}";
            sql = string.Format(temp, pageSize, colList, tableName, condition, colName, orderBy == 1 ? "desc" : "asc", pageSize * (pageIndex - 1));

            //第一页
            if (pageIndex == 1)
            {
                temp = "select {1} from {2} where {3} order by {4} {5} limit {0}";
                sql = string.Format(temp, pageSize, colList, tableName, condition, colName, orderBy == 1 ? "desc" : "asc");
            }

            return sql;
        }

    }
}
