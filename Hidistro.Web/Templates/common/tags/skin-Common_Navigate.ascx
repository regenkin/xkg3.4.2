<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<a href="<%#Eval("LoctionUrl")%>">
    <div>
        <div class="icon"  name="icon">
            <img src='<%#Eval("ImageUrl")%>'  width="50px" height="50px;"/>
        </div>
        <div class="name">
            <%#Eval("ShortDesc")%></div>
    </div>
</a>