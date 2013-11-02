using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;
using Jqpress.Blog.Configuration;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Entity.Enum;
using Jqpress.Blog.Services;
using Jqpress.Framework.Configuration;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;
using OperateType = Jqpress.Blog.Common.OperateType;

namespace Jqpress.Web.admin.blog
{

    public partial class post_edit : AdminPage
    {
        #region 变量
        /// <summary>
        /// ID
        /// </summary>
        protected int postId = PressRequest.GetQueryInt("postid", 0);

        /// <summary>
        /// 默认分类ID
        /// </summary>
        protected int categoryId = PressRequest.GetQueryInt("categoryid", 0);

        protected string headerTitle = "添加文章";

        /// <summary>
        /// 提示
        /// </summary>
        protected int message = PressRequest.GetQueryInt("message", 0);
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("添加文章");


            if (!IsPostBack)
            {
                LoadDefault();

                if (Operate == OperateType.Update)
                {
                    BindPost();
                    headerTitle = "修改文章";
                    btnEdit.Text = "修改";
                    SetPageTitle("修改文章");
                    switch (message)
                    {
                        case 1:
                            ShowMessage(string.Format("添加成功! <a href=\"{0}\">{0}</a>", PostService.GetPost(postId).Url));
                            break;
                        case 2:
                            ShowMessage(string.Format("修改成功! <a href=\"{0}\">{0}</a>", PostService.GetPost(postId).Url));
                            break;
                    }

                }
                //else if (Action == "delete")
                //{
                //    DeleteArticle();
                //}
                //else
                //{
                //    ddlCategory.SelectedValue = CategoryID.ToString();
                //}
            }
        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        protected void LoadDefault()
        {
            List<CategoryInfo> list = CategoryService.GetCategoryTreeList();
            if (list.Count == 0)
            {
                CategoryInfo c = new CategoryInfo();
                c.CateName = "默认分类";
                c.CreateTime = DateTime.Now;
                c.Description = "这是系统自动添加的默认分类";
                c.Slug = "default";
                c.SortNum = 1000;

                c.PostCount = 0;
                CategoryService.InsertCategory(c);
            }
            list = CategoryService.GetCategoryTreeList();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("无分类", "0"));
            foreach (CategoryInfo c in list)
            {
                ddlCategory.Items.Add(new ListItem(c.TreeChar + c.CateName + " (" + c.PostCount + ") ", c.CategoryId.ToString()));
            }

            //ddlUrlType.Items.Clear();
            //  ddlUrlFormat.Items.Add(new ListItem("默认(由程序决定)", "0"));
           // ddlUrlType.Items.Add(new ListItem(ConfigHelper.SiteUrl + "post/" + DateTime.Now.ToString(@"yyyy\/MM\/dd") + "/slug" + setting.RewriteExtension, "1"));
           // ddlUrlType.Items.Add(new ListItem(ConfigHelper.SiteUrl + "post/slug" + setting.RewriteExtension, "2"));

            //   ddlUrlType.Items.Add(new ListItem(ConfigHelper.AppUrl + "post/分类别名/别名或ID" + setting.RewriteExtension, "3"));

