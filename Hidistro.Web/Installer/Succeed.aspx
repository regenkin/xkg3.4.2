<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Succeed.aspx.cs" Inherits="Hidistro.UI.Web.Installer.Succeed" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>恭喜，您的产品已安装成功！</title>
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { LoadSetting() });

        function LoadSetting() {
            $.ajax({
                url: "./Succeed.aspx?callback=true",
                type: "POST",
                dataType: "text",
                success: function (msg) {
                    if (msg == "true") {

                    }
                    else {
                        alert(msg);
                    }
                },
                error: function (xmlHttpRequest, error) {
                    alert(error);
                }
            });
        }
    </script>
<link rel="stylesheet" type="text/css" href="style/install.css"/>
</head>
<body>
<form id="form1" runat="server">
<div class="header">
    <div class="header_c">
      <div class="l_logo"><img src="images/process/weilog.png" /></div>
      <div class="r_word">
         <p><a href="#">退出安装</a></p>
     
      </div>
    </div>
</div>

<div class="main">
   <div class="main_c">
      <p class="m_hd">恭喜，您的产品已安装成功！</p>
      <div class="m_bd">
         <div class="success">
            <h2>公众帐号配置</h2>
            <div class="prompt">请将URL与TOKEN配置到 微信公众平台 -功能-高级功能-开发模式下。</div>
            <div class="url_token">
              <p><span>URL：</span><em><asp:Literal runat="server" ID="txtUrl"/></em></p>
              <p><span>TOKEN：</span><em><asp:Literal runat="server" ID="txtToken"/></em></p>
            </div>
         </div>
         <div class="btn"><span><a href="../Admin/Login.aspx">登录管理页面</a></span></div>
      </div>
   </div>
</div>

<div class="footer">
   <p>Copyright 2015 pufang.net all Rrghts Reserved.本产品资源均为 普方软件有限公司 版权所有</p>
</div>
</form>
</body>
</html>
