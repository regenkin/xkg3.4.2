<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPrizeLists.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.promotion.UCPrizeLists" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<script type="text/javascript">
    $(function () {
        setActive();

    });
    function setActive() {
        var isFinished = $("#isFinished").val();
        if (isFinished == "1") {
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
    #ctl00_ContentPlaceHolder1_UCPrizeLists1_grdPrizeLists th {
        margin: 0px;
        border-left: 0px;
        border-right: 0px;
        background-color: #F7F7F7;
        text-align: center;
        vertical-align: middle;
    }

    #ctl00_ContentPlaceHolder1_UCPrizeLists1_grdPrizeLists td {
        margin: 0px;
        border-left: 0px;
        border-right: 0px;
        vertical-align: middle;
    }

</style>
<input type="hidden" id="isFinished" value="<%=isFinished%>" />
<div class="page-header">
    <h2>
        <asp:Label ID="lbGameName" runat="server" Text=""></asp:Label>

        中奖结果

    </h2>
</div>
<div id="allProductsDiv">
    <div class="play-tabs">
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist">
                <li id="tabHeader_active" role="presentation" class="active">
                    <a href="?isFinished=1&gameId=<%=gameId %>">已领取</a>
                </li>
                <li id="tabHeader_over" role="presentation">
                    <a href="?isFinished=0&gameId=<%=gameId %>">未领取</a>
                </li>
            </ul>
            <div class="page-box" style="margin-right: 15px;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane active">
            <div style="margin-top: 5px;">
                <UI:Grid ID="grdPrizeLists" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                    DataKeyNames="LogId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                    GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="中奖用户" SortExpression="UserName">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# Eval("UserName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="奖品" ShowHeader="true">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%-- <%# ControlPanel.Promotions.GameHelper.GetPrizeName((Hidistro.Entities.Promotions.PrizeType)int.Parse(Eval("PrizeType").ToString()),ControlPanel.Promotions.GameHelper.GetPrizeFullName(new Hidistro.Entities.Promotions.PrizeResultViewInfo(){ PrizeType=(Hidistro.Entities.Promotions.PrizeType)int.Parse(Eval("PrizeType").ToString()), GivePoint=int.Parse(Eval("GivePoint").ToString()), GiveCouponId=(Eval("GiveCouponId")==null?"":Eval("GiveCouponId").ToString()), GiveShopBookId=(Eval("GiveShopBookId")==null?"":Eval("GiveShopBookId").ToString()) })) %>--%>
                             <%# Eval("Prize") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="等级" ShowHeader="true">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((Hidistro.Entities.Promotions.PrizeGrade)int.Parse(Eval("PrizeGrade").ToString())).ToString() %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="中奖时间" ShowHeader="true">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="200" />
                            <ItemTemplate>
                                <%# Eval("PlayTime","{0:yyy-MM-dd HH:mm:ss}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" style="width: auto; float: right;">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
