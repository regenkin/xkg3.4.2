<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
            <li>
                <div class="shopimg">
                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl310" Width="80" BorderWidth="1px" Height="80" BorderStyle="Solid" CssClass="_pimg" />
                </div>
                <div class="shopinfo">
                    <p class="intro">

                        <a class="Plink" href='<%#"/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>'>
                <%# Eval("ProductName") %></a>

                    </p>

                    <p class="Price">￥<%# Eval("SalePrice", "{0:F2}") %></p>
                    <p class="saleshare clearfix">
                        <span class="sale">已售<%# Eval("SaleCounts")%>件</span>
                        <a class="share  btn-share" href="javascript:void(0)">分享</a>
                    </p>
                </div>
                <div class="checkbox" id='CheckGroup<%#Eval("ProductId") %>' key='<%# Eval("ProductId") %>'>
                    <span></span>
                </div>
            </li>