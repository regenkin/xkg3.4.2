// JavaScript Document

//选择点击一级菜单显示
//selecttype

///////////////////////////////////////////////////////////////////////////////////
// IE Check
///////////////////////////////////////////////////////////////////////////////////
var isIE = (document.all) ? true : false;

///////////////////////////////////////////////////////////////////////////////////
// Image Helper
///////////////////////////////////////////////////////////////////////////////////
var imageObject = null;
var currentObject = null;

function ResizeImage(I, W, H) {
    if (I.length > 0 && imageObject != null && currentObject != I) {
        setTimeout("ResizeImage('" + I + "'," + W + "," + H + ")", 100);
        return;
    }

    var F = null;
    if (I.length > 0) {
        F = document.getElementById(I);
    }

    if (F != null) {
        imageObject = F;
        currentObject = I;
    }

    if (isIE) {
        if (imageObject.readyState != "complete") {
            setTimeout("ResizeImage(''," + W + "," + H + ")", 50);
            return;
        }
    }
    else if (!imageObject.complete) {
        setTimeout("ResizeImage(''," + W + "," + H + ")", 50);
        return;
    }

    var B = new Image();
    B.src = imageObject.src;
    var A = B.width;
    var C = B.height;
    if (A > W || C > H) {
        var a = A / W;
        var b = C / H;
        if (b > a) {
            a = b;
        }
        A = A / a;
        C = C / a;
    }
    if (A > 0 && C > 0) {
        imageObject.style.width = A + "px";
        imageObject.style.height = C + "px";
    }

    imageObject.style.display = '';
    imageObject = null;
    currentObject = null;
}

///////////////////////////////////////////////////////////////////////////////////
// 商品展示页
///////////////////////////////////////////////////////////////////////////////////
function OneProductView(articleid) {
    var content = '<iframe src="/ProductDetails.aspx?ProductId=' + articleid + '" id="ifmMobile" width="100%" scrolling="no" frameborder="0"></iframe>';
    MobileContentShow('商品展示页', content);
}

///////////////////////////////////////////////////////////////////////////////////
// 素材展示页
///////////////////////////////////////////////////////////////////////////////////
function ArticleView(articleid) {
    var content = '<iframe src="/vshop/ArticleShow.aspx?id=' + articleid + '" id="ifmMobile" width="100%" scrolling="no" frameborder="0"></iframe>';
    MobileContentShow('素材展示页', content);
}
///////////////////////////////////////////////////////////////////////////////////
// 手机预览页面的iframe重置高度调用
///////////////////////////////////////////////////////////////////////////////////
function CallBack_MobileFramMain(h) {
    //alert(h)
    $("#ifmMobile").height(h);
}
///////////////////////////////////////////////////////////////////////////////////
// 设置对象的高度
///////////////////////////////////////////////////////////////////////////////////
function SetObjHeight(obj, h) {
    $(obj).height(h);
}
///////////////////////////////////////////////////////////////////////////////////
// 手机展示效果页
///////////////////////////////////////////////////////////////////////////////////
function MobileContentShow(title, content) {
    var divID = "previewshow";
    var s = '<div class="modal fade" id="' + divID + '">' +
    '            <div class="modal-dialog">'+
    '                 <div class="modal-content">'+
    '                   <div class="modal-header">'+
    '                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>'+
    '                        <h4 class="modal-title">手机预览效果</h4>'+
    '                    </div>'+
    '                    <div class="modal-body">' +
    '                        <div class="edit-text-left"><div class="mobile-border">'+
    '                           <div class="mobile-d">'+
    '                                <div class="mobile-header">'+
    '                                    <i></i>'+
    '                                    <div class="mobile-title" onclick="history.go(-1)">'+title+'</div>'+
    '                                </div>'+
    '                                <div class="set-overflow">'+
    '                                    <div class="mobile mate-list">'+
                                                content +
    '                                    </div>'+
    '                                </div>'+
    '                            </div>'+                           
    '                            <div class="clear-line"><div class="mobile-footer"></div></div>'+
    '                        </div></div>'+
    '                    </div>'+
    '                    <div class="modal-footer"><button type="button" class="btn btn-primary" data-dismiss="modal">关闭</button>' +
    '                    </div>'+
    '                </div>'+
    '            </div>'+
    '        </div>';
    

    if (document.getElementById(divID)) { $("#" + divID).remove(); }
    $(document.body).append(s);
    $('#previewshow').modal('toggle').children().css({
        width: '400px'
    })

}
var $DialogFrame_ReturnValue = "";
function DialogFrame(url, title_tip, w_width, h_height, callBack,ShowBtn) {
    var tmpwidth = 900;
    var tmpheight = 450;
    $DialogFrame_ReturnValue = "";
    if (w_width != null) {
        tmpwidth = w_width;
    }
    if (h_height != null) {
        tmpheight = h_height;
    }
    var divID = "divmyIframeModal";//+ (new Date().getTime());
    var divIDBtn = "divmyIframeModalBtn";
    if (document.getElementById(divID)) { $("#" + divID).remove(); }

    var tips = '<div class="modal fade" id="' + divID + '"><div class="modal-dialog"><div class="modal-content"><div class="modal-header">' +
             '           <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
             '   <h4 class="modal-title"><strong>' + title_tip + '</strong></h4></div><div class="modal-body">' +
              '  <iframe src="" id="' + divID + 'Iframe" width="' + tmpwidth + '" height="' + tmpheight + '" scrolling="no"></iframe>';

    if (ShowBtn != null) {
        tips += '            <div class="modal-footer">' +
                '                <button type="button" class="btn btn-success btn-sm IframeModal_Btn" key="OK"  >确定</button>' +
                '                <button type="button" class="btn btn-sm  IframeModal_Btn"  key="close" >取消</button>' +
                '            </div>';
    }
    tips += '</div></div></div></div>';

    $(document.body).append(tips);

    $("#" + divID + "Iframe").attr("src", url);
    $('#' + divID).modal('toggle').children().css({
        width: (tmpwidth+50)+'px',
        height: tmpheight+'px'
    })
    $("#" + divID).modal({ show: true });

    if (ShowBtn != null) {
        $(".IframeModal_Btn").on('click', function () {
            if (callBack) {
                callBack($(this).attr("key"), $DialogFrame_ReturnValue); //atcion,value
                $("#" + divID).modal('hide');
            }
            else {
                $("#" + divID).modal('hide');
            }
        })
    }
    else {
        //关闭出发
        $("#" + divID).on('hide.bs.modal', function () {
            if (callBack) {
                callBack($DialogFrame_ReturnValue);
            }
        })
    }
    
}


