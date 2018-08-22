<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <a href='<%# Globals.ApplicationPath + "ProductList.aspx?categoryId=" + Eval("CategoryId") %> '>
        
        <%# Eval("Name") %>
    </a>
</li>