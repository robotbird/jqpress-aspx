using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Jqpress.Blog.Entity;

namespace Jqpress.Blog.Services
{
 /// <summary>
    /// 主题管理
    /// </summary>
    public class ThemeService
    {
        public static ThemeInfo GetTemplate(string xmlPath)
        {
            ThemeInfo theme = new ThemeInfo();
            theme.Name = "";
            theme.Author = "";
            theme.PubDate = "";
            theme.Version = "";
            theme.Email = "";
            theme.SiteUrl = "";


            if (!System.IO.File.Exists(xmlPath + @"\theme.xml"))
            {
                return theme;

            }
            try
            {
                XmlDocument xml = new XmlDocument();

                xml.Load(xmlPath + @"\theme.xml");

                theme.Name = xml.SelectSingleNode("theme/name").InnerText;
                theme.Author = xml.SelectSingleNode("theme/author").InnerText;
                theme.Email = xml.SelectSingleNode("theme/email").InnerText;
                theme.SiteUrl = xml.SelectSingleNode("theme/siteurl").InnerText;
                theme.PubDate = xml.SelectSingleNode("theme/pubdate").InnerText;
                theme.Version = xml.SelectSingleNode("theme/version").InnerText;
                return theme;
            }
            catch
            {
                return theme;

            }

        }
    }
}
