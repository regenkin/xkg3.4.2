<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotPermisson.aspx.cs" Inherits="Hidistro.UI.Web.Admin.NotPermisson" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>出错了</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="renderer" content="webkit" />
    <link rel="icon" href="images/hi.ico" />
    <style type="text/css">
    *{margin: 0;padding: 0;}
    html,body,form{width: 100%;height: 100%;background-color: #F4F4F4;}
    .content404{width: 810px; height: 100%;margin: 0 auto;background: url(images/404-1.png) no-repeat left 100px;}
    .content404 .inner{padding-top: 100px;width: 522px;float: right;}
    .content404 .inner h1{width: 522px;height: 70px;text-indent: -9999px;background:url(images/404title.jpg) no-repeat left top; margin-bottom: 30px;}
    .text-prompt{margin-left: 55px;font-size: 18px;color: #7F7F7F;font-weight: 700;}
    .text-prompt .left{float: left;}
    .text-prompt .right{float: left;margin-left: 30px;}
    .text-prompt .right ul li{margin-bottom: 15px;}
    .text-prompt .right a{display: block;width: 90px;height: 30px;text-align: center;line-height: 30px;color: #E7FEFF;letter-spacing: 2px;background-color: #3C75F6;text-decoration: none;font-weight: normal;font-size: 14px;border-radius: 3px;margin-top: 20px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content404">
            <div class="inner">
                <h1>访问出错了</h1>
                <div class="text-prompt">
                    <div class="left">可能原因:</div>
                    <div class="right">
                        <ul>
                           
                            <asp:Literal ID="litMsg" runat="server"></asp:Literal>
                        </ul>
                        <a href="/Admin/Default.aspx">返回首页</a>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
