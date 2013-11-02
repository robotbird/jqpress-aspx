using System;
using Jqpress.Blog;
namespace Jqpress.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 301跳转用
            string url = Jqpress.Framework.Web.PressRequest.GetUrl();
            if (url.IndexOf("http://www.") < 0 && url.IndexOf("localhost") < 0)
            {
                if (url.ToLower() == "http://jqpress.com/default.aspx")
                {
                    Jqpress.Framework.Web.HttpHelper.Redirect301("http://www.jqpress.com");
                }
                else
                {
                    Jqpress.Framework.Web.HttpHelper.Redirect301("http://www.jqpress.com/" + Request.RawUrl);
                }
            }
            #endregion
            new BlogController(Framework.Web.PressRequest.GetQueryString("type", true));
        }
    }
}
