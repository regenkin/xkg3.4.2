<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<div class="rollCollar" CouponId="<%# Eval("CouponId") %>" IsAllProduct="<%# Eval("CouponId") %>" >
                <a style="height: 140px;" href="javascript:void(0)">
                    <div class="left">
                        <span><%# Eval("CouponName") %></span>
                        <span>￥<i><%# Eval("CouponValue","{0:F0}") %></i></span>
                    </div>
                    <div class="pright">
                        <h5><%# Eval("useConditon") %></h5>
                        <p>生效时间：<%# Eval("BeginDate","{0:yyyy-MM-dd HH:mm:ss}") %></p>
                        <p>到期时间：<%# Eval("EndDate","{0:yyyy-MM-dd HH:mm:ss}") %></p>
                    </div>
                </a>
</div>
