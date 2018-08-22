<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="ShipAddress.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.ShipAddress" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-group {
            width: 700px;
            margin-bottom: 10px;
        }
    </style>
    <form runat="server">

        <div class="form-horizontal">
            <div class="set-switch">原地址：<asp:Literal ID="lblOriAddress" runat="server"></asp:Literal></div>
            <div class="form-group">
                <label for="ctl00_ContentPlaceHolder1_txtShipTo" class="col-xs-2 control-label"><em>*</em>收货人姓名：</label>
                <asp:TextBox ID="txtShipTo" runat="server" CssClass="form-control inputw200"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ctl00_ContentPlaceHolder1_txtCellPhone" class="col-xs-2 control-label"><em></em>手机号码：</label>
                <asp:TextBox ID="txtCellPhone" runat="server" CssClass="form-control inputw200"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ctl00_ContentPlaceHolder1_txtTelPhone" class="col-xs-2 control-label"><em></em>座机号码：</label>
                <asp:TextBox ID="txtTelPhone" runat="server" CssClass="form-control inputw200"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ddlRegions1" class="col-xs-2 control-label"><em></em>选择所在地：</label>
                <Hi:RegionSelector runat="server" ID="dropRegions" />
            </div>
            <div class="form-group">
                <label for="ctl00_ContentPlaceHolder1_txtAddress" class="col-xs-2 control-label"><em>*</em>详细地址：</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="multiLine" Width="390"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ctl00_ContentPlaceHolder1_txtZipcode" class="col-xs-2 control-label"><em></em>邮政编码：</label>
                <asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control inputw200"></asp:TextBox>
            </div>


            <div class="form-group">
                <div class="col-xs-offset-4 col-xs-10">
                    <asp:Button ID="btnMondifyAddress" runat="server" Text="确定" CssClass="btn btn-success" OnClientClick="return ValidationAddress()" />
                    <input type="button" value="关闭" class="btn btn-default" onclick="parent.$('#divmyIframeModal').modal('hide')" />
                </div>
            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_dropRegions').find('select').each(function () {
                $(this).removeClass();
                $(this).addClass('form-control inl autow mr5');
            });
        })
        function ValidationAddress() {
            arrytext = null;
            var shipTo = document.getElementById("ctl00_ContentPlaceHolder1_txtShipTo").value;
            if (shipTo.length < 2 || shipTo.length > 20) {
                parent.ShowMsg("收货人名字不能为空，长度在2-20个字符之间", false);
                return false;
            }
            var cellPhone = document.getElementById("ctl00_ContentPlaceHolder1_txtCellPhone").value;
            var telPhone = document.getElementById("ctl00_ContentPlaceHolder1_txtTelPhone").value;
            if (cellPhone.length == 0 && telPhone.length == 0) {
                parent.ShowMsg("手机号码和座机号码至少输入一个！", false);
                return false;
            }
            //else {
            if (cellPhone.length > 0 && ( cellPhone.length != 11 || !/^\d+$/.test(cellPhone))) {
                parent.ShowMsg("请正确输入手机号码", false);
                return false;
            }
            //}
            //if (telPhone.length == 0) {
            //    return true;
            //}
            //else {
            if (telPhone.length>0&&(telPhone.length < 3 || telPhone.length > 20 )) {
                alert("请正确输入座机号码");
                return false;
            }
            //}
            var address = document.getElementById("ctl00_ContentPlaceHolder1_txtAddress").value;
            if (address.length < 3 || address.length > 200) {
                parent.ShowMsg("详细地址不能为空，长度在3-200个字符之间", false);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
