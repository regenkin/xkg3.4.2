<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.SaleSystem.Vshop" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("RequestTime","{0:d}")%>
    </td>
    <td>
        <%# Eval("Amount","{0:F2}") %>
    </td>
    <td class="DrawStatus" IsCheck="<%# Eval("IsCheck") %>" SerialID="<%# Eval("SerialID") %>">
        <%# DistributorsBrower.GetBalanceDrawRequestStatus((int)Eval("IsCheck"))%>
    </td>
</tr>
