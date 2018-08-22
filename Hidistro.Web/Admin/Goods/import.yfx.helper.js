var tbTypes;
var txtPTXml, txtProductTypeXml;
var infoZone;
var chkFlag;
var ptRow;
var attributesDoc = null, valuesDoc = null;

$(document).ready(function () {
    tbTypes = $("#tbTypes");
    infoZone = $("#infoZone");
    txtPTXml = $("#ctl00_ContentPlaceHolder1_txtPTXml");
    chkFlag = $("#ctl00_ContentPlaceHolder1_chkFlag");
    txtProductTypeXml = $("#ctl00_ContentPlaceHolder1_txtProductTypeXml");
    ptRow = $("#ptRow");

    init();
    $("#tTypes").treeTable({
        initialState: "expanded"
    });
});

function init() {
    if ($(chkFlag).attr("checked") == "checked") {
        processTypes();
        $(infoZone).show();
    }
    else {
        $(tbTypes).empty();
        $(infoZone).hide();
    }
}

function processTypes() {
    var doc;

    if (/msie/.test(navigator.userAgent.toLowerCase())) {
        doc = new ActiveXObject("Microsoft.XMLDOM");
        doc.async = false;
        doc.loadXML(txtPTXml.val());
    }
    else {
        doc = new DOMParser().parseFromString(txtPTXml.val(), "text/xml");
    }

    var typeList = $(doc).find("indexes>types>type");

    if (typeList.length == 0) {
        $(ptRow).hide();
        return;
    }

    $.each(typeList, function() {
        var typeId = $(this).find("typeId").text();
        var typeName = $(this).find("typeName").text();
        var typeRowId = "t-" + typeId;
        var typeRow = $(String.format("<tr id=\"{0}\"><td><strong>{1}</strong><\/td><td><\/td><\/tr>", typeRowId, typeName));

        var typeSelector = createTypeSelector(typeId, typeName);
        var selectedTypeId = fillTypeSelector(typeName, typeSelector);

        $(tbTypes).append(typeRow);
        $("td", typeRow).eq(1).append(typeSelector);

        var attributeList = $(this).find("attributes>attribute");
        processAttributes(selectedTypeId, typeId, attributeList, typeRowId);
    });

    $(ptRow).show();
}

function processAttributes(selectedTypeId, mappedTypeId, attributeList, typeRowId) {
    $.each(attributeList, function() {
        var attributeId = $(this).find("attributeId").text();
        var attributeName = $(this).find("attributeName").text();
        var attributeRowId = typeRowId + "-" + attributeId;
        var attributeRow = $(String.format("<tr id=\"{0}\" class=\"child-of-{1}\"><td><u>{2}</u><\/td><td><\/td><\/tr>", attributeRowId, typeRowId, attributeName));

        var attributeSelector = createAttributeSelector(attributeId, attributeName, mappedTypeId);
        var selectedAttributeId = fillAttributeSelector(selectedTypeId, attributeName, attributeSelector);

        $(tbTypes).append(attributeRow);
        $("td", attributeRow).eq(1).append(attributeSelector);

        var valueList = $(this).find("values>value");
        processValues(selectedAttributeId, attributeId, valueList, attributeRowId);
    });
}

function processValues(selectedAttributeId, mappedAttributeId, valueList, attributeRowId) {
    $.each(valueList, function() {
        var valueId = $(this).find("valueId").text();
        var valueStr = $(this).find("valueStr").text();

        var usageMode = $(this).find("usageMode").text();
        var useAttributeImage = $(this).find("useAttributeImage").text();
        var image = $(this).find("image").text();

        var valueRowId = attributeRowId + "-" + valueId;
        var valueRow = $(String.format("<tr id=\"{0}\" class=\"child-of-{1}\"><td>{2}<\/td><td><\/td><\/tr>", valueRowId, attributeRowId, valueStr, selectedAttributeId, valueId));

        var valueSelector = createValueSelector(valueId, valueStr, mappedAttributeId, usageMode, useAttributeImage, image);
        fillValueSelector(selectedAttributeId, valueStr, valueSelector);

        $(tbTypes).append(valueRow);
        $("td", valueRow).eq(1).append(valueSelector);
    });
}

function createTypeSelector(mappedTypeId, mappedTypeName) {
    var selector = $(String.format("<select t=\"type\" mappedTypeId=\"{0}\" mappedTypeName=\"{1}\"><\/select>", mappedTypeId, mappedTypeName));
    $(selector).append($("<option value=\"0\">新增<\/option>"));
    $(selector).bind("change", function() { typeChanged($(this)); });
    return selector;
}

function createAttributeSelector(mappedAttributeId, mappedAttributeName, mappedTypeId) {
    var selector = $(String.format("<select t=\"attribute\" mappedAttributeId=\"{0}\" mappedTypeId=\"{1}\" mappedAttributeName=\"{2}\"><\/select>", mappedAttributeId, mappedTypeId, mappedAttributeName));
    $(selector).append($("<option value=\"0\">新增<\/option>"));
    $(selector).bind("change", function() { attributeChanged($(this)); });
    return selector;
}

