<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="StoreCardSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.StoreCardSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Utility/uploadImage/jquery.form.js"></script>
    <script src="/Utility/uploadImage/uploadPreview.js"></script>
    <script type="text/javascript" src="/admin/js/jquery-json-2.4.js"></script>
    <script type="text/javascript" src="/admin/js/jscolor.min.js"></script>
    <style type="text/css">
        input[type="color"] {
            width: 40px;
            border: none;
            vertical-align: middle;
        }
   .upFile .adds {width: 100px; height: 100px; color: #FFF; position: absolute; top: 0px;text-align: center;line-height: 100px; font-size: 28px; font-weight: 600;}
  .upFile .bgImg {border: 2px dashed #FF8182;width: 100px; height: 100px; margin-bottom: 5px;margin-right: 10px;cursor:pointer;}
  .upFile img { width: 100%;height: 100%;}
   .upFile .adds input {position: absolute;top: 0px;width: 100px;height: 100px;display: block;overflow: hidden;text-indent: -9999px;opacity: 0;cursor:pointer;
}
   .Vjscolor{display:none}
   .jscolor{width: 40px;
            font-size:12px;
            line-height:23px;
            border: none;
            vertical-align: middle;color:#eee}
    </style>
    <script type="text/javascript">

        var alert_h = function (msg) {
            HiTipsShow(msg, "warning"); //前端提醒转为后端提醒样式
        }

        function upLoadFile() {
            var options = {
                type: "POST",
                url: '/API/Files.ashx?action=BackgroundUpload',
                success: showResponse
            };
            // 将options传给ajaxForm
            $('#ImageForm').ajaxSubmit(options); //上传图片
        }


        function showResponse(data) {


            if (data == "0") {
                alert_h("背景图片上传图片失败!");
                $("#preservation").attr("disabled", false);
                return;
            } else if (data == "1") {
                alert_h("背景图片大小超过1M，不能上传!");
                $("#preservation").attr("disabled", false);
                return;

            } else if (data == "2") {
                alert_h("上传的背景图片文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)!");
                $("#preservation").attr("disabled", false);
                return;

            } else if (data == "3") {
                data = ""; //为空值
                data = ybgimg;  //如果没有图片上,使用默认图片
            }

            var jsonArr = [];
            $('.touchmove').each(function () {
                jsonArr.push({
                    left: parseInt($(this).css('left')) / 320,
                    top: parseInt($(this).css('top')),
                    width: $(this).find('img').width() / 320,
                })
            })

          
            var $color = $(".Vjscolor");

            var postData = {
                posList: jsonArr,
                DefaultHead: $('input:radio[name=UserHeader]:checked').val(),
                myusername: $('#myusername').val(),
                shopname: $('#shopname').val(),
                BgImg: data,
                myusernameSize: $("#myusernameSize").val(),
                shopnameSize: $("#shopnameSize").val(),
                myusernameColor:"#"+ $color.eq(0).val(),
                shopnameColor: "#" + $color.eq(2).val(),
                nickNameColor: "#" + $color.eq(1).val(),
                storeNameColor: "#" + $color.eq(3).val()
            }

            if (!/^\d+$/.test(postData.myusernameSize) || !/^\d+$/.test(postData.shopnameSize)) {
                alert_h("文字大小请填写整数值！");
                $("#preservation").attr("disabled", false);
                return;
            }

            
           

            $.ajax({
                url: "StoreCardSet.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "Edit", SotreCardJson:$.toJSON(postData) },
                cache: false,
                success: function (resultData) {
                    if (resultData.success) {
                    
                        HiTipsShow("保存成功！","success");
                        $("#preservation").attr("disabled", false);
                    } else {
                    
                        alert_h(resultData.msg);
                        $("#preservation").attr("disabled", false);
                    }
                },
                error: function (data, status, e) {
                  
                    alert_h("系统错误，请重试！");
                    $("#preservation").attr("disabled", false);
                }
            });


        }


        function ReadSetData(SetData) {

            $("#idImg").attr("src", SetData.BgImg);
            $('.exitbusinesscard').css({
                "background": 'url(' + SetData.BgImg + ') no-repeat center',
                'backgroundSize': 'cover'
            });

            $('input:radio[name=UserHeader][value='+SetData.DefaultHead+']').attr("checked", true); //选中
            $('input:radio[name=UserHeader][value='+SetData.DefaultHead+']').trigger("click");

          
            //延迟处理一下，等待jscolor执行完，以免颜色不对
            setTimeout(function () {

                $color = $(".Vjscolor");

                $color.eq(0).val(SetData.myusernameColor.replace("#", ""));
                $color.eq(1).val(SetData.nickNameColor.replace("#", ""));
                $color.eq(2).val(SetData.shopnameColor.replace("#", ""));
                $color.eq(3).val(SetData.storeNameColor.replace("#", ""));

                $jscolor = $(".jscolor");
                $jscolor.eq(0).css("background", SetData.myusernameColor);
                $jscolor.eq(1).css("background", SetData.nickNameColor);
                $jscolor.eq(2).css("background", SetData.shopnameColor);
                $jscolor.eq(3).css("background", SetData.storeNameColor);
                $color.trigger("change");

            }, 100);

           

            $("#myusername").val(SetData.myusername);
            $("#shopname").val(SetData.shopname);

            $("#myusername,#shopname").trigger("keyup");
           
            $("#myusernameSize").val(SetData.myusernameSize);
            $("#shopnameSize").val(SetData.shopnameSize);
            $("#myusernameSize,#shopnameSize").trigger("change");


            
            
            $('.touchmove').each(function (i) {
                $(this).css({ "left": SetData.posList[i].left * 320 + "px", "top": SetData.posList[i].top + "px" });
                $(this).find('img').width(SetData.posList[i].width * 320).height(SetData.posList[i].width * 320);
            })

         
        }

        var ybgimg = "/Utility/pics/storecardbg.png"; //默认图片

        $(function () {


            


            //上传控件调用
            $("#idFile").uploadPreview({
                Img: "idImg", Width: 100, Height: 100, Callback: function (bgurl) {

                    if (bgurl != "") {
                        if (bgurl.indexOf("IE^") == 0) {
                            //兼容IE浏览器的方法
                            var bgurls = bgurl.split("^");
                            $('.exitbusinesscard').css({
                                'filter': 'progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)',
                                'width': '320px',
                                'height':'490px'
                            });
                            $('.exitbusinesscard')[0].filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = bgurls[1];

                        }
                        else {
                            $('.exitbusinesscard').css({
                                "background": 'url(' + bgurl + ') no-repeat center',
                                'backgroundSize': 'cover'
                            });
                        }
                    }
                }
            });

            ///是否启用头像
            $('input:radio[name=UserHeader]').click(function () {
                if ($(this).val() == 2)
                {
                    $("#HeadPanel").hide();
                } else {
                    $("#HeadPanel").show();
                };
            });

            //调整字体大小
            $("#myusernameSize,#shopnameSize").change(function () {
               
                var flag = $(this).attr("flag");

                if (!/^\d+$/.test($(this).val())) {
                    alert_h("请填整数值！");
                    return;
                }


                var resize = parseInt($(this).val());
                if (flag == 1) {
                    $("#UnamePanel").css("fontSize", resize);

                } else {
                    $("#DescPanel").css("fontSize", resize);
                }
            });


            //调整字体大小
            $("#myusername,#shopname").keyup(function () {
                var flag = $(this).attr("flag");
                var ctx = $(this).val();
                var Thtml = ctx.replace("{{昵称}}", "<strong>昵称</strong>").replace("{{店铺名称}}", "<strong>店铺名称</strong>");
                if (flag == 1) {
                    $("#UnamePanel").html(Thtml);
                } else {
                    $("#DescPanel").html(Thtml);
                }
            });


            //input type="color" 调整颜色

            $(".Vjscolor").change(function () {
                var flag = $(this).attr("flag");
                var recolor ="#"+ $(this).val();
              
              

                if (flag == 1) {
                    $("#UnamePanel").css("color", recolor);
                }
                else if (flag == 2) {
                    $("#UnamePanel").find("strong").css("color", recolor);
                }
                else if (flag == 3) {
                    $("#DescPanel").css("color", recolor);
                }
                else if (flag == 4) {
                 
                    $("#DescPanel").find("strong").css("color", recolor);
                }

            });

            $('body').on('mousedown', '.touchmove', function (evt) {
                evt.preventDefault();
                var downX = evt.clientX,
                    downY = evt.clientY,
                    topN = downY - parseInt($(this).css('top')),
                    leftN = downX - parseInt($(this).css('left')),
                    winH = $('.exitbusinesscard').outerHeight(),
                    elemH = $(this).height(),
                    winW = $('.exitbusinesscard').width(),
                    elemW = $(this).outerWidth(),
                    _this = $(this);
                _this.css('zIndex', 100);
                $(document).on('mousemove', function (evt) {
                    var moveX = evt.clientX - leftN,
                        moveY = evt.clientY - topN;
                    if (moveY < 0) moveY = 0;
                    if (moveY > winH - elemH) moveY = winH - elemH;
                    if (moveX < 0) moveX = 0;
                    if (moveX > winW - elemW) moveX = winW - elemW;
                    _this.css({
                        'left': moveX,
                        'top': moveY
                    })
                })
                $(document).on('mouseup', function () {
                    $(document).off('mousemove');
                    $(document).off('mouseup');
                    _this.css('zIndex', 0);
                })
            });
            $('.touchmove .left,.touchmove .right').on('mousedown', function (evt) {
                evt.stopPropagation();
                evt.preventDefault();
                var downX = evt.clientX,
                    imgElement = $(this).parent().find('img'),
                    imgW = imgElement.width(),
                    imgH = imgElement.height(),
                    classNa = $(this)[0].className,
                    touchmoveEl = $(this).parent(),
                    moveL = parseInt(touchmoveEl.css('left')),
                    $obj = 320 - $(this).parent().width() - parseInt($(this).parent().css('left'));
                $(document).on('mousemove', function (evt) {
                    var moveX = evt.clientX - downX;

                    if (classNa == 'left') {
                        moveX = -moveX;
                        touchmoveEl.css('left', moveL - moveX / 2 + 'px')
                    }
                    if (moveX > $obj) {

                        return;
                    }
                    imgElement.css({
                        'width': imgW + moveX,
                        'height': imgH + moveX
                    })
                })
                $(document).on('mouseup', function () {
                    $(document).off('mousemove');
                    $(document).off('mouseup');
                })
            })
            $('.touchmove .bottom,.touchmove .top').on('mousedown', function (evt) {
                evt.stopPropagation();
                evt.preventDefault();
                var downY = evt.clientY,
                    imgElement = $(this).parent().find('img'),
                    imgW = imgElement.width(),
                    imgH = imgElement.height(),
                    classNa = $(this)[0].className,
                    touchmoveEl = $(this).parent(),
                    moveT = parseInt(touchmoveEl.css('top'));
                $(document).on('mousemove', function (evt) {
                    var moveY = evt.clientY - downY;
                    if (classNa == 'top') {
                        moveY = -moveY;
                        touchmoveEl.css('top', moveT - moveY / 2 + 'px')
                    }
                    imgElement.css({
                        'width': imgW + moveY,
                        'height': imgH + moveY
                    })
                })
                $(document).on('mouseup', function () {
                    $(document).off('mousemove');
                    $(document).off('mouseup');
                })
            })
            $('.cardqrcode .cardimg,.exitbusinesscard .img').hover(function () {
                $(this).addClass('border').find('span').show();
            }, function () {
                $(this).removeClass('border').find('span').hide();
            })
            $('#preservation').click(function () {
                $("#preservation").attr("disabled", true)
                upLoadFile();

            });
            // $('#myusername').keydown(function (){
            //     $('.myusername strong').text($(this).val());
            // })



            //读取原设置数据
            jQuery.getJSON("/Storage/Utility/StoreCardSet.js", { random: Math.random() }, function (SetData) {
                if (SetData != null) {
                    ybgimg = SetData.BgImg;
                    ReadSetData(SetData);
                }
            });


        })

       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>掌柜名片设置</h2>
    </div>




    <div class="clearfix">

        <div class="edit-text-left">
            <div class="mobile-border">
                <div class="mobile-d">
                    <div class="mobile-header">
                        <i></i>
                        <div class="mobile-title">掌柜名片</div>
                    </div>
                    <div class="upshop-view">
                        <div class="exitbusinesscard" style="overflow: hidden; background: url(/Utility/pics/storecardbg.png) no-repeat center; background-size: 100%;">
                            <div style="z-index: 0; left: 92px; top: 24px;" id="HeadPanel" class="touchmove img">
                                <img style="width: 133px; height: 133px;" src="/Utility/pics/headLogo.jpg" width="135">
                                <span style="display: none;" class="left"></span>
                                <span style="display: none;" class="top"></span>
                                <span style="display: none;" class="right"></span>
                                <span style="display: none;" class="bottom"></span>
                            </div>
                            <h1 class="touchmove myusername" id="UnamePanel">我是<strong>昵称</strong></h1>
                            <h3 class="touchmove shopname" id="DescPanel">邀请您加入<strong>店铺名称</strong>分销团队</h3>
                            <div class="cardqrcode" id="CodePanel">
                                <div class="touchmove cardimg">
                                    <img src="/Utility/pics/wfxqrcode.jpg" width="200">
                                    <span style="display: none;" class="left"></span>
                                    <span style="display: none;" class="top"></span>
                                    <span style="display: none;" class="right"></span>
                                    <span style="display: none;" class="bottom"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mobile-footer"></div>
                </div>
            </div>
        </div>

        <div class="edit-text-right">
            <div class="edit-inner" style="border-radius: 5px; width: 630px; margin-top: 70px;">
                <strong style="color: red; font-weight: normal; display: block; margin-bottom: 30px;">注意：可以使用鼠标左键拖动左图中各元素,进行位置调整</strong>
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-xs-2 pad resetSize control-label" for="setdate">默认头像：</label>
                        <div class="form-inline journal-query col-xs-9">
                            <div class="form-group pt3">
                                <label class="middle mr10">
                                    <input type="radio" checked="checked" name="UserHeader" value="0">使用会员头像</label>
                                <label class="middle mr10">
                                    <input type="radio" name="UserHeader"  value="1">使用分销商店铺Logo</label>
                                <label class="middle mr10">
                                    <input type="radio" name="UserHeader"  value="2">不使用头像</label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-2 pad resetSize control-label" for="setdate">个人介绍：</label>
                        <div class="form-inline journal-query col-xs-10">
                            <div class="form-group">
                                <input type="text" class="form-control resetSize inputw200" id="myusername" flag="1" value="我是{{昵称}}">
                                &nbsp;&nbsp;&nbsp;&nbsp;文字样式：<button class="jscolor {valueElement:'chosen-value1'}">文字</button><input id="chosen-value1" type="text" class="Vjscolor" flag="1">
                                <button class="jscolor {valueElement:'chosen-value2'}">昵称</button>
                                <input type="text" class="Vjscolor" id="chosen-value2" flag="2">
                                &nbsp;&nbsp;<input type="number"  id="myusernameSize" flag="1" style="width: 40px;vertical-align:middle" value="14">&nbsp;像素
                            </div>
                            <small style="margin-left: -15px;">“{{昵称}}”为系统参数，实际显示以分销商的昵称替代</small>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-2 pad resetSize control-label" for="setdate">推广口号：</label>
                        <div class="form-inline journal-query col-xs-10">
                            <div class="form-group">
                               
                                <textarea id="shopname" flag="2"  class="form-control resetSize inputw200" style="height:50px">邀请您加入{{店铺名称}}分销团队</textarea>
                          &nbsp;&nbsp;&nbsp;&nbsp;文字样式：
                                <button class="jscolor {valueElement:'chosen-value3'}">文字</button>
                                <input type="text" class="Vjscolor"  id="chosen-value3"  flag="3">
                                <button class="jscolor {valueElement:'chosen-value4'}">店铺</button>
                                <input type="text" class="Vjscolor" id="chosen-value4" flag="4">
                                &nbsp;&nbsp;<input type="number" id="shopnameSize" flag="2"  style="width: 40px;vertical-align:middle" value="14">&nbsp;像素
                            </div>
                            <small style="margin-left: -15px;">“{{店铺名称}}”为系统参数，实际显示以分销商的店铺名称替代</small>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-2 pad resetSize control-label" for="setdate">背景图片：</label>
                        <div class="form-inline journal-query col-xs-9">
                            <div class="form-group pt3">

                              
                                <form id="ImageForm" method="post" enctype="multipart/form-data">
                                    <div class="upFile clearfix">
                                        <div class="bgImg">
                                            <img id="idImg" src="/Utility/pics/storecardbg.png" width="100" clientidmode="Static" height="100" runat="server" />
                                        </div>
                                        <div class="adds">
                                            <input id="idFile" name="idFile" type="file" />
                                        </div>
                                    </div>
                                </form>

                            </div>
                            <small style="margin-left: -15px;">点击图片上传，建议上传640*1138px，小于1M，png、jpg格式图片</small>
                        </div>
                    </div>
                    <div class="form-group pt10">
                        <label class="col-xs-2 pad resetSize control-label" for="setdate">&nbsp;</label>
                        <div class="form-inline journal-query col-xs-9">
                            <button class="btn btn-primary" id="preservation" onclick="return false" style="margin-left: -15px; width: 100px;">保存</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
