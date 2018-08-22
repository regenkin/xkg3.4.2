$(function () {
    Defaults = {
        12: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 12,
                type: "Header_style12",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/PublicMob/images/indexbg/12.jpg",
                    photo: "/PublicMob/images/index12-2.jpg"
                }
            }],
            LModules: [{
                id: "10",
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: []
                }
            },
            {
                id: 2,
                type: "Header_style12_nav",
                draggable: !0,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "酒店预订",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index12-4.png",
                        bgColor: "#01afda",
                        fotColor:"#fff"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "机票预订",
                        title: "",
                        pic: "/PublicMob/images/index12-5.png",
                        bgColor: "#ffde00",
                        fotColor: "#fff"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "订餐",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index12-6.png",
                        bgColor: "#fcb05f",
                        fotColor: "#fff"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "出租车",
                        title: "",
                        pic: "/PublicMob/images/index12-7.png",
                        bgColor: "#a4cf3e",
                        fotColor: "#fff"
                    }]
                }
            }]
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style12 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style12").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style12").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style12(d, b)
        };
        d.find(".j-modify-bg").click(function () {
            return HiShop.popbox.ImgPicker(function (a) {
                b.content.bg = a[0],
                g()
            }),
            !1
        }),
        d.find(".j-modify-photo").click(function () {
            return HiShop.popbox.ImgPicker(function (a) {
                b.content.photo = a[0],
                g()
            }),
            !1
        })
    },
    HiShop.DIY.Unit.event_typeHeader_style12_nav = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style12_nav").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style12_nav").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".mobile12_content").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style12_nav(d, b)
        };
        d.find(".j-moveup").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            if (0 != a) {
                var c = b.content.dataset.slice(a, a + 1)[0];
                b.content.dataset.splice(a, 1),
                b.content.dataset.splice(a - 1, 0, c),
                g()
            }
        }),
        d.find(".j-movedown").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = b.content.dataset.length;
            if (a != c - 1) {
                var d = b.content.dataset.slice(a, a + 1)[0];
                b.content.dataset.splice(a, 1),
                b.content.dataset.splice(a + 1, 0, d),
                g()
            }
        }),
        d.find(".ctrl-item-list-add").click(function () {
            var a = {
                link: "/Shop/index",
                linkType: 6,
                showtitle: "首页",
                title: "链接到店铺主页",
                pic: "/PublicMob/images/ind3_1.png",
                bgColor: "#07a0e7"
            };
            b.content.dataset.push(a),
            g()
        }),
        d.find(".j-del").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            b.content.dataset.splice(a, 1),
            g()
        }),
        d.find("input[name='navtitle']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = $(this).val();
            b.content.dataset[a].showtitle = c,
            g()
        }),
        d.find(".droplist li").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (c, d) {
                    b.content.dataset[a].title = c.title,
                    b.content.dataset[a].link = c.link,
                    b.content.dataset[a].linkType = d,
                    g()
                }
            })
        }),
        d.find("input[name='customlink']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            b.content.dataset[a].link = $(this).val()
        }),
        d.find(".j-navModifyIcon").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            HiShop.popbox.ImgPicker(function (c) {
                b.content.dataset[a].pic = c[0],
                g()
            })
        }),
        d.find("select[name='navbgColor']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = $(this).val();
            b.content.dataset[a].bgColor = c,
            g()
        }),
        //导航颜色选择器
         d.find(".colorPicker").each(function (e) {
             var name = $(this).data("name"),
                 color = $(this).data("color"),
                 selector = "#j_clp_col" + name;
             // alert($(selector).html());
             $(this).ColorPicker({
                 color: color,
                 onShow: function (colpkr) {
                     $(colpkr).fadeIn(500);
                     return false;
                 },
                 onHide: function (colpkr) {
                     $(colpkr).fadeOut(500);
                     g();
                     return false;
                 },
                 onChange: function (hsb, hex, rgb) {
                     var hex = '#' + hex;
                     $(selector).css("background-color", hex);
                     b.content.dataset[e].fotColor = hex;
                 }
             });
         });
    }
});