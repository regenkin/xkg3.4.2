<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<span><a href='<%# Globals.ApplicationPath + "/Vshop/GroupBuyList.aspx?categoryId=" + Eval("CategoryId") %> '><%# Eval("Name") %></a></span>