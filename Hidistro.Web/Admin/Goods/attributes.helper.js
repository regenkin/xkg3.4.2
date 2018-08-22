var productTypeSelector;
var brandCategorysSelector;
var skuFields, cellFields;
var currentTypeId;

var skuTableHolder, skuFieldHolder;
var skuTable, tableBody, tableHeader;
var htSkuFields = new jQuery.Hashtable();
var htSkuItems = new jQuery.Hashtable();

// 以下为需要根据当前商品类型是否具有扩展属性控制显示隐藏的项目
var attributeRow; // 扩展属性操作区域（默认不显示）
var attributeContentHolder; //扩展属性内容显示和操作容器

// 以下为需要根据当前商品类型是否有规格控制显示隐藏的项目
var skuTitle; // “商品规格”标题显示（默认不显示）
var enableSkuRow; // 开启规格提示及操作按钮的显示（默认不显示）

// 以下为点击开起规格以后需要显示的项目
var skuRow; // 规格操作区域（默认不显示）

// 以下为开启规格需要隐藏，关闭规格后显示的项目
var skuCodeRow;// 货号
var salePriceRow; // 现价
//var costPriceRow; // 成本价
var qtyRow; // 库存
//var weightRow; // 重量

var skuEnabled = false;

$(document).ready(function () {
    skuTitle = document.getElementById("skuTitle");
    attributeRow = document.getElementById("attributeRow");
    enableSkuRow = document.getElementById("enableSkuRow");
    skuRow = document.getElementById("skuRow");
    skuCodeRow = document.getElementById("skuCodeRow");
    salePriceRow = document.getElementById("salePriceRow");
    //costPriceRow = document.getElementById("costPriceRow");
    qtyRow = document.getElementById("qtyRow");
    //weightRow = document.getElementById("weightRow");

    productTypeSelector = $(".productType"); //商品类型
    brandCategorysSelector = $("#ctl00_ContentPlaceHolder1_dropBrandCategories"); //商品品牌
    attributeContentHolder = $("#attributeContent"); //扩展属性区域容器
    skuTableHolder = $("#skuTableHolder");
    skuFieldHolder = $("#skuFieldHolder");

    productTypeSelector.bind("change", function () { reset(); });
    $("#btnEnableSku").bind("click", function () { enableSku(); $("html,body").animate({ scrollTop: $(skuTitle).offset().top }, 500); });
    $("#btnAddItem").bind("click", function () { addItem(); });
    $("#btnCloseSku").bind("click", function () { closeSku(); });
    $("#btnGenerateAll").bind("click", function () { generateAll(); });
    $("#btnshowSkuValue").bind("click", function () { showSkuValue(); });
    $("#btnGenerate").bind("click", function () { generateSku(); });

    init();
});

function init() {
    currentTypeId = productTypeSelector.val();
    if (currentTypeId.length == 0 || currentTypeId == "0")
        return;
    prepareControls(false);
    restoreState();
}

// 重新选择商品类型以后重置页面所有相关内容
function reset() {
    if(currentTypeId != ""){
        if (!confirm('切换商品类型将会导致已经编辑的品牌，属性和规格数据丢失，确定要切换吗？')) {
            productTypeSelector.val(currentTypeId);
            return false;
        }
    }

    skuEnabled = false;
    currentTypeId = productTypeSelector.val();
    attributeContentHolder.empty();
    skuTableHolder.empty();
    setCtrlDisplay("");
    skuRow.style.display = "none";
    htSkuFields.clear();
    htSkuItems.clear();
    reBindValid();
    $("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked", false);

    prepareControls(true);
}

function prepareControls(isReset) {
    if (currentTypeId.length == 0) currentTypeId = "0";
    var postUrl = "productedit.aspx?isCallback=true&action=getPrepareData&timestamp=";
    postUrl += new Date().getTime() + "&typeId=" + currentTypeId;
    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function (resultData) {
            var hasAttribute = (resultData.HasAttribute == "True");
            var hasSku = (resultData.HasSKU == "True")
            var hasBrandCategory = (resultData.HasBrandCategory == "True")

            setCtrlDisplay("");
            updateDisplayStatus(hasAttribute, hasSku);
            if (hasAttribute) {
                prepareAttributes(resultData.Attributes);
            }
            if (hasBrandCategory && isReset) {
                prepareBrandCategories(resultData.BrandCategories);
            }

            if (hasSku && resultData.SKUs.length > 0) {
                cellFields = new Array(resultData.SKUs.length);

                $.each(resultData.SKUs, function (i, skuItem) {
                    cellFields[i] = skuItem.AttributeId;
                    htSkuFields.add(skuItem.AttributeId, skuItem);
                });
            }
        }
    });
}

function updateDisplayStatus(hasAttribute, hasSku) {
    attributeRow.style.display = hasAttribute ? "" : "none";
    skuTitle.style.display = hasSku ? "" : "none";
    enableSkuRow.style.display = hasSku ? "" : "none";
    skuRow.style.display = "none";
}

