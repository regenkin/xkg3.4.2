<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well red-bg">
	<ul class="red-content">
		<li style="width: 25%; border-right: 1px dashed #d7d7d7;">
			<div class="money">￥<span><%# Eval("Amount", "{0:F0}")%></span></div>
			<div class="money-info">满 <%# Eval("OrderAmountCanUse", "{0:F2}")%>可用</div>
		</li>
		<li style="width: 75%">
			<div class="shop-info">
				<h1><%#Eval("RedPagerActivityName") %></h1>
				<span>有效期至 <%#Eval("ExpiryTime","{0:yyyy-MM-dd HH:mm:ss}") %></span>
				<em>该券可用于任意商品的抵扣</em>
			</div>
		</li>
	</ul>
    <!-- <table style="width:100%"><tr>
        <td style="width:35%;"><div><span style="color:red">¥</span><span style="font-size:28px;color:red"><%# Eval("Amount", "{0:F0}")%></span></div><div>满 ¥<%# Eval("OrderAmountCanUse", "{0:F2}")%>可用</div></td>
        <td style="width:64%;">
            <div style="font-weight:bold;"><%# Eval("RedPagerActivityName")%></div>
            <div><%#Eval("ExpiryTime","{0:yyyy-MM-dd HH:mm:ss}") %>前有效</div>
        </td>
   </tr></table> -->
</div>
