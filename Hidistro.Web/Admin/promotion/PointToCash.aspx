<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointToCash.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.PointToCash" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .errorInput{border:1px,solid;border-color:#a94442;}
        .normalInput{border:1px,solid;border-color:#cccccc;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type="text"]').each(function () {
                $(this).blur(function () {
                    testInput(this);
                });
            });

            //$('li').each(function () {
            //    $(this).click(function () {
            //        var id = $(this).attr('id');
            //        if (id == "Headersetting") {
            //            $('#orderList').css('display', 'none');
            //            $('#setting').css('display', '');
            //        }
            //        else
            //        {
            //            $('#orderList').css('display', '');
            //            $('#setting').css('display', 'none');
            //        }
            //    });
            //});

            $('#Headersetting').click();
        });

        function setEnable(obj) {
            var type = "0";
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
                url: "ConfigHandler.ashx",
                data: { type: type, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('积分抵现已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('积分抵现已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }



        function testRegex(rgx, str,bflag) {
            if (str == "")
            {
                if (bflag)
                { return true; }
                else { return false; }
            }
            return result = rgx.test(str);
        }

        function testInput(obj) {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;

            var flag = false;
            if (id == $('#<%=txt_Rate.ClientID%>').attr('id')) {
                regex = /^[0-9]*$/;
                parent = $(obj).parent().parent().parent();
                flag = true;
            }
            if (id == $('#<%=txt_MaxAmount.ClientID%>').attr('id')) {
                regex = /^\d+(\.\d{2})?$/;
                parent = $(obj).parent().parent().parent();
                flag = true;
            }

            if (flag) {
                if (testRegex(regex, content,false)) {
                    $(obj).removeClass();
                    $(obj).addClass("form-control");
                }
                else {
                    $(obj).removeClass();
                    $(obj).addClass("form-control errorInput");
                }
            }             
            setBtnEnable();
        }

        function setBtnEnable() {
            var error = $(".errorInput");
            if (error.length > 0) {
                $('#<%=saveBtn.ClientID%>').removeAttr('disabled');
                $('#<%=saveBtn.ClientID%>').prop('disabled', 'disabled');
            }
            else {
                $('#<%=saveBtn.ClientID%>').removeAttr('disabled');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="set-switch">
            <strong>积分抵现</strong>
            <p>
                开启以后，会员下单时可使用账户的积分来抵扣订单金额。
            </p>
            <div id="offlineEnable" class="<%=enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                <%=enable?"已开启":"已关闭"%>
                <i></i>
            </div>
        </div>
        <div class="play-tabs">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active" id="Headersetting">
                    <a href="#setting" aria-controls="setting" role="tab" data-toggle="tab">设置规则</a>
                </li>
                <li role="presentation" id="HeaderOrderList" style="display:none;">
                    <a href="#orderList" aria-controls="orderList" role="tab" data-toggle="tab">积分抵现列表</a>
                </li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="setting">
                    <div id="maindiv"  style="<%=enable?"margin-left: 15px; ": "margin-left: 15px; display:none ;" %>">
                        <div class="form-group">
                            <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>抵现比例：</label>
                            <div class="col-xs-6">
                                <div class="form-inline">
                                    每
                                    <asp:TextBox runat="server" class="form-control" Width="100px" ID="txt_Rate"></asp:TextBox>
                                    积分可抵扣1元
                                <%--<label><em>1元=<label id="lbl_Rate">10</label>积分</em></label>--%>
                                </div>
                                <small>在订单结算时，会员可直接用积分抵扣订单金额（到分）。该部分抵扣金额，由平台承担。当配置为0时，表示积分不能抵扣金额。</small>
                            </div>                            
                        </div>

                        <div class="form-group">
                            <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>单次最高抵现金额：</label>
                            <div class="col-xs-5">
                                <div class="form-inline">                                
                                    <asp:TextBox runat="server" class="form-control" Width="100px" ID="txt_MaxAmount"></asp:TextBox>
                                    元 
                                    <%--&nbsp;&nbsp;&nbsp;单次最高抵现积分数：
                                <label><em><label id="lbl_MaxPoint">2000</label>积分</em></label>--%>
                                </div>  
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-xs-offset-2 marginl" >
                                <asp:Button runat="server" ID="saveBtn" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="保存" />
                            </div>
                        </div>
                    </div>              

                </div>
                <div role="tabpanel" class="tab_pane" id="orderList" style="display:none;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div class="form-inline" style="margin-top: 5px; margin-bottom: 5px; vertical-align: central;">
                                    <label>订单号:</label>
                                    <asp:TextBox ID="txt_name" Width="100" runat="server" CssClass="form-control" />
                                    <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary" />
                                </div>
                            </td>
                            <td style="vertical-align: bottom;">
                                <div class="page-box" style="margin-right: 15px; text-align: right;">
                                    <div class="page fr">
                                        <div class="form-group">
                                            <label for="exampleInputName2">每页显示数量：</label>
                                            <UI:PageSize runat="server" ID="hrefPageSize" />
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="sell-table">
                        <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="50%">用途</th>
                                        <th width="10%">消耗积分</th>
                                        <th width="10%">抵现（元）</th>
                                        <th width="30%">抵现（时间）</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="content-table">
                            <table class="table">
                                <tbody>
                                    <asp:Repeater ID="grdProducts" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="50%">
                                                    <div class="img fl mr10">
                                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" Width="60"
                                                            Height="60" />
                                                    </div>

                                                    <div class="shop-info">
                                                        <p class="mb5"><%# Eval("ProductName") %></p>
                                                        <p>订单号：<%# Eval("OrderNo") %></p>
                                                    </div>
                                                </td>
                                                <td width="10%">
                                                    <%# Eval("PointNumber") %>
                                                </td>
                                                <td width="10%">
                                                    <%# Eval("CashAmount") %>
                                                </td>
                                                <td width="30%">
                                                    <%# Eval("Time") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</asp:Content>