function createValueSelector(mappedValueId, mappedValueStr, mappedAttributeId, usageMode, useAttributeImage, image) {
    var selector = $(String.format("<select t=\"value\" mappedValueId=\"{0}\" mappedAttributeId=\"{1}\" mappedValueStr=\"{2}\"><\/select>", mappedValueId, mappedAttributeId, mappedValueStr));
    $(selector).append($("<option value=\"0\">新增<\/option>"));
    return selector;
}

function fillTypeSelector(mappedTypeName, selector) {
    var types = $($(txtProductTypeXml).val()).find("item");
    var selectedTypeId = "0";
    var loweredTypeName = mappedTypeName.toLowerCase();

    if (types.length > 0) {
        $(selector).append($("<option value=\"\" disabled>--------------<\/option>"));
    }

    $.each(types, function() {
        var ctrlOption;
        if (selectedTypeId == 0 && $(this).attr("typeName").toLowerCase() == loweredTypeName) {
            selectedTypeId = $(this).attr("typeId");
            ctrlOption = $(String.format("<option value=\"{0}\" selected=\"selected\">{1}<\/option>", selectedTypeId, $(this).attr("typeName")));
        }
        else {
            ctrlOption = $(String.format("<option value=\"{0}\">{1}<\/option>", $(this).attr("typeId"), $(this).attr("typeName")));
        }
        $(selector).append(ctrlOption);
    });

    return selectedTypeId;
}

function fillAttributeSelector(selectedTypeId, mappedAttributeName, selector) {
    var selectedAttributeId = "0";
    var loweredAttributeName = mappedAttributeName.toLowerCase();

    if (attributesDoc == null || $(attributesDoc).find("item[typeId='" + selectedTypeId + "']").length == 0) {
        getAttributes(selectedTypeId);
    }

    var attributes = $(attributesDoc).find("item[typeId='" + selectedTypeId + "']");
    if (attributes.length > 0) {
        $(selector).append($("<option value=\"\" disabled>--------------<\/option>"));
    }

    $.each(attributes, function() {
        var ctrlOption;
        if (selectedAttributeId == 0 && $(this).attr("attributeName").toLowerCase() == loweredAttributeName) {
            selectedAttributeId = $(this).attr("attributeId");
            ctrlOption = $(String.format("<option value=\"{0}\" selected=\"selected\">{1}<\/option>", selectedAttributeId, $(this).attr("attributeName")));
        }
        else {
            ctrlOption = $(String.format("<option value=\"{0}\">{1}<\/option>", $(this).attr("attributeId"), $(this).attr("attributeName")));
        }
        $(selector).append(ctrlOption);
    });

    return selectedAttributeId;
}

function fillValueSelector(selectedAttributeId, valueStr, selector) {
    var selectedValueId = "0";
    var loweredValueStr = valueStr.toLowerCase();

    if (valuesDoc == null || $(valuesDoc).find("item[attributeId='" + selectedAttributeId + "']").length == 0) {
        getValues(selectedAttributeId);
    }

    var values = $(valuesDoc).find("item[attributeId='" + selectedAttributeId + "']");
    if (values.length > 0) {
        $(selector).append($("<option value=\"\" disabled>--------------<\/option>"));
    }

    $.each(values, function() {
        var ctrlOption;
        if (selectedValueId == 0 && $(this).attr("valueStr").toLowerCase() == loweredValueStr) {
            selectedValueId = $(this).attr("valueId");
            ctrlOption = $(String.format("<option value=\"{0}\" selected=\"selected\">{1}<\/option>", selectedValueId, $(this).attr("valueStr")));
        }
        else {
            ctrlOption = $(String.format("<option value=\"{0}\">{1}<\/option>", $(this).attr("valueId"), $(this).attr("valueStr")));
        }
        $(selector).append(ctrlOption);
    });

    return selectedValueId;
}

function getAttributes(typeId) {
    var postUrl = "ImportFromYfx.aspx?isCallback=true&action=getAttributes&timestamp=";
    postUrl += new Date().getTime() + "&typeId=" + typeId;

    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'xml', timeout: 10000,
        async: false,
        success: function(resultData) {
            if (attributesDoc == null)
                attributesDoc = resultData;
            else {
                var root = $(attributesDoc).find("attributes");
                $.each($(resultData).find("item"), function() {
                    $(root).append($(this));
                });
            }
        }
    });
}

function getValues(attributeId) {
    var postUrl = "ImportFromYfx.aspx?isCallback=true&action=getValues&timestamp=";
    postUrl += new Date().getTime() + "&attributeId=" + attributeId;

    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'xml', timeout: 10000,
        async: false,
        success: function(resultData) {
            if (valuesDoc == null)
                valuesDoc = resultData;
            else {
                var root = $(valuesDoc).find("values");
                $.each($(resultData).find("item"), function() {
                    $(root).append($(this));
                });
            }
        }
    });
}

