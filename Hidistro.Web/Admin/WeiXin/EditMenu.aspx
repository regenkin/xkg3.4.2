<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.EditMenu" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="renderer" content="webkit">
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css">
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/admin/css/common.css" />
    <script src="/admin/js/Framenew.js"></script>
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body style="overflow: hidden;">
    <form runat="server"><asp:HiddenField ID="hdfIframeHeight" runat="server" Value="" />
        <div class="form-horizontal">
            <div class="form-group">
                <label for="inputPassword3" class="col-xs-4 control-label">菜单名称：</label>
                <asp:TextBox ID="txtMenuName" runat="server" CssClass="form-control inputw200" MaxLength="14"/>
            </div>
            <div class="form-group" runat="server" id="liParent">
                <label for="inputPassword3" class="col-xs-4 control-label">上级菜单：</label>
                <div class="form-control resetText"><asp:Literal runat="server" ID="lblParent"></asp:Literal></div>
            </div>
            <div class="form-group" runat="server" id="liBind">
                <label for="inputPassword3" class="col-xs-4 control-label">绑定对象：</label>
               <asp:DropDownList ID="ddlType" runat="server" CssClass="productType form-control" 
                      AutoPostBack="True" ClientIDMode="Static" 
                      onselectedindexchanged="ddlType_SelectedIndexChanged" Width="200">
                    <asp:ListItem Text="不绑定" Value="0"></asp:ListItem>
                    <asp:ListItem Text="关键字" Value="1"></asp:ListItem> 
                    <%--<asp:ListItem Text="专题" Value="2"></asp:ListItem>--%>
                    <asp:ListItem Text="首页" Value="3"></asp:ListItem>
                    <asp:ListItem Text="产品分类" Value="4"></asp:ListItem>
                    <asp:ListItem Text="购物车" Value="5"></asp:ListItem>
                    <asp:ListItem Text="会员中心" Value="6"></asp:ListItem>
                    <asp:ListItem Text="链接" Value="8"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group" id="liValue" runat="server"><label class="col-xs-4 control-label"></label>
                <asp:DropDownList ID="ddlValue" runat="server" CssClass="productType form-control inputw200" />
            </div>
            <div class="form-group" id="liUrl" runat="server">
                <label for="inputPassword3" class="col-xs-4 control-label">链接地址：</label>
                <asp:TextBox ID="txtUrl" runat="server" Text="http://" CssClass="form-control inputw200" MaxLength="256" />
            </div>
            <div class="form-group">
                <div class="col-xs-offset-4 col-xs-10">
                    <asp:Button ID="btnAddMenu" runat="server" Text="确 定" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        if (parent) {/*控制iframe的高度*/
            if (parent.CallBack_MobileFramMain) {
                var h = document.getElementById("<%=hdfIframeHeight.ClientID%>").value;
                parent.CallBack_MobileFramMain(h)
            }
        }
        InitTextCounter(<%=iNameByteWidth%>, "#txtMenuName", null);
    </script>
</body>
</html>
