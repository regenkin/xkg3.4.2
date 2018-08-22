function drawg() {
    $('.drag-module-lists li').on('mousedown', function (event) {
        var _this = $(this);
        var $diy_contain = $("#diy-contain");
        var phoneAreaStartX = $('#diy-contain').offset().left;
        var phoneAreaStartT = $('#diy-contain').offset().top - 50;
        var objVar = {
            downX: event.pageX,//按下时鼠标X轴位置
            downY: event.pageY,//按下时鼠标Y轴位置
            elementX: event.pageX - _this.offset().left,//计算按下时鼠标离当前区块的X轴的距离
            elementY: event.pageY - (_this.offset().top - $(document).scrollTop()),//计算按下时鼠标离当前区块的Y轴的距离
            html: _this.html(),
            moveX: 0,//移动时的鼠标X位置
            moveY: 0,//移动时的鼠标Y位置
            phoneAreaStartX: phoneAreaStartX,//手机框的X轴开始位置
            phoneAreaEndX: phoneAreaStartX + $('#diy-contain').width(),//手机框的X轴结束位置
            phoneAreaStartT: phoneAreaStartT,
            phoneAreaEndT: phoneAreaStartT + $('#diy-contain').height() + 150,
            type: _this.attr('data-type'),
            flag: false,
            moduleArrT: []
        };
        $('.drag .diy-conitem').each(function (i) {
            var _this = $(this);
            var thisT = parseInt($(this).offset().top);
            if (i == $('.drag .diy-conitem').length - 1) {
                objVar.moduleArrT.push([
            {
                startT: thisT - 100,
                endT: thisT + _this.height() / 2
            },
            {
                startT: thisT + _this.height() / 2,
                endT: thisT + _this.height()+100
            }
                ]);
            } else {
                objVar.moduleArrT.push([
                {
                    startT: thisT - 100,
                    endT: thisT + _this.height() / 2
                },
                {
                    startT: thisT + _this.height() / 2,
                    endT: thisT + _this.height()
                }
                    ]);
            }
        })
        event.preventDefault();//阻止默认事件防止拖动的时候出现复制效果
        $('body').append("<div class='drag-module-lists addmodule' style='position:fixed;z-index:9999;left:" + (objVar.downX - objVar.elementX) + " !important;top:" + (objVar.downY - objVar.elementY) + " !important;cursor:move;'><ul><li>" + objVar.html + "</li></ul></div>");
        var scrollTopM = 0;
        var docScrollTop = $(document).scrollTop();
        $(window).scroll(function () {
            scrollTopM = $(document).scrollTop() - docScrollTop;
        })
        $(document).on('mousemove', function (event) {
            event.preventDefault();
            objVar.moveX = event.pageX;
            objVar.moveY = event.pageY;
            $('.addmodule').css({
                top: objVar.moveY - objVar.elementY - scrollTopM + 'px',
                left: objVar.moveX - objVar.elementX + 'px'
            });
            if (objVar.moveX >= objVar.phoneAreaStartX && objVar.moveX <= objVar.phoneAreaEndX) {
                if (objVar.moveY >= objVar.phoneAreaStartT && objVar.moveY <= objVar.phoneAreaEndT) {
                    if ($('.drag .diy-conitem').length > 0) {
                        $.each(objVar.moduleArrT, function (i) {
                            $.each(this, function (j) {
                                if (j == 0) {
                                    if (objVar.moveY >= this.startT && objVar.moveY <= this.endT) {
                                        if (!$('.drag .diy-conitem').eq(i).prev().hasClass('drag-highlight')) {
                                            $('.drag-highlight').remove();
                                            $('.drag .diy-conitem').eq(i).before('<div class="drag-highlight"></div>');
                                            return false;
                                        }
                                    }
                                } else {
                                    if (objVar.moveY >= this.startT && objVar.moveY <= this.endT) {
                                        if (!$('.drag .diy-conitem').eq(i).next().hasClass('drag-highlight')) {
                                            $('.drag-highlight').remove();
                                            $('.drag .diy-conitem').eq(i).after('<div class="drag-highlight"></div>');
                                            return false;
                                        }
                                    }
                                }
                            })
                        })
                    } else {
                        if (!$('.nodrag').eq(0).next().hasClass('drag-highlight')) {
                            $('.nodrag').eq(0).after('<div class="drag-highlight"></div>');
                        }
                    }
                }
            }
        })
        $(document).on('mouseup', function () {
            $('.addmodule').remove();
            var $actionPanel = null;
            var data = null;
            if (!objVar.flag) {
                if (objVar.moveX >= objVar.phoneAreaStartX && objVar.moveX <= objVar.phoneAreaEndX) {
                    if (objVar.moveY >= objVar.phoneAreaStartT && objVar.moveY <= objVar.phoneAreaEndT) {
                        //alert(objVar.type)
                        //addMoulde(parseInt(objVar.type));

                        data = dataInit(parseInt(objVar.type));
                        //添加手机内容
                        if (data.type != 5) {
                            $actionPanel = insertDom(data);
                        } else {
                            HiShop.Goods.GoodsList(data);
                        }
                        objVar.flag = true;
                    }
                } else {
                    $('.drag-highlight').remove();
                }
            }

            $(document).off(),
            $(document).on("mouseover", ".droplist .j-droplist-toggle",
               function () {

                   $(this).siblings(".droplist-menu").show()
               }),
               $(document).on("mouseleave", ".droplist .droplist-menu",
               function () {
                   $(this).hide()
               }),
               $(document).on("mouseleave", ".droplist",
               function () {
                   $(this).find(".droplist-menu").hide()
               }),
               $(document).on("click", ".droplist .droplist-menu a",
               function () {
                   $(this).parents(".droplist-menu").hide()
               })

        })
    });
}


