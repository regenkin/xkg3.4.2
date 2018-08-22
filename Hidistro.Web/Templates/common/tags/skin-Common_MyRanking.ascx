<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="my-store-top"><input type="hidden" id="hdMyUserID" value="<%#Eval("UserID") %>" />
    <div class="my-num"><%#Int32.Parse(Eval("ccount").ToString())>100?"100+":Eval("ccount").ToString() %></div>
    <div class="my-img"><img src="<%#Eval("Logo").ToString().Length>5?Eval("Logo").ToString():"/Utility/pics/headLogo.jpg" %>"></div>
    <div class="my-exp">
        <div class="exp-my">
            <span><%#Eval("StoreName") %></span>
            <em><%#Eval("Blance","{0:F2}") %>元</em>
        </div>
        <div class="exp-bottom"><em title="<%#Eval("Blance") %>"></em></div>
    </div>
</div>