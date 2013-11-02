using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Jqpress.Blog.Common;

namespace Jqpress.Web.admin.blog
{
    public partial class file_list :AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("附件管理");
        }
    }
}
