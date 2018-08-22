<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hiRegionSelect.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.hiRegionSelect" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<html>
    <head>
        <title>区域选择</title>
        <link rel="stylesheet" type="text/css" href="/admin/css/bootstrap-datetimepicker.min.css" />
         <link rel="stylesheet" href="/admin/css/common.css" />
        <style>
            body{overflow:hidden}
             #ddlRegions1,#ddlRegions2{margin-right:3px}
        </style>
        <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
        <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
        <script>
            $(function () {

                $("select[selectset=regions]").addClass("form-control  resetSize addwauto"); //区域选择
            });
        </script>
    </head>
    <body style="text-align:left;margin:0px">
        <form runat="server">
           <Hi:RegionSelector runat="server" ID="SelReggion" />
        </form>
    </body>
</html>