function prepareAttributes(attributes) {
    if (attributes == null || attributes == undefined || attributes.length == 0)
        return;

    var ulTag = $("<ul><\/ul>");
    attributeContentHolder.append(ulTag);

    $.each(attributes, function(i, attribute) {
        var liTag = $(String.format("<li class=\"attributeItem\" attributeId=\"{0}\" usageMode=\"{1}\"><\/li>", attribute.AttributeId, attribute.UsageMode));
        var liLDiv=$("<div style=\"float:left\"></div>");
        var titleSpan = $(String.format("<span class=\"formitemtitle5\" style=\"float:left\">{0}：<\/span>", attribute.Name));
        liLDiv.append(titleSpan);

        if (attribute.UsageMode == "1") {
            var selectTag = $(String.format("<select id=\"attribute{0}\" class=\"formselect input162\" style=\"float:left\"><\/select>", attribute.AttributeId));
            selectTag.append($("<option value=\"\">--\u8BF7\u9009\u62E9--<\/option>"));

            if (attribute.AttributeValues.length > 0) {
                $.each(attribute.AttributeValues, function(vIndex, attributeValue) {
                    selectTag.append($(String.format("<option value=\"{0}\">{1}<\/option>", attributeValue.ValueId, attributeValue.ValueStr)));
                });
            }
            liLDiv.append(selectTag);
        }
        else if (attribute.UsageMode == "0") {
            if (attribute.AttributeValues.length > 0) {
                var checkGroupName = "vallist" + attribute.AttributeId;

                $.each(attribute.AttributeValues, function(vIndex, attributeValue) {
                    var cid = String.format("att_{0}_{1}", attribute.AttributeId, attributeValue.ValueId);
                    var valItem = $("<span id=\"span_"+attribute.AttributeId+"\" class=\"valspan\"><\/span>")
                    valItem.append($(String.format("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" valueId=\"{2}\" \/>", checkGroupName, cid, attributeValue.ValueId)));
                    valItem.append($(String.format("<label for=\"{0}\">{1}<\/label>", cid, attributeValue.ValueStr)));
                    liLDiv.append(valItem);
                });
            }
        }
        liLDiv.append($("<a href=\"javascript:void(0)\" onclick=\"ShowAttributeValue(" + attribute.AttributeId + ",this)\" class=\"add btn btn-primary btn-xs\">添加</a>"));
        liTag.append(liLDiv);
        liTag.append($("<div id=\"div_" + attribute.AttributeId + "\" style=\"display:none;float:left;\"><input type=\"text\" id=\"txtvalue" + attribute.AttributeId + "\" class=\"mr5 inputw150\"/><input type=\"button\" class=\"btn btn-success btn-xs\" value=\"保存\" onclick=\"javascript:AddValue('" + attribute.AttributeId + "','" + attribute.UsageMode + "',this)\"/></div>"));
        ulTag.append(liTag);
    });

    attributeRow.style.display = "";
}


//显示添加属性值的文本框
function ShowAttributeValue(attributeId,obja){
    if($(obja).text()=="添加"){
         $("#div_"+attributeId).show('slow');
         $(obja).text('取消');
         $(obja).attr('class', 'del btn btn-danger btn-xs');
    }else{
         $("#div_"+attributeId).hide('hide');
         $(obja).text('添加');
         $(obja).attr('class', 'add btn btn-primary btn-xs');
    }
}

//保存属性
function AddValue(attributeId,userMode,btnobj){
    var valuename=$("#txtvalue"+attributeId).val().replace(/\s/g,"");
    if(valuename==""){
        ShowMsg("请输入要添加的属性值",false);
        return false;
    }
    $("#div_"+attributeId).hide('hide');
    $("#txtvalue"+attributeId).val('');
    $(btnobj).parent().parent().find("a").text('添加');
    $(btnobj).parent().parent().find("a").attr("class", "add btn btn-primary btn-xs");
    AddAttributeValue(attributeId,valuename,userMode);
   
}


function AddAttributeValue(attributeId,valuename,userMode) {
    $.ajax({
        url: "AddSpecification.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { AttributeId: attributeId, ValueName: valuename, Mode: "Add", isAjax: "true" },
        async: false,
        success: function (data) {
          
            if (data.Status == "true") {
                ShowMsg("添加属性值成功", true);
                if (userMode == "1") {
                    $("#attribute" + attributeId).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}<\/option>", data.msg, valuename)));
                    $("#attribute" + attributeId).val(data.msg);
                } else {

                    var spanObj = document.createElement('span');
                    spanObj.className = "valspan";
                    spanObj.id = "span_" + attributeId;

                    var checkGroupName = "vallist" + attributeId;
                    var cid = String.format("att_{0}_{1}", attributeId, data.msg);
                    ///onsole.log();
                    $(spanObj).append($(String.format("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" valueId=\"{2}\" checked=\"checked\" \/>", checkGroupName, cid, data.msg)));
                    $(spanObj).append($(String.format("<label for=\"{0}\">{1}<\/label>", cid, valuename)));
                    //  $(spanObj).insertAfter($(".formitemtitle5"));
                    $(spanObj).insertAfter($(".attributeItem[attributeid='" + attributeId + "']").find('.formitemtitle5'));
                }
                restoreState();
            }
            else {
                ShowMsg(data.msg, false);
            }
        }
    });
}


function prepareBrandCategories(brandCategories) {
    document.getElementById("ctl00_ContentPlaceHolder1_dropBrandCategories").options.length = 0;
    brandCategorysSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");

    $.each(brandCategories, function(i, brand) {
        brandCategorysSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", brand.BrandId, brand.BrandName));
    });
}

