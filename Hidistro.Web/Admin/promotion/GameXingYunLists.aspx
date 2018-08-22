<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="GameXingYunLists.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.GameXingYunLists" %>
<%@ Register Src="UCGameLists.ascx" TagName="UCGameLists" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            setActive();
        });
        function setActive() {
            var isFinished = $("#isFinished").val();
            if (isFinished == "0") {
                $('#tabHeader_over').removeClass();
                $('#tabHeader_active').addClass('active');
                $("#search").hide();
            }
            else {
                $('#tabHeader_active').removeClass();
                $('#tabHeader_over').addClass('active');
                $("#search").show();

            }
        }
    </script>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_grdCoupondsList th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:center; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_grdCoupondsList td {margin:0px;border-left:0px;border-right:0px;vertical-align:middle;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <input type="hidden" id="isFinished" value="<%=isFinished%>" />
        <div class="page-header">
            <h2><%=PGameType.ToString() %></h2>
        </div>
        <div class="blank">
            <a href="AddGameXingYun.aspx" class="btn btn-primary">新建<%=PGameType.ToString() %></a>
        </div>
        <div id="allProductsDiv">
            <div class="play-tabs">
                <uc1:UCGameLists ID="UCGameLists1" runat="server" />
            </div>
        </div>
    </form>
</asp:Content>
