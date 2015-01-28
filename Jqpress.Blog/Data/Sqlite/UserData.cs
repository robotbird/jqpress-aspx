using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using Jqpress.Blog.Entity;
using Jqpress.Framework.DbProvider.Sqlite;
using Jqpress.Framework.Configuration;

namespace Jqpress.Blog.Data.Sqlite
{
    public partial class DataProvider
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public int InsertUser(UserInfo userinfo)
        {
            string cmdText =string.Format(@" insert into [{0}users](
                                [UserType],[UserName],[NickName],[Password],[Email],[SiteUrl],[AvatarUrl],[Description],[sortnum],[Status],[PostCount],[CommentCount],[CreateTime])
                                values (
                                @UserType,@UserName,@NickName,@Password,@Email,@SiteUrl,@AvatarUrl,@Description,@SortNum,@Status, @PostCount,@CommentCount,@CreateTime )",ConfigHelper.Tableprefix);
            SqliteParameter[] prams = { 
                                        SqliteHelper.MakeInParam("@UserType", DbType.Int32,4, userinfo.UserType),
                                        SqliteHelper.MakeInParam("@UserName", DbType.String,50, userinfo.UserName),
                                        SqliteHelper.MakeInParam("@NickName", DbType.String,50, userinfo.NickName),
                                        SqliteHelper.MakeInParam("@Password", DbType.String,50, userinfo.Password),
                                        SqliteHelper.MakeInParam("@Email", DbType.String,50, userinfo.Email),
                                        SqliteHelper.MakeInParam("@SiteUrl", DbType.String,255, userinfo.SiteUrl),
                                        SqliteHelper.MakeInParam("@AvatarUrl", DbType.String,255, userinfo.AvatarUrl),
                                        SqliteHelper.MakeInParam("@Description", DbType.String,255, userinfo.Description),
                                        SqliteHelper.MakeInParam("@SortNum", DbType.Int32,4, userinfo.SortNum),
                                        SqliteHelper.MakeInParam("@Status", DbType.Int32,4, userinfo.Status),                           
                                        SqliteHelper.MakeInParam("@PostCount", DbType.Int32,4, userinfo.PostCount),
                                        SqliteHelper.MakeInParam("@CommentCount", DbType.Int32,4, userinfo.CommentCount),
                                        SqliteHelper.MakeInParam("@CreateTime", DbType.Date,8, userinfo.CreateTime),
                                        
                                    };
            int r = SqliteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return Convert.ToInt32(SqliteHelper.ExecuteScalar(string.Format("select [UserId] from [{0}users]  order by [UserId] desc limit 1",ConfigHelper.Tableprefix)));
            }
            return 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public int UpdateUser(UserInfo userinfo)
        {
            string cmdText =string.Format( @"update [{0}users] set
                                [UserType]=@UserType,
                                [UserName]=@UserName,
                                [NickName]=@NickName,
                                [Password]=@Password,
                                [Email]=@Email,
                                [SiteUrl]=@SiteUrl,
                                [AvatarUrl]=@AvatarUrl,
                                [Description]=@Description,
                                [SortNum]=@SortNum,
                                [Status]=@Status,
                                [PostCount]=@PostCount,
                                [CommentCount]=@CommentCount,
                                [CreateTime]=@CreateTime
                                where UserId=@UserId",ConfigHelper.Tableprefix);
            SqliteParameter[] prams = { 
                                        SqliteHelper.MakeInParam("@UserType", DbType.Int32,4, userinfo.UserType),
                                        SqliteHelper.MakeInParam("@UserName", DbType.String,50, userinfo.UserName),
                                        SqliteHelper.MakeInParam("@NickName", DbType.String,50, userinfo.NickName),
                                        SqliteHelper.MakeInParam("@Password", DbType.String,50, userinfo.Password),
                                        SqliteHelper.MakeInParam("@Email", DbType.String,50, userinfo.Email),
                                        SqliteHelper.MakeInParam("@SiteUrl", DbType.String,255, userinfo.SiteUrl),
                                        SqliteHelper.MakeInParam("@AvatarUrl", DbType.String,255, userinfo.AvatarUrl),
                                        SqliteHelper.MakeInParam("@Description", DbType.String,255, userinfo.Description),
                                        SqliteHelper.MakeInParam("@SortNum", DbType.String,255, userinfo.SortNum),
                                        SqliteHelper.MakeInParam("@Status", DbType.Int32,4, userinfo.Status),                           
                                        SqliteHelper.MakeInParam("@PostCount", DbType.Int32,4, userinfo.PostCount),
                                        SqliteHelper.MakeInParam("@CommentCount", DbType.Int32,4, userinfo.CommentCount),
                                        SqliteHelper.MakeInParam("@CreateTime", DbType.Date,8, userinfo.CreateTime),
                                        SqliteHelper.MakeInParam("@UserId", DbType.Int32,4, userinfo.UserId),
                                    };
            return SqliteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteUser(int userid)
        {
            string cmdText = string.Format("delete from [{0}users] where [userid] = @userid",ConfigHelper.Tableprefix);
            SqliteParameter[] prams = { 
								        SqliteHelper.MakeInParam("@userid",DbType.Int32,4,userid)
							        };
            return SqliteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserList()
        {
            string cmdText = string.Format("select * from [{0}users]  order by [sortnum] asc,[userid] asc",ConfigHelper.Tableprefix);
            return DataReaderToUserList(SqliteHelper.ExecuteReader(cmdText));

        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private static List<UserInfo> DataReaderToUserList(SqliteDataReader read)
        {
            var list = new List<UserInfo>();
            while (read.Read())
            {
                var userinfo = new UserInfo
                                   {
                                       UserId = Convert.ToInt32(read["UserId"]),
                                       UserType = Convert.ToInt32(read["UserType"]),
                                       UserName = Convert.ToString(read["UserName"]),
                                       NickName = Convert.ToString(read["NickName"]),
                                       Password = Convert.ToString(read["Password"]),
                                       Email = Convert.ToString(read["Email"]),
                                       SiteUrl = Convert.ToString(read["SiteUrl"]),
                                       AvatarUrl = Convert.ToString(read["AvatarUrl"]),
                                       Description = Convert.ToString(read["Description"]),
                                       SortNum = Convert.ToInt32(read["SortNum"]),
                                       Status = Convert.ToInt32(read["Status"]),
                                       PostCount = Convert.ToInt32(read["PostCount"]),
                                       CommentCount = Convert.ToInt32(read["CommentCount"]),
                                       CreateTime = Convert.ToDateTime(read["CreateTime"])
                                   };


                list.Add(userinfo);
            }
            read.Close();
            return list;
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            string cmdText = string.Format("select count(1) from [{0}users] where [userName] = @userName ",ConfigHelper.Tableprefix);
            SqliteParameter[] prams = { 
                                        SqliteHelper.MakeInParam("@userName",DbType.String,50,userName),
							        };
            return Convert.ToInt32(SqliteHelper.ExecuteScalar(CommandType.Text, cmdText, prams)) > 0;
        }
    }
}
