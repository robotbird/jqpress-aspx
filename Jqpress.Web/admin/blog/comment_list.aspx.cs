using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public partial class comment_list :  AdminPage
    {
        /// <summary>
        /// 审核
        /// </summary>
        int approved = PressRequest.GetQueryInt("approved", -1);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("评论管理");

            OperateComment();

            ShowResult();

            if (!IsPostBack)
            {
                BindCommentList();
            }

        }

        /// <summary>
        /// 审核,删除单条记录
        /// </summary>
        private void OperateComment()
        {
            int commentId = PressRequest.GetQueryInt("commentid", 0);
            if (Operate == OperateType.Delete)
            {
                CommentService.DeleteComment(commentId);

                Response.Redirect("comment_list.aspx?result=3&page=" + Pager1.PageIndex);
            }
            else if (Operate == OperateType.Update)
            {
                CommentInfo comment = CommentService.GetComment(commentId);
                if (comment != null)
                {
                    comment.Approved = (int)ApprovedStatus.Success;
                    CommentService.UpdateComment(comment);

                    Response.Redirect("comment_list.aspx?result=4&page=" + Pager1.PageIndex);
                }
            }
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
                case 4:
                    ShowMessage("审核成功!");
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 绑定
        /// </summary>
        protected void BindCommentList()
        {
            int totalRecord = 0;

            List<CommentInfo> list = CommentService.GetCommentList(Pager1.PageSize, Pager1.PageIndex, out totalRecord, 1, -1, -1, -1, approved, -1, string.Empty);
            rptComment.DataSource = list;
            rptComment.DataBind();
            Pager1.RecordCount = totalRecord;
        }

        /// <summary>
        /// 文章连接
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        protected string GetPostLink(int postId)
        {

            PostInfo post = PostService.GetPost(postId);
            if (post != null)
            {
                return string.Format(" 评: {0}", StringHelper.CutString(post.Title, 0,20));
            }
            return string.Empty;
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (RepeaterItem item in rptComment.Items)
            {
                HtmlInputCheckBox box = ((HtmlInputCheckBox)item.FindControl("chkRow"));
                if (box.Checked)
                {
                    int commentId = Convert.ToInt32(box.Value);
                    CommentService.DeleteComment(commentId);
                    i++;

                }
            }


            Response.Redirect("comment_list.aspx?result=3&page=" + Pager1.PageIndex + "&approved=" + approved);

        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (RepeaterItem item in rptComment.Items)
            {
                HtmlInputCheckBox box = ((HtmlInputCheckBox)item.FindControl("chkRow"));
                if (box.Checked)
                {
                    int commentID = Convert.ToInt32(box.Value);
                    CommentInfo c = CommentService.GetComment(commentID);
                    if (c != null)
                    {
                        c.Approved = (int)ApprovedStatus.Success;
                        if (CommentService.UpdateComment(c) > 0)
                        {
                            i++;
                        }
                    }
                }
            }
            Response.Redirect("comment_list.aspx?result=4&page=" + Pager1.PageIndex + "&approved=" + approved);
        }
    }
}
