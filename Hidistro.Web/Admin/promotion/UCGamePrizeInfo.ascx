<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCGamePrizeInfo.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.promotion.UCGamePrizeInfo" %>
<link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
<link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">

<!-- diy css start-->
<%--<link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">--%>

<link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">

<script src="/Admin/shop/Public/js/dist/underscore.js"></script>
<script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
<%--<script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>--%>
<script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
<script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
<script src="/Admin/shop/Public/js/config.js"></script>

<script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>

<script src="/Admin/js/HiShopComPlugin.js"></script>
<script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
<script src="/Admin/js/weiboHelper.js"></script>
<script type="text/javascript">
    $(function () {
        $('.setTab .tabContent').eq(0).show();
        //$('.setTab ul.navTab li').on("click", function () {
        //    $('.setTab ul.navTab li').removeClass('active');
        //    $(this).addClass('active');
        //    $('.setTab .tabContent').hide();
        //    $('.setTab .tabContent').eq($(this).index()).show();
        //});
        $('#setTab').on('click', 'li', function () {
            $('.setTab ul.navTab li').removeClass('active');
            $(this).addClass('active');
            $('.setTab .tabContent').hide();
            $('.setTab .tabContent').eq($(this).index()).show();
        })
        SelectPrize();
        $('#setTab').on('click', '.selectradio input', function () {

            $(this).parents('.tabContent').find('.give').hide();
            var index = $(this)[0].id.substring(2, 3);
            var indexValue = '';
            switch (index) {
                case "0":
                    {
                        indexValue = '一等奖：';
                        break;
                    }
                case "1":
                    {
                        indexValue = '二等奖：';
                        break;
                    }
                case "2":
                    {
                        indexValue = '三等奖：';
                        break;
                    }
                case "3":
                    {
                        indexValue = '四等奖：';
                        break;
                    }
            }
            switch ($(this).val()) {
                case '0':
                    $("#sPrizeGade" + index).html(indexValue + "赠送积分");
                    $(this).parents('.tabContent').find('.giveint').show();
                    break;
                case '1':
                    $("#sPrizeGade" + index).html(indexValue + "赠送优惠券");
                    $(this).parents('.tabContent').find('.givecop').show();
                    break;
                case '2':
                    $("#sPrizeGade" + index).html(indexValue + "赠送商品");
                    $(this).parents('.tabContent').find('.giveshop').show();
                    break;
                case '3':
                    $("#sPrizeGade" + index).html(indexValue + "其他商品");
                    $(this).parents('.tabContent').find('.other').show();
                    break;
                default:
                    return false;
            }
        })
        addLastDelBtn();
    });

    function ShowStep1() {
        $("#step1").show();
        $("#step2").hide();
    }


    function SelectPrize() {
        for (var i = 0; i < 4; i++) {
            switch ($("#prizeTypeValue" + i).val()) {
                case '0':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').show();
                    break;
                case '1':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.givecop').show();
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').hide();
                    break;
                case '2':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveshop').show();
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').hide();
                    break;
                case '3':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.other').show();
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').hide();
                    break;
                default:
                    return false;
            }
        }
    }

    function SelectPrizeImage(index) {
        HiShop.popbox.ImgPicker(function (obj) {
            BindPicData(obj, index);
        });
    }

    function BindPicData(img, index) {
        $("#PrizeImage" + index).attr("src", img[0]);
        $("#hiddPrizeImage" + index).attr("value", img);
    }

    function SelectShopBookId(index) {
        HiShop.popbox.GoodsAndGroupPicker("goods", function (list) {
            $("#imgProduct" + index).attr("src", list.pic);
            $("#txtShopbookId" + index).val(list.item_id);
            $("#txtProductPic" + index).val(list.pic);
        });
    }

    function addLastDelBtn() {
        $("#tabGameMenu li i").removeClass("glyphicon").removeClass("glyphicon-remove");
        $("#divAddPrize").prev().find("i").addClass("glyphicon glyphicon-remove");
        $("#divAddPrize").prev().click();
    }
    ///删除奖项
    function DelPrize(obj) {
        var prizeId = $(obj).parent().attr("lival");
        if (prizeId == undefined) {
            var tempid = $(obj).parent().attr("tempidval");
            $("#tempid" + tempid).remove();
            $("#tempContentId" + tempid).remove();
            addLastDelBtn();
        } else {
            var gameId = $("#ctl00_ContentPlaceHolder1_UCGameInfo_hfGameId").val();
            $.ajax({
                type: "post",
                url: "PromotionGamePrizesHandler.ashx?action=DeletePrize&gameId=" + gameId + "&prizeId=" + prizeId,
                dataType: "json",
                success: function (data) {
                    if (data.type == "ok") {
                        //window.location.reload();
                        $("#li" + prizeId).remove();
                        $("#div" + prizeId).remove();
                        addLastDelBtn();
                    }
                    else {
                        ShowMsg(data.message)
                    }
                }
            });
        }
    }

    function AddPrize(index) {
        var gameId = $("#ctl00_ContentPlaceHolder1_UCGameInfo_hfGameId").val();
        $.ajax({
            type: "post",
            url: "PromotionGamePrizesHandler.ashx?action=AddPrize&gameId=" + gameId + "&index=" + index,
            dataType: "json",
            success: function (data) {
                if (data.type == "ok") {
                    $(".tabContent:last").after(data.message);
                    $(".tabContent:last").attr("style", "display: block;");
                    var str="";
                    switch (index) {
                        case 0:
                            str = "一等奖";
                            break;
                        case 1:
                            str = "二等奖";
                            break;
                        case 2:
                            str = "三等奖";
                            break;
                        case 3:
                            str = "四等奖";
                            break;
                        default:
                            return;
                    }
                    $("#sPrizeGade" + index).html(str + "：赠送积分");
                    addLastDelBtn();
                }
            }
        });
    }

    $(function () {
        $("#addPrize").on("click", function () {
            var index = $("#tabGameMenu li").length;
            var gameName = "";
            switch (index) {
                case 0:
                    gameName = "一等奖";
                    break;
                case 1:
                    gameName = "二等奖";
                    break;
                case 2:
                    gameName = "三等奖";
                    break;
                case 3:
                    gameName = "四等奖";
                    break;
                default:
                    ShowMsg("最多只能添加4个奖项！");
                    return;
            }
            $('.setTab ul.navTab li').removeClass('active');
            $('.setTab .tabContent').hide();
            var prizeHtml = "<li class=\"active\" id=\"tempid" + index + "\" tempidval=\"" + index + "\">" + gameName + "<i class='' onclick='DelPrize(this)'></i></li>";
            $("#tabGameMenu li:last").after(prizeHtml);
            AddPrize(index);
        })
        InitTextCounter(50, "#ctl00_ContentPlaceHolder1_UCGamePrizeInfo_txtNotPrzeDescription", null);
        $("#txtPrize1,#txtPrize2,#txtPrize3,#txtPrize0").attr("title", "文字长度请勿超出输入框宽度");
    });
