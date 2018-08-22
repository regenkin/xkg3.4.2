<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="SendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.SendOrderGoods" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .listInfoLeft {
            width: 730px;
        }
        .orderManagementList {
            width: 732px;
            margin-bottom:0px;
        }
        #divEdit {
            max-height: 400px;
            overflow-y: auto;
        }
        .borderred {
            border: 1px solid red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div id="divEdit">
            <div class="orderManagementList orderManagementListOther">
                <ul>
                    <li>
                        <div class="listTitle">
                            <span style="width: 125px; display: inline-block; margin-left: 2px;">收货人姓名</span>
                            <span style="width: 160px; display: inline-block; margin-left: 2px;">订单号</span>
                            <span style="width: 130px; display: inline-block; margin-left: 2px;">物流公司</span>
                            <span style="width: 135px; display: inline-block; margin-left: 2px;">快递单号</span>
                            <span style="width: 120px; display: inline-block; margin-left: 2px;">发货状态</span>
                        </div>
                        <table id="tbShow">
                            <tbody>
                                <tr>
                                    <td width="730">
                                        <div class="listInfoLeft">
                                            <asp:Repeater ID="rptItemList" runat="server">
                                                <ItemTemplate>
                                                    <div class="orderInfolist clearfix">
                                                        <span style="width: 125px; display: inline-block; height: 16px; overflow: hidden;" title="<%#Server.HtmlEncode(Eval("ShipTo").ToString()) %>"><%#Eval("ShipTo") %></span>
                                                        <span style="width: 160px; display: inline-block;" title="orderid"><%#Eval("OrderID") %></span>
                                                        <span style="width: 132px; display: inline-block;" class="getcompanyname" title="<%#Eval("ExpressCompanyAbb") %>"></span>
                                                        <span style="width: 138px; display: inline-block;">
                                                            <input type="text" name="shipordernumber" style="width: 125px;" value="<%#Eval("ShipOrderNumber") %>" title="快递单号" maxlength="30" onkeyup="value=value.replace(/[\W]/g,'')" />
                                                        </span>
                                                        <span style="width: 120px; display: inline-block;" id="showitemprice"><Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <p class="alignc pt10">订单数：<asp:Literal ID="litOrdersCount" runat="server" Text="1"></asp:Literal></p>
                    </li>
                </ul>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-success inputw100" id="btnConfirm">确定</button>
            <button type="button" class="btn btn-default inputw100" id="btnCancel">取消</button>
        </div>
    </form>
    <script type="text/javascript">
        function CreateSelectToObj(obj, jsondata) {
            var selHtml = "";
            selHtml = "<select name='comp'><option value=''>请选择</option>";
            for (var i = 0; i < jsondata.length; i++) {
                if (jsondata[i].code == $(obj).attr("title")) {
                    selHtml += "<option value='" + jsondata[i].code + "' selected='selected'>" + jsondata[i].name + "</option>";
                } else {
                    selHtml += "<option value='" + jsondata[i].code + "'>" + jsondata[i].name + "</option>";
                }
            }
            selHtml += "</select>";
            $(obj).html(selHtml);
        }

        function isSFNo(no) {

            if (!is_num(no) || no.length != 12) {
                return false;
            } else {
                return true;
            }
        }

        function is_num(str) {
            var pattrn = /^[0-9]+$/;
            if (pattrn.test(str)) {
                return true;
            } else {
                return false;
            }
        }
        function isEMSNo(no) {
            if (no.length != 13) {
                return false;
            }

            if (getEMSLastNum(no) == no.substr(10, 1)) {
                return true;
            } else {
                return false;
            }
        }
        function getEMSLastNum(no) {
            var v = new Number(no.substr(2, 1)) * 8;
            v += new Number(no.substr(3, 1)) * 6;
            v += new Number(no.substr(4, 1)) * 4;
            v += new Number(no.substr(5, 1)) * 2;
            v += new Number(no.substr(6, 1)) * 3;
            v += new Number(no.substr(7, 1)) * 5;
            v += new Number(no.substr(8, 1)) * 9;
            v += new Number(no.substr(9, 1)) * 7;
            v = 11 - v % 11;
            if (v == 10)
                v = 0;
            else if (v == 11)
                v = 5;
            return v.toString();
        }
        $(document).ready(function () {
            iframeHeight = 200;
            var lineCount = $(".orderInfolist.clearfix").length;
            if (lineCount > 1 && lineCount < 7) {
                iframeHeight += (lineCount - 1) * 60;
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            } else if (lineCount == 1) {
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            } else {
                iframeHeight = 460;
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            }
            /*获取物流公司选择*/
            $.ajax({
                url: "sendordergoods.aspx",
                type: "post",
                data: "posttype=getcompany&t=" + (new Date()).getTime(),
                datatype: "json",
                success: function (json) {
                    if (json[0].type == "1") {
                        var datalist = json[0].data;
                        var selectobj =
                        $(".getcompanyname").each(function () {
                            CreateSelectToObj(this, datalist);
                        })
                    }
                }
            });

            $("#btnConfirm").click(function () {
                var jdata = "";
                var ispass = true;
                $(".orderInfolist").each(function () {
                    var obj = $(this).find("select[name='comp']");
                    var compname = $.trim($(obj).find("option:selected").text());
                    var shipordernumber = $.trim($(this).find("input[name='shipordernumber']").val());
                    var companycode = $.trim($(this).find("select[name='comp']").val());
                    var orderid = $.trim($(this).find("span[title='orderid']").text());                    
                    if (companycode == "") {
                        parent.ShowMsg("请选择物流公司", false);
                        ispass = false;
                        return false;
                    }
                    if (ispass) {
                        if (shipordernumber == "") {
                            parent.ShowMsg("请输入快递单号", false);
                            ispass = false;
                            return false;
                        }
                        if (ispass) {
                            //var end = shipordernumber.substr(shipordernumber.length - 1, 1);
                            //if (!is_num(end)) {
                            //    parent.ShowMsg('请输入正确的快递单号!');
                            //    ispass = false;
                            //    return false;
                            //}
                            //else if (compname == "EMS" && !isEMSNo(shipordernumber)) {
                            //    parent.ShowMsg('请输入正确的EMS单号!');
                            //    ispass = false;
                            //    return false;
                            //}
                            //else if (compname == "顺丰快递" && !isSFNo(shipordernumber)) {
                            //    parent.ShowMsg('请输入正确的顺丰单号!');
                            //    ispass = false;
                            //    return false;
                            //}
                            var reg= /^([a-zA-Z0-9]{1,30})$/;
                            if (!reg.test(shipordernumber)) {
                                parent.ShowMsg('请输入正确的快递单号!');
                                ispass = false;
                                return false;
                            }
                            if (ispass) {
                                if (jdata == "") {
                                    jdata = "{\"orderid\":\"" + orderid + "\",\"compname\":\"" + compname + "\",\"shipordernumber\":\"" + shipordernumber + "\",\"companycode\":\"" + companycode + "\"}";
                                } else {
                                    jdata += ",{\"orderid\":\"" + orderid + "\",\"compname\":\"" + compname + "\",\"shipordernumber\":\"" + shipordernumber + "\",\"companycode\":\"" + companycode + "\"}";
                                }
                            }
                        }
                    }
                });
                
                if (ispass) {
                    var jsonTips = "[" + jdata + "]";
                    $.ajax({
                        url: "sendordergoods.aspx",
                        type: "post",
                        data: "posttype=<%=type%>&data=" + jsonTips,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                parent.HiTipsShow(json.tips, "success", function () {
                                    //parent.$('#divmyIframeModal').modal('hide');
                                    parent.location.href = "<%=ReUrl%>";
                                })
                            } else {
                                parent.ShowMsg(json.tips, false);
                            }
                        }
                    });
                }
            })
            $("#btnCancel").click(function () {
                parent.$('#divmyIframeModal').modal('hide')
            })
            $("[data-toggle='tooltip']").tooltip({ html: false });
        });
    </script>
</asp:Content>
