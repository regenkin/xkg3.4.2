<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="GameLists.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.GameLists" %>
<%@ Register src="UCGameLists.ascx" tagname="UCGameLists" tagprefix="uc1" %>
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
                $("input[type=checkbox]").hide();
                $("th").eq(0).text("").width(3);
            }
            else {
                $('#tabHeader_active').removeClass();
                $('#tabHeader_over').addClass('active');
                $("#search").show();
                $("th").eq(0).width(30);
            }
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <input type="hidden" id="isFinished" value="<%=isFinished%>" />
        <div class="page-header">
            <h2><%=PGameType.ToString() %></h2>
        </div>
        <div class="blank">
            <a href="AddGame.aspx" class="btn btn-primary">新建<%=PGameType.ToString() %></a>
        </div>
        <div id="allProductsDiv">
            <div class="play-tabs">
                <uc1:UCGameLists ID="UCGameLists1" runat="server" />
            </div>
        </div>
    </form>
</asp:Content>
