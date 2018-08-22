$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //立即购买
    $("[name='iDelete']").bind("click", function () {
        var obj = this;
        myConfirm('询问', '确定要从购物车里删除该商品吗？', '确认删除', function () {
            deleteCartProduct(obj);
        });
    }); //立即购买
    $("#selectShippingType").bind("change", function () { chageShippingType() });
    $("#selectCoupon").bind("change", function () { chageCoupon() });
    $("#aSubmmitorder").bind("click", function () { submmitorder() });
});

function deleteCartProduct(obj) {
    var type = $(obj).attr("stype");
    if (type == null || type == undefined) {
        type = 0;
    }
    limitedTimeDiscountId = $(obj).attr("limitedTimeDiscountId");
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: $(obj).attr("skuId"), type: type, limitedTimeDiscountId: limitedTimeDiscountId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "/Vshop/ShoppingCart.aspx";
            }
        }
    });
}

function chageQuantity(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: $(obj).attr("skuId"), quantity: parseInt($(obj).val()) },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "/Vshop/ShoppingCart.aspx";
            }
            else {
                alert_h("最多可购买" + resultData.Status + "件", function () {
                    location.href = "/Vshop/ShoppingCart.aspx";
                });

            }
        }
    });
}






function chageShippingType() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != undefined && $("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function chageCoupon() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function submmitorder() {
    var ispass = true;
    $(".MemberPointNumber").each(function () {
        if ($(this).html() < 0) {
            ispass = false;
        }
    })
    if(!ispass){    
        alert_h("您的积分不足，请重新选择商品购买数量！", function () { location.href = "shoppingCart.aspx"; });
        return false;
    }
    var shippingType = "";
    var b = false;
    $(".selectShippingTypeValue").each(function () {
        if ($(this).val() == "") {
            b = true;
        }
        else {
            shippingType += $(this).val() + ",";
        }
    });
    if (b)
    {
        alert_h("请选择配送方式");
        return false;
    }
    if (parseFloat($("#total").html()) < 0)
    {
        alert_h("实付总额为负数，不能支付！");
        return false;
    }
    var remark = "";
    $("textarea").each(function () {
        remark += $(this).val() + ",";
    });
    remark = remark.substr(0, remark.length - 1);
    var selectCouponValue = "";
    $(".selectCouponValue").each(function () {
        selectCouponValue += $(this).val() + ",";
    });

    selectCouponValue = selectCouponValue.substr(0, selectCouponValue.length - 1);
     
    shippingType = shippingType.substr(0, shippingType.length - 1);
    var PointNumber = "";
    $(".txtPointNumber").each(function () {
        PointNumber += $(this).val() + ",";
    });
    PointNumber = PointNumber.substr(0, PointNumber.length - 1);
    
    if (!$("#selectPaymentType").val()) {
        alert_h("请选择支付方式");
        return false;
    }

    if (!$('#selectShipToDate').val()) {
        alert_h("请选择送货上门时间");
        return false;
    }

    var bargainDetialID = parseInt(getParam("bargainDetialId"));
    var limitedTimeDiscountId = parseInt(getParam("limitedTimeDiscountId"));
    maskayer(0);
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            action: "Submmitorder", PointNumber:PointNumber,selectCouponValue: selectCouponValue, Shippingcity: $("#Shippingcity").val(), shippingType: shippingType, paymentType: $("#selectPaymentType").val(), couponCode: $("#selectCoupon").val(), redpagerid: $("#selectRedPager").val(), shippingId: $('#selectShipTo').val(),
            productSku: getParam("productSku"), buyAmount: getParam("buyAmount"), from: getParam("from"), shiptoDate: $("#selectShipToDate").val(), groupbuyId: $('#groupbuyHiddenBox').val(), remark: remark, bargainDetialId: bargainDetialID, limitedTimeDiscountId: limitedTimeDiscountId
        },
        success: function (resultData) {
            maskayer(1);
            if (resultData.Status == "OK") {

                //if (resultData.OrderMarkingStatus=="0")
                location.href = "/Vshop/FinishOrder.aspx?orderId=" + resultData.OrderId;
            }
            else if (resultData.ErrorMsg) {
               
                alert_h(resultData.ErrorMsg);
            }
        }
    });
}


