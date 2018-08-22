var priceContent;
var popTitle;
var priceBox;
var priceValueHolder;

$(document).ready(function() {
    priceContent = $("#priceContent");
    popTitle = $("#popTitle");
    priceBox = $("#priceBox");
});

function editMemberPrice(holderId, basePrice) {
    priceValueHolder = $("#" + holderId);
    var postUrl = "productedit.aspx?isCallback=true&action=getMemberGradeList&timestamp=";
    postUrl += new Date().getTime() + "&typeId=" + currentTypeId;

    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function(resultData) {
            if (resultData.Status == "OK") {
                initPriceBox("编辑会员价", "会员等级", resultData.MemberGrades, basePrice);
            }
        }
    });
}

function editProductMemberPrice() {
    editMemberPrice("ctl00_ContentPlaceHolder1_txtMemberPrices", getSalePrice());
}

function editSkuMemberPrice(rowIndex) {
    editMemberPrice("gradeSalePrice_" + rowIndex, getPrice($("#salePrice_" + rowIndex).val()));
}

function initPriceBox(title, gradeTitle, gradeList, basePrice) {
    $(priceContent).empty();
    $.each($(".gradeRow"), function() { $(this).remove(); });
    $("#popTitle").text(title);
    
    var priceTable = $("<table id=\"priceTable\" cellPadding=\"2\" class=\"table table-bordered table-hover\"><\/table>");
    var tbHeader = $($(String.format("<thead><tr><th style=\"text-align:center\"><strong>{0}<\/strong><\/th><th style=\"width:33%;text-align:center\"><strong>价格<\/strong><\/th><th style=\"width:33%;text-align:center\"><strong>留空默认<\/strong><\/th><\/tr><\/thead>", gradeTitle)));
    $(priceTable).append(tbHeader);

    $.each(gradeList, function(i, grade) {
        var gradeRow = $(String.format("<tr class=\"gradePriceTr gradeRow\" gradeId=\"{0}\"><\/tr>", grade.GradeId));
        var priceCell = $("<td align=\"center\"><\/td>");
        var defaultPrice = (basePrice == 0.01) ? "0.01" : "0.00";

        if (basePrice > 0.01) {
            defaultPrice = ((basePrice * parseInt(grade.Discount)) / 100).toFixed(2);
        }

        priceCell.append($(String.format("<input type=\"text\" class=\"inputGradePrice\" gradeId=\"{0}\" id=\"gradePrice_{0}\" \/>", grade.GradeId)));
        gradeRow.append($(String.format("<td align=\"center\">{0}<\/td>", grade.Name)));
        gradeRow.append(priceCell);
        gradeRow.append($(String.format("<td align=\"center\">￥{0}<\/td>", defaultPrice)));
        $(priceTable).append(gradeRow);
    });

    $(priceContent).append(priceTable);

    if ($(priceValueHolder).val().length > 0) {
        var xml;
        if (/msie/.test(navigator.userAgent.toLowerCase())) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML($(priceValueHolder).val());
        }
        else {
            xml = new DOMParser().parseFromString($(priceValueHolder).val(), "text/xml");
        }

        $.each($(xml).find("grande"), function () {
            var ctrl = $("input[gradeId='" + $(this).attr("id") + "']");
            if (ctrl.length > 0) $(ctrl).val($(this).attr("price"));
        });
    }
    
    var objDiv = "priceBox";
    $("#" + objDiv).modal({ show: true });
}

function getSalePrice() {
    return getPrice($("#ctl00_ContentPlaceHolder1_txtSalePrice").val());
}

function getPrice(strPrice) {
    if (strPrice.length == 0) return 0;

    var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
    if (!exp.test(strPrice)) return 0;

    var price = parseFloat(strPrice).toFixed(2);
    if (!((price >= 0.01) && (price <= 10000000))) return 0;

    return price;
}

function doneEditPrice(objId) {
    var inputItems = $(".inputGradePrice");
    if (!checkGradePrice(inputItems)) return;
    
    var priceXml = "<xml><gradePrices>";
    $.each(inputItems, function() {
        priceXml += String.format("<grande id=\"{0}\" price=\"{1}\" \/>", $(this).attr("gradeId"), $(this).val());
    });
    priceXml += "<\/gradePrices><\/xml>";
    
    $(priceValueHolder).val(priceXml);
    //CloseDiv('priceBox');

    $('#' + objId).modal('hide');
}

function checkGradePrice(inputItems) {
    var validated = true;
    var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

    $.each(inputItems, function() {
        var val = $(this).val();

        if (val.length > 0) {
            // 检查输入的是否是有效的金额
            if (!exp.test(val)) {
                alert("价格输入有误，请输入正确的价格");
                $(this).focus();
                validated = false;
                return false;
            }

            // 检查金额是否超过了系统范围
            var num = parseFloat(val);
            if (!((num >= 0.01) && (num <= 10000000))) {
                alert("输入的价格超出了系统表示范围！");
                $(this).focus();
                validated = false;
                return false;
            }
        }
    });

    return validated;
}