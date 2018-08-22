<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="BatchPrintData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.BatchPrintData" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .InputShow {
            margin: 0px auto;
        }

            .InputShow p {
                margin: 15px 0;
            }

            .InputShow input, .InputShow select {
                width: 160px;
            }

        .pr0 {
            padding-right: 0;
        }

        .listInfoLeft {
            width: 520px;
        }

        .orderManagementList {
            width: 522px;
            margin-bottom:0px;
        }

        #divEdit {
            max-height: 188px;
            overflow-y: auto;
        }

        .borderred {
            border: 1px solid red;
        }

        /**
 *    打印相关
*/
        @media print {
            .notprint {
                display: none;
            }

            .PageNext {
                page-break-after: always;
            }
        }

        @media screen {
            .notprint {
                display: inline;
                cursor: pointer;
            }
        }

        #bg {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.7;
            opacity: .70;
            filter: alpha(opacity=70);
        }

        #show {
            display: none;
            position: absolute;
            top: 35%;
            left: 29%;
            width: 43%;
            height: 15%;
            padding: 8px;
            border: 8px solid #E8E9F7;
            background-color: #FFF;
            z-index: 1002;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/LodopFuncs.js"></script>
    <script src="/admin/js/bootstrapSwitch.js" type="text/javascript"></script>
    <link href="/admin/css/bootstrapSwitch.css" rel="stylesheet" type="text/css" />
    <form runat="server">
        <div id="divEdit">
            <div class="orderManagementList">
                <table class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th style="width: 30%;">收货人姓名</th>
                            <th style="width: 30%;">订单号</th>
                            <th style="width: 40%;">快递单号</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptItemList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td title="<%#Eval("ShippingRegion").ToString()+", "+Eval("Address").ToString().Trim() %>"><%#Eval("ShipTo") %></td>
                                    <td title='orderid'><%#Eval("OrderID") %></td>
                                    <td>
                                        <input type="text" name="shipordernumber" style="width: 160px;" value="<%#Eval("ShipOrderNumber") %>" tid="<%#Eval("rownumber") %>" title="快递单号" maxlength="20" onkeyup="value=value.replace(/[\W]/g,'')" /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>

        <div style="width: 100%">
            <div class="InputShow">
                <asp:Panel runat="server" ID="pnlShipper" Style="width: 100%;">

                    <p class="row">
                        <span class="col-xs-4 alignR pr0">发货地址:</span><span class="col-xs-8">
                            <Hi:ShippersDropDownList runat="server" ID="ddlShoperTag" /> <span style="color:blue; cursor:pointer;" id="spanModifyAddr"><a href="../Settings/shippers.aspx" target="_blank">添加发货地址</a></span>
                        </span>
                    </p>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEmptySender">
                    <span><a href="../Settings/shippers.aspx" target="_parent" class="red">您还没有添加发货人信息，请先点击这里添加发货人信息!</a></span>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTemplates">
                    <p class="row">
                        <span class="col-xs-4 alignR pr0">物流公司:</span><span class="col-xs-8">
                            <asp:DropDownList runat="server" ID="ddlTemplates">
                            </asp:DropDownList></span>
                    </p>
                    <p class="row"><span class="col-xs-4 alignR pr0">起始快递单号:</span><span class="col-xs-8"><asp:TextBox runat="server" ID="txtStartCode" /></span></p>

                    <div class="row">
                        <div class="col-xs-4 alignR pr0">自动合并:</div>
                        <div class="col-xs-8">
                            &nbsp;<div id="mySwitch" class="switch">
                                <input id="cbIsCombine" type="checkbox" name="cbIsCombine" checked="checked" value="1" />
                            </div>
                            <span>快递单数:<span id="spanNum"><asp:Literal ID="litNumber" runat="server" /></span></span> 
                            <small>将具有相同收货信息和配送方式的订单合并成一个包裹发货</small>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEmptyTemplates">
                    <span><a href="AddExpressTemplate.aspx" target="_parent">您还没有添加快递单模板，请先点击这里添加快递单模板!</a></span>
                </asp:Panel>
            </div>

            <%--            <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
                <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
            </object>--%>
            <div id='bg'></div>
            <div id='show'>正在打印，请勿关闭窗口。。。</div>
        </div>

        <div class="modal-footer">
            <asp:Button runat="server" ID="btnPrint" CssClass="btn btn-success" Text="打印" OnClientClick="return DoPrint();" Visible="false" />
            <input type="button" id="btnPrintAll" class="btn btn-success" value="打印" style="display:<%=btnShowStyle%>"/>
            <button type="button" id="btnCancel" class="btn btn-default" data-dismiss="modal">关闭</button>

        </div>

    </form>
    <div>
        <iframe id="ifPrint" name="ifPrint"></iframe>
        <form id="formPrint" method="post" action="batchprintdata.aspx" target="ifPrint">
            <input type="hidden" name="posttype" value="printall" /><input type="hidden" name="data" id="data" /></form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            iframeHeight = 290;
            var lineCount = $(".table-bordered tbody tr").length;
            if (lineCount < 3) {
                iframeHeight += lineCount * 60;
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            } else if (lineCount == 1) {
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            } else {
                iframeHeight = 430;
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            }

            $("#spanModifyAddr").click(function () {
                if ($(this).html() != "刷新") {
                    $(this).html("刷新").click(function () {
                        
                        $.ajax({
                            url: "BatchPrintData.aspx",
                            type: "post",
                            data: "posttype=getmyaddr",
                            datatype: "json",
                            success: function (json) {
                                if (json.type == '1') {
                                    //$("#ctl00_ContentPlaceHolder1_ddlShoperTag").empty();
                                    //alert(json.data)
                                    $("#ctl00_ContentPlaceHolder1_ddlShoperTag").html(json.data);
                                }
                            }
                        });
                    })
                }
            })

            $("#btnCancel").click(function () {
                parent.$('#divmyIframeModal').modal('hide')
            })
            $('#mySwitch').on('switch-change', function (e, data) {
                //var $el = $(data.el)
                //  , value = data.value;
                ///onsole.log(e, $el, value);
                $("#ctl00_ContentPlaceHolder1_txtStartCode").blur();
            });
            $("#ctl00_ContentPlaceHolder1_txtStartCode").blur(function () {
                var bl = $("#cbIsCombine").is(":checked");                
                var baseval = $.trim($(this).val());
                if (baseval > 0) {
                    var ordercount = 0;
                    $(".table-bordered tbody tr").each(function () {
                        var tempobj = $(this).find("input[name='shipordernumber']");
                        if ($(tempobj).attr("tid") == "1") {
                            tempobj.val(baseval).css("font-weight", "normal");
                        } else {
                            if (bl) {
                                baseval--;
                                ordercount--;
                                $(tempobj).focus(function () {
                                    $(this).css("font-weight", "normal");
                                })
                                tempobj.val(baseval).css("font-weight", "bold");
                            } else {
                                tempobj.val(baseval).css("font-weight", "normal");
                            }
                        }
                        ordercount++
                        baseval++;
                    })                    
                    $("#spanNum").html(ordercount);
                }
            })
            $("#btnPrintAll").click(function () {
                var jdata = "";
                var ispass = true;

                if ($("#ctl00_ContentPlaceHolder1_ddlShoperTag").val().length == 0) {
                    alert("请选择一个发货地址!");
                    return false;
                }
                var compname = $.trim($("#ctl00_ContentPlaceHolder1_ddlTemplates").find("option:selected").text());
                if (compname == "-请选择-") {
                    parent.ShowMsg('请先选择物流公司!', false);
                    ispass = false;
                    return false;
                }
                var iscombine = $("#cbIsCombine").is(":checked") ? 1 : 0;
                if (ispass) {
                    $(".table-bordered tbody tr").each(function () {
                        var shipordernumber = $.trim($(this).find("input[name='shipordernumber']").val());
                        var orderid = $.trim($(this).find("td[title='orderid']").text());

                        var end = shipordernumber.substr(shipordernumber.length - 1, 1);
                        if (!is_num(end)) {
                            parent.ShowMsg('请输入正确的快递单号!', false);
                            ispass = false;
                            return false;
                        }
                        else if (compname == "EMS" && !isEMSNo(shipordernumber)) {
                            parent.ShowMsg('请输入正确的EMS单号!', false);
                            ispass = false;
                            return false;
                        }
                        else if (compname == "顺丰快递" && !isSFNo(shipordernumber)) {
                            parent.ShowMsg('请输入正确的顺丰单号!', false);
                            ispass = false;
                            return false;
                        }
                        if (ispass) {
                            if (jdata == "") {
                                jdata = "{\"orderid\":\"" + String2Json(orderid) + "\",\"shipordernumber\":\"" + String2Json(shipordernumber) + "\"}";
                            } else {
                                jdata += ",{\"orderid\":\"" + String2Json(orderid) + "\",\"shipordernumber\":\"" + String2Json(shipordernumber) + "\"}";
                            }
                        }
                    })
                    var jsonTips = "[{\"compname\":\"" + String2Json(compname) + "\",\"iscombine\":\"" + String2Json(iscombine) + "\",\"shoper\":\"" + String2Json($("#ctl00_ContentPlaceHolder1_ddlShoperTag").val()) + "\",\"l\":\"" + String2Json($("#ctl00_ContentPlaceHolder1_ddlTemplates").val()) + "\",\"data\":[" + jdata + "]}]";

                    $("#data").val(jsonTips);
                    $("#formPrint").submit();
                }
            })
        })
        function String2Json(str) {
            return str;
        }
        function showdiv() {
            document.getElementById('bg').style.display = 'block';
            document.getElementById('show').style.display = 'block';
        }
        function hidediv() {
            document.getElementById('bg').style.display = 'none';
            document.getElementById('show').style.display = 'none';
        }

        function DoPrint() {
            if (true) {
                if ($("#ctl00_ContentPlaceHolder1_ddlTemplates").val().length == 0) {
                    alert("请选择一个快递单模板");
                    return false;
                }
                if ($("#ctl00_ContentPlaceHolder1_txtStartCode").val().length == 0) {
                    alert("请录入快递单起始编号");
                    return false;
                }
                var mailNo = parseInt($("#ctl00_ContentPlaceHolder1_txtStartCode").val(), 10);
                if (isNaN(mailNo)) {
                    alert("快递单起始编号须为自然数");
                    return false;
                }
                if (mailNo <= 0) {
                    alert("快递单起始编号须为数字格式！");
                    return false;
                }
                var com = $("#ctl00_ContentPlaceHolder1_ddlTemplates option:selected").text();
                if (com == "EMS" && !isEMSNo(mailNo)) {
                    alert('请输入正确的EMS单号!');
                    return false;
                }
                if (com == "顺丰快递" && !isSFNo(mailNo)) {
                    alert('请输入正确的顺丰单号!');
                    return false;
                }
                return true;
            }
            return false;
        }

        function isSFNo(no) {
            no = String(no);
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
            var nostr = String(no);
            if (nostr.length != 13) {
                return false;
            }

            if (getEMSLastNum(nostr) == nostr.substr(10, 1)) {
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
    </script>
</asp:Content>
