var pluginContainer, templateRow;
var dropPlugins, selectedNameCtl, configDataCtl;
var InputType = { "TextBox": 0, "TextArea": 1, "DropDownList": 2, "CheckBox": 3, "Password": 5 };

function SelectPlugin(pluginType) {
    ResetContainer($(dropPlugins).val(), pluginType);
}

function ResetContainer(name, pluginType) {
    if (pluginContainer.length == 0) {
        return;
    }

    $.each($(pluginContainer).find("[rowType=attributeContent]"), function(i, item) {
            $(item).remove();
    });

    if (name.length == 0)
        return;
        
    // 如果选择了插件，则根据选择的插件获取对应的配置信息
    $.ajax({
        url: "PluginHandler.aspx?type=" + pluginType + "&name=" + name + "&action=getmetadata",
        type: 'GET',
        dataType: 'xml',
        timeout: 10000,
        success: function(xml) {
            CreateContainer(xml,name);
        }
    });
}

function CreateContainer(meta,typename) {
    var dataXml;
    var hasValue = false;

    if ($(configDataCtl).val().length > 0) {
        var s = $(configDataCtl).val().replace("<xml>", "<root>");
        s = s.replace("</xml>", "</root>");

        if ($.browser.msie) {
            dataXml = new ActiveXObject("Microsoft.XMLDOM");
            dataXml.async = false;
            dataXml.loadXML("<xml>" + s + "</xml>");
        }
        else {
            dataXml = new DOMParser().parseFromString("<xml>" + s + "</xml>", "text/xml");
        }

        hasValue = true;
    }

    $.each($(meta).find("att"), function(i, att) {
        var attributeRow = templateRow.clone();
        var t = attributeRow.html();
        t = t.replace("$Name$", $(att).attr("Name"));
        t = t.replace("$Description$", $(att).attr("Description"));

        var strInputInnerHtml = "";
        var strLink="";
        var strItem = "";
        var strData = "";
        switch(typename){
            case "hishop.plugins.payment.alipayassure.assurerequest"://担保交易
                strLink="　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=escrow' target='_blank'>获取PID、Key</a>";
            break;
            case "hishop.plugins.payment.alipaydirect.directrequest"://即时交易
                strLink="　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=fastpay' target='_blank'>获取PID、Key</a>";
            break;
            case "hishop.plugins.payment.alipay.standardrequest"://双接口
                strLink="　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=dualpay' target='_blank'>获取PID、Key</a>";
            break;
            default:
                strLink="";
            break;
        };

        if (hasValue) {
            strData = $(dataXml).find($(att).attr("Property")).text();
        }

        switch (Number($(att).attr("InputType"))) {
            case InputType.TextBox:
                strInputInnerHtml = "<input name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' value='" + strData + "'\/>";
                 if($(att).attr("Property")=="Partner"){
                    strInputInnerHtml+=strLink;
                }
                break;

            case InputType.Password:
                strInputInnerHtml = "<input name='" + $(att).attr("Property") + "' type='password' id='" + $(att).attr("Property") + "' value='" + strData + "'\/>";
                break;

            case InputType.TextArea:
                strInputInnerHtml = "<textarea name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "'>" + strData + "<\/textarea>";
                break;

            case InputType.CheckBox:
                var check = strData;
                if (strData.toLowerCase() == "true") {
                    strData = "checked=checked";
                }

                strInputInnerHtml = "<input type='checkbox' onclick='this.value=this.checked;' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' " + strData + " value='" + check + "'\/>";
                break;

            case InputType.DropDownList:
                strInputInnerHtml = "<select name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "'>";
                $.each($(att).find("Options").find("Item"), function(j, item) {
                    if (strData == $(item).text())
                        strItem = "<Option selected='selected' value='" + $(item).text() + "'>" + $(item).text() + "<\/Option>";
                    else
                        strItem = "<Option value='" + $(item).text() + "'>" + $(item).text() + "<\/Option>";
                    strInputInnerHtml += strItem;
                });
                strInputInnerHtml += "<\/select>";
                break;

            default:
                break;
        }
        //alert("strInputInnerHtml=" + strInputInnerHtml);

        t = t.replace("$Input$", strInputInnerHtml);
        attributeRow.html(t);
        attributeRow.attr("rowType", "attributeContent");
        attributeRow.css('display', '');
        attributeRow.appendTo(pluginContainer);
    });
}