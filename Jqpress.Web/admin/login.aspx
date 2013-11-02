<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Jqpress.Web.admin.login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr" lang="zh-CN"> 
<head> 
	<title>机器鸟cms &rsaquo; 登录</title> 
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
<link rel='stylesheet' id='login-css'  href='styles/login.css?ver=20110121' type='text/css' media='all' /> 
<meta name='robots' content='noindex,nofollow' />
<script type="text/javascript">
if(top.location!=self.location)
{
	top.location.href = "login.aspx";
}
</script>
</head> 
<body class="login" > 
<div id="login">
  <h1>Jqpress</h1> 
<br />
 
<form name="loginform" id="loginform" method="post" runat="server"> 

	   <span class="td1">
		<asp:literal id="Msg" runat="server"></asp:literal>
	    <br />
		<br />
		</span>
	<p> 
		<label>用户名<br /> 
		<input type="text" name="username" id="username" class="input" value="" size="20" tabindex="10" /></label> 
	</p> 
	<p> 
		<label>密码<br /> 
		<input type="password" name="password" id="password" class="input" value="" size="20" tabindex="20" /></label> 
	</p> 
	<p class="forgetmenot"><label><input name="rememberme" type="checkbox" id="rememberme" value="forever" tabindex="90" /> 记住我的登录信息</label></p> 
	<p class="submit"> 
		<input type="submit" name="wp-submit" id="wp-submit" class="button-primary" value="登录" tabindex="100" /> 
		<input type="hidden" name="act" value="login" /> 
		<input type="hidden" name="testcookie" value="1" /> 
	</p> 
</form> 
 
<p id="nav"> 
<a href="#" title="找回密码">忘记密码？</a> 
</p> 
</div> 
<p id="backtoblog"><a href="/" title="不知道自己在哪？">&larr; 回到 主页</a></p> 
 
<script type="text/javascript"> 
function wp_attempt_focus(){
setTimeout( function(){ try{
d = document.getElementById('username');
d.value = '';
d.focus();
} catch(e){}
}, 200);
}
 
if(typeof wp_attempt_focus=='function')wp_attempt_focus();
</script> 
</body> 
</html> 