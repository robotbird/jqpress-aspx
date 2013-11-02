using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Jqpress.Blog.Common;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Models;
using Jqpress.Blog.Services;
using Jqpress.Framework.Configuration;
using Jqpress.Framework.Utils;
using Jqpress.Framework.ViewEngine.NVelocityEngine;

namespace Jqpress.Blog
{
    public class BlogController:BasePage
    {
        private string _listTemplate = "default.html";
        private string _postTemplate = "post.html";
        /// <summary>
        /// 默认主题
        /// </summary>
        private string _themeName = "default";
        private string templatePath = string.Empty;
        int categoryId = -1;
        int tagId = -1;
        int userId = -1;
        string begindate = string.Empty;
        string enddate = string.Empty;
        string keyword = string.Empty;
        string slug = string.Empty;
        int pageindex = -1;
        string url = string.Empty;
        /// <summary>
        /// 模板封装类
        /// </summary>
        private NVelocityHelper th = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action"></param>
        public BlogController(string action)
        {
            BlogInit(action);//初始化
            switch (action)
            {
                case "home":Home();break;
                case "category":Category();break;
                case "tag": Tag(); break;
                case "author":Author();break;
                case "search":Search();break;
                case "archive":Archive();break;
                case "post": Post(); break;
                case "feed":Feed();break;
                case "rsd": Rsd(); break;
                case "wlwmanifest": Wlwmanifest(); break;
                case "metaweblog": Metaweblog(); break;
                default:Home();break;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        void BlogInit(string action) 
        {
            if (action == "feed")
            {
                templatePath = HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + "common/config/");
            }
            else
            {
                templatePath = HttpContext.Current.Server.MapPath(string.Format("{0}/themes/default/template/", ConfigHelper.SitePath));
            }
            if (BlogConfig.GetSetting().SiteStatus == 0)
            {
                ResponseError("网站已关闭", "网站已关闭,请与站长联系!");
            }
           
            pageindex = Framework.Web.PressRequest.GetQueryInt("page", 1);
            slug = Framework.Web.PressRequest.GetQueryString("slug");

            UpdateViewCount();//更新访问量
            //主题处理
            _themeName = BlogConfig.GetSetting().Theme;
            string previewThemeName = Jqpress.Framework.Web.PressRequest.GetQueryString("theme", true);
            //if (Jqpress.Framework.Web.PressRequest.IsMobile)
            //{
            //    _themeName = BlogConfig.GetSetting().MobileTheme;
            //}
            if (!string.IsNullOrEmpty(previewThemeName))
            {
                _themeName = previewThemeName;
            }
            //非预览时
            if (!System.IO.Directory.Exists(templatePath) && string.IsNullOrEmpty(previewThemeName))
            {
                BlogConfigInfo s = BlogConfig.GetSetting();
                if (Jqpress.Framework.Web.PressRequest.IsMobile)
                {
                    s.MobileTheme = "default";
                }
                else
                {
                    s.Theme = "default";
                }
                _themeName = "default";

               // BlogConfig.UpdateSetting();

                templatePath = Server.MapPath(string.Format("{0}/themes/{1}/template/", ConfigHelper.SitePath, _themeName));
            }
            if (action != "feed")
            {
                templatePath = Server.MapPath(string.Format("{0}/themes/{1}/template/", ConfigHelper.SitePath, _themeName));            
            }
            th = new NVelocityHelper(templatePath);
        }
        /// <summary>
        /// 首页
        /// </summary>
        public void Home()
        {
            var model = new PostListModel();
            model.IsDefault = 1;
            model.ThemeName = _themeName;
            model.PostMessage = string.Empty;
            model.Url = ConfigHelper.SiteUrl + "page/{0}" + BlogConfig.GetSetting().RewriteExtension; ;
            int recordCount = 0;
            model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                     out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                     begindate, enddate, keyword);
            model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);
            th.Put("Model", model);

