<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li onclick="location.href='/ProductDetails.aspx?ProductId=<%#Eval("ProductId") %>'">
					<div  class="y-shopimg">
						<Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl220"/>
					</div>
					<div class="y-shopinfo">
						<div class="name"><%# Eval("ProductName") %></div>
						<div class="price">
	                        ¥<%# Eval("SalePrice", "{0:F2}") %><%#(Eval("SalePrice").ToString()==Eval("MaxShowPrice").ToString())?"":"起" %> <del class="text-muted font-xs">¥<%# Eval("MarketPrice", "{0:F2}") %> </del>
	                    </div>
	                    <div class="sales">已售<%# Eval("SaleCounts")%>件</div>
					</div>
				</li>

