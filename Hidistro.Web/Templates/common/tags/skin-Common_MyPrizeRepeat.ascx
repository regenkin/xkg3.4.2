<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="ControlPanel.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <div class="outbox">
        <div class="shopInfo clearfix" onclick="javascript:window.location='/ProductDetails.aspx?productId=<%# Eval("ProductId") %>'"
             PrizeGrade="<%#   GameHelper.GetPrizeGradeName(Eval("PrizeGrade").ToString()) %>" PrizeType="<%# Eval("PrizeType") %>"    GiveCouponId="<%# Eval("GiveCouponId") %>" GivePoint="<%# Eval("GivePoint") %>"  Prize="<%# Eval("Prize") %>"
             <asp:Literal ID="ItemHtml" runat="server"></asp:Literal>
             >
            <div class="img"  >
                <Hi:ListImage ID="ProductImage" runat="server" DataField="ThumbnailUrl100" Width="60" Height="60" />
            </div>
            <div class="imgInfo">
                <div>
                    <p>
                        <%# Eval("ProductName") %>
                    </p>
                    <p><%#   GameHelper.GetPrizeGradeName(Eval("PrizeGrade").ToString()) %>　数量：1</p>
                </div>
            </div>
        </div>
    </div>
    <div class="outbox">
        <div class="prizeInfo">
            <p>中奖编号：<%# Eval("PlayTime","{0:yyyyMMdd}")%>-<%# Eval("PrizeGrade") %>-<%# Eval("LogId")%></p>
            <p>活动名称：<a href="<%#Eval("GameUrl") %>"> <span style="color:#1F89D3"><%#Eval("GameTitle") %></span></a><span>[<%# GameHelper.GetGameTypeName(Eval("GameType").ToString()) %>]</span></p>
            <p>中奖时间：<%# Eval("PlayTime","{0:yyyy-MM-dd HH:mm:ss}")%></p>
            <p>发放状态：<%# GameHelper.GetPrizesDeliveStatus(Eval("status").ToString(),Eval("IsLogistics").ToString(),Eval("PrizeType").ToString(),Eval("gametype").ToString())%></p>
            <div class="btnright" LogId="<%# Eval("LogId") %>" Dstatus="<%# Eval("status") %>" Pid="<%# Eval("Id") %>" IsLogistics="<%# Eval("IsLogistics") %>" PrizeType="<%# Eval("PrizeType") %>">   
        </div>
    </div>

</li>