            Display(th,_listTemplate);
        }
        /// <summary>
        /// 分类
        /// </summary>
        public void Category()
        {
            var model = new PostListModel();

            CategoryInfo cate = CategoryService.GetCategory(slug);
            if (cate!=null)
            {
                categoryId = cate.CategoryId;
                model.MetaKeywords = cate.CateName;
                model.MetaDescription = cate.Description;
                model.PageTitle = cate.CateName;
                model.PostMessage = string.Format("<h2 class=\"post-message\">分类:{0}</h2>", cate.CateName);
                model.Url = ConfigHelper.SiteUrl + "category/" + HttpContext.Current.Server.UrlEncode(slug) + "/page/{0}" + BlogConfig.GetSetting().RewriteExtension;
                int recordCount = 0;
                model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                         out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                         begindate, enddate, keyword);
                model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);

            }
            model.IsDefault = 0;
            model.ThemeName = _themeName;
            th.Put("Model", model);

            Display(th, _listTemplate);
        }
        /// <summary>
        /// 标签
        /// </summary>
        public void Tag()
        {
            var model = new PostListModel();

            TagInfo tag = TagService.GetTagBySlug(slug);
            if (tag != null)
            {
                tagId = tag.TagId;
                model.MetaKeywords = tag.CateName;
                model.MetaDescription = tag.Description;
                model.PageTitle = tag.CateName;
                model.PostMessage = string.Format("<h2 class=\"post-message\">标签:{0}</h2>", tag.CateName);
                model.Url = ConfigHelper.SiteUrl + "tag/" + HttpContext.Current.Server.UrlEncode(slug) + "/page/{0}" + BlogConfig.GetSetting().RewriteExtension;
                int recordCount = 0;
                model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                         out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                         begindate, enddate, keyword);
                model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);

            }
            model.IsDefault = 0;
            model.ThemeName = _themeName;
            th.Put("Model", model);

            Display(th, _listTemplate);
        }
        /// <summary>
        /// 作者
        /// </summary>
        public void Author()
        {
            var model = new PostListModel();

            string  userName = Jqpress.Framework.Web.PressRequest.GetQueryString("username");
            UserInfo user = UserService.GetUser(userName);
            if (user != null)
            {
                userId = user.UserId;
                model.MetaKeywords = user.NickName;
                model.MetaDescription = user.Description;
                model.PageTitle = user.NickName;

                model.PostMessage = string.Format("<h2 class=\"post-message\">作者:{0}</h2>", user.NickName);
                model.Url = ConfigHelper.SiteUrl + "author/" + HttpContext.Current.Server.UrlEncode(userName) + "/page/{0}" + BlogConfig.GetSetting().RewriteExtension;
                int recordCount = 0;
                model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                         out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                         begindate, enddate, keyword);
                model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);
            }

            model.IsDefault = 0;
            model.ThemeName = _themeName;
            th.Put("Model", model);

            Display(th, _listTemplate);
        }
        /// <summary>
        /// 搜索
        /// </summary>
        public void Search()
        {
            var model = new PostListModel();
            keyword = StringHelper.CutString(StringHelper.SqlEncode(Jqpress.Framework.Web.PressRequest.GetQueryString("keyword")), 0, 50);
            model.MetaKeywords = keyword;
            model.MetaDescription = keyword;
            model.PageTitle = keyword;
            model.PostMessage = string.Format("<h2 class=\"post-message\">搜索:{0}</h2>", keyword);
            model.Url = ConfigHelper.SiteUrl + "search" + BlogConfig.GetSetting().RewriteExtension + "?keyword=" + HttpContext.Current.Server.UrlEncode(keyword) + "&page={0}";
            int recordCount = 0;
            model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                     out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                     begindate, enddate, keyword);
            model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);

            model.IsDefault = 0;
            model.ThemeName = _themeName;
            th.Put("Model", model);

            Display(th, _listTemplate);
        }
        /// <summary>
        /// 归档
        /// </summary>
        public  void Archive()
        {
            var model = new PostListModel();
            string year = Jqpress.Framework.Web.PressRequest.GetQueryString("year");
            string month = Jqpress.Framework.Web.PressRequest.GetQueryString("month");
            DateTime date = Convert.ToDateTime(year + "-" + month);
            begindate = date.ToString();
            enddate = date.AddMonths(1).ToString();

            model.MetaKeywords = "归档";
            model.MetaDescription = BlogConfig.GetSetting().SiteName + date.ToString("yyyy-MM") + "的归档";
            model.PageTitle = "归档:" + date.ToString("yyyy-MM");
            model.PostMessage = string.Format("<h2 class=\"post-message\">归档:{0}</h2>", date.ToString("yyyy-MM"));
            model.Url = ConfigHelper.SiteUrl + "archive/" + date.ToString("yyyyMM") + "/page/{0}" + BlogConfig.GetSetting().RewriteExtension;
            int recordCount = 0;
            model.PostList = PostService.GetPostList(BlogConfig.GetSetting().PageSizePostCount, pageindex,
                                                     out recordCount, categoryId, tagId, userId, -1, 1, -1, 0,
                                                     begindate, enddate, keyword);
            model.Pager = Pager.CreateHtml(BlogConfig.GetSetting().PageSizePostCount, recordCount, model.Url);

            model.IsDefault = 0;
            model.ThemeName = _themeName;
            th.Put("Model", model);

            Display(th, _listTemplate);
        }
        /// <summary>
        /// 文章
        /// </summary>
        public void Post() 
        {
            var model = new PostModel();
            model.IsPost = 1;
            PostInfo post = null;

            int postId = -1;
            string name = Jqpress.Framework.Web.PressRequest.GetQueryString("name");

            if (Jqpress.Framework.Utils.Validate.IsInt(name))
            {
                post = PostService.GetPost(Jqpress.Framework.Utils.TypeConverter.StrToInt(name, 0));
            }
            else
            {
                post = PostService.GetPost(StringHelper.SqlEncode(name));
            }

            if (post == null)
            {
                BasePage.ResponseError("文章未找到", "囧！没有找到此文章！", 404);
            }

            if (post.Status == (int)PostStatus.Draft)
            {
                BasePage.ResponseError("文章未发布", "囧！此文章未发布！");
            }

            string cookie = "isviewpost" + post.PostId;
            int isview = Jqpress.Framework.Utils.TypeConverter.StrToInt(Jqpress.Framework.Web.PressRequest.GetCookie(cookie), 0);
            //未访问或按刷新统计
            if (isview == 0 || BlogConfig.GetSetting().SiteTotalType == 1)
            {
                PostService.UpdatePostViewCount(post.PostId, 1);
            }
            //未访问
            if (isview == 0 && BlogConfig.GetSetting().SiteTotalType == 2)
            {
                Jqpress.Framework.Web.PressRequest.WriteCookie(cookie, "1", 1440);
            }

            model.Post=post;
            model.PageTitle=post.Title;

            string metaKeywords = string.Empty;
            foreach (TagInfo tag in post.Tags)
            {
                metaKeywords += tag.CateName + ",";
            }
            if (metaKeywords.Length > 0)
            {
                metaKeywords = metaKeywords.TrimEnd(',');
            }
            model.MetaKeywords=metaKeywords;

            string metaDescription = post.Summary;
            if (string.IsNullOrEmpty(post.Summary))
            {
                metaDescription = post.PostContent;
            }
            model.MetaDescription=StringHelper.CutString(StringHelper.RemoveHtml(metaDescription), 50).Replace("\n", "");

            int recordCount = 0;
            model.Comments=CommentService.GetCommentList(BlogConfig.GetSetting().PageSizeCommentCount, Pager.PageIndex, out recordCount, BlogConfig.GetSetting().CommentOrder, -1, post.PostId, 0, -1, -1, null);
            model.Pager=Pager.CreateHtml(BlogConfig.GetSetting().PageSizeCommentCount, recordCount, post.PageUrl + "#comments");

            //同时判断评论数是否一致
            if (recordCount != post.CommentCount)
            {
                post.CommentCount = recordCount;
                PostService.UpdatePost(post);
            }
            model.IsDefault = 0;
            model.ThemeName = _themeName;
            model.EnableVerifyCode = BlogConfig.GetSetting().EnableVerifyCode;
            th.Put("Model", model);
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(string.Format("{0}/themes/{1}/template/{2}", ConfigHelper.SitePath, _themeName, post.Template))))
            {
                 Display(th, post.Template);
            }
            else
            {
                Display(th, _postTemplate);
            }
           
        }
        /// <summary>
        /// 更新访问次数
        /// </summary>
        public static void UpdateViewCount()
        {
            string cookie = "isview";
            int isview = Jqpress.Framework.Utils.TypeConverter.StrToInt(Jqpress.Framework.Web.PressRequest.GetCookie(cookie), 0);
            //未访问或按刷新统计
            if (isview == 0 || BlogConfig.GetSetting().SiteTotalType == 1)
            {

                if (StatisticsService.GetStatistics() != null)
                {
                    StatisticsInfo stat = StatisticsService.GetStatistics();
                    stat.VisitCount += 1;
                    StatisticsService.UpdateStatistics();
                }

            }
            //未访问
            if (isview == 0 && BlogConfig.GetSetting().SiteTotalType == 2)
            {
                Jqpress.Framework.Web.PressRequest.WriteCookie(cookie, "1", 1440);
            }
        }

        /// <summary>
        /// 加载feed
        /// </summary>
        public  void Feed()
        {
            int categoryId = Jqpress.Framework.Web.PressRequest.GetQueryInt("categoryid", -1);
            int postId = Jqpress.Framework.Web.PressRequest.GetQueryInt("postid", -1);
            string action = Jqpress.Framework.Web.PressRequest.GetQueryString("action", true);

            //   HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/xml";
            if (BlogConfig.GetSetting().RssStatus == 1)
            {
                switch (action)
                {
                    case "comment":
                        List<CommentInfo> commentList = CommentService.GetCommentList(BlogConfig.GetSetting().RssRowCount, 1, -1, postId, 0, 1, -1, null);
                        PostInfo commentPost = PostService.GetPost(postId);
                        HttpContext.Current.Response.Write("<rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\">\r\n");
                        HttpContext.Current.Response.Write("    <channel>\r\n");
                        HttpContext.Current.Response.Write("        <title><![CDATA[" + (commentPost == null ? BlogConfig.GetSetting().SiteName : commentPost.Title) + "的评论]]></title>\r\n");
                        HttpContext.Current.Response.Write("        <link>" + (commentPost == null ? ConfigHelper.SiteUrl : commentPost.Url) + "</link>\r\n");
                        HttpContext.Current.Response.Write("        <description><![CDATA[" + BlogConfig.GetSetting().SiteDescription + "]]></description>\r\n");
                        HttpContext.Current.Response.Write("        <pubDate>" + DateTime.Now.ToString("r") + "</pubDate>\r\n");
                        HttpContext.Current.Response.Write("        <generator>jqpress</generator>\r\n");
                        HttpContext.Current.Response.Write("        <language>zh-cn</language>\r\n");
                        foreach (CommentInfo comment in commentList)
                        {
                            HttpContext.Current.Response.Write("        <item>\r\n");
                            HttpContext.Current.Response.Write("            <title><![CDATA[" + comment.Author + "对" + comment.Post.Title + "的评论]]></title>\r\n");
                            HttpContext.Current.Response.Write("            <link>" + comment.Url + "</link>\r\n");
                            HttpContext.Current.Response.Write("            <guid>" + comment.Url + "</guid>\r\n");
                            HttpContext.Current.Response.Write("            <author><![CDATA[" + comment.Author + "]]></author>\r\n");

                            HttpContext.Current.Response.Write(string.Format("          <description><![CDATA[{0}]]></description>\r\n", comment.Contents));
                            HttpContext.Current.Response.Write("            <pubDate>" + comment.CreateTime.ToString("r") + "</pubDate>\r\n");
                            HttpContext.Current.Response.Write("        </item>\r\n");
                        }
                        HttpContext.Current.Response.Write("    </channel>\r\n");
                        HttpContext.Current.Response.Write("</rss>\r\n");
                        break;
                    default:
                        var model = new PostListModel();
                        model.PostList = PostService.GetPostList(BlogConfig.GetSetting().RssRowCount, categoryId, -1, -1, 1, -1, 0);
                        th.Put("Model", model);
                        Display(th, "feed.config");
                        break;
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<rss>error</rss>\r\n");
            }
            //  HttpContext.Current.Response.End();
        }
        /// <summary>
        /// Metaweblog 接口操作
        /// </summary>
        public  void Metaweblog()
        {
            Jqpress.Blog.XmlRpc.MetaWeblogHelper mw = new Jqpress.Blog.XmlRpc.MetaWeblogHelper();
            mw.ProcessRequest(HttpContext.Current);
        }
        /// <summary>
        /// 加载WLW配置
        /// </summary>
        public  void Wlwmanifest()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
        + "<manifest xmlns=\"http://schemas.microsoft.com/wlw/manifest/weblog\">"
         + " <options>"
          + "  <clientType>Metaweblog</clientType>"
          + "  <supportsKeywords>Yes</supportsKeywords>"
          + "  <supportsNewCategories>Yes</supportsNewCategories>"
           + " <supportsMultipleCategories>No</supportsMultipleCategories>"
            + "<supportsNewCategoriesInline>Yes</supportsNewCategoriesInline>"
           + " <supportsCommentPolicy>Yes</supportsCommentPolicy>"
            + "<supportsSlug>Yes</supportsSlug>"
            + "<supportsExcerpt>Yes</supportsExcerpt>"
            + "<supportsGetTags>Yes</supportsGetTags>"
            + "<supportsPages>No</supportsPages>"
            + "<supportsAuthor>No</supportsAuthor>"
            + "<supportsCustomDate>No</supportsCustomDate>"
            + "<requiresHtmlTitles>No</requiresHtmlTitles>"
          + "</options>"
          + "<weblog>"
        + "    <ServiceName>jqpress</ServiceName>"


            //+ "<imageUrl>../common/images/logo.gif</imageUrl>"  //ICO?
                //+ "<watermarkImageUrl>../common/images/watermark/watermark.gif</watermarkImageUrl>" //LOGO


            + "<imageUrl>../common/images/wlw/icon.png</imageUrl>"  //ICO?
            + "<watermarkImageUrl>../common/images/wlw/watermark.png</watermarkImageUrl>" //LOGO  Live Writer version:14.0 , 83*83


            + "<homepageLinkText>查看网站</homepageLinkText>"
            + "<adminLinkText>管理网站</adminLinkText>"
            + "<adminUrl><![CDATA[{blog-homepage-url}admin]]></adminUrl>"
          + "</weblog>"
          + "<buttons>"
        + "    <button>"
              + "<id>1</id>"
              + "<text>网站预览</text>"
              + "<imageUrl>../common/images/wlw/sitepreview.png</imageUrl>"
              + "<contentUrl><![CDATA[{blog-homepage-url}]]></contentUrl>"
              + "<contentDisplaySize>980,550</contentDisplaySize>"
            + "</button>"
          + "</buttons>"
        + "</manifest>";

            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
        }
        /// <summary>
        /// Rsd
        /// from BlogEngine Source
        /// </summary>
        public  void Rsd()
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            using (XmlTextWriter rsd = new XmlTextWriter(HttpContext.Current.Response.OutputStream, Encoding.UTF8))
            {
                rsd.Formatting = Formatting.Indented;
                rsd.WriteStartDocument();

                // Rsd tag
                rsd.WriteStartElement("rsd");
                rsd.WriteAttributeString("version", "1.0");

                // Service 
                rsd.WriteStartElement("service");
                rsd.WriteElementString("engineName", "jqpress" + BlogConfig.GetSetting().Version);
                rsd.WriteElementString("engineLink", "http://www.jqpress.com");
                rsd.WriteElementString("homePageLink", ConfigHelper.SiteUrl);

                // APIs
                rsd.WriteStartElement("apis");

                // MetaWeblog
                rsd.WriteStartElement("api");
                rsd.WriteAttributeString("name", "MetaWeblog");
                rsd.WriteAttributeString("preferred", "true");
                rsd.WriteAttributeString("apiLink", ConfigHelper.SiteUrl + "xmlrpc/metaweblog.aspx");
                rsd.WriteAttributeString("blogID", "1");
                rsd.WriteEndElement();

                // WordPress
                rsd.WriteStartElement("api");
                rsd.WriteAttributeString("name", "WordPress");
                rsd.WriteAttributeString("preferred", "false");
                rsd.WriteAttributeString("apiLink", ConfigHelper.SiteUrl + "xmlrpc/metaweblog.aspx");
                rsd.WriteAttributeString("blogID", "1");
                rsd.WriteEndElement();

                // BlogML
                //rsd.WriteStartElement("api");
                //rsd.WriteAttributeString("name", "BlogML");
                //rsd.WriteAttributeString("preferred", "false");
                //rsd.WriteAttributeString("apiLink", Utils.AbsoluteWebRoot + "api/BlogImporter.asmx");
                //rsd.WriteAttributeString("blogID", Utils.AbsoluteWebRoot.ToString());
                //rsd.WriteEndElement();

                // End APIs
                rsd.WriteEndElement();

                // End Service
                rsd.WriteEndElement();

                // End Rsd
                rsd.WriteEndElement();

                rsd.WriteEndDocument();

            }
        }

        /// <summary>
        /// 显示模板
        /// </summary>
        /// <param name="templatFileName">模板文件名</param>
        public static void Display(NVelocityHelper th, string templateFile)
        {

            //全局
            th.Put("querycount", querycount);
            th.Put("processtime", DateTime.Now.Subtract(starttick).TotalMilliseconds / 1000);
            th.Put("sqlanalytical", SqlAnalytical);

            // HttpContext.Current.Response.Clear();
            //   HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.Write(th.BuildString(templateFile));

            //  HttpContext.Current.Response.End();
        }
    }
}
