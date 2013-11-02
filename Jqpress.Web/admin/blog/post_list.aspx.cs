using System;
using System.Collections.Generic;
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
    public partial class post_list : AdminPage
    {
        /// <summary>
        /// postid
        /// </summary>
        protected int postId = PressRequest.GetQueryInt("postid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("文章管理");



            if (Operate == OperateType.Delete)
            {
                Delete();
            }
            if (PressRequest.GetQueryString("action") == "deletes")//ajax批量删除
            {
                Deletes();
            }

            if (!IsPostBack)
            {
                LoadDefaultData();

                BindPostList();
            }

            ShowResult();

        }

        /// <summary>
        /// 显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = PressRequest.GetQueryInt("result");
            switch (result)
            {

                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 444:
                    ShowMessage("权限不够!");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        protected void Delete()
        {
            PostInfo post = PostService.GetPost(postId);
            if (post == null)
            {
                return;
            }
            if (CurrentUser.UserType != (int)UserType.Administrator && CurrentUser.UserId != post.UserId)
            {
                Response.Redirect("post_list.aspx?result=444");
            }

            PostService.DeletePost(postId);

            Response.Redirect("post_list.aspx?result=3");
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        protected void Deletes()
        {
            if (CurrentUser.UserType != (int)UserType.Administrator)
            {
                Response.Write("没有权限");
            }
            else
            {
                string strid = PressRequest.GetQueryString("strid");
                if (strid.Length>0)
                {
                    foreach (string id in strid.Split(','))
                    {
                        if (id!="")
                        {
                            PostService.DeletePost(Convert.ToInt32(id));
                        }      
                    }
                }
                Response.Write("success");
            }
            Response.End();
        }

        protected string GetEditLink(object postId, object userId)
        {

            string t = " <a href=\"post_edit.aspx?operate=update&postid=" + postId + "\">编辑</a>";
            if (Convert.ToInt32(userId) == CurrentUser.UserId || CurrentUser.UserType == (int)UserType.Administrator)
            {
                return t;
            }
            return string.Empty;
        }

        protected string GetDeleteLink(object postId, object userId)
        {

            string t = " <a href=\"post_list.aspx?operate=delete&postid=" + postId + "\" onclick=\"return confirm('删除文章同时会删除该文章的相关评论,确定要删除吗?');\">删除</a>";
            if (Convert.ToInt32(userId) == CurrentUser.UserId || CurrentUser.UserType == (int)UserType.Administrator)
            {
                return t;
            }
            return string.Empty;
        }


        /// <summary>
        /// 加载默认数据
        /// </summary>
        private void LoadDefaultData()
        {

            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("不限", "-1"));
            foreach (CategoryInfo term in CategoryService.GetCategoryList())
            {
                ddlCategory.Items.Add(new ListItem(term.CateName, term.CategoryId.ToString()));
            }

            //ddlAuthor.Items.Clear();
           // ddlAuthor.Items.Add(new ListItem("不限", "-1"));
           // foreach (UserInfo user in UserService.GetUserList())
            //{
                //ddlAuthor.Items.Add(new ListItem(user.NickName, user.UserId.ToString()));
            //}
        }

        /// <summary>
        /// 绑定
        /// </summary>
        protected void BindPostList()
        {
            string keyword = StringHelper.SqlEncode(PressRequest.GetQueryString("keyword"));
            int categoryId = PressRequest.GetQueryInt("categoryid", -1);
            int userId = PressRequest.GetQueryInt("userid", -1);
            int recommend = PressRequest.GetQueryInt("recommend", -1);
            int hide = PressRequest.GetQueryInt("hide", -1);

           // txtKeyword.Text = keyword; 暂时注释
            ddlCategory.SelectedValue = categoryId.ToString();
            //ddlAuthor.SelectedValue = userId.ToString();
            //  chkRecommend.Checked = recommend == 1 ? true : false; 暂时注释
            //chkHideStatus.Checked = hide == 1 ? true : false; 暂时注释

            int totalRecord = 0;

            List<PostInfo> list = PostService.GetPostList(Pager1.PageSize, Pager1.PageIndex, out totalRecord, categoryId, -1, userId, recommend, -1, -1, hide, string.Empty, string.Empty, keyword);
            rptPost.DataSource = list;
            rptPost.DataBind();
            Pager1.RecordCount = totalRecord;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
             string keyword = HttpHelper.UrlEncode(txtKeyword.Value);
            int categoryId = TypeConverter.StrToInt(ddlCategory.SelectedValue, -1);
           // int userId = TypeConverter.StrToInt(ddlAuthor.SelectedValue, -1);
            // int recommend = chkRecommend.Checked ? 1 : -1; 暂时注释
            // int hide = chkHideStatus.Checked ? 1 : -1;  暂时注释

              Response.Redirect(string.Format("post_list.aspx?keyword={0}&categoryid={1}", keyword, categoryId));
        }
    }
}