function restoreState() {
    if ($("#ctl00_ContentPlaceHolder1_txtAttributes").val().length > 0) {
        var xml;
        if (/msie/.test(navigator.userAgent.toLowerCase())) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML($("#ctl00_ContentPlaceHolder1_txtAttributes").val());
        }
        else {
            xml = new DOMParser().parseFromString($("#ctl00_ContentPlaceHolder1_txtAttributes").val(), "text/xml");
        }

        // 属性值回发状态维护
        var selectedAttributes = $(xml).find("item");
        $.each(selectedAttributes, function(itemIndex, itemNode) {
            var attributeId = $(itemNode).attr("attributeId");
            var valueList = $(itemNode).children();

            if ($(itemNode).attr("usageMode") == "0") {
                $.each(valueList, function() {
                    var ctl = $("input[name='vallist" + attributeId + "'][valueId='" + $(this).attr("valueId") + "']");
                    if ($(ctl).length > 0) $(ctl).attr("checked", "checked");
                });
            }
            else {
                var attributeControl = $("#attribute" + attributeId);
                if ($(attributeControl).length > 0) $(attributeControl).val($(itemNode).children().eq(0).attr("valueId"));
            }
        });
    }

    if ($("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked") == "checked") {
        // 规格值回发状态维护
        enableSku();

        var xml;
        if (/msie/.test(navigator.userAgent.toLowerCase())) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML($("#ctl00_ContentPlaceHolder1_txtSkus").val());
        }
        else {
            xml = new DOMParser().parseFromString($("#ctl00_ContentPlaceHolder1_txtSkus").val(), "text/xml");
        }

        var selectedSkus = $(xml).find("item");
        $.each($(".fieldCell"), function() {
            var skuId = $(this).attr("skuId");
            if ($(selectedSkus[0]).find("sku[attributeId='" + skuId + "']").length == 0) {
                removeSkuField(skuId, $(this).children().eq(0).text(), false);
            }
        });

        $.each(selectedSkus, function() {
            var rowIndex = addItem();
            $("#skuCode_" + rowIndex).val($(this).attr("skuCode"));
            $("#salePrice_" + rowIndex).val($(this).attr("salePrice"));
            $("#costPrice_" + rowIndex).val($(this).attr("costPrice"));
            $("#qty_" + rowIndex).val($(this).attr("qty"));
            $("#weight_" + rowIndex).val($(this).attr("weight"));

            $.each($(this).find("sku"), function() {
                var attributeId = $(this).attr("attributeId");
                var valueId = $(this).attr("valueId");
                selectSkuValue(rowIndex, attributeId, valueId, $("span[class='sku" + attributeId + "values'][valueId='" + valueId + "']").text());
            });
            
            var s="";
            if ($(this).find("memberGrande").length > 0) {
                s = "<xml><gradePrices>";
                $.each($(this).find("memberGrande"), function() {
                    s += String.format("<grande id=\"{0}\" price=\"{1}\" \/>", $(this).attr("id"), $(this).attr("price"));
                });
                s += "<\/gradePrices><\/xml>";
            }
            $("#gradeSalePrice_" + rowIndex).val(s);
        });
    }
}

function setCtrlDisplay(displayCssStatus) {
    skuCodeRow.style.display = displayCssStatus;
    salePriceRow.style.display = displayCssStatus;
    //costPriceRow.style.display = displayCssStatus;
    qtyRow.style.display = displayCssStatus;
    qtyRow.value=0;
    //weightRow.style.display = displayCssStatus;
    enableSkuRow.style.display = displayCssStatus;
}

// 开启规格
function enableSku() {
    setCtrlDisplay("none");
    skuRow.style.display = "";
    cancelValid();
    prepareSkus();
    skuEnabled = true;
    $("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked", "checked");
}

// 关闭规格
function closeSku() {
    if ($(".SpecificationTr").length > 0 && !confirm("关闭规格后现已添加的所有规格数据都会丢失，确定要关闭吗？"))
        return;

    setCtrlDisplay("");
    skuRow.style.display = "none";
    skuTableHolder.empty();
    htSkuItems.clear();
    reBindValid();
    skuEnabled = false;
    $("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked", false);
}

function prepareSkus() {
    skuTable = $("<table width=\"98%\" class=\"table table-bordered table-hover\"><\/table>");
    tableBody = $("<tbody><\/tbody>");
    tableHeader = $("<tr class=\"SpecificationTh\"><\/tr>");

    for (cellIndex = 0; cellIndex < cellFields.length; cellIndex++) {
        var skuId = cellFields[cellIndex];
        var skuItem = htSkuFields.get(skuId);
        var fieldCell = createFieldCell(skuId, skuItem.Name);
        tableHeader.append(fieldCell);
        var skuBox = $(String.format("<div style=\"display: none; position: absolute; z-index: 999;\" id=\"skuBox_{0}\" class=\"target_box\"><\/div>", skuId));
        fillSkuBox(skuBox, skuId, skuItem.SKUValues);
        skuTableHolder.append(skuBox);
    }

    tableHeader.append($("<td align=\"center\">商家编码<\/td>"));
    tableHeader.append($("<td align=\"center\" style=\"width:130px\"><em >*<\/em>现价(元)<\/td>"));
    tableHeader.append($("<td align=\"center\" style=\"width:100px;display:none\">成本价(元)<\/td>"));
    tableHeader.append($("<td align=\"center\" style=\"width:95px\"><em >*<\/em>库存<\/td>"));
    tableHeader.append($("<td align=\"center\" style=\"width:65px;display:none\">重量(克)<\/td>"));
    tableHeader.append($("<td align=\"center\" style=\"width:60px\">操作<\/td>"));
    tableBody.append(tableHeader);

    skuTable.append(tableBody);
    skuTableHolder.append(skuTable);
}

