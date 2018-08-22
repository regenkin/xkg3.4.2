<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
           <li val="<%# Eval("id")%>" detial="<%# Eval("bargainDetialID")%>">
					<div class="luckdrawinfo">
						<div class="img">
							<img src="<%# (string.IsNullOrEmpty(Eval("ActivityCover").ToString())?"/utility/pics/none.gif":Eval("ActivityCover").ToString()) %>" width="80" height="80">
						</div>
						<div class="drawinfo">
							<h3><%# Eval("ProductName")%></h3>
							<p class="no color"> <label id="lbDay" ><%# Hidistro.ControlPanel.Bargain.BargainHelper.GetDay(Eval("hou"))  %></label> </p>
							<div class="progress">
							  	<div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style='width:<%# Eval("process") %>%'>
							    	<span class="sr-only">60% Complete (warning)</span>
							  	</div>
							</div>
							<div class="clearfix color">
								<p class="fl" style="color:red">当前价格￥<%# Eval("Price").ToString ()==""?"0.00":Eval("Price", "{0:F2}") %></p>
								<p class="fr">活动底价<span style="color:#009DDA">￥<%# Eval("FloorPrice").ToString ()==""?"0.00":Eval("FloorPrice", "{0:F2}") %></span></p>
							</div>
						</div>
					</div>
				</li>
