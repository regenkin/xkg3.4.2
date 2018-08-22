<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="yiyuanPanel">
            <div class="prize-picture">
                <img src="<%# Eval("HeadImgage") %>" style="width:100%">
            </div>
            <div class="mobile-prize-textinfo">
                <h3><%# Eval("Title") %></h3>
                <p class="PrizeName">奖品名称：<span><%# Eval("ProductTitle") %></span></p>
                <p>奖品说明：<span>数量<%# Eval("PrizeNumber") %>，每份价格<di>￥<%# Eval("ProductPrice","{0:F2}") %></di>，限购<%# Eval("EachCanBuyNum") %>份</span></p>
                <p>开奖方式：<span class="Opentype" ReachType='<%# Eval("ReachType") %>' ReachNum='<%# Eval("ReachNum") %>'>开奖方式</span></p>
                <p>距离结束：<span class="mobile-prize-red" EndTime="<%# Eval("EndTime") %>">计算中...</span></p>
            </div>
            <div class="y3-mobilebtn">
                <button class="btn btn-primary btn-xs">分享</button>
                <a class="btn btn-danger btn-xs" href="ViewOneTao.aspx?vaid=<%# Eval("ActivityId")%>">去看看</a>
            </div>
        </div>