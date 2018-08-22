<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorUpdateSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorUpdateSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/bootstrapSwitch.css" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/bootstrapSwitch1.js" />
    <style type="text/css">        .hiddefield {
    display:none;    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
    <div class="page-header">
        <h2>分销商升级奖励设置</h2>
        <small>分销商获得佣金自动升级，会产生一定的现金奖励，并计入分销商的佣金总额（系统调整佣金影响的等级变化，在下一次消费时生效）。<%--分销商完成购物自动升级，会额外给予一定的现金奖励，并计入分销商的佣金总额（手动调整的等级变化不参与该奖励,一次升多级只计最终等级的奖励）。--%></small>
    </div>
    <div class="table-page">
        <ul class="nav nav-tabs">
            <li class="active">
                <a href="distributorupdateset.aspx"><span>奖励设置</span></a></li>
            <li><a href="distributorupdatelist.aspx"><span>升级佣金明细</span></a></li>
        </ul>
    </div>
        <div class="tab-content"> <div class="tab-pane active">
                    <div class="set-switch">
                        <div class="form-inline mb10"></div></div></div></div>
    <div class="y3-distributionaward">
        <div class="form-group clearfix">
            <label class="col-xs-1 pad resetSize control-label" for="ddlCouponType">开启奖励：</label>
            <div class="col-xs-11">
                <div class="switch1 has-switch" id="mySwitch">
                    <asp:CheckBox ID="cbIsAddCommission" runat="server" />
                </div>
            </div>
        </div>
        <div class="form-group clearfix hiddefield">
            <label class="col-xs-1 pad resetSize control-label" for="ddlCouponType">有效时间：</label>
            <div class="col-xs-11">
               <Hi:DateTimePicker ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;至
                            <Hi:DateTimePicker ID="calendarEndDate" runat="server" CssClass="form-control resetSize inputw150" />
            </div>
        </div>
        <div class="form-group clearfix hiddefield">
            <label class="col-xs-1 pad resetSize control-label" for="ddlCouponType">奖励设置：</label>
            <div class="col-xs-11">
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>序号</th>
                            <th>奖励时间</th>
                            <th>奖励方式</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                        <tr>
                            <td><%#Container.ItemIndex+1 %></td>
                            <td>升级到<span class="colorg"><%#Eval("Name") %></span>分销商</td>
                            <td>额外奖励佣金&nbsp;<input type="text" placeholder="请填写数字金额" value="<%#Eval("AddCommission") %>" maxlength="9" gid="<%#Eval("GradeId") %>" class="inputcommission" />&nbsp;元&nbsp;<span class="colorc">（可提现，计入升级分销商等级的佣金总额）</span></td>
                        </tr></ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <input type="button" id="btnSave" value="保存" class="btn btn-success inputw100" />
            </div>
        </div>
    </div></form>
    <script type="text/javascript">
        var ischeck = $("#ctl00_ContentPlaceHolder1_cbIsAddCommission").is(":checked");
        if (ischeck) {
            $(".hiddefield").show();
        }
        $(document).ready(function () {
            $('#mySwitch').on('switch-change', function (e, data) {
                ischeck = data.value;
                if (ischeck) {
                    $(".hiddefield").show();
                } else {
                    /*执行关闭操作*/
                    var data = "posttype=save&isadd=0"
                    $.ajax({
                        url: "distributorupdateset.aspx",
                        type: "post",
                        data: data,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                HiTipsShow(json.tips, "success", function () { location.href = 'distributorupdateset.aspx' });
                            } else {
                                HiTipsShow(json.tips, "error");
                            }
                        }
                    })
                    $(".hiddefield").hide();
                }
            })
            $(".inputcommission").keyup(function () {
                var temp = $(this).val();
                if ('' != temp.replace(/\d{1,}\.{0,1}\d{0,}/, '')) {
                    temp = temp.match(/\d{1,}\.{0,1}\d{0,}/) == null ? '' : temp.match(/\d{1,}\.{0,1}\d{0,}/);
                }
                $(this).val(temp);
            })
            $("#btnSave").click(function () {
                ischeck = $("#ctl00_ContentPlaceHolder1_cbIsAddCommission").is(":checked");
                if (ischeck) {
                    var starttime = $("#ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker").val();
                    var endtime = $("#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker").val();
                    var obj = $("input[type='text'][class='inputcommission']");
                    var str = "[";
                    for (var i = 0; i < obj.length; i++) {
                        if (i == 0) {
                            str += "{\"gradeid\":\"" + $(obj[i]).attr("gid") + "\",\"addcommission\":\"" + $(obj[i]).val() + "\"}";
                        } else {
                            str += ",{\"gradeid\":\"" + $(obj[i]).attr("gid") + "\",\"addcommission\":\"" + $(obj[i]).val() + "\"}";
                        }
                    }
                    str = str + "]";
                    //alert(str)
                    if (starttime.length < 10) {
                        ShowMsg("请输入有效开始时间", false);
                        return false;
                    }
                    if (endtime.length < 10) {
                        ShowMsg("请输入有效结束时间", false);
                        return false;
                    }

                    var data = "posttype=save&isadd=1&starttime=" + starttime + "&endtime=" + endtime + "&data=" + str;
                    //alert(data);
                    //return false;
                    $.ajax({
                        url: "distributorupdateset.aspx",
                        type: "post",
                        data: data,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                HiTipsShow(json.tips, "success", function () { location.href = 'distributorupdateset.aspx' });
                            } else {
                                HiTipsShow(json.tips, "error");
                            }
                        }
                    })
                }
            })
        })
    </script>
</asp:Content>
