<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="post_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.post_list" %>
<%@ Register Assembly="Jqpress.Blog" Namespace="Jqpress.Blog.Common" TagPrefix="jqpress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
$(document).ready(function(){
  ///全选
  $("#checkAll").click(function(){
   if(this.checked){
     $("input[name='chkid']").attr("checked", true); 
   }else{
     $("input[name='chkid']").attr("checked", false);
   }
  });
  ///批量操作
  $("#doaction").click(function(){
        var chkid=$("input[name='chkid']");
        var strid="";
        $.each(chkid, function(i, n){if(n.checked){strid=strid+(n.value)+",";} }); 
        if(strid!=""){
            $.ajax({
               type: "GET",
               url: "post_list.aspx",
               data: "action="+$("#selectact").val()+"&strid="+strid,
               success: function(msg){$.each(chkid, function(i, n){if(n.checked){$("#post"+n.value).hide();}});}
            }); 
        }
  });
  
});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div class="body_nav">
			<div class="left">文章管理 <span style="font-size:9px;">&gt;&gt;</span>添加文章</div>
			<div class="right" style="padding-top:6px;">
			  <a class="addbtn" href="post_edit.aspx">创建新文章</a></div>
			<div class="clr"></div>
		</div>
        
        
<div class="table-top">
  <select name="action" id="selectact" class="select">
    <option value="-1" selected="selected">批量操作</option>
	<option value="deletes">删除</option>
  </select>
   <input type="button" name="" id="doaction" class="button" value="应用" />
   
   <input type="text" value="" id="txtKeyword" runat="server" class="solid-text" size="10" /> 关键字 
   <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select" ></asp:DropDownList>
   <asp:Button ID="btnFilter" runat="server" CssClass="button" Text="过滤" OnClick="btnSearch_Click" />
</div>

<table class="tbl1" width="100%" border="0" cellspacing="0" cellpadding="0">
        
		 <tr class="header">
			<td><input type="checkbox" name="checkAll" id="checkAll" /></td>
			<td>标题</td>
			<td>评论/浏览</td>
			<td>日期</td>
			<td>操作</td>
			
		  </tr>
          		  
		 <asp:Repeater ID="rptPost" runat="server">
            <ItemTemplate>
            
		  <tr id="post<%#DataBinder.Eval(Container.DataItem, "postid")%>">
			<td><input type="checkbox" name="chkid" value="<%#DataBinder.Eval(Container.DataItem, "postid")%>" /> </td>
			<td>
			[<%# DataBinder.Eval(Container.DataItem, "Author.Link")%>]
			<%# DataBinder.Eval(Container.DataItem, "Link")%>
			<%# DataBinder.Eval(Container.DataItem, "TopStatus").ToString()=="1" ?"[置顶]" : ""%>
			<%# DataBinder.Eval(Container.DataItem, "Recommend").ToString() == "1" ? "[推荐]" : ""%>
			<%# DataBinder.Eval(Container.DataItem, "Status").ToString()=="1" ?"" : "[草稿]"%>
			<%# DataBinder.Eval(Container.DataItem, "HideStatus").ToString() == "1" ? "[隐藏]" : ""%>
			</td>
			<td><%# DataBinder.Eval(Container.DataItem, "CommentCount")%>/<%# DataBinder.Eval(Container.DataItem, "ViewCount")%></td>
			<td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "PostTime")).ToString("yyyy-MM-dd")%></td>
			<td>
			  	<span>[<%#GetEditLink(DataBinder.Eval(Container.DataItem, "postid"), DataBinder.Eval(Container.DataItem, "userid"))%>]</span>
				<span>[<%#GetDeleteLink(DataBinder.Eval(Container.DataItem, "postid"), DataBinder.Eval(Container.DataItem, "userid"))%>]</span>
		    </td>
		  </tr>
		  
             </ItemTemplate>
        </asp:Repeater>
          
</table>
		  		
<jqpress:PagerControl id="Pager1" runat="server" PageSize="10"  CssClass="pager"></jqpress:PagerControl>
		
</asp:Content>
