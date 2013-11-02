<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="blog_db.aspx.cs" Inherits="Jqpress.Web.admin.blog.blog_db" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Jqpress.Framework.Utils" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="body_nav"><div class="left">数据管理</div></div>
<%=ResponseMessage %>
<h4>当前数据库(Access)</h4>
<table width="100%" class="tbl1">
    <tr class="row">
        <td  style="width:20%;">路径:</td>
        <td><%=DbInfo.FullName   %></td>
    </tr>
    <tr class="row">
        <td>大小:</td>
        <td><%=FileHelper.ConvertUnit(DbInfo.Length)%>  
       
         </td>
    </tr>
    <tr class="row">
        <td>创建时间:</td>
        <td><%=DbInfo.CreationTime %></td>
    </tr>
    <tr class="row">
        <td>最后修改时间:</td>
        <td><%=DbInfo.LastWriteTime %></td>
    </tr>
    <tr class="rowend">
        <td ></td>
        <td>
           
            <asp:Button ID="btnBak" runat="server" Text="备份" onclick="btnBak_Click"  CssClass="button"/> 
            <asp:Button ID="btnCompact" runat="server"  Text="压缩(部分空间不支持)" onclick="btnCompact_Click" CssClass="button" /> 
        </td>
    </tr>
                <%
                    DirectoryInfo dircss = new DirectoryInfo(Server.MapPath( BackupPath ));
                    if (dircss.GetFiles().Length > 0)
              { %>
    <tr>
        <td colspan="2">
            <table>
               <tr class="category">
                    <td >备份列表</td>
                    <td>大小</td>
                    <td>备份时间</td>
                    <td>操作</td>
                </tr>
             <%
                 foreach (FileInfo    f in dircss.GetFiles   ())
                 {
            %>

                <tr class="row">
                     
                    <td><%=BackupPath %><%=f.Name  %></td>
                    <td><%=FileHelper.ConvertUnit(f.Length) %></td>
                    <td><%=f.CreationTime %></td>
                    <td>
                        <a href="blog_db.aspx?operate=down&name=<%=f.Name %>" >下载</a>
                        <a href="blog_db.aspx?operate=restore&name=<%=f.Name %>" onclick="return confirm('还原将会覆盖当前数据库，确定要继续吗？');">还原</a>
                        <a href="blog_db.aspx?operate=delete&name=<%=f.Name %>" onclick="return confirm('确定要删除吗？');">删除</a>
                    </td>
                </tr>

            <%} %>
            </table>
        </td>
    </tr>
    <%} %>
</table>
</asp:Content>
