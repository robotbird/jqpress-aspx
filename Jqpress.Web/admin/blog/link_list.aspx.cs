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
    public partial class link_list : AdminPage
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        protected int linkId = PressRequest.GetQueryInt("linkid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("链接管理");

            if (!IsPostBack)
            {
                BindLinkList();
            }
            if (Request["act"] == "add" || Request["act"] == "update")
            {
                SaveData(Request["act"]);
            }
            if (Request["act"] == "getjson")
            {
                GetLinkJson();
            }
            if (Request["act"] == "delete")
            {
                DeleteLink();
            }
        }

        /// <summary>
        /// ajax删除
        /// </summary>
        protected void DeleteLink()
        {
            LinkService.DeleteLink(Convert.ToInt32(Request["id"]));
            Response.Write("success");
            Response.End();
        }

        /// <summary>
        /// ajax获取json值
        /// </summary>
        protected void GetLinkJson()
        {
            LinkInfo link = LinkService.GetLink(Convert.ToInt32(Request["id"]));
            Dictionary<string, string> jsondic = new Dictionary<string, string>();
            jsondic.Add("LinkId",link.LinkId.ToString());
            jsondic.Add("LinkName", link.LinkName);
            jsondic.Add("LinkUrl", link.LinkUrl);
            jsondic.Add("Description", link.Description);
            jsondic.Add("SortNum", link.SortNum.ToString());
            chkStatus.Checked = link.Status == 1 ? true : false;
            chkPosition.Checked = link.Position == (int)LinkPosition.Navigation ? true : false;
            chkTarget.Checked = link.Target == "_blank" ? true : false;
            Response.Write(JsonHelper.DictionaryToJson(jsondic));
            Response.End();
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindLinkList()
        {
            List<LinkInfo> list = LinkService.GetLinkList();
            rptLink.DataSource = list;
            rptLink.DataBind();
        }
        /// <summary>
        /// ajax保存
        /// </summary>
        protected void SaveData(string act)
        {
            LinkInfo link = new LinkInfo();
            if (act == "update")
            {
                link = LinkService.GetLink(PressRequest.GetFormInt("hidLinkId", 0));
            }
            else
            {
                link.CreateTime = DateTime.Now;
                link.Type = 0;// (int)LinkType.Custom;
            }
            link.LinkName = HttpHelper.HtmlEncode(txtName.Text.Trim());
            link.LinkUrl = HttpHelper.HtmlEncode(txtLinkUrl.Text.Trim());
            link.Description = HttpHelper.HtmlEncode(txtDescription.Text);
            link.SortNum = TypeConverter.StrToInt(txtDisplayOrder.Text, 1000);
            link.Status = chkStatus.Checked ? 1 : 0;
            link.Position = chkPosition.Checked ? (int)LinkPosition.Navigation : (int)LinkPosition.General;
            link.Target = chkTarget.Checked ? "_blank" : "_self";

            if (link.LinkName == "")
            {
                return;
            }

            Dictionary<string, string> jsondic = new Dictionary<string, string>();

            jsondic.Add("LinkName", link.LinkName);
            jsondic.Add("LinkUrl", link.LinkUrl);
            jsondic.Add("Description", link.Description);
            jsondic.Add("SortNum", link.SortNum.ToString());
            jsondic.Add("Position", link.Position == (int)Jqpress.Blog.Entity.Enum.LinkPosition.Navigation ? "[导航]" : "");
            jsondic.Add("Status", link.Status == 0 ? "[隐藏]" : "");


            if (act == "update")//更新操作
            {
                jsondic.Add("CreateTime", link.CreateTime.ToShortDateString());
                jsondic.Add("LinkId", link.LinkId.ToString());

                LinkService.UpdateLink(link);
                Response.Write(JsonHelper.DictionaryToJson(jsondic));
            }
            else//添加操作
            {
                int LinkId = LinkService.InsertLink(link);
                jsondic.Add("LinkId", LinkId.ToString());
                jsondic.Add("CreateTime", DateTime.Now.ToShortDateString());
                Response.Write(JsonHelper.DictionaryToJson(jsondic));

            }
            Response.End();
        }
    }
}
