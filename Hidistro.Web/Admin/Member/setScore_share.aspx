<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="setScore_share.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.setScore_share" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .errorInput{border:1px,solid;border-color:#a94442;}
        .normalInput{border:1px,solid;border-color:#cccccc;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $('input[type=text]').each(function () {
                $(this).on('blur',function () {                   
                    id = $(this).attr("id");                    
                    var btn;                  

                    if (id.indexOf("txt_ShareScore") > 0)
                    {
                        btn = $('#<%=btn_shareSave%>');
                        var flag1 = testInput($('#<%=txt_ShareScore.ClientID%>'));
                        if (!flag1) {
                            $(btn).attr('disabled', 'disabled');
                        }
                        else {
                            $(btn).removeAttr('disabled');
                        }
                    }

                });

            });


            $("div[name='enableDiv']").each(function () {
                $(this).click(function () {

                    var cls = $(this).attr("class");
                    var enable = "false";
                    if (cls == "switch-btn") {

                        $(this).empty();
                        $(this).append("已关闭 <i></i>")
                        $(this).removeClass();
                        $(this).addClass("switch-btn off");
                        enable = "false";

                    }
                    else {
                        $(this).empty();
                        $(this).append("已开启 <i></i>")
                        $(this).removeClass();
                        $(this).addClass("switch-btn");
                        enable = "true";
                    }
                    var type = 0;
                    var id = $(this).attr("id");
                    if (id == "signEnableBtn") {
                        type = 0;
                    }
                    else if (id == "shoppingEnableBtn") {
                        type = 1;
                    }
                    else {
                        type = 2;
                    }

                    $.ajax({
                        type: "post",
                        url: "ScoreConfigHandler.ashx",
                        data: { type: type, enable: enable },
                        dataType: "text",
                        success: function (data) {
                            var divId = "";
                            var msg = "";
                            if (type == 0)
                            {
                                divId = "signmainDiv";
                                msg = "签到送积分";
                            }
                            else if (type == 1)
                            {
                                divId = "shoppingmainDiv";
                                msg = "购物送积分";
                            }
                            else if(type==2)
                            {
                                divId = "sharemainDiv";
                                msg = "分享送积分";
                            }
                            if (enable == 'true') {                               
                                showMsg(msg+'已开启！');
                                $('#'+divId).css('display', '');   
                            }
                            else {
                                showMsg(msg + '已关闭！');
                                $('#' + divId).css('display', 'none');
                            }
                        }
                    });

                });

            });
        
        });



        function testInput(obj)
        {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;
            if (id == "ctl00_ContentPlaceHolder1_txtEverDayScore") {
                regex = /^[0-9]*$/;
                parent = $(obj).parent().parent().parent();
                
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txtStraightDay") {
                        regex = /^[0-9]*$/;
                        parent = $(obj).parent().parent().parent();
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txt_sign_RewardScore") {
                        regex = /^[0-9]*$/;
                        parent = $(obj).parent().parent().parent();
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txt_ShareScore") {
                        regex = /^[0-9]*$/;
                        parent = $(obj).parent().parent().parent();
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txt_ShoppingScore") {
                        regex = /^[0-9]*$/;
                        parent = null
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txt_OrderValue") {
                        regex = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
                    }
                    if (id == "ctl00_ContentPlaceHolder1_txt_shopping_RewardScore") {
                        regex = /^[0-9]*$/;
                    }

                    if (testRegex(regex, content)) {
                        if (parent != null) {
                            $(parent).removeClass();
                            $(parent).addClass("form-group");
                        }
                        else {
                            $(obj).removeClass();
                            $(obj).addClass("form-control");
                        }
                        return true;
                    }
                    else {
                        if (parent != null) {
                            $(parent).removeClass();
                            $(parent).addClass("form-group has-error");
                        }
                        else {
                            $(obj).removeClass();
                            $(obj).addClass("form-control errorInput");
                        }
                        return false;
                    }
        }

        function testRegex(rgx,str)
        {
            if (str == "") return true;
            return result = rgx.test(str);
        }


        function showMsg(msg) {
            HiTipsShow(msg, 'success');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="thisForm" runat="server" class="form-horizontal">
    <input type="hidden" id="urlType"  value="<%=_urlType%>"/>
    <div class="page-header">
        <h2>会员积分设置</h2>        
    </div>
    <div class="play-tabs">
       
        <ul id="tabHeader" class="nav nav-tabs" role="tablist">
            <li role="presentation" id="signTab" ><a href="setScore_sign.aspx">签到送积分</a></li>
            <li role="presentation" id="shoppingTab"><a href="setScore_shopping.aspx">
                购物送积分</a></li>
            <li role="presentation" id="shareTab" class="active"><a href="setScore_shopping.aspx">
                分享送积分</a></li>
        </ul>
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="shareScoreDiv">
                <div class="set-switch">
                    <strong>分享送积分</strong>
                    <p>
                        启用后，当有人通过分享链接成为店铺会员，将给分享者发放积分。
                    </p>
                    <div id="shareEnableBtn" name="enableDiv" class="<%=_shareEnable?"switch-btn":"switch-btn off" %>" >
                        <%=_shareEnable?"已开启":"已关闭"%>
                        <i></i>
                    </div>
                </div>
                <div id="sharemainDiv" style="<%=_shareEnable?"": "display:none" %>">
                <div class="form-group">
                    <div class="form-inline">
                        <label for="inputEmail3" class="col-xs-2 control-label resetSize" style="font-weight: bold;"><em>
                            *</em>分享奖励积分：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txt_ShareScore" CssClass="form-control resetSize" runat="server" /><label style="font-weight: normal;
                                margin-left: 3px;">分</label>
                        </div>
                    </div>
                </div>
                <div class="form-group" style="width: 110%;">
                    <div class="col-xs-offset-2 marginl">
                        <asp:Button runat="server" class="btn btn-success inputw100" ID="btn_shareSave" Text="保存" />
                    </div>
                </div>
                </div>
            </div>

        </div>
    </div>
</form>
</asp:Content>