function generateAll() {
    if (cellFields.length == 0) {
        ShowMsg("生成所有规格以前至少需要加入一个规格项！",false);
        return;
    }
    
    var dataRows = $(".SpecificationTr");
    if (dataRows.length > 0 && !confirm("生成所有规格前会先删除已编辑的所有规格，确定吗？"))
        return;
   
    var skuValues = htSkuFields.get(cellFields[0]).SKUValues;
    var skuArray = new Array(skuValues.length);
    if (skuArray.length == 0)
        ShowMsg("生成所有规格以前规格值不能为空！",false);
    $.each(skuValues, function(i, skuValue) {
        skuArray[i] = new Array(1);
        skuArray[i][0] = skuValue;
    });
 
    for (index = 1; index < cellFields.length; index++) {
        skuValues = htSkuFields.get(cellFields[index]).SKUValues;
        var tmpArray = new Array(skuArray.length * skuValues.length);
        var rowCounter = 0;

        for (sindex = 0; sindex < skuValues.length; sindex++) {
            for (cindex = 0; cindex < skuArray.length; cindex++) {
                tmpArray[rowCounter] = new Array(index + 1);
                for (rindex = 0; rindex < (index + 1); rindex++) {
                    if (rindex == index)
                        tmpArray[rowCounter][rindex] = skuValues[sindex];
                    else {
                        tmpArray[rowCounter][rindex] = skuArray[cindex][rindex];
                    }
                }
                rowCounter++;
            }
        }

        skuArray = tmpArray;
    }
 
    $.each(dataRows, function() { $(this).remove(); });
 
   
    for (i = 0; i < skuArray.length; i++) {
        var rowIndex = addItem();
        for (j = 0; j < cellFields.length; j++) {
            var skuItem = skuArray[i][j];
            selectSkuValue(rowIndex, cellFields[j], skuItem.ValueId, skuItem.ValueStr);
        }
    }
}
// 生成部分规格
function generateSku(){
    var dataRows = $(".SpecificationTr");
    var currentSkuFields = getSkuFields();
    var skuValues = currentSkuFields.get(cellFields[0]).SKUValues;
    var skuArray = new Array(skuValues.length);

    $.each(skuValues, function(i, skuValue) {
        skuArray[i] = new Array(1);
        skuArray[i][0] = skuValue;
    });

    for (index = 1; index < cellFields.length; index++) {
        skuValues = currentSkuFields.get(cellFields[index]).SKUValues;
        var tmpArray = new Array(skuArray.length * skuValues.length);
        var rowCounter = 0;

        for (sindex = 0; sindex < skuValues.length; sindex++) {
            for (cindex = 0; cindex < skuArray.length; cindex++) {
                tmpArray[rowCounter] = new Array(index + 1);
                for (rindex = 0; rindex < (index + 1); rindex++) {
                    if (rindex == index)
                        tmpArray[rowCounter][rindex] = skuValues[sindex];
                    else {
                        tmpArray[rowCounter][rindex] = skuArray[cindex][rindex];
                    }
                }
                rowCounter++;
            }
        }

        skuArray = tmpArray;
    }

    
    $.each(dataRows, function() { $(this).remove(); });
    for (i = 0; i < skuArray.length; i++) {
        var rowIndex = addItem();
        for (j = 0; j < cellFields.length; j++) {
            var skuItem = skuArray[i][j];
            selectSkuValue(rowIndex, cellFields[j], skuItem.ValueId, skuItem.ValueStr);
        }
    }
    var objId = 'skuValueBox';
    $('#' + objId).modal('hide');
    //CloseDiv('skuValueBox');
}
// 获取要生成哪些规格
function getSkuFields(){
    var currentSkuFields = new jQuery.Hashtable();
    for (i = 0; i < cellFields.length; i++){
        var skuItems = $(String.format("input[type='checkbox'][name='cp_{0}']:checked", cellFields[i]));
        var skuStr = "({";
        skuStr += String.format("\"Name\":\"{0}\",",htSkuFields.get(cellFields[i]).Name);
        skuStr += String.format("\"AttributeId\":\"{0}\",",cellFields[i]);
        
        var skuValueStr = "";
        $.each(skuItems, function(itemIndex, skuItem) {
            skuValueStr += "{" + String.format("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"",  $(skuItem).attr("valueId"), $(skuItem).attr("valueStr")) + "},";            
        });   
        if(skuValueStr != "")
            skuValueStr = skuValueStr.substring(0, skuValueStr.length - 1);
        skuStr += String.format("\"SKUValues\":[{0}]", skuValueStr);
        skuStr += "})"         
        currentSkuFields.add(cellFields[i],eval(skuStr));
    }
    return currentSkuFields;
}
// 展示要生成的部分规格内容
function showSkuValue(){
    if (cellFields.length == 0) {
        ShowMsg("生成部分规格以前至少需要加入一个规格项！",false);
        return;
    }
    
    $("#skuItems").empty();
    
    var ulTag = $("<ul><\/ul>");
    $("#skuItems").append(ulTag);
    
    var   values;
    for (index = 0; index < cellFields.length; index++) {
        var liTag = $(String.format("<li class=\"skuItem clearfix\" skuId=\"{0}\"><\/li>", cellFields[index]));
        var titleSpan = $(String.format("<span class=\"formitemtitle4\">{0}：<\/span>", htSkuFields.get(cellFields[index]).Name));
        var flag = (htSkuFields.get(cellFields[index]).UseAttributeImage == "1");
        values = htSkuFields.get(cellFields[index]).SKUValues;
        var contentSpan = $("<span class=\"skuItemList\"><\/span>");
        var contentUl = $("<ul><\/ul>");
        contentSpan.append(contentUl);
        liTag.append(titleSpan);
        liTag.append(contentSpan);
        ulTag.append(liTag);
        
         $.each(values, function(i, skuValue) {
            var contentLi = $("<li style=\"clear:none;\"><\/li>");
            var chkItem = $(String.format("<input type=\"checkbox\" name=\"cp_{0}\" id=\"prop_{0}_{1}\" value=\"{0}:{1}\" valueId=\"{1}\" valueStr=\"{2}\" \/>", 
                cellFields[index], skuValue.ValueId, skuValue.ValueStr));
            var valueSpan = $(String.format("<span itemId=\"prop_{1}_{2}\">{0}<\/span>", skuValue.ValueStr, cellFields[index], skuValue.ValueId));
            contentLi.append(chkItem);
            contentLi.append(valueSpan);
            contentUl.append(contentLi);
        });
        if (!flag) {
            var btnLi = $(String.format("<li style=\"float:left;overflow:hidden;width:auto;\" id='li_{0}'><\/li>", cellFields[index]));
            var btn = $(String.format("<input type=button id='btnadd_{0}' class='btn btn-primary btn-xs' value='添加规格值'>", cellFields[index]));
            var ins = $(String.format("<span style='display:none;' id='sp_{0}'><input type=text id='txt_{0}' name='txt_{0}' class='mr5 inputw100'><input type=button id='btnins_{0}' value='保存' onclick='addSkuValue({0})' class='btn btn-success btn-xs'></span>", cellFields[index]));
            $(btn).click(function() {
                $(this).next().show();
                $(this).hide();
            });
            btnLi.append(btn);
            btnLi.append(ins);
            contentUl.append(btnLi);
        }
     }
    // 已经生成的规格默认选中
    $.each($(".specdiv"), function(itemIndex, itemNode) {
        var skuItems = $("input[type='checkbox']");
        $.each(skuItems, function(attIndex, attNode) {
            if($(itemNode).attr("valueId") == $(attNode).attr("valueId"))                
                $(attNode).attr("checked", true);
        });
    });
    
    $("#skuValueBox").modal({ show: true });
}
function addSkuValue(attrId) {
    var valueStr = $("#txt_" + attrId).val();
    $.ajax({
        url: "AddSpecification.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { AttributeId: attrId, ValueName: valueStr, Mode: "AddSkuItemValue", isAjax: "true" },
        async: false,
        success: function(data) {
            if (data.Status == "true") {
                var valueId = data.msg;
                var contentLi = $("<li style=\"clear:none;\"><\/li>");
                var chkItem = $(String.format("<input type=\"checkbox\" name=\"cp_{0}\" id=\"prop_{0}_{1}\" value=\"{0}:{1}\" valueId=\"{1}\" valueStr=\"{2}\"  \/>",
                attrId, valueId, valueStr));
                chkItem.attr("checked", true);
                var valueSpan = $(String.format("<span itemId=\"prop_{1}_{2}\">{0}<\/span>", valueStr, attrId, valueId));
                contentLi.append(chkItem);
                contentLi.append(valueSpan);
                contentLi.insertBefore($("#li_" + attrId));
                $("#sp_" + attrId).hide();
                $("#txt_" + attrId).val();
                $("#btnadd_" + attrId).show();
                var skuItems = htSkuFields.get(attrId).SKUValues;
                var skuItem = { "ValueId": valueId, "ValueStr": valueStr };
                skuItems.push(skuItem);
                //htSkuFields[attrId].SKUValues = skuItems;
            }
            else {
                alert(data.msg)
            }
        }
    });
}
function removeSkuField(skuId, skuName, showConfirm) {
    if (showConfirm && !confirm("确定要从商品规格中删除 \"" + skuName + "\" 吗？"))
        return;

    var fieldCell = $(".table-bordered td[skuId='" + skuId + "']");
    var cellIndex = fieldCell.parent("tr").children().index(fieldCell);

    $(".table-bordered tr").each(function () {
        $("td:eq(" + cellIndex + ")", $(this)).remove();
    });

    var tmpArr = new Array(cellFields.length - 1);
    var counter = 0;
    for (i = 0; i < cellFields.length; i++) {
        if (cellFields[i] != skuId) {
            tmpArr[counter] = cellFields[i];
            counter++;
        }
    }
    cellFields = tmpArr;

    var skuField = $(String.format("<span class=\"skufield\" onclick=\"addSkuField($(this));\" cellIndex=\"{0}\" skuId=\"{1}\" skuName=\"{2}\"><a href=\"javascript:;\">{2}<sup>＋<\/sup><\/a><\/span>", cellIndex, skuId, skuName));
    skuFieldHolder.append(skuField);
    skuFieldHolder.css("display", "");

    htSkuItems.clear();

    $.each($(".SpecificationTr"), function() {
        var rowId = $(this).attr("rowindex");
        var rowIdentity = getRowIdentity(rowId);
        if (htSkuItems.containsValue(rowIdentity))
            $(this).remove();
        else
            htSkuItems.add(rowId, rowIdentity);
    });
}

