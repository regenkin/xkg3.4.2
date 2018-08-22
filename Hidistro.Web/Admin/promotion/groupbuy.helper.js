//var item = 1;
//function AddConditionInput() {
//    item++;
//    var objTable = $("#ulCondition");
//    objTable.append("<dd>数量达到:<input type=\"text\" name=\"number\" id=\"number" + item + "\" onchange=\"CheckValue(this)\" pricenumber=\"number\"/> 享受价格:<input type='text' name='price' id=\"price" + item + "\" onchange=\"CheckPrice(this)\" pricenumber=\"price\"/><a href='javascript:void(0);' onclick='Disposed(this)' class='del'>[-]</a></dd>");
//}

//function Disposed(arg_obj_item) {
//    var objdd = $(arg_obj_item).parent();
//    objdd.remove();
//}


//function CollectTotalValue() {
//    $("#ctl00_contentHolder_txtConditionValue").val("");
//    var arraynumber = new Array();
//    var arrayprice = new Array();

//    $("input[name='number']").each(function(index, obj) {
//        var pricenumber = $(obj).attr("pricenumber");
//        if (pricenumber == "number")
//            arraynumber[index] = $(obj).val();
//    });


//    $("input[name='price']").each(function(index, obj) {
//        var pricenumber = $(obj).attr("pricenumber");
//        if (pricenumber == "price")
//            arrayprice[index] = $(obj).val();
//    });

//    var valStr = "";

//    for (var i = 0; i < arraynumber.length; i++) {
//        if (arraynumber[i] != "" && arrayprice[i] != "")
//            valStr = valStr + arraynumber[i] + "_" + arrayprice[i] + "|";
//    }

//    valStr = valStr.substring(0, valStr.lastIndexOf("|"));
//    $("#ctl00_contentHolder_txtConditionValue").val(valStr);

//}


function ResetGroupBuyProducts() {
    var categoryId = $("#ctl00_contentHolder_dropCategories").val();
    var sku = $("#ctl00_contentHolder_txtSKU").val();
    var productName = $("#ctl00_contentHolder_txtSearchText").val();
    var postUrl = "addgroupbuy.aspx?isCallback=true&action=getGroupBuyProducts&timestamp=";
    postUrl += new Date().getTime() + "&categoryId=" + categoryId + "&sku=" + encodeURI(sku) + "&productName=" +encodeURI(productName);

    document.getElementById("ctl00_contentHolder_dropGroupBuyProduct").options.length = 0;
    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function(resultData) {
            if (resultData.Status == "0") {
            }
            else if (resultData.Status == "OK") {
                FillProducts(resultData.Product);
            }
        }
    });
}

function FillProducts(product) {
    var productSelector = $("#ctl00_contentHolder_dropGroupBuyProduct");
    productSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");

    $.each(product, function(i, product) {
        productSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", product.ProductId, product.ProductName));
    });
}

//$(document).ready(function() {
//    var conditionValue = $("#ctl00_contentHolder_txtConditionValue").val().split("|");
//    item = 1;
//    for (i = 0; i < conditionValue.length; i++) {
//        var condition = conditionValue[i].split("_");
//        if (i == 0) {

//            $("#number1").val(condition[0]);
//            $("#price1").val(condition[1]);
//        }
//        else {
//            item++;
//            var objTable = $("#ulCondition");
//            objTable.append("<dd>数量达到:<input type=\"text\" value=\"" + condition[0] + "\" name=\"number\" id=\"number" + item + "\" onchange=\"CheckValue(this)\" pricenumber=\"number\"/> 享受价格:<input type='text' value=\"" + condition[1] + "\" name='price' id=\"price" + item + "\" onchange=\"CheckPrice(this)\" pricenumber=\"price\"/><a href='javascript:void(0);' onclick='Disposed(this)' class='del'>[-]</a></dd>");
//        }
//    }
//    item = conditionValue.length;
//});



//function CheckValue(id) {
//    var value = $(id).val();
//    var exp = new RegExp("-?[0-9]\\d*");

//    if (!exp.test(value)) {
//        $(id).val("");
//        alert("输入的数量无效");
//        return false;
//    }
//    $("input[name='number']").each(function(index, obj) {
//        var objvalue = $(obj).val();
//        if ($(id).attr("id") != $(obj).attr("id") && value == objvalue) {
//            $(id).val("");
//            alert("存在相同的数量");
//            return false;
//        }

//    });

//}

//function CheckPrice(obj) {
//    var value = $(obj).val();
//    var exp = new RegExp("(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)");

//    if (!exp.test(value)) {
//        alert("输入的价格无效");
//        return false;
//    }
//}