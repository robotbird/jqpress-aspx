<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upfilebyeditor.aspx.cs" Inherits="Jqpress.Web.admin.blog.upfilebyeditor" %>
<%@ Register src="~/admin/controls/upfilemanager.ascx" tagname="upfilemanager" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
   
<style type="text/css">
  @charset "utf-8";
body{font-size:12px;line-height:14px;font-family:Verdana;background-color:#fff;z-index:300;}
.clr{clear:both;}
.clear{width:100%;height:1px;line-height:1px;clear:both;font:0/0 Arial;}
p,ul,h1,h2,h3,form{margin:0;padding:0;}
li{ list-style:none;}
img{border:0;}
:focus{outline:0;}
a:link{color:#058EC4;text-decoration:none;}
a:visited{color:#058EC4;text-decoration:none;}
a:active{color:#058EC4;text-decoration:none;}
a:hover {background-color: #ccc;}


input[type="text"],
input[type="password"],
input[type="file"],
input[type="button"],
input[type="submit"],
input[type="reset"] {
	-moz-border-radius: 4px;
	-khtml-border-radius: 4px;
	-webkit-border-radius: 4px;
	border-radius: 4px;
	behavior:url(styles/ie-css3.htc);
}

/*按钮样式begin*/
.submit input,
.button,
input.button,
.button-primary,
input.button-primary,
.button-secondary,
input.button-secondary,
.button-highlighted,
input.button-highlighted,
#postcustomstuff .submit input {
	text-decoration: none;
	font-size: 11px !important;
	line-height: 16px;
	padding: 0px 8px;
	cursor: pointer;
	border-width: 1px;
	border-style: solid;
	-moz-border-radius: 4px;
	-khtml-border-radius: 4px;
	-webkit-border-radius: 4px;
	border-radius: 4px;
	-moz-box-sizing: content-box;
	-webkit-box-sizing: content-box;
	-khtml-box-sizing: content-box;
	box-sizing: content-box;
}

.button {BORDER-RIGHT: #365D95 1px solid; PADDING-RIGHT: 3px; BORDER-TOP: #365D95 1px solid; PADDING-LEFT: 3px; BACKGROUND:#365D95; PADDING-BOTTOM: 0px; FONT: 12px/20px Verdana, Arial, Helvetica, sans-serif; BORDER-LEFT: #365D95 1px solid; PADDING-TOP: 0px; BORDER-BOTTOM: #365D95 1px solid; HEIGHT: 22px; cursor: pointer; color:#FFF;-moz-border-radius:4px;-webkit-border-radius:4px;border-radius:4px;behavior:url(styles/ie-css3.htc);
}
/*按钮样式end*/
        
 .body_middle{ margin:10px;}
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
    <style type="text/css">
        #content { margin :0px; border:0px;} 
    </style>
</head>
<body>
<div class="body_middle">
    <form id="form1" runat="server">
    <div id="content">   
        <uc1:upfilemanager ID="upfilemanager1" runat="server" />
        <div style="clear:both;"></div>
    </div>
    </form>
</div>
</body>
</html>