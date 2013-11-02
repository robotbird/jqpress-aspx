using System;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Net.Mail;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;

namespace Jqpress.Web.admin.blog
{
    public partial class blog_set : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("网站设置");

            if (!IsPostBack)
            {
                BindSetting();
            }
            Page.MaintainScrollPositionOnPostBack = true;
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
                case 2:
                    ShowMessage("保存成功!");
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 绑定
        /// </summary>
        protected void BindSetting()
        {
            LoadDefaultData();

            BlogConfigInfo s = BlogConfig.GetSetting();
            if (s != null)
            {
                txtSiteName.Text = HttpHelper.HtmlDecode(s.SiteName);
                txtSiteDescription.Text = HttpHelper.HtmlDecode(s.SiteDescription);
                txtMetaKeywords.Text = HttpHelper.HtmlDecode(s.MetaKeywords);
                txtMetaDescription.Text = HttpHelper.HtmlDecode(s.MetaDescription);

                chkSiteStatus.Checked = s.SiteStatus == 1 ? true : false;


                chkEnableVerifyCode.Checked = s.EnableVerifyCode == 1 ? true : false;


                txtSidebarPostCount.Text = s.SidebarPostCount.ToString();
                txtSidebarCommentCount.Text = s.SidebarCommentCount.ToString();
                txtSidebarTagCount.Text = s.SidebarTagCount.ToString();

                txtPageSizeCommentCount.Text = s.PageSizeCommentCount.ToString();
                txtPageSizePostCount.Text = s.PageSizePostCount.ToString();
                //    txtPageSizeTagCount.Text = s.PageSizeTagCount.ToString();

                txtPostRelatedCount.Text = s.PostRelatedCount.ToString();

                txtFooterHtml.Text = s.FooterHtml;

                // ddlRewriteExtension.SelectedValue = s.RewriteExtension;

                //chkCommentApproved.Checked = s.CommentApproved == 1 ? true : false;

                //水印
                ddlWatermarkType.SelectedValue = s.WatermarkType.ToString();
                txtWatermarkText.Text = s.WatermarkText;
                ddlWatermarkFontName.SelectedValue = s.WatermarkFontName;
                ddlWatermarkFontSize.SelectedValue = s.WatermarkFontSize.ToString();
                txtWatermarkImage.Text = s.WatermarkImage;
                ddlWatermarkTransparency.SelectedValue = s.WatermarkTransparency.ToString();
                ddlWatermarkPosition.SelectedValue = s.WatermarkPosition.ToString();
                ddlWatermarkQuality.SelectedValue = s.WatermarkQuality.ToString();

                //评论
                chkCommentStatus.Checked = s.CommentStatus == 1 ? true : false;
                ddlCommentOrder.SelectedValue = s.CommentOrder.ToString();
                ddlCommentApproved.SelectedValue = s.CommentApproved.ToString();
                txtCommentSpamwords.Text = s.CommentSpamwords;

                //rss
                chkRssStatus.Checked = s.RssStatus == 1 ? true : false;
                txtRssRowCount.Text = s.RssRowCount.ToString();
                ddlRssShowType.SelectedValue = s.RssShowType.ToString();

                //rewrite
                ddlUrlType.Items.Clear();
                ddlUrlType.Items.Add(new ListItem(Jqpress.Framework.Configuration.ConfigHelper.SiteUrl + "post/" + DateTime.Now.ToString(@"yyyy\/MM\/dd") + "/slug" + setting.RewriteExtension, "1"));
                ddlUrlType.Items.Add(new ListItem(Jqpress.Framework.Configuration.ConfigHelper.SiteUrl + "post/slug" + setting.RewriteExtension, "2"));
                ddlUrlType.SelectedValue = s.UrlFormatType.ToString();
                ddlRewriteExtension.SelectedValue = s.RewriteExtension;

                //total
                ddlTotalType.SelectedValue = s.SiteTotalType.ToString();

                ddlPostShowType.SelectedValue = s.PostShowType.ToString();

                //邮件
                txtSmtpEmail.Text = s.SmtpEmail;
                txtSmtpServer.Text = s.SmtpServer;
                txtSmtpServerPort.Text = s.SmtpServerPost.ToString();
                txtSmtpUserName.Text = s.SmtpUserName;
                txtSmtpPassword.Text = s.SmtpPassword;
                chkSmtpEnableSsl.Checked = s.SmtpEnableSsl == 1 ? true : false;

                //发送邮件设置
                chkSendMailAuthorByPost.Checked = s.SendMailAuthorByPost == 1 ? true : false;
                chkSendMailAuthorByComment.Checked = s.SendMailAuthorByComment == 1 ? true : false;
                chkSendMailNotifyByComment.Checked = s.SendMailNotifyByComment == 1 ? true : false;


            }

        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        private void LoadDefaultData()
        {

            ddlWatermarkFontName.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                ddlWatermarkFontName.Items.Add(new ListItem(family.Name, family.Name));
            }

            ddlWatermarkQuality.Items.Clear();
            for (int i = 100; i >= 0; i--)
            {
                string text = i.ToString();
                if (i == 100)
                {
                    text += "(最高)";
                }
                if (i == 80)
                {
                    text += "(推荐)";
                }
                ddlWatermarkQuality.Items.Add(new ListItem(text, i.ToString()));

            }

            ddlWatermarkTransparency.Items.Clear();
            for (int i = 10; i > 0; i--)
            {
                string text = i.ToString();
                if (i == 10)
                {
                    text += "(不透明)";
                }

                ddlWatermarkTransparency.Items.Add(new ListItem(text, i.ToString()));

            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            BlogConfigInfo s = BlogConfig.GetSetting();
            if (s != null)
            {
                s.SiteName = HttpHelper.HtmlEncode(txtSiteName.Text);
                s.SiteDescription = HttpHelper.HtmlEncode(txtSiteDescription.Text);
                s.MetaKeywords = HttpHelper.HtmlEncode(txtMetaKeywords.Text);
                s.MetaDescription = HttpHelper.HtmlEncode(txtMetaDescription.Text);

                //   s.RewriteExtension = ddlRewriteExtension.SelectedValue;
                s.SiteStatus = chkSiteStatus.Checked ? 1 : 0;

                //  s.CommentApproved = chkCommentApproved.Checked ? 1 : 0;
                //    //c.GuestBookVerifyStatus = chkGuestBookVerifyStatus.Checked ? 1 : 0;
                s.EnableVerifyCode = chkEnableVerifyCode.Checked ? 1 : 0;


                
                //  s.PageSizeTagCount = TypeConverter.StrToInt(txtPageSizeTagCount.Text, 10);
                s.PageSizePostCount = TypeConverter.StrToInt(txtPageSizePostCount.Text, 10);
                s.PageSizeCommentCount = TypeConverter.StrToInt(txtPageSizeCommentCount.Text, 50);

                s.SidebarPostCount = TypeConverter.StrToInt(txtSidebarPostCount.Text, 10);
                s.SidebarTagCount = TypeConverter.StrToInt(txtSidebarTagCount.Text, 10);
                s.SidebarCommentCount = TypeConverter.StrToInt(txtSidebarCommentCount.Text, 10);

                //    //c.TagArticleNum = TypeConverter.StrToInt(txtTagArticleNum.Text, 10);

                s.FooterHtml = txtFooterHtml.Text;

                //    //c.ArticleShowType = TypeConverter.StrToInt(ddlArticleShowType.SelectedValue, 0);


                //水印

                s.WatermarkType = TypeConverter.StrToInt(ddlWatermarkType.SelectedValue, 1);
                s.WatermarkText = txtWatermarkText.Text;
                s.WatermarkFontName = ddlWatermarkFontName.SelectedValue;
                s.WatermarkFontSize = TypeConverter.StrToInt(ddlWatermarkFontSize.SelectedValue, 14);
                s.WatermarkImage = txtWatermarkImage.Text;
                s.WatermarkTransparency = TypeConverter.StrToInt(ddlWatermarkTransparency.SelectedValue, 10);
                s.WatermarkPosition = TypeConverter.StrToInt(ddlWatermarkPosition.SelectedValue, 1);
                s.WatermarkQuality = TypeConverter.StrToInt(ddlWatermarkQuality.SelectedValue, 100);


                //评论
                s.CommentStatus = chkCommentStatus.Checked ? 1 : 0;
                s.CommentOrder = TypeConverter.StrToInt(ddlCommentOrder.SelectedValue, 1);
                s.CommentApproved = TypeConverter.StrToInt(ddlCommentApproved.SelectedValue, 1);
                s.CommentSpamwords = txtCommentSpamwords.Text;

                //rss
                s.RssStatus = chkRssStatus.Checked ? 1 : 0;
                s.RssRowCount = TypeConverter.StrToInt(txtRssRowCount.Text, 20);
                s.RssShowType = TypeConverter.StrToInt(ddlRssShowType.SelectedValue, 2);

                //rewrite
                s.RewriteExtension = ddlRewriteExtension.SelectedValue;
                s.UrlFormatType = TypeConverter.StrToInt(ddlUrlType.SelectedValue,1);

                //total
                s.SiteTotalType = TypeConverter.StrToInt(ddlTotalType.SelectedValue, 1);

                s.PostShowType = TypeConverter.StrToInt(ddlPostShowType.SelectedValue, 2);


                //邮件
                s.SmtpEmail = txtSmtpEmail.Text.Trim();
                s.SmtpServer = txtSmtpServer.Text.Trim();
                s.SmtpServerPost = TypeConverter.StrToInt(txtSmtpServerPort.Text, 25);
                s.SmtpUserName = txtSmtpUserName.Text.Trim();
                s.SmtpPassword = txtSmtpPassword.Text.Trim();
                s.SmtpEnableSsl = chkSmtpEnableSsl.Checked == true ? 1 : 0;

                //发送邮件设置
                s.SendMailAuthorByPost = chkSendMailAuthorByPost.Checked == true ? 1 : 0;
                s.SendMailAuthorByComment = chkSendMailAuthorByComment.Checked == true ? 1 : 0;
                s.SendMailNotifyByComment = chkSendMailNotifyByComment.Checked == true ? 1 : 0;

                if (BlogConfig.UpdateSetting())
                {
                    Response.Redirect("blog_set.aspx?result=2");
                }
            }
        }

        /// <summary>
        /// 测试邮箱设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTestSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Framework.Utils.Validate.IsValidEmail(txtTestEmail.Text))
                {
                    ltTestSendMessage.Text = "<span class=\"m_error\" >请输入正确的测试邮箱!</span>";
                    ShowError("请输入正确的测试邮箱!");
                    return;
                }
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(txtSmtpEmail.Text, txtSiteName.Text);
                mail.To.Add(txtTestEmail.Text);
                mail.Subject = "这封邮件来自" + txtSiteName.Text;
                mail.Body = "测试邮件发送成功!";
                SmtpClient smtp = new SmtpClient(txtSmtpServer.Text);
                smtp.Credentials = new System.Net.NetworkCredential(txtSmtpUserName.Text, txtSmtpPassword.Text);
                smtp.EnableSsl = chkSmtpEnableSsl.Checked;
                smtp.Port = int.Parse(txtSmtpServerPort.Text, CultureInfo.InvariantCulture);
                smtp.Send(mail);
                ltTestSendMessage.Text = "<span class=\"m_pass\" >发送成功!</span>";
                ShowMessage("发送成功!");
            }
            catch (Exception ex)
            {
                ltTestSendMessage.Text = string.Format("<span class=\"m_error\" >发送失败!{0}</span>", ex.Message);
                ShowError("发送失败!");
            }
        }
    }
}
