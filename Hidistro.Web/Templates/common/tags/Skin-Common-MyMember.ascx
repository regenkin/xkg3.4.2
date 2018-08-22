<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
             <li>
					<h3><%# Eval("UserName")%>【<%# Eval("GradeName")%>】</h3>
					<div class="userinfobox">
						<div class="userimg">
							<img src='<%# Eval("UserHead")%>'>
						</div>
						<div class="usertextinfo clearfix">
							<div class="left">
								<p><span class="colorc">注册时间：</span><%# Eval("CreateDate","{0:yyyy-MM-dd}")%></p>
								<p><span class="colorc">订单数量：</span><span class="colorg"><%# Eval("OrderMumber")%></span> 单</p>
							</div>
							<div class="right1">
								<p><span class="colorc">最近下单：</span><%# Eval("OrderDate","{0:yyyy-MM-dd}")%></p>
								<p><span class="colorc">消费金额：</span><span class="colorg">￥<%# string.IsNullOrEmpty(Eval("OrdersTotal", "{0:F2}"))?"0":Eval("OrdersTotal", "{0:F2}") %></span></p>
							</div>
						</div>
					</div>
					<span class="left radius"></span>
					<span class="right radius"></span>
				</li>
