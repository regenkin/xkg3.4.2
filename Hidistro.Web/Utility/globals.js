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
// String Helper
///////////////////////////////////////////////////////////////////////////////////
String.format = function() {
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


///////////////////////////////////////////////////////////////////////////////////
// DataList Select Helper
///////////////////////////////////////////////////////////////////////////////////
function SelectAll() {

    var checkbox = document.getElementsByName("CheckBoxGroup");
    
    if (checkbox == null) {
        return false;
    }
    if (typeof checkbox.length != 'undefined') {
        if (checkbox.length > 0) {
            for (var i = 0; i < checkbox.length; i++) {
                checkbox[i].checked = true;
            }

        }
    }

    else {
        checkbox.checked = true;
    }


    return false;
}

function ReverseSelect() {

    var checkbox = document.getElementsByName("CheckBoxGroup");
    
    if (checkbox == null) {
        return false;
    }
    if (typeof checkbox.length != 'undefined') {
        if (checkbox.length > 0) {
            for (var i = 0; i < checkbox.length; i++) {
                checkbox[i].checked = !checkbox[i].checked;
            }

        }
    }

    else {
        checkbox.checked = !checkbox.checked;
    }
    return false;

}


//计算坐标方法,得到某obj的x,y坐标,兼容浏览器
function getWinElementPos(obj)
{
 var ua = navigator.userAgent.toLowerCase();
 var isOpera = (ua.indexOf('opera') != -1);
 var isIE = (ua.indexOf('msie') != -1 && !isOpera); // not opera spoof
 var el = obj;
 if(el.parentNode === null || el.style.display == 'none') 
 {
  return false;
 }
 var parent = null;
 var pos = [];
 var box;
 if(el.getBoundingClientRect) //IE
 {
  box = el.getBoundingClientRect();
  var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
  var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
  return {x:box.left + scrollLeft, y:box.top + scrollTop};
 }
 else if(document.getBoxObjectFor) 
 {
  box = document.getBoxObjectFor(el);
  var borderLeft = (el.style.borderLeftWidth)?parseInt(el.style.borderLeftWidth):0;
  var borderTop = (el.style.borderTopWidth)?parseInt(el.style.borderTopWidth):0;
  pos = [box.x - borderLeft, box.y - borderTop];
 }
 else // safari & opera
 {
  pos = [el.offsetLeft, el.offsetTop];
  parent = el.offsetParent;
  if (parent != el) {
   while (parent) {
    pos[0] += parent.offsetLeft;
    pos[1] += parent.offsetTop;
    parent = parent.offsetParent;
   }
  }
  if (ua.indexOf('opera') != -1 
   || ( ua.indexOf('safari') != -1 && el.style.position == 'absolute' )) 
  {
    pos[0] -= document.body.offsetLeft;
    pos[1] -= document.body.offsetTop;
  } 
 }
 if (el.parentNode) { parent = el.parentNode; }
 else { parent = null; }
 while (parent && parent.tagName != 'BODY' && parent.tagName != 'HTML') 
 { // account for any scrolled ancestors
  pos[0] -= parent.scrollLeft;
  pos[1] -= parent.scrollTop;
  
  if (parent.parentNode) { parent = parent.parentNode; } 
  else { parent = null; }
 }
 return {x:pos[0], y:pos[1]};
}

window.onerror=function(){return true;}