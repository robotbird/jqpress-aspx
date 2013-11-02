using System;
using System.Collections.Generic;
using Jqpress.Blog.Common;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Services;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;


namespace Jqpress.Web.admin.blog
{
    public partial class tag_list : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("标签管理");

            if (!IsPostBack)
            {
                BindTagList();
            }
            if (Request["act"] == "add" || Request["act"] == "update")
            {
                SaveData(Request["act"]);
            }
            if (Request["act"] == "getjson")
            {
                GetTagJson();
            }
            if (Request["act"] == "delete")
            {
                Delete();
            }
        }

        /// <summary>
        /// ajax删除操作
        /// </summary>
        protected void Delete()
        {
            TagService.DeleteTag(Convert.ToInt32(Request["id"]));
            Response.Write("success");
            Response.End();
        }
        /// <summary>
        /// ajax获取json值
        /// </summary>
        protected void GetTagJson()
        {
            TagInfo tag = TagService.GetTag(Convert.ToInt32(Request["id"]));
            Dictionary<string, string> jsondic = new Dictionary<string, string>();

            jsondic.Add("CateName", tag.CateName);
            jsondic.Add("SortNum", tag.SortNum.ToString());
            jsondic.Add("Description", tag.Description);
            jsondic.Add("TagId", tag.TagId.ToString());
            Response.Write(JsonHelper.DictionaryToJson(jsondic));
            Response.End();
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindTagList()
        {
            int recordCount = 0;

            List<TagInfo> list = TagService.GetTagList(Pager1.PageSize, Pager1.PageIndex, out recordCount);
            rptCategory.DataSource = list;
            rptCategory.DataBind();

            Pager1.RecordCount = recordCount;
        }

        /// <summary>
        /// ajax保存
        /// </summary>
        protected void SaveData(string act)
        {
            TagInfo tag = new TagInfo();
            if (act == "update")
            {
                tag = TagService.GetTag(PressRequest.GetFormInt("hidTagId", 0));
            }
            else
            {
                tag.CreateTime = DateTime.Now;
                tag.PostCount = 0;
            }

            tag.CateName = HttpHelper.HtmlEncode(txtName.Text);
            tag.Slug = StringHelper.FilterSlug(tag.CateName, "tag");
            tag.Description = HttpHelper.HtmlEncode(txtDescription.Text);
            tag.SortNum = TypeConverter.StrToInt(txtDisplayOrder.Text, 1000);

            if (tag.CateName == "")
            {
                return;
            }
            Dictionary<string, string> jsondic = new Dictionary<string, string>();

            jsondic.Add("CateName", tag.CateName);
            jsondic.Add("Url", tag.Url);
            jsondic.Add("SortNum", tag.SortNum.ToString());
            jsondic.Add("Description", tag.Description);


            if (act == "update")//更新操作
            {
                jsondic.Add("Slug", tag.Slug);
                jsondic.Add("PostCount", tag.PostCount.ToString());
                jsondic.Add("CreateTime", tag.CreateTime.ToShortDateString());
                jsondic.Add("TagId", tag.TagId.ToString());

                TagService.UpdateTag(tag);
                Response.Write(JsonHelper.DictionaryToJson(jsondic));
            }
            else//添加操作
            {
                int tagid = TagService.InsertTag(tag);
                jsondic.Add("TagId", tagid.ToString());
                jsondic.Add("PostCount", "0");
                jsondic.Add("CreateTime", DateTime.Now.ToShortDateString());
                Response.Write(JsonHelper.DictionaryToJson(jsondic));

            }
            Response.End();
        }
    }
}
