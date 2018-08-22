<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
 <a href="/ProgressDetail.aspx?ReturnsId=<%#Eval("ReturnsId")%>">
    <div class="progress-check well">
    	<div class="title">售后单号：<span><%#Eval("ReturnsId")%></span></div>
    	<hr style="margin:0 -10px 0 -10px">
    	<div class="info"><div class="text-overflow">商品名称：<span><%#Eval("ProductName")%></span></div>
<div class="text-overflow">订单编号：<span><%#Eval("OrderId")%></span></div>
<div class="text-overflow">申请时间：<span><%#Eval("ApplyForTime")%></span></div>
<div>售后状态：<span class="text-danger"><%#Eval("HandleStatus").ToString().Trim() == "4" ? "待审核" : Eval("HandleStatus").ToString().Trim() == "6" ? "待退款" : Eval("HandleStatus").ToString().Trim() == "5" ? "已审核" :  Eval("HandleStatus").ToString().Trim() == "7" ?"审核未通过":Eval("HandleStatus").ToString().Trim() == "8" ?"拒绝退款":"已退款"%></span></div>
<%#Eval("HandleStatus").ToString()=="8"?"<div >拒绝理由："+Eval("AdminRemark")+"</div>":""%>
<span class="glyphicon glyphicon-menu-right"></span>
	</div>
    </div>
	</a>