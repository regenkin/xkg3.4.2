<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Login" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html>
<html style="overflow-y:auto;">
<head><Hi:HeadContainer runat="server" />
    <title><%=htmlWebTitle %></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="renderer" content="webkit" />
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
    <script type="text/javascript" src="/admin/js/browserdetect.js"></script>
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="./js/jquery.formvalidation.js"></script>
    <link rel="stylesheet" href="./css/common.css" />
    <style type="text/css">
    	html,body{width: 100%;height: 100%;background-color: #F1F1F1; position: relative;}
    	.dislogin{width: 340px;padding: 100px 20px 20px; position: absolute;left: 50%;margin-left: -170px; background:url(./images/login-title.png) no-repeat 25px 35px #fff;box-shadow: 0 0 300px #fff;}
    	.dislogin .form-group{margin-bottom: 10px;position: relative;}
    	.dislogin .form-group label{position: absolute;left: 10px;top: 0;height: 40px;line-height: 40px;color: #B3B3B3;z-index: 10;}
    	.dislogin-input .form-control{height: 40px;padding: 6px 12px 6px 65px;}
    	.dislogin .vercode{position: relative;}
    	.dislogin .vercode .form-control{width: 195px;}
    	.dislogin .vercode .imgcode{position: absolute;top: 0px;right: 0; width: 77px;height: 40px;overflow: hidden;}
    	.dislogin .vercode .error{position: absolute;width: 18px;height: 18px;left: 200px; top: 8px;}
        .dislogin-btn .btn{display: block;width: 100%;height: 40px;}
    	form{margin-bottom: 30px;}
    	.wechat-code{position: relative;height: 95px;}
    	.wechat-code .code-img{width: 95px;height: 95px;position: absolute;left: 0;top: 0;}
    	.wechat-code p{margin-left: 105px;color: #464646;text-shadow:0 0 0 #A5A5A5;line-height: 25px;padding-top: 11px;}
    	.wechatimg,.shoppingimg,.modimg,.userimg{position: absolute;width: 60px;height: 60px;background-image: url(./images/loginbg2.png);background-repeat: no-repeat;}
    	.wechatimg{left: -100px;top: 100px;background-position: 0 1px;}
    	.shoppingimg{right: -100px;top: 100px;background-position: 0 -62px;}
    	.modimg{left: -100px;bottom: 120px;background-position:15px -133px;}
    	.userimg{right: -100px;bottom: 120px;background-position:11px -199px;}
        small {clear:both;}
    </style>
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <script type="text/javascript">
        if (window != window.top) {
            top.location.href = location.href;
        }
    	$(function (){
    		var elemTop=($(window).height()-$('.dislogin').innerHeight())/2;
    		$('.dislogin').css('top',elemTop);
    		$(window).resize(function (){
    			var elemTop=($(window).height()-$('.dislogin').innerHeight())/2;
    			$('.dislogin').css('top',elemTop);
    		})
    	})
    </script>
</head>
<body>
	<div class="dislogin">
    <form id="aspnetForm" runat="server">
			<div class="form-group">
                <label class="control-label" for="username">用户名</label>
                <div class="dislogin-input">
                    <asp:TextBox ID="txtAdminName" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label" for="passwrod">密码</label>
                <div class="dislogin-input">
                    <asp:TextBox ID="txtAdminPassWord" CssClass="form-control" runat="server" TextMode="Password" MaxLength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="control-label" for="vercode">验证码</label>
                <div class="dislogin-input vercode clearfix">
                    <div class="imgcode fl">
						<img id="imgVerifyCode" alt="加载中..." src='<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx" %>'
                                        style="border-style: none" onclick="javascript:refreshCode();" title="点击图片获取新的验证码" />
                    </div>
                    <div class="error"><img id="img_txtCode" src="" alt="" /></div>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control fl" MaxLength="4"></asp:TextBox>
                </div>
            </div>
            <div class="dislogin-btn">
                <asp:Button ID="btnAdminLogin" runat="server" Text="点击登录" CssClass="btn btn-success" /> <Hi:SmallStatusMessage ID="lblStatus" runat="server" Visible="False" Width="260px" />
            </div>
		</form>
		<div class="wechatimg"></div>
		<div class="shoppingimg"></div>
		<div class="modimg"></div>
		<div class="userimg"></div>
	</div>
    
    <script type="text/javascript">
        function refreshCode() {
            var img = document.getElementById("imgVerifyCode");
            if (img != null) {
                var currentDate = new Date();
                img.src = '<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx?t=" %>' + currentDate.getTime();
            }
        }
        $(document).ready(function () {
            $("#img_txtCode").hide();
            $("#txtCode").keyup(function () {
                var value = $(this).val();
                var temp;
                if (value.length < 4) {
                    $("#img_txtCode").hide();
                    temp = "";
                }
                else if (value.length == 4) {
                    if (temp != value) {
                        $("#img_txtCode").show();
                        $.ajax({
                            url: "Login.aspx",
                            type: 'post', dataType: 'json', timeout: 10000,
                            data: {
                                isCallback: "true",
                                code: $("#txtCode").val()
                            },
                            async: false,
                            success: function (resultData) {
                                var flag = resultData.flag;
                                if (flag == "1") {
                                    $("#img_txtCode").attr("src", "images/true.gif");
                                }
                                else if (flag == "0") {
                                    $("#img_txtCode").attr("src", "images/false.gif");
                                }
                            }
                        });
                    }
                    temp = value;
                }
            });

            //$('#aspnetForm').formvalidation({
            //    'txtAdminName': {
            //        validators: {
            //            notEmpty: {
            //                message: '请输入用户名'
            //            }
            //        }
            //    },
            //    'txtAdminPassWord': {
            //        validators: {
            //            notEmpty: {
            //                message: '请输入密码'
            //            }
            //        }
            //    },
            //    'txtCode': {
            //        validators: {
            //            notEmpty: {
            //                message: '请输入验证码'
            //            },
            //            regexp: {
            //                regexp: /^(\w{4})$/,
            //                message: ''
            //            }
            //        }
            //    }
            //});
            $("label").click(function () { $(this).next().find("input")[0].select();})
        });
    </script>
</body>