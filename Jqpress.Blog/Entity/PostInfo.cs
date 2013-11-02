using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Services;
using Jqpress.Framework.Configuration;
using Jqpress.Framework.Utils;

namespace Jqpress.Blog.Entity
{
    /// <summary>
    /// 文章实体
    /// 是否要加个排序字段
    /// 是否要加个图片URL字段
    /// </summary>
    public class PostInfo : IComparable<PostInfo>
    {
        public int CompareTo(PostInfo other)
        {

            return other.PostId.CompareTo(this.PostId);
        }
 
        private int _urltype;
        private int _commentstatus;
        private string _template = "post.html";


        #region 非字段

        private string _url;
        private string _link;
        private string _detail;
        private UserInfo _user;
        private CategoryInfo _category;
        private List<TagInfo> _tags;
        private List<PostInfo> _relatedposts;

        /// <summary>
        /// 内容地址
        /// </summary>
        public string Url
        {
            get
            {
                string url = string.Empty;
                switch ((PostUrlFormat)this.UrlFormat)
                {

                    case PostUrlFormat.Slug:
                        url = string.Format("{0}post/{1}{2}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? Jqpress.Framework.Web.HttpHelper.UrlEncode(this.Slug) : this.PostId.ToString(), BlogConfig.GetSetting().RewriteExtension);
                        break;

                    case PostUrlFormat.Date:
                    default:
                        url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.SiteUrl, this.PostTime.ToString(@"yyyy\/MM\/dd"), !string.IsNullOrEmpty(this.Slug) ? Jqpress.Framework.Web.HttpHelper.UrlEncode(this.Slug) : this.PostId.ToString(), BlogConfig.GetSetting().RewriteExtension);
                        break;
                }
                return url;
            }
        }

        /// <summary>
        /// 分页URL
        /// </summary>
        public string PageUrl
        {
            get
            {
                string url = string.Empty;
                switch ((PostUrlFormat)this.UrlFormat)
                {

                    case PostUrlFormat.Slug:

                        url = string.Format("{0}post/{1}/page/{2}{3}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? Jqpress.Framework.Web.HttpHelper.UrlEncode(this.Slug) : this.PostId.ToString(), "{0}", BlogConfig.GetSetting().RewriteExtension);
                        break;

                    case PostUrlFormat.Date:
                    default:
                        url = string.Format("{0}post/{1}/{2}/page/{3}{4}", ConfigHelper.SiteUrl, this.PostTime.ToString(@"yyyy\/MM\/dd"), !string.IsNullOrEmpty(this.Slug) ? Jqpress.Framework.Web.HttpHelper.UrlEncode(this.Slug) : this.PostId.ToString(), "{0}", BlogConfig.GetSetting().RewriteExtension);
                        break;
                }
                return url;
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\">{1}</a>", this.Url, this.Title); }
        }

        /// <summary>
        /// 订阅URL
        /// </summary>
        public string FeedUrl
        {
            get
            {
                return string.Format("{0}feed/comment/postid/{1}{2}", ConfigHelper.SiteUrl, this.PostId, BlogConfig.GetSetting().RewriteExtension);
            }
        }

        /// <summary>
        /// 订阅连接
        /// </summary>
        public string FeedLink
        {
            get
            {
                return string.Format("<a href=\"{0}\" title=\"订阅:{1} 的评论\">订阅</a>", this.FeedUrl, this.Title);
            }
        }

        /// <summary>
        /// 文章 详情
        /// 根据设置而定,可为摘要,正文
        /// </summary>
        public string Detail
        {
            get
            {
                switch (BlogConfig.GetSetting().PostShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return this.Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(this.PostContent), 0, 200);
                    case 4:
                        return this.PostContent;
                }
            }
        }

