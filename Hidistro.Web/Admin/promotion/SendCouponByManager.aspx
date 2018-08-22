<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SendCouponByManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.SendCouponByManager" %>

<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    .has-error small{color:#a94442;}
        .contentp p { line-height:25px; }
        .contentp span { color:red; }
</style>
    <script type="text/javascript">
        $(function () {
            $('.selectpreferentialvolume input').change(function () {
                console.log("dd");
                var now = $('.selectpreferentialvolume input:checked').val(),
                    parentElement = $(this).parents('.selectpreferentialvolume');
                $('.manualreleasebox').hide();
                if (parentElement.nextAll('.manualreleasebox').eq(now).get(0)) {
                    $('.manualreleasebox').eq(now).show();
                } else {
                    if (now == 1) {
                        parentElement.nextAll('.manualreleasebox').after('<div class="manualreleasebox boxsize"><span class="title">请指会员微信昵称</span><div class="form-horizontal"><div class="form-group mt10"><label for="inputEmail3" class="col-xs-3 control-label resetSize">微信昵称：</label><div class="col-xs-9"><textarea id="weixinname" class="resetSize inputw250" rows="5" cols="10"></textarea><small>填写单个或多个微信昵称，每个微信昵称占一行</small></div></div></div></div>');
                    } else if (now == 2) {
                        parentElement.nextAll('.manualreleasebox:last-child').after('<div class="manualreleasebox boxsize"><span class="title">请指会员用户名</span><div class="form-horizontal"><div class="form-group mt10"><label for="inputEmail3" class="col-xs-3 control-label resetSize">用户名：</label><div class="col-xs-9"><textarea id="usernamename" class="resetSize inputw250" rows="5" cols="10"></textarea><small>填写单个或多个用户名，每个用户名占一行</small></div></div></div></div>');
                    }
                }
            });

            GetCouponInfo();
            $("[id$='ddlCouponList']").change(function () { GetCouponInfo(); });

            //验证上级店铺名称
            $("#txtStoreName").blur(function () {
                var _this = $(this);
                var value = $(this).val();
                if (value != '')
                {
                    $.ajax({
                        type: 'get',
                        dataType: 'json',
                        data: { "action": "getstroeidbystorename", "storeName": value },
                        url: 'GetMemberGradesHandler.ashx',
                        success: function (data)
                        {
                            if (data.status == 'ok') {
                                $("#txtStoreId").val(data.storeId);
                                $("#storeNameErr").html('');
                                _this.parent().removeClass();
                                _this.parent().addClass("col-xs-9");
                            } else {
                                $("#storeNameErr").html(data.description);
                                $("#txtStoreId").val('');
                                _this.parent().removeClass();
                                _this.parent().addClass("col-xs-9 has-error");
                            }
                        }
                    });
                } else
                {
                    $("#txtStoreId").val('');
                    $("#storeNameErr").html('');
                    _this.parent().removeClass();
                    _this.parent().addClass("col-xs-9");
                }
            });

            $("#btnSend").click(function () {
                var sendType = $(".sendType:checked").val();
                if (sendType == '0') {
                    BeginSend()
                } else if(sendType=='1') {
                    BeginSend1()
                }
                else if (sendType == '2') {
                    BeginSend2()
                }
                
            });

            $("#btnSubmitSend").click(function () {
                var sendType = $(".sendType:checked").val();
                if (sendType == '0') {
                    SubmitSend();
                } else {
                    SubmitSend1();
                }
                
            });
           
        

        });

        function GetCouponInfo() {
            var couponId = $("[id$='ddlCouponList']").val();
            $.ajax({
                type: 'get',
                dataType: 'json',
                data: { "action": "getcouponinfo", "id": couponId },
                url: 'GetMemberGradesHandler.ashx',
                success: function (data) {
                    if (data.Status == 2) {
                        $("#couponCount").html('还剩 ' + data.Count + ' 张');
                        $("#pCouponCount").html(data.Count);
                        $("#pCouponCount1").html(data.Count);
                        $("#couponDate").html('有效期：' + data.BeginTime + ' 到 ' + data.EndTime);
                    }
                }
            });
        }

        function BeginSend()
        {
            var text = $("#storeNameErr").text();
            if (text != '')
            {
                ShowMsg("请输入正确的上级店铺名称", false);
                return;
            }
            //会员等级
            var gradeIds="";
            var checkGrades = $(".memberGradeCheck:checked");
            if (checkGrades.size() == 0)
            {
                gradeIds = "0";
            } else if (checkGrades.size() == $(".memberGradeCheck").size()) {
                gradeIds = "0";
            } else {
                checkGrades.each(function () {
                    gradeIds += $(this).val() + ',';
                });
                gradeIds = gradeIds.substring(0, gradeIds.length - 1);
            }
            checkGrades = undefined;
            //上级店铺
            var referralUserId = $("#txtStoreId").val();
            //注册时间起
            var beginCreateDate = $("#<%=this.ucDateBeginDate.ClientID %>_txtDateTimePicker").val();
            //注册时间止
            var endCreateDate = $("#<%=this.ucDateEndDate.ClientID %>_txtDateTimePicker").val();
            //新会员、活跃会员、沉睡会员
            var userType='';
            var checkUserType = $(".userTypeClass:checked");
            if (checkUserType.size() == 0)
            {
                userType = "0";
            } else if (checkUserType.size() == $(".userTypeClass").size()) {
                userType = "0";
            } else {
                checkUserType.each(function () { userType+=String($(this).val()) });
            }
            checkUserType = undefined;
            //自定义分组
            var customGroup = '';
            var checkCustomGroup = $(".customGroup:checked");
            if (checkCustomGroup.size() == 0) {
                customGroup = "0";
            } else {
                checkCustomGroup.each(function () {
                    customGroup += $(this).val() + ',';
                });
                customGroup = customGroup.substring(0, customGroup.length - 1);
            }
            checkCustomGroup = undefined;

            $.ajax({
                type: 'get',
                dataType: 'json',
                data: { 'action': 'getsearchusercount', 'gId': gradeIds, 'rId': referralUserId, 'bDate': beginCreateDate, 'eDate': endCreateDate, 'uType': userType, 'cGroup': customGroup },
                url: 'GetMemberGradesHandler.ashx',
                ansy: false,
                success: function (data) {
                    if (data.status == 'ok') {
                        var couponCount = $("#pCouponCount").html();
                        if (Number(couponCount >= data.count)) {
                            $("#pSelectCount").html(data.count);
                            $('#myModal1').modal('toggle').children().css({
                                width: '530px'
                            })
                        } else {
                            $("#pSelectCount1").html(data.count);
                            $('#myModal').modal('toggle').children().css({
                                width: '530px'
                            })
                        }
                    }
                }
            });
        }

        ///确认发放
        function SubmitSend()
        {
            var couponId = $("[id$='ddlCouponList']").val();
            $.ajax({
                type: 'get',
                dataType: 'json',
                data: { "action": "sendcoupontosearchuser", "couponId": couponId },
                url: 'GetMemberGradesHandler.ashx',
                success: function (data) {
                    if (data.status == 'ok') {
                        ShowMsg(data.msg, true);
                        GetCouponInfo();
                    } else {
                        ShowMsg(data.msg, false);
                    }
                }
            });
        }
        //确认发放
        function SubmitSend1() {
            var couponId = $("[id$='ddlCouponList']").val();
            $.ajax({
                type: 'get',
                dataType: 'json',
                data: { "action": "sendcoupontousers", "cId": couponId, "ids": $("#txtUserIds").val() },
                url: 'GetMemberGradesHandler.ashx',
                success: function (data) {
                    if (data.status == 'ok') {
                        ShowMsg(data.msg, true);
                        GetCouponInfo();
                    } else {
                        ShowMsg(data.msg, false);
                    }
                }
            });
        }

        function BeginSend1()
        {
            var text = $("#weixinname").val();
            if (text.indexOf("_")>-1)
            {
                ShowMsg("不能输入带\"_\"线的微信昵称！请使用其它方式发送", false);
                return;
            }
            if (text == '')
            {
                ShowMsg("微信昵称不能为空！", false);
                return;
            }
            text = text.replace(/\n/g, "_");
            $.ajax({
                type: 'post',
                dataType: 'json',
                data: { "nich": text },
                url: 'GetMemberGradesHandler.ashx?action=getuseridbynich',
                ansy:false,
                success: function (data)
                {
                    if (data.status == 'ok')
                    {
                        $("#txtUserIds").val(data.ids);
                        var couponCount = $("#pCouponCount").html();
                        if (Number(couponCount >= data.count)) {
                            $("#pSelectCount").html(data.count);
                            $('#myModal1').modal('toggle').children().css({
                                width: '530px'
                            })
                        } else {
                            $("#pSelectCount1").html(data.count);
                            $('#myModal').modal('toggle').children().css({
                                width: '530px'
                            })
                        }
                    } else
                    {
                        ShowMsg(data.msg, false);
                    }
                }
            });
        }
        function BeginSend2() {
            var text = $("#usernamename").val();
            if (text.indexOf("_") > -1) {
                ShowMsg("不能输入带\"_\"线的会员用户名！请使用其它方式发送", false);
                return;
            }
            if (text == '') {
                ShowMsg("会员用户名不能为空！", false);
                return;
            }
            text = text.replace(/\n/g, "_");
            $.ajax({
                type: 'post',
                dataType: 'json',
                data: { "uname": text },
                url: 'GetMemberGradesHandler.ashx?action=getuseridbyusername',
                ansy: false,
                success: function (data) {
                    if (data.status == 'ok') {
                        $("#txtUserIds").val(data.ids);
                        var couponCount = $("#pCouponCount").html();
                        if (Number(couponCount >= data.count)) {
                            $("#pSelectCount").html(data.count);
                            $('#myModal1').modal('toggle').children().css({
                                width: '530px'
                            })
                        } else {
                            $("#pSelectCount1").html(data.count);
                            $('#myModal').modal('toggle').children().css({
                                width: '530px'
                            })
                        }
                    } else {
                        ShowMsg(data.msg, false);
                    }
                }
            });
        }
        //清除条件
        function clearInput()
        {
            $(".memberGradeCheck").each(function () { $(this).prop("checked", false); });
            $(".userTypeClass").each(function () { $(this).prop("checked", false); });
            $("#<%=this.ucDateBeginDate.ClientID %>_txtDateTimePicker").val('');
            $("#<%=this.ucDateEndDate.ClientID %>_txtDateTimePicker").val('');
            $("#txtStoreId").val('');
            $("#txtStoreName").val('');
            $("#storeNameErr").html('');
            $("#txtStoreName").parent().removeClass();
            $("#txtStoreName").parent().addClass("col-xs-9");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" id="form">
        <div class="page-header">
            <h2>优惠券手动发放</h2>
        </div>
        <input type="hidden" id="txtUserIds" />
        <div class="form-horizontal manualrelease">
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label">选中优惠券：</label>
                <div class="col-xs-10">
                    <asp:DropDownList ID="ddlCouponList" runat="server" Style="width: 200px;" CssClass="form-control inputw250 mb5">
                    </asp:DropDownList>
                    <small id="couponCount"></small>
                    <small id="couponDate"></small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label">发放范围：</label>
                <div class="col-xs-10">
                    <div class="mb20 selectpreferentialvolume">
                        <label class="middle mr20">
                            <input type="radio" name="condition" class="sendType" value="0" checked="checked">指定条件发放
                        </label>
                        <label class="middle mr20">
                            <input type="radio" name="condition" class="sendType" value="1">指定微信昵称发放
                        </label>
                        <label class="middle mr20">
                            <input type="radio" name="condition" class="sendType" value="2">指定用户名发放
                        </label>
                    </div>
                    <div class="manualreleasebox">
                        <span class="title">请指定发放条件</span>
                        <div class="form-horizontal">
                            <div class="form-group mt10">
                                <label for="inputEmail3" class="col-xs-3 control-label">会员等级：</label>
                                <div class="col-xs-9">
                                    <div class="setlabel">
                                        <%=GetMemberGrande() %>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mt10">
                                <label for="inputEmail3" class="col-xs-3 control-label">会员分组：</label>
                                <div class="col-xs-9">
                                    <div class="setlabel">
                                        <label class="middle mr20">
                                            <input type="checkbox" value="1" class="userTypeClass">新会员
                                        </label>
                                        <label class="middle mr20">
                                            <input type="checkbox" value="2" class="userTypeClass">活跃会员
                                        </label>
                                        <label class="middle mr20">
                                            <input type="checkbox" value="3" class="userTypeClass">沉睡会员
                                        </label>
                                    </div>
                                </div>
                                <div class="col-xs-9">
                                    <div class="setlabel">
                                        <%=GetMemberCustomGroup() %>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mt10 ">
                                <label for="inputEmail3" class="col-xs-3 control-label resetSize">上级店铺名称：</label>
                                <div class="col-xs-9">
                                    <input name="txtStoreName" type="text" value="" id="txtStoreName" class="form-control inputw150 resetSize">
                                    <input type="hidden" name="txtStoreId" id="txtStoreId" value="" />
                                    <small id="storeNameErr"></small>
                                </div>
                            </div>
                            <div class="form-group mt10">
                                <label for="inputEmail3" class="col-xs-3 control-label resetSize">注册时间：</label>
                                <div class="col-xs-9">
                                    <div class="form-inline mb5">
                                        <div class="form-group">
                                            <uc1:ucDateTimePicker runat="server" ID="ucDateBeginDate" CssClass="form-control resetSize inputw150" />
                                        </div>
                                        <div class="form-group">
                                            <label for="sellshop6">&nbsp;&nbsp;至&nbsp;&nbsp;</label>
                                            <uc1:ucDateTimePicker runat="server" ID="ucDateEndDate" CssClass="form-control resetSize inputw150" />
                                        </div>
                                    </div>
                                    <a href="javascript:void(0)" onclick="clearInput();">清除条件</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"></label>
                <div class="col-xs-10">
                    <button type="button" class="btn btn-primary" id="btnSend" >确定发送</button>
                </div>
            </div>
        </div>

        <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="w-modalbox">
                        <h5 style="background: none; text-indent: 10px;">优惠券库存不足</h5>
                        <div class="titileBorderBox borderSolidB">
                            <div class="contentBox pl20 contentp">
                                <p>已选定<span id="pSelectCount1"></span>位待发放优惠券会员</p>
                                <p>优惠券库存<span id="pCouponCount1"></span>张</p>
                                <p> 优惠券库存不够，请修改发放范围或者选择新的优惠券</p>
                            </div>
                        </div>
                        <div class="y-ikown pt10 pb10">
                            <input type="submit"  value="关闭窗口"  class="btn btn-default inputw100" data-dismiss="modal">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="w-modalbox">
                        <h5 style="background:none;text-indent:10px;">优惠券确认发放</h5>
                        <div class="titileBorderBox borderSolidB">
                            <div class="contentBox pl20 contentp">
                                <p>已选定<span id="pSelectCount"></span>位待发放优惠券会员</p>
                                <p>优惠券库存<span id="pCouponCount"></span>张</p>
                                <p>是否立即发放？</p> 
                            </div>
                        </div>
                        <div class="y-ikown pt10 pb10">
                            <input type="submit"  value="立即发放" id="btnSubmitSend" class="btn btn-success inputw100" data-dismiss="modal">
                            <input type="submit" value="关闭窗口"  class="btn btn-default inputw100" data-dismiss="modal">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
