<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="NewCoupon.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.NewCoupon" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagName="SetMemberRange" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .smallTitleCss{color:black; font-weight:bold; font-size:13px; margin-left:5px;}
        .graylineCss{ background-color: #ccc; height:1px;width:auto;margin-left:5px;margin-right:5px;margin-top:5px;margin-bottom:10px;}
        .graylabel{font-size:10px;color:#737373;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type="radio"]').each(function () {
                $(this).click(function () {
                    radionBtnClick(this);                   
                });
            });         
            
            readMemberGrades();
            readCouponType();
            setConponsMobile();

            $('input[type="radio"][name="ProductsGroup"]').each(function () {
                $(this).click(function () {
                    var id = $(this).attr('id');
                    if(id=="product_rd1")
                    {                      
                        $('#saveBtn').css('display', '');
                        $('#nextBtn').css('display', 'none');
                        $('#labelstatus').text('全部商品参与状态');
                    }
                    else
                    {      
                        $('#saveBtn').css('display', 'none');
                        $('#nextBtn').css('display', '');
                        $('#labelstatus').text('部分商品参与状态');
                    }
                });
            });
            $('#product_rd1').click();
        });

        function testRegex(rgx, str) {
            if (str == "") return true;
            return result = rgx.test(str);
        }


        function testInput(obj) {           
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;

            if (id == $('#<%=txt_name.ClientID%>').attr('id')) {
                var flag = true;
                if($(obj).val()=="")
                {
                    flag = false;
                }
                else if($(obj).val().length>20)
                {
                    flag = false;
                }
                if(!flag)
                {
                    parent = $(obj).parent();
                    var html = '<small id="smalltxt" class="help-block" data-bv-validator="notEmpty" data-bv-for="username" data-bv-result="NOT_VALIDATED">请输入优惠券名称，长度1-20位</small>';
                    $('#smalltxt').remove();
                    parent.append(html);
                    parent = $(obj).parent().parent();
                    $(parent).removeClass();
                    $(parent).addClass("form-group has-error");
                }
                else
                {
                    $('#smalltxt').remove();
                    parent = $(obj).parent().parent();
                    $(parent).removeClass();
                    $(parent).addClass("form-group");;
                }
            }
            else
            {
                // regex = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
                var flag = false;
                if (id == $('#<%=txt_Value.ClientID%>').attr('id')) {
                    regex = /^[1-9](\d*)?$/;
                    parent = $(obj).parent().parent();
                    flag = true;
                }
                if (id == $('#<%=txt_conditonVal.ClientID%>').attr('id')) {
                    if ($('#<%=rdt_1.ClientID%>').attr('checked') == 'checked') {
                        return true;
                    }
                    else {
                        regex = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
                        parent = $(obj).parent().parent();
                        flag = true;
                    }
                }
                if (id == $('#<%=txt_totalNum.ClientID%>').attr('id')) {
                    regex = /^[0-9]*$/;
                    parent = $(obj).parent().parent();
                    flag = true;
                }

                if (flag)
                {
                    if (testRegex(regex, content) && content != "") {
                        $(parent).removeClass();
                        $(parent).addClass("form-group");
                    }
                    else {
                        $(parent).removeClass();
                        $(parent).addClass("form-group has-error");
                    }
                }                
            }           
            setBtnEnable();
        }
        
        function setConponsMobile() {
            $('#<%=txt_name.ClientID%>').blur(function () {
                testInput(this);
            });

            $('#<%=txt_Value.ClientID%>').blur(function () {             
                var val=$('#<%=txt_Value.ClientID%>').val();
                if (val == "") val = "0.00";
                $('#pValue').text("￥" + Number(val).toFixed(2));                
                changeConditon();                
                testInput(this);
            });

            $('#<%=txt_conditonVal.ClientID%>').blur(function () {
                
                testInput(this);
                changeConditon();
            });

            $('input[type="radio"][name="ctl00$ContentPlaceHolder1$conditonGroup"]').each(function () {
                $(this).click(function () {                   
                    changeConditon();
                    if($(this).attr('id')=='ctl00_ContentPlaceHolder1_rdt_1')
                    {                      
                        $('#<%=txt_conditonVal.ClientID%>').attr('disabled', 'disabled');
                    }
                    else
                    {                     
                        $('#<%=txt_conditonVal.ClientID%>').removeAttr('disabled');
                    }
                });
            });

            $('#<%=txt_totalNum.ClientID%>').blur(function () {
                testInput(this);
            });

            $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').change(function () {
                var val = $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val();
                $('#pEndDate').text("到期时间：  " + val);
            });

            $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').change(function () {
                var val = $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val();
                $('#pBeginDate').text("生效时间：  " + val);
            });

        }

        function setBtnEnable() {
            var error = $(".has-error");
            if(error.length>0)
            {
                $('#saveBtn').removeAttr('disabled');
                $('#saveBtn').attr('disabled', 'disabled');
                $('#nextBtn').removeAttr('disabled');
                $('#nextBtn').attr('disabled', 'disabled');
            }
            else
            {
                $('#saveBtn').removeAttr('disabled');
                $('#nextBtn').removeAttr('disabled');
            }
        }

        function changeConditon()
        {
            var val = $('#<%=txt_Value.ClientID%>').val();
            if ($('#<%=rdt_1.ClientID%>').attr('checked') != "checked")
            {
                var conditon = $('#<%=txt_conditonVal.ClientID%>').val();
                if (conditon == "") conditon = "0";
                if (Number(val) > Number(conditon))
                {
                    if ($('#<%=txt_Value.ClientID%>').val() != "" && $('#<%=txt_conditonVal.ClientID%>').val() != "")
                    {
                        ShowMsg("减免金额不能大于满足金额！", false);
                    }                    
                }
                $('#pCondtion').text("满" + conditon + "减" + Number(val));
            }
            else
            {
                $('#<%=txt_conditonVal.ClientID%>').val("");
                $('#<%=txt_conditonVal.ClientID%>').parent().parent().removeClass();
                $('#<%=txt_conditonVal.ClientID%>').parent().parent().addClass("form-group");
                $('#pCondtion').text("直减" + Number(val));
            }
        }
       
        function radionBtnClick(obj)
        {
            var name = $(obj).attr('name');
            var id = $(obj).attr('id');
            $('input[name="' + name + '"]').each(function () {
                if ($(this).attr('id') != id) {
                    $(this).removeAttr('checked');
                }
                else
                {
                    $(this).attr('checked', 'checked');
                }
            });                   
        }



        function readMemberGrades() {
            $.ajax({
                type: "post",
                url: "GetMemberGradesHandler.ashx?action=getMemberGrade",
                data: {},
                dataType: "json",
                success: function (data) {
                    if(data.type=="success")
                    {
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
                                if(hasChecked == all)
                                {
                                    $('#allmemberGrade').attr('checked', 'checked');
                                    $('#allmemberGrade').prop("checked", true);
                                }
                                else
                                {
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
                    }
                    else
                    {
                        ShowMsg("加载会员等级信息失败!");
                    }
                }
            });
        }

        function readCouponType()
        {
            $.ajax({
                type: "post",
                url: "GetMemberGradesHandler.ashx?action=getcoupontype",
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        var resultArr = data.data;
                        //$('#memberGradediv').empty();
                        var shtml = '';
                        $.each(resultArr, function (i,result) {
                            shtml += '<label class="middle" style="margin-left: 15px; "> <input type="checkbox" name="couponType" value="' + result.id + '" />' + result.Name + '</label>';
                            
                        });
                        $('#couponTypeDiv').append(shtml);
                       
                    }
                    else {
                        ShowMsg("加载优惠券类型信息失败!");
                    }
                }
            });
        }

        function beforeSave()
        {
           
            $('input[type="text"]').each(function () {
                testInput(this);
            });

            var error = $(".has-error");
            if (error.length > 0)
            {
                var obj = $(error[0]).find('input[type=text]');
                if($(obj).attr('id')==$('#<%=txt_name.ClientID%>'))
                {
                    ShowMsg('请输入正确的优惠券名称！', false);
                }
                if ($(obj).attr('id') == $('#<%=txt_Value.ClientID%>')) {
                    ShowMsg('请输入正确的优惠券面值！', false);
                }
                if ($(obj).attr('id') == $('#<%=txt_conditonVal.ClientID%>')) {
                    ShowMsg('请输入正确的满足金额！', false);
                }
                if ($(obj).attr('id') == $('#<%=txt_totalNum.ClientID%>')) {
                    ShowMsg('请输入正确的发行数量！', false);
                }
                $(obj).focus();
                return false;
            }
            var val = Number($('#<%=txt_Value.ClientID%>').val());
            var conditon = Number($('#<%=txt_conditonVal.ClientID%>').val());
            if(val>conditon)
            {
                if ($('#<%=rdt_2%>').attr("checked") == "checked")
                {
                    ShowMsg('优惠券面值不能大于满足金额！', false);
                    $('#<%=txt_Value.ClientID%>').focus();
                    return false;
                }                
            }

            if ($('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val() == "")
            {
                ShowMsg('请选择优惠券生效日期！', false);
                return false;
            }
            if ($('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val() == "") {
                ShowMsg('请选择优惠券失效日期！', false);
                return false;
            }
            
            var begin = strToDate($('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val());
            var end = strToDate($('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val());
            if(begin>end)
            {
                ShowMsg('优惠券失效日期不能早于生效日期！', false);
                return false;
            }
            if (end <= new Date()) {
                ShowMsg('优惠券失效日期不能早于当前日期！', false);
                return false;
            }
            var flag = false;
            $('#memberGradediv').find('input').each(function () {
                if ($(this).prop('checked'))
                {
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
            if(!flag)
            {
                ShowMsg('请选择会员等级！', false);
                return false;
            }
            if ($("#couponTypeDiv").find("input:checked").size() == 0)
            {
                ShowMsg('请选择优惠券类型！', false);
                return false;
            }
            return true;
        }

        function strToDate(str) {
            var tempStrs = str.split(" ");
            var dateStrs = tempStrs[0].split("-");
            var year = parseInt(dateStrs[0], 10);
            var month = parseInt(dateStrs[1], 10) - 1;
            var day = parseInt(dateStrs[2], 10);
            var date = new Date(year, month, day, 0, 0, 0);
            return date;
        }

        function saveData()
        {
            if (!beforeSave()) return;
            var val = $('#<%=txt_Value.ClientID%>').val();
            var name = $('#<%=txt_name.ClientID%>').val();
            var condtion = $('#<%=txt_conditonVal.ClientID%>').val();
            if ($('#<%=rdt_1.ClientID%>').attr('checked') == 'checked')
            {
                condtion = "0";
            }
            var begin = $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val();
            var end = $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val();
            var totalNum = $('#<%=txt_totalNum.ClientID%>').val();
            var memberlvl = "";
            var defualtGroup="";
            var customGroup = "";
            var isAllmember = false;
            var type;
            if ($('#allmember').attr('checked') == "checked")
            {
                isAllmember = true;
            }
            else
            {
                isAllmember = false;
                $('#memberGradediv').find('input').each(function () {
                    if ($(this).attr('checked') == "checked") {
                        memberlvl +=','+$(this).val();
                    }
                });
                if(memberlvl.length>1)
                {
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
            
            var maxNum = Number($('#<%=ddl_maxNum.ClientID%>').val());
            if ($('#product_rd1').attr('checked') == "checked")
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            var couponType = '';
            $("#couponTypeDiv").find("input:checked").each(function () {
                if (couponType != '')
                {
                    couponType += ',';
                }
                couponType += $(this).val();
            });

            
            var conpons = {
                name: name,
                val: val,
                condition: condtion,
                begin: begin,
                end: end,
                total: totalNum,
                isAllMember: isAllmember,
                memberlvl: memberlvl,
                defualtGroup: defualtGroup,
                customGroup:customGroup,
                maxNum: maxNum,
                type: type,
                couponType: couponType
            };
            $.ajax({
                type: "post",
                url: "SaveCouponDataHandler.ashx",
                data: conpons,
                dataType: "json",
                success: function (data) {
                    if(data.type=="success")
                    {                      
                        if(type==0) //回优惠券列表
                        {                           
                            ShowMsg("创建优惠券成功！", true);
                            document.location.href = "CouponsList.aspx";
                        }
                        else
                        {
                            ShowMsg("创建优惠券成功！", true);
                            document.location.href = "AddProductToCoupon.aspx?id=" + data.data;
                        }
                    }
                    else
                    {
                        ShowMsg("创建优惠券失败！（" + data.data+ ")", false);
                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>优惠券</h2>
            <%--<small>优惠券分为有门槛和无门槛优惠券，主要作用有提升客单价、提高回购率、吸引新老顾客形成购买转化。</small>--%>
        </div>
        <table style="width:100%;">
            <tr>
                <td style="width:360px;">
                    <div class="edit-text-left">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">优惠券</div>
                                    </div>
                                    <div class="set-overflow" style="height:450px;">
                                        <div class="white-box">
                                            <div class="coupons autol">
                                                <div class="fl">
                                                    优惠券
                                                </div>
                                                <div class="fr">
                                                    <p class="mo" id="pValue">￥100.00</p>
                                                    <p class="over" id="pCondtion" >满500减100</p>
                                                    <p id="pBeginDate">生效时间：<span>2015-07-28 11:18:11</span></p>
                                                    <p id="pEndDate">到期时间：<span>2015-07-28 11:18:11</span></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mobile-nav">
                                        <ul class="clearfix">
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
                                        </ul>
                                    </div>
                                </div>
                                <div class="clear-line">
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="width:auto; vertical-align:top;">
                    <div class="set-switch" style="width: 95%;">
                        <label class="smallTitleCss">优惠券券面信息：</label>
                        <div class="graylineCss"></div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>优惠券名称：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" class="form-control" ID="txt_name" Width="200px"></asp:TextBox>                          
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>面值：</label>
                            <div class="form-inline" >                                
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;" MaxLength="9" ID="txt_Value"></asp:TextBox>
                                <label style="margin-left:3px;">元</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>使用条件：</label>
                            <div class="form-inline">
                                <input type="radio" id="rdt_1" style="margin-left: 15px;" runat="server"  name="conditonGroup"  checked="true" />
                                <label style="margin-left: 3px;">不限制</label>
                            </div>
                            <div class="form-inline">
                                <input type="radio" id="rdt_2" style="margin-left: 15px;" name="conditonGroup" runat="server" />
                                <label style="margin-left: 3px; margin-right:3px;">订单满</label>
                                <asp:TextBox runat="server" class="form-control" Width="100px" disabled="disabled"
                                    ID="txt_conditonVal"></asp:TextBox >
                                <label style="margin-left: 3px; margin-right: 3px;">元可用</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>有效期限：</label>
                            <div class="form-inline">
                                <Hi:DateTimePicker runat="server" Style="margin-left: 15px;" CssClass="form-control" ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" />
                                <label>至</label>
                                <Hi:DateTimePicker runat="server" CssClass="form-control" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>优惠券类型：</label>
                            <div class="form-inline" style="padding-top:7px;" id="couponTypeDiv">
                               
                            </div>
                        </div>
                    </div>
                    <div class="set-switch" style="width: 95%;">
                        <label class="smallTitleCss">基本规则：</label>
                        <div class="graylineCss"></div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>发放总量：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;" ID="txt_totalNum"></asp:TextBox>
                                <label style="margin-left: 3px;">张</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>适用会员：</label>
                            <div class="form-inline" style="margin-left: 170px;">
                                <Hi:SetMemberRange runat="server" ID="SetMemberRange" />
                                <%--<div id="allmemberGradediv">
                                    <input type="checkbox" value="0" id="allmemberGrade" />全部会员
                                </div>
                                <div id="memberGradediv" >

                                </div>--%>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label">每人限领：</label>
                            <div style="position: relative; width: 200px; float: left">
                                <asp:DropDownList runat="server" ID="ddl_maxNum" Style="margin-left: 15px;" CssClass="form-control"></asp:DropDownList>
                                <small class="help-block" style="margin-left: 15px;">每个用户自助领券上限，如不选，则默认为1张</small>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label">活动商品：</label>
                            <div class="form-inline">
                                <input type="radio" ID="product_rd1" style="margin-left: 15px;" name="ProductsGroup"  checked="checked"/>                
                                <label style="margin-left: 3px;">全部商品参与</label>
                                <label class="graylabel">&nbsp;&nbsp;&nbsp;提示：新发布的商品也能适应此规则哦！</label>
                            </div>
                            <div class="form-inline">
                                <input type="radio" id="product_rd2" style="margin-left: 15px;" name="ProductsGroup" />
                                <label style="margin-left: 3px; margin-right: 3px;">部分商品参与</label>
                                <label class="graylabel">&nbsp;&nbsp;已选商品(<em>0</em>)件</label>                                
                            </div>
                        </div>                        
                    </div>
                </td>
            </tr>
        </table>
        <div style="height:50px;"></div>

        <div class="footer-btn navbar-fixed-bottom autow">
            <button type="button" id="saveBtn" class="btn btn-success" onclick="saveData();">保存</button>
            <button type="button" id="nextBtn" class="btn btn-primary" onclick="saveData();">下一步，选择宝贝</button>
            <label id="labelstatus">全部商品参与状态</label>
        </div>
    </form>
</asp:Content>
