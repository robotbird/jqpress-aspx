<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="post_edit.aspx.cs" Inherits="Jqpress.Web.admin.blog.post_edit" %>
<%@ Import Namespace="Jqpress.Blog.Entity" %>
<%@ Import Namespace="Jqpress.Blog.Services" %>
<%@ Import Namespace="Jqpress.Framework.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="../../common/styles/thickbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../../common/scripts/jquery.tagto.js"></script>
<script type="text/javascript" src="../../common/scripts/thickbox.js"></script>

<style type="text/css">
/*tag choose*/
.selected {background:#c00; color:#fff;}

</style>
<script type="text/javascript">
        (function($){
            $(document).ready(function(){
                $("#taglist").tagTo("#<%=txtTags.ClientID %>",",","selected");
            });    
        })(jQuery);
</script>
<!-- TinyMCE -->
<script type="text/javascript" src="../../common/editors/tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		// General options
		mode : "exact",
		elements : "<%=txtContents.ClientID%>",
		theme: "advanced",
        language:"zh-cn",
        
      plugins : "autolink,lists,spellchecker,style,save,advhr,advimage,advlink,emotions,inlinepopups,insertdatetime,searchreplace,contextmenu,paste,fullscreen,xhtmlxtras,insertcode",//autolink,lists,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,insertcode
        // Theme options
        theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect,insertcode",
        theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,code,|,insertdate,inserttime,|,forecolor,backcolor,insertcode|,fullscreen",
        theme_advanced_buttons3 : "",//tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen
        theme_advanced_buttons4 : "",//insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,blockquote,pagebreak,|,insertfile,imagemanager,insertcode
        theme_advanced_toolbar_location : "top",
        theme_advanced_toolbar_align : "left",
        theme_advanced_statusbar_location : "bottom",
        theme_advanced_resizing : true,


		// Example content CSS (should be your site CSS)
		content_css : "../../common/editors/css/content.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "../../common/editors/lists/template_list.js",
		external_link_list_url : "../../common/editors/lists/link_list.js",
		external_image_list_url : "../../common/editors/lists/image_list.js",
		media_external_list_url : "../../common/editors/lists/media_list.js",

		// Style formats
		style_formats : [
			{title : 'Bold text', inline : 'b'},
			{title : 'Red text', inline : 'span', styles : {color : '#ff0000'}},
			{title : 'Red header', block : 'h1', styles : {color : '#ff0000'}},
			{title : 'Example 1', inline : 'span', classes : 'example1'},
			{title : 'Example 2', inline : 'span', classes : 'example2'},
			{title : 'Table styles'},
			{title : 'Table row 1', selector : 'tr', classes : 'tablerow1'}
		],

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
</script>
<!-- /TinyMCE -->
<script type="text/javascript">

function addFileToEditor(fileUrl,fileExtension)
{
   
    if(fileExtension=='.gif' || fileExtension=='.jpg' || fileExtension=='.jpeg' || fileExtension=='.bmp' || fileExtension=='.png'){
        var imageTag="<img src=\""+fileUrl+"\"/>";
       tinyMCE.execCommand('mceInsertContent',false,imageTag);  
        
    }else{
        var imageTag="<a href=\""+fileUrl+"\">"+fileUrl+"</a>";
        tinyMCE.execCommand('mceInsertContent',false,imageTag); 
    }
    
}

function createSummary(type) {
    var postContent =tinyMCE.activeEditor.getContent();
    var postSummary =$("#<%=txtSummary.ClientID %>");

    var iLength  = postSummary.text().length;

    var r=true;
    if(iLength>0){
        if(!confirm('提取将会覆盖已有摘要,要继续吗?')){
		    r=false;
        }
    }
    if(r){
       var tmpcotent=postContent;
        if(type =='full'){
          
            postSummary.text(tmpcotent);  
        }
        else{
			postSummary.text(tmpcotent.replace(/<[^>]+>/g, "").substring(0,300));
		}
    }
    return false;
}
</script>
<script type="text/javascript">
function checkForm()
{
  var title=$("#<%=txtTitle.ClientID%>");
  var content=$("#<%=txtContents.ClientID%>");
  
  if(title.text()==""){
     //$("#lableTitle").css("color","red");
     //return false;
  }
    if(content.text()==""){
    // return false;
  }
}
</script>

</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<div class="body_nav">
		<div class="left">文章管理 <span style="font-size:9px;">&gt;&gt;</span>添加文章</div>
			<div class="right" style="padding-top:6px;">
			  <a class="addbtn" href="#">创建新文章</a>			</div>
			<div class="clr"></div>
</div>
        
		
<div class="formzone">
<form name="form" action="" method="post">

<div id="titlewrap">
	<asp:TextBox ID="txtTitle"   maxlength="200"  Width="60%" runat="server" CssClass="solid-text"></asp:TextBox>
	<label id="lableTitle" for="txtTitle"  >标题不能为空</label>
</div>
<div id="contentswrap">
     
     <a href="upfilebyeditor.aspx?keepThis=true&getfile=1&TB_iframe=true&height=420&width=620" title="插入图片/文件" class="thickbox" target="_blank">插入图片/文件</a>
	<asp:TextBox ID="txtContents" runat="server" Height="400" TextMode="MultiLine"    Width="95%"></asp:TextBox>
</div>
<div id="summarywrap">
	 <label  class="label" for="<%=txtSummary.ClientID %>">摘要: <a href="javascript:void(0);" onclick="createSummary('part');">从正文提取部分</a>/<a href="javascript:void(0);" onclick="createSummary('full');">全部</a></label>
        <asp:TextBox ID="txtSummary" runat="server" Height="120px" TextMode="MultiLine"    Width="95%"></asp:TextBox>
</div>
<div id="categorywrap">
	<label for="ddlCategory"  >分类&nbsp;</label>
	<asp:DropDownList ID="ddlCategory" runat="server" CssClass="select" ></asp:DropDownList>
</div>
<div id="tagwrap">
     <label for="txtTags">标签&nbsp;</label>
	<asp:TextBox ID="txtTags"  runat="server" Width="49%" CssClass="solid-text"></asp:TextBox> 多个TAG请用逗号隔开!
        <a  href="###" onclick="showTag();" >查看常用标签↓</a>
        <div id="taglist" style=" border:2px solid #ccc;display:none;  line-height:135%; padding:3px ;  ">
            
            <%
                System.Collections.Generic.List<TagInfo> tagList = TagService.GetTagList(20);
                foreach (TagInfo tag in  tagList)
              { %>
              
                  <a href="###" style="padding-left:3px;"><%=tag.CateName %></a><span class="gray small">(<%=tag.PostCount %>)</span>
                   
            <%} %>
            
            <%if (tagList.Count == 0)
              { %>
              暂无
            <%} %>
            
        </div>
                 
<script type="text/javascript">
    function showTag(){
        if(document.getElementById('taglist').style.display==''){
            document.getElementById('taglist').style.display='none';
        }else{
            document.getElementById('taglist').style.display='';
        }
    }
</script>    
        
</div>
<div id="slugwrap">
	<label for="title" id="title-prompt-text" >链接&nbsp;</label>
	<asp:TextBox ID="txtCustomUrl"    runat="server" Width="49%" CssClass="solid-text"></asp:TextBox>
</div>

<div id="templatewrap">
	<label for="title" id="title-prompt-text" >模板</label>
	<asp:DropDownList ID="ddlTemplate" runat="server"  CssClass="select"></asp:DropDownList>
        <span  class="gray small"> 
          (位于<%=ConfigHelper.SitePath %>themes/<%=setting.Theme %>/template/下,当然,您也可以自己制作,文件名必须以"post"开头)</span>
</div>
<div id="propertywrap">
        <asp:CheckBox ID="chkStatus" runat="server" Text="发布" Checked="true" />
        <asp:CheckBox ID="chkCommentStatus" runat="server" Text="允许评论" Checked="true" />
        <asp:CheckBox ID="chkRecommend" runat="server" Text="推荐"  />
        <asp:CheckBox ID="chkTopStatus" runat="server" Text="置顶"  />
         <asp:CheckBox ID="chkHomeStatus" runat="server" Text="首页"  />
        <asp:CheckBox ID="chkHideStatus" runat="server" Text="列表隐藏" ToolTip="不会出现在文章列表,常用于留言本,关于我的页面!"  />
        <asp:CheckBox ID="chkSaveImage" runat="server" Text="保存远程图片" ToolTip="自动将远程图片保存到本地，仅保存静态地址图片" />
</div>
<div id="buttonwrap">
 <asp:Button ID="btnEdit" runat="server" Text="添加" onclick="btnEdit_Click"  CssClass="button" OnClientClick="return checkForm();" />
 <input class="button" type="button" value="返回" onClick="history.go(-1)"> 

</div>

</form>
</div>  
 

</asp:Content>
