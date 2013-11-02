<%@ Page Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Jqpress.Web.admin.blog.index" Title="无标题页" %>
<%@ Import Namespace="Jqpress.Blog.Entity" %>
<%@ Import Namespace="Jqpress.Blog.Services" %>
<%@ Import Namespace="Jqpress.Framework.Utils" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
p, dl, dd, dt {
    line-height: 140%;
}
p {
    margin: 1em 0;
}
ul li{list-style:none;}
.postbox p, .postbox ul, .postbox ol, .postbox blockquote, #wp-version-message {
    font-size: 13px;
}
#dashboard_right_now p.sub {
    font-size: 14px;
    font-style: normal;
}

#dashboard_right_now p.sub {
    color: #777777;
    font-size: 13px;
    left: 15px;
    padding: 5px 10px 15px;
    position: absolute;
    top: -17px;
}
    element.style {
    width: 49%;
}
.postbox-container {
    float: left;
    padding-right: 0.5%;
}

#dashboard_right_now .versions {
    clear: both;
    padding: 6px 10px 12px;
}
.widget, .postbox {
    background-color: #FFFFFF;
}
.postbox div{ margin:10px;}
.widget, #widget-list .widget-top, .postbox, #titlediv, #poststuff .postarea, .stuffbox {
    border-color: #DFDFDF;
}
.postbox {
    min-width: 255px;
    position: relative;
    width: 99.5%;
}

.widget, .postbox, .stuffbox {
    border-radius: 6px 6px 6px 6px;
    border-style: solid;
    border-width: 1px;
    line-height: 1;
    margin-bottom: 20px;
}

body, #wpbody, .form-table .pre {
    color: #333333;
}
.postbox .handlediv {
    float: right;
    height: 26px;
    width: 23px;
}
.widget, .postbox, .stuffbox {
    border-style: solid;
    line-height: 1;
}


.ui-sortable .postbox h3 {
    color: #464646;
}

.postbox .hndle {
    cursor: move;
}

.widget .widget-top, .postbox h3, .stuffbox h3 {
    background: url("../images/gray-grad.png") repeat-x scroll left top #DFDFDF;
    text-shadow: 0 1px 0 #FFFFFF;
}

#poststuff h3, .metabox-holder h3 {
    font-size: 12px;
    font-weight: bold;
    line-height: 1;
    margin: 0;
    padding: 7px 9px;
}

.widget .widget-top, .postbox h3, .postbox h3, .stuffbox h3 {
    border-radius: 6px 6px 0 0;
}
#dashboard_right_now .inside {
    font-size: 12px;
    padding-top: 20px;
}
div.postbox div.inside {
    margin: 10px;
    position: relative;
}
.inside div{margin: 10px;}
#dashboard_right_now .table_content {
    border-top: 1px solid #ECECEC;
    float: left;
    width: 45%;
}
#dashboard_right_now .table {
    margin: 0 -9px;
    padding: 0 10px;
    position: relative;
}
#dashboard_right_now .table_discussion {
    border-top: 1px solid #ECECEC;
    float: right;
    width: 45%;
}
#dashboard_right_now .table {
    margin: 0 -9px;
    padding: 0 10px;
    position: relative;
}
#dashboard_right_now .versions {
    clear: both;
    padding: 6px 10px 12px;
}
#dashboard_right_now p.sub, #dashboard_right_now .table, #dashboard_right_now .versions {
    margin: -12px;
}
.ui-sortable .postbox h3 {
    color: #464646;
}

.postbox .hndle {
    cursor: move;
}
.widget .widget-top, .postbox h3, .stuffbox h3 {
    background: url("../images/gray-grad.png") repeat-x scroll left top #DFDFDF;
    text-shadow: 0 1px 0 #FFFFFF;
}
#poststuff h3, .metabox-holder h3 {
    font-size: 12px;
    font-weight: bold;
    line-height: 1;
    margin: 0;
    padding: 7px 9px;
}
.widget .widget-top, .postbox h3, .postbox h3, .stuffbox h3 {
    border-radius: 6px 6px 0 0;
}
.rss-widget ul li {
    line-height: 1.5em;
    margin-bottom: 12px;
}
.textright {
    text-align: right;
}
#dashboard_right_now a.button {
    clear: right;
    float: right;
    position: relative;
    top: -5px;
}

#dashboard-widgets a {
    text-decoration: none;
}

a.button, a.button-primary, a.button-secondary {
    line-height: 15px;
    padding: 3px 10px;
    white-space: nowrap;
}

.submit input, .button, input.button, .button-primary, input.button-primary, .button-secondary, input.button-secondary, .button-highlighted, input.button-highlighted, #postcustomstuff .submit input {
    font-size: 12px !important;
}

.button, .submit input, .button-secondary {
    background: url("../images/white-grad.png") repeat-x scroll left top #F2F2F2;
    text-shadow: 0 1px 0 #FFFFFF;
}

.button, .button-secondary, .submit input, input[type="button"], input[type="submit"] {
    border-color: #BBBBBB;
    color: #464646;
}