            ddlTemplate.Items.Clear();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Server.MapPath(ConfigHelper.SitePath + "themes/" + setting.Theme + "/template"));
            foreach (System.IO.FileInfo file in dir.GetFiles("post*", System.IO.SearchOption.TopDirectoryOnly))
            {
                ddlTemplate.Items.Add(new ListItem(file.Name));
            }
        }

        /// <summary>
        /// 绑定实体
        /// </summary>
        protected void BindPost()
        {
            PostInfo p = PostService.GetPost(postId);
            if (p != null)
            {
                txtTitle.Text = HttpHelper.HtmlDecode(p.Title);
                txtSummary.Text = p.Summary;
                txtContents.Text = p.PostContent;
                chkStatus.Checked = p.Status == 1 ? true : false;
                ddlCategory.SelectedValue = p.CategoryId.ToString();
                chkCommentStatus.Checked = p.CommentStatus == 1 ? true : false;

                txtCustomUrl.Text = HttpHelper.HtmlDecode(p.Slug);

                chkTopStatus.Checked = p.TopStatus == 1 ? true : false;
                chkHomeStatus.Checked = p.HomeStatus == 1 ? true : false;
                chkRecommend.Checked = p.Recommend == 1 ? true : false;
                chkHideStatus.Checked = p.HideStatus == 1 ? true : false;

                //ddlUrlType.SelectedValue = p.UrlFormat.ToString();
                ddlTemplate.SelectedValue = p.Template;

                //绑定标签,需改进
                string tag = p.Tag;
                tag = tag.Replace("{", "");
                string[] taglist = tag.Split('}');
                foreach (string tagId in taglist)
                {
                    TagInfo taginfo = TagService.GetTag(TypeConverter.StrToInt(tagId, 0));

                    //  string tagName = Tags.GetTagName(Convert.ToInt32(tagID));

                    if (taginfo != null)
                    {
                        txtTags.Text += taginfo.CateName + ",";
                    }
                }
                txtTags.Text = txtTags.Text.TrimEnd(',');

                if (p.UserId != CurrentUser.UserId && CurrentUser.UserType == (int)UserType.Administrator)
                {
                    Response.Redirect("post_list.aspx?result=444");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //老标签
            string oldTag = string.Empty;

            PostInfo p = new PostInfo();

            if (Operate == OperateType.Update)
            {
                p = PostService.GetPost(postId);
                oldTag = p.Tag;
            }
            else
            {
                p.CommentCount = 0;
                p.ViewCount = 0;
                p.PostTime = DateTime.Now;
                p.UserId = CurrentUserId;
            }

            p.CategoryId = TypeConverter.StrToInt(ddlCategory.SelectedValue, 0);
            p.CommentStatus = chkCommentStatus.Checked ? 1 : 0;
            p.PostContent = txtContents.Text;
            p.Slug = StringHelper.FilterSlug(txtCustomUrl.Text, "post", true);
            p.Status = chkStatus.Checked ? 1 : 0;
            p.TopStatus = chkTopStatus.Checked ? 1 : 0;
            p.HomeStatus = chkHomeStatus.Checked ? 1 : 0;
            p.HideStatus = chkHideStatus.Checked ? 1 : 0;
            p.Summary = txtSummary.Text;
            p.Tag = GetTagIdList(txtTags.Text.Trim());
            p.Title = HttpHelper.HtmlEncode(txtTitle.Text);
            //  p.TopStatus = chkTopStatus.Checked ? 1 : 0;

           // p.UrlFormat = TypeConverter.StrToInt(ddlUrlType.SelectedValue, 1);
            p.UrlFormat = setting.UrlFormatType;
            p.Template = ddlTemplate.SelectedValue;
            p.Recommend = chkRecommend.Checked ? 1 : 0;


            //  p.Type = 0;// (int)PostType.Article;
            p.UpdateTime = DateTime.Now;

            if (chkSaveImage.Checked)
            {
                p.PostContent = SaveRemoteImage(p.PostContent);
            }

            if (Operate == OperateType.Update)
            {
                PostService.UpdatePost(p);
                //  TagService.ResetTag(oldTag + p.Tag);
                //Response.Redirect("post_edit.aspx?operate=update&postid=" + postId + "&message=2");
                Response.Redirect("post_list.aspx");
            }
            else
            {
                p.PostId = PostService.InsertPost(p);

                SendEmail(p);

                // TagService.ResetTag(p.Tag);

                //Response.Redirect("post_edit.aspx?operate=update&postid=" + p.PostId + "&message=1");
                Response.Redirect("post_list.aspx");
            }
        }

        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="post"></param>
        private void SendEmail(PostInfo post)
        {
            if (BlogConfig.GetSetting().SendMailAuthorByPost == 1)
            {
                List<UserInfo> list = UserService.GetUserList();
                List<string> emailList = new List<string>();

                foreach (UserInfo user in list)
                {
                    if (!Jqpress.Framework.Utils.Validate.IsEmail(user.Email))
                    {
                        continue;
                    }
                    //自己不用发
                    if (CurrentUser.Email == user.Email)
                    {
                        continue;
                    }
                    //不重复发送
                    if (emailList.Contains(user.Email))
                    {
                        continue;
                    }
                    emailList.Add(user.Email);

                    string subject = string.Empty;
                    string body = string.Empty;

                    subject = string.Format("[新文章通知]{0}", post.Title);
                    body += string.Format("{0}有新文章了:<br/>", BlogConfig.GetSetting().SiteName);
                    body += "<hr/>";
                    body += "<br />标题: " + post.Link;
                    body += post.Detail;
                    body += "<hr/>";
                    body += "<br />作者: " + CurrentUser.Link;
                    body += "<br />时间: " + post.PostTime;
                    body += "<br />文章连接: " + post.Link;
                    body += "<br />注:系统自动通知邮件,不要回复。";

                    EmailHelper.SendAsync(user.Email, subject, body);
                }
            }
        }

        /// <summary>
        /// 由标签名称列表返回标签ID列表,带{},新标签自动添加
        /// </summary>
        /// <param name="tagNameList"></param>
        /// <returns></returns>
        protected string GetTagIdList(string tagNames)
        {
            if (string.IsNullOrEmpty(tagNames))
            {
                return string.Empty;
            }
            string tagIds = string.Empty;
            tagNames = tagNames.Replace("，", ",");

            string[] names = tagNames.Split(',');

            foreach (string n in names)
            {
                if (!string.IsNullOrEmpty(n))
                {
                    TagInfo t = TagService.GetTag(n);

                    if (t == null)
                    {
                        t = new TagInfo();

                        t.PostCount = 0;
                        t.CreateTime = DateTime.Now;
                        t.Description = n;
                        t.SortNum = 1000;
                        t.CateName = n;
                        t.Slug = HttpHelper.HtmlEncode(StringHelper.FilterSlug(n, "tag"));

                        t.TagId = TagService.InsertTag(t);
                    }
                    tagIds += "{" + t.TagId + "}";
                }
            }
            return tagIds;
        }

        /// <summary>
        /// 保存远程图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string SaveRemoteImage(string html)
        {
            string Reg = @"<img.*src=.?(http|https).+>";
            string currentHost = Request.Url.Host;
            // <img.*?src="(?<url>.*?)".*?>
            List<Uri> urlList = new List<Uri>();

            //获取图片URL地址
            foreach (Match m in Regex.Matches(html, Reg, RegexOptions.IgnoreCase | RegexOptions.Compiled))
            {
                //  Response.Write(m.Value + "||<br>");
                Regex reg = new Regex(@"src=('|"")?(http|https).+?('|""|>| )+?", RegexOptions.IgnoreCase); //空格的未考虑
                string imgUrl = reg.Match(m.Value).Value.Replace("src=", "").Replace("'", "").Replace("\"", "").Replace(@">", "");
                //  Response.Write(imgUrl +"<br>");
                Uri u = new Uri(imgUrl);
                if (u.Host != currentHost)
                {
                    urlList.Add(u);
                }
            }

            //去掉重复
            List<Uri> urlList2 = new List<Uri>();
            foreach (Uri u2 in urlList)
            {
                if (!urlList2.Contains(u2))
                {
                    urlList2.Add(u2);
                }
            }

            //保存
            WebClient wc = new WebClient();
            int i = 0;
            foreach (Uri u2 in urlList2)
            {
                i++;
                string extName = ".jpg";
                if (System.IO.Path.HasExtension(u2.AbsoluteUri))
                {
                    extName = System.IO.Path.GetExtension(u2.AbsoluteUri);
                    if (extName.IndexOf('?') >= 0)
                    {
                        extName = extName.Substring(0, extName.IndexOf('?'));
                    }
                }

                string path = ConfigHelper.SitePath + "upfiles/" + DateTime.Now.ToString("yyyyMM") + "/";


                if (!System.IO.Directory.Exists(Server.MapPath(path)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(path));
                }
                //  Response.Write(newDir);

                string newFileName = path + "auto_" + DateTime.Now.ToString("ddHHmmss") + i + extName;

                wc.DownloadFile(u2, Server.MapPath(newFileName)); //非图片后缀要改成图片后缀
                //  Response.Write(u2.AbsoluteUri + "||<br>");

                //是否合法
                if (IsAllowedImage(Server.MapPath(newFileName)))
                {
                    html = html.Replace(u2.AbsoluteUri, newFileName);
                }
                else
                {
                    System.IO.File.Delete(Server.MapPath(newFileName));
                }
            }
            return html;
        }

        /// <summary>
        /// 检查是否为允许的图片格式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsAllowedImage(string filePath)
        {
            bool ret = false;

            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();
            fs.Close();
            /*文件扩展名说明
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             */


            // String[] fileType = { "255216", "7173", "6677", "13780", "8297", "5549", "870", "87111", "8075" };
            string[] fileType = { "255216", "7173", "6677", "13780" };

            for (int i = 0; i < fileType.Length; i++)
            {
                if (fileclass == fileType[i])
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

    }
}
