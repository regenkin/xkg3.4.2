<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    <li role="presentation"><a role="menuitem" tabindex="-1" value='<%# Eval("CategoryId") %>' onclick="javascript:location.href='?categoryId=<%#Eval("CategoryId") %>'"><%# Eval("Name") %></a></li>