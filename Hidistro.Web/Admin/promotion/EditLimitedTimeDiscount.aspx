<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditLimitedTimeDiscount.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.EditLimitedTimeDiscount" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagPrefix="Hi" TagName="SetMemberRange" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //数据验证
            $(".container").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_btnSave',
                'ctl00$ContentPlaceHolder1$txtActivityName': {
                    validators: {
                        notEmpty: {
                            message: '活动标签不能为空'
                        },
                        stringLength: {
                            min: 2,
                            max: 5,
                            message: '活动标签2-5个字符'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtLimitNumber': {
                    validators: {
                        notEmpty: {
                            message: '每人限购不能为空'
                        },
                        regexp: {
                            regexp: /^(0|[1-9]+?[0-9]*)$/,
                            message: '每人限购只能输入实数型数值'
                        }
                    }
                }
            });
            //按钮的显示，隐藏
            var id = getUrlParam("id");
            if (id == null || id == "") {
                $("#ctl00_ContentPlaceHolder1_btnSave").hide();
                $("#btnUpdateProduct").hide();
                $("#ctl00_ContentPlaceHolder1_btnSaveAndNext").show();
            }
            else {
                $("#ctl00_ContentPlaceHolder1_btnSave").show();
                $("#btnUpdateProduct").show();
                $("#ctl00_ContentPlaceHolder1_btnSaveAndNext").hide();
            }

            var id = getUrlParam("id");
            if (id == "" || id == null) {
                $(".spEdit").html("添加");
            }
        });
        
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2><span class="spEdit">编辑</span>限时折扣</h2>
        </div>
        <div class="set-switch resetBorder">
            <p><strong>基本信息：</strong></p>
            <div class="form-horizontal clearfix">
                <div class="form-group setmargin">
                    <label class="col-xs-3 control-label pt4"><em>*</em>活动名称：</label>
                    <div class="form-inline col-xs-9">
                        <asp:TextBox runat="server" class="form-control resetSize" ID="txtActivityName" MaxLength="10"></asp:TextBox>
                        <p class="colorc">显示在商品详情的价格后面，长度在2-5字符之间</p>
                    </div>
                </div>
                <div class="form-group setmargin mb20">
                    <label class="col-xs-3 pad resetSize control-label pt4" for="setdate"><em>*</em>活动时间：</label>
                    <div class="form-inline journal-query col-xs-9">
                        <div class="form-group">
                            <Hi:DateTimePicker runat="server" name="canTest" CssClass="form-control resetSize"
                                ID="dateBeginTime" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="开始时间" />
                            <label>至</label>
                            <Hi:DateTimePicker runat="server" name="canTest" CssClass="form-control resetSize"
                                ID="dateEndTime" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" PlaceHolder="结束时间" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-xs-3 control-label pt4">活动备注：</label>
                    <div class="form-inline col-xs-9">
                        <asp:TextBox runat="server" class="form-control resetSize" ID="txtDescription" MaxLength="30"></asp:TextBox>
                         <p class="colorc">活动备注仅在后台页面显示，30字以内</p>
                    </div>
                </div>
            </div>
            <p><strong>活动设置：</strong></p>
            <div class="form-horizontal clearfix">
                <div class="form-group setmargin">
                    <label class="col-xs-3 control-label pt4"><em>*</em>每人限购：</label>
                    <div class="form-inline col-xs-9">
                        <asp:TextBox runat="server" class="form-control resetSize" MaxLength="3" ID="txtLimitNumber"></asp:TextBox>
                        <p class="colorc">值为0则不限购</p>
                    </div>
                </div>
                <div class="form-group setmargin">
                    <label class="col-xs-3 control-label pt2"><em>*</em>适用会员：</label>
                    <div class="form-inline col-xs-9">
                        <Hi:SetMemberRange runat="server" ID="memberRange" />
                    </div>
                </div>
            </div>
             <div class="form-inline" style="margin-left:260px">
                 <asp:Button runat="server" ID="btnSaveAndNext" Text="下一步,添加活动商品"  class="btn btn-primary" OnClick="btnSaveAndNext_Click"/>
                 <asp:Button runat="server" ID="btnSave" Text="保存" OnClick="btnSave_Click" class="btn btn-primary"/>
                 <a href="LimitedTimeDiscountProduct.aspx?id=<%=id %>" id="btnUpdateProduct" class="btn btn-primary" >修改活动商品</a>
             </div>
        </div>
    </form>
</asp:Content>
