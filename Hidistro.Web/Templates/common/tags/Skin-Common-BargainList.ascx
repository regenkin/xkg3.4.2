<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
             <li>
                <div class="shopimg">
                    <img src='<%# (string.IsNullOrEmpty(Eval("ActivityCover").ToString())?"/utility/pics/none.gif":Eval("ActivityCover").ToString()) %>'>
                    <p class="mask">
                        <span class="fl"><%# Eval("ProductName")%></span>
                        <span class="fr">原价：￥<%# Eval("SalePrice").ToString ()==""?"0.00":Eval("SalePrice", "{0:F2}") %></span>
                    </p>
                </div>
                <div class="bargain-info">
                    <p>砍至底价：<strong class="colorr">￥<%# Eval("FloorPrice").ToString ()==""?"0.00":Eval("FloorPrice", "{0:F2}") %></strong></p>
                    <p>结束时间：<%# Eval("EndDate","{0:yyyy-MM-dd HH:mm:ss}")%></p>                    
                    <%# Hidistro.ControlPanel.Bargain.BargainHelper.GetLinkHtml(Eval("id").ToString(),Eval("bargainstatus").ToString(),Eval("ActivityStock").ToString(),Eval("TranNumber").ToString()) %>
                </div>
            </li> 
