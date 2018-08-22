<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
 <div class="progress-detail well">
		<div class="progress-detail-title">
			<p>售后状态：<span class="text-danger"><%#Eval("HandleStatus").ToString().Trim() == "4" ? "待审核" : Eval("HandleStatus").ToString().Trim() == "6" ? "待退款" : Eval("HandleStatus").ToString().Trim() == "5" ? "已审核" :  Eval("HandleStatus").ToString().Trim() == "7" ?"审核未通过":Eval("HandleStatus").ToString().Trim() == "8" ?"拒绝退款":"已退款"%></span></p>
			<p>订单编号：<span><%#Eval("OrderId")%></span></p>
			<p>售后类型：<span><%#Eval("RefundType").ToString()=="1"?"退货":"退款"%></span></p>
		</div>
		<div class="progress-detail-content">
			<div class="clearfix"><span>商品名称：</span><p><%#Eval("ProductName")%></p></div>
            <div class="clearfix"><span>退款金额：</span><p>￥<%#Eval("RefundMoney","{0:F2}")%></p></div>
			<div class="clearfix"><span>申请时间：</span><p><%#Eval("ApplyForTime")%></p></div>
			<div class="clearfix"><span>申请原因：</span><p><%#Eval("Comments")%></p></div>
		</div>
	</div>
    <div class="progress-follow well">
		<div class="progress-detail-title">进度跟踪</div>
		<div class="follow-content">
			<p <%#Eval("ApplyForTime").ToString()!=""?"":"style=\"display:none\"" %>>
            <span>等待平台审核</span><%#Eval("ApplyForTime")%></p>
			<p style="display:none" <%#Eval("AuditTime").ToString()!=""?"":"" %>><span>通过平台审核</span><%#Eval("AuditTime")%></p>
			<p style="display:none" <%#Eval("AuditTime").ToString()!=""?"":"" %>><span>等待平台退款</span><%#Eval("AuditTime")%></p>
			<p <%#Eval("RefundTime").ToString()!=""?"":"style=\"display:none\"" %> class="text-danger"><span>退款成功</span><%#Eval("RefundTime")%></p>
            <p <%#Eval("HandleStatus").ToString()=="7"?"":"style=\"display:none\"" %> class="text-danger"><span>未通过平台审核</span><%#Eval("HandleTime")%></p>
            <p <%#Eval("HandleStatus").ToString()=="8"?"":"style=\"display:none\"" %> class="text-danger"><span>平台拒绝退款</span><%#Eval("HandleTime")%></p>
            <p <%#Eval("HandleStatus").ToString()=="8"?"":"style=\"display:none\""%> class="text-danger">拒绝退款原因：<%#Eval("AdminRemark") %></p>
		</div>
	</div>