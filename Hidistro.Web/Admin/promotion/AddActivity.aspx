<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddActivity.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"  
    Inherits="Hidistro.UI.Web.Admin.promotion.AddActivity" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagName="SetMemberRange" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .titleLable{font-weight:bold;font-size:14px;margin-left:10%;}
        .graytext{font-size:10px;color:#cccccc;}
        .divClass{margin-bottom:10px;margin-top:10px; margin-left:25%; vertical-align:central;}
        .textClass{margin-left: 5px; margin-right:5px;width: 200px;}
        .selectClass{margin-left: 5px; margin-right:5px; margin-top:10px;}
        .errorInput{border:1px,solid;border-color:#a94442;}
    </style>
    <script type="text/javascript">
        var isView=<%=IsView%>;
        $(document).ready(function () {  
            

          

            readMemberGrades();
            $('#rad1').click(function () {
                setAddShow(false);
            });
            $('#rad2').click(function () {
                setAddShow(true);
            });
            $('#rad3').click(function () {
                setMeetType(0);
            });
            $('#rad4').click(function () {
                setMeetType(1);
            });

            setCouponList();
            $('input[type="text"]').change(function () {
                testInput(this);
            });           
                   
            $('input[type="radio"][name="ProductsGroup"]').each(function () {
                $(this).click(function () {
                    var id = $(this).attr('id');
                    if (id == "product_rd1") {
                        $('#saveBtn').css('display', '');
                        $('#nextBtn').css('display', 'none');
                        $('#labelstatus').text('全部商品参与状态');
                    }
                    else {
                        $('#saveBtn').css('display', 'none');
                        $('#nextBtn').css('display', '');
                        $('#labelstatus').text('部分商品参与状态');
                    }
                });
            });

            if ('<%=_id%>' != '0') {
                $('#product_rd1').attr("disabled", "disabled");
                $('#product_rd2').attr("disabled", "disabled");
                var isAllProduct = '<%=isAllProduct%>';
                if(isAllProduct == "True")
                {
                    $('#product_rd1').prop('checked', true);
                    $('#saveBtn').css('display', '');
                    $('#nextBtn').css('display', 'none');
                    $('#labelstatus').text('全部商品参与状态');
                }
                else
                {
                    $('#product_rd2').prop('checked', true);
                    $('#saveBtn').css('display', 'none');
                    $('#nextBtn').css('display', '');
                    $('#labelstatus').text('部分商品参与状态');
                }               
            }
            else {
                var hasPartAct = '<%=hasPartProductAct%>';
                if (hasPartAct == "True") {
                    $('#product_rd1').attr("disabled", "disabled");
                    $('#product_rd2').click();
                }
                else {
                    $('#product_rd1').click();
                }
            }
        });

        function setAct() {
            var attendType = '<%=_act.attendType%>';
            if (attendType == 0) {
                $('#rad1').prop('checked',true);
            }
            else
            {
                $('#rad2').prop('checked', true);
            }
                        
            var lst = JSON.parse('<%=_json%>');
            if (lst.length > 0) {
                $('#maintable').find('tr').each(function () {
                    if ($(this).attr('id') != 'tr_1') {
                        $(this).remove();
                    }
                });
                for(var i=1;i<lst.length;i++)
                {
                    Addlvl();
                }
            }


            var trs = $('#maintable').find('tr');
           
            if (lst.length > 0) {
                $(trs).each(function (i,e) {
                    var meet = lst[i].MeetMoney;
                    var meetNumber = lst[i].MeetNumber;
                    var ReductionMoney = lst[i].ReductionMoney;
                    var bFreeShipping = lst[i].bFreeShipping;
                    var point = lst[i].Integral;
                    var coupon = lst[i].CouponId;
                    var lvl = Number($(this).attr('id').replace('tr_', ''))
                    $('#txt_Meet_' + lvl).val(meet);
                    $('#txt_MeetNumber_' + lvl).val(meetNumber);
                    if (Number(ReductionMoney) != 0) {
                        $('#chk1_' + lvl).prop('checked', true);
                        $('#txt_redus_' + lvl).val(ReductionMoney);
                    }
                    else
                    {
                        $('#chk1_' + lvl).prop('checked', false);
                        $('#txt_redus_' + lvl).val('');
                    }


                    if(!bFreeShipping)
                    {
                        $('#chk2_' + lvl).prop('checked', false);
                    }
                    else
                    {
                        $('#chk2_' + lvl).prop('checked', true);
                    }

                    if (Number(point) != 0) {
                        $('#chk3_' + lvl).prop('checked', true);
                        $('#txt_point_' + lvl).val(point);
                    }
                    else {
                        $('#chk3_' + lvl).prop('checked', false);
                        $('#txt_point_' + lvl).val('');
                    }

                    if (Number(coupon) != 0) {
                        $('#chk4_' + lvl).prop('checked', true);
                        $('#sel_' + lvl).val(String(coupon));
                    }
                    else {
                        $('#chk4_' + lvl).prop('checked', false);
                        //$('#sel_' + lvl).val('0');
                    }
                });
            }
            var meetType = '<%=_act.MeetType%>';
            if (meetType == 0) {
                $('#rad3').click();
            }
            else {
                $('#rad4').click();
            }
        }

   <%--     function setMember(){
            var lvl = '<%=_act.MemberGrades%>';
            var flag = false;
                        
            if (lvl == "0") {
                $('#allmemberGrade').prop('checked', true);
                $('#allmemberGrade').attr('checked', 'checked');
                flag = true;
            }
            else {
                $('#allmemberGrade').prop('checked', false);
                flag = false;
            }
            $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                var vl = $(this).val();

                if (flag) {
                    $(this).prop('checked', true);
                    //$(this).prop('disabled', true);
                }
                else {
                    //$(this).prop('disabled', false);
                    if ((',' + lvl + ',').indexOf(',' + vl + ',') >= 0) {
                        $(this).prop('checked', true);
                    }
                }

            });
        }--%>




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
                        $('#maintable').find('select').each(function () {
                            $(this).empty();
                            $(this).append(html);
                        });
                        if ('<%=_id%>' != '0') {
                            setAct();
                        }
                    }
                    else {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }




        function setMeetType(flag)
        {
            $('label[for="meetType"]').each(function () {
                if (flag == 0) {
                    $(this).text('元');
                }
                else
                {
                    $(this).text('件');
                }
            });
            $('input[name="Meet"]').each(function () {
                if (flag == 0) {
                    $(this).css('display', '');
                }
                else {
                    $(this).css('display', 'none');
                }
            });
            $('input[name="MeetNumber"]').each(function () {
                if (flag == 0) {
                    $(this).css('display', 'none');
                }
                else {
                    $(this).css('display', '');
                }
            });
            


            $('#maintable').find('tr').each(function () {
                var i = Number($(this).attr('id').replace('tr_', ''))

                //lblMeetType_1
            });
        }


        function setAddShow(flag) {
            if (flag) {
                $('#addtable').css('display', '');
            }
            else
            {
                $('#addtable').css('display', 'none');
                $(".delDiv:gt(0)").trigger("click");

            }
        }

        function readMemberGrades() {
            $.ajax({
                type: "post",
                url: "GetMemberGradesHandler.ashx?action=getmembergrade",
                data: {},
                dataType: "json",
                success: function (data) {

                   

                    if (data.type == "success") {
                        var resultArr = data.data;
                        $('#memberGradediv').empty();
                        var shtml = '';
                        $.each(resultArr, function (i, result) {
                            if (i == 0)
                                shtml += '<input type="checkbox" name="memberGrade" value="' + result.GradeId + '"/>' + result.Name;
                            else
                                shtml += '<input type="checkbox" style="margin-left:10px;" name="memberGrade" value="' + result.GradeId + '"/>' + result.Name;
                        });
                        $('#memberGradediv').append(shtml);
                        $('#memberGradediv').find('input').each(function () {
                            $(this).click(function () {
                                if ($(this).attr('checked') == "checked") {
                                    $(this).removeAttr('checked');
                                }
                                else {
                                    $(this).attr('checked', 'checked');
                                }

                                var hasChecked = $("[name='memberGrade'][checked]").length;
                                var all = $("[name='memberGrade']").length;
                                if (hasChecked == all) {
                                    $('#allmemberGrade').attr('checked', 'checked');
                                    $('#allmemberGrade').prop("checked", true);
                                }
                                else {
                                    $('#allmemberGrade').removeAttr('checked');
                                    $('#allmemberGrade').prop("checked", false);
                                }
                            });
                        });
                        $('#allmemberGrade').click(function () {
                            if ($(this).attr('checked') == "checked") {
                                $(this).removeAttr('checked');
                            }
                            else {
                                $(this).attr('checked', 'checked');
                            }
                            var checked = $(this).attr('checked');
                            $("[name='memberGrade']").each(function () {
                                if (checked == "checked") {
                                    $(this).attr('checked', 'checked');
                                    $(this).prop("checked", true);
                                }
                                else {
                                    $(this).removeAttr('checked');
                                    $(this).prop("checked", false);
                                }
                            });
                        });
                        //$('#allmemberGrade').click();
                       // setMember();

                        if(isView==1){
                            $("input").prop("disabled",true);
                            $("[name='member']").prop("disabled",true)
                            $("#atitle").text("查看满减活动详情");
                            $(".footer-btn").hide();
                        }

                    }
                    else {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }

        function Addlvl() {
            var lb = $('#maintable').find('tr');
            var lvl = 1;

            lvl = lb.length + 1;
            if (lvl > 5) {
                ShowMsg('最多只能添加5级优惠', false);
                return;
            }
            var i = Number($(lb[lb.length - 1]).attr('id').replace('tr_', '')) + 1;
            var html = $("#tr_1").html();
            html = "<tr  id=\"tr_1\">".replace("_1", "_" + i) + html.replace(/_1/g, "_" + i) + "</tr>";
            $('#maintable').append(html);
            $('#lblLvl_' + i).text(lvl);            
            $('#del_' + i).css('display', '');
            $('#tr_' + i).find('input[type="text"]').each(function () {
                $(this).removeClass('errorInput');
                $(this).change(function () {
                    testInput(this);
                });
            });
        }

        function Dellvl(obj) {
            var tr = $(obj).parent().parent();
            //$(tr).remove();

            var trs = $('#maintable').find('tr');
            var count = trs.length;
            var flag = false;
            for (var i = count-1; i >= 0; i--) {
                if ($(trs[i]).attr('id') == $(tr).attr('id')) {
                    flag = true;
                    $(tr).remove();
                }
                else
                {
                    if (!flag) {
                        var lvl = Number($(trs[i]).attr('id').replace('tr_', '')) -1;
                        $('#lblLvl_' + (i+1)).text(lvl);
                    }
                }
            }            
        }


        function testRegex(rgx, str, bflag) {
            if (str == "") {
                if (bflag)
                { return true; }
                else { return false; }
            }
            return result = rgx.test(str);
        }
        function ConventDate(str){
            str = str.replace(/-/g,"/");
            var date = new Date(str);
            return date;
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

        function testInput(obj) {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;

            var flag = false;
            var bError=false;
            if (id == $('#<%=txt_name.ClientID%>').attr('id'))
            {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
            }
            
            if (id == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').attr('id'))
            {
                if ($('#' + id).val() == "")
                {
                    bError = true;
                }
                //if(!isDate($('#' + id).val()))
                //{
                //    bError=true;
                //}
            }

            if (id == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').attr('id')) {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
                //if(!isDate($('#' + id).val()))
                //{
                //    bError=true;
                //}
            }
            if ($('#rad3').prop('checked'))
            {
                if ($(obj).attr('name') == "Meet") {
                    regex = /^\d+(\.\d{2})?$/;
                    parent = null;
                    flag = true;
                }
            }
            else
            {
                if ($(obj).attr('name') == "MeetNumber") {
                    regex = /^[0-9]*$/;
                    parent = null;
                    flag = true;
                }
            }

            if ($(obj).attr('name') == "redus") {
                regex = /^\d+(\.\d{2})?$/;                
                parent = null;
                var chk = $('#chk1' + id.replace('txt_redus', ''));
                flag = $(chk).prop('checked');
            }
            if ($(obj).attr('name') == "point") {
                regex = /^[0-9]*$/;
                parent = null;
                var chk = $('#chk3' + id.replace('txt_redus', ''));
                flag = $(chk).prop('checked');
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
            setBtnEnable();
            return !bError;
        }

        function setBtnEnable() {
            var error = $(".errorInput");
            if (error.length > 0) {
                
                $('#saveBtn').removeAttr('disabled');
                $('#saveBtn').prop('disabled', 'disabled');
                $('#nextBtn').removeAttr('disabled');
                $('#nextBtn').prop('disabled', 'disabled');
            }
            else {
                $('#saveBtn').removeAttr('disabled');
                $('#nextBtn').removeAttr('disabled');
            }
        }
        
        function beforeSave() {
            $('input[type="text"]').each(function () {
                testInput(this);
            });

            var error = $(".errorInput");
            if (error.length > 0) {
                var obj = $(error[0]).find('input[type=text]');
                if ($(obj).attr('id') == $('#<%=txt_name.ClientID%>')) {
                    ShowMsg('请输入正确的活动名称！', false);
                   
                }
                if ($(obj).attr('id') == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker')) {
                    ShowMsg('请输入正确的开始时间！', false);
                   
                }
                if ($(obj).attr('id') == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker')) {
                    ShowMsg('请输入正确的结束时间！', false);
                   
                }

                if ($(obj).attr('name') == "Meet") {
                    ShowMsg('请输入正确的满足金额！', false);
                   
                }

                if ($(obj).attr('name') == "MeetNumber") {
                    ShowMsg('请输入正确的满足次数！', false);

                }

                if ($(obj).attr('name') == "redus") {
                    ShowMsg('请输入正确的减免金额！', false);
                   
                }
                if ($(obj).attr('name') == "point") {
                    ShowMsg('请输入正确的赠送积分！', false);
                    
                }
                $(obj).focus();
                return false;
            }            
              var flag = false;
              $('#memberGradediv').find('input').each(function () {
                  if ($(this).prop('checked')) {
                      flag = true;
                  }
              });
              $('#memberDefualtGroupdiv').find('input').each(function () {
                  if ($(this).prop('checked')) {
                      flag = true;
                  }
              });
              $('#memberCustomGroupdiv').find('input').each(function () {
                  if ($(this).prop('checked')) {
                      flag = true;
                  }
              });
              if (!flag) {
                  ShowMsg('请选择会员等级！', false);
                  return false;
              }
              return true;
        }


        function GetAttend() {

            var stk=new Array();
            $('#maintable').find('tr').each(function () {
                var lvl = Number($(this).attr('id').replace('tr_', ''));
                var meet = $('#txt_Meet_' + lvl).val();
                var meetNumber = $('#txt_MeetNumber_' + lvl).val();
                var redus = 0;

                if ($('#rad3').prop('checked')) {
                    meetNumber = "0";
                }
                else
                {
                    meet = "0";
                }

                if ($('#chk1_' + lvl).prop('checked')) {
                    redus = $('#txt_redus_' + lvl).val();
                }
                var free = 0;
                if ($('#chk2_' + lvl).prop('checked')) {
                    free = 1;
                }
                var point = 0;
            
                if ($('#chk3_' + lvl).prop('checked')) {
                    point = $('#txt_point_' + lvl).val();
                }
                var coupon = 0;

                if ($('#chk4_' + lvl).prop('checked')) {
                    coupon = $('#sel_' + lvl).val();
                }
                stk.push({
                    meet: meet,
                    meetNumber:meetNumber,
                    redus: redus,
                    free: free,
                    point: point,
                    coupon: coupon
                });
            });
            return stk;
        }


        function saveData() {
            if (!beforeSave()) return;
            var name = $('#<%=txt_name.ClientID%>').val();
            var begin = $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val();
            var end = $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val();
            var maxNum = Number($('#<%=ddl_maxNum.ClientID%>').val());
            var attendType = 0;
            var meetType = 0;
            if ($('#rad1').prop('checked'))
            {
                attendType = 0;
            }
            else
            {
                attendType = 1;
            }

            var memberlvl = "";
            var defualtGroup="";
            var customGroup="";
            var isAllmember = false;
            var type;
            
            if ($('#memberdiv input:checked').length == 0) {
                ShowMsg("请选择适用会员！")
                return false;
            }
            if ($('#allmember').prop('checked')) {
                isAllmember = true;
                memberlvl = "0";
                defualtGroup="0";
                customGroup="0";
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
            if ($('#product_rd1').prop('checked')) {
                type = 0;
            }
            else {
                type = 1;
            }
            if ($('#rad3').prop('checked')) {
                meetType = 0;
            }
            else {
                meetType = 1;
            }


            var stk = GetAttend();
            var conpons = {
                id: '<%=_id%>',
                name: name,
                begin: begin,
                end: end,
                memberlvl: memberlvl,
                defualtGroup:defualtGroup,
                customGroup:customGroup,
                maxNum: maxNum,
                type: type,
                attendType: attendType,
                meetType:meetType,
                stk: JSON.stringify(stk)
            };

            $.ajax({
                type: "post",
                url: "SaveActivityHandler.ashx",
                data: conpons,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        if (type == 0) //回列表
                        {
                            ShowMsg("创建满减活动成功！", true);
                            HiTipsShow("活动保存成功，2秒后转至活动列表！", "success", function () {
                               document.location.href = "ActivityList.aspx";
                            })

                            
                        }
                        else {
                            //ShowMsg("创建满减活动成功！", true);
                            document.location.href = "AddProductToActivity.aspx?id=" + data.data;
                        }
                    }
                    else {
                        ShowMsg("创建满减活动失败！（" + data.data + ")", false);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2 id="atitle">添加满减活动</h2>            
        </div>

        <div>
            <label class="titleLable">活动信息：</label>
            <div class="form-group">
                <label class="col-xs-3 control-label"><em>*</em>活动名称：</label>
                <div class="col-xs-4">
                    <asp:TextBox runat="server" class="form-control" ID="txt_name" Width="200px" ></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-3 control-label"><em>*</em>有效期限：</label>
                <div class="form-inline">
                    <Hi:DateTimePicker runat="server" Style="margin-left: 15px;" CssClass="form-control"
                        ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" />
                    <label>至</label>
                    <Hi:DateTimePicker runat="server" CssClass="form-control" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" />
                </div>
            </div>

            <label class="titleLable">规则设置：</label>
            <div class="form-group">
                <label class="col-xs-3 control-label"><em>*</em>适用会员：</label>
                <div  class="form-inline col-xs-7">
                   <%-- <div id="allmemberGradediv">
                        <input type="checkbox" value="0" id="allmemberGrade" />全部会员
                    </div>
                    <div id="memberGradediv" style="vertical-align:central;">

                    </div>--%>
                     <Hi:SetMemberRange runat="server" ID="SetMemberRange" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-3 control-label">参与限制：</label>
                <div style="position: relative; width: 200px; float: left">
                    <asp:DropDownList runat="server" ID="ddl_maxNum" Style="margin-left: 15px;" CssClass="form-control">
                    </asp:DropDownList>                    
                </div>
            </div>

            <label class="titleLable">优惠设置：</label>
            <div class="form-group">
                <label class="col-xs-3 control-label"><em>*</em>优惠方式：</label>
                <div class="form-inline">
                    <input type="radio" name="attendsetting" id="rad1" style="margin-left: 15px;"  checked="checked" /> 普通优惠
                </div>
                <div class="form-inline">
                    <input type="radio" name="attendsetting" id="rad2" style="margin-left: 15px;" /> 多级优惠<label
                        class="graytext">（每级优惠不累积叠加）</label>                   
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-3 control-label">优惠条件：</label>
                <div class="form-inline" style="padding-top:7px;">
                    <input type="radio" name="meetType" id="rad3" style="margin-left: 15px;" checked="checked" />满额
                    <input type="radio" name="meetType" id="rad4" style="margin-left: 15px;" />满件
                </div>
            </div>


            <div class="form-inline">
                <div class="sell-table" style="margin-left: 15%; width:100%;">
                    <div class="title-table" style="width: 80%;">
                        <table  class="table">
                            <thead>
                                <tr style="width: 100%;" class="table_title">
                                    <th style="width: 10%;">层级</th>
                                    <th style="width: 20%;">优惠门槛</th>
                                    <th style="width: 60%;">优惠方式（可多选）</th>
                                    <th style="width: 10%;">操作</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="content-table" style="width: 80%;">
                        <table class="table" id="maintable">
                            <tr id="tr_1">
                                <td style="width: 10%;">
                                    <label id="lblLvl_1"  for="lvl">1</label>
                                </td>
                                <td style="width: 20%; text-align:left;">
                                    <label>满</label>
                                    <input type="text" name="Meet" id="txt_Meet_1" class="form-control" style="width:100px;"/>
                                    <input type="text" name="MeetNumber" id="txt_MeetNumber_1" class="form-control" style="width: 100px; display:none; " />
                                    <label for="meetType" id="lblMeetType_1">元</label>
                                </td>
                                <td style="width: 60%; text-align: left; vertical-align: central;">
                                    <div class="divClass">
                                        <input type="checkbox" name="select_1" id="chk1_1" />
                                        减 <input type="text" name="redus" id="txt_redus_1" class="form-control" style="width:100px;" /> 元
                                    </div>
                                    <div class="divClass">
                                        <input type="checkbox" name="select_1" id="chk2_1" /> 包邮
                                    </div>
                                    <div class="divClass">
                                        <input type="checkbox" name="select_1" id="chk3_1" />
                                        送 <input type="text" name="point" id="txt_point_1" class="form-control" style="width:100px;" /> 积分
                                    </div>
                                    <div class="divClass">
                                        <input type="checkbox" name="select_1" id="chk4_1" />
                                        送优惠券
                                        <select id="sel_1" class="form-control selectClass" style="width:150px;"></select>
                                        <a href="NewCoupon.aspx" target="_blank">新建</a>
                                    </div>
                                </td>
                                <td style="width:10%; ">
                                    <a href="javascript:void(0);" class="delDiv" id="del_1" style="display:none" onclick="Dellvl(this);">删除</a>
                                </td>

                            </tr>
                        </table>
                        <table class="table" id="addtable" style="display:none;">
                            <tr>
                                <td style="text-align:left;">
                                    <a onclick="Addlvl();" style="cursor:pointer;">+ &nbsp;新增一级优惠</a>
                                    <label class="graytext">最多可设置5个层级</label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>

            <label class="titleLable">选择活动商品：</label>
            <div class="form-group">
                <label class="col-xs-3 control-label">活动商品：</label>
                <div class="form-inline">
                    <input type="radio" id="product_rd1" style="margin-left: 15px;" name="ProductsGroup" />
                    <label style="margin-left: 3px;">全部商品参与</label>
                    <label class="graylabel">&nbsp;&nbsp;&nbsp;提示：新发布的商品也能适应此规则哦！</label>
                </div>
                <div class="form-inline">
                    <input type="radio" id="product_rd2" style="margin-left: 15px;" name="ProductsGroup" />
                    <label style="margin-left: 3px; margin-right: 3px;">部分商品参与</label>
                    <label class="graylabel">&nbsp;&nbsp;已选商品(<em id="productCount" runat="server">0</em>)件</label>
                </div>
            </div>
        </div>
        <div style="height: 50px;"></div>
        <div class="footer-btn navbar-fixed-bottom autow">
            <button type="button" id="saveBtn" class="btn btn-primary" onclick="saveData();">保存</button>
            <button type="button" id="nextBtn" class="btn btn-primary" onclick="saveData();">下一步，选择宝贝</button>
            <label id="labelstatus">全部商品参与状态</label>
        </div>
    </form>
</asp:Content>
