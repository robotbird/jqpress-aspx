<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Blog.Master" AutoEventWireup="true" CodeBehind="user_list.aspx.cs" Inherits="Jqpress.Web.admin.blog.user_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
 //修改操作
  function getJsonData(id){
      $.getJSON("user_list.aspx?act=getjson&id="+id+"&r="+Math.random(), function(json){
          $("#<%=txtUserName.ClientID%>").val(json.UserName);
          $("#<%=txtNickName.ClientID%>").val(json.NickName);
          $("#<%=txtEmail.ClientID%>").val(json.Email);
           $("#<%=txtSortNum.ClientID%>").val(json.SortNum);
          $("#hidUserId").val(json.UserId);
          $("#submit").val("修改");
          $(".formzone").show("slow");
      }); 
  }
    //删除操作
  function deleteRow(id){
      if(confirm('删除用户不会删除所属的文章,确定要删除吗?')){
        $.ajax({
          url: "user_list.aspx?act=delete&id="+id+"&r="+Math.random(),
          success: function(msg){$("#row"+id).fadeOut(500);}
        }); 
    }
  }

$(document).ready(function(){
  //初始化表单
  function InitForm(){
    $(':text').each(function(i,n) {n.value='';});
    $('#hidUserId').each(function(i,n) {n.value='';});
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
    if ($("#txtUserName").val() == '') { return; }
    var act="add";
    if($("#hidUserId").val()!=''){act="update";}
    
    jQuery.ajax({
        type: 'POST', // 设置请求类型为 ‘POST’，默认为 ‘GET’
        url: 'user_list.aspx?act='+act, //
        data: $('#aspnetForm').serialize(), // 从表单中获取数据  
        beforeSend: function() {},
        error: function(XMLHttpRequest, textStatus, errorThrown) { alert(textStatus);},
        dataType:'json',
        success: function(data) {
        
            if(data.err!=undefined){if(data.err!=''){alert(data.err);return;}}
            
            var _tr= "<tr class='row' style='background:#FFECC6' id='row"+data.UserId+"'>";
                  _tr+="<td>"+data.SortNum+"</td>";
                    _tr+=" <td>["+data.UserType+"]"+data.Link+"</td>";
                    _tr+=" <td>"+data.PostCount+"/"+data.CommentCount+"</td>";
                    _tr+=" <td>"+(data.Status=="1"?"<img src=\"../images/yes.gif\" title=\"使用\"/>":"<img src=\"../images/no.gif\" title=\"停用\"/>")+"</td>";  
                    _tr+=" <td>"+data.CreateTime+"</td>";  
                    _tr+="<td><a class='edit' href='javascript:getJsonData("+data.UserId+")' id='"+data.UserId+"'>编辑</a> <a class='delete' href='javascript:deleteRow("+data.UserId+")'  id='"+data.UserId+"'>删除</a></td>";
                _tr+="</tr>";  
            InitForm();
            $(".formzone").hide("slow");
              
            if(act=="update"){
              $("#row"+data.UserId).replaceWith(_tr);
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
		<div class="left">用户管理</div>
			<div class="right" style="padding-top:6px;">
			  <a class="addbtn" href="javascript:void(0)" id="addbtn">添加用户</a></div>
	   <div class="clr"></div>
</div>
<div class="formzone" style="display:none;">
   <input type="hidden" id="hidUserId" name="hidUserId" value="" />
    <div>
        <label class="label" for="<%=ddlUserType.ClientID %>">角色:</label>
        <asp:DropDownList ID="ddlUserType" runat="server"  CssClass="select">
            <asp:ListItem Value="1" Text="管理员"></asp:ListItem>
            <asp:ListItem Value="5" Text="写作者"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div>
        <label class="label" for="<%=txtUserName.ClientID %>">用户名:<span class="small gray">(字母,数字,中文,连字符)</span></label>
        <asp:TextBox ID="txtUserName"    MaxLength="50"  runat="server" Width="20%"  CssClass="solid-text" ></asp:TextBox>
    </div>
    <div>
        <label class="label" for="<%=txtNickName.ClientID %>">昵称:</label>
        <asp:TextBox ID="txtNickName"   MaxLength="50"  runat="server" Width="20%" CssClass="solid-text" ></asp:TextBox>
    </div>
    <div>
        <label class="label" for="<%=txtPassword.ClientID %>">密码:<span class="small gray"></span></label>
        <asp:TextBox ID="txtPassword" TextMode="password"    runat="server" Width="20%" CssClass="solid-text" ></asp:TextBox>
    </div>
    <div>
        <label class="label" for="<%=txtPassword2.ClientID %>">确认密码:<span class="small gray"></span></label>
        <asp:TextBox ID="txtPassword2" TextMode="password" runat="server" Width="20%" CssClass="solid-text" ></asp:TextBox>
    </div>
    <div><label class="label" for="<%=txtEmail.ClientID %>">邮箱:</label>
        <asp:TextBox ID="txtEmail"   runat="server" Width="20%" CssClass="solid-text" ></asp:TextBox>
    </div>
    <div><label class="label" for="<%=txtSortNum.ClientID %>">排序:</label><asp:TextBox ID="txtSortNum" runat="server" Width="50" CssClass="solid-text"></asp:TextBox>
        <span class="m_desc">越小越靠前</span>
    </div>
    <div>
        <asp:CheckBox ID="chkStatus" runat="server" Text="使用" Checked="true" />
     </div>
     <div>
         <input type="submit" class="button" id="submit" value="添加" />
        <input type="button" class="button" id="btncancle" value="取消" />
     </div>
</div>

    <table width="100%" class="tbl1">
        <tr class="header">
            <td>排序</td>
            <td>作者(用户名)</td>
            <td>文章/评论</td>
            <td>状态</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptUser" runat="server">
            <ItemTemplate>
                <tr class="row" id="row<%# DataBinder.Eval(Container.DataItem, "UserId")%>">                 
                    <td>
                    
                    <%# DataBinder.Eval(Container.DataItem, "SortNum")%></td>
                    <td>
                        [<%#GetUserType(DataBinder.Eval(Container.DataItem, "UserType"))%>]
                        <%# DataBinder.Eval(Container.DataItem, "Link")%>
                    </td>
                    
                    <td><%#  DataBinder.Eval(Container.DataItem,"PostCount")%> / <%# DataBinder.Eval(Container.DataItem, "CommentCount")%></td>
                    
                    <td><%# DataBinder.Eval(Container.DataItem, "Status").ToString() == "1" ? "<img src=\"../images/yes.gif\" title=\"使用\"/>" : "<img src=\"../images/no.gif\" title=\"停用\"/>"%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateTime")).ToString("yyyy-MM-dd")%></td>
                    <td><a class="edit" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "UserId")%>">编辑</a> <a class="delete" href="javascript:void(0)" id="<%# DataBinder.Eval(Container.DataItem, "UserId")%>">删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

</asp:Content>
