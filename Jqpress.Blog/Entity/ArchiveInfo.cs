using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Configuration;
using Jqpress.Framework.Configuration;

namespace Jqpress.Blog.Entity
{
    /// <summary>
    /// 归档实体
    /// </summary>
    public class ArchiveInfo
    {
        /// <summary>
        /// 日期,用于拼Url
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url
        {
            get
            {
                return ConfigHelper.SiteUrl + "archive/" + this.Date.ToString("yyyyMM") + BlogConfig.GetSetting().RewriteExtension;
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public string Link
        {

            get
            {
                return string.Format("<a href=\"{0}\" >{1}</a>", this.Url, this.Date.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}
