$(function () {
    HiShop.DIY = HiShop.DIY ? HiShop.DIY : {}; //DIY 命名空间
    HiShop.DIY.Unit = HiShop.DIY.Unit ? HiShop.DIY.Unit : {}; //unit
    HiShop.DIY.PModules = HiShop.DIY.PModules ? HiShop.DIY.PModules : []; //页面模块
    HiShop.DIY.LModules = HiShop.DIY.LModules ? HiShop.DIY.LModules : []; //自定义模块列表

    var $diy_contain = $("#diy-contain"), //Diy 内容显示区域
        $diy_ctrl = $("#diy-ctrl"); //Diy 控制器显示区域

    //DIY常量
    HiShop.DIY.constant = {
        diyoffset: $(".diy").offset()
    };

    //获取当前时间戳
    HiShop.DIY.getTimestamp = function () {
        var date = new Date();
        return "" + date.getFullYear() + parseInt(date.getMonth() + 1) + date.getDate() + date.getHours() + date.getMinutes() + date.getSeconds() + date.getMilliseconds();
    };

    /*
     * 添加模块
     * @param type 模块类型
     * @param data 模块数据
     * @param showctrl 是否添加完后立即显示控制内容
     */
    HiShop.DIY.add = function (data, showctrl) {
        //添加手机内容
        var html_con = _.template($("#tpl_diy_con_type" + data.type).html(), data), //内容
            html_conitem = _.template($("#tpl_diy_conitem").html(), {
                html: html_con
            }), //diy通用外层容器
            $render_conitem = $(html_conitem); //渲染模板

        data.dom_conitem = $render_conitem; //缓存手机内容dom

        var $actionPanel = $render_conitem.find(".diy-conitem-action"),
            $btn_edit = $actionPanel.find(".j-edit"),
            $btn_del = $actionPanel.find(".j-del"),
            $btn_Up = $actionPanel.find(".j-Up"),
            $btn_Down = $actionPanel.find(".j-Down");

        //绑定编辑模块事件
        $actionPanel.click(function () {
            $(".diy-conitem-action").removeClass("selected");
            $(this).addClass("selected");
            HiShop.DIY.edit(data,true);
        });

        //是否插入可拖拽区域
        var dragPanel = "";

        //只有可拖拽的模块才可以被删除
        if (data.draggable) {
            //绑定删除模块事件
            $btn_del.click(function () {
                HiShop.DIY.del(data);
                return false;
            });
          
            $btn_Up.click(function () {
                $(this).parent().parent().parent().insertBefore($(this).parent().parent().parent().prev());
                
                HiShop.DIY.Unit.UpOrDownShow();
            });
            $btn_Down.click(function () {
              
                $(this).parent().parent().parent().insertAfter($(this).parent().parent().parent().next());
                HiShop.DIY.Unit.UpOrDownShow();          
            });
            dragPanel = ".drag";
        } else {
            $btn_Down.remove();
            $btn_Up.remove();
            $btn_del.remove();
            dragPanel = ".nodrag";
        }

        $diy_contain.find(dragPanel).append($render_conitem); //插入文档

        //是否添加完后立即显示控制内容
        showctrl = showctrl ? showctrl : false;

        if (showctrl) {
            ////$actionPanel.click(); //触发一次编辑事件
            //$(".diy-conitem-action").removeClass("selected");
            //////$(this).addClass("selected");
            //$render_conitem.find(".diy-conitem-action").addClass("selected");
            //HiShop.DIY.edit(data, true);
        }

        //根据是否可拖动插入不同的缓存数组
        if (data.draggable) {
            HiShop.DIY.LModules.push(data);
        } else {
            HiShop.DIY.PModules.push(data);
        }

        return false;
    };

    /*
     * 编辑模块
     * @param data 模块数据
     */
    HiShop.DIY.edit = function (data,move) {
        //移除之前的模块控制内容
        $diy_ctrl.find(".diy-ctrl-item[data-origin='item']").remove();

        //渲染模板
        var html_ctrl_panel = $("#tpl_diy_ctrl").html(),
            html_ctrl_con = _.template($("#tpl_diy_ctrl_type" + data.type).html(), data),
            html_ctrl = _.template(html_ctrl_panel, { html: html_ctrl_con }),
            $render_ctrl = $(html_ctrl);

        $diy_ctrl.append($render_ctrl); //插入dom
        if (move) {
            HiShop.DIY.repositionCtrl(data.dom_conitem, $render_ctrl); //设置控制内容的位置
        }
        HiShop.DIY.bindEvents($render_ctrl, data); //绑定各种事件

        $render_ctrl.show().siblings(".diy-ctrl-item").hide(); //显示控制内容，并隐藏其它

        return false;
    };

    /*
     * 重设控制内容的位置
     * @param conitem 手机视图dom对象
     * @param ctrl 控制内容dom对象
     */
    HiShop.DIY.repositionCtrl = function (conitem, ctrl) {
        var top_conitem = conitem.offset().top,
            curPosTop = top_conitem - HiShop.DIY.constant.diyoffset.top;

        ctrl.css("marginTop", curPosTop);//设置位置

        $("html,body").animate({ scrollTop: curPosTop }, 300);//滚动页面
    };
    /*
     * 重设控制内容的位置
     * @param conitem 手机视图dom对象
     * @param ctrl 控制内容dom对象
     */
    HiShop.DIY.repositionCtrlByAdd = function (conitem, ctrl,topOrBom) {
        var top_conitem = conitem.offset().top;
        var curPosTop = top_conitem - HiShop.DIY.constant.diyoffset.top;
        if (topOrBom==1) {
            curPosTop = curPosTop + $(".drag-highlight").height();
        } else if (topOrBom == 2) {
            curPosTop = curPosTop - $(".drag-highlight").height();
        } else {

        }
       
        ctrl.css("marginTop", curPosTop);//设置位置

        $("html,body").animate({ scrollTop: curPosTop }, 300);//滚动页面
    };
    /*
     * 删除模块
     * @param data 模块数据
     */
    HiShop.DIY.del = function (data) {
        if (!data) return;
        //提示删除
        $.jBox.show({
            title: "提示",
            content: _.template($("#tpl_jbox_simple").html(), {
                content: "删除后将不可恢复，是否继续？"
            }),
           // content: null,
            btnOK: {
                onBtnClick: function (jbox) {
                    $.jBox.close(jbox);

                    //从缓存数组中删除
                    var lists = HiShop.DIY.LModules,
                        lists_len = HiShop.DIY.LModules.length;

                    for (var i = 0; i < lists_len; i++) {
                        if (lists[i].id == data.id) {
                            lists.splice(i, 1);
                            break;
                        }
                    }
                    //从文档中删除
                    data.dom_conitem.remove();
                    $diy_ctrl.find(".diy-ctrl-item[data-origin='item']").remove();
                }
            }
        });
        return false;
    };

    /*
     * 绑定ctrl事件
     * @param ctrldom 空中内容dom
     * @param data 模块数据
     */
    HiShop.DIY.bindEvents = function (ctrldom, data) {

        if (data.type == 6 ) return;
        HiShop.DIY.Unit["event_type" + data.type](ctrldom, data);
    };

    /*
     * 重新计算装修模块的排序
     */
    HiShop.DIY.reCalcPModulesSort = function () {
        _.each(HiShop.DIY.LModules, function (module, index) {
            module.sort = module.dom_conitem.index();
        });
    };

    /*
     * 获取装修数据
     */
    HiShop.DIY.Unit.getData = function () {
        HiShop.DIY.reCalcPModulesSort(); //重新计算模块的排序

        //数据格式
        var data = {
            page: {}, //页面信息
            PModules: {}, //页面模块
            LModules: {} //装修模块
        };

        data.page.title = $(".j-pagetitle-ipt").val(); //获取页面标题数据
        data.page.subtitle = $(".j-pagesubtitle-ipt").val(); //获取页面标题数据
        data.page.view_pic = $(".j-view_pic-ipt").prop('src');
        data.page.praise_num = $(".j-pagepraisenum").val();
        data.PModules = HiShop.DIY.PModules; //获取页面模块数据
       


        data.page.goto_time = $('.j-gototime-ipt').val(); // 获取时间
        //缓存排序后的自定义模块数组
        var newsortarr = [];

        //重排序
        for (var i = 0; i < HiShop.DIY.LModules.length; i++) {
            var tmp = HiShop.DIY.LModules[i];
            if (tmp != '') {
                newsortarr[tmp.sort] = tmp;
            }
        }

        data.LModules = newsortarr;

        var tmp = $.extend(true, {}, data);

        _.each(tmp.LModules, function (item) {
            if (item.type == 5) {
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(HiShop.Config.HiTempLatePath.GroupGoodTemp + item.content.layout + HiShop.Config.HiTempLatePath.TemplateExt));
            }
            else if (item.type == 9) {
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template($("#tpl_diy_con_type" + item.type + "Phone").html(), item)));
            } else if (item.type == 14) {//视频特殊处理
                item.content.website = HiShop.DIY.Unit.verifyVideoSrc(item.content.website);
                if (item.content.website == false) {
                    tmp = false;
                    return;
                }
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template($("#tpl_diy_con_type" + item.type + "Phone").html(), item)));
            }
            else {
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template($("#tpl_diy_con_type" + item.type).html(), item)));
            }



            item.dom_ctrl = null;
            item.ue = null;
        });

        _.each(tmp.PModules, function (item) {
            var head = "";
            if (item.type == 9) {
                head = $("#tpl_diy_con_type" + item.type + "Phone").html();
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template(head, item)));
            } else if (item.type == "Header_style11" || item.type == "Header_style14") {
                head = $("#tpl_diy_con_type" + item.type + "Phone").html();
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template(head, item) + HiShop.DIY.Unit.HStyleAddJs(item.content.bg)));
            } else {
                head = $("#tpl_diy_con_type" + item.type).html();
                item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template(head, item)));
            }
            //item.dom_conitem = HiShop.Base64.encode(HiShop.Base64.utf16to8(_.template(head, item) + HiShop.DIY.Unit.HStyleAddJs(item.content.bg)));


            item.dom_ctrl = null;
            item.ue = null;
        });

        return tmp;
    }
    HiShop.DIY.Unit.verifyVideoSrc = function (website) {
        var idstr_reg = /vid\=([^\&]*)($|\&)+/g,
            idstr_reg2 = /sid\/\w*.*?/g;
        var videoSrc, qqvideoID, youkuvideoID;
        qqvideoID = website.match(idstr_reg);
        youkuvideoID = website.match(idstr_reg2);
        if (qqvideoID) {
            qqvideoID = qqvideoID.toString()
            videoSrc = 'http://v.qq.com/iframe/player.html?' + qqvideoID + '&tiny=0&auto=0';
        };
        if (youkuvideoID) {
            youkuvideoID = youkuvideoID.toString();
            youkuvideoID = youkuvideoID.split('/v.swf');
            youkuvideoID = youkuvideoID.toString();
            youkuvideoID = youkuvideoID.replace('sid/', '').replace(',', '');
            videoSrc = 'http://player.youku.com/embed/' + youkuvideoID;
        };
        if (qqvideoID === null && youkuvideoID === null) {
            HiShop.hint("danger", "请填写正确的视频网址");
            return false;
        };
        return videoSrc;
    };

    HiShop.DIY.Unit.HStyleAddJs = function (bg)
    {
        html = "  <script src=\"http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js\" type=\"text/javascript\"></script>\n\
                  \<script type=\"text/javascript\">\n\
                    $(function () {\n\
                        $('body,html').css({\n\
                            'height':'100%'\n\
                        })\n\
                        $('#divCommon').css({\n\
                            'height':'100%',\n\
                            'max-width':'640px',\n\
                            'background':'url(" + bg + ") no-repeat',\n\
                            'backgroundSize':'cover'\n\
                        })\n\
                    })\n\
                </script>";
        return html;
    }


    HiShop.DIY.Unit.html_encode = function (str) {
        var s = "";

        if (str.length == 0) return "";
        s = str.replace(/&/g, "&amp;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/ /g, "&nbsp;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");

        return s;
    }




    HiShop.DIY.Unit.html_decode = function (str) {
        var s = "";

        if (str.length == 0) return "";
        s = str.replace(/&amp;/g, "&");
        s = s.replace(/&lt;/g, "<");
        s = s.replace(/&gt;/g, ">");
        s = s.replace(/&nbsp;/g, " ");
        s = s.replace(/&#39;/g, "\'");
        s = s.replace(/&quot;/g, "\"");

        return s;
    }
 
    HiShop.DIY.Unit.UpOrDownShow = function () {
        //alert($(".drag .diy-conitem-action-btns").length);
        if ($(".drag .diy-conitem-action-btns").length < 2) {
            $(".drag .j-Down").hide();
            $(".drag .j-Up").hide();
        } else if ($(".drag .diy-conitem-action-btns").length == 2) {
            $(".drag .j-Down").eq(0).show();
            $(".drag .j-Up").eq(0).hide();
            $(".drag .j-Down").eq(1).hide();
            $(".drag .j-Up").eq(1).show();
        } else {
            $(".drag .j-Down").show();
            $(".drag .j-Up").show();
            $(".drag .j-Down").eq(0).show();
            $(".drag .j-Up").eq(0).hide();
            $(".drag .j-Down").eq($(".drag .diy-conitem-action-btns").length - 1).hide();
            $(".drag .j-Up").eq($(".drag .diy-conitem-action-btns").length - 1).show();
        }
        
    }
});