function typeChanged(selector) {
    var selectedTypeId = $(selector).val();
    var mappedTypeId = $(selector).attr("mappedTypeId");
    var attributeSelectors = $("select[t='attribute'][mappedTypeId='" + mappedTypeId + "']", tbTypes);

    $.each(attributeSelectors, function() {
        var mappedAttributeName = $(this).attr("mappedAttributeName");
        var newSelector = createAttributeSelector($(this).attr("mappedAttributeId"), mappedAttributeName, $(this).attr("mappedTypeId"));
        var selectedAttributeId = "0";

        if (selectedTypeId != "0") {
            selectedAttributeId = fillAttributeSelector(selectedTypeId, mappedAttributeName, newSelector);
        }

        var mappedAttributeId = $(this).attr("mappedAttributeId");
        resetValueSelectors(selectedAttributeId, mappedAttributeId);

        $(this).parent().append(newSelector);
        $(this).remove();
    });
}

function attributeChanged(selector) {
    var selectedAttributeId = $(selector).val();
    var mappedAttributeId = $(selector).attr("mappedAttributeId");
    resetValueSelectors(selectedAttributeId, mappedAttributeId);
}

function resetValueSelectors(selectedAttributeId, mappedAttributeId) {
    var valuesSelectors = $("select[t='value'][mappedAttributeId='" + mappedAttributeId + "']", tbTypes);
    $.each(valuesSelectors, function() {
        var mappedValueStr = $(this).attr("mappedValueStr");

        var usageMode = $(this).find("usageMode").text();
        var useAttributeImage = $(this).find("useAttributeImage").text();
        var image = $(this).find("image").text();

        var newValueSelector = createValueSelector($(this).attr("mappedValueId"), mappedValueStr, $(this).attr("mappedAttributeId"), usageMode, useAttributeImage, image);
        if (selectedAttributeId != "0") {
            fillValueSelector(selectedAttributeId, mappedValueStr, newValueSelector);
        }
        
        $(this).parent().append(newValueSelector);
        $(this).remove();
    });
}

function doImport() {
    if ($("#ctl00_ContentPlaceHolder1_dropImportVersions").val().length == 0) {
        alert("请选择一个导入插件！");
        return false;
    }

    if ($("#ctl00_ContentPlaceHolder1_dropFiles").val().length == 0) {
        alert("请选择要导入的数据包文件！");
        return false;
    }

    if ($("#ctl00_ContentPlaceHolder1_dropCategories").val().length == 0) {
        alert("请选择要导入的商品分类！");
        return false;
    }

    if ($("#ctl00_ContentPlaceHolder1_dropProductLines").val().length == 0) {
        alert("请选择要导入的产品线！");
        return false;
    }

    if ($(tbTypes).children().length > 0) {
        loadDataTypes();
    }

    return true;
}

function loadDataTypes() {
    var dataXml = "<xml><types>";
    var typeSelectors = $("select[t='type']", tbTypes);

    $.each(typeSelectors, function() {
        var mappedTypeId = $(this).attr("mappedTypeId");
        var mappedTypeName = $(this).attr("mappedTypeName");
        var selectedTypeId = $(this).val();

        dataXml += String.format("<type mappedTypeId=\"{0}\" mappedTypeName=\"{1}\" selectedTypeId=\"{2}\">", mappedTypeId, mappedTypeName, selectedTypeId);
        dataXml += "<attributes>";

        var attributeSelectors = $("select[t='attribute'][mappedTypeId='" + mappedTypeId + "']", tbTypes);
        $.each(attributeSelectors, function() {
            var mappedAttributeId = $(this).attr("mappedAttributeId");
            var mappedAttributeName = $(this).attr("mappedAttributeName");
            var selectedAttributeId = $(this).val();

            dataXml += String.format("<attribute mappedAttributeId=\"{0}\" mappedAttributeName=\"{1}\" selectedAttributeId=\"{2}\">", mappedAttributeId, mappedAttributeName, selectedAttributeId)
            dataXml += "<values>";

            var valueSelectors = $("select[t='value'][mappedAttributeId='" + mappedAttributeId + "']", tbTypes);
            $.each(valueSelectors, function() {
                var mappedValueId = $(this).attr("mappedValueId");
                var mappedValueStr = $(this).attr("mappedValueStr");
                var selectedValueId = $(this).val();

                dataXml += String.format("<value mappedValueId=\"{0}\" mappedValueStr=\"{1}\" selectedValueId=\"{2}\" \/>", mappedValueId, mappedValueStr, selectedValueId);
            });

            dataXml += "<\/values>";
            dataXml += "<\/attribute>";
        });

        dataXml += "<\/attributes>";
        dataXml += "<\/type>";
    });

    dataXml += "<\/types><\/xml>";
    $("#ctl00_ContentPlaceHolder1_txtMappedTypes").val(dataXml);
}