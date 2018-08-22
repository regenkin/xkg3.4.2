<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CustomDistributorAddMembers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.CustomDistributorAddMembers" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            setGradeCheck();
            setDefualtGroupCheck();
            setCustomGroupCheck();
            setDateTime();
        });

        //加载会员等级选中状态
        function setGradeCheck() {
            var Grades = $('#<%=hdGrades.ClientID%>').val();
            if (Grades != "" && Grades != "-1") {
                $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                    var vl = $(this).val();
                    if (Grades == "0") {
                        $(this).prop('checked', true);
                    }
                    else {
                        if ((',' + Grades + ',').indexOf(',' + vl + ',') >= 0) {
                            $(this).prop('checked', true);
                        }
                    }
                });
            }
        }

        //加载默认分组选中状态
        function setDefualtGroupCheck() {
            var DefualtGroup = $('#<%=hdDefualtGroup.ClientID%>').val();
            if (DefualtGroup != "" && DefualtGroup != "-1") {
                $('#memberDefualtGroupdiv').find('input[type="checkbox"]').each(function () {
                    var vl = $(this).val();
                    if (DefualtGroup == "0") {
                        $(this).prop('checked', true);
                    }
                    else {
                        if ((',' + DefualtGroup + ',').indexOf(',' + vl + ',') >= 0) {
                            $(this).prop('checked', true);
                        }
                    }
                });
            }
        }

        //加载自定义分组选中状态
        function setCustomGroupCheck() {
            var CustomGroup = $('#<%=hdCustomGroup.ClientID%>').val();
            if (CustomGroup != "" && CustomGroup != "-1") {
                $('#memberCustomGroupdiv').find('input[type="checkbox"]').each(function () {
                    var vl = $(this).val();
                    if (CustomGroup == "0") {
                        $(this).prop('checked', true);
                    }
                    else {
                        if ((',' + CustomGroup + ',').indexOf(',' + vl + ',') >= 0) {
                            $(this).prop('checked', true);
                        }
                    }
                });
            }
        }

        function setDateTime() {
            var RegisterDate = $('#<%=hdRegisterDate.ClientID%>').val();
            var TradeDat = $('#<%=hdTradeDate.ClientID%>').val();
            if (RegisterDate != "") {
                $("input[name='RegisterDate'][value='" + RegisterDate + "']").attr("checked", true);
            } else {
                $("input[name='RegisterDate'][value='all']").attr("checked", true);
            }
            if (TradeDat != "") {
                $("input[name='TradeDate'][value='" + TradeDat + "']").attr("checked", true);
            } else {
                $("input[name='TradeDate'][value='all']").attr("checked", true);
            }
        }

        function setCheckText() {
            var TradeMoney1 = $("#ctl00_ContentPlaceHolder1_txtTradeMoney1").val();
            var TradeMoney2 = $("#ctl00_ContentPlaceHolder1_txtTradeMoney2").val();

            var TradeNum1 = $("#ctl00_ContentPlaceHolder1_txtTradeNum1").val();
            var TradeNum2 = $("#ctl00_ContentPlaceHolder1_txtTradeNum2").val();

            var reg = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
            if (TradeMoney1 != "") {
                if (!reg.test(TradeMoney1)) {
                    ShowMsg("请正确输入交易金额", false);
                    return false;
                }
            }
            if (TradeMoney2 != "") {
                if (!reg.test(TradeMoney2)) {
                    ShowMsg("请正确输入交易金额", false);
                    return false;
                }
            }
            reg = /^[0-9]*$/;
            if (TradeNum1 != "") {
                if (!reg.test(TradeNum1)) {
                    ShowMsg("请正确输入交易次数", false);
                    return false;
                }
            }
            if (TradeNum2 != "") {
                if (!reg.test(TradeNum2)) {
                    ShowMsg("请正确输入交易次数", false);
                    return false;
                }
            }

            var grade = "";
            var DefualtGroup = "";
            var CustomGroup = "";

            //会员等级
            var checkGrade = $(".memberGradeCheck:checked");
            if (checkGrade.size() == 0) {
                grade = "-1";
            } else if (checkGrade.size() == $(".memberGradeCheck").size()) {
                grade = "0";
            } else {
                checkGrade.each(function () {
                    grade += $(this).val() + ',';
                });
                grade = grade.substring(0, grade.length - 1);
            }
            //默认分组
            var checkDefualtGroup = $(".DefualtGroup:checked");
            if (checkDefualtGroup.size() == 0) {
                DefualtGroup = "-1";
            } else if (checkDefualtGroup.size() == $(".DefualtGroup").size()) {
                DefualtGroup = "0";
            } else {
                checkDefualtGroup.each(function () {
                    DefualtGroup += $(this).val() + ',';
                });
                DefualtGroup = DefualtGroup.substring(0, DefualtGroup.length - 1);
            }
            //自定义分组
            var checkCustomGroup = $(".CustomGroup:checked");
            if (checkCustomGroup.size() == 0) {
                CustomGroup = "-1";
            } else if (checkCustomGroup.size() == $(".CustomGroup").size()) {
                CustomGroup = "0";
            } else {
                checkCustomGroup.each(function () {
                    CustomGroup += $(this).val() + ',';
                });
                CustomGroup = CustomGroup.substring(0, CustomGroup.length - 1);
            }

            var RegisterDate = $(".Register:checked").val();
            var TradeDate = $(".Trade:checked").val();

            $('#<%=hdGrades.ClientID%>').val(grade);
            $('#<%=hdDefualtGroup.ClientID%>').val(DefualtGroup);
            $('#<%=hdCustomGroup.ClientID%>').val(CustomGroup);
            $('#<%=hdRegisterDate.ClientID%>').val(RegisterDate);
            $('#<%=hdTradeDate.ClientID%>').val(TradeDate);

            return true;
        }

        function AddMembers() {
            location.href = "/Admin/member/CustomDistributorDetail.aspx?GroupId=<%=currentGroupId%>";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div id="datadiv">
            <div class="page-header">
                <h2>分组管理><asp:Literal runat="server" ID="GroupName"></asp:Literal></h2>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label">店铺名称：</label>
                <div class="col-xs-4" style="margin-top: 5px;">
                    <Hi:TrimTextBox ID="txtStroeName" CssClass="form-control inputw150 resetSize" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label resetSize">交易总金额：</label>
                <div class="col-xs-9">
                    <div class="form-inline">
                        <Hi:TrimTextBox runat="server" CssClass="form-control resetSize inputw150" ID="txtTradeMoney1" />
                        <label for="sellshop6">&nbsp;～&nbsp;&nbsp;</label>
                        <Hi:TrimTextBox runat="server" CssClass="form-control resetSize inputw150" ID="txtTradeMoney2" />&nbsp;&nbsp;元 
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label resetSize">交易次数：</label>
                <div class="col-xs-9">
                    <div class="form-inline">
                        <Hi:TrimTextBox ID="txtTradeNum1" CssClass="form-control resetSize inputw150" runat="server" />
                        <label for="sellshop6">&nbsp;～&nbsp;&nbsp;</label><Hi:TrimTextBox ID="txtTradeNum2" CssClass="form-control resetSize inputw150" runat="server" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label">用户等级：</label>
                <div class="col-xs-9">
                    <div id="memberGradediv" style="padding-top: 6px;">
                        <%=GetMemberGrande() %>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label">所在分组：</label>
                <div class="col-xs-9">
                    <div id="memberDefualtGroupdiv" style="padding-top: 6px;">
                        <label class="middle mr20">
                            <input type="checkbox" value="1" class="DefualtGroup">新会员
                        </label>
                        <label class="middle mr20">
                            <input type="checkbox" value="2" class="DefualtGroup">活跃会员
                        </label>
                        <label class="middle mr20">
                            <input type="checkbox" value="3" class="DefualtGroup">沉睡会员
                        </label>
                    </div>
                </div>
                <div class="col-xs-9" style="padding-top: 12px;">
                    <div id="memberCustomGroupdiv" style="padding-top: 6px;">
                        <%=GetMemberCustomGroup() %>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label">用户注册时间：</label>
                <div class="col-xs-6" style="padding-top: 6px;">
                    <label class="middle mr20">
                        <input type="radio" name="RegisterDate" class="Register" value="all">不限</label>
                    <label class="middle mr20">
                        <input type="radio" name="RegisterDate" class="Register" value="week">一周内</label>
                    <label class="middle mr20">
                        <input type="radio" name="RegisterDate" class="Register" value="month">一月个内</label>
                    <label class="middle mr20">
                        <input type="radio" name="RegisterDate" class="Register" value="threeMonth">三个月内</label>
                    <label class="middle mr20">
                        <input type="radio" name="RegisterDate" class="Register" value="moreMonth">三个月以上</label>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-3 control-label">最后成交时间：</label>
                <div class="col-xs-6" style="padding-top: 6px;">
                    <label class="middle mr20">
                        <input type="radio" name="TradeDate" class="Trade" value="all">不限</label>
                    <label class="middle mr20">
                        <input type="radio" name="TradeDate" class="Trade" value="week">一周内</label>
                    <label class="middle mr20">
                        <input type="radio" name="TradeDate" class="Trade" value="month">一月个内</label>
                    <label class="middle mr20">
                        <input type="radio" name="TradeDate" class="Trade" value="threeMonth">三个月内</label>
                    <label class="middle mr20">
                        <input type="radio" name="TradeDate" class="Trade" value="moreMonth">三个月以上</label>
                </div>
            </div>
            <br />
            <br />
            <div class="form-group">
                <div class="col-xs-offset-2 marginl">
                    <table>
                        <tr>
                            <td style="width: 120px;">
                                <asp:Button runat="server" ID="btnSelect" class="btn btn-primary inputw100"
                                    Text="统计" OnClientClick="return setCheckText()" />
                            </td>
                            <td style="width: 220px;">
                                <div id="resultDiv" runat="server">
                                    共筛选出&nbsp;<span style="color: red"><asp:Literal runat="server" ID="litMembersNum"></asp:Literal></span>
                                    位会员符合条件  
                                </div>
                            </td>
                            <td>
                                <a href="#" onclick="AddMembers()">试试手动添加会员?>></a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-xs-offset-2 marginl">
                    <table>
                        <tr>
                            <td style="width: 120px;">
                                <asp:Button runat="server" ID="btnJoin" class="btn btn-success inputw100"
                                    Text="确定加入" OnClientClick="setCheckText()" />
                            </td>
                            <td>
                                <a href="<%=localUrl %>">清空条件</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdGrades" runat="server" />
        <asp:HiddenField ID="hdDefualtGroup" runat="server" />
        <asp:HiddenField ID="hdCustomGroup" runat="server" />
        <asp:HiddenField ID="hdRegisterDate" runat="server" />
        <asp:HiddenField ID="hdTradeDate" runat="server" />
    </form>
</asp:Content>