</script>
<style type="text/css">
    .goodsearch {
        margin-left: 100px;
        padding: 0 10px;
    }

        .goodsearch input {
            display: inline-block;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            border: 1px solid #ddd;
            width: 350px;
            height: 30px;
            line-height: 30px;
            vertical-align: 0px;
            padding: 0 5px;
            margin: 0 10px;
        }

    .FloatCenter {
        width: 100%;
        height: 27px;
        padding: 6px 12px;
        font-size: 14px;
        line-height: 1.42857143;
        color: #555;
        background-color: #fff;
        background-image: none;
        border: 1px solid #ccc;
        border-radius: 4px;
        -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
        box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
        -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
        -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    }

    .setTab .navTab li {
        position: relative;
    }

        .setTab .navTab li i {
            position: absolute;
            top: 0;
            right: 0;
            display: none;
        }

        .setTab .navTab li:hover i {
            display: inline-block;
        }
        #tabGameMenu li{border-bottom:none;}
        
    #txtPrize1,#txtPrize2,#txtPrize3,#txtPrize0{width: 126px;}
    .setTab .tabContent{
        padding-top:30px;
    }
</style>
<div class="set-switch resetBorder">
    <div class="setTab">
        <asp:HiddenField ID="hfGameId" runat="server" />
        <asp:HiddenField ID="hfIndex" runat="server" Value="0" />
        <div class="form-horizontal clearfix">
            <div class="form-group" style="text-align: center;">
                <h5 class="winningResults mb20"><strong>中奖概率</strong></h5>
                综合中奖率：<asp:TextBox runat="server" ID="txtPrizeRate" class="FloatCenter" Width="70px" MaxLength="6"></asp:TextBox>%
              <br />
                <small>活动有效期内，中奖概率保持不变</small>
            </div>
        </div>
        <div class="form-horizontal clearfix">
            <div class="form-group" style="text-align: center">
                <h5 class="winningResults mb20"><strong>设置奖品</strong></h5>
                <small>等级设置的奖品数量越多，则该等级中奖率越高。<br />
                    例如：设置一等奖 10个，二等奖20个，则中二等奖概率高于一等奖</small>


            </div>
        </div>
    </div>
    <div class="setTab" id="setTab">
        <ul class="navTab clearfix" id="tabGameMenu" style="width:609px;border:1px solid #ccc; border-top:none;border-right:none; margin-top:10px">
            <%=GetGameMenu() %>
            <div style="float: left; padding-right: 10px;" id="divAddPrize">
                <input type="button" id="addPrize" style="width: 100px; height: 26px;margin-top:2px;" class="btnLink pad" value="+添加奖项" />
            </div>
        </ul>
        <!--奖品信息-->
        <%=PrizeInfoHtml() %>
        <h5 class="winningResults mb20"><strong>中奖结果说明</strong></h5>
        <div class="form-horizontal clearfix">
            <div class="form-group">
                <label class="col-xs-3 pad resetSize control-label" for="setdate">未中奖说明：</label>
                <div class="form-inline journal-query col-xs-9">
                    <div class="form-group">

                        <asp:TextBox runat="server" TextMode="MultiLine" class="form-control inputtext"
                            ID="txtNotPrzeDescription" Text="谢谢参与!" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