        /// <summary>
        /// Rss 详情
        /// 根据设置而定,可为摘要,正文,前200字,空等
        /// </summary>
        public string FeedDetail
        {
            get
            {
                switch (BlogConfig.GetSetting().RssShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return this.Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(this.PostContent), 0, 200);
                    case 4:
                        return this.PostContent;
                }
            }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public UserInfo Author
        {
            get
            {
                UserInfo user = Jqpress.Blog.Services.UserService.GetUser(this.UserId);
                if (user != null)
                {
                    return user;
                }
                return new UserInfo();

            }
        }

        /// <summary>
        /// 所属分类
        /// </summary>
        public CategoryInfo Category
        {
            get
            {
                CategoryInfo category = Jqpress.Blog.Services.CategoryService.GetCategory(this.CategoryId);
                if (category != null)
                {
                    return category;
                }
                return new CategoryInfo();
            }
        }

        /// <summary>
        /// 对应标签
        /// </summary>
        public List<TagInfo> Tags
        {
            get
            {
                string temptags = this.Tag.Replace("{", "").Replace("}", ",");

                if (temptags.Length > 0)
                {
                    temptags = temptags.TrimEnd(',');
                }
                return TagService.GetTagList(temptags);
            }
        }

        /// <summary>
        /// 下一篇文章
        /// </summary>
        public PostInfo Next
        {
            get
            {
                List<PostInfo> list = PostService.GetPostList();
                PostInfo post = list.Find(p => p.HideStatus == 0 && p.Status == 1 && p.PostId > this.PostId);
                return post != null ? post : new PostInfo();
            }
        }

        /// <summary>
        /// 上一篇文章
        /// </summary>
        public PostInfo Previous
        {
            get
            {
                
                List<PostInfo> list = PostService.GetPostList();

                PostInfo post = list.Find(p => p.HideStatus == 0 && p.Status == 1 && p.PostId < this.PostId);

                return post != null ? post : new PostInfo();
            }
        }

        /// <summary>
        /// 相关文章
        /// </summary>
        public List<PostInfo> RelatedPosts
        {
            get
            {
                if (string.IsNullOrEmpty(this.Tag))
                {
                    return new List<PostInfo>();
                }

                List<PostInfo> list = PostService.GetPostList().FindAll(p => p.HideStatus == 0 && p.Status == 1);
                string tags = this.Tag.Replace("}", "},");
                tags = tags.TrimEnd(',');

                string[] temparray = tags.Split(',');

                int num = 0;
                List<PostInfo> list2 = list.FindAll(p =>
                {
                    if (num >= BlogConfig.GetSetting().PostRelatedCount)
                    {
                        return false;
                    }

                    foreach (string tag in temparray)
                    {
                        if (p.Tag.IndexOf(tag) != -1 && p.PostId != this.PostId)
                        {
                            num++;
                            return true;
                        }
                    }
                    return false;

                });


                return list2;
            }
        }


        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        public string PostContent { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 是否允许评论
        /// </summary>
        public int CommentStatus
        {
            set { _commentstatus = value; }
            get
            {
                if (_commentstatus == 1 && BlogConfig.GetSetting().CommentStatus == 1)
                {
                    return 1;
                }
                return 0;

            }
        }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 点击数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// URL类型,见枚举PostUrlFormat
        /// </summary>
        public int UrlFormat
        {
            set { _urltype = value; }
            get { return Jqpress.Blog.Configuration.BlogConfig.GetSetting().UrlFormatType; }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public string Template 
        {
            get { return _template; }
            set { _template = value; } 
        }

        /// <summary>
        /// 推荐
        /// </summary>
        public int Recommend { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public int TopStatus { get; set; }
        /// <summary>
        /// 是否首页显示
        /// </summary>
        public int HomeStatus { get; set; }
        /// <summary>
        /// 隐藏于列表
        /// </summary>
        public int HideStatus { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime PostTime { get; set; }
        /// <summary>
        /// 时间
        /// </summary>更新
        public DateTime UpdateTime { get; set; }
    }
}
