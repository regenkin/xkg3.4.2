<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OfflinePay.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Settings.OfflinePay" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl"
    TagPrefix="Kindeditor" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto; margin-left:5px; margin-right:5px;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#rd3").click();
            //var val = $('#lb1').text();
            //if(val=="已启用")
            //{
            //    $("#offlineEnable").attr("class", "switch-btn");
            //}
            //else
            //{
            //    $("#offlineEnable").attr("class", "switch-btn off");
            //}
            //val = $('#lb2').text();
            //if (val == "已启用") {
            //    $("#PodEnable").attr("class", "switch-btn");
            //}
            //else {
            //    $("#PodEnable").attr("class", "switch-btn off");
            //}
        });

        function change(obj)
        {
            var val = obj.value;
            if(val=="0")
            {
                $('#payTypeName').text("银行卡转帐");
                $('#bankdiv').removeClass();
                $('#bankdiv').addClass("tab-pane active");
                $('#alipaydiv').removeClass();
                $('#alipaydiv').addClass("tab-pane");
                $('#otherdiv').removeClass();
                $('#otherdiv').addClass("tab-pane");
            }
            else if(val=="1")
            {
                $('#payTypeName').text("支付宝转帐");
                $('#bankdiv').removeClass();
                $('#bankdiv').addClass("tab-pane");
                $('#alipaydiv').removeClass();
                $('#alipaydiv').addClass("tab-pane active");
                $('#otherdiv').removeClass();
                $('#otherdiv').addClass("tab-pane");
            }
            else{
                $('#payTypeName').text("其他方式转账");
                $('#bankdiv').removeClass();
                $('#bankdiv').addClass("tab-pane");
                $('#alipaydiv').removeClass();
                $('#alipaydiv').addClass("tab-pane");
                $('#otherdiv').removeClass();
                $('#otherdiv').addClass("tab-pane active");
            }
        }

        function setEnable(obj) {

            var type = "-2";
            if (obj.id == "offlineEnable")
            {
                type = "-2";
            }
            else
            {
                type = "-1";
            }
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "PayConfigHandler.ashx",
                data: { type: type, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('线下支付已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('线下支付已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function beforeSaveData(obj)
        {
            var content = editor.html();
            if (content == "") {
                errAlert("请输入内容!");
                $('#fkContent').focus();
                return false;
            }
            return true;
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>设置收款账号</h2>
        </div>
        <div>
        <ul class="nav nav-tabs" role="tablist">
            <li role="presentation"><a href="WeixinPay.aspx">微信支付</a></li>
            <li role="presentation"><a href="Alipay.aspx">支付宝</a></li>
            <%--<li role="presentation" ><a href="ChinaBank.aspx">网银在线</a></li>--%>
            <li role="presentation"><a href="ShengPay.aspx">盛付通</a></li>
            <li role="presentation" class="active"><a href="OfflinePay.aspx">线下支付</a></li>
            <li role="presentation"><a href="COD.aspx">货到付款</a></li>
        </ul>
        <div>
            <div class="set-switch">
                <strong>线下支付收款设置</strong>               
                <p>线下支付提供：银行卡转账、支付宝转账或其他方式供您选择，请认真填写并核对相关信息，</p>
                <p>如若因手动填写错误而造成的一切损失，本平台概不负责！</p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
                <div style="width: auto;">
                    <div id="otherdiv" class="tab-pane active" style="width: auto;">
                        <div class="form-group" style="margin-left:5px; width:auto;" >
                            <Kindeditor:KindeditorControl ID="fkContent" runat="server"  Width="670" Height="200"/>                          
                        </div>
                        <div class="form-group" style="text-align:center; width:60%;">
                            <div class="col-xs-offset-2 marginl">
                                <asp:Button runat="server" OnClick="Unnamed_Click" class="btn btn-success inputw100"
                                    OnClientClick="return beforeSaveData(this)" Text="保存" />
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </div>
    </div>
  </form>
</asp:Content>
