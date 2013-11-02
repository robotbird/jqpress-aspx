using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Entity;
using System.Web;

namespace Jqpress.Blog.Configuration
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class BlogConfig
    {
        static string BlogConfigPath = HttpContext.Current.Server.MapPath("~/common/config/blog.config");
        /// <summary>
        /// 静态变量
        /// </summary>
        private static BlogConfigInfo _setting;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static BlogConfig()
        {
            LoadSetting();
        }

        /// <summary>
        /// 单例初始化
        /// </summary>
        public static void LoadSetting()
        {
            if (_setting == null)
            {
                lock (lockHelper)
                {
                    if (_setting == null)
                    {
                        object obj = Jqpress.Framework.Xml.SerializationHelper.Load(typeof(BlogConfigInfo), BlogConfigPath);
                        if (obj == null)
                        {
                            _setting= new BlogConfigInfo();
                        }

                        _setting= (BlogConfigInfo)obj;
                    }
                }
            }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static BlogConfigInfo GetSetting()
        {
            return _setting;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static bool UpdateSetting()
        {
           bool result= Jqpress.Framework.Xml.SerializationHelper.Save(_setting, BlogConfigPath);
           return result;
        }

    }
}
