<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGameAct.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.AddGameAct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <style type="text/css">
        .headtext{margin-left:15px;}
        .headdiv{border-bottom:1px solid;border-bottom-color:#cecece; margin-left:5px;margin-right:5px; margin-top:5px;margin-bottom:5px;}
        .divcss{border:1px solid; border-color:#cecece; -moz-border-radius: 15px; -webkit-border-radius: 15px; 
                order-radius:15px; }
        .errorInput{border:1px,solid;border-color:#a94442;}
        .addDiv{width:30px;height:30px;border:1px,solid;border-color:#a94442;border-bottom:1px solid;border-bottom-color:#cecece; -moz-border-radius: 15px; -webkit-border-radius: 15px; order-radius:15px; margin-left:5px;margin-bottom:5px;text-align:center;vertical-align:middle;line-height:30px;   }
        .inputCss{width:50px;height:20px;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            setStep();
            readMemberGrades();
            $('#divstep1').find('input[type="text"]').each(function () {
                $(this).change(function () {
                    testInput(this, 1);
                });               
            });
            $('#divstep2').find('input[type="text"]').each(function () {
                $(this).change(function () {
                    testInput(this, 2);
                });                
            });
            $('#divstep3').find('input[type="text"]').each(function () {
                $(this).change(function () {
                    testInput(this, 3);
                });               
            });

            setCouponList();

            setPrizeType();
            $('#rd1_0').click();
            //addLvl();
            if ('<%=_step%>' == "3") {
                if ('<%=_json%>' != "") {
                    setPrize();
                }
                else
                {
                    addLvl("一等奖");
                    //addLvl("二等奖");
                    //addLvl("三等奖");
                    //addLvl("普通奖");
                }
            }
        });        

        function setEdit(obj) {
            var input = $(obj).next();
            $(input).val($(obj).text());
            $(input).css('display', '');
            $(obj).css('display', 'none');
            $(input).focus();
        }

        function setShowHeader(obj) {
            var lb = $(obj).prev();
            $(lb).text($(obj).val());
            $(lb).css('display', '');
            $(obj).css('display', 'none');
        }

        function addLvl(obj,prizeId) {
            $('#tabHeader').find('li[role="presentation"]').each(function () {
                $(this).removeClass('active');
            });
            $('#addBtn').remove();
            var html = " <li id=\"li_0\" role=\"presentation\" class=\"active\"> <a id=\"a_0\" href=\"#prize_0\" aria-controls=\"prize_0\" role=\"tab\" ondblclick=\"setEdit(this)\" data-toggle=\"tab\">一等奖 </a> <input type=\"text\" id=\"txt_header_0\" name=\"editInput\" onblur=\"setShowHeader(this)\"  style=\"margin-top:10px; display: none;\"  class=\"inputCss\"/> </li>";
            var div = $('div[title="point"]');
            var i = Number($($(div)[div.length - 1]).attr('id').replace('pointDiv_', '')) + 1;
            html = html.replace(/_0/g, "_" + i) + "<li class=\"addDiv\" id=\"addBtn\" onclick=\"addLvl()\">+</li>  </li> ";
            $('#tabHeader').append(html);
            if (obj == null) {
                obj = "奖项" + i;
            }
            $('#a_'+i).text(obj);


            $('#tabContent').find('div[role="tabpanel"]').each(function () {
                $(this).removeClass('active');
            });
            var contentHtml = "<div role=\"tabpanel\" class=\"tab-pane active\" id=\"prize_0\">" + $('#prize_0').html() + "</div>";
            contentHtml = contentHtml.replace(/_0/g, "_" + i);
            $('#tabContent').append(contentHtml);
            if (prizeId != null)
            {
                $('#txt_prizeId_' + i).val(prizeId);
            }
            else
            {
                $('#txt_prizeId_' + i).val("0");
            }
            
            AddPrizeTypeFun($('#prize_' + i));
            return i;
        }

        function setPrize() {
            var json = '<%=_json%>';
            if (json.length <= 0) return;
            var lst = JSON.parse(json);
            if (lst.length > 0) {
                //$('#maintable').find('tr').each(function () {
                //    if ($(this).attr('id') != 'tr_1') {
                //        $(this).remove();
                //    }
                //});
                for (var i = 0; i < lst.length; i++) {
                    var Id = lst[i].Id;
                    var GameId = lst[i].GameId;
                    var PrizeName = lst[i].PrizeName;
                    var sort = lst[i].sort;
                    var PrizeType = lst[i].PrizeType;
                    var PointNumber = lst[i].PointNumber;
                    var PointRate = lst[i].PointRate
                    var GrivePoint = lst[i].GrivePoint;
                    var GiveCouponId = lst[i].GiveCouponId
                    var CouponNumber = lst[i].CouponNumber;
                    var CouponRate = lst[i].CouponRate
                    var GiveProductId = lst[i].GiveProductId
                    var ProductNumber = lst[i].ProductNumber
                    var ProductRate = lst[i].ProductRate
                    var lv = addLvl(PrizeName);
                    $('#prize_' + lv).find('input[type="radio"]').each(function () {
                        var val = Number($(this).val());
                        if (val == PrizeType) {
                            $(this).click();
                        }
                    });
                    $('#txt_prizeId_' + lv).val(Id);
                    if (PrizeType == 1) {
                        $('#txt_point_' + lv).val(GrivePoint);
                        $('#txt_pointNumber_' + lv).val(PointNumber);
                        $('#txt_pointRate_' + lv).val(PointRate);
                    }
                    else if (PrizeType == 2) {
                        $('#txt_coupon_' + lv).val(GiveCouponId);
                        $('#txt_couponNumber_' + lv).val(CouponNumber);
                        $('#txt_couponRate_' + lv).val(CouponRate);
                    }
                    else if (PrizeType == 4) {
                        $('#txt_product_' + lv).val(GiveProductId);
                        $('#txt_productNumber_' + lv).val(ProductNumber);
                        $('#txt_productRate_' + lv).val(ProductRate);
                    }
                }
            }
        }


        function getSavePrize() {
            var stk = new Array();
            var rate = 0;
            $('#tabContent').find('div[role="tabpanel"]').each(function () {
                var i = $(this).attr('id').replace("prize_", "");
                if (i != "0") {
                    var prizeId = Number($('#txt_prizeId_' + i).val());
                    var prizeName = $('#a_' + i).text();
                    var item = getPriezeDetail(this, i, prizeName, prizeId);
                    if (item != null) {
                        rate += item.couponRate + item.pointRate + item.productRate;
                        stk.push(item);
                    }
                }
            });
            if (rate > 100) {
                ShowMsg("中奖率不能大于100！", false);
    
                return false;
            }
            var json = JSON.stringify(stk);
            $('#<%=txt_json.ClientID%>').val(json);
            return true;
        }

        function getPriezeDetail(parent, i,prizeName,prizeId) {
            if (i == "0") return null;
            var prizeType = "1";
            $(parent).find('input[type="radio"]').each(function () {
                if ($(this).prop('checked')) {
                    alert($(this).val());
                    prizeType = $(this).val();
                    return false;
                }
            });
            alert(prizeType);
            var prize = {
                prizeId: prizeId,
                prizeName: prizeName,
                prizeType: Number(prizeType),
                point: 0,
                pointNumber: 0,
                pointRate: 0,
                coupon: 0,
                couponNumber: 0,
                couponRate: 0,
                product: 0,
                productNumber: 0,
                productRate:0
                };
            if (prizeType == "1") {
                var point = $('#txt_point_' + i).val();
                var pointNumber = $('#txt_pointNumber_' + i).val();
                var pointRate = $('#txt_pointRate_' + i).val();
                if (!isInt(point)) {
                    ShowMsg(prizeName + "的积分奖励输入错误！", false);
                    return null;
                }
                if (!isInt(pointNumber)) {
                    ShowMsg(prizeName + "的积分奖励奖品数量输入错误！", false);
                    return null;
                }
                if (!isInt(pointRate)) {
                    ShowMsg(prizeName + "的积分奖励中奖率输入错误！", false);
                    return null;
                }
                prize.point = Number(point);
                prize.pointNumber = Number(pointNumber);
                prize.pointRate = Number(pointRate);
            }
            if (prizeType == "2") {
                var coupon = $('#txt_coupon_' + i).val();
                var couponNumber = $('#txt_couponNumber_' + i).val();
                var couponRate = $('#txt_couponRate_' + i).val();
                if (!isInt(coupon)) {
                    ShowMsg(prizeName + "的优惠券奖励输入错误！", false);
                    return null;
                }
                if (!isInt(couponNumber)) {
                    ShowMsg(prizeName + "的优惠券奖励奖品数量输入错误！", false);
                    return null;
                }
                if (!isInt(couponRate)) {
                    ShowMsg(prizeName + "的优惠券奖励中奖率输入错误！", false);
                    return null;
                }
                prize.coupon = Number(coupon);
                prize.couponNumber = Number(couponNumber);
                prize.couponRate = Number(couponRate);
            }
            if (prizeType == "4") {
                var product = $('#txt_product_' + i).val();
                var productNumber = $('#txt_productNumber_' + i).val();
                var productRate = $('#txt_productRate_' + i).val();
                if (!isInt(product)) {
                    ShowMsg(prizeName + "的商品奖励输入错误！", false);
                    return null;
                }
                if (!isInt(productNumber)) {
                    ShowMsg(prizeName + "的商品奖励奖品数量输入错误！", false);
                    return null;
                }
                if (!isInt(productRate)) {
                    ShowMsg(prizeName + "的商品奖励中奖率输入错误！", false);
                    return null;
                }
                prize.product = Number(product);
                prize.productNumber = Number(productNumber);
                prize.productRate = Number(productRate);
            }
            return prize;
        }



        function setCouponList() {
            var html;
            $.ajax({
                type: "post",
                url: "GetCouponListHandler.ashx",
                data: {},
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        var resultArr = data.data;
                        var html = "";
                        for (var i = 0; i < resultArr.length; i++) {
                            html += "<option value=\"" + resultArr[i].CouponId + "\">" + resultArr[i].CouponName + "</option>";
                        }
                       $('select').each(function () {
                            $(this).empty();
                            $(this).append(html);
                        });
             <%--           if ('<%=_id%>' != '0') {
                            setAct();
                        }--%>
                    }
                    else {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }

        function AddCouponList(obj) {
            var html;
            $.ajax({
                type: "post",
                url: "GetCouponListHandler.ashx",
                data: {},
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        var resultArr = data.data;
                        var html = "";
                        for (var i = 0; i < resultArr.length; i++) {
                            html += "<option value=\"" + resultArr[i].CouponId + "\">" + resultArr[i].CouponName + "</option>";
                        }
                        $(obj).find('select').each(function () {
                            $(this).empty();
                            $(this).append(html);
                        });
             <%--           if ('<%=_id%>' != '0') {
                            setAct();
                        }--%>
                    }
                    else {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }



        function setPrizeType() {
            $('div[role="tabpanel"]').each(function () {
                $(this).find('input[type="radio"]').each(function () {
                    $(this).click(function () {
                        var pointDiv = $(this).parent().parent().next();
                        var couponDiv = $(this).parent().parent().next().next();
                        var productDiv = $(this).parent().parent().next().next().next();
                        var val = $(this).val();
                        if (val == "1") {
                            $(pointDiv).css('display', '');
                            $(couponDiv).css('display', 'none');
                            $(productDiv).css('display', 'none');
                        }
                        else if (val == "2") {
                            $(pointDiv).css('display', 'none');
                            $(couponDiv).css('display', '');
                            $(productDiv).css('display', 'none');
                        }
                        else if (val == "4") {
                            $(pointDiv).css('display', 'none');
                            $(couponDiv).css('display', 'none');
                            $(productDiv).css('display', '');
                        }
                    });
                });
            });
        }


        function AddPrizeTypeFun(obj) {
            $(obj).find('input[type="radio"]').each(function () {
                $(this).click(function () {
                    var pointDiv = $(this).parent().parent().next();
                    var couponDiv = $(this).parent().parent().next().next();
                    var productDiv = $(this).parent().parent().next().next().next();
                    var val = $(this).val();
                    if (val == "1") {
                        $(pointDiv).css('display', '');
                        $(couponDiv).css('display', 'none');
                        $(productDiv).css('display', 'none');
                    }
                    else if (val == "2") {
                        $(pointDiv).css('display', 'none');
                        $(couponDiv).css('display', '');
                        $(productDiv).css('display', 'none');
                    }
                    else if (val == "4") {
                        $(pointDiv).css('display', 'none');
                        $(couponDiv).css('display', 'none');
                        $(productDiv).css('display', '');
                    }
                });
            });
        }



        function testInput(obj,step) {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;

            var flag = false;
            var bError = false;
            if (id == $('#<%=txt_name.ClientID%>').attr('id')) {
                if ($(obj).val() == "") {
                    bError = true;
                }
                if($(obj).val().length>30){
                    bError=true;
                }
            }

            if (id == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').attr('id')) {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
                if (!isDate($('#' + id).val())) {
                    bError = true;
                }
            }

            if (id == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').attr('id')) {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
                if (!isDate($('#' + id).val())) {
                    bError = true;
                }
            }
            if (id == $('#<%=txt_uPoint.ClientID%>').attr('id')) {
                regex = /^[0-9]*$/;
                parent = null;
                flag = true;
            }
            if (id == $('#<%=txt_gPoint.ClientID%>').attr('id')) {
                if ($(obj).val() == "")
                {
                    $(obj).val("0");
                }
                regex = /^[0-9]*$/;
                parent = null;
                flag = true;
            }          
            if (flag) {
                if (testRegex(regex, content, false)) {
                    $(obj).removeClass();
                    $(obj).addClass("form-control");
                }
                else {
                    bError = true;

                }
            }
            if (bError) {
                $(obj).removeClass();
                $(obj).addClass("form-control errorInput");
            }
            else {
                $(obj).removeClass();
                $(obj).addClass("form-control");
            }
            setBtnEnable(step);
            return !bError;
        }

        function testRegex(rgx, str, bflag) {
            if (str == "") {
                if (bflag)
                { return true; }
                else { return false; }
            }
            return result = rgx.test(str);
        }

        function isInt(str){
            regex = /^[0-9]*$/;
            if (str == "") {
                return false;
            }
            return result = regex.test(str);
        }

        function isDate(dateString) {
            if (dateString.trim() == "") return false;
            var r = dateString.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
            if (r == null) {
                return false;
            }
            var d = new Date(r[1], r[3] - 1, r[4]);
            var num = (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
            if (num == 0) {
                return false;
            }
            return (num != 0);
        }

        function setBtnEnable(step) {
            if (step == 3) {
                var error = $('#divstep3').find('.errorInput');
                if (error.length > 0) {

                    $('#<%=save_Step3.ClientID%>').removeAttr('disabled');
                    $('#<%=save_Step3.ClientID%>').prop('disabled', 'disabled');

                }
                else {
                    $('#<%=save_Step3.ClientID%>').removeAttr('disabled');
                }
            }
            else if (step == 2) {
                var error = $('#divstep2').find('.errorInput');
                if (error.length > 0) {

                    $('#<%=save_Step2.ClientID%>').removeAttr('disabled');
                    $('#<%=save_Step2.ClientID%>').prop('disabled', 'disabled');

                }
                else {
                    $('#<%=save_Step2.ClientID%>').removeAttr('disabled');
                }
            }
            else {
                var error = $('#divstep1').find('.errorInput');
                if (error.length > 0) {

                    $('#<%=saveBtn.ClientID%>').removeAttr('disabled');
                    $('#<%=saveBtn.ClientID%>').prop('disabled', 'disabled');

                }
                else {
                    $('#<%=saveBtn.ClientID%>').removeAttr('disabled');
                }
            }
        }

        function setStep() {
            var step = '<%=_step%>';
            var nstep = Number(step);
            var proBtn = $('#progressdiv').find('.speedProgressList');
            var proline = $('#progressdiv').find('.borderLine');
            $(proBtn).each(function () {
                $(this).removeClass('complete');
            });
            $(proline).each(function () {
                $(this).removeClass('complete');
            });
            for (var i = 0; i < proBtn.length - 1; i++) {
                if (i < nstep) {
                    $(proBtn[i]).addClass('complete');
                }
            }
            for (var i = 0; i < proline.length - 1; i++) {
                if (i < (nstep - 1)) {
                    $(proline[i]).addClass('complete');
                }
            }


            if (step == "4") {
                $('#divstep1').css('display', 'none');
                $('#divstep2').css('display', 'none');
                $('#divstep3').css('display', 'none');
                $('#divstep4').css('display', '');
            }
            else if (step == "3") {
                $('#divstep1').css('display', 'none');
                $('#divstep2').css('display', 'none');
                $('#divstep3').css('display', '');
                $('#divstep4').css('display', 'none');                
            }
            else if (step == "2") {
                $('#divstep1').css('display', 'none');
                $('#divstep2').css('display', '');
                $('#divstep3').css('display', 'none');
                $('#divstep4').css('display', 'none');
            }
            else
            {
                $('#divstep1').css('display', '');
                $('#divstep2').css('display', 'none');
                $('#divstep3').css('display', 'none');
                $('#divstep4').css('display', 'none');
            }
        }
        
        function readMemberGrades() {
            $.ajax({
                type: "post",
                url: "GetMemberGradesHandler.ashx",
                data: {},
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        var resultArr = data.data;
                        $('#memberGradediv').empty();
                        var shtml = ' <input id="allmemberGrade" type="checkbox" style="margin-left:15px;" name="memberGrade" value="0"/>全部会员 <input type="checkbox" style="margin-left:10px;" name="memberGrade" value="-1" />潜在会员';
                        $.each(resultArr, function (i, result) {
                            shtml += '<input type="checkbox" style="margin-left:10px;" name="memberGrade" value="' + result.GradeId + '"/>' + result.Name
                        });
                        $('#memberGradediv').append(shtml);
                        $('#memberGradediv').find('input').each(function () {
                            $(this).click(function () {
                                //if ($(this).prop('checked')) {
                                //    $(this).removeAttr('checked');
                                //}
                                //else {
                                //    $(this).attr('checked', 'checked');
                                //}
                            });
                        });
                        $('#allmemberGrade').click(function () {
                            var checked = $(this).prop('checked');
                            $('#memberGradediv').find('input').each(function () {
                                if ($(this).attr('id') != "allmemberGrade") {
                                    if (checked) {
                                        $(this).removeAttr('disabled');
                                        $(this).attr('disabled', 'disabled');
                                        $(this).removeAttr('checked');
                                    }
                                    else {
                                        $(this).removeAttr('disabled');
                                    }
                                }
                            });
                        });

                        if ('<%=_gameId%>' != '0') {
                            setMember();
                        }
                        else
                        {
                           $('#allmemberGrade').click();
                        }
                        
                        

                    }
                    else {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }

        function ToDate(str) {
            str = str.replace(/-/g, "/");
            var date = new Date(str);
            return date;
        }

        function beforeSave_step1() {
            var flag = false;
            $('#divstep1').find('input[type="text"]').each(function () {
                if (!testInput(this, 1)) {
                    if ($(this).attr('id') == '<%=txt_name.ClientID%>') {
                        ShowMsg("请输入正确的游戏名称！");
                    }
                    if ($(this).attr('id') == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').attr('id')) {
                        ShowMsg("请输入正确的开始时间！");
                    }
                    if ($(this).attr('id') == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').attr('id')) {
                        ShowMsg("请输入正确的结束时间！");
                    }
                    flag = true;
                    return false;
                }
            });
            if (flag) {               
                return false;
            }
            if (ToDate($('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val()) > ToDate($('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val())) {
                ShowMsg("结束时间不能早于开始时间！");
                return false;
            }
            return true;
        }

        function beforeSave_step2() {
            var flag = false;
            $('#divstep2').find('input[type="text"]').each(function () {
                if (!testInput(this, 1)) {
                    if ($(this).attr('id') == '<%=txt_uPoint.ClientID%>') {
                        ShowMsg("请输入正确的消耗积分！");
                    }
                    if ($(this).attr('id') == $('#<%=txt_gPoint.ClientID%>').attr('id')) {
                        ShowMsg("请输入正确的赠送积分！");
                    }  
                    flag = true;
                    return false;
                }
            });
            if (flag) {
                return false;
            }
            var grade = setGrade();
            if (grade == "")
            {
                ShowMsg("请选择适用会员等级！");
                return false;
            }
  
            return true;
        }

        function setGrade() {
            var grade = "";
            if ($('#allmemberGrade').prop('checked')) {
                grade = "0";
            }
            else {
                $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                    if ($(this).prop('checked')) {
                        grade += ',' + $(this).val();
                    }
                });
                if (grade.length > 1) {
                    grade = grade.substring(1);
                }
            }

            $('#<%=txt_grades.ClientID%>').val(grade);
            return grade;
        }

        function setMember() {

            var lvl = '<%=_grade%>';

             var flag = false;

             if (lvl == "0") {
                 $('#allmemberGrade').prop('checked', true);
                 flag = true;
             }
             else {
                 $('#allmemberGrade').prop('checked', false);
                 flag = false;
             }
             $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                 var vl = $(this).val();

                 if ($(this).attr('id') == 'allmemberGrade') {

                 }
                 else {
                     if (flag) {
                         $(this).prop('checked', false);
                         $(this).prop('disabled', true);

                     }
                     else {
                         $(this).prop('disabled', false);
                         if ((',' + lvl+',').indexOf(',' + vl + ',') >= 0) {
                             $(this).prop('checked', true);
                         }
                     }
                 }

             });

        }


        function showModel() {

            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });
        }

        function selectProduct() {
            showModel();
        }

    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>新建<%=_gameType.ToString()%></h2>
        </div>
        <div style="margin-bottom:30px;">
            <div id="progressdiv" class="speedProgress clearfix" style="width:605px;margin:0 auto;">
                <div class="speedProgressList complete">
                    <span>1</span>
                    <p>添加游戏</p>
                </div>
                <div class="borderLine"></div>
                <div class="speedProgressList">
                    <span>2</span>
                    <p>奖项设置</p>
                </div>
                <div class="borderLine"></div>
                <div class="speedProgressList">
                    <span>3</span>
                    <p>活动设置</p>
                </div>
                <div class="borderLine mr10"></div>
                <div class="speedProgressList">
                    <span>4</span>
                    <p>完成</p>
                </div>
            </div>
        </div>


        <table style="width: 100%;">
            <tr>
                <td style="width: 360px;">
                    <div class="edit-text-left">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title"><%=_gameType.ToString()%></div>
                                    </div>
                                    <div class="set-overflow" style="height: 450px;">
                                        <div class="white-box">
                                            <div class="coupons autol">
                                                <div class="fl">
                                                    
                                                </div>
                                                <div class="fr">
                                                   
                                                </div>
                                                <div>
                                                    游戏时间:<br />
                                                    <label id="lblTime">2015-08-01 00:00:00 至 2015-08-29 00:00:00 </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mobile-nav">
    <%--                                    <ul class="clearfix">
                                            <li>
                                                <span class="glyphicon glyphicon-home"></span>
                                                <p>店铺主页</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-user"></span>
                                                <p>会员主页</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-list"></span>
                                                <p>所有商品</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-duplicate"></span>
                                                <p>申请分销</p>
                                            </li>
                                        </ul>--%>
                                    </div>
                                </div>
                                <div class="clear-line">
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="width: auto; vertical-align: top;">
                    <input type="hidden" id="txt_id" value="<%=_gameId%>" />
                    <input type="hidden" id="txt_step" value="<%=_step%>" />
                    <div id="divstep1" class="divcss">
                        <div class="headdiv">
                            <label class="headtext">添加游戏：</label>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>游戏名称：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server"  class="form-control" ID="txt_name" Width="200px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>有效期限：</label>
                            <div class="form-inline">
                                <Hi:DateTimePicker runat="server" name="canTest" Style="margin-left: 15px;" CssClass="form-control"
                                    ID="calendarStartDate" />
                                <label>至</label>
                                <Hi:DateTimePicker runat="server" name="canTest" CssClass="form-control" ID="calendarEndDate" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label">活动说明：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" TextMode="MultiLine" Height="100px"  class="form-control" ID="txt_decrip" Width="400px"></asp:TextBox>
                                <small style="width:400px;">当前已输入<span id="s_inputNumber">0</span>个字符，您还可以输入<span id="s_canInputNumber">600</span>个字符</small>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-offset-3 marginl">
                                <asp:Button runat="server" ID="saveBtn" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="下一步" OnClientClick="return beforeSave_step1();" />
                            </div>
                        </div>
                    </div>
                    <div id="divstep2" class="divcss">
                        <div class="headdiv">
                            <label class="headtext">活动设置：</label>
                        </div>
      
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>适用会员：</label>
                            <div class="form-inline">
                                <div id="memberGradediv" style="vertical-align: central;">
                                    <input type="checkbox" name="memberGrade" value="0" />全部会员
                                    <input type="checkbox" name="memberGrade" value="-1" />潜在会员
                                </div>
                                <div style="display:none">
                                    <asp:TextBox runat="server" class="form-control" ID="txt_grades" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>消耗积分：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" name="canTest" class="form-control" ID="txt_uPoint" Width="200px"></asp:TextBox>
                                分/次      
                                <small style="margin-left: 25%;">用户每次参与活动需要消耗积分，值为0时不消耗</small>                          
                            </div>  
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label">参与送积分：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" name="canTest" class="form-control" ID="txt_gPoint" Width="200px"></asp:TextBox>
                                分/次 
                                <asp:CheckBox id="onlyChk" runat="server" style="margin-left:15px;" />
                                仅送给未中奖的用户
                                <small style="margin-left:25%;">值为0时不赠送积分</small>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>参与限制：</label>
                            <div class="form-inline">
                                <asp:RadioButton runat="server" GroupName="attend" Checked="true" ID="rd1" />
                                一天一次
                                <asp:RadioButton runat="server" GroupName="attend" ID="rd2" />
                                一人一次
                                <asp:RadioButton runat="server" GroupName="attend" ID="rd3" />
                                一天两次
                                <asp:RadioButton runat="server" GroupName="attend" ID="rd4" />
                                一人两次
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-xs-offset-3 marginl">
                                <asp:Button runat="server" ID="back_Step2" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="上一步"  />

                                <asp:Button runat="server" ID="save_Step2" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="下一步" OnClientClick="return beforeSave_step2();" />
                            </div>
                        </div>
                    </div>
                    <div id="divstep3" class="divcss">
                        <div class="headdiv">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <label class="headtext">奖项设置：</label>
                                    </td>
                                    <td>
                                        <small>奖项等级设置的奖品数量越多，则该等级中奖率越高。</small>
                                        <small>例如：设置一等奖 10个，二等奖20个，则中二等奖概率高于一等奖</small>
                                    </td>
                                </tr>
                            </table>                            
                        </div>
                        <div class="play-tabs" style="margin-left:5px;margin-right:5px;">
                            <div style="display:inline">
                             <ul class="nav nav-tabs" role="tablist" id="tabHeader">
                                <li id="li_0" role="presentation" class="active" style="display:none;">
                                   
                                        <a href="#prize_0" aria-controls="prize_0" role="tab" ondblclick="setEdit(this)"
                                            data-toggle="tab" >一等奖
                                        </a>
                                        <input type="text" id="txt_header_0" name="editInput" onblur="setShowHeader(this)"
                                            class="inputCss" style="display: none;" />
                               

                                </li>  
                                 <li class="addDiv" id="addBtn"> 
                                     + </li>                      
                            </ul>
                            
                            </div>
                            <div style="display:none" >
                                <asp:TextBox runat="server" ID="txt_json"></asp:TextBox>
                            </div>

                            <div class="tab-content" id="tabContent">
                                <div role="tabpanel" class="tab-pane active" id="prize_0">
                                    <input type="hidden" id="txt_prizeId_0" />
                                    <div class="form-group">
                                        <label class="col-xs-3 control-label"><em>*</em>奖品类别：</label>
                                        <div class="form-inline">
                                            <input type="radio" id="rd1_0" name="prizeType_0" style="margin-left: 15px;"                           checked="checked" value="1" /> 赠送积分

                                            <input type="radio" id="rd2_0" name="prizeType_0" style="margin-left:5px;" value="2" />
                                            赠送优惠券

                                            <input type="radio" id="rd3_0" name="prizeType_0" style="margin-left: 5px;" value="4" />
                                            赠送商品
                                        </div>
                                    </div>
                                    <div id="pointDiv_0" title="point">
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>赠送积分：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_point_0" class="form-control" />                                               
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>奖品数量：</label>
                                            <div class="col-xs-4">
                                            <input type="text" id="txt_pointNumber_0" class="form-control" />
                                                <small>奖品数量为0时不设此奖项</small>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>中奖率：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_pointRate_0" class="form-control" />
                                                <small>中奖率必须为整数</small>
                                            </div>
                                        </div>
                                    </div>
                            <div id="couponDiv_0" title="coupon">
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>赠送优惠券：</label>
                                            <div class="col-xs-4">
                                                <select id="txt_coupon_0"  class="form-control">
                                                </select>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>奖品数量：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_couponNumber_0" class="form-control" />
                                                <small>奖品数量为0时不设此奖项</small>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>中奖率：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_couponRate_0" class="form-control" />
                                                <small>中奖率必须为整数</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="productDiv_0" title="product">
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>赠送商品：</label>
                                            <div class="col-xs-4">
                                                <img id="img_product_0" src="/Storage/master/product/thumbs60/60_e9e27cc8f8d548d7bd58ce41cf382fc3.png" onclick="selectProduct();" />
                                                <input type="hidden" id="txt_product_0" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>奖品数量：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_productNumber_0" class="form-control" />
                                                <small>奖品数量为0时不设此奖项</small>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-3 control-label"><em>*</em>中奖率：</label>
                                            <div class="col-xs-4">
                                                <input type="text" id="txt_productRate_0" class="form-control" />
                                                <small>中奖率必须为整数</small>
                                            </div>
                                        </div>
                                    </div>
                                </div>
        
                            </div>
                        </div>

                        
                        <div class="form-group">
                            <div class="col-xs-offset-3 marginl">
                                <asp:Button runat="server" ID="back_Step3" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="上一步"  />

                                <asp:Button runat="server" ID="save_Step3" class="btn btn-success bigsize" Style="margin-left: 15px;"
                                    OnClientClick="return getSavePrize();" Text="下一步" />
                            </div>
                        </div>
                    </div>
                    <div id="divstep4" class="divcss">
                        div4
                        <div class="col-xs-offset-3 marginl">
                            <asp:Button runat="server" ID="Button1" class="btn btn-success bigsize" Style="margin-left: 15px;"
                                Text="上一步" />

                            <asp:Button runat="server" ID="Button2" class="btn btn-success bigsize" Style="margin-left: 15px;"
                                OnClientClick="return getSavePrize();" Text="完成" />
                        </div>
                    </div>


                </td>
            </tr>
        </table>



        <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal" id="hform">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span
                            aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">选择商品</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="htxtRoleId" runat="server" />

                        <div class="form-inline">
                            <label>商品名称</label>
                            <input type="text" id="txt_queryName" class="form-control" style="width:200px;" />
                            <select id="txt_queryStatus" class="form-control" style="width: 200px;">
                                <option value="0">出售中</option>
                                <option value="1">仓库中</option>
                            </select>
                        </div>          
                        <table style="width:100%" class="table" id="tabMod">
                            <tr id="tr_0">
                                <td width="50%">                                    
                                    <div class="img fl mr10">
                                        <img id="tableimg_0" src="/Storage/master/product/thumbs60/60_e9e27cc8f8d548d7bd58ce41cf382fc3.png"                                            style="height: 60px; width: 60px; border-width: 0px;">
                                    </div>
                                    <div class="shop-info">
                                        <a id="productName_0" class="mb5">test</a>   
                                        <p>&nbsp;</p>  
                
                                        <p id="productPrice_0" class="er">￥100 </p>
                                    </div>
                                </td>
                                <td style="text-align: center; width: 40%; vertical-align: middle;">
                                    <button type="button" id="selectBtn_0" class="btn btn-info btn-sm"
                                        name="selectBtn">
                                        选取                                            
                                    </button>
                                    <button type="button" id="checkedBtn_0" class="btn btn-success btn-sm" style="display: none">
                                        已选
                                            <span class="glyphicon glyphicon-ok-circle" aria-hidden="true"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <input type="button" ID="btnSubmitRoles" value="确定" class="btn  btn-success" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



    </form>
</asp:content>