function divmyIframeModalClose()
{
    $("#divmyIframeModal").modal('hide');
}

///////////////////////////////////////////////////////////////////////////////////
// 管理后台服务器提醒控件调用的错误或成功提醒方法，并且跳转到新页面
///////////////////////////////////////////////////////////////////////////////////
function ShowMsgAndReUrl(msg, success, url, target) {
    var type = success ? "success" : "error";
    //if (success) {
        HiTipsShow(msg, type, function () {
            switch (target) {
                case "parent":
                    parent.location.href = url;
                    break;
                case "top":
                    top.location.href = url;
                    break;
                default:
                    location.href = url;
                    break;
            }
        })
    //}
}

///////////////////////////////////////////////////////////////////////////////////
// 管理后台服务器提醒控件调用的错误或成功提醒方法
///////////////////////////////////////////////////////////////////////////////////
function ShowMsg(msg, success) {
    if (success) {
        HiTipsShow(msg, "success");
    }
    else { 
        HiTipsShow(msg, "error");
    }
}
///////////////////////////////////////////////////////////////////////////////////
// 错误或成功提醒方法
///////////////////////////////////////////////////////////////////////////////////
function HiTipsShow(msg, type, callBack,title) {
    switch(type)
    {
        case "success":
            var divID = "divtips" + (new Date().getTime());
            var tips = '<div style="z-index: 10000; top: 65px; left: 50%; position: fixed;display:none" id="' + divID + '"><div class="alert in fade alert-success" style="text-indent:20px;padding:15px;"><i style="display:block;width:18px;height:18px;background:url(/admin/images/true.gif) no-repeat;position:absolute;left:10px; top:18px;"></i>' + msg + '</div></div>';
            $(document.body).append(tips);
            var obj = $("#" + divID);
            var divLeft = ($("body").width() - obj.width()) / 2;
            obj.css("left", divLeft + "px")
            obj.slideDown("slow", function () { setTimeout(function () { obj.slideUp("slow", function () { if (callBack) { callBack()} obj.remove(); }) }, 1500) });
            //obj.slideDown("slow");
            break;
        case "fail":
        case "error":
            var divID = "divfailtips";//+ (new Date().getTime());
            if (document.getElementById(divID)) { $("#" + divID).remove(); }
            var tips = '<div class="modal fade in" id="' + divID + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false"><div class="modal-dialog"><div class="alert alert-danger" style="text-indent:20px;position: relative;padding: 15px;"><i style="display:block;width:18px;height:18px;background:url(/admin/images/false.gif) no-repeat;position:absolute;left:10px; top:15px;"></i>' + msg + '<a href="#" class="close" data-dismiss="modal" aria-hidden="true" style="position: absolute; top: 15px; right: 7px;">&times;</a></div></div></div>';
            $(document.body).append(tips);
            $('#' + divID).modal('show');

            $("#" + divID).on('hide.bs.modal', function () {
                $("body").removeAttr("style");
                if (callBack) {
                    callBack();
                }
            })
            break;
        case "warning":
            var divID = "divwarningtips";//+ (new Date().getTime());
            if (document.getElementById(divID)) { $("#" + divID).remove(); }
            var tips = '<div class="modal fade in" id="' + divID + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false"><div class="modal-dialog"><div class="alert alert-warning" style="text-indent:20px;position: relative;padding: 15px;"><i style="display:block;width:18px;height:18px;background:url(/admin/images/warning.gif) no-repeat;position:absolute;left:10px;"></i>' + msg + '<a href="#" class="close" data-dismiss="modal" aria-hidden="true" style="position: absolute; top: 2px; right: 7px;">&times;</a></div></div></div>';
            $(document.body).append(tips);
            //window.location = "#divwarningtips";
            $('#' + divID).modal('show');

            $("#" + divID).on('hide.bs.modal', function () {

                if (callBack) {
                    callBack();
                }
            })
            break;
        case "confirm":
            var objID = callBack;
            var divID = "divconfirmtips";//+ (new Date().getTime());
            if (document.getElementById(divID)) { $("#" + divID).remove(); }
            var tips = '<div class="modal fade mymodal" id="' + divID + '">' +
                '    <div class="modal-dialog">' +
                '        <div class="modal-content">' +
                '            <div class="modal-header">' +
                '                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                '            </div>' +
                '            <div class="modal-body">' +
                '                <div class="information clearfix">' +
                '                    <i class="glyphicon glyphicon-exclamation-sign fl"></i>' +
                '                    <div class="info">' + msg  +
                '                    </div>' +
                '                </div>' +
                '            </div>' +
                '            <div class="modal-footer">' +
                '                <button type="button" class="btn btn-success btn-sm"  data-dismiss="modal" onclick="isconfirmOK=true;$(\'#' + objID + '\').trigger(\'click\');isconfirmOK=false;">确定</button>' +
                '                <button type="button" class="btn btn-sm" data-dismiss="modal">取消</button>' +
                '            </div>' +
                '        </div>' +
                '    </div>' +
                '</div>';

            $(document.body).append(tips);
            $('#' + divID).modal('toggle').children().css({
                width: '500px'
            })
            break;
        case "confirmII":
            //回调函数型
            var divID = "divconfirmtips";//+ (new Date().getTime());
            var Stitle = "信息提醒";
            if (title!=null) {
                Stitle = title;
               
            };
            if (document.getElementById(divID)) { $("#" + divID).remove(); }
            var tips = '<div class="modal fade mymodal" id="' + divID + '">' +
                '    <div class="modal-dialog">' +
                '        <div class="modal-content">' +
                '            <div class="modal-header">' +
                '                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                '<h4 class="modal-title" style="text-align:left;margin-bottom:3px;line-height:14px" >' + Stitle + '</h4>' +
                '            </div>' +
                '            <div class="modal-body">' +
                '                <div class="information clearfix">' +
                '                    <i class="glyphicon glyphicon-exclamation-sign fl"></i>' +
                '                    <div class="info">' + msg +
                '                    </div>' +
                '                </div>' +
                '            </div>' +
                '            <div class="modal-footer">' +
                '                <button type="button" class="btn btn-success btn-sm" id="Confirm_' + divID + '" >确定</button>' +
                '                <button type="button" class="btn btn-sm" data-dismiss="modal">取消</button>' +
                '            </div>' +
                '        </div>' +
                '    </div>' +
                '</div>';

            $(document.body).append(tips);
            $('#' + divID).modal('toggle').children().css({
                width: '500px'
            });
            $("#Confirm_" + divID).click(function () {
                $('#' + divID).modal('toggle');
                if (callBack) {
                    callBack();
                }
            })
            break;
        default:
            break;
    }
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
function getParam(paramName) {
    paramValue = "";
    isFound = false;
    paramName = paramName.toLowerCase();
    var arrSource = this.location.search.substring(1, this.location.search.length).split("&");
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        if (paramName == "returnurl") {
            var retIndex = this.location.search.toLowerCase().indexOf('returnurl=');
            if (retIndex > -1) {
                var returnUrl = unescape(this.location.search.substring(retIndex + 10, this.location.search.length));
                if ((returnUrl.indexOf("http") != 0) && returnUrl != "" && returnUrl.indexOf(location.host.toLowerCase()) == 0) returnUrl = "http://" + returnUrl;
                return returnUrl;
            }
        }
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].toLowerCase().split(paramName + "=")[1];
                    paramValue = arrSource[i].substr(paramName.length + 1, paramValue.length);
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}

