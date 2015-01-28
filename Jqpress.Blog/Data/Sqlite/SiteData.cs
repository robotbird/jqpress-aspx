using System;
using System.Collections.Generic;
using Jqpress.Blog.Entity;
using Mono.Data.Sqlite;
using System.Data;
using Jqpress.Framework.DbProvider.Sqlite;
using Jqpress.Framework.Configuration;
namespace Jqpress.Blog.Data.Sqlite
{
    public partial class DataProvider
    {
        public bool UpdateStatistics(StatisticsInfo statistics)
        {
            string cmdText =string.Format( "update [{0}sites] set PostCount=@PostCount,CommentCount=@CommentCount,VisitCount=@VisitCount,TagCount=@TagCount",ConfigHelper.Tableprefix);
            SqliteParameter[] prams = {
					                        SqliteHelper.MakeInParam("@PostCount", DbType.Int32,4,statistics.PostCount),
					                        SqliteHelper.MakeInParam("@CommentCount", DbType.Int32,4,statistics.CommentCount),
					                        SqliteHelper.MakeInParam("@VisitCount", DbType.Int32,4,statistics.VisitCount),
					                        SqliteHelper.MakeInParam("@TagCount", DbType.Int32,4,statistics.TagCount),
                                        };

            return SqliteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams) == 1;
        }

        public StatisticsInfo GetStatistics()
        {
            string cmdText = string.Format("select  * from [{0}sites] limit 1", ConfigHelper.Tableprefix);

            string insertText = string.Format("insert into [{0}sites] ([PostCount],[CommentCount],[VisitCount],[TagCount]) values ( '0','0','0','0')", ConfigHelper.Tableprefix);

            List<StatisticsInfo> list = DataReaderToListSite(SqliteHelper.ExecuteReader(cmdText));

            if (list.Count == 0)
            {
                SqliteHelper.ExecuteNonQuery(insertText);
            }
            list = DataReaderToListSite(SqliteHelper.ExecuteReader(cmdText));

            return list.Count > 0 ? list[0] : null;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">SqliteDataReader</param>
        /// <returns>TermInfo</returns>
        private static List<StatisticsInfo> DataReaderToListSite(SqliteDataReader read)
        {
            var list = new List<StatisticsInfo>();
            while (read.Read())
            {
                var site = new StatisticsInfo
                               {
                                   PostCount = Convert.ToInt32(read["PostCount"]),
                                   CommentCount = Convert.ToInt32(read["CommentCount"]),
                                   VisitCount = Convert.ToInt32(read["VisitCount"]),
                                   TagCount = Convert.ToInt32(read["TagCount"])
                               };

                list.Add(site);
            }
            read.Close();
            return list;
        }


    }
}
