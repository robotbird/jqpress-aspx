using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;
using Jqpress.Blog.Entity;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;
using Jqpress.Blog.Services;

namespace Jqpress.Web.admin
{
    public partial class login :BasePage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public login() { }
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack) 
            {
               if(Jqpress.Framework.Web.PressRequest.GetQueryString("act")=="logout")
               {
                 Jqpress.Blog.Common.AdminPage.RemoveUserCookie();
                 Response.Redirect("../");
               }
            }

            if (Page.IsPostBack)
            {
                VerifyLogin();//对提供的信息进行验证
            }
            //else 
            //{
            //    Response.Redirect("login.aspx?result=4");
            //}
                
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        public void VerifyLogin()
        {
            UserInfo user = null;
            string userName = PressRequest.GetFormString("username");
            string password =Jqpress.Framework.Utils.EncryptHelper.MD5(PressRequest.GetFormString("password"));
            int expires = PressRequest.GetFormString("rememberme") == "forever" ? 43200 : 0;


              user = UserService.GetUser(userName, password);

            if (user != null)
            {
                if (user.Status == 0)
                {
                    Msg.Text = "此用户已停用!";
                    return;
                }
                WriteUserCookie(user.UserId, user.UserName, user.Password, expires);
                Response.Redirect("blog/index.aspx");
            }
            else
            {
                Msg.Text = "用户名或密码错误!";
            }
        }

    }
}
