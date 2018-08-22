<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li class="well myConsultation">
    <h3 class="y-position">
		<p class="y-title">商品名称：</p>
		<p class="y-textin">
			<a class="link" href="<%#"/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                <%# Eval("ProductName")%></a>
		</p>
	</h3>
    <div class="y-position">
		<p class="y-title">咨询内容：</p>
		<p class="y-textin">
			<%# Eval("ConsultationText")%>
			<time>【<%#Eval("ConsultationDate","{0:yyyy/MM/dd HH:mm:ss}") %>】</time>
		</p>
	</div>
    <div class="y-position y-businessreply">
		<p class="y-title">商家回复：</p>
		<p class="y-textin">
			<%# Eval("ReplyText")%>
			<time class="detailtime"><%# Eval("ReplyDate","{0:【yyyy/MM/dd HH:mm:ss】}") %></time>
		</p>
	</div>
</li>
