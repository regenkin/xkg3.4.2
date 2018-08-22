<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="PrizeListsDaFuWen.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.PrizeListsDaFuWen" %>
<%@ Register Src="UCPrizeLists.ascx" TagName="UCPrizeLists" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <uc1:UCPrizeLists ID="UCPrizeLists1" runat="server" />
    </form>
</asp:Content>
