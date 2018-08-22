function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&");
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}

///////////////////////////////////////////////////////////////////////////////////
// 检验textarea输入字数控制的方法
///////////////////////////////////////////////////////////////////////////////////
function InitTextCounter(MaxInputLength, CounterInput, ShowCounter) {
    var obj = CounterInput;
    var showid = ShowCounter;
    var maxlimit = MaxInputLength;

    maxlimit = maxlimit * 2;
    var len = byteLength($(obj).val());

    if (len > maxlimit) {
        $(obj).val(getStrbylen($(obj).val(), maxlimit));
        if (ShowCounter != null) {
            $(showid).text('0');
        }
    } else {
        if (ShowCounter != null) {
            $(showid).text(Math.floor((maxlimit - len) / 2));/*(maxlimit - len) / 2*/
        }
    }

    $(obj).unbind("keyup");
    $(obj).bind("keyup", function () { InitTextCounter(MaxInputLength, CounterInput, ShowCounter) });
    $(obj).unbind("paste");
    $(obj).bind("paste", function () { InitTextCounter(MaxInputLength, CounterInput, ShowCounter) });
}
///////////////////////////////////////////////////////////////////////////////////
// 获取字节数
///////////////////////////////////////////////////////////////////////////////////
function byteLength(sStr) {
    aMatch = sStr.match(/[^\x00-\x80]/g);
    return (sStr.length + (!aMatch ? 0 : aMatch.length));
}
///////////////////////////////////////////////////////////////////////////////////
// 返回指定长度字符串
///////////////////////////////////////////////////////////////////////////////////
function getStrbylen(str, len) {
    var num = 0;
    var strlen = 0;
    var newstr = "";
    var obj_value_arr = str.split("");
    for (var i = 0; i < obj_value_arr.length; i++) {
        if (i < len && num + byteLength(obj_value_arr[i]) <= len) {
            num += byteLength(obj_value_arr[i]);
            strlen = strlen + 1;
        }
    }
    if (str.length > strlen) {
        newstr = str.substr(0, strlen);
    } else {
        newstr = str;
    }
    return newstr;
}
