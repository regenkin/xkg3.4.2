/*
*作者:周祥
*时间:2014年08月19日
*介绍:图片上传预览插件 兼容浏览器(IE 谷歌 火狐) 不支持safari 当然如果是使用这些内核的浏览器都兼容
*网站:http://jquery.decadework.com http://www.oschina.net/code/snippet_1049351_26784
*QQ:200592114
*版本:1.2
*参数说明: Img:图片ID;Width:预览宽度;Height:预览高度;ImgType:支持文件类型;Callback:选择文件后回调方法;
*插件说明: 基于JQUERY扩展,需要引用JQUERY库。
*使用方法:  <div>
<img id="ImgPr" width="120" height="120" /></div>
<input type="file" id="up" />
  
把需要进行预览的IMG标签外 套一个DIV 然后给上传控件ID给予uploadPreview事件
  
$("#up").uploadPreview({ Img: "ImgPr", Width: 120, Height: 120, ImgType: ["gif", "jpeg", "jpg", "bmp", "png"], Callback: function () { }});   
  
另外注意一下 使用该插件预览图片 选择文件的按钮在IE下不能是隐藏的 你可以换种方式隐藏 比如:top left 负几千像素
  
v1.2:更新jquery1.9以上版本 插件报错BUG 
*/
jQuery.fn.extend({
    uploadPreview: function (opts) {
        var _self = this, _this = $(this);
        opts = jQuery.extend({
            Img: "ImgPr",
            ImgUrl:"",
            Width: 100,
            Height: 100,
            ImgType: ["gif", "jpeg", "jpg", "bmp", "png"],
            Callback: function () { }
        }, opts || {});
        _self.getObjectURL = function (file) {
            var url = null;
            if (window.createObjectURL != undefined) {
                url = window.createObjectURL(file);
            } else if (window.URL != undefined) {
                url = window.URL.createObjectURL(file);
            } else if (window.webkitURL != undefined) {
                url = window.webkitURL.createObjectURL(file);
            }
           
            return url;
        }
        _this.change(function () {
            if (this.value) {
                if (!RegExp("\.(" + opts.ImgType.join("|") + ")$", "i").test(this.value.toLowerCase())) {
                    alert("选择文件错误,图片类型必须是" + opts.ImgType.join("，") + "中的一种");
                    this.value = "";
                    return false;
                }
                if (navigator.userAgent.indexOf("MSIE") > -1) {
                    try {
                        opts.ImgUrl = _self.getObjectURL(this.files[0]);
                        $("#" + opts.Img).attr('src', opts.ImgUrl);
                    } catch (e) {
                       
                        var src = "";
                        var obj = $("#" + opts.Img);
                        var div = obj.parent("div")[0];
                        _self.select();
                        if (top != self) {
                            window.parent.document.body.focus();
                        } else {
                            _self.blur();
                        }

                        src = document.selection.createRange().text;
                        document.selection.empty();
                        obj.hide();
                        obj.parent("div").css({
                            'filter': 'progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)',
                            'width': opts.Width + 'px',
                            'height': opts.Height + 'px'
                        });
                        div.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = src;
                        opts.ImgUrl = "IE^"+src;
                    }
                } else {
                    opts.ImgUrl=_self.getObjectURL(this.files[0])
                    $("#" + opts.Img).attr('src', opts.ImgUrl);
                }
                opts.Callback(opts.ImgUrl);
            }
        });
    }
});