function addSkuField(skuField) {
    var skuId = $(skuField).attr("skuId");
    var fieldCell = createFieldCell(skuId, $(skuField).attr("skuName"));

    $(fieldCell).insertBefore($("td:eq(0)", $(tableHeader)));
    $.each($(".SpecificationTr"), function() {
        var skuCell = createSkuCell($(this).attr("rowindex"), skuId);
        $(skuCell).insertBefore($("td:eq(0)", $(this)));
    });

    var tmpArr = new Array(cellFields.length + 1);
    tmpArr[0] = skuId;
    for (i = 1; i <= cellFields.length; i++) {
        tmpArr[i] = cellFields[i - 1];
    }
    cellFields = tmpArr;

    $(skuField).remove();
    if ($(skuFieldHolder).children().length == 0)
        skuFieldHolder.css("display", "none");
}

function fillSkuBox(box, skuId, skuValues) {
    $.each(skuValues, function(valIndex, val) {
        box.append($(String.format("<span valueId=\"{0}\" class=\"sku{1}values\" style=\"padding:3px;\">{2}<\/span>", val.ValueId, skuId, val.ValueStr)));
    });
}

var newRowIndex = 0;
// 增加一个规格
function addItem() {
    if(cellFields.length == 0){
        ShowMsg("增加一个规格前必须先选择一个规格名！",false);
        return false;
    }
    newRowIndex++;
    var dataRow = $(String.format("<tr id=\"sku_{0}\" rowindex=\"{0}\" class=\"SpecificationTr\"><\/tr>", newRowIndex));

    for (itemIndex = 0; itemIndex < cellFields.length; itemIndex++) {
        dataRow.append(createSkuCell(newRowIndex, cellFields[itemIndex]));
    }
    dataRow.append(createSkuCodeCell(newRowIndex));
    dataRow.append(createSalePriceCell(newRowIndex));
    dataRow.append(createCostPriceCell(newRowIndex));
    dataRow.append(createQtyCell(newRowIndex));
    dataRow.append(createWeightCell(newRowIndex));
    dataRow.append(createActionCell(newRowIndex));

    tableBody.append(dataRow);
    return newRowIndex;
}

