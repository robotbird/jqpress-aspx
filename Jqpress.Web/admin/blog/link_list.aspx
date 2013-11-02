<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="link_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.link_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
 //修改操作
  function getJsonData(id){
      $.getJSON("link_list.aspx?act=getjson&id="+id+"&r="+Math.random(), function(json){
          $("#<%=txtName.ClientID%>").val(json.LinkName);
          $("#<%=txtDescription.ClientID%>").val(json.Description);
          $("#<%=txtLinkUrl.ClientID%>").val(json.LinkUrl);
          $("#<%=txtDisplayOrder.ClientID%>").val(json.SortNum);
          $("#hidLinkId").val(json.LinkId);
          $("#submit").val("修改");
          $(".formzone").show("slow");
      }); 
  }
    //删除操作
  function deleteRow(id){
      if(confirm('确定要删除吗?')){
        $.ajax({
          url: "link_list.aspx?act=delete&id="+id+"&r="+Math.random(),
          success: function(msg){$("#row"+id).fadeOut(500);}
        }); 
    }
  }

$(document).ready(function(){
  //初始化表单
  function InitForm(){
    $(':text').each(function(i,n) {n.value='';});
    $('#hidLinkId').each(function(i,n) {n.value='';});
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
    if($("#hidLinkId").val()!=''){act="update";}
    
    jQuery.ajax({
        type: 'POST', // 设置请求类型为 ‘POST’，默认为 ‘GET’
        url: 'link_list.aspx?act='+act, //
        data: $('#aspnetForm').serialize(), // 从表单中获取数据  
        beforeSend: function() {},
        error: function(XMLHttpRequest, textStatus, errorThrown) { alert(textStatus);},
        dataType:'json',
        success: function(data) {
            var _tr= "<tr class='row' style='background:#FFECC6' id='row"+data.LinkId+"'>";
                  _tr+="<td>"+data.SortNum+"</td>";
                    _tr+=" <td><a href='"+data.LinkUrl+"' >"+data.LinkName+"</a>"+data.Position+data.Status+"</td>";
                    _tr+=" <td>"+data.Description+"</td>";
                    _tr+=" <td>"+data.CreateTime+"</td>";  
                    _tr+="<td><a class='edit' href='javascript:getJsonData("+data.LinkId+")' id='"+data.LinkId+"'>编辑</a> <a class='delete' href='javascript:deleteRow("+data.LinkId+")'  id='"+data.LinkId+"'>删除</a></td>";
                _tr+="</tr>";  
            InitForm();
            $(".formzone").hide("slow");
              
            if(act=="update"){
              $("#row"+data.LinkId).replaceWith(_tr);
            }else{
              $(".tbl1").append(_tr);
              $(".total strong").text(parseInt($(".total strong").text())+1)
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
		<div class="left">链接管理</div>
			<div class="right" style="padding-top:6px;">
			  <a class="addbtn" href="javascript:void(0)" id="addbtn">添加链接</a></div>
	   <div class="clr"></div>
</div>

<div class="formzone"  style="display:none">
     <input type="hidden" id="hidLinkId" name="hidLinkId" value="" />
    <div><label class="label"  for="<%=txtName.ClientID %>">名称:</label><asp:TextBox ID="txtName" runat="server" Width="20%"  CssClass="solid-text" ></asp:TextBox></div>
    <div><label class="label"  for="<%=txtLinkUrl.ClientID %>">链接地址:</label><asp:TextBox ID="txtLinkUrl" runat="server" Width="20%"   CssClass="solid-text"></asp:TextBox></div>
    <div><label class="label"  for="<%=txtDescription.ClientID %>">描述:</label><asp:TextBox ID="txtDescription" runat="server" Width="20%"  CssClass="solid-text" ></asp:TextBox></div>
    <div><label class="label" for="<%=txtDisplayOrder.ClientID %>">排序:</label><asp:TextBox ID="txtDisplayOrder" runat="server" Width="50" CssClass="solid-text"></asp:TextBox>
        <span class="m_desc">越小越靠前</span>
    </div>
    <div>
        <asp:CheckBox ID="chkStatus" runat="server" Text="显示" Checked="true" />
        <asp:CheckBox ID="chkPosition" runat="server" Text="导航"   />
        <asp:CheckBox ID="chkTarget" runat="server" Text="新窗口"  Checked="true" />
    </div>
    <div>
    <input type="submit" class="button" id="submit" value="添加" />
    <input type="button" class="button" id="btncancle" value="取消" />
    </div>
    <div class="notice">小提示:${siteurl} 表示根目录.</div>
</div>
<div class="left" >
    <table width="100%" class="tbl1">
        <tr class="header">
            <td>排序</td>
            <td>名称</td>
            <td style="width:40%;">描述</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptLink" runat="server">
            <ItemTemplate>
                <tr class="row" id="row<%# DataBinder.Eval(Container.DataItem, "LinkId")%>">
                    <td><%# DataBinder.Eval(Container.DataItem, "SortNum")%></td>
                    <td>
                        <a href="<%# DataBinder.Eval(Container.DataItem, "LinkUrl")%>" ><%# DataBinder.Eval(Container.DataItem, "LinkName")%></a>
                        <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Position")) == (int)Jqpress.Blog.Entity.Enum.LinkPosition.Navigation ? "[导航]" : ""%>
                        <%# DataBinder.Eval(Container.DataItem, "Status").ToString()=="0"?"[隐藏]":""%>
                    </td>
                  
                    <td><%# DataBinder.Eval(Container.DataItem, "Description")%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateTime")).ToString("yyyy-MM-dd")%></td>
                           
                    <td><a class="edit" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "LinkId")%>">编辑</a> <a class="delete" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "LinkId")%>">删除</a></td>

                    
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
</asp:Content>
