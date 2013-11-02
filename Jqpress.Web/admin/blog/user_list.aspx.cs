using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Services;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;
using OperateType = Jqpress.Blog.Common.OperateType;

namespace Jqpress.Web.admin.blog
{
    public partial class user_list : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("用户管理");


            if (!IsPostBack)
            {
                BindUserList();
            }
            if (Request["act"] == "add" || Request["act"] == "update")
            {
                SaveData(Request["act"]);
            }
            if (Request["act"] == "getjson")
            {
                GetUserJson();
            }
            if (Request["act"] == "delete")
            {
                DeleteUser();
            }
        }


        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindUserList()
        {
            List<UserInfo> list = UserService.GetUserList();
            rptUser.DataSource = list;
            rptUser.DataBind();
        }
        /// <summary>
        /// ajax获取json值
        /// </summary>
        protected void GetUserJson()
        {
  
            Dictionary<string, string> jsondic = new Dictionary<string, string>();
            UserInfo u = UserService.GetUser(Convert.ToInt32(Request["id"]));
            if (u != null)
            {
                jsondic.Add("UserId",u.UserId.ToString());
                jsondic.Add("UserName",u.UserName);
                jsondic.Add("NickName", u.NickName);
                jsondic.Add("Email", u.Email);
                jsondic.Add("Status", u.Status.ToString());
                jsondic.Add("SortNum", u.SortNum.ToString());
                jsondic.Add("UserType", u.UserType.ToString());

            }
            if (u.UserId == CurrentUserId)
            {
                ddlUserType.Enabled = false;
                chkStatus.Enabled = false;
            }
            Response.Write(JsonHelper.DictionaryToJson(jsondic));
            Response.End();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        protected void DeleteUser()
        {
            if (Convert.ToInt32(Request["id"]) == CurrentUserId)
            {
                Response.Write("不能删除当前用户");
            }
            else
            {
                UserService.DeleteUser(Convert.ToInt32(Request["id"]));
            }
            Response.Write("success");
            Response.End();

        }

        protected void AjaxShowErr(string err)
        {
            Dictionary<string, string> jsondic = new Dictionary<string, string>();
            jsondic.Add("err", err);
            Response.Write(JsonHelper.DictionaryToJson(jsondic));
            Response.End();
            return;
        }

        protected string GetUserType(object userType)
        {
            if (!Jqpress.Framework.Utils.Validate.IsInt(userType.ToString()))
            {
                return "";
            }
            int type = Convert.ToInt32(userType);
            switch (type)
            {
                case (int)UserType.Administrator:
                    return "管理员";
                case (int)UserType.Author:
                    return "写作者";
                default:
                    return "未知身份";
            }
        }

        /// <summary>
        /// ajax保存
        /// </summary>
        protected void SaveData(string act)
        {
            Dictionary<string, string> jsondic = new Dictionary<string, string>();
            UserInfo u = new UserInfo();
            if (act == "update")
            {
                u = UserService.GetUser(PressRequest.GetFormInt("hidUserId", 0));
            }
            else
            {
                u.CommentCount = 0;
                u.CreateTime = DateTime.Now;
                u.PostCount = 0;
                u.UserName = HttpHelper.HtmlEncode(txtUserName.Text.Trim());
            }

            u.Email = HttpHelper.HtmlEncode(txtEmail.Text.Trim());
            u.SiteUrl = string.Empty;// HttpHelper.HtmlEncode(txtSiteUrl.Text.Trim());
            u.Status = chkStatus.Checked ? 1 : 0;
            u.Description = string.Empty;// StringHelper.TextToHtml(txtDescription.Text);
            u.UserType = TypeConverter.StrToInt(ddlUserType.SelectedValue, 0);
            u.NickName = HttpHelper.HtmlEncode(txtNickName.Text.Trim());
            u.AvatarUrl = string.Empty;
            u.SortNum = TypeConverter.StrToInt(txtSortNum.Text, 1000);

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                u.Password = EncryptHelper.MD5(txtPassword.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text != txtPassword2.Text)
            {
                AjaxShowErr("两次密码输入不相同!");
            }

            jsondic.Add("UserName", u.UserName);
            jsondic.Add("UserType", GetUserType(u.UserType));
            jsondic.Add("Link",u.Link);
            jsondic.Add("SortNum", u.SortNum.ToString());
            jsondic.Add("Status", u.Status.ToString());

            if (act == "update")//更新操作
            {
                
                jsondic.Add("PostCount", u.PostCount.ToString());
                jsondic.Add("CommentCount", u.CommentCount.ToString());
                jsondic.Add("CreateTime", u.CreateTime.ToShortDateString());
                jsondic.Add("UserId", u.UserId.ToString());

                UserService.UpdateUser(u);

                //  如果修改自己,更新COOKIE
                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && u.UserId == CurrentUserId)
                {
                    WriteUserCookie(u.UserId, u.UserName, u.Password, 0);
                }
                Response.Write(JsonHelper.DictionaryToJson(jsondic));
            }
            else//添加操作
            {
                #region 验证处理
                if (string.IsNullOrEmpty(u.UserName))
                {
                    AjaxShowErr("请输入登陆用户名!");
                }

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[A-Za-z0-9\u4e00-\u9fa5-]");
                if (!reg.IsMatch(u.UserName))
                {
                    AjaxShowErr("用户名限字母,数字,中文,连字符!");
                }
                if (Jqpress.Framework.Utils.Validate.IsInt(u.UserName))
                {
                    AjaxShowErr("用户名不能为全数字!");
                }

                if (string.IsNullOrEmpty(u.Password))
                {
                    AjaxShowErr("请输入密码!");
                }
                if (UserService.ExistsUserName(u.UserName))
                {
                    AjaxShowErr("该登陆用户名已存在,请换之");
                }
                #endregion

                int userid = UserService.InsertUser(u);
                jsondic.Add("UserId", userid.ToString());
                jsondic.Add("PostCount", "0");
                jsondic.Add("CommentCount", "0");
                jsondic.Add("CreateTime", DateTime.Now.ToShortDateString());
                Response.Write(JsonHelper.DictionaryToJson(jsondic));

            }
            Response.End();
        }
    }
}
