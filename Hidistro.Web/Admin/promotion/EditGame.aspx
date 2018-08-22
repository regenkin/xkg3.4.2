<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true"
    CodeBehind="EditGame.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.EditGame" %>

<%@ Register Src="~/Admin/promotion/UCGameInfo.ascx" TagPrefix="Hi" TagName="UCGameInfo" %>
<%@ Register Src="~/Admin/promotion/UCGamePrizeInfo.ascx" TagPrefix="Hi" TagName="UCGamePrizeInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript" src="Game.js?2016"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2><%= gameType.ToString() %></h2>
        </div>
        <div class="shop-navigation pb100 clearfix">
            <div class="fl">
                <div class="mobile-border">
                    <div class="mobile-d">
                        <div class="mobile-header">
                            <i></i>
                            <div class="mobile-title">店铺主页</div>
                        </div>
                        <div class="set-overflow">
                            <div class="white-box resetH">
                                <div class="topimg"></div>
                                <div class="gamePrizes" id="DivGameNum" style="text-align: center; padding-top: 10px;">
                                    <p id="pGameNum">
                                        您还有<asp:Label ID="lbGameNum" runat="server" ForeColor="White" Text=""></asp:Label>机会
                                    </p>
                                </div>
                                <div class="bottomimg"></div>
                                <div class="gamePrizes">
                                    <h3>活动奖品：</h3>
                                    <p>
                                        <span><span id="sPrizeGade0">
                                            <asp:Label ID="lbPrizeGade0" runat="server" Text=""></asp:Label></span></span>
                                    </p>
                                    <p>
                                        <span><span id="sPrizeGade1">
                                            <asp:Label ID="lbPrizeGade1" runat="server" Text=""></asp:Label></span></span>
                                    </p>
                                    <p>
                                        <span><span id="sPrizeGade2">
                                            <asp:Label ID="lbPrizeGade2" runat="server" Text=""></asp:Label></span></span>
                                    </p>
                                    <p>
                                        <span><span id="sPrizeGade3">
                                            <asp:Label ID="lbPrizeGade3" runat="server" Text=""></asp:Label></span></span>
                                    </p>
                                </div>
                                <div class="gamePrizes">
                                    <h3>活动说明：</h3>
                                    <p id="pGameDescription">
                                        <asp:Label ID="lbGameDescription" runat="server" Text=""></asp:Label>
                                    </p>
                                </div>
                                <div class="gamePrizes">
                                    <h3>活动时间：</h3>
                                    <p>
                                        <span id="sBeginTime">
                                            <asp:Label ID="lbBeginTime" runat="server" Text=""></asp:Label></span>至 <span id="sEedTime">
                                                <asp:Label ID="lbEedTime" runat="server" Text=""></asp:Label></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear-line">
                        <div class="mobile-footer"></div>
                    </div>
                </div>
            </div>
            <!--第一步-->
            <Hi:UCGameInfo runat="server" ID="UCGameInfo" />
            <!--第二步-->
            <div id="step2" style="display: none;" class="fl frwidth marTop">
                <Hi:UCGamePrizeInfo runat="server" ID="UCGamePrizeInfo" />
                <div class="footer-btn navbar-fixed-bottom btn-step2">
                    <button type="button" class="btn btn-primary" onclick="ShowStep1()">上一步</button>
                    <asp:Button ID="btnSubmit1" runat="server" OnClick="btnSubmit_Click" Text="提交" OnClientClick="return CheckPrizeInfo();"
                        CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <!--第三步-->
        <div id="btnSubmit" style="display: none;" class="footer-btn navbar-fixed-bottom">
            <a href="GameLists.aspx" class="btn btn-primary">完成</a>
        </div>
    </form>
</asp:Content>
