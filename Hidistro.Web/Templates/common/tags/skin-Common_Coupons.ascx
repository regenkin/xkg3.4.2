<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="alert alert-success alert-dismissable">
    <div class="name font-xl">
        <%# Eval("Name") %>（满<%# Eval("Amount", "{0:F2}")%>减<%# Eval("DiscountValue", "{0:F2}")%>）</div>
    <div class="date">
        有效期：<%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %>至<%# Convert.ToDateTime(Eval("ClosingTime")).ToString("yyyy-MM-dd") %></div>
</div>
