using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;

namespace Jqpress.Web.admin
{
    public partial class _default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsLogin)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
                Response.End();
            }
            else
            {
                HttpContext.Current.Response.Redirect("blog/index.aspx");
            }
        }
    }
}
