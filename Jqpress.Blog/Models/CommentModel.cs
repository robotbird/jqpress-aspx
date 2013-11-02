using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Services;
using Jqpress.Framework.Configuration;

namespace Jqpress.Blog.Models
{
    public class CommentModel
    {
        public CommentModel() 
        {
        }
        public List<CommentInfo> RecentComments { get { return CommentService.GetCommentListByRecent(BlogConfig.GetSetting().SidebarCommentCount); } }
    }
}