.submit input, .button, input.button, .button-primary, input.button-primary, .button-secondary, input.button-secondary, .button-highlighted, input.button-highlighted, #postcustomstuff .submit input {
    -moz-box-sizing: content-box;
    border-radius: 11px 11px 11px 11px;
    border-style: solid;
    border-width: 1px;
    cursor: pointer;
    font-size: 11px !important;
    line-height: 13px;
    padding: 3px 8px;
    text-decoration: none;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="body_nav"><div class="left">控制台</div></div>


<div id="dashboard-widgets-wrap">

<div class="metabox-holder" id="dashboard-widgets">
	<div style="width:50%;" class="postbox-container">
<div class="meta-box-sortables ui-sortable" id="normal-sortables">
<div class="postbox " id="dashboard_right_now">
<div title="显示/隐藏" class="handlediv"><br></div><h3 class="hndle"><span>概况</span></h3>
	<div>文章：<%= StatisticsService.GetStatistics().PostCount %> 篇</div>
	<div>评论：<%=StatisticsService.GetStatistics().CommentCount %> 条</div>
    <div>标签：<%=StatisticsService.GetStatistics().TagCount %> 个</div>
    <div>访问量：<%=StatisticsService.GetStatistics().VisitCount %> 次</div>
    <div style="display:none">
    <p><asp:Button ID="btnCategory" runat="server" CssClass="button"  onclick="btnCategory_Click"  Text="重新统计分类文章数" Width="180" Font-Bold="false" /></p>
    <p><asp:Button ID="btnTag" runat="server"  CssClass="button" onclick="btnTag_Click"  Text="重新统计标签文章数"   Width="180" Font-Bold="false"  /></p>
    <p><asp:Button ID="btnUser" runat="server"  CssClass="button" onclick="btnUser_Click"  Text="重新统计作者文章和评论数" Width="180"   Font-Bold="false" /></p>
    <p class="notice">小提示:这些操作比较耗时.</p>
    </div>
</div>

<div class="postbox " id="dashboard_systeminfo">
<div title="显示/隐藏" class="handlediv"><br></div><h3 class="hndle"><span>系统信息</span></h3>
    <div>数据库： <%=DbPath %> (<%=DbSize%>) </div>
    <div>附件：<%=UpfilePath %> (共<%=UpfileCount%> 个, <%=UpfileSize%>)</div>
    <div>程序目录：<%= Request.PhysicalApplicationPath%></div>    
    <div>程序版本:<%= setting.Version %> <a href="http://www.jqpress.com" target="_blank">去官网查看新版</a></div>
    <div>CPU：<%=Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") %> (<%=Environment.ProcessorCount%> 核)</div>
    <div>缓存数量：<%=Cache.Count.ToString() %></div>
    <div>操作系统：<%=Environment.OSVersion %></div>
    <div>.NET 版本：<%=Environment.Version%></div>
    <div>IIS 版本：<%=Request.ServerVariables["SERVER_SOFTWARE"] %></div>
    <div>服务器IP：<%=Request.ServerVariables["LOCAl_ADDR"]%></div>

</div>
</div>	
</div>

<div style="width:49%;" class="postbox-container">

<div class="meta-box-sortables ui-sortable" id="side-sortables">
<div class="postbox " id="dashboard_recent_comments">
<div title="显示/隐藏" class="handlediv"><br></div><h3 class="hndle"><span>近期评论</h3>
<div class="inside">
	      <%foreach (CommentInfo comment in commentlist)
         { %>
               <p><a href="comment_list.aspx?operate=update&commentid=<%=comment.CommentId%>">审核</a>
                    <a href="comment_list.aspx?operate=delete&commentid=<%=comment.CommentId%>" onclick="return confirm('确定要删除吗?')">删除</a>      
                   [<%=comment.AuthorUrl%>] <span style="cursor:help;" title="<%=comment.Contents%>"><%=StringHelper.CutString(StringHelper.RemoveHtml(comment.Contents), 0, 50)%></span>
                   
               </p>
        <%} %>
        <%if (commentlist.Count == 0){ %> <p> 暂无待审核评论</p> <%} %>

</div>
</div>

<div class="postbox " id="dashboard_incoming_links" style="display:none">
<div title="显示/隐藏" class="handlediv"><br></div><h3 class="hndle"><span>引入链接 </span></h3>
<div class="inside" style=""><p>这个控制板小工具会查询 <a href="http://blogsearch.google.com/">Google 博客搜索</a>。当有其它博客链接到您的站点时，它们会显示在这里。目前还未找到引入的的链接，但您不用急。</p>
</div>
</div>

<div class="postbox " id="dashboard_primary">
<script type="text/javascript">
$(document).ready(function(){
  $.get("index.aspx?act=getrss",function(data){
     $(".rss-widget ul").empty().append(data);
  });
}); 
</script>
<div title="显示/隐藏" class="handlediv"><br></div><h3 class="hndle"><span>jqpress新闻 </span></h3>
<div class="inside" style="">
    <div class="rss-widget">
       <ul>
       加载新闻中...
          
          </ul>
         </div>
        </div>
</div>

</div>	

</div>


</div>

<div class="clear"></div>
</div><!--end of dashboard-widgets-wrap-->


</asp:Content>
