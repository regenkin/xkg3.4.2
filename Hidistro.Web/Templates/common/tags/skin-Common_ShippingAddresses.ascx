<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well address-box">
    <div class="font-xl">
        <%#Eval("ShipTo")%></span>&nbsp;<%#Eval("CellPhone")%>&nbsp;
            <div>
                <a onclick='UpdateShipping(<%#Eval("ShippingId") %>)' href="javascript:void(0)"><span
                    class="glyphicon glyphicon-pencil"></span></a><a href="javascript:void(0)" onclick='DeleteShippingAddress(<%#Eval("ShippingId") %>,this)'>
                        <span class="glyphicon glyphicon-trash"></span></a>
            </div>
    </div>
    <div class="font-m">
        <Hi:RegionAllName ID="regionname" runat="server" RegionId='<%#Eval("RegionId") %>'></Hi:RegionAllName></div>
    <div class="font-m">
        <%#Eval("Address")%></div>
</div>
