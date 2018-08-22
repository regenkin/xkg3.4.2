<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCGameInfo.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.promotion.UCGameInfo" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagPrefix="Hi" TagName="SetMemberRange" %>


<script src="../js/ZeroClipboard.min.js"></script>
<link rel="stylesheet" href="../css/bootstrapSwitch.css" />
<script type="text/javascript" src="../js/bootstrapSwitch1.js"></script>
<script type="text/javascript">
    $(function () {
        $('body').css('background', 'none');
        $('.container').css('padding', 0);
        $('.page-header h2').css('fontSize', '20px');
        $("#allMember").on("change", function () {
            var allRadio = $(this).parents('.resetradio').next().find('input');
            if (this.checked) {
                allRadio.prop('checked', true).attr('disabled', true);
            } else {
                allRadio.prop('checked', false).attr('disabled', false);
            }
        });
        $('.allradio input').change(function () {
            if ($('.allradio input:checked').length == 3) {
                $("#allMember").prop('checked', true);
            } else {
                $("#allMember").prop('checked', false);
            }
        })
        $("#<%= dateBeginTime.ClientID%>_txtDateTimePicker").change(function () {
            IsHaveValue(this);
            $("#sBeginTime").html($(this).val());
        });
        $("#<%= dateEndTime.ClientID%>_txtDateTimePicker").change(function () {
            IsHaveValue(this);
            $("#sEedTime").html($(this).val());
        });
        $("#<%= txtDescription.ClientID %>").change(function () {
            $("#pGameDescription").html($(this).val().replace(/\n/g, "<br/>"));
        });
        var GameNum = $("#<%= txtLimitEveryDay.ClientID %>").val();
        if (GameNum > 0) {
            $("#DivGameNum").show();
            $("#ctl00_ContentPlaceHolder1_lbGameNum").html(GameNum.replace(/\n/g, "<br/>"));
        }
        else {
            $("#DivGameNum").hide();
        }
        $("#<%= txtLimitEveryDay.ClientID %>").change(function () {
            if ($("#<%= txtLimitEveryDay.ClientID %>").val() != "0") {
                $("#DivGameNum").show();
                $("#ctl00_ContentPlaceHolder1_lbGameNum").html($(this).val().replace(/\n/g, "<br/>"));
            }
            else {
                $("#DivGameNum").hide();
            }
        });
        SetBulr();
        var copy = new ZeroClipboard(document.getElementById("copybutton"), {
            moviePath: "../js/ZeroClipboard.swf"
        });
        copy.on('complete', function (client, args) {
            HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
        });
        <% if (Hidistro.Core.Globals.RequestQueryStr("action") == "deital") {%>
        $("input,textarea").attr("disabled", "false");
        <% }%>
    })
    function BeforeSave_Game() {
        var name = $("[id$=txtGameTitle]").val().trim();
        if (name == '') {
            ShowMsg("请输入活动名称！")
            return;
        }
        var beginTime = $("#<%= dateBeginTime.ClientID%>_txtDateTimePicker").val();
        if (beginTime == '') {
            ShowMsg("请输入活动时间开始时间！")
            return;
        }
        var endTime = $("#<%= dateEndTime.ClientID%>_txtDateTimePicker").val();
        if (endTime == '') {
            ShowMsg("请输入活动时间结束时间！")
            return;
        }
        if ($('#memberdiv input:checked').length == 0) {
            ShowMsg("请选择适用会员！")
            return;
        }
        var needPoint = $("[id$=txtNeedPoint]").val();
        if (needPoint == '') {
            ShowMsg("请输入消耗积分！");
            return;
        }
        if (!IsNum(needPoint)) {
            ShowMsg("输入消耗积分必须为大于或等于0的整数！");
            return;
        }

        var givePoint = $("[id$=txtGivePoint]").val();
        if (givePoint == '') {
            ShowMsg("请输入参与送积分！");
            return;
        }
        if (!IsNum(givePoint)) {
            ShowMsg("输入参与送积分必须为大于或等于0的整数！");
            return;
        }
        var limitEveryDay = $("#ctl00_ContentPlaceHolder1_UCGameInfo_txtLimitEveryDay").val();
        if (limitEveryDay == '') {
            ShowMsg("请输入每人每天限次！");
            return;
        }
        if (!IsNum(limitEveryDay)) {
            ShowMsg("输入每人每天限次必须为大于或等于0的整数！");
            return;
        }

        var maximumDailyLimit = $("#ctl00_ContentPlaceHolder1_UCGameInfo_txtMaximumDailyLimit").val();
        if (maximumDailyLimit == '') {
            ShowMsg("请输入每人最多限次！");
            return;
        }
        if (!IsNum(maximumDailyLimit)) {
            ShowMsg("输入每人最多限次必须为大于或等于0的整数！");
            return;
        }
        var index = $("#tabGameMenu li").length;

        //$("#sPrizeGade0").html("一等奖：赠送积分");
        //$("#sPrizeGade1").html("二等奖：赠送积分");
        //$("#sPrizeGade2").html("三等奖：赠送积分");
        //$("#sPrizeGade3").html("四等奖：赠送积分");
        $("#step1").hide();
        $("#step2").show();
        var value = $("#ctl00_ContentPlaceHolder1_UCGamePrizeInfo_txtPrizeRate").val();
        if (value == "" && index == 1) {
            $("#sPrizeGade0").html("一等奖：赠送积分");
        }

    }
    function IsNum(str) {
        var re = /^[0-9]*]*$/
        return re.test(str);
    }
    function SetBulr() {
        $("#<%=txtGameTitle.ClientID %>").blur(function () {
            IsHaveValue(this);
        });
        $("#<%=this.dateBeginTime.ClientID %>_txtDateTimePicker").blur(function () {
            IsHaveValue(this);
        });
        $("#<%=this.dateEndTime.ClientID %>_txtDateTimePicker").blur(function () {
            IsHaveValue(this);
        });
        $("#<%=this.txtNeedPoint.ClientID %>").blur(function () {
            IsHaveValue(this);
        });
        $("#<%=this.txtGivePoint.ClientID %>").blur(function () {
            IsHaveValue(this);
        });

    }
    ///判断是否有值
    function IsHaveValue(obj) {
        var value = $(obj).val();
        if (value != '') {
            $(obj).parent().removeClass("has-error");
        } else {
            $(obj).parent().addClass("has-error");
        }
    }

    function CopyUrl(obj) {
        var clip = new ZeroClipboard(obj, {
            moviePath: "../js/ZeroClipboard.swf"
        });

    }
    function winqrcode() {
        var url = $("[id$=txtGameUrl]").val();
        $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + encodeURIComponent(url).replace("&", "%26"));
    }
