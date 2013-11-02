<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="category_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.category_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
 //修改操作
  function getJsonData(id){
      $.getJSON("category_list.aspx?act=getjson&id="+id+"&r="+Math.random(), function(json){
          $("#<%=txtName.ClientID%>").val(json.CateName);
          $("#<%=txtSlug.ClientID%>").val(json.Slug);
          $("#<%=txtDescription.ClientID%>").val(json.Description);
          $("#<%=txtDisplayOrder.ClientID%>").val(json.SortNum);
          
          
          $("select#<%=ddlCategory.ClientID%> option[value='"+json.ParentId+"']").attr('selected', 'true');
          $("#hidCategoryId").val(json.CategoryId);
          $("#submit").val("修改");
          $(".formzone").show("slow");
      }); 
  }
    //删除操作
  function deleteRow(id){
      if(confirm('删除分类不会删除所属的文章,确定要删除吗?')){
        $.ajax({
          url: "category_list.aspx?act=delete&id="+id+"&r="+Math.random(),
          success: function(msg){
             $("#row"+id).fadeOut(500);
           }
        }); 
    }
  }

$(document).ready(function(){

  //初始化表单
  function InitForm(){
    $(':text').each(function(i,n) {n.value='';});
   $("select#<%=ddlCategory.ClientID%> option[value='0']").attr('selected', 'true');
     
    $('#hidCategoryId').each(function(i,n) {n.value='';});
    $("#submit").val("添加");
  }
 
  //添加事件
  $("#addbtn").click(function(){
    if($(".formzone").css("display")=="none"){
       InitForm();
       $(".formzone").show("slow");
    }else{
       $(".formzone").hide("slow");
    }  
  });
  //绑定删除事件
  $(".row td .delete").bind("click",function(){deleteRow(this.id);});
  //绑定修改事件
  $(".row td .edit").bind("click",function(){getJsonData(this.id)});
  
  //取消事件
  $("#btncancle").click(function(){
    if($(".formzone").css("display")=="none"){
      InitForm();
      $(".formzone").show("slow");
    }else{
      $(".formzone").hide("slow");
    }  
  });
  //保存事件
  $('#aspnetForm').submit(function() {
    if ($("#txtName").val() == '') { return; }
    var act="add";
    if($("#hidCategoryId").val()!=''){act="update";}
    
    jQuery.ajax({
        type: 'POST', // 设置请求类型为 ‘POST’，默认为 ‘GET’
        url: 'category_list.aspx?act='+act, //
        data: $('#aspnetForm').serialize(), // 从表单中获取数据  
        beforeSend: function() {},
        error: function(XMLHttpRequest, textStatus, errorThrown) { alert(textStatus);},
        dataType:'json',
        success: function(data) {
            var _tr= "<tr class='row' style='background:#FFECC6' id='row"+data.CategoryId+"'>";
                  _tr+="<td>"+data.SortNum+"</td>";
                    _tr+=" <td>"+data.TreeChar+"<a href='"+data.Url+"' >"+data.CateName+"</a></td>";
                    _tr+=" <td>"+data.Description+"</td>";
                    _tr+=" <td>"+data.PostCount+"</td>";
                    _tr+=" <td>"+data.CreateTime+"</td>";  
                    _tr+="<td><a class='edit' href='javascript:getJsonData("+data.CategoryId+")' id='"+data.CategoryId+"'>编辑</a> <a class='delete' href='javascript:deleteRow("+data.CategoryId+")'  id='"+data.CategoryId+"'>删除</a></td>";
                _tr+="</tr>";  
            InitForm();
            $(".formzone").hide("slow");
              
            if(act=="update"){
              $("#row"+data.CategoryId).replaceWith(_tr);
            }else{
             if(data.ParentId>0){
               $("#row"+data.ParentId).after(_tr);
             }else{
               $(".tbl1").append(_tr);
             }
            }
           
        }
    });
    return false;
});

  
});
 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="body_nav">
		<div class="left">分类管理 <span style="font-size:9px;">&gt;&gt;</span>添加分类</div>
			<div class="right" style="padding-top:6px;">
			  <a class="addbtn" href="javascript:void(0)" id="addbtn">添加分类</a></div>
			<div class="clr"></div>
		</div>
<div class="formzone" style="display:none;">
    <input type="hidden" id="hidCategoryId" name="hidCategoryId" value="" />
    <div><label class="label" for="<%=txtName.ClientID %>">名称:</label>
    <asp:TextBox ID="txtName" runat="server"  Width="150"  CssClass="solid-text"></asp:TextBox>
    <span>这将是它在站点上显示的名字</span>
    </div>
    <div><label class="label" for="<%=ddlCategory.ClientID %>">父类:</label>
    <asp:DropDownList ID="ddlCategory" CssClass="select" runat="server"></asp:DropDownList>
    <span>你可以在一个分类下创建很多子类，这样便于管理和分类</span>
    </div>
    
    <div><label class="label"  for="<%=txtSlug.ClientID %>">别名:<span  class="gray small"  ></span></label>
    <asp:TextBox ID="txtSlug" runat="server"  Width="150"  CssClass="solid-text"></asp:TextBox>
    <span>“别名”是对于 URL 友好的一个别称。它通常为小写并且只能包含字母，中文，数字和连字符（-）。</span>
    </div>
    <div><label class="label"  for="<%=txtDescription.ClientID %>">描述:</label>
    <asp:TextBox ID="txtDescription" runat="server"  Width="300"  CssClass="solid-text"></asp:TextBox>
    <span>描述不一定会显示，但是他是很友好的提示。</span>
    </div>
    <div><label class="label" for="<%=txtDisplayOrder.ClientID %>">排序:</label>
    <asp:TextBox ID="txtDisplayOrder" runat="server" Width="30" CssClass="solid-text"></asp:TextBox>
    <span class="m_desc">越小越靠前</span>
    </div>
    <div>
    <input type="submit" class="button" id="submit" value="添加" />
    <input type="button" class="button" id="btncancle" value="取消" />
    </div>
</div>

<table width="100%" class="tbl1">
        <tr class="header">
            <td>排序</td>
            <td>名称</td>
            <td style="width:40%;">描述</td>
            <td>文章</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptCategory" runat="server">
            <ItemTemplate>
                <tr class="row" id="row<%# DataBinder.Eval(Container.DataItem, "categoryid")%>">
                    <td><%# DataBinder.Eval(Container.DataItem, "SortNum")%></td>
                    <td><%#  DataBinder.Eval(Container.DataItem,"TreeChar")%> <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" ><%# DataBinder.Eval(Container.DataItem, "CateName")%></a></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Description")%></td>
                    <td><%#  DataBinder.Eval(Container.DataItem,"PostCount")%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateTime")).ToString("yyyy-MM-dd")%></td>            
                    <td><a class="edit" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "categoryid")%>">编辑</a> <a class="delete" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "categoryid")%>">删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
