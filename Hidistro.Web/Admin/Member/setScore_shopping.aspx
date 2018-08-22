<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="setScore_shopping.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.setScore_shopping" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/bootstrapSwitch.css" />
    <script type="text/javascript" src="../js/bootstrapSwitch.js"></script>
    <style type="text/css">
        .errorInput{border:1px,solid;border-color:#a94442;}
        .normalInput{border:1px,solid;border-color:#cccccc;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type=text]').each(function () {
                $(this).on('blur', function () {
                    id = $(this).attr("id");
                    var btn;

                    if (id.indexOf("txt_ShoppingScore") > 0 || id.indexOf("txt_OrderValue") > 0 || id.indexOf("txt_shopping_RewardScore") > 0) {
                        btn = $('#<%=btn_shoppingSave.ClientID%>');
                        var flag1 = testInput($('#<%=txt_ShoppingScore.ClientID%>'));
                        var flag2 = testInput($('#<%=txt_OrderValue.ClientID%>'));
                        var flag3 = testInput($('#<%=txt_shopping_RewardScore.ClientID%>'));
                        if (!(flag1 && flag2 && flag3)) {
                            $(btn).attr('disabled', 'disabled');
                        }
                        else {
                            $(btn).removeAttr('disabled');
                        }
                    }


                });

            });

            $('#mySwitch').on('switch-change', function (e, data) {
                var type = "1";
                var enable = data.value;
                $.ajax({
                    type: "post",
                    url: "ScoreConfigHandler.ashx",
                    data: { type: type, enable: enable },
                    dataType: "text",
                    success: function (data) {
                        if (data == "保存成功") {
                            if (enable == true) {
                                $('#shoppingmainDiv').show();
                            }
                            else {
                                $('#shoppingmainDiv').hide();
                            }
                        }
                        else {
                            ShowMsg("修改失败（" + data + ")");
                        }
                    }
                });
            });

            $('#<%=chk.ClientID%>').click(function () {
                if ($(this).attr("checked") == "checked") {
                    $(this).attr("checked", false);
                } else {
                    $(this).attr("checked", true);
                }
                var chk_enable = $(this).attr("checked");
                setCheckEnable(chk_enable);
            });
            var chk_enable = $('#<%=chk.ClientID%>').attr("checked");
            setCheckEnable(chk_enable);
        });

        function setCheckEnable(val)
        {
            if (val == "checked") {
                $('#<%=txt_OrderValue.ClientID%>').removeAttr("disabled");
                $('#<%=txt_shopping_RewardScore.ClientID%>').removeAttr("disabled");
            }
            else {
                $('#<%=txt_OrderValue.ClientID%>').attr("disabled", "disabled");
                $('#<%=txt_shopping_RewardScore.ClientID%>').attr("disabled", "disabled");
            }
        }


        function testInput(obj) {
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
            <li role="presentation" id="signTab"><a href="setScore_sign.aspx">签到送积分</a></li>
            <li role="presentation" id="shoppingTab" class="active"><a href="setScore_shopping.aspx">购物送积分</a>
            </li>
        </ul>
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="shoppingScoreDiv">               
                <div class="form-group">
                    <label for="" class="col-xs-2 control-label">是否开启</label>
                    <div class="col-xs-4">
                        <div class="switch" id="mySwitch">
                            <input type="checkbox" <%=_shoppingEnable ? "checked" : ""%> />
                        </div>
                    </div>                    
                </div>
                <div id="shoppingmainDiv" style="<%=_shoppingEnable?"": "display:none" %>">
                    <div class="form-group">
                        <div class="form-inline">
                            <label for="" class="col-xs-2 control-label">购物成功每</label>
                            <div class="col-xs-6">
                                <asp:TextBox ID="txt_ShoppingScore" CssClass="form-control" runat="server" />
                                <label style="font-weight: normal; margin-left: 3px;">元奖励1分 （不包含运费）</label> 
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-inline">                           
                            <label for="" class="col-xs-2 control-label"><asp:CheckBox runat="server" Checked="true"  ID="chk"/> 单笔订单满</label>
                            <div class="col-xs-6">             
                                <asp:TextBox ID="txt_OrderValue" CssClass="form-control" runat="server" Width="100" />
                                <label style="font-weight: normal;margin-left: 3px;">元，奖励</label>
                                <asp:TextBox ID="txt_shopping_RewardScore" CssClass="form-control" runat="server" Width="100" />
                                <label style="font-weight: normal; margin-left: 3px;">分</label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="width: 110%;">
                        <div class="col-xs-offset-2 marginl">
                            <asp:Button runat="server" class="btn btn-success inputw100" ID="btn_shoppingSave" Text="保存" />
                        </div>
                    </div>
                </div>
                
            </div>       
        </div>
    </div>
</form>
</asp:Content>