function addMoulde(type) {
    if (type != 5) {

        //添加模块
        HiShop.DIY.add(dataInit(type), true);
    } else {

        var moduleData = dataInit(type);
        HiShop.Goods.GoodsList(moduleData);
    }

}
function dataInit(type) {
    //默认数据
    var moduleData = {
        id: HiShop.DIY.getTimestamp(), //模块ID
        type: type, //模块类型
        draggable: true, //是否可拖动
        sort: 0, //排序
        content: null //模块内容
    };

    //根据模块类型设置默认值
    switch (type) {
        //富文本
        case 1:
            moduleData.ue = null;
            moduleData.content = {
                fulltext: ""
            };
            break;
            //标题
        case 2:
            moduleData.content = {
                title: "标题名称",
                subtitle: "『副标题』",
                direction: "left",
                space: 0,
                pic: "",
                bgcorlor: "#fff",
                ftcorlor: "#707070"
            };
            break;
            //自定义模块
        case 3: break;
            //商品
        case 4:
            moduleData.content = {
                layout: 1,
                showPrice: true,
                showIco: true,
                showName: 1,
                goodslist: []
            }
            break;
            //商品列表（分组标签）
        case 5:
            moduleData.content = {
                layout: 1,
                showPrice: true,
                showIco: true,
                showName: true,
                group: null,
                firstPriority: 2,
                secondPriority: 3,
                goodsize: 6,
                showMaketPrice: true,
                goodslist: [
                   {
                       item_id: "1",
                       link: "#",
                       pic: "/Admin/shop/Public/images/diy/goodsView1.jpg",
                       price: "188.00",
                       original_price: "188.00",
                       title: "第一个商品"
                   },
                   {
                       item_id: "2",
                       link: "#",
                       pic: "/Admin/shop/Public/images/diy/goodsView2.jpg",
                       price: "288.00",
                       original_price: "288.00",
                       title: "第二个商品"
                   },
                   {
                       item_id: "3",
                       link: "#",
                       pic: "/Admin/shop/Public/images/diy/goodsView3.jpg",
                       price: "388.00",
                       original_price: "388.00",
                       title: "第三个商品"
                   },
                   {
                       item_id: "4",
                       link: "#",
                       pic: "/Admin/shop/Public/images/diy/goodsView4.jpg",
                       price: "488.00",
                       original_price: "488.00",
                       title: "第四个商品"
                   }]
            }
            break;
            //搜索
        case 6: break;
            //文本导航
        case 7:
            moduleData.content = {
                dataset: [
					{
					    linkType: 6,
					    link: "/default.aspx",
					    title: "店铺主页",
					    showtitle: "店铺主页"
					}
                ]
            }
            break;
            //图片导航
        case 8:
            moduleData.content = {
                dataset: [
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    showtitle: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					},
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    showtitle: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					},
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    showtitle: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					},
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    showtitle: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					}
                ]
            }
            break;
            //广告图片
        case 9:
            moduleData.content = {
                showType: 1,
                space: 0,
                margin: 10,
                dataset: []
            }
            break;
            //分割线
        case 10:
            moduleData.content = {
                styleClass: "custom-line",
                style: 0,//0：虚线 1：实线
                corlor: "#DDD"
            }
            break;
            //辅助空白
        case 11:
            moduleData.content = {
                height: 10
            }
            break;
            // 顶部菜单
        case 12:
            moduleData.content = {
                style: '0',
                marginstyle: '0',
                dataset: [
					{
					    link: "/Default.aspx",
					    linkType: 6,
					    showtitle: "首页",
					    title: "店铺主页",
					    pic: "/Admin/shop/PublicMob/images/ind3_1.png",
					    bgColor: "#07a0e7",
					    cloPicker: '0',
					    fotColor: '#fff'
					},
					{
					    link: "/ProductList.aspx",/*/Admin/shop/Default.aspx 该链接错误，kuaiwei修改*/
					    linkType: 6,
					    showtitle: "新品",
					    title: "",
					    pic: "/Admin/shop/PublicMob/images/ind3_2.png",
					    bgColor: "#72c201",
					    cloPicker: '1',
					    fotColor: '#fff'
					},
					{
					    link: "/Default.aspx",
					    linkType: 6,
					    showtitle: "热卖",
					    title: "店铺主页",
					    pic: "/Admin/shop/PublicMob/images/ind3_3.png",
					    bgColor: "#ffa800",
					    cloPicker: '2',
					    fotColor: '#fff'
					},
					{
					    link: "/Default.aspx",
					    linkType: 6,
					    showtitle: "推荐",
					    title: "",
					    pic: "/Admin/shop/PublicMob/images/ind3_4.png",
					    bgColor: "#d50303",
					    cloPicker: '3',
					    fotColor: '#fff'
					}
                ]
            }
            break;
            // 顶部菜单
        case 13:
            moduleData.content = {
                layout: '1',
                dataset: [
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					},
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					},
					{
					    linkType: 0,
					    link: "#",
					    title: "导航名称",
					    pic: "/Admin/shop/Public/images/diy/waitupload.png"
					}
                ]
            }
            break;
            // 视频
        case 14:
            moduleData.content = {
                website: ''
            }
            break;
            // 音频
        case 15:
            moduleData.content = {
                direct: 0,
                imgsrc: '',
                audiosrc: ''
            }
            break;
            // 公告
        case 16:
            moduleData.content = {
                linkType: 0,
                title: "公告",
                showtitle: "请填写内容，如果过长，将会滚动显示"
            }
            break;
    }
    return moduleData;
}

