<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    <div class="mate-inner top">
        <span class="share_title"><%# Eval("PubTime", "{0:yyyy-MM-dd HH:mm:ss}")%></span>
        <div class="mate-img-div">
            <a href='/vshop/FriendsCircleDetail.aspx?id=<%# Eval("ArticleId")  %>&UserId=<%=Globals.RequestQueryStr("UserId") %>'>
            <div class="mate-img">
                <img src='<%# Eval("ImageUrl")  %>' style="width:100%" class="img-responsive">
                <div class="title"><%# Eval("Title")  %></div>
            </div>
            </a>
            <asp:Literal ID="ItemHtml" runat="server"></asp:Literal>
            <asp:Repeater ID="ItemInfo" runat="server">
                <ItemTemplate>    
            <div class="mate-sub clear">
                 <a href='/vshop/ArticleDetail.aspx?iid=<%# Eval("Id")  %>&ReferralId=<%=Globals.RequestQueryStr("UserId") %>'>
                <img class="subimg" src='<%# Eval("ImageUrl")  %>' style="" />
                <div class="subtitle"><%# Eval("Title")  %></div>
                <div class="clear"></div>
              </a>
            </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
