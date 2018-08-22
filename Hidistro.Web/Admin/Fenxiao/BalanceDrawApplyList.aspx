<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BalanceDrawApplyList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.BalanceDrawApplyList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.ControlPanel.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #selrows {
        }

        .selrows li {
            border: 1px solid #ccc;
            padding: 3px 10px 3px 10px;
            float: left;
            margin: 3px;
            width: 150px;
            overflow-x: hidden;
        }

            .selrows li span:first-child {
                float: right;
                display: inline-block;
                /*width: 50px;*/
                overflow-x: hidden;
            }

            .selrows li span:last-child {
                color: #d35e0b;
            }

        .batchtxt {
            padding: 0px 0px 10px 50px;
        }

        .errRow {
            padding: 20px;
            font-weight: normal;
            color: #555;
        }
    </style>
    <script>

        var DrawPayTypeSet = "<%=DrawPayType%>"; //提现可支持的方式

        $(function () {
            $('.allselect').change(function () {
                $('.content-table input[type="checkbox"]').prop('checked', $(this)[0].checked);
            });


            var tableTitle = $('.title-table').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.title-table').css({
                        position: 'fixed',
                        top: '58px'
                    })
                }
                if ($(document).scrollTop() + $('.title-table').height() + 58 <= tableTitle) {
                    $('.title-table').removeAttr('style');
                }
            });

            $("#RefuseShow").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_Button2',
                'ctl00$ContentPlaceHolder1$RefuseMks': {
                    validators: {
                        notEmpty: {
                            message: '请填写驳回理由'
                        }
                    }
                }
            });

            $("#RefuseSignalShow").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_Button3',
                'ctl00$ContentPlaceHolder1$SignalrefuseMks': {
                    validators: {
                        notEmpty: {
                            message: '请填写驳回理由'
                        }
                    }
                }
            });

            $("#payError").click(function () {

                $('#PayWait').modal("toggle");

            });

            $("#paySuccessAll").click(function () {
                window.location.href = window.location.href; //刷新页面
            });

            //bankPayShow
            $("#bankPayShow").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_BankPaySave',
                'ctl00$ContentPlaceHolder1$bankPayNum': {
                    validators: {
                        notEmpty: {
                            message: '流水号不能为空'
                        },
                        stringLength: {
                            min: 6,
                            max: 30,
                            message: '请填写准确的流水号！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$bankPayDate': {
                    validators: {
                        notEmpty: {
                            message: '支付日期不能为空'
                        },
                        regexp: {
                            regexp: /^(\d{4})\-(\d{2})\-(\d{2}) (\d{2}):(\d{2})$/,
                            message: '请填写正确的日期格式'
                        }
                    }
                }
            });


            $('#ctl00_ContentPlaceHolder1_bankPayDate').datetimepicker({
                format: 'yyyy-mm-dd hh:ii'
            }).on('changeDate', function () {
                $(this).trigger("blur"); //触发验证事件;
            });




        });


        function ShowGrade() {

            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                HiTipsShow("请先选择要批量审核的分销商！", "error")
                return;
            }

            var selhtml = "";
            $('.content-table input[type="checkbox"]:checked').each(function () {

                var rowParentTd = $(this).parents("tr").children("td");
                var ts = rowParentTd.eq(1).html().split("\<br");
                ts[0] = ts[0].replace(/&nbsp;/g, "").trim();

                selhtml += "<li><span>" + ts[0] + "</span><span>" + rowParentTd.eq(3).text() + "</span></li>";
            });


            $("#selPass").html(selhtml + "<div  style='clear:both'></div>");

            // <li><span>-</span><span>￥100.00　</span></li>

            $('#GradeShow').modal('toggle').children().css({
                width: '500px', top: "170px"
            });

        }

        function ShowRefuse() {

            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                HiTipsShow("请先选择要批量审核的分销商！", "error")
                return;
            }

            var selhtml = "";
            $('.content-table input[type="checkbox"]:checked').each(function () {

                var rowParentTd = $(this).parents("tr").children("td");
                var ts = rowParentTd.eq(1).html().split("\<br");
                ts[0] = ts[0].replace(/&nbsp;/g, "").trim();

                selhtml += "<li><span>" + ts[0] + "</span><span>" + rowParentTd.eq(3).text() + "</span></li>";
            });


            $("#selRefuse").html(selhtml + "<div  style='clear:both'></div>");

            // <li><span>-</span><span>￥100.00　</span></li>

            $('#RefuseShow').modal('toggle').children().css({
                width: '500px', top: "170px"
            });

        }

        function ShowRefuseSignalShow(sid) {
            $('#RefuseSignalShow').modal('toggle').children().css({
                width: '500px', top: "170px"
            });
            $("#ctl00_ContentPlaceHolder1_hSerialID").val(sid);
            // alert($("#ctl00_ContentPlaceHolder1_hSerialID").val())
            // return false;
        }


        //

        function ShowPassSignal(obj) {

            try {
                var IsCheck = $(obj).attr("IsCheck");
                var rowParentTd = $(obj).parents("tr").children("td");
                var ReqDate = rowParentTd.eq(2).html();
                var ReqPayType = rowParentTd.eq(4).html();
                var ReqPayName = rowParentTd.eq(6).html();
                var ReqMerchantCode = rowParentTd.eq(5).text().trim();
                var ReqSum = rowParentTd.eq(3).html();
                var sid = $(obj).attr("title");
                var Userid = rowParentTd.eq(5).attr("Userid")
                var RequestType = rowParentTd.eq(4).attr("title");
                ReqSum = ReqSum.replace("￥", "").trim();

                if (IsCheck == 0) {
                    //审核

                    $("#ctl00_ContentPlaceHolder1_HSid").val(sid);
                    $('#PassSignalShow').modal('toggle').children().css({
                        width: '400px', top: "130px"
                    });

                    $("#ReqDate").html(ReqDate);
                    $("#ReqPayType").html(ReqPayType);
                    $("#ReqPayName").html(ReqPayName);
                    $("#ReqMerchantCode").html(ReqMerchantCode);
                    $("#ctl00_ContentPlaceHolder1_ReqSum").val(ReqSum);

                }
                else if (IsCheck == 1) {
                    //提现RequestType 0微信，1淘宝，2打款
                    // HiTipsShow("暂未有定义", "error");

                    if (RequestType == 0) {

                        //微信支付(非红包)
                        $('#weiPayShow').modal('toggle').children().css({
                            width: '400px', top: "130px"
                        });

                        $("#ctl00_ContentPlaceHolder1_hduserid").val(Userid);
                        $("#ctl00_ContentPlaceHolder1_HiddenSid").val(sid);
                        $("#ctl00_ContentPlaceHolder1_hdreferralblance").val(ReqSum)

                        $("#ReqDate3").html(ReqDate);
                        $("#ReqPayType3").html(ReqPayType);
                        $("#ReqPayName3").html(ReqPayName);
                        $("#ReqMerchantCode3").html(ReqMerchantCode);
                        $("#ReqAmount3").html(ReqSum);




                    } else if (RequestType == 1) {

                        //支付宝支付

                        $('#aliPayShow').modal('toggle').children().css({
                            width: '400px', top: "130px"
                        });

                        $("#ctl00_ContentPlaceHolder1_hduserid").val(Userid);
                        $("#ctl00_ContentPlaceHolder1_HiddenSid").val(sid);
                        $("#ctl00_ContentPlaceHolder1_hdreferralblance").val(ReqSum)

                        $("#ReqDate4").html(ReqDate);
                        $("#ReqPayType4").html(ReqPayType);
                        $("#ReqPayName4").html(ReqPayName);
                        $("#ReqMerchantCode4").html(ReqMerchantCode);
                        $("#ReqAmount4").html(ReqSum);

                        $("#AliPayRealName").val(ReqPayName);
                        $("#AliPayUser").val(ReqMerchantCode);

                    }
                    else if (RequestType == 2) {

                        //网银支付

                        $('#bankPayShow').modal('toggle').children().css({
                            width: '400px', top: "130px"
                        });

                        $("#ctl00_ContentPlaceHolder1_hduserid").val(Userid);
                        $("#ctl00_ContentPlaceHolder1_HiddenSid").val(sid);
                        $("#ctl00_ContentPlaceHolder1_hdreferralblance").val(ReqSum)

                        $("#ReqDate1").html(ReqDate);
                        $("#ReqPayType1").html(ReqPayType);
                        $("#ReqPayName1").html(ReqPayName);
                        $("#ReqMerchantCode1").html(ReqMerchantCode);
                        $("#ReqAmount").html(ReqSum);




                    }
                    else if (RequestType == 3) {
                        //微信红包支付
                        $('#weiRedShow').modal('toggle').children().css({
                            width: '400px', top: "130px"
                        });

                        $("#ctl00_ContentPlaceHolder1_hduserid").val(Userid);
                        $("#ctl00_ContentPlaceHolder1_HiddenSid").val(sid);
                        $("#ctl00_ContentPlaceHolder1_hdreferralblance").val(ReqSum)

                        $("#ReqDate2").html(ReqDate);
                        $("#ReqPayType2").html(ReqPayType);
                        $("#ReqPayName2").html(ReqPayName);
                        $("#ReqMerchantCode2").html(ReqMerchantCode);
                        $("#ReqAmount2").html(ReqSum);



                    }
                    else {
                        HiTipsShow("暂未有定义", "error");
                    }
                }
            }
            catch (e) {

            }
            return false;
        }

        function checkAlipay() {

            $('#aliPayShow').modal('toggle');
            $('#PayWait').modal({ backdrop: 'static' }).children().css({
                width: '400px'
            });

            var paydata = $("#ctl00_ContentPlaceHolder1_HiddenSid").val() + "^" + $("#AliPayUser").val().trim().replace("&nbsp;", "") + "^" + $("#AliPayRealName").val().trim().replace("&nbsp;", "");
            paydata += "^" + $("#ctl00_ContentPlaceHolder1_hdreferralblance").val() + "^佣金提现";

            var jsondata = { Paydata: paydata };
            _postpgnow("/Admin/OutPay/alipayOut.aspx", jsondata);
            return false;

        }


        function checkBatchAlipay() {

            if ($("#ctl00_ContentPlaceHolder1_PaybatchType").val() != "1") {
                return true;  // 如果非支付宝支付，直接返回
            }


            $('#BatchPay').modal('toggle');
            $('#PayWait').modal({ backdrop: 'static' }).children().css({
                width: '400px'
            });

            //var paydata = $("#ctl00_ContentPlaceHolder1_HiddenSid").val() + "^" + $("#AliPayUser").val().trim().replace("&nbsp;", "") + "^" + $("#AliPayRealName").val().trim().replace("&nbsp;", "");
            //paydata += "^" + $("#ctl00_ContentPlaceHolder1_hdreferralblance").val() + "^佣金提现";

            //var jsondata = { Paydata: paydata };
            //_postpgnow("/Admin/OutPay/alipayOut.aspx", jsondata);


            var PostData = "";

            $('.content-table input[type="checkbox"]:checked').each(function () {


                var rowParentTd = $(this).parents("tr").children("td");
                var RequestType = rowParentTd.eq(4).attr("title");
                var SerialID = rowParentTd.eq(5).attr("SerialID");
                var MerchantCode = rowParentTd.eq(5).attr("MerchantCode");
                var AccountName = rowParentTd.eq(6).text();
                var ReqSum = rowParentTd.eq(3).text();
                ReqSum = ReqSum.replace("￥", "").trim() * 1;

                PostData += "|" + SerialID + "^" + MerchantCode + "^" + AccountName + "^" + ReqSum + "^佣金支付";
            });

            if (PostData != "") {
                PostData = PostData.substring(1);
            }


            var jsondata = { Paydata: PostData };
            _postpgnow("/Admin/OutPay/alipayOut.aspx", jsondata);

            return false;

        }

        function _postpgnow(_url, jsondata) {
            var $form = $("<form></form>");
            $form.attr('action', _url);
            $form.attr('method', 'post');
            $form.attr('target', '_black');
            $form.attr("display", "none");
            for (var x in jsondata) {
                $form.append("<input type='hidden' name='" + x + "' value = '" + jsondata[x] + "'></input>");
            }
            $form.appendTo("body");
            $form.submit();
        }

        function BatchPayShow(tid) {

            $("#ctl00_ContentPlaceHolder1_PaybatchType").val(tid);
            var batchAlipay = "<%=BatchAlipay%>";
            var batchWeipay = "<%=BatchWeipay%>";

            if (tid == "1") {

                if (batchAlipay == "False") {
                    HiTipsShow("支付宝批量付款未设置，请至‘提现设置’中修改！", "error")
                    return;
                }

                $("#PbatchTitle").html("支付宝批量支付");
            } else {
                if (batchWeipay == "False") {
                    HiTipsShow("微信批量支付功能未开通，请至‘提现设置’中修改！", "error")
                    return;
                }
                $("#PbatchTitle").html("微信批量支付");
            }

            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                HiTipsShow("请先选择要批量提现的分销商！", "error")
                return;
            }


            var selhtml = "";
            var totalNum = 0;
            var totalSum = 0;

            $('.content-table input[type="checkbox"]:checked').each(function () {


                var rowParentTd = $(this).parents("tr").children("td");
                var ts = rowParentTd.eq(1).html().split("\<br");
                var RequestType = rowParentTd.eq(4).attr("title");
                var IsCheck = rowParentTd.eq(4).attr("IsCheck");

                var MerchantCode = rowParentTd.eq(5).attr("MerchantCode");
                var AccountName = rowParentTd.eq(6).text();

                if (RequestType != tid || IsCheck != "1" || MerchantCode.length < 5 || AccountName.length < 1) {
                    $(this).prop("checked", false);
                }
                else {

                    ts[0] = ts[0].replace(/&nbsp;/g, "").trim();
                    var ReqSum = rowParentTd.eq(3).text();
                    selhtml += "<li><span>" + ts[0] + "</span><span>" + ReqSum + "</span></li>";
                    totalNum++;
                    totalSum += ReqSum.replace("￥", "").trim() * 1;
                }


            });

            if (totalSum == 0 || totalNum == 0) {
                HiTipsShow("选择的提现申请不符合批量发放要求！", "error")
                return;
            }

            $("#batchSum").html(totalSum);
            $("#batchNum").html(totalNum);

            $("#selbatch").html(selhtml + "<div  style='clear:both'></div>");

            $('#BatchPay').modal('toggle').children().css({
                width: '500px', top: "170px"
            });

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <asp:HiddenField ID="HiddenSid" Value="" runat="server" />
        <input type="hidden" id="hduserid" runat="server" />
        <input type="hidden" id="hdreferralblance" runat="server" />
        <input type="hidden" id="hdredpackrecordnum" runat="server" />


        <!--支付宝支付等待-->
        <div class="modal fade" id="PayWait">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left" id="batchTitle">支付宝付款中</h4>
                    </div>
                    <input type="hidden" id="AliPayRealName" value="" />
                    <input type="hidden" id="AliPayUser" value="" />
                    <div class="set-switch form-horizontal" style="padding: 20px; text-align: center">

                        <input type="button" class="btn btn-info" id="payError" value="　支付遇到问题　" />
                        <input type="button" id="paySuccessAll" class="btn btn-success" value="　　支付完成　　" />

                    </div>

                    <div style="clear: both">
                    </div>

                    <div class="modal-footer">
                    </div>

                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->


        <!--批量支付-->
        <div class="modal fade" id="BatchPay">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left" id="PbatchTitle">批量支付</h4>
                    </div>
                    <input type="hidden" id="PaybatchType" value="" runat="server" />

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">

                        <div class="form-group" style="margin-bottom: 5px">
                            <div class="batchtxt">本次总打款：<span id="batchSum">111456.00</span>元　　　总计收款人数：<span id="batchNum">21</span>人</div>
                        </div>
                        <ul id="selbatch" class="selrows"></ul>

                    </div>

                    <div style="clear: both">
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="BatchPaySend" OnClientClick="return checkBatchAlipay()" class="btn btn-primary" Text="确定" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->


        <!--支付宝提现-->
        <div class="modal fade" id="aliPayShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">提现支付-支付宝提现</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label">申请日期：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqDate4"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>申请金额：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqAmount4"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款帐号：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqMerchantCode4"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款人：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayName4"></span>
                            </div>
                        </div>

                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="alipaySend" OnClientClick="return checkAlipay()" class="btn btn-primary" Text="立即支付" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!--微信支付提现-->
        <div class="modal fade" id="weiPayShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">提现支付-微信钱包</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label">申请日期：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqDate3"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>申请金额：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqAmount3"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款帐号：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqMerchantCode3"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款人：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayName3"></span>
                            </div>
                        </div>

                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="weipaySend" class="btn btn-primary" Text="立即支付" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->


        <!--微信红包支付-->
        <div class="modal fade" id="weiRedShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">提现支付-微信红包</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label">申请日期：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqDate2"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>申请金额：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqAmount2"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款帐号：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqMerchantCode2"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款人：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayName2"></span>
                            </div>
                        </div>

                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="WeiRedPack" class="btn btn-primary" Text="立即发送红包" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



        <!--网银转账登记-->
        <div class="modal fade" id="bankPayShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">提现支付登记-网银转帐</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>支付日期：</label>
                            <div class="col-xs-7">
                                <asp:TextBox ID="bankPayDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>流水号：</label>
                            <div class="col-xs-7">
                                <asp:TextBox ID="bankPayNum" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label">申请日期：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqDate1"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>申请金额：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqAmount"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款帐号：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqMerchantCode1"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款人：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayName1"></span>
                            </div>
                        </div>




                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="BankPaySave" class="btn btn-primary" Text="确定" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



        <!--标题-->
        <div class="page-header">
            <h2>提现申请列表</h2>
        </div>

        <div class="set-switch">
            <div class="form-horizontal clearfix">


                <div class="form-inline  mr20">


                    <div class="form-inline journal-query">
                        <div class="form-group">
                            <label for="sellshop1">店铺名：</label>
                            <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                        </div>

                        <div class="form-group" style="padding-left: 4px">
                            <label for="setdate">时间范围：</label>
                            <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw100" />
                            &nbsp;至&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw100" />
                            &nbsp;&nbsp;&nbsp;
                        </div>
                        <asp:Button ID="btnSearchButton" runat="server" class="btn resetSize btn-primary" Text="查询" />&nbsp;&nbsp; <%--OnClick="btnQueryLogs_Click"--%>
                        <div class="form-group">
                            <label for="exampleInputName2">快速查看</label>

                            <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click1" />
                            <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click1" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--数据tab-->
        <div class="play-tabs" style="padding-bottom: 3px">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active">
                    <asp:LinkButton ID="Frist" Text="待发放(0)" runat="server"></asp:LinkButton>
                <li role="presentation" class="">
                    <asp:LinkButton ID="Second" Text="发放异常(0)" runat="server" OnClick="Second_Click"></asp:LinkButton></li>
            </ul>
        </div>

        <!--数据列表-->
        <div>

            <div class="title-table">
                <table class="table" style="margin-bottom: 0px">
                    <thead>
                        <tr>
                            <th width="42"></th>
                            <th width="140" style="text-align: left">店铺名称/手机</th>
                            <th width="140">申请时间</th>
                            <th width="140">提现金额</th>
                            <th width="140">账号类型</th>
                            <th width="140">账号/卡号/OpenID</th>
                            <th width="140">收款人</th>
                            <th width="140"></th>
                        </tr>
                    </thead>
                    <tbody style="background: #fff">
                        <tr>
                            <td colspan="8">
                                <div class="mb10 table-operation">
                                    &nbsp;
                                               &nbsp;&nbsp;<input type="checkbox" id="sells1" class="allselect">
                                    <label for="sells1">全选</label>
                                    <button type="button" onclick="ShowGrade()" class="btn resetSize btn-primary ">批量审核</button>&nbsp;
                                                 <button type="button" class="btn resetSize btn-primary " onclick="ShowRefuse()">批量驳回</button>
                                    &nbsp;&nbsp;︱&nbsp;&nbsp;
                                                <button type="button" onclick="BatchPayShow(1)" class="btn resetSize btn-primary ">支付宝批量发放</button>&nbsp;
                                                <button type="button" onclick="BatchPayShow(0)" class="btn resetSize btn-primary ">微信支付批量发放</button>

                                    &nbsp;&nbsp;
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="content-table">
                <table class="table table-hover mar table-bordered" style="table-layout: fixed">
                    <tbody>

                        <asp:Repeater ID="reCommissions" runat="server" OnItemDataBound="rptList_ItemDataBound">
                            <%--OnItemCommand="rptList_ItemCommand"--%>
                            <ItemTemplate>
                                <tr class="td_bg">
                                    <td width="30">&nbsp;<input name="CheckBoxGroup" type="checkbox" title="<%#Eval("IsCheck") %>" value='<%#Eval("SerialID") %>' /></td>
                                    <td width="100" style="text-align: left">&nbsp;&nbsp;<%# Eval("StoreName")%>&nbsp;<br />
                                        &nbsp;&nbsp;<%# Eval("CellPhone")%></td>
                                    <td width="100"><%# Eval("RequestTime", "{0:yyyy-MM-dd HH:mm:ss}")%>&nbsp;</td>
                                    <td width="100">￥<%# Eval("Amount", "{0:F2}")%></td>
                                    <td width="100" ischeck='<%# Eval("IsCheck")%>' title="<%# Eval("RequestType")%>"><%# VShopHelper.GetCommissionPayType(Eval("RequestType").ToString()) %>&nbsp;
                                    </td>
                                    <td width="100" serialid="<%# Eval("SerialID")%>" merchantcode="<%# Eval("MerchantCode") %>" userid="<%# Eval("UserId")%>">
                                        <%# Eval("MerchantCode") %><br />
                                        <%# Eval("bankName") %>
                                    </td>
                                    <td width="100"><%# Eval("AccountName") %></td>

                                    <td width="100">
                                        <p style="margin-bottom: 4px">
                                            <Hi:ImageLinkButton ID="CheckOrGive" CssClass="btn btn-primary btn-xs" OnClientClick='return ShowPassSignal(this);return false' IsCheck='<%# Eval("IsCheck")%>' title='<%# Eval("SerialID")%>' CommandName="pass" CommandArgument='<%# Eval("SerialID")%>' runat="server" Text="审核" IsShow="true"
                                                DeleteMsg="审核通过？" />
                                        </p>

                                        <p>
                                            <a href="javascript:ShowRefuseSignalShow(<%# Eval("SerialID")%>)" class="btn btn-default btn-xs">驳回</a>
                                        </p>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>



            <!--数据列表底部功能区域-->
            <br />
            <div class="select-page clearfix">
                <%--<div class="form-horizontal fl">
                    <a onclick="javascript:history.go(-1)" class="btn btn-primary">返回</a>
                </div>--%>
                <div class="page fr">
                    <div class="pageNumber">
                        <div class="pagination" style="margin: 0px">
                            <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="20" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="clearfix" style="height: 30px"></div>

        </div>

        <!--批量审核-->
        <div class="modal fade" id="GradeShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">批量审核</h4>
                    </div>
                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <ul id="selPass" class="selrows">
                        </ul>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="BatchPass" class="btn btn-primary" Text="确定审核" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!--批量驳回-->
        <div class="modal fade" id="RefuseShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">批量驳回</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">

                        <div class="form-group" style="margin-bottom: 5px">
                            <label class="col-xs-3 control-label" style="text-align: left; width: 95px"><em>*</em>驳回理由：</label>
                            <div class="col-xs-9" style="width: 400px">
                                <asp:TextBox ID="RefuseMks" TextMode="MultiLine" CssClass="form-control  inputw120" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <ul id="selRefuse" class="selrows"></ul>

                    </div>

                    <div style="clear: both">
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="Button2" class="btn btn-primary" Text="确定驳回" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!--申请驳回-->
        <div class="modal fade" id="RefuseSignalShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">申请驳回</h4>
                    </div>
                    <asp:HiddenField ID="hSerialID" Value="" runat="server" />
                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <div class="form-group" style="margin-bottom: 0px">
                            <label class="col-xs-3 control-label" style="text-align: left; width: 95px"><em>*</em>驳回理由：</label>
                            <div class="col-xs-9" style="width: 400px">
                                <asp:TextBox ID="SignalrefuseMks" TextMode="MultiLine" CssClass="form-control  inputw120" Height="100" runat="server"></asp:TextBox>
                                <small>驳回理由必需填写</small>
                            </div>
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="Button3" class="btn btn-primary" Text="确定" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!--审核通过-->
        <div class="modal fade" id="PassSignalShow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">提现审核</h4>
                    </div>

                    <div class="set-switch form-horizontal" style="margin-bottom: 0px">
                        <asp:HiddenField ID="HSid" Value="" runat="server" />
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label">申请日期：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqDate"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>申请金额：</label>
                            <div class="col-xs-6">
                                <asp:TextBox ID="ReqSum" CssClass="form-control  inputw100" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款帐号：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqMerchantCode"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>收款人：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayName"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>帐号类型：</label>
                            <div class="col-xs-6">
                                <span class="setControl" id="ReqPayType"></span>
                            </div>
                        </div>


                        <div style="clear: both"></div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="PassCheck" class="btn btn-primary" Text="确定" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->


    </form>
</asp:Content>
