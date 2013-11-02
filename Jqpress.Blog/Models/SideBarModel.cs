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
    public class SideBarModel
    {
        public SideBarModel() 
        {

        }
        /// <summary>
        /// 归档列表
        /// </summary>
        public List<ArchiveInfo> Archives { get { return ArchiveService.GetArchive(); } }
        /// <summary>
        /// 作者列表
        /// </summary>
        public List<UserInfo> Authors { get { return UserService.GetUserList().FindAll(delegate(UserInfo user) { return user.Status == 1; }); } }
        /// <summary>
        /// 分类列表
        /// </summary>
        public List<CategoryInfo> Categories { get { return CategoryService.GetCategoryList(); } }
        /// <summary>
        /// 最近信息列表
        /// </summary>
        public List<PostInfo> RecentPosts { get { return PostService.GetPostList(BlogConfig.GetSetting().SidebarPostCount, -1, -1, 1, 1, -1, 0); } }
        /// <summary>
        /// 标签
        /// </summary>
        public List<TagInfo> RecentTags { get { return TagService.GetTagList(BlogConfig.GetSetting().SidebarTagCount); } }
        /// <summary>
        /// 普通链接
        /// </summary>
        public List<LinkInfo> GeneralLinks { get { return LinkService.GetLinkList((int)LinkPosition.General, 1); } }
        /// <summary>
        /// 文章数
        /// </summary>
        public int PostCount {get{return StatisticsService.GetStatistics().PostCount;}}
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount{get{return StatisticsService.GetStatistics().CommentCount;}}
        /// <summary>
        /// 访问量
        /// </summary>
        public int ViewCount{get{return StatisticsService.GetStatistics().VisitCount;}}
        /// <summary>
        /// 作者文章统计
        /// </summary>
        public int AuthorCount{get{return UserService.GetUserList().FindAll(user => user.Status == 1).Count;}}

        
    }
}
