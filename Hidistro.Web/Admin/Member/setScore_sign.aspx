<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="setScore_sign.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.setScore_sign" %>
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
            $('#tabHeader>li').each(function () {
                $(this).click(function (){
                    $('#tabHeader>li').each(
                        function () {
                            $(this).removeClass();
                        });
                    $(this).addClass('active');
                    var id = $(this).attr("id").replace("Tab", "ScoreDiv");
                    $("div[role='tabpanel']").each(function () {
                        $(this).removeClass();
                        $(this).addClass("tab-pane");
                    });
                    $('#' + id).removeClass();
                    $('#' + id).addClass("tab-pane active");                    
                });
            });

            var type = $('#urlType').val();
            if(type!="")
            {
                $('#' + type + 'Tab').click();
            }


            $('input[type=text]').each(function () {
                $(this).on('blur',function () {                   
                    id = $(this).attr("id");                    
                    var btn;
                    if (id.indexOf("txtEverDayScore") > 0 || id.indexOf("txtStraightDay") > 0 || id.indexOf("txt_sign_RewardScore") > 0)
                    {
                        btn = $('#<%=btn_signSave.ClientID%>');
                        var flag1 = testInput($('#<%=txtEverDayScore.ClientID%>'));
                        var flag2 = testInput($('#<%=txtStraightDay.ClientID%>'));
                        var flag3 = testInput($('#<%=txt_sign_RewardScore.ClientID%>'));
                        if(!(flag1 && flag2 && flag3))
                        {
                            $(btn).attr('disabled', 'disabled');
                        }
                        else
                        {
                            $(btn).removeAttr('disabled');
                        }
                    }
                });
            });

            $('#mySwitch').on('switch-change', function (e, data) {
                var type = "0";
                var enable = data.value;
                $.ajax({
                    type: "post",
                    url: "ScoreConfigHandler.ashx",
                    data: { type: type, enable: enable },
                    dataType: "text",
                    success: function (data) {
                        if (data == "保存成功") {
                            if (enable == true) {
                                $('#signmainDiv').show();
                            }
                            else {
                                $('#signmainDiv').hide();
                            }
                        }
                        else {
                            ShowMsg("修改失败（" + data + ")");
                        }
                    }
                });
            });
         
            //var continuityEnable = "<%=_continuityEnable%>";
            if ($('#chkContinuity').prop("checked") == true)
            {                              
                $('#<%=txtStraightDay.ClientID%>').removeAttr("disabled");
                $('#<%=txt_sign_RewardScore.ClientID%>').removeAttr("disabled");
                $('#<%=hdContinuityEnable.ClientID%>').val(1); 
            }
            else
            {                
                $('#<%=txtStraightDay.ClientID%>').attr("disabled", "disabled");
                $('#<%=txt_sign_RewardScore.ClientID%>').attr("disabled", "disabled");
                $('#<%=hdContinuityEnable.ClientID%>').val(0);
            }
            
            $('#chkContinuity').change(function () {
                if ($('#chkContinuity').prop("checked") == true) {                    
                    $('#<%=txtStraightDay.ClientID%>').removeAttr("disabled");
                    $('#<%=txt_sign_RewardScore.ClientID%>').removeAttr("disabled");
                    $('#<%=hdContinuityEnable.ClientID%>').val(1);
                    $('#chkContinuity').attr("checked", "checked");
                }
                else
                {
                    $('#<%=txtStraightDay.ClientID%>').attr("disabled", "disabled");
                    $('#<%=txt_sign_RewardScore.ClientID%>').attr("disabled", "disabled");
                    $('#<%=hdContinuityEnable.ClientID%>').val(0);
                    $('#chkContinuity').removeAttr("checked");
                }
            });
        });



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
    <asp:HiddenField ID="hdContinuityEnable" runat="server" />

    <div class="page-header">
        <h2>会员积分设置</h2>        
    </div>
    <div class="play-tabs">       
        <ul id="tabHeader" class="nav nav-tabs" role="tablist">
            <li role="presentation" id="signTab" class="active"><a href="setScore_sign.aspx">签到送积分</a></li>
            <li role="presentation" id="shoppingTab" ><a href="setScore_shopping.aspx">购物送积分</a></li>
        </ul>
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="signScoreDiv">
                <div class="form-group">
                    <label for="" class="col-xs-2 control-label">是否开启</label>
                    <div class="col-xs-4">
                        <div class="switch" id="mySwitch">
                            <input type="checkbox" <%=_signEnable ? "checked" : ""%> />
                        </div>
                    </div>                    
                </div>
                <div id="signmainDiv" style="<%=_signEnable?"": "display:none" %>">
                    <div class="form-group" >
                        <div class="form-inline">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>签到一次奖励</label>
                            <div class="col-xs-3">
                                <asp:TextBox ID="txtEverDayScore" CssClass="form-control" runat="server" /><label style="font-weight:normal ; margin-left:3px;" >分</label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-inline">
                            <label for="inputEmail3" class="col-xs-2 control-label">
                                <input id="chkContinuity" type="checkbox" <%=_continuityEnable ? "checked" : ""%> /> 每连续签到</label>
                            <div class="col-xs-6">
                                <asp:TextBox ID="txtStraightDay" CssClass="form-control" runat="server" Width="100" />
                                <label style="font-weight: normal; margin-left: 3px;"> 天</label>
                                <label style="font-weight: normal; margin-left: 3px;"> 奖励</label>
                                <asp:TextBox ID="txt_sign_RewardScore" CssClass="form-control" runat="server" Width="100" />
                                <label style="font-weight: normal; margin-left: 3px;">分</label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="width: 110%;">                    
                        <div class="col-xs-offset-2 marginl" >
                            <asp:Button runat="server" ID="btn_signSave"  class="btn btn-success inputw100" Text="保存" />
                        </div>                  
                    </div>
                </div>
            </div>

        </div>
    </div>
</form>
</asp:Content>

