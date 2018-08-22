<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master"
     AutoEventWireup="true" CodeBehind="WXSendDemo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.WXSendDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var copy = new ZeroClipboard(document.getElementById("copyurl"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
            });

            var copy2 = new ZeroClipboard(document.getElementById("copytoken"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy2.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');

                window.location.href = "WXConfigBinkOK.aspx";
            });
        })

        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

          
    <form id="form1" runat="server">
        <p>
            订单号：<asp:TextBox ID="txtOrderId" runat="server">201509210433814</asp:TextBox>
        </p>
        <p>
            消息：<asp:Label runat="server"  ID="lbMsg" Text="" /> </p>
        <p>
            一、新订单</p>
        <p>
            <asp:Button ID="btnSend_NewOrder" runat="server" OnClick="btnSend_NewOrder_Click" Text="发微信消息" />
        </p>
        <p>
            &nbsp;</p>
        <p>
            二、新品上线</p>
        <p>
            <asp:Button ID="btnSend_NewProduct" runat="server" OnClick="btnSend_NewProduct_Click" Text="发微信消息" />
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
    </form>

          
</asp:Content>
