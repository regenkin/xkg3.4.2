<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Activation.aspx.cs" Inherits="Hidistro.UI.Web.Installer.Activation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" type="text/css" href="style/install.css"/>
<script language="javascript" type="text/javascript">
    // <!CDATA[
    function btnContinue_onclick() {
        document.location = "Activation.aspx";
    }
    // ]]>

    function ConfrimInstall() {
        var conf = confirm("确定要退出安装吗？");
        if (conf) {
            window.open('', '_self', '');
            window.close();
        }
    }
    </script>


    <title>请激活您的产品</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
    <div class="header_c">
      <div class="l_logo"><img src="images/process/weilog.png" /></div>
      <div class="r_word">
         <p><a href="#" onclick="ConfrimInstall()">退出安装</a></p>
       
      </div>
    </div>
</div>

<div class="main">
   <div class="main_c">
      <p class="m_hd">请激活您的产品</p>
      <div class="m_bd">
         <div class="m_ac">
            <div class="ac_pic"><img src="images/process/2w.jpg" /></div>
            <div class="ac_text">
              <p>获取免费激活码?</p>
              1、使用微信扫描左侧二维码关注我们的公众账号。<br />
              2、发送关键字“key”。   <br />
              3、等待公众账号发送激活码。<br />
            </div>
         </div>
         <div class="ac_enter">激活码：<input  id="txtcode" runat="server" type="text" /></div>
         <span style=" margin-left:172px; display:block;"><asp:Label ID="lblErrMessage" runat="server" CssClass="exp"></asp:Label></span>
         <div class="btn"><p onclick="history.go(-1);">上一步</p>
         <b><asp:Button ID="btnInstall"  
                 runat="server" Text="下一步" CssClass="done" onclick="btnInstall_Click" /></b>
                 
                 </div>
      </div>
   </div>
</div>

<div class="footer">
   <p>Copyright 2015 pufang.net all Rrghts Reserved.本产品资源均为 普方软件有限公司 版权所有</p>
</div>
    </form>
</body>
</html>