///////////////////////////////////////////////////////////////////////////////////
// String Helper
///////////////////////////////////////////////////////////////////////////////////
String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}
///////////////////////////////////////////////////////////////////////////////////
// URL Helper
///////////////////////////////////////////////////////////////////////////////////
function GetQueryString(key) {
    var url = location.href;
    if (url.indexOf("?") <= 0) {
        return "";
    }

    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {};

    for (i1 = 0; j = paraString[i1]; i1++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }

    var returnValue = paraObj[key.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

function GetQueryStringKeys() {
    var keys = {};
    var url = location.href;

    if (url.indexOf("?") <= 0) {
        return keys;
    }

    keys = url.substring(url.indexOf("?") + 1, url.length).split("&");
    for (i2 = 0; i2 < keys.length; i2++) {
        if (keys[i2].indexOf("=") >= 0) {
            keys[i2] = keys[i2].substring(0, keys[i2].indexOf("="));
        }
    }

    return keys;
}

function GetCurrentUrl() {
    var url = location.href;

    if (url.indexOf("?") >= 0) {
        return url.substring(0, url.indexOf("?"));
    }

    return url;
}

function AppendParameter(key, pvalue) {
    var reg = /^[0-9]*[1-9][0-9]*$/;
    var url = GetCurrentUrl() + "?";
    var keys = GetQueryStringKeys();

    if (keys.length > 0) {
        for (i3 = 0; i3 < keys.length; i3++) {
            if (keys[i3] != key) {
                url += keys[i3] + "=" + GetQueryString(keys[i3]) + "&";
            }
        }
    }

    if (!reg.test(pvalue)) {
        alert_h("只能输入正整数");
        return url.substring(0, url.length - 1);
    }

    url += key + "=" + pvalue;
    return url;
}

function AddDialog(divID, msg, callback)
{
    var tips = '<div class="modal fade mymodal" id="' + divID + '">' +
       '    <div class="modal-dialog">' +
       '        <div class="modal-content">' +
       '            <div class="modal-header">' +
       '                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
       '            </div>' +
       '            <div class="modal-body">' +
       '                <div class="information clearfix">' +
       '                    <i class="glyphicon glyphicon-exclamation-sign fl"></i>' +
       '                    <div class="info">' + msg +
       '                    </div>' +
       '                </div>' +
       '            </div>' +
       '            <div class="modal-footer">' +
       '                <button type="button" id="btn_' + divID + '" dataID="" class="btn btn-success btn-sm"  data-dismiss="modal" >确定</button>' +
       '                <button type="button" class="btn btn-sm" data-dismiss="modal">取消</button>' +
       '            </div>' +
       '        </div>' +
       '    </div>' +
       '</div>';
    $(tips).appendTo('body');

  
    //$(document.body).append(tips);
    //$('#' + divID).modal('toggle').children().css({
    //    width: '400px'
    //})
    $('#btn_' + divID).click(callback);
}


var isconfirmOK = false;
function HiConform(msg, obj) {
    var otype= $(obj).attr("type")
    if (otype == "submit" || otype == "button") {//undefined == obj
        if (!isconfirmOK) {
            HiTipsShow(msg, "confirm", obj.id);
        } else {
            isconfirmOK = false;
            return true;
        }
        return false;
    } else {
        return confirm(msg);
    }
}
//function ViewOrderDetail(orderid) {
//    var url = "OrderDetails.aspx?OrderId=" + orderid;
//    if (navigator.userAgent.indexOf("Chrome") > 0) {
//        var winOption = "height=600,width=1300,top=5,left=5,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,fullscreen=0";
//        return window.open(url, window, winOption);
//    } else {
//        window.showModalDialog(url, null, "dialogWidth=1800px;dialogHeight=1900px;resizable=yes;");
//    }
//}
