using System;
using System.Collections.Generic;
//using Mono.Data.Sqlite;
using System.Data.SQLite;
using System.Data;
using Jqpress.Blog.Entity;
using Jqpress.Framework.DbProvider.SQLite;
using Jqpress.Framework.Configuration;

namespace Jqpress.Blog.Data.SQLite
{
    public partial class DataProvider
    {
        public int InsertLink(LinkInfo link)
        {
            string cmdText = string.Format(@"insert into [{0}links]
                            (
                            [type],[linkname],[linkurl],[position],[target],[description],[sortnum],[status],[createtime]
                            )
                            values
                            (
                            @type,@linkname,@linkurl,@position,@target,@description,@sortnum,@status,@createtime
                            )",ConfigHelper.Tableprefix);
            SQLiteParameter[] prams = { 
                                SQLiteHelper.MakeInParam("@type",DbType.Int32,4,link.Type),
								SQLiteHelper.MakeInParam("@linkname",DbType.String,100,link.LinkName),
                                SQLiteHelper.MakeInParam("@linkurl",DbType.String,255,link.LinkUrl),
                                SQLiteHelper.MakeInParam("@position",DbType.Int32,4,link.Position),
                                SQLiteHelper.MakeInParam("@target",DbType.String,50,link.Target),
								SQLiteHelper.MakeInParam("@description",DbType.String,255,link.Description),
                                SQLiteHelper.MakeInParam("@sortnum",DbType.Int32,4,link.SortNum),
								SQLiteHelper.MakeInParam("@status",DbType.Int32,4,link.Status),
								SQLiteHelper.MakeInParam("@createtime",DbType.Date,8,link.CreateTime),
							};

            int r = SQLiteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return Convert.ToInt32(SQLiteHelper.ExecuteScalar(string.Format("select  [linkid] from [{0}links]  order by [linkid] desc limit 1",ConfigHelper.Tableprefix)));
            }
            return 0;
        }

        public int UpdateLink(LinkInfo link)
        {
            string cmdText = string.Format(@"update [{0}links] set
                                [type]=@type,
                                [linkname]=@linkname,
                                [linkurl]=@linkurl,
                                [position]=@position,
                                [target]=@target,
                                [description]=@description,
                                [sortnum]=@sortnum,
                                [status]=@status,
                                [createtime]=@createtime
                                where linkid=@linkid",ConfigHelper.Tableprefix);
            SQLiteParameter[] prams = { 
                                SQLiteHelper.MakeInParam("@type",DbType.Int32,4,link.Type),
								SQLiteHelper.MakeInParam("@linkname",DbType.String,100,link.LinkName),
                                SQLiteHelper.MakeInParam("@linkurl",DbType.String,255,link.LinkUrl),
                                SQLiteHelper.MakeInParam("@position",DbType.Int32,4,link.Position),
                                SQLiteHelper.MakeInParam("@target",DbType.String,50,link.Target),
								SQLiteHelper.MakeInParam("@description",DbType.String,255,link.Description),
                                SQLiteHelper.MakeInParam("@sortnum",DbType.Int32,4,link.SortNum),
								SQLiteHelper.MakeInParam("@status",DbType.Int32,4,link.Status),
								SQLiteHelper.MakeInParam("@createtime",DbType.Date,8,link.CreateTime),
                                SQLiteHelper.MakeInParam("@linkid",DbType.Int32,4,link.LinkId),
							};

            return Convert.ToInt32(SQLiteHelper.ExecuteScalar(CommandType.Text, cmdText, prams));
        }

        public int DeleteLink(int linkId)
        {
            string cmdText = string.Format("delete from [{0}links] where [linkid] = @linkid",ConfigHelper.Tableprefix);
            SQLiteParameter[] prams = { 
								SQLiteHelper.MakeInParam("@linkid",DbType.Int32,4,linkId)
							};
            return SQLiteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);


        }


        public List<LinkInfo> GetLinkList()
        {

            string cmdText = string.Format("select * from [{0}links]  order by [sortnum] asc,[linkid] asc",ConfigHelper.Tableprefix);

            return DataReaderToListLink(SQLiteHelper.ExecuteReader(cmdText));

        }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param LinkName="read">SQLiteDataReader</param>
        /// <param name="read"></param>
        /// <returns>LinkInfo</returns>
        private static List<LinkInfo> DataReaderToListLink(SQLiteDataReader read)
        {
            var list = new List<LinkInfo>();
            while (read.Read())
            {
                var link = new LinkInfo
                               {
                                   LinkId = Convert.ToInt32(read["Linkid"]),
                                   Type = Convert.ToInt32(read["Type"]),
                                   LinkName = Convert.ToString(read["LinkName"]),
                                   LinkUrl = Convert.ToString(read["LinkUrl"]),
                                   Target = Convert.ToString(read["Target"]),
                                   Description = Convert.ToString(read["Description"]),
                                   SortNum = Convert.ToInt32(read["SortNum"]),
                                   Status = Convert.ToInt32(read["Status"]),
                                   CreateTime = Convert.ToDateTime(read["CreateTime"])
                               };
                if (read["Position"] != DBNull.Value)
                {
                    link.Position = Convert.ToInt32(read["Position"]);
                }



                list.Add(link);
            }
            read.Close();
            return list;
        }

    }
}
