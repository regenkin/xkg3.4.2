var weiboHelper = {
    options: {
        Emotions: [["ca/bbqnkkbaoyou_thumb.gif", "康康保佑"], ["e4/bbhlyanzhigao_thumb.gif", "甜馨颜值高"], ["e4/bbhlyanzhigao_thumb.gif", "甜馨尴尬"], ["66/huaqianguxiaogu_thumb.gif", "小骨最萌了"], ["08/dorahaose_thumb.gif", "哆啦A梦花心"], ["c8/lxhzuiyou_thumb.gif", "最右"], ["64/lxhtongku_thumb.gif", "泪流满面"], ["67/gangnamstyle_thumb.gif", "江南style"], ["fa/lxhtouxiao_thumb.gif", "偷乐"], ["b6/doge_thumb.gif", "doge"], ["7a/shenshou_thumb.gif", "草泥马"], ["4a/mm_thumb.gif", "喵喵"], ["34/xiaoku_thumb.gif", "笑cry"], ["60/horse2_thumb.gif", "神马"], ["bc/fuyun_thumb.gif", "浮云"], ["c9/geili_thumb.gif", "给力"], ["f2/wg_thumb.gif", "围观"], ["70/vw_thumb.gif", "威武"], ["6e/panda_thumb.gif", "熊猫"], ["81/rabbit_thumb.gif", "兔子"], ["bc/otm_thumb.gif", "奥特曼"], ["15/j_thumb.gif", "囧"], ["89/hufen_thumb.gif", "互粉"], ["c4/liwu_thumb.gif", "礼物"], ["ac/smilea_thumb.gif", "呵呵"], ["0b/tootha_thumb.gif", "嘻嘻"], ["6a/laugh.gif", "哈哈"], ["14/tza_thumb.gif", "可爱"], ["af/kl_thumb.gif", "可怜"], ["a0/kbsa_thumb.gif", "挖鼻屎"], ["f4/cj_thumb.gif", "吃惊"], ["6e/shamea_thumb.gif", "害羞"], ["c3/zy_thumb.gif", "挤眼"], ["29/bz_thumb.gif", "闭嘴"], ["71/bs2_thumb.gif", "鄙视"], ["6d/lovea_thumb.gif", "爱你"], ["9d/sada_thumb.gif", "泪"], ["19/heia_thumb.gif", "偷笑"], ["8f/qq_thumb.gif", "亲亲"], ["b6/sb_thumb.gif", "生病"], ["58/mb_thumb.gif", "太开心"], ["17/ldln_thumb.gif", "懒得理你"], ["98/yhh_thumb.gif", "右哼哼"], ["6d/zhh_thumb.gif", "左哼哼"], ["a6/x_thumb.gif", "嘘"], ["af/cry.gif", "衰"], ["73/wq_thumb.gif", "委屈"], ["9e/t_thumb.gif", "吐"], ["f3/k_thumb.gif", "打哈欠"], ["27/bba_thumb.gif", "抱抱"], ["7c/angrya_thumb.gif", "怒"], ["5c/yw_thumb.gif", "疑问"], ["a5/cza_thumb.gif", "馋嘴"], ["70/88_thumb.gif", "拜拜"], ["e9/sk_thumb.gif", "思考"], ["24/sweata_thumb.gif", "汗"], ["7f/sleepya_thumb.gif", "困"], ["6b/sleepa_thumb.gif", "睡觉"], ["90/money_thumb.gif", "钱"], ["0c/sw_thumb.gif", "失望"], ["40/cool_thumb.gif", "酷"], ["8c/hsa_thumb.gif", "花心"], ["49/hatea_thumb.gif", "哼"], ["36/gza_thumb.gif", "鼓掌"], ["d9/dizzya_thumb.gif", "晕"], ["1a/bs_thumb.gif", "悲伤"], ["62/crazya_thumb.gif", "抓狂"], ["91/h_thumb.gif", "黑线"], ["6d/yx_thumb.gif", "阴险"], ["89/nm_thumb.gif", "怒骂"], ["40/hearta_thumb.gif", "心"], ["ea/unheart.gif", "伤心"], ["58/pig.gif", "猪头"], ["d6/ok_thumb.gif", "ok"], ["d9/ye_thumb.gif", "耶"], ["d8/good_thumb.gif", "good"], ["c7/no_thumb.gif", "不要"], ["d0/z2_thumb.gif", "赞"], ["40/come_thumb.gif", "来"], ["d8/sad_thumb.gif", "弱"], ["91/lazu_thumb.gif", "蜡烛"], ["d3/clock_thumb.gif", "钟"], ["1b/m_thumb.gif", "话筒"], ["6a/cake.gif", "蛋糕"]]
    },
    FilterEmotionHtml: function (str) {
        var obj = weiboHelper.options.Emotions;
        var html = str;
        for (var i = 0; i < obj.length; i++) {
            var m = "\/\\[" + obj[i][1] + "\\]\/g";
            html = html.replace(eval(m), "<img src='http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/" + obj[i][0] + "' width='22' height='22'>");
        }
        var reg = /http:\/\/[\w-]*(\.[\w-]*)+/ig;
        var reg =new RegExp("http://t.cn(/[a-zA-Z0-9\\.\\-~!@#$%^&*+?:_/=<>]*)?", "gi");
        //html = html.replace(reg, function (m) { return '<a href="' + m + '" target="_blank">' + m + '</a>'; });
        html = html.replace(reg, function (m) { return '<a class="W_btn_cardlink" title="' + m + '" href="' + m + '" target="_blank"><i class="W_ficon ">O</i><i class="W_vline"></i><em class="W_autocut">网页链接</em></a>'; });
        return html;
    },
    InsertImgToPosition: function (obj) {
        var imgSrc = $(obj).css("background-image")
        if (imgSrc.length > 5) {
            $(".js-choose-img img").remove();
            $(".js-trigger-image").css("padding-left", "5px");
            var img = '<img src="' + imgSrc.slice(5, imgSrc.length - 2) + '" data-full-size="' + imgSrc.slice(5, imgSrc.length - 2) + '" width="100" height="100" class="thumb-image">';
            $(img).insertBefore(".js-trigger-image");
            $('#myModal').modal('hide');
        }
    }
};


