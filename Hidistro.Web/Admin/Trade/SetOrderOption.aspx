<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SetOrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.SetOrderOption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtCloseOrderDays': {
                    validators: {
                        notEmpty: {
                            message: '过期几天自动关闭订单不能为空'
                        },
                        regexp: {
                            regexp: /^(90|[1-9]|[1-8]\d?)$/,
                            message: '只能是1-90之间的整数'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtFinishOrderDays': {
                    validators: {
                        notEmpty: {
                            message: '发货几天自动完成订单不能为空'
                        },
                        regexp: {
                            regexp: /^(90|[1-9]|[1-8]\d?)$/,
                            message: '只能是1-90之间的整数'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtMaxReturnedDays': {
                    validators: {
                        notEmpty: {
                            message: '收货几天后不能够退货不能为空'
                        },
                        regexp: {
                            regexp: /^(90|[1-9]|[1-8]\d?)$/,
                            message: '只能是1-90之间的整数'
                        }
                    }
                },
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>订单设置</h2>
    </div>
    <form runat="server" class="form-horizontal">

        <div class="form-group" style="display: none">
            <label class="col-xs-2 control-label resetSize">
                <span>显示几天内订单数：</span>
            </label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtShowDays" CssClass="form-control resetSize" runat="server" MaxLength="10" />
               <%-- <small>前台发货查询中显示最近几天内的订单项</small>--%>
            </div>
        </div>
        <div class="form-group">
            <label class="col-xs-2 control-label resetSize">
                <em>*</em><span>拍下</span>
            </label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtCloseOrderDays" CssClass="form-control resetSize inputw100 inl" runat="server" MaxLength="10" /> 天没付款的订单自动关闭
               <%-- <small>下单后过期几天系统自动关闭未付款订单</small>--%>
            </div>
        </div>
        <div class="form-group">
            <label class="col-xs-2 control-label resetSize">
                <em>*</em><span>发货</span>
            </label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtFinishOrderDays" CssClass="form-control resetSize inputw100 inl" runat="server" MaxLength="10" /> 天后自动完成订单
               <%-- <small>发货几天后，系统自动把订单改成已完成状态</small>--%>
            </div>
        </div>
        <div class="form-group" style="display:none">
            <label class="col-xs-2 control-label resetSize">
                <em>*</em><span>收货</span>
            </label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtMaxReturnedDays" CssClass="form-control resetSize inputw100 inl" runat="server" MaxLength="10" Text="15" /> 天后不能再申请售后
             <%--   <small>确认收货几天后的订单不允许退货</small>--%>
            </div>
        </div>
        <div class="form-group" style="display: none">
            <label class="col-xs-2 control-label resetSize">
                <span>发货几天自动完成订单：</span>
            </label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtTaxRate" CssClass="form-control resetSize" runat="server" MaxLength="5" />%
                <small>发票收税比率，0表示顾客将不承担订单发票税金</small>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-10 col-xs-offset-3 setmargin">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success float inputw100" OnClick="btnSave_Click" />
            </div>
        </div>
    </form>
</asp:Content>
