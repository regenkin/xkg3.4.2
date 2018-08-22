<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="BrandDetail.aspx?BrandId=<%# Eval("BrandId")%>">
    <div class="well">
        <img src="<%# Eval("Logo")%>" class="img-responsive">
        <div class="name font-l">
            <%# Eval("BrandName")%></div>
    </div>
</a>