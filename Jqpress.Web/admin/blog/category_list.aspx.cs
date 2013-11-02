using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jqpress.Blog.Common;
using Jqpress.Blog.Entity;
using Jqpress.Blog.Services;
using Jqpress.Framework.Utils;
using Jqpress.Framework.Web;


namespace Jqpress.Web.admin.blog
{
    public partial class category_list : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("分类管理");

            if (!IsPostBack)
            {
                BindCategoryList();
            }
            if (Request["act"] == "add" || Request["act"] == "update") 
            {
                SaveData(Request["act"]);
            }
            if (Request["act"]=="getjson")
            {
                GetCategoryJson();
            }
            if (Request["act"] == "delete")
            {
                Delete();
            }
        }


        #region  ajax操作
        /// <summary>
        /// ajax删除操作
        /// </summary>
        protected void Delete()
        {
            CategoryService.DeleteCategory(Convert.ToInt32(Request["id"]));
            Response.Write("success");
            Response.End();
        }
        /// <summary>
        /// ajax获取json值
        /// </summary>
        protected void GetCategoryJson() 
        {
            CategoryInfo term = CategoryService.GetCategory(Convert.ToInt32(Request["id"]));
            Dictionary<string, string> jsondic = new Dictionary<string, string>();

            jsondic.Add("CateName", term.CateName);
            jsondic.Add("Slug", term.Slug);
            jsondic.Add("SortNum", term.SortNum.ToString());
            jsondic.Add("Description", term.Description);
            jsondic.Add("CategoryId", term.CategoryId.ToString());
            jsondic.Add("ParentId", term.ParentId.ToString());
            Response.Write(JsonHelper.DictionaryToJson(jsondic));
            Response.End();
        }
        /// <summary>
        /// ajax保存
        /// </summary>
        protected void SaveData(string act)
        {
            CategoryInfo term = new CategoryInfo();
            if (act == "update")
            {
                term = CategoryService.GetCategory(PressRequest.GetFormInt("hidCategoryId", 0));
            }
            else
            {
                term.CreateTime = DateTime.Now;
                term.PostCount = 0;
            }
            term.CateName = HttpHelper.HtmlEncode(txtName.Text);
            term.ParentId =Convert.ToInt32( ddlCategory.SelectedValue);
            term.Slug = txtSlug.Text.Trim();
            if (string.IsNullOrEmpty(term.Slug))
            {
                term.Slug = term.CateName;
            }

            term.Slug = HttpHelper.HtmlEncode(StringHelper.FilterSlug(term.Slug, "cate"));

            term.Description = HttpHelper.HtmlEncode(txtDescription.Text);
            term.SortNum = TypeConverter.StrToInt(txtDisplayOrder.Text, 1000);

            if (term.CateName == "")
            {
                return;
            }
            Dictionary<string, string> jsondic = new Dictionary<string, string>();

            jsondic.Add("CateName", term.CateName);
            jsondic.Add("Url", term.Url);
            jsondic.Add("SortNum", term.SortNum.ToString());
            jsondic.Add("Description", term.Description);


            if (act == "update")//更新操作
            {
                jsondic.Add("Slug", term.Slug);
                jsondic.Add("PostCount", term.PostCount.ToString());
                jsondic.Add("CreateTime", term.CreateTime.ToShortDateString());
                jsondic.Add("CategoryId", term.CategoryId.ToString());
                jsondic.Add("ParentId", term.ParentId.ToString());
                jsondic.Add("TreeChar", CategoryService.GetCategoryTreeList().Find(c=>c.CategoryId==term.CategoryId).TreeChar);

                CategoryService.UpdateCategory(term);
                Response.Write(JsonHelper.DictionaryToJson(jsondic));
            }
            else//添加操作
            {
                int categoryid = CategoryService.InsertCategory(term);
                jsondic.Add("CategoryId", categoryid.ToString());
                jsondic.Add("PostCount", "0");
                jsondic.Add("CreateTime", DateTime.Now.ToShortDateString());
                jsondic.Add("ParentId", term.ParentId.ToString());
                jsondic.Add("TreeChar", CategoryService.GetCategoryTreeList().Find(c => c.CategoryId == categoryid).TreeChar);
                Response.Write(JsonHelper.DictionaryToJson(jsondic));

            }
            Response.End();
        }
        #endregion

        /// <summary>
        /// 初始化下拉菜单
        /// </summary>
        protected void InitddlCategory()
        {
            var treelist = CategoryService.GetCategoryTreeList();
            ddlCategory.Dispose();
            ddlCategory.Items.Add(new ListItem("作为一级栏目", "0"));
            foreach(var cate in treelist)
            {
                ddlCategory.Items.Add(new ListItem(cate.TreeChar+cate.CateName,cate.CategoryId.ToString()));            
            }
        
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindCategoryList()
        {
            InitddlCategory();
            rptCategory.DataSource = CategoryService.GetCategoryTreeList();
            rptCategory.DataBind();
        }


    }
}
