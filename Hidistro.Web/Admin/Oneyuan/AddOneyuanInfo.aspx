<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="AddOneyuanInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.AddOneyuanInfo" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" TagName="ViewTab" Src="~/Admin/Oneyuan/OneTaoViewTab.ascx" %> 
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagName="SetMemberRange" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Utility/uploadImage/jquery.form.js"></script>
    <script src="/Utility/uploadImage/uploadPreview.js"></script>
    <script type="text/javascript" src="/admin/js/jquery-json-2.4.js"></script>
    <style type="text/css">
        .upFile .adds { width: 160px; height:50px; color: #FFF; position: absolute; top: 0px; text-align: center; line-height: 100px; font-size: 28px; font-weight: 600; }
        .upFile .bgImg {background:url(/Utility/pics/addup.png) center center no-repeat; border: 2px dashed #ccc; width: 160px; height: 50px; margin-bottom: 5px; margin-right: 10px; cursor: pointer; }
        .upFile img { width: 100%;display:none }
        .upFile .adds input { position: absolute; top: 0px; width: 160px; height: 50px; display: block; overflow: hidden; text-indent: -9999px; opacity: 0; cursor: pointer; }
   .popover-title{padding:8px 14px;margin:0;font-size:14px;background-color:#f7f7f7;border-bottom:1px solid #ebebeb;border-radius:5px 5px 0 0}
.popover{position:absolute;top:0;left:0;z-index:1060;display:none;max-width:350px;padding:1px;font-family:"Helvetica Neue",Helvetica,Arial,sans-serif;font-size:14px;font-style:normal;font-weight:400;line-height:1.42857143;text-align:left;text-align:start;text-decoration:none;text-shadow:none;text-transform:none;letter-spacing:normal;word-break:normal;word-spacing:normal;word-wrap:normal;white-space:normal;background-color:#fff;-webkit-background-clip:padding-box;background-clip:padding-box;border:1px solid #ccc;border:1px solid rgba(0,0,0,.2);border-radius:6px;-webkit-box-shadow:0 5px 10px rgba(0,0,0,.2);box-shadow:0 5px 10px rgba(0,0,0,.2);line-break:auto}
.popover-title{background:#808080;color:#fff}
h2 small{margin-top:3px;float:right;color:#ff6a00}
di{color:red}
         </style>
    <script type="text/javascript">
        var viewAid = "<%=viewAid%>"; //查看ID值
        var OneTaoState = "<%=OneTaoState%>";
        var EditJsonData = null;
        <%= EditJsonDataStr%>;

        var alert_h = function (msg) {
            HiTipsShow(msg, "warning"); //前端提醒转为后端提醒样式
        }

        var postData = null;

        if (EditJsonData != null && EditJsonData.ActivityId != null) {
            postData = EditJsonData;
            postData.action = "edit";
        }
        else {
            postData = {
                action: "save", ActivityId: "", IsOn: true, Title: "活动标题未设置", StartTime: "", EndTime: "", HeadImgage: "http://fpoimg.com/320x100", ReachType: 0,
                ActivityDec: "", ProductId: 0, ProductPrice: 0, ProductImg: "", ProductTitle: "未设置", SkuId: "none", PrizeNumber: 1, EachPrice: 1,
                EachCanBuyNum: 1, FitMember: "",DefualtGroup:"",CustomGroup:"", ReachNum: 0, FinishedNum: 0
            }; //需要提交的参数
        }





        function saveinfo(obj) {
            if (postData.Title == "活动标题未设置") {
                HiTipsShow("活动标题未设置", "error");
                return;
            }

            if (postData.StartTime == "") {
                HiTipsShow("开始时间未设置", "error");
                return;
            }

            if (postData.EndTime == "") {
                HiTipsShow("活动结束时间未设置", "error");
                return;
            }

            if (postData.ProductTitle == "" || postData.ProductTitle == "未设置" || postData.ProductId == 0) {

                HiTipsShow("活动奖品未选择", "error");
                return;
            }

            if (postData.ReachNum == 0) {

                HiTipsShow("满足份数未设置！", "error");
                return;
            }

            var endDate = new Date(postData.EndTime.replace(/-/g, "/"));
            var nowDate = new Date();
            var StartDate = new Date(postData.StartTime.replace(/-/g, "/"));

            if (endDate < nowDate) {
                HiTipsShow("结束时间不能少于当前时间", "error");
                return;
            }

            if (endDate < StartDate) {
                HiTipsShow("结束时间不能少于开始时间", "error");
                return;
            }



            if (postData.ReachType == 0) {

                HiTipsShow("开奖方式未选择", "error");
                return;
            }


          

            if (postData.ReachType == 1 && postData.EachCanBuyNum * 1 > postData.ReachNum * 1) {
                HiTipsShow("当前开奖方式，‘每人限购’份数不能大于‘满足份数’", "error");
                return;
            }

            postData.ActivityDec = $("#ADesc").val();

            if (postData.ActivityDec.length > 100) {
                HiTipsShow("活动描述内容太长，最多不能大于100字！", "error");
                return;
            }

            upLoadFile(obj);
        }

        function ShowProduct() {

            if (viewAid != "" || OneTaoState == "进行中")
                return; //阻止修改商品信息


            $DialogFrame_ReturnValue = "";// 返回值，IsMultil=1可多选
            DialogFrame("ProductSelect.aspx?IsMultil=0", "选择商品",720, 500, function (rs) {

                if (rs != "") {
                    var rsArray = rs.split("^");

                    if (rsArray.length == 7) {
                        //获取到正确的数值44^男鞋^8.00^8.00^/Storage/master/product/thumbs60/60_b1d772be0a2b4f469cca08044c1c4c48.jpg^3000^0
                        if (rsArray[4] == "")
                            rsArray[4] = "/utility/pics/none.gif";

                        var phtml = '  <div class="shop-img fl">' +
                        '  <img src="' + rsArray[4] + '" width="60" style="height:60px!important" /></div>' +
                        '  <div class="shop-username fl ml10">' +
                        '   <p style="color:#222">' + rsArray[1] + '</p></div>' +
                        '  <p class="fl ml20">现价：￥' + rsArray[2] + '~￥' + rsArray[3] + '</p>' +
                        '  <p class="fl ml20">库存：' + rsArray[5] + '</p>';
                        $("#productInfo").html(phtml);

                        postData.ProductImg = rsArray[4];
                        postData.ProductPrice = rsArray[2];
                        postData.ProductId = rsArray[0];
                        postData.ProductTitle = rsArray[1];

                        setValue();
                    }

                }
            });

        }


        function setValue() {
            $("#LeftTopImg").css({
                "background": 'url(' + postData.HeadImgage + ') no-repeat center',
                'backgroundSize': 'cover'
            });

            $("#LeftProductTitle").text(postData.ProductTitle);
            $("#LeftProductDesc").html("总数量" + postData.PrizeNumber + "，每份价格<di>￥" + postData.EachPrice + "</di>，每人限购" + postData.EachCanBuyNum + "份");

            $("#LeftPtype").text("未设置");


            $("#leftTitle").text(postData.Title);

            if (postData.ReachType == "1") {
                $("#LeftPtype").html("活动结束前，购买满<di>" + postData.ReachNum + "</di>份后自动开奖");
            }
            else if (postData.ReachType == "2") {
                $("#LeftPtype").text("不计销售份数，到结束时间自动开奖");

            }
            else if (postData.ReachType == "3") {

                $("#LeftPtype").html("到结束时间，购买数份数大于<di>" + postData.ReachNum + "</di>份自动开奖");
            }

            $("#LeftDatatx").text("0天 0小时 0分 0秒");

            if (postData.EndTime != "") {



                var endDate = new Date(postData.EndTime.replace(/-/g, "/"));
                var nowDate = new Date();

                if (endDate > nowDate) {
                    var date3 = endDate - nowDate;
                    //计算出相差天数
                    var days = Math.floor(date3 / (24 * 3600 * 1000))

                    //计算出小时数
                    var leave1 = date3 % (24 * 3600 * 1000)    //计算天数后剩余的毫秒数
                    var hours = Math.floor(leave1 / (3600 * 1000))

                    //计算相差分钟数
                    var leave2 = leave1 % (3600 * 1000)        //计算小时数后剩余的毫秒数
                    var minutes = Math.floor(leave2 / (60 * 1000))

                    //计算相差秒数
                    var leave3 = leave2 % (60 * 1000)      //计算分钟数后剩余的毫秒数
                    var seconds = Math.round(leave3 / 1000)
                    $("#LeftDatatx").text(days + "天 " + hours + "小时 " + minutes + "分 " + seconds + "秒");
                }



            }


        }

        function upLoadFile(obj) {

            if ($("#idFile").val() == "") {
                showResponse(3);
                return;
            }

            var options = {
                type: "POST",
                url: '/API/Files.ashx?action=OneTao',
                success: showResponse,
                error: function () {
                    alert("0");
                    $(obj).attr("disabled", false); //禁止多次点击
                    $(obj).popover('hide');
                    
                }
            };
            // 将options传给ajaxForm
            $('#aspnetForm').ajaxSubmit(options); //上传图片
        }

        function showResponse(data) {

            var obj = "#saveBtn";
            if (data == "0") {
                alert_h("背景图片上传图片失败!");
                $(obj).attr("disabled", false); //禁止多次点击
                $(obj).popover('hide');
                return;
            } else if (data == "1") {
                alert_h("背景图片大小超过1M，不能上传!");
                $(obj).attr("disabled", false); //禁止多次点击
                $(obj).popover('hide');
                return;

            } else if (data == "2") {
                alert_h("上传的背景图片文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)!");
                $(obj).attr("disabled", false); //禁止多次点击
                $(obj).popover('hide');
                return;

            } else if (data == "3") {

                data = ""; //为空值

                if (postData.action == "save") {
                    $(obj).attr("disabled", false); //禁止多次点击
                    $(obj).popover('hide');
                    alert_h("没有图片上传成功！");
                    return;
                }
                else {
                    data = postData.HeadImgage; //当编辑时，没有图片上传
                }

            }
            var memberlvl = "";
            var defualtGroup = "";
            var customGroup = "";
            if ($('#allmember').prop('checked')) {
                isAllmember = true;
                memberlvl = "0";
                defualtGroup = "0";
                customGroup = "0";
            }
            else {
                isAllmember = false;
                $('#memberGradediv').find('input').each(function () {
                    if ($(this).prop('checked')) {
                        memberlvl += ',' + $(this).val();
                    }
                });
                if (memberlvl.length > 1) {
                    memberlvl = memberlvl.substring(1);
                }
                $('#memberDefualtGroupdiv').find('input').each(function () {
                    if ($(this).prop('checked')) {
                        defualtGroup += ',' + $(this).val();
                    }
                });
                if (defualtGroup.length > 1) {
                    defualtGroup = defualtGroup.substring(1);
                }
                $('#memberCustomGroupdiv').find('input').each(function () {
                    if ($(this).prop('checked')) {
                        customGroup += ',' + $(this).val();
                    }
                });
                if (customGroup.length > 1) {
                    customGroup = customGroup.substring(1);
                }
            }
            postData.HeadImgage = data;
            postData.FitMember = memberlvl;
            postData.CustomGroup = customGroup;
            postData.DefualtGroup = defualtGroup;
            if (postData.FitMember == "" && postData.DefualtGroup == "" && postData.CustomGroup == "") {

                HiTipsShow("适用会员未选择", "error");
                return;
            }
            $.ajax({
                type: "post",
                url: "AddOneyuanInfo.aspx",
                data: postData,
                async: false,
                dataType: "json",
                success: function (data) {

                    if (data.state) {
                        HiTipsShow(data.msg, "success", function () {
                            window.location.href = "OneyuanList.aspx"; //转到列表
                        });

                    } else {
                        HiTipsShow(data.msg, "error");
                        $(obj).attr("disabled", false); //禁止多次点击
                        $(obj).popover('hide');
                    };
                }
            });



        }


        var $IsShowErr = false;

         $(function ()
       // $(document).ready(function ()
        {
            initEditinfo();
            setValue();
            $("#StartTime,#EndTime").change(function () {
                var tid = $(this).attr("id");
                postData[tid] = $(this).val();
                setValue();
            });


            $("input[name=ReachType]").click(function () {
                postData.ReachType = $(this).val();
                setValue();

            });

            //图片显示控件，只显示，不上传
            $("#idFile").uploadPreview({
                Img: "idImg", Width: 100, Height: 100, Callback: function () {

                    //alert($("#idFile").val());

                    var bgurl = $("#idImg").attr("src");
                    if (bgurl != "") {
                        postData.HeadImgage = bgurl;
                        $('#idImgWarp').css({
                            "background": 'url(' + bgurl + ') no-repeat center',
                            'backgroundSize': 'cover'
                        });

                        setValue();
                    }
                }
            });


            var $leftTitle = $("#leftTitle");

            //同步显示活动标题
            $("#ATitle").keyup(function () {

                var v = $(this).val();

                if (v.trim() == "")
                    $leftTitle.text("活动标题未设置");
                else if (v.length > 20) {
                    if (!$IsShowErr) {
                        $IsShowErr = true;
                        HiTipsShow("标题文字不能超过20个字符！", "error", function () {
                            postData.Title = v.substring(0, 19);
                            $IsShowErr = false;
                        });
                    }

                    postData.Title = v.substring(0, 19);
                    $(this).val(postData.Title);
                    $leftTitle.text(postData.Title);
                }
                else {
                    $leftTitle.text(v);
                    postData.Title = v;

                }


            });

            $("#PrizeNum").keyup(function () {

                if (!/^\d+$/.test($(this).val())) {
                    HiTipsShow("请填数值！", "error");
                    $(this).val(postData.PrizeNumber);
                    return;
                }

                postData.PrizeNumber = $(this).val();
                setValue();
            });

            $("#EachCanBuyNum").keyup(function () {

                if (!/^\d+$/.test($(this).val())) {
                    HiTipsShow("请填数值！", "error");
                    $(this).val(postData.EachCanBuyNum);
                    return;
                }
                postData.EachCanBuyNum = $(this).val();
                setValue();
            });


            $("#EachPrice").keyup(function () {

                if (!/^\d+(\.)?(\d{1,2})?$/.test($(this).val())) {
                    HiTipsShow("请填数值,保留两位小数！", "error");
                    $(this).val(postData.EachPrice);
                    return;
                }

                postData.EachPrice = $(this).val();
                setValue();
            });


            $("#ReachNum").keyup(function () {

                if (!/^[1-9]\d{0,7}$/.test($(this).val())) {
                    HiTipsShow("请填大于0的数值！", "error");
                    $(this).val(postData.ReachNum);
                    return;
                }

                postData.ReachNum = $(this).val();
                
                setValue();
               
            });





            //$("#allmember").click(function () {
            //    //$(".FitMember").prop("checked", $(this).prop("checked"));

            //    if ($(this).prop("checked")) {
            //        var selMember = "0";
            //        var defualtGroup = "0";
            //        var customGroup = "0";
            //        postData.FitMember = selMember;
            //        postData.DefualtGroup = defualtGroup;
            //        postData.CustomGroup = customGroup;
            //    } else {
            //        postData.FitMember = "-1";
            //        postData.DefualtGroup = "-1";
            //        postData.CustomGroup = "-1";
            //    }
            //});

            //$(['name = "member"']).on("click", function () {
            //    alert(1)
            //    var selMember = "0";
            //    if ($(".Grade:checked").length == $(".Grade").length) {
            //        postData.FitMember = selMember;
            //        return;
            //    }
            //    $(".Grade:checked").each(function () {
            //        selMember += "," + $(this).val();
            //    });
            //    postData.FitMember = selMember.substring(2);


            //});
            //$(".DefualtGroup").click(function () {
            //    var defualtGroup = "0";
            //    if ($(".DefualtGroup:checked").length == $(".DefualtGroup").length) {
            //        postData.DefualtGroup = defualtGroup;
            //        return;
            //    }

            //    $(".DefualtGroup:checked").each(function () {
            //        defualtGroup += "," + $(this).val();
            //    })
            //    postData.DefualtGroup = defualtGroup.substring(2);
            //});
            //$(".CustomGroup").click(function () {
            //    var customGroup = "0";
            //    if ($(".CustomGroup:checked").length == $(".CustomGroup").length) {
            //        postData.CustomGroup = customGroup;
            //        return;
            //    }
            //    $(".CustomGroup:checked").each(function () {
            //        customGroup += "," + $(this).val();
            //    })
            //    postData.CustomGroup = customGroup.substring(2);
            //});
            $(".start_datetime").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm-dd hh:ii:ss',
                minView: 0, isadmin: true, isEnd: 0
            });


        });


        function initEditinfo() {
            if (postData.action == "save") return;
            $("#ATitle").val(postData.Title);
            $("#ADesc").val(postData.ActivityDec);
            $('#idImgWarp').css({
                "background": 'url(' + postData.HeadImgage + ') no-repeat center',
                'backgroundSize': 'cover'
            });

            var phtml = '  <div class="shop-img fl">' +
                     '  <img src="' + postData.ProductImg + '" width="60" style="height:60px!important" /></div>' +
                     '  <div class="shop-username fl ml10">' +
                     '   <p style="color:#222">' + postData.ProductTitle + '</p></div>' +
                         '  <p class="fl ml20">现价：￥' + postData.ProductPrice + '~￥' + postData.MaxPrice + '</p>' +
                     '  <p class="fl ml20">库存：' + postData.storeKc + '</p>';
            $("#productInfo").html(phtml);


            $("#PrizeNum").val(postData.PrizeNumber);
            $("#EachPrice").val(postData.EachPrice);
            $("#EachCanBuyNum").val(postData.EachCanBuyNum);
            $("#ReachNum").val(postData.ReachNum);
            $("#StartTime").val(postData.StartTime);
            $("#EndTime").val(postData.EndTime);

            $("input[name=ReachType][value=" + postData.ReachType + "]").prop("checked", true);


            //if (postData.FitMember == "0") {

            //    $("#FitMember0").prop("checked", true);
            //    $(".FitMember").prop("checked", true);
            //}
            //else {
            //    var selmember = postData.FitMember.split(",");
            //    $.each(selmember, function (i, value) {
            //        $("input[class=FitMember][type=checkbox][value='" + value + "']").prop("checked", true);
            //    });
            //}


            //if ($(".FitMember:checked").length == $(".FitMember").length)
            //    $("#FitMember0").prop("checked", true);


            setDisable();

        }

        function setDisable() {
            if (OneTaoState == "进行中") {
                $("input").prop("disabled", true);
                $("[name='member']").prop("disabled", true)
                $("#ATitle").prop("disabled", false);
                $("#ADesc").prop("disabled", false);
                $("#idFile").prop("disabled", false);
            }

            if (viewAid != "") {
                $("#returnDiv").hide();
                $("#_footerBtoom").hide();
                $("#DisableDiv").show();//给出透明层，阻止修改
            }
        }
       

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <form id="thisForm" runat="server" class="form-horizontal" enctype="multipart/form-data">
      <div class="page-header">
        <h2 id="txtEditInfo" runat="server">新增一元夺宝
        </h2>
    </div>

    <Hi:ViewTab ID="ViewTab1"  runat="server"></Hi:ViewTab>

    <div class="blank" id="returnDiv">
        <a href="OneyuanList.aspx" class="btn btn-primary btn-sm inputw100">&lt;&lt; 返回</a>
    </div>

    <div class="shop-navigation pb100 clearfix" style="position:relative">
         <div id="DisableDiv" style="display:none;width:100%;height:1100px;position:absolute;top:0px;left:0px;z-index:9999;"></div>
        <div class="fl">
            <div class="mobile-border">
                <div class="mobile-d">
                    <div class="mobile-header">
                        <i></i>
                        <div class="mobile-title">店铺主页</div>
                    </div>
                    <div class="set-overflow">
                        <div style="min-height: 350px;">
                            <div class="prize-picture" id="LeftTopImg" style="height:100px;background:url('http://fpoimg.com/320x100')">
                               
                            </div>

                            <div class="mobile-prize-textinfo">
                                <h3 id="leftTitle">活动标题</h3>
                                <p>奖品名称：<span id="LeftProductTitle">苹果（Apple）iPhone 6 Plus A1524 16Glus A1524 16G..</span></p>
                                <p>奖品说明：<span id="LeftProductDesc">数量10，每份价格￥10，限购5份</span></p>
                                <p>开奖方式：<span id="LeftPtype">未设置</span></p>
                                <p>距离结束：<span class="red" id="LeftDatatx">0天 0小时 0分 0秒</span></p>
                            </div>
                            <div class="y3-mobilebtn">
                                <button class="btn btn-primary btn-xs">分享</button>
                                <button class="btn btn-danger btn-xs">去看看</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear-line">
                    <div class="mobile-footer"></div>
                </div>
            </div>
        </div>
        <div class="fl frwidth">
            <div class="set-switch resetBorder">
                <p class="mb10 borderSolidB pb5"><strong>基本信息：</strong></p>
                <div class="form-horizontal clearfix">
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动标题：</label>
                        <div class="form-inline col-xs-9">
                            <input type="text" class="form-control resetSize inputw300" id="ATitle" placeholder="填写不多于20个字的标题">
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动封面：</label>
                        <div class="form-inline journal-query col-xs-9">
                                <div class="upFile clearfix">
                                    <div class="bgImg" id="idImgWarp">
                                        <img id="idImg" src=""  clientidmode="Static" runat="server" />
                                    </div>
                                    <div class="adds">
                                        <input id="idFile" name="idFile" type="file" />
                                    </div>
                                </div>
                            <small>点击上传，建议尺寸：600 x 200 像素，小于300KB</small>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-xs-3 pad resetSize control-label" for="setdate">活动说明：</label>
                        <div class="form-inline journal-query col-xs-9">
                            <div class="form-group">
                                <textarea class="form-control inputtext" id="ADesc" placeholder="作为分享出去的主要内容"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="set-switch resetBorder">
                <p class="mb10 borderSolidB pb5"><strong>奖项设置：</strong></p>
                <div class="form-horizontal clearfix">
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;选择奖品：</label>
                        <div class="form-inline col-xs-9" style="margin-top:2px" >
                            <a href="javascript:ShowProduct()" >点击选择</a>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername">&nbsp;&nbsp;</label>
                        <div class="form-inline col-xs-9">
                            <div class="y3-prize-info clearfix" id="productInfo">
                                夺宝商品尚未设置，请选择夺宝商品！
                            </div>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;奖品数量：</label>
                        <div class="form-inline col-xs-9">
                            <input type="text" class="form-control resetSize inputw100" id="PrizeNum" value="1" maxlength="8">
                            <span class="colorc">设置夺宝商品开奖时，发放的商品总数量</span>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;夺宝定价：</label>
                        <div class="form-inline col-xs-9">
                            <input type="text" class="form-control resetSize inputw100" id="EachPrice" value="1"  maxlength="8">&nbsp;元
                                        <span class="colorc">设置夺宝商品购买其中一份的价格</span>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;每人限购：</label>
                        <div class="form-inline col-xs-9">
                            <input type="text" class="form-control resetSize inputw100" id="EachCanBuyNum" value="1"   maxlength="8">&nbsp;份
                                        <span class="colorc">该商品单个会员最大购买份数</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="set-switch resetBorder">
                <p class="mb10 borderSolidB pb5"><strong>活动设置：</strong></p>
                <div class="form-horizontal clearfix">
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;适用会员：</label>
                        <div class="form-inline col-xs-9">
                            <Hi:SetMemberRange runat="server" ID="SetMemberRange" />
                           <%-- <div class="resetradio mb5 pt3">
                                <label>
                                    <input type="checkbox" id="FitMember0" value="0">全部会员</label>
                            </div>
                            <div class="resetradio mb5 pt3" id="FitMemberList" runat="server">
                               
                            </div>--%>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动时间：</label>
                        <div class="form-inline journal-query col-xs-9">
                            <div class="form-group">
                                <input type="text" class="form-control resetSize start_datetime" id="StartTime" placeholder="开始日期">&nbsp;&nbsp;至&nbsp;
                                            <input type="text" class="form-control resetSize start_datetime" id="EndTime" placeholder="结束日期">
                            </div>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label"><em>*</em>&nbsp;&nbsp;满足份数：</label>
                        <div class="form-inline col-xs-9">
                            <input type="text" class="form-control resetSize inputw100" id="ReachNum"   maxlength="8">
                            <span class="colorc">设置夺宝商品最少被购买多少份才能开奖</span>
                        </div>
                    </div>
                    <div class="form-group setmargin">
                        <label class="col-xs-3 pad resetSize control-label"><em>*</em>&nbsp;&nbsp;开奖方式：</label>
                        <div class="form-inline col-xs-9">
                            <div class="resetradio mb5 pt3">
                                <label class="mr20">
                                    <input type="radio" name="ReachType" value="1">达到满足份数开奖</label>
                                <small>在结束时间前，达到满足份数后自动开奖。</small>
                            </div>
                            <div class="resetradio mb5 pt3">
                                <label class="mr20">
                                    <input type="radio" name="ReachType"  value="2">到结束时间开奖</label>
                                <small>在结束时间前，可以一直购买，达到结束时间后就自动开奖。</small>
                            </div>
                            <div class="resetradio mb5 pt3">
                                <label class="mr20">
                                    <input type="radio" name="ReachType" value="3">到结束时间并达到满足份数开奖</label>
                                <small>在结束时间前，可以一直购买，达到结束时间且达到满足份数后自动开奖。
                                </small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="footer-btn navbar-fixed-bottom" id="_footerBtoom">
        <button type="button"  onclick="saveinfo(this)"
            data-container="body"  id="saveBtn"
            title="数据处理中..." 
            data-toggle="popover" data-placement="top" data-content="正在上传图片，保存数据，请稍后"
             class="btn btn-primary">保存</button>
    </div>
       </form>
    
 
</asp:Content>
