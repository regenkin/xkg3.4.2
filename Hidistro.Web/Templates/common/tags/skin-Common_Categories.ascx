<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
<a name="<%# Eval("HasChildren") %>" value="<%# Eval("CategoryId") %>"  onclick="goUrl('ProductList.aspx?categoryId=<%# Eval("CategoryId") %>');event.cancelBubble = true;">
   <div class="select-tags-img">  
        
        <asp:Literal ID="litpromotion" runat="server" Text='<%# Eval("IconUrl") %>'></asp:Literal> 

   </div>
            <div class="select-tags-name"> <%# Eval("Name") %></div> 
  </a>
</li>