function showpictrue(img, tid) {
    var obj = $("#detail" + tid);
    var imgobj = obj.find("img[node-type='imgShow']");
    $(imgobj).attr("src", img).animate({ opacity: '1' }, 1000);//.animate({ opacity: '0.8' }, 1);//

    var imgarr = obj.find(".wb-media-list img");
    var imglistnum = imgarr.length;
    obj.find(".choose_box a").removeClass("current");
    var selid = 0;
    for (var i = 0; i < imglistnum; i++) {
        if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == img) {
            selid = i;
            $(obj.find(".choose_box a")[i]).attr("class", "current");
            break;
        }
    }
    if (imglistnum > 7) {
        if (selid > 5) {
            $("#detail" + tid).find("a[action-type='next']").click();
        } else {
            $("#detail" + tid).find("a[action-type='prev']").click();
        }
    }
    if (selid == 0 || selid == imglistnum - 1) {
        $("#detail" + tid).find('li[node-type="imgBox"]').attr("class", "clearfix smallcursor");
    }
}
/*隐藏或显示查看大图层*/
function upordownpiclist(tid) {
    var obj = $("#detail" + tid);
    if ($(obj.find(".wb-image--expand")).is(":visible")) {
        $(obj.find(".wb-image--expand")).hide();
        $(obj.find(".wb-media")).show();
    } else {
        $(obj.find(".wb-image--expand")).show();
        $(obj.find(".wb-media")).hide();
    }
}
function imageonmousemove(evnt, obj, imgarr) {
    if (evnt) {
        //alert(evnt.clientX)
        //获取当前对象后面是否还是图片
        var imglistnum = imgarr.length;
        var hasnext = false;
        var hasprev = false;
        for (var i = 0; i < imglistnum;i++){
            if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == $(obj).attr("src")){
                if (i < imglistnum-1) {
                    hasnext = true;
                }
                if (i > 0) {
                    hasprev = true;
                }
            }
        }
        nx = (parseInt(evnt.clientX) - $(obj).offset().left) / ($(obj).css("width").replace("px",""));      
        if (nx > 0.5) {
            if (hasnext) {
                $($(obj).parent().parent().parent()).attr("class", "clearfix rightcursor");
            } else {
                $($(obj).parent().parent().parent()).attr("class", "clearfix smallcursor");
            }
        } else {
            if (hasprev) {
                $($(obj).parent().parent().parent()).attr("class", "clearfix leftcursor");
            } else {
                $($(obj).parent().parent().parent()).attr("class", "clearfix smallcursor");
            }
        }
    }
}
/*旋转效果，暂时隐藏*/
function rotate(objImg, type) {
    if ($(objImg).attr("pvalue") == undefined) {
        $(objImg).attr("pvalue", 0);
    }
    var oldvalue = parseInt($(objImg).attr("pvalue"));
    var value = (type = "right" ? oldvalue + 90 : (oldvalue - 90));
    $(objImg).attr("pvalue", value);
    var tid = $(objImg).attr("tid");
    var parent = $("#detail" + tid).find("div[node-type='imgSpanBox']");
    var tmp = $(objImg).clone();

    var theImage = new Image();
    theImage.src = $(objImg).attr("src");
    var tmpwidth = theImage.width;
    var tmpheight = theImage.height;
    if (value % 180 != 0) {
        var t = tmpwidth;
        tmpwidth = tmpheight;
        tmpheight = t;
    }
    var $div = $('<div style="position: relative; text-align: center; height: ' + tmpheight + 'px; width: ' + tmpwidth + 'px;"></div>');
    $(tmp).css("webkitTransform", "rotate(" + value + "deg)");
    $(tmp).css("msTransform", "rotate(" + value + "deg)");
    $(tmp).css("MozTransform", "rotate(" + value + "deg)");
    $(tmp).css("OTransform", "rotate(" + value + "deg)");
    $(tmp).css("transform", "rotate(" + value + "deg)");
    $div.append(tmp);
    $(parent).html("");
    parent.append($div);

    //$("#detail" + tid).find('img[node-type="imgShow"]').mousemove(function (event) {
    //    imageonmousemove(event, this, imgarr);
    //});
    //$("#detail" + tid).find('img[node-type="imgShow"]').click(function () {
    //    var tid = $(this).attr("tid");
    //    //获取当前顶部的样式
    //    var liClassName = $($(this).parent().parent().parent()).attr("class");
    //    var imglistnum = imgarr.length;
    //    if (liClassName.indexOf("rightcursor") != -1) {
    //        for (var i = 0; i < imglistnum; i++) {
    //            if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == $(this).attr("src")) {
    //                if (i < imglistnum - 1) {
    //                    showpictrue($(imgarr[i + 1]).attr("src").replace(/\/square\//g, "/bmiddle/"), tid);
    //                    break;
    //                }
    //            }
    //        }
    //    } else if (liClassName.indexOf("leftcursor") != -1) {
    //        for (var i = 0; i < imglistnum; i++) {
    //            if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == $(this).attr("src")) {
    //                if (i > 0) {
    //                    showpictrue($(imgarr[i - 1]).attr("src").replace(/\/square\//g, "/bmiddle/"), tid);
    //                    break;
    //                }
    //            }
    //        }
    //    } else {
    //        upordownpiclist(tid);
    //    }
    //});
}




