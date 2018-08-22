<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("OrderId") %>
    </td>
    <td>
       <%# Eval("StoreName")%>
    </td>
    <td>
        <%# Eval("CommTotal","{0:F2}")%>元
    </td>
</tr>
