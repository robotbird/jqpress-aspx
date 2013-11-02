using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Services;
using Jqpress.Framework.Configuration;

namespace Jqpress.Blog.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            sidebar = new SideBarModel();
            comment = new CommentModel();
        }

        #region 站点
        /// <summary>
        /// 站点路径
        /// </summary>
        public string SiteUrl { get { return ConfigHelper.SiteUrl; } }
        /// <summary>
        /// 站点路径
        /// </summary>
        public string SitePath { get { return ConfigHelper.SitePath; } }
        /// <summary>
        /// 主题名称
        /// </summary>
        public string ThemeName { get; set; }
        /// <summary>
        /// 样式路径
        /// </summary>
        public string ThemeUrl { get { return ConfigHelper.SiteUrl + "themes/" + ThemeName + "/"; } }
        /// <summary>
        /// 样式虚拟目录
        /// </summary>
        public string ThemePath { get; set; }
        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDescription { get { return BlogConfig.GetSetting().SiteDescription; } }
        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get { return BlogConfig.GetSetting().SiteName; }
        }
        #endregion

        /// <summary>
        /// 是否首页
        /// </summary>
        public int IsDefault { get; set; }
        /// <summary>
        /// 是否post
        /// </summary>
        public int IsPost { get; set; }
        /// <summary>
        /// rss路径
        /// </summary>
        public string FeedUrl { get { return ConfigHelper.SiteUrl + "feed/post" + BlogConfig.GetSetting().RewriteExtension; } }
        /// <summary>
        /// 评论rss路径
        /// </summary>
        public string FeedCommentUrl { get { return ConfigHelper.SiteUrl + "feed/comment" + BlogConfig.GetSetting().RewriteExtension; } }

        /// <summary>
        /// 当前url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 当前日期
        /// </summary>
        public string Date { get; set; }

        private string _searchkeyword;
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchKeyword 
        {
            get { return _searchkeyword ?? string.Empty; ; }
            set { _searchkeyword = value; }
        }

        /// <summary>
        /// 导航
        /// </summary>
        public List<LinkInfo> NavLinks { get { return LinkService.GetLinkList((int)LinkPosition.Navigation, 1); } }

        #region 头部
        /// <summary>
        /// 标题
        /// </summary>
        public string PageTitle { get; set; }

        
        /// <summary>
        /// 关键词
        /// </summary>
        private string _metaKeywords;
        public string MetaKeywords 
        {
            get { return _metaKeywords ?? (_metaKeywords = BlogConfig.GetSetting().MetaKeywords); }
            set { _metaKeywords = value; }
        }

        
        /// <summary>
        /// 描述
        /// </summary>
        private string _metaDescription;
        public string MetaDescription
        {
            get { return _metaDescription ?? (_metaDescription = BlogConfig.GetSetting().MetaDescription); }
            set { _metaDescription = value; }
        }
        /// <summary>
        /// 头部wlwmanifest
        /// </summary>
        public string Head
        {
            get
            {
                string headhtml = string.Empty;

                headhtml += string.Format("<meta name=\"generator\" content=\"jqpress {0}\" />\n", BlogConfig.GetSetting().Version);
                headhtml += "<meta name=\"author\" content=\"jqpress Team\" />\n";
                headhtml += string.Format("<meta name=\"copyright\" content=\"2011-{0} jqpress Team.\" />\n", DateTime.Now.Year);
                headhtml += string.Format("<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0}\"  href=\"{1}\"  />\n", BlogConfig.GetSetting().SiteName, ConfigHelper.SiteUrl + "feed/post" + BlogConfig.GetSetting().RewriteExtension);
                headhtml += string.Format("<link rel=\"EditURI\" type=\"application/rsd+xml\" title=\"RSD\" href=\"{0}xmlrpc/rsd.aspx\" />\n", ConfigHelper.SiteUrl);
                headhtml += string.Format("<link rel=\"wlwmanifest\" type=\"application/wlwmanifest+xml\" href=\"{0}xmlrpc/wlwmanifest.aspx\" />", ConfigHelper.SiteUrl);

                return headhtml;
            }
        }
        #endregion

        #region 底部
        /// <summary>
        /// 底部版权信息
        /// </summary>
        public string FooterHtml { get { return BlogConfig.GetSetting().FooterHtml; } }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        #endregion
        /// <summary>
        /// 侧栏
        /// </summary>
        public SideBarModel sidebar { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public CommentModel comment { get; set; }

    }
}
