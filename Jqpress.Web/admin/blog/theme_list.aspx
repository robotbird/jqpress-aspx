<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="theme_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.theme_list" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Jqpress.Blog.Configuration" %>
<%@ Import Namespace="Jqpress.Blog.Services" %>
<%@ Import Namespace="Jqpress.Framework.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.theme li{  float:left;  margin:15px 20px 0 0; padding:5px 8px;  border:1px solid #ccc;  }
.theme .current{background:#f3f9f5; border:1px solid #FFCC66;}
.theme p{ margin: 0; padding:2px 0; white-space:nowrap;  overflow:hidden;text-overflow:ellipsis; width:225px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="body_nav"><div class="left">主题管理</div></div>
<%=ResponseMessage%>
<h4>电脑,手机主题<span class="small gray normal">(default 为系统默认主题,禁止删除,不建议修改)</span></h4>
<ul class="theme">
<%
DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../../themes/"));
foreach (DirectoryInfo d in dir.GetDirectories())
{
    if (d.Name == ".svn") continue;
%>
    <li <%=(BlogConfig.GetSetting().Theme == d.Name || BlogConfig.GetSetting().MobileTheme == d.Name)?" class=\"current\"":"" %>  >
        <p class="gray"><%=ConfigHelper.SitePath%>themes/<%=d.Name%>/</p>
        <p><img src="../../themes/<%=d.Name %>/theme.jpg" width="200" height="150" alt=""  style="border:1px solid #ccc;" /></p>
        <h5 style="margin:5px 0 3px 0 ;"><%=ThemeService.GetTemplate(d.FullName).Name%></h5>
        <p  class="gray">作者: <a href="mailto:<%=ThemeService.GetTemplate(d.FullName).Email %>"><%=ThemeService.GetTemplate(d.FullName).Author%> </a></p>
        <p class="gray">主页: <a href="<%=ThemeService.GetTemplate(d.FullName).SiteUrl %>" target="_blank"><%=ThemeService.GetTemplate(d.FullName).SiteUrl%></a></p>
        <p class="gray" style="border-bottom:1px solid #ccc; padding-bottom:5px;">发布: <%=ThemeService.GetTemplate(d.FullName).PubDate%></p>
       
        <p style="border-bottom:1px solid #ccc; padding:5px 0;">
        <%if (BlogConfig.GetSetting().Theme == d.Name)
          {%>     电脑版使用
        <%}
          else
          { %>
         <a href="theme_list.aspx?operate=update&type=pc&themename=<%=d.Name %>">电脑版使用</a>
           
        <%} %>
       
        <%if (BlogConfig.GetSetting().MobileTheme == d.Name)
          {%>     手机版使用
        <%}
          else
          { %>
            <a href="theme_list.aspx?operate=update&type=mobile&themename=<%=d.Name %>">手机版使用</a> 
        <%} %>
        </p>
        <p style=" padding-top:5px ;">
            <a href="../../theme/<%=d.Name %>.aspx">预览</a>
            <a href="theme_edit.aspx?themename=<%=d.Name%>" title="编辑模板,样式,脚本,其它文本文件">编辑</a> 
            <a href="theme_list.aspx?operate=insert&themename=<%=d.Name %>"  title="复制该主题">复制</a>
            <%if (d.Name != "default")
              { %>
            <a href="theme_list.aspx?operate=delete&themename=<%=d.Name %>" onclick=" return confirm('确定要删除吗?');" title="删除该主题">删除</a>
            <%} %>
       </p>
    </li>
    
<%} %>
</ul>
</asp:Content>