function events() {
    //初始化布局拖动事件
    $("#diy-phone .drag").sortable({
        revert: true,
        placeholder: "drag-highlight",
        stop: function (event, ui) {
            //alert(ui.item);
            HiShop.DIY.repositionCtrl(ui.item, $(".diy-ctrl-item[data-origin='item']")); //重置ctrl的位置
        }
    }).disableSelection();

    //编辑页面标题
    $(".j-pagetitle").click(function () {
        $(".diy-ctrl-item[data-origin='pagetitle']").show().siblings(".diy-ctrl-item[data-origin='item']").hide();
    });

    //编辑页面标题同步到手机视图中
    $(".j-pagetitle-ipt").change(function () {
        $(".j-pagetitle").text($(this).val());
    });
    $("#diy-phone .drag .j-Up").show();
    $("#diy-phone .drag .j-Down").show();
    $("#diy-phone .drag .j-Up").eq(0).hide();
    $("#diy-phone .drag .j-Down").eq($("#diy-phone .drag .j-Down").length - 1).hide();
}

function insertDom(data) {
    var $diy_contain = $("#diy-contain");
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
        HiShop.DIY.edit(data, true);
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
            //上移过程中  如果是从第一位移动到第二位  那么  第二位将变为第一位  故把第二位上移隐藏 第一位上移显示
            HiShop.DIY.Unit.UpOrDownShow();
        });
        $btn_Down.click(function () {

            $(this).parent().parent().parent().insertAfter($(this).parent().parent().parent().next());
            //下移过程中  如果是从第一位移动到第二位  那么  第二位将变为第一位  故把第二位上移隐藏 第一位上移显示
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


    
    //$('.drag .diy-conitem:last').hide();
    if ($('.drag .diy-conitem').length != 1) {
        $('.drag .diy-conitem').each(function (i) {
            if ($(this).prev().hasClass('drag-highlight')) {

                $(this).prev().before($render_conitem);
                //$('.drag .diy-conitem').show();
                return false;
            }
            if ($(this).next().hasClass('drag-highlight')) {
                $(this).next().after($render_conitem);
                //$('.drag .diy-conitem').show();
                return false;
            }
        });
    }
    //绑定事件
    events();
    $('.app_inner_content').css('minHeight', $('.diy-phone-outbox').height(), 0);
    $('.drag-highlight').remove();
    //是否添加完后立即显示控制内容
    if ($actionPanel != null) {
        $actionPanel.click(); //触发一次编辑事件
    }
    //根据是否可拖动插入不同的缓存数组
    if (data != null) {
        if (data.draggable) {
            HiShop.DIY.LModules.push(data);
        } else {
            HiShop.DIY.PModules.push(data);
        }
    }
    $(document).off(),
    $(document).on("mouseover", ".droplist .j-droplist-toggle",
        function () {

            $(this).siblings(".droplist-menu").show()
        }),
        $(document).on("mouseleave", ".droplist .droplist-menu",
        function () {
            $(this).hide()
        }),
        $(document).on("mouseleave", ".droplist",
        function () {
            $(this).find(".droplist-menu").hide()
        }),
        $(document).on("click", ".droplist .droplist-menu a",
        function () {
            $(this).parents(".droplist-menu").hide()
        })
    return $actionPanel;
}