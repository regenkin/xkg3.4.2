<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="OrderSplit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.OrderSplit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .table-bordered > tbody > tr > td.whcenter {
            text-align: center;
            vertical-align: middle;
        }

        .fblue {
            color: blue;
        }
    </style>
    <div class="page-header">
        <h2>拆分订单</h2>
        <small>商家对同一个订单中的不同商品需要选择不同的发货方式、或者发货地点，可以选择该功能。</small>
    </div>
    <div id="content"></div>
<script type="text/template" id="listTpl">
<div class='pb5 pt5' style='background: #F2F8FC'>
    <span class='ml10'><span class="cssorderid">原订单</span>：<span class="orderid"></span></span><span class='ml20'>运费：<input name="adjustedfreight" type='text' value="" splitid="" onblur="modifyFright(this)" oldvalue="" /></span>
</div>
<table class='table table-bordered table-hover'>
    <thead>
        <tr>
            <th style='width: 520px'>商品信息</th>
            <th style='width: 110px; text-align: center'>单价</th>
            <th style='width: 90px; text-align: center'>数量</th>
            <th style='width: 115px; text-align: center'>售后</th>
            <th style='text-align: center'>操作</th>
        </tr>
    </thead>
    <tbody>
        
    </tbody>
</table>
</script>
<script type="text/template" id="itemTpl">
        <tr>
            <td>
                <div class='orderInfolist clearfix'>
                    <div class='orderImg fl clearfix'>
                        <div class='img fl'>
                            <img src='' title="..." style='height: 60px; width: 60px; border-width: 0px;'>
                        </div>
                        <div class='imgInfo fl'>
                            <p class='setColor'><a href='#' target='_blank'>{商品名称}</a></p>
                            <p class="skuContent">{规格}</p>
                        </div>
                    </div>
                </div>
            </td>
            <td class='whcenter modprice'>￥{价格}</td>
            <td class='whcenter modnum'>1{数量}</td>
            <td class='whcenter fblue modstate'>退款中{状态}</td>
            <td class='whcenter'>
                <input type='button' class='btn btn-success btn-sm' value='拆分订单' orderid="0" skuid="" itemid="0" id="0" /></td>
        </tr>
        </script>

    <div style="text-align: center;display:none;" id="divoperator">
        <input id="btnCancel" type="button" class="btn btn-default" value="取消修改" />
        <input id="btnSave" type="button" class="btn btn-success inputw100" value="保存" />
        <span>
            <span style="font-size: 14px; color: #999;">注：只有保存之后订单拆分才会生效</span>
        </span>
    </div>


    <div class="modal fade" id="RemarkOrder">
        <div class="modal-dialog">
            <div class="modal-content form-horizontal">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">拆分到订单</h4><input type="hidden" id="orderid" /><input type="hidden" id="skuid" /><input type="hidden" id="itemid" />
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="col-xs-3 control-label">拆分到：</label><select class="form-control inl autow mr5 resetSize" id="ddlToOrder"><option value="0">新增一个订单</option>
                        </select>
                    </div>
                    <div class="modal-footer">
                        <input type="button" value="确定" id="btnConfirm" class="btn btn-success" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //动态加载订单信息
        var orderid = "<%=orderId%>";
        $(document).ready(function () {
            GetOrderSplitList();
            $("#btnConfirm").click(function () {
                var toOrderID = $("#ddlToOrder").val();
                var fromorderid = $("#orderid").val();
                var fromskuid = $("#skuid").val();
                var itemid = $("#itemid").val();
                var data = "posttype=savesplit&toorderid=" + toOrderID + "&fromorderid=" + fromorderid + "&fromskuid=" + fromskuid + "&itemid="+itemid;
                //alert(data)
                $.ajax({
                    url: "ordersplit.aspx",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            HiTipsShow("拆分成功", "success", function () {
                                $('#RemarkOrder').modal('hide'); GetOrderSplitList();
                            });
                        } else if (json.type == "3") {
                            HiTipsShow(json.tips, "error", function () {
                                GetOrderSplitList();
                            });
                        } else {
                            ShowMsg(json.tips, false);
                        }
                    }
                });
            })
            $("#btnCancel").click(function () {
                if (HiConform("确定要将取消订单拆分吗？", this)) {
                    var data = "posttype=cancelsplittoorder&fromorderid=" + orderid;
                    $.ajax({
                        url: "ordersplit.aspx",
                        type: "post",
                        data: data,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                HiTipsShow("取消成功", "success", function () {
                                    GetOrderSplitList();
                                });
                            } else {
                                ShowMsg(json.tips, false);
                            }
                        }
                    });
                }
            })
            $("#btnSave").click(function () {
                if (HiConform("确定要将订单拆分吗？拆分后将不可以还原！", this)) {
                    var data = "posttype=savesplittoorder&fromorderid=" + orderid;
                    $.ajax({
                        url: "ordersplit.aspx",
                        type: "post",
                        data: data,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                HiTipsShow("拆分成功", "success", function () {
                                    $('#RemarkOrder').modal('hide');
                                    location.href = "<%=reUrl%>";
                                });
                            } else {
                                ShowMsg(json.tips, false);
                            }
                        }
                    });
                }
            })
        })

        function CancelOrderSplit(obj) {
            var itemid = $(obj).attr("itemid");
            var temporderid = $(obj).attr("orderid");
            var fromsplitid = $(obj).attr("id");
            var data = "posttype=cancelordersplit&itemid=" + itemid + "&fromsplitid=" + fromsplitid + "&fromorderid=" + temporderid;// + "&fromskuid=" + skuid;
            $.ajax({
                url: "ordersplit.aspx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        HiTipsShow("取消拆分成功", "success", function () {
                            GetOrderSplitList();
                        });
                    } else {
                        ShowMsg(json.tips, false);
                    }
                }
            });
            //alert("取消拆分！");
        }
        function GetOrderSplitList() {
            $("#content").html("");
            var data = "posttype=getordersplit&orderid=" + orderid
            $.ajax({
                url: "ordersplit.aspx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.id == "splited") {
                        //如果已经拆分过
                        //去掉操作按钮
                        $("#divoperator").html("");
                        var content = '<div class="pb5 pt5" style="background: #F2F8FC"><span class="ml10"><span class="cssorderid">原订单已被拆分过</span></div>'
                        $("#content").html(content);
                    } else {

                        //清空下拉框
                        document.getElementById("ddlToOrder").innerHTML = "";
                        var optionObj = new Option("新增一个订单", "0");
                        document.getElementById("ddlToOrder").options.add(optionObj);

                        for (var i = 0; i < json.length; i++) {
                            var itemsData = json[i].data;
                            if (itemsData.length > 0) {
                                var listtpl = $($("#listTpl").html());
                                listtpl.find('.orderid').html(json[i].orderid);
                                listtpl.find('input[type="text"]').val(Number(json[i].adjustedfreight).toFixed(2));
                                listtpl.find('input[type="text"]').attr("splitid", json[i].id).attr("oldvalue", Number(json[i].adjustedfreight).toFixed(2));
                                if (i > 0) {
                                    listtpl.find('.cssorderid').html("订单" + (i + 1))
                                }
                                var ispaid = false;//是否付款
                                for (var m = 0; m < itemsData.length; m++) {
                                    var itemtpl = $($("#itemTpl").html());
                                    itemtpl.find("img").attr("src", itemsData[m].ThumbnailsUrl);
                                    itemtpl.find("a").attr("href", "/ProductDetails.aspx?productId=" + itemsData[m].ProductID);
                                    itemtpl.find(".skuContent").html(itemsData[m].SKUContent);
                                    itemtpl.find("a").html(itemsData[m].ItemDescription);
                                    itemtpl.find(".modprice").html("￥" + itemsData[m].ItemListPrice);
                                    itemtpl.find(".modnum").html(itemsData[m].Quantity);
                                    itemtpl.find(".modstate").html(itemsData[m].OrderItemsStatus);
                                    var obj = itemtpl.find("input[type='button']");
                                    var status = itemsData[m].OrderItemsStatus;
                                    //alert(ispaid)
                                    if (status == "已付款") {
                                        ispaid = true;
                                    }
                                    if (status == "" || status == "已付款") {
                                        obj.attr("itemid", itemsData[m].ID).attr("id", json[i].id).attr("orderid", orderid).attr("skuid", itemsData[m].SkuID);
                                        if (json[i].orderid == orderid) {
                                            obj.attr("value", "拆分订单").click(function () { ShowSplitLayer(this); })
                                        } else {
                                            obj.attr("value", "取消拆分").attr("class", "btn btn-default btn-sm").click(function () { CancelOrderSplit(this); })
                                        }
                                    } else {
                                        obj.attr({ "disabled": "disabled" }).attr("class", "btn btn-default btn-sm");
                                    }

                                    listtpl.find('tbody').append(itemtpl);
                                }
                                $("#content").append(listtpl);
                                if (ispaid) {
                                    listtpl.find('input[type="text"]').attr({ "disabled": "disabled" });//不修改价格
                                }
                            }

                            if (i > 0) {
                                var optionObj = new Option("订单：" + json[i].orderid, json[i].id);
                                document.getElementById("ddlToOrder").options.add(optionObj);
                            }
                        }
                        $("#divoperator").show();
                    }
                }
            })
        }
        function modifyFright(obj) {
            var id = $(obj).attr("splitid");
            var val = $(obj).val();
            var oldvalue = $(obj).attr("oldvalue");
            //alert(id + "-" + val)
            if (id > 0) {//&& val > 0
                if (val != oldvalue) {
                    var data = "posttype=editfright&id=" + id + "&val=" + val;
                    $.ajax({
                        url: "ordersplit.aspx",
                        type: "post",
                        data: data,
                        datatype: "json",
                        success: function (json) {
                            if (json.type == "1") {
                                //HiTipsShow("价格修改成功", "success", function () {
                                //    GetOrderSplitList();
                                //});
                                GetOrderSplitList();
                            }
                        }
                    })
                }
            } else {
                $(obj).val("0.00");
            }
        }
        //备注拆分订单的层
        function ShowSplitLayer(obj) {
            $("#orderid").val($(obj).attr("orderid"));
            $("#skuid").val($(obj).attr("skuid"));
            $("#itemid").val($(obj).attr("itemid"))
            $('#RemarkOrder').modal('toggle').children().css({
                width: '320px',
                height: '150px'
            })
            $("#RemarkOrder").modal({ show: true });
        }
    </script>
</asp:Content>