function createFieldCell(skuId, skuName) {
    var fieldCell = $(String.format("<td align=\"center\" class=\"fieldCell\" style=\"width:120px\" skuId=\"{0}\"><span>{1}<\/span><\/td>", skuId, skuName));
    var delCtl = $("<a href=\"javascript:;\" onclick=\"removeSkuField(" + skuId + ",'" + skuName + "', true);\" title=\"删除此规格项\" style=\"color:red;\"><sup>×<\/sup><\/a>");
    fieldCell.append(delCtl);
    return fieldCell;
}

function createSkuCell(rowIndex, skuId) {
    var cell = createCell();
    var panel = $(String.format("<div id=\"skuDisplay_{0}_{1}\" rowId=\"{0}\" skuId=\"{1}\" valueId=\"\" class=\"specdefault\">请选择<\/div>", rowIndex, skuId));
    $(panel).powerFloat({
        eventType: "click",
        target: $("#skuBox_" + skuId),
        showCall: adjustSkuBox
    });
    cell.append(panel);
    return cell;
}

function adjustSkuBox() {
    var rowId = $(this).attr("rowId");
    var skuId = $(this).attr("skuId");
    var skuBox = $("#skuBox_" + skuId);
    var valueList = $(String.format(".sku{0}values", skuId));

    $.each(valueList, function(i, listItem) {
        $(listItem).unbind("click"); //因为每个规格都是用的同一个弹出层，所以必须先取消上一次事件绑定
        if (checkValue(rowId, skuId, $(this).attr("valueId"))) {
            $(listItem).addClass("specsna");
        }
        else {
            $(listItem).addClass("specspan").removeClass("specsna");
            $(listItem).bind("click", function() { selectSkuValue(rowId, skuId, $(this).attr("valueId"), $(this).html()); });
        }
    });
}

function selectSkuValue(rowId, skuId, valueId, valueStr) {
    var displayCtl = $(String.format("#skuDisplay_{0}_{1}", rowId, skuId));
    $(displayCtl).html(valueStr);
    $(displayCtl).attr("valueId", valueId);
    $(displayCtl).addClass("specdiv").removeClass("specdefault");

    var rowIdentity = getRowIdentity(rowId);
    if (htSkuItems.containsKey(rowId))
        htSkuItems.items[rowId] = rowIdentity;
    else
        htSkuItems.add(rowId, rowIdentity);

    $.powerFloat.hide();
}

function checkValue(rowId, skuId, valueId) {
    var rowIdentity = "";
    for (index = 0; index < cellFields.length; index++) {
        if (cellFields[index] == skuId)
            rowIdentity += valueId + "-";
        else
            rowIdentity += $(String.format("#skuDisplay_{0}_{1}", rowId, cellFields[index])).attr("valueId") + "-";
    }
    return htSkuItems.containsValue(rowIdentity);
}

function getRowIdentity(rowId) {
    var rowIdentity = "";
    for (index = 0; index < cellFields.length; index++) {
        rowIdentity += $(String.format("#skuDisplay_{0}_{1}", rowId, cellFields[index])).attr("valueId") + "-";
    }
    return rowIdentity;
}

function createSalePriceCell(rowIndex) {
    var cell = createCell();
    var priceCell = $(String.format("<input type=\"text\" class=\"skuItem_SalePrice\" id=\"salePrice_{0}\"\ style=\"width:50px;\" \/>", rowIndex));
    var gradePrice = $(String.format("<input type=\"text\" id=\"gradeSalePrice_{0}\"\ style=\"width:50px;display:none\" \/>", rowIndex));
    var btnSkuMemberPrice = $("<input type=\"button\" value=\"会员价\" onclick=\"editSkuMemberPrice(" + rowIndex + ");\" \/>");
    $(priceCell).val($("#ctl00_ContentPlaceHolder1_txtSalePrice").val());
    $(gradePrice).val($("#ctl00_ContentPlaceHolder1_txtMemberPrices").val());
    cell.append(priceCell);
    cell.append(gradePrice);
    cell.append(btnSkuMemberPrice);
    return cell;
}

function createCostPriceCell(rowIndex) {
    var cell = $("<td align=\"center\" style=\"display:none\"><\/td>");// createCell();
    var priceCell = $(String.format("<input type=\"text\" class=\"skuItem_CostPrice\" id=\"costPrice_{0}\" style=\"width:50px;\" \/>", rowIndex));
    $(priceCell).val($("#ctl00_ContentPlaceHolder1_txtCostPrice").val());
    cell.append(priceCell);
    return cell;
}

function createQtyCell(rowIndex) {
    var cell = createCell();
    var quantityCell = $(String.format("<input type=\"text\" class=\"skuItem_Qty\" id=\"qty_{0}\" style=\"width:90px;\" \/>", rowIndex));
    $(quantityCell).val($("#ctl00_ContentPlaceHolder1_txtStock").val());
    cell.append(quantityCell);
    return cell;
}