</script>
<div id="step1" class="fl frwidth marTop">
    <div class="set-switch resetBorder">
        <p class="mb10 borderSolidB pb5"><strong>基本信息：</strong></p>
        <div class="form-horizontal clearfix">
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动名称：</label>
                <div class="form-inline col-xs-9">
                    <asp:TextBox runat="server" class="form-control resetSize" MaxLength="20" ID="txtGameTitle"></asp:TextBox>
                    <asp:HiddenField ID="hfGameId" runat="server" />
                </div>
            </div>
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动时间：</label>
                <div class="form-inline journal-query col-xs-9">
                    <div class="form-group">
                        <Hi:DateTimePicker runat="server" name="canTest" CssClass="form-control resetSize"
                            ID="dateBeginTime" DateFormat="yyyy-MM-dd HH:mm:ss" />
                        <label>至</label>
                        <Hi:DateTimePicker runat="server" name="canTest" CssClass="form-control resetSize"
                            ID="dateEndTime" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-3 pad resetSize control-label" for="setdate">活动说明：</label>
                <div class="form-inline journal-query col-xs-9">
                    <div class="form-group">
                        <asp:TextBox runat="server" TextMode="MultiLine" class="form-control inputtext" ID="txtDescription" MaxLength="80"></asp:TextBox>
                    </div>
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


                    <Hi:SetMemberRange runat="server" ID="memberRange" />
                </div>
            </div>
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" style="padding-top: 8px;"><em>*</em>&nbsp;&nbsp;消耗积分：</label>
                <div class="form-inline col-xs-9">
                    <asp:TextBox runat="server" name="canTest" class="form-control resetSize" ID="txtNeedPoint"
                        Text="0" MaxLength="10"></asp:TextBox>
                    积分/次
                     <small>用户每次参与活动需要消耗积分，值为0时不消耗</small>
                </div>
            </div>
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" style="padding-top: 8px;"><em>*</em>&nbsp;&nbsp;参与送积分：</label>
                <div class="form-inline col-xs-9">
                    <asp:TextBox runat="server" name="canTest" class="form-control resetSize" ID="txtGivePoint" MaxLength="10" Text="0"></asp:TextBox>积分/次
                      <label class="ml10 resetradio">
                          <asp:CheckBox ID="cbOnlyGiveNotPrizeMember" runat="server" />仅送给未中奖的用户</label>
                    <small>值为0时不赠送积分</small>
                </div>
            </div>
            <%-- <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label"><em>*</em>&nbsp;&nbsp;每人参与次数：</label>
                <div class="form-inline col-xs-9">
                    <div class="resetradio mb5 pt3">
                        <label class="mr20">
                            <asp:RadioButton ID="rbdPlayType1" GroupName="playType" runat="server" Text="一次" />
                        </label>
                        <label class="mr20">
                            <asp:RadioButton ID="rbdPlayType3" GroupName="playType" runat="server" Text="两次" /></label>
                        <label class="mr20">
                            <asp:RadioButton ID="rbdPlayType0" GroupName="playType" Checked="true" runat="server"
                                Text="一天一次" />
                        </label>                      
                        <label class="mr20">
                            <asp:RadioButton ID="rbdPlayType2" GroupName="playType" runat="server" Text="一天两次" />
                        </label>                     
                    </div>
                </div>
            </div>--%>
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" style="padding-top:8px;"><em>*</em>&nbsp;&nbsp;参与次数：</label>
                <div class="form-inline col-xs-9">
                    <div class="resetradio mb5 pt3">
                        每人每天限<asp:TextBox runat="server" name="txtLimitEveryDay" class="form-control" Width="50px" Height="27px" ID="txtLimitEveryDay" MaxLength="4" Text="1"></asp:TextBox>次 <small>0为不限制 </small>
                        <br />
                        每人最多限<asp:TextBox runat="server" name="txtMaximumDailyLimit" class="form-control" Width="50px" Height="27px" ID="txtMaximumDailyLimit" MaxLength="4" Text="0"></asp:TextBox>次    <small>0为不限制 </small>
                    </div>
                </div>
            </div>
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label" style="padding-top: 12px;"><em>*</em>&nbsp;&nbsp;会员信息校验：</label>
                <div class="form-inline col-xs-9">
                    <div class="resetradio mb5 pt3">
                        <div class="switch1 fl" id="mySwitch">
                            <input type="checkbox" name="MemberCheck" id="MemberCheck" <%= memberCheck?"checked":"" %> />
                        </div>
                        <div class="mt9"> 会员参与活动是否需要绑定手机号</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="footer-btn navbar-fixed-bottom">
        <button type="button" class="btn btn-primary" onclick="BeforeSave_Game()">下一步</button>
    </div>
