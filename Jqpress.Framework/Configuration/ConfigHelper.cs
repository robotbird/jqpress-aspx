using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jqpress.Framework.Configuration
{
   public class ConfigHelper
    {
        private static string _siteprefix;

        /// <summary>
        /// Cache、Session、Cookies 等应用程序变量名前缀
        /// </summary>
        public static string SitePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_siteprefix))
                {
                    _siteprefix = GetValue("blog_siteprefix");
                }
                return _siteprefix;
            }
        }

        private static string _sitepath;

        /// <summary>
        /// 程序相对根路径
        /// </summary>
        public static string SitePath
        {
            get
            {
                if (string.IsNullOrEmpty(_sitepath))
                {
                    _sitepath = GetValue("blog_sitepath");
                    if (_sitepath!=null)
                    {
                        if (!_sitepath.EndsWith("/"))
                        {
                            _sitepath += "/";
                        }
                    }

                }

                return _sitepath;
            }
        }

        private static string _siteurl;

        /// <summary>
        /// 程序Url
        /// 未考虑https://;存在多个域名指向时，有BUG，因为静态变量已存在，如www.blog.com,blog.com
        /// </summary>
        public static string SiteUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_siteurl))
                {
                    _siteurl = "http://" + System.Web.HttpContext.Current.Request.Url.Host + SitePath;
                    if (System.Web.HttpContext.Current.Request.Url.Port != 80)
                    {
                        _siteurl = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + SitePath;
                    }
                }
                return _siteurl;
            }
        }

        private static string _dbconnection;

        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string DbConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_dbconnection))
                {
                    _dbconnection = GetValue("blog_dbconnection");
                }
                return _dbconnection;
            }
        }

        private static string _dbType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DbType
        {
            get
            {
                if (string.IsNullOrEmpty(_dbType))
                {
                    _dbType = GetValue("DbType");
                }
                return _dbType;
            }
        }

       private static string _tableprefix;
       /// <summary>
       /// 表前缀
       /// </summary>
       public static string Tableprefix
       {
           get
           {
               if (string.IsNullOrEmpty(_tableprefix))
               {
                   _tableprefix = GetValue("Tableprefix");
               }
               return _tableprefix;
           }
       }

        ///// <summary>
        ///// 数据库类型
        ///// </summary>
        //public static string DbType
        //{
        //    get { return GetValue("blog_dbtype"); }
        //}

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];

        }
    }
}
