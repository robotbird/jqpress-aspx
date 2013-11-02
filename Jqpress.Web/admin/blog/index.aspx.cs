using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Schema;

using Jqpress.Blog.Common;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Services;
using Jqpress.Blog.Configuration;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;
using Jqpress.Framework.Configuration;

namespace Jqpress.Web.admin.blog
{
    public partial class index : AdminPage
    {
        /// <summary>
        /// 数据库大小
        /// </summary>
        protected string DbSize;

        /// <summary>
        /// 数据库路径
        /// </summary>
        protected string DbPath;

        /// <summary>
        /// 附件大小
        /// </summary>
        protected string UpfileSize;

        /// <summary>
        /// 附件路径
        /// </summary>
        protected string UpfilePath;

        /// <summary>
        /// 附件个数
        /// </summary>
        protected int UpfileCount = 0;

        //   private string CommentMessage = "";

        protected List<CommentInfo> commentlist;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("控制台");
            if (Request["act"] == "getrss")
            {
                Response.Write(RssNews);
                Response.End();
            }

            if (!IsPostBack)
            {
                CheckStatistics();

                commentlist = CommentService.GetCommentList(15, 1, -1, -1, -1, (int)ApprovedStatus.Wait, -1, string.Empty);
                //rptComment.DataSource = list;
                //rptComment.DataBind();
                //if (list.Count == 0)
                //{
                //    CommentMessage = "暂时无待审核评论";
                //}


                DbPath = ConfigHelper.SitePath + ConfigHelper.DbConnection;
                System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));
                DbSize = GetFileSize(file.Length);

                UpfilePath = ConfigHelper.SitePath + "upfiles";

                GetDirectorySize(Server.MapPath(UpfilePath));

                UpfileSize = GetFileSize(dirSize);

                GetDirectoryCount(Server.MapPath(UpfilePath));
            }
            ShowResult();

        }

        /// <summary>
        /// 显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = PressRequest.GetQueryInt("result");
            switch (result)
            {
                case 11:
                    ShowMessage("统计分类文章数完成!");
                    break;
                case 12:
                    ShowMessage("统计标签文章数完成!");
                    break;
                case 13:
                    ShowMessage("统计作者文章和评论数完成!");
                    break;
                default:
                    break;
            }
        }

        protected void CheckStatistics()
        {
            StatisticsInfo s = StatisticsService.GetStatistics();
            bool update = false;

            int totalPosts = PostService.GetPostCount(-1, -1, -1, (int)PostStatus.Published, 0);
            if (totalPosts != s.PostCount)
            {
                s.PostCount = totalPosts;
                update = true;
            }

            int totalComments = CommentService.GetCommentCount(true);
            if (totalComments != s.CommentCount)
            {
                s.CommentCount = totalComments;
                update = true;
            }
            int totalTags = TagService.GetTagList().Count;
            if (totalTags != s.TagCount)
            {
                s.TagCount = totalTags;
                update = true;
            }
            if (update == true)
            {
                StatisticsService.UpdateStatistics();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">byte</param>
        /// <returns></returns>
        protected string GetFileSize(long size)
        {
            string FileSize = string.Empty;
            if (size > (1024 * 1024 * 1024))
                FileSize = ((double)size / (1024 * 1024 * 1024)).ToString(".##") + " GB";
            else if (size > (1024 * 1024))
                FileSize = ((double)size / (1024 * 1024)).ToString(".##") + " MB";
            else if (size > 1024)
                FileSize = ((double)size / 1024).ToString(".##") + " KB";
            else if (size == 0)
                FileSize = "0 Byte";
            else
                FileSize = ((double)size / 1).ToString(".##") + " Byte";

            return FileSize;
        }

        /// <summary>
        /// 文件夹大小
        /// </summary>
        public long dirSize = 0;

        /// <summary>
        /// 递归文件夹大小
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private long GetDirectorySize(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    dirSize += fi.Length;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    string new_dir = di.FullName;
                    GetDirectorySize(new_dir);
                }
            }
            return dirSize;
        }



        /// <summary>
        /// 递归文件数量
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private int GetDirectoryCount(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    //   FileInfo fi = (FileInfo)fsi;
                    UpfileCount += 1;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    string new_dir = di.FullName;
                    GetDirectoryCount(new_dir);
                }
            }
            return UpfileCount;
        }
        public List<FeedInfo> GetRss()
        {
            List<FeedInfo> listrss = new List<FeedInfo>();
            try
            {
                string rssurl = "http://www.jqpress.com/feed/post.aspx";
                XmlTextReader reader = new XmlTextReader(rssurl);
                DataSet ds = new DataSet();
                ds.ReadXml(reader);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[2];

                    foreach (DataRow dr in dt.Rows)
                    {
                        FeedInfo post = new FeedInfo();
                        post.title = dr["title"].ToString();
                        post.description = dr["description"].ToString();
                        post.link = dr["link"].ToString();
                        post.pubDate = dr["pubDate"].ToString();
                        listrss.Add(post);
                    }
                }

            }
            catch (Exception e) { }
            return listrss;

        }

        public string RssNews
        {
            get
            {
                string tmpl = string.Empty;
                var list = GetRss();
                for (int i = 0; i < (list.Count > 4 ? 4 : list.Count); i++)
                {
                    tmpl += "<a title=" + list[i].description + " href=" + list[i].link + " class=rsswidget>" + list[i].title + "</a>";
                    tmpl += "<span class=rss-date>" + Jqpress.Framework.Utils.DateTimeHelper.DateToChineseString(Convert.ToDateTime(list[i].pubDate)) + "</span>";
                    tmpl += "<div class=rssSummary>" + Jqpress.Framework.Utils.StringHelper.CutString(list[i].description, 0, 80) + " […]</div>";
                }
                return tmpl;
            }
        }
        public class FeedInfo
        {
            public string title { get; set; }
            public string link { get; set; }
            public string guid { get; set; }
            public string author { get; set; }
            public string category { get; set; }
            public string description { get; set; }
            public string pubDate { get; set; }
        }
        protected void btnCategory_Click(object sender, EventArgs e)
        {
            List<CategoryInfo> list = CategoryService.GetCategoryList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].PostCount = PostService.GetPostCount(list[i].CategoryId, -1, -1, (int)PostStatus.Published, 0);
                CategoryService.UpdateCategory(list[i]);
            }
            Response.Redirect("index.aspx?result=11");
        }

        protected void btnTag_Click(object sender, EventArgs e)
        {
            List<TagInfo> list = TagService.GetTagList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].PostCount = PostService.GetPostCount(-1, list[i].TagId, -1, (int)PostStatus.Published, 0);
                TagService.UpdateTag(list[i]);
            }
            Response.Redirect("index.aspx?result=12");
        }

        protected void btnUser_Click(object sender, EventArgs e)
        {

            List<UserInfo> list = UserService.GetUserList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].PostCount = PostService.GetPostCount(-1, -1, list[i].UserId, (int)PostStatus.Published, 0);
                list[i].CommentCount = CommentService.GetCommentCount(list[i].UserId, -1, false);
                UserService.UpdateUser(list[i]);
            }
            Response.Redirect("index.aspx?result=13");
        }
    }
}