function createWeightCell(rowIndex) {
    var cell = $("<td align=\"center\" style=\"display:none\"><\/td>");// createCell();
    var weightCell = $(String.format("<input type=\"text\" class=\"skuItem_Weight\" id=\"weight_{0}\" style=\"width:35px;\" \/>", rowIndex));
    $(weightCell).val($("#ctl00_ContentPlaceHolder1_txtWeight").val());
    cell.append(weightCell);
    return cell;
}

function createSkuCodeCell(rowIndex) {
    var cell = createCell();
    var skuCodeCell = $(String.format("<input type=\"text\" class=\"skuItem_SkuCode\" id=\"skuCode_{0}\" \/>", rowIndex));
    if ($("#ctl00_ContentPlaceHolder1_txtSku").val().length > 0) $(skuCodeCell).val($("#ctl00_ContentPlaceHolder1_txtSku").val() + "-" + rowIndex);
    cell.append(skuCodeCell);
    return cell;
}

function createActionCell(rowIndex) {
    var cell = createCell();
    var actionCell = $(String.format("<a href=\"javascript:;\" onclick=\"removeSku({0});\"><img src=\"../images/ta.gif\" title=\"删除此商品规格\" border=\"0\" \/><\/a>", rowIndex));
    cell.append(actionCell);
    return cell;
}

function createCell() {
    return $("<td align=\"center\"><\/td>");
}

function removeSku(rowIndex) {
    if (!confirm("规格数据删除以后不能恢复，确定要删除吗？"))
        return;

    $("#sku_" + rowIndex).remove();
    htSkuItems.remove(rowIndex);
}

/* 
开启规格以后，需要把隐藏的有客户端验证的控件取消验证，关闭规格后，又要重新开启验证
原理：每个需要验证的控件在initValid中都会被追加一个ValidateGroup属性，如果开发人员没有手工指定属性值，
则属性值默认为：“default”，执行PageIsValid()开始验证时，脚本会使用属性筛选器通过ValidateGroup属性筛选出
需要执行验证的控件，然后分别对每个控件执行验证。所以，如果要取消某个控件不进行客户端验证，只要
把这个控件的ValidateGroup属性删掉，需要再次验证时，再添加上去即可。
*/

// 取消需隐藏控件的客户端验证
function cancelValid() {
    $("#ctl00_ContentPlaceHolder1_txtSalePrice").removeAttr("ValidateGroup");
    $("#ctl00_ContentPlaceHolder1_txtCostPrice").removeAttr("ValidateGroup");
    $("#ctl00_ContentPlaceHolder1_txtStock").removeAttr("ValidateGroup");
    $("#ctl00_ContentPlaceHolder1_txtWeight").removeAttr("ValidateGroup");
}

// 重新绑定需隐藏控件的客户端验证
function reBindValid() {
    $("#ctl00_ContentPlaceHolder1_txtSalePrice").attr("ValidateGroup", "default");
    $("#ctl00_ContentPlaceHolder1_txtCostPrice").attr("ValidateGroup", "default");
    $("#ctl00_ContentPlaceHolder1_txtStock").attr("ValidateGroup", "default");
    $("#ctl00_ContentPlaceHolder1_txtWeight").attr("ValidateGroup", "default");
}

function doSubmit() {
    // 1.先执行jquery客户端验证检查其他表单项
    //if (!PageIsValid())
    //    return false;
    
    // 2.如果开启了规格，则做以下检查
    if (skuEnabled) {
       
        // 商品规格数量需大于等于2
        if ($(".SpecificationTr").length < 1) {
            ShowMsg("开启规格以后，您至少需要增加一个商品规格！", false);
            return false;
        }
        // 检查有无规格值为空的情况
        if ($(".specdefault").length > 0) {
            ShowMsg("您需要为每一个规格项选择一个值！", false);
            return false;
        }

        // 检查规格字段输入数据的有效性
        if (!checkSkuCode()) return false;
        if (!checkSalePrice()) return false;
        if (!checkCostPrice()) return false;
        if (!checkQty()) return false;
        if (!checkWeight()) return false;
    } else {
       
        var saleprice = $("#ctl00_ContentPlaceHolder1_txtSalePrice").val();
        var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
        if (!exp.test(saleprice)) {
            var obj = $("#ctl00_ContentPlaceHolder1_txtSalePrice");
            obj.parent().parent().addClass("has-error");
            obj.blur(function () {
                if (exp.test($(this).val())) {
                    obj.parent().parent().removeClass("has-error");
                } else {
                    obj.parent().parent().addClass("has-error");
                }
            })
        } else {
            var obj = $("#ctl00_ContentPlaceHolder1_txtSalePrice");
            obj.parent().parent().removeClass("has-error");
        }
    }
    
    // 2.如果开启了规格，则做以下检查
    // 收集扩展属性和规格数据
    loadAttributes();
    loadSkus();

    //这一段必须加。有的浏览器在attr("checked") == "checked" 仍然是未选中状态
    if ($("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked") == "checked" || $("#ctl00_ContentPlaceHolder1_chkSkuEnabled").attr("checked")==true) {
        $("#ctl00_ContentPlaceHolder1_chkSkuEnabled").prop("checked", true);
    };
  
    return true;
}

