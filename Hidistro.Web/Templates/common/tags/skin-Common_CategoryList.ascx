<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li>
    <p style="cursor:pointer;">
        <%# Eval("Name") %>
        <asp:Literal ID="litPlus" runat="server"><i class="glyphicon glyphicon-plus"></i></asp:Literal>
        <input id="hdid" type="hidden" value="<%# Eval("CategoryId") %>" />
    </p>
    
    <div class="downDis">
        <ul>
            <asp:Repeater ID="rptSubCategories" runat="server" DataSource='<%# Eval("SubCategories") %>'>
                <ItemTemplate>
                    <li>
                        <a href="/productlist.aspx?categoryId=<%# Eval("CategoryId") %>"><%# Eval("Name") %></a>
                        <i class="glyphicon glyphicon-chevron-right"></i>
                    </li>
                </ItemTemplate>
            </asp:Repeater>           
        </ul>
    </div>
</li>