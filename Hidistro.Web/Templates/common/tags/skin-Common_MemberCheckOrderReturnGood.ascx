<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%--<a href="<%# Globals.ApplicationPath + "ProductDetails.aspx?productId=" + Eval("ProductId")%>">--%>
 
    <div class="box">
        <div class="left">
            <Hi:ListImage runat="server" DataField="ThumbnailsUrl" />
        </div>
        <div class="right">
            <div class="name bcolor">
                <%# Eval("ItemDescription")%></div>
           
            <div class="price text-danger">
                ¥<%# Eval("ItemAdjustedPrice", "{0:F2}")%><span class="bcolor">  数量
                    <%# Eval("Quantity")%></span></div>
                   
        </div>
    </div>
 
<script type="text/javascript">
    $(function () {
       

    });
        
        
</script>
