# 博客程序架构
本博客程序是博主11年的时候参考loachs小泥鳅博客内核开发的.net跨平台博客cms，距今已有6年多了，个人博客网站一直在用，虽然没有wordpress那么强大，但是当时在深究.net的同时，自己能写一个博客程序，并且基于独立Linux服务器搭建一个自己的.net网站还是挺有意思。
## 技术栈
- .net framework 4.0
- sqlite 数据库
- mono linux 运行环境以及mono下的sqlite库
- nvelocity 模板引擎
- dapper 轻量级orm框架
- vs2010
博客首页
![首页](http://images2017.cnblogs.com/blog/94489/201712/94489-20171209220004931-929713759.png)


后台登录，默认用户名admin，密码admin
![登录](http://images2017.cnblogs.com/blog/94489/201712/94489-20171209230531857-927221251.png)

后台首页
![管理后台首页](http://images2017.cnblogs.com/blog/94489/201712/94489-20171209222459841-1698271926.png)

文章编辑
![文章编辑](http://images2017.cnblogs.com/blog/94489/201712/94489-20171209222740122-1851304180.png)


## linux部署方式
linux下需要安装mono和jexus就可以运行起来，mono作为.net framework的linux运行环境，jexus作为web服务器。


## 说明文章
http://www.cnblogs.com/jqbird/p/7965995.html