$(document).ready(function () {
    var EmotionFace = weiboHelper.options.Emotions;

    $(".js-open-emotion").click(function (event) {
        event.preventDefault()
        event.stopPropagation();
        if ($(".emotion-wrapper").is(":visible")) {
            $(".emotion-wrapper").hide()
        } else {
            var emotionHtml = "";
            for (var i = 0; i < EmotionFace.length; i++) {
                emotionHtml += '<li><img src="http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/' + EmotionFace[i][0] + '" alt="[' + EmotionFace[i][1] + ']" title="[' + EmotionFace[i][1] + ']"></li>';
            }
            $(".emotion-container").html(emotionHtml);
            $(".emotion-wrapper").show("slow", function () {
                $(".emotion-container img").unbind("click");
                $(".emotion-container img").click(function () {
                    $("#txtContent").val($("#txtContent").val() + ($(this).attr("alt"))).keyup();
                    $(".emotion-wrapper").hide();
                    $("#txtContent").css("display", "block");
                    $("#showdiv").css("display", "none");
                    ReceiverType = "text";
                })
            });
        }
    })

})
function closeModal(modalid, txtContentid, value) {
    $("#" + txtContentid).val(' ' + $("#" + txtContentid).val() + 'http://' + value + " ").keyup();
    $('#' + modalid).modal('hide');
    $("#txtshowContent").val('http://' + value + " ").keyup();
}
function closeModalPic(modalid, Imgvalue) {
    
    var src = "";
    if (Imgvalue[0] != "") {
        src = Imgvalue[0].substring(Imgvalue[0].indexOf('/Storage'), Imgvalue[0].length);
    }
    var img = "<div class='inlie'><img src='" + src + "' id='imgvalue' width='100' height='100'/><i class='glyphicon glyphicon-remove' id='removeimg'></i></div>";
    $("#picback").html('');
    $("#picback").append(img);
    $('#' + modalid).modal('hide')
    $('#removeimg').click(function () {
        $(this).parent().remove();
    });
}
function ErrorAndIsAuthorized(d) {
    if (typeof (d.IsAuthorized) != "undefined") {
        if (d.IsAuthorized == "0")
            document.location.href = "setting.aspx";
    }
    if (typeof (d.error) != "undefined") {
      
        HiTipsShow(d.error, 'fail');
       
    }
}
