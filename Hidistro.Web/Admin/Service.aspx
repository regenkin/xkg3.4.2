<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="Service.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Service" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

 <%-- <Hi:HiControls ID="HiControls1" LinkURL="http://www.shopefx.com/zengzhi.html" Height="1500" runat="server"/>--%>
 <div  class="areacolumn clearfix" style="width:980px;">
 <iframe src="http://www.shopefx.com/zengzhi.html" style="border:0px;background-color:Transparent; height:1500px;width:980px;" scrolling="no" allowTransparency="true" frameborder="0"></iframe></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <script type="text/javascript" language="javascript">
          function gotoWeb(src)
          {
            window.open(src,"_blank")
          }
        </script>
</asp:Content>
