<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplaceCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.goods.DisplaceCategory" %>
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
    <form runat="server">
        <div class="form-horizontal">
            <div class="form-group">
                <label for="inputPassword3" class="col-xs-4 control-label">需要转移商品的分类：</label>
                <Hi:ProductCategoriesDropDownList ID="dropCategoryFrom" Width="239px" runat="server" AutoDataBind="false" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="inputPassword3" class="col-xs-4 control-label">转移至：</label>
                <Hi:ProductCategoriesDropDownList ID="dropCategoryTo" Width="239px" runat="server" AutoDataBind="true" CssClass="form-control" />
            </div>
            <div class="form-group">
                <div class="col-xs-offset-4 col-xs-10">
                    <asp:Button ID="btnSaveCategory" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