</div>

<div id="step3" style="display: none;" class="fl frwidth marTop">
    <div class="set-switch resetBorder">
        <p class="mb10 borderSolidB pb5 pageNumber"><strong>您已成功创建该活动！</strong></p>
        <div class="form-horizontal clearfix">
            <div class="form-group setmargin">
                <label class="col-xs-3 pad resetSize control-label">链接地址：</label>
                <div class="form-group col-xs-7">
                    <asp:HiddenField ID="hfKeyWord" runat="server" />
                    <asp:TextBox ID="txtGameUrl" runat="server" CssClass="form-control resetSize"></asp:TextBox>
                    <small>复制该链接给你的粉丝</small>
                </div>
                <div class="form-inline journal-query col-xs-2">

                    <button type="button" id="copybutton" class="btn resetSize btn-primary" onclick="CopyUrl(this)"
                        data-clipboard-target="<%=txtGameUrl.ClientID %>" data-clipboard-text="copy">
                        复制</button>

                </div>
            </div>
            <div class="form-group setmargin give giveint">
                <label class="col-xs-3 pad resetSize control-label" for="pausername">活动二维码：</label>
                <div class="form-inline col-xs-9">
                    <img src="http://fpoimg.com/150x150" id="imagecode">
                </div>
            </div>
        </div>
    </div>
</div>
