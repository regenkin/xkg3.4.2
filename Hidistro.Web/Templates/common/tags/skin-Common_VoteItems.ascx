<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li item='<%# Eval("VoteItemId")%>'>
    <i>&nbsp;</i>
    <div>
        <p><%# Eval("VoteItemName")%></p>
        <samp><em>&nbsp;<%# Eval("Lenth")%></em><span><%# Eval("Percentage")%>%(<%# Eval("ItemCount")%>)</span><samp>
    </div>
</li>