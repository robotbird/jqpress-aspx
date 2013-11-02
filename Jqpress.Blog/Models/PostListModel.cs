using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Entity;

namespace Jqpress.Blog.Models
{
    public class PostListModel:BaseModel
    {
        public PostListModel()
        {
            PostList = new List<PostInfo>();
        }
        /// <summary>
        /// 文章列表信息(作者,分类等)
        /// </summary>
        public string PostMessage { get; set; }
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<PostInfo> PostList { get; set; }
        /// <summary>
        /// 分页字符串
        /// </summary>
        public string Pager { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
    }
}