function loadAttributes() {
    var attributesXml = "<xml><attributes>";
    $.each($(".attributeItem"), function (i, att) {
        var attributeId = $(att).attr("attributeId");
        var usageMode = $(att).attr("usageMode");
        var itemXml = String.format("<item attributeId=\"{0}\" usageMode=\"{1}\">", attributeId, usageMode);
        if (usageMode == "0") {
            //多选属性
            $.each($("input[name='vallist" + attributeId + "']:checked"), function () {
                itemXml += String.format("<attValue valueId=\"{0}\" \/>", $(this).attr("valueId"));
            });
        }
        else {
            itemXml += String.format("<attValue valueId=\"{0}\" \/>", $("#attribute" + attributeId).val());
        }

        itemXml += "<\/item>";
        attributesXml += itemXml;
    });

    attributesXml += "<\/attributes><\/xml>";
    $("#ctl00_ContentPlaceHolder1_txtAttributes").val(attributesXml);
}

function loadSkus() {
    var skusXml = "<xml><productSkus>";
    $.each($(".SpecificationTr"), function (i, valitem) {
        var rowIndex = $(valitem).attr("rowindex");
        var skuCode = $("#skuCode_" + rowIndex).val();
        var salePrice = $("#salePrice_" + rowIndex).val();
        var costPrice = $("#costPrice_" + rowIndex).val();
        var purchasePrice = $("#purchasePrice_" + rowIndex).val();
        var qty = $("#qty_" + rowIndex).val();
        var alertQty = $("#alertQty_" + rowIndex).val();
        var weight = $("#weight_" + rowIndex).val();
        var itemXml = String.format("<item skuCode=\"{0}\" salePrice=\"{1}\" costPrice=\"{2}\" purchasePrice=\"{3}\" qty=\"{4}\" alertQty=\"{5}\" weight=\"{6}\">", skuCode, salePrice, costPrice, purchasePrice, qty, alertQty, weight);
        itemXml += "<skuFields>";
        for (i = 0; i < cellFields.length; i++) {
            itemXml += String.format("<sku attributeId=\"{0}\" valueId=\"{1}\" \/>", cellFields[i], $(String.format("#skuDisplay_{0}_{1}", rowIndex, cellFields[i])).attr("valueId"));
        }
        itemXml += "<\/skuFields>";

        // 获取每个规格的会员价
        if ($("#gradeSalePrice_" + rowIndex).val().length > 0) {
            itemXml += "<memberPrices>";

            var xml;
            if (/msie/.test(navigator.userAgent.toLowerCase())) {//;$.browser.msie
                xml = new ActiveXObject("Microsoft.XMLDOM");
                xml.async = false;
                xml.loadXML($("#gradeSalePrice_" + rowIndex).val());
            }
            else {
                xml = new DOMParser().parseFromString($("#gradeSalePrice_" + rowIndex).val(), "text/xml");
            }

            $.each($(xml).find("grande"), function () {
                itemXml += String.format("<memberGrande id=\"{0}\" price=\"{1}\" />", $(this).attr("id"), $(this).attr("price"));
            });
            itemXml += "<\/memberPrices>";
        }        

        itemXml += "<\/item>";
        skusXml += itemXml;
    });
    skusXml += "<\/productSkus><\/xml>";
    $("#ctl00_ContentPlaceHolder1_txtSkus").val(skusXml);
}

function checkSkuCode() {
    var validated = true;
    $.each($(".skuItem_SkuCode"), function() {
        if ($(this).val().length > 20) {
            ShowMsg("商家编码的长度不能超过20个字符！", false);
            $(this).focus();
            validated = false;
            return false;
        }
    });

    return validated;
}

function checkSalePrice() {
    return checkPrice($(".skuItem_SalePrice"), true, "现价");
}

function checkCostPrice() {
    return checkPrice($(".skuItem_CostPrice"), false, "成本价");
}

function checkQty() {
    return checkNumber($(".skuItem_Qty"), true, "库存");
}

function checkWeight() {
    return checkPrice($(".skuItem_Weight"), false, "重量");
}

function checkPrice(inputItems, required, priceName) {
    var validated = true;
    var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

    $.each(inputItems, function() {
        var val = $(this).val();
        // 检查必填项是否填了
        if (required && val.length == 0) {
            ShowMsg(String.format("商品规格的{0}为必填项！", priceName),false);
            $(this).focus();
            validated = false;
            return false;
        }

        if (val.length > 0) {
            // 检查输入的是否是有效的金额
            if (!exp.test(val)) {
                ShowMsg(String.format("商品规格的{0}输入有误！", priceName),false);
                $(this).focus();
                validated = false;
                return false;
            }

            // 检查金额是否超过了系统范围
            var num = parseFloat(val);
            if (!((num >= 0.01) && (num <= 10000000))) {
                ShowMsg(String.format("商品规格的{0}超出了系统表示范围！", priceName),false);
                $(this).focus();
                validated = false;
                return false;
            }
        }
    });

    return validated;
}

function checkNumber(inputItems, required, numberName) {
    var validated = true;
    var exp = new RegExp("^-?[0-9]\\d*$", "i");

    $.each(inputItems, function() {
        var val = $(this).val();

        // 检查必填项是否填了
        if (required && val.length == 0) {
            ShowMsg(String.format("商品规格的{0}为必填项！", numberName),false);
            $(this).focus();
            validated = false;
            return false;
        }

        if (val.length > 0) {
            // 检查输入的是否是有效的数字
            if (!exp.test(val)) {
                ShowMsg(String.format("商品规格的{0}输入有误！", numberName),false);
                $(this).focus();
                validated = false;
                return false;
            }

            // 检查数字是否超过了系统范围
            var num = parseFloat(val);
            if (!((num >= 0) && (num <= 9999999))) {
                ShowMsg(String.format("商品规格的{0}超出了系统表示范围！", numberName),false);
                $(this).focus();
                validated = false;
                return false;
            }
        }
    });
    return validated;
}