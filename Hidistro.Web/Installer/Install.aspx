<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Install.aspx.cs" Inherits="Hidistro.UI.Web.Installer.Install" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>普方分销 安装向导</title>
    <script type="text/javascript" language="javascript" src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js"></script>
    <link href="style/install.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnTest").bind("click", function () { RunTest(); });
        });

        var dbServer, dbName, dbUsername, dbPassword;
        var username, email, password, password2;
        var isAddDemo, testSuccessed = false;
        var siteName, siteDescription;

        function GetValues() {
            dbServer = $("#txtDbServer").val();
            dbName = $("#txtDbName").val();
            dbUsername = $("#txtDbUsername").val();
            dbPassword = $("#txtDbPassword").val();

            username = $("#txtUsername").val();
            email = $("#txtEmail").val();
            password = $("#txtPassword").val();
            password2 = $("#txtPassword2").val();

            isAddDemo = $("#chkIsAddDemo").attr("checked");
        }

        function Callback(action) {
            var resultData;

            $.ajax({
                url: "Install.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: {
                    isCallback: "true",
                    action: action,
                    DBServer: dbServer,
                    DBName: dbName,
                    DBUsername: dbUsername,
                    DBPassword: dbPassword,
                    Username: username,
                    Email: email,
                    Password: password,
                    Password2: password2,
                    IsAddDemo: isAddDemo,
                    TestSuccessed: testSuccessed
                },
                async: false,
                success: function (result) {
                    resultData = result;
                }
            });

            return resultData;
        }

        function RunTest() {
            if (testSuccessed && (confirm("上一次的安装环境测试已成功，您确定要再次测试吗？") == false)) {
                return;
            }

            DisableButtons();
            GetValues();
            var resultData = Callback("Test")

            if (resultData.Status == "OK") {
                testSuccessed = true;
                alert("测试成功，当前环境符合安装要求");
            }
            else {
                testSuccessed = false;
                ShowErrors(resultData);
            }

            EnableButtons();
        }

        function ShowErrors(resultData) {
            var msg = "";
            $.each(resultData.ErrorMsgs, function (i, item) {
                msg += item.Text + "\r\n";
            });
            alert(msg);
        }

        function ConfrimInstall() {
            var conf = confirm("确定要退出安装吗？");
            if (conf) {
                window.open('', '_self', '');
                window.close();
            }
        }

        function EnableButtons() {
            $("#btnTest").removeAttr("disabled");
            $("#btnInstall").removeAttr("disabled");
        }

        function DisableButtons() {
            $("#btnTest").attr({ "disabled": "disabled" });
            $("#btnInstall").attr({ "disabled": "disabled" });
        }

    </script>
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
            <p class="m_hd">请填写您的产品安装信息</p>
            <div class="m_bd">
            <asp:Label ID="lblErrMessage" runat="server" CssClass="exp"></asp:Label>
                <div class="config_info">
                    <div class="c_i_l">
                    
                        <h2>
                            数据库配置</h2>
                        <p>
                            <span>数据库地址：</span><asp:TextBox ID="txtDbServer" runat="server" CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>数据库名称：</span><asp:TextBox ID="txtDbName" runat="server" CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>数据库登录名：</span><asp:TextBox ID="txtDbUsername" runat="server" CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>数据库密码：</span><asp:TextBox ID="txtDbPassword" TextMode="Password" runat="server"
                                CssClass="txt" /><em>*</em></p>
                    </div>
                    <div class="c_i_r">
                        <h2>
                            管理员设置<asp:HiddenField ID="hdfSiteName" runat="server" Value="普方分销" /></h2>
                        <p>
                            <span>用户名：</span><asp:TextBox ID="txtUsername" runat="server" CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>电子邮件：</span><asp:TextBox ID="txtEmail" runat="server" CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>登录密码：</span><asp:TextBox ID="txtPassword" TextMode="Password" runat="server"
                                CssClass="txt" /><em>*</em></p>
                        <p>
                            <span>确认密码：</span><asp:TextBox ID="txtPassword2" TextMode="Password" runat="server"
                                CssClass="txt" /><em>*</em></p>
                        
                    </div>
                </div>
                <p class="addDemo" style="display:none;"><asp:CheckBox ID="chkIsAddDemo" runat="server" />&nbsp;&nbsp;<span>添加演示数据</span></p>

                <asp:Label ID="litSetpErrorMessage" runat="server" CssClass="exp"></asp:Label>
                 

                <div class="btn">
                    <em onclick="history.go(-1);">上一步</em>
                    <span><input id="btnTest" name="btnTest" type="button" value="测试安装环境" class="test" /></span>
                    <samp><asp:Button ID="btnInstall" runat="server" Text="开始安装" CssClass="done" /></samp>
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
