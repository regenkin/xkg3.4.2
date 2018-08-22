(function ($) {


    $.prototype.data = {
        text: null,
        value: null
    }

    $.fn.vshopSelector = function (params) {
        /// <param name="data" type="object Array">
        /// 每个数组结构为包含如下属性或方法:
        /// text:需要显示的文本
        /// value:文本对应的值，如果该属性为空，则默认使用text
        /// type: 选项类型："text"文本，"divider":分隔条
        /// href:选项的链接地址
        /// </param>
        /// <param name="onchanged" type="function">参数eg:{text:'abc',value:1}</param>
        /// <param name="onload" type="function">Description</param>

        var thisObj = $(this);

        if (params && params.data) {
            return this.each(function () {
                var render = function (callBack) {
                    var htmlText = '<div id="vshopSelector_' + thisObj.attr('Id') + '" class="btn-group" >\
            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">\
            <label class="vshopSelector_text"></label>\
            <span class="caret"></span>\
            </button>\
            <ul class="dropdown-menu" role="menu">\
            </ul>\
            </div>';
                    var container = thisObj;
                    container.html(htmlText);
                    if (params.height) {
                        container.css('height', params.height);
                        container.find('button').css('height', params.height);
                    }
                    if (params.width) {
                        container.css('width', params.width);
                        container.find('button').css('width', params.width);
                    }

                    if (params.data) {
                        //填充数据
                        var ul = $(container.find('ul')[0]);
                        var curValue = '', curText = '', selected = null, href = null;
                        $.each(params.data, function (i, item) {
                            if (!item.type || item.type == 'text') {
                                href = item.href;
                                if (item.value == null)
                                    item.value = item.text;
                                curValue = item.value;
                                curText = item.text;
                                if (item.selected)
                                    selected = item;
                                href = item.href ? item.href : '#';

                                ul.append('<li><a href="' + href + '" value="' + curValue + '">' + curText + '</a></li>');
                            }
                            else if (item.type == 'divider') {
                                ul.append('<li class="divider"></li>');
                            }
                        });

                        //设置当前选中
                        if (selected) {
                            this.data.text = selected.text;
                            this.data.value = selected.value;
                        }
                        else
                            this.data.text = params.defaultText ? params.defaultText : '请选择';
                        $(container.find('.vshopSelector_text')).html(this.data.text);
                        params.onchanged && params.onchanged({ text: this.data.text, value: this.data.value }); //触发改变事件

                        $('#' + thisObj.attr('id') + ' li a').click(function () {
                            //写入更改的值
                            text = $(this).html();
                            $('#' + thisObj.attr('id') + ' .vshopSelector_text').html(text);
                            value = $(this).attr('value');

                            //触发改变事件
                            setTimeout(function () {
                                params.onchanged && params.onchanged({ text: text, value: value });
                            }, 100);


                        });
                    }
                    if (callBack) {
                        setTimeout(function () {
                            callBack();
                        }, 100);
                    }
                    return this;
                }
                render(function () {
                    params.onload && params.onload();
                });
            });
        }
    }

    $.fn.getValue = function () {

        return value;

    }

})(jQuery);