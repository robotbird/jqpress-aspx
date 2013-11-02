<%@ Page Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="file_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.file_list" Title="无标题页" %>
<%@ Register src="~/admin/controls/upfilemanager.ascx" tagname="upfilemanager" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
 .body_middle .right{ width:200px;  float:right; border:0px solid gray;  margin-top:0px;  }
 .body_middle .right p{ margin:10px 0 0 10px;  }
 .body_middle .left{ _float:left; margin-right:200px !important;  _margin-right:-10px; } 
 .upfile li{  float:left;  margin:5px 3px 25px 0;    width:90px; height:88px; border:0px solid #ccc; text-align:center;}
.upfile .current{   background:#f3f9f5;}
.upfile p{ margin:3px 0; overflow:hidden;text-overflow:ellipsis; white-space:nowrap; line-height:150%; }
.upfile a{   width:48px; height:48px;   }
.upfile img{ border:0px solid #ccc; }
.upfile .delete{position:relative;  width:20px; height:20px;   top:-92px;   right:-40px;  border:0px solid gray; color:Gray; font-size:10px; }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="body_nav"><div class="left">附件管理</div></div>
    <uc1:upfilemanager ID="upfilemanager1" runat="server" />
</asp:Content>

