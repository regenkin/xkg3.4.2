<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestExpress.aspx.cs" Inherits="Hidistro.UI.Web.TestExpress" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     
</head>
<body>
    <form id="form1" runat="server">
    <div>
        快递公司：<input type="text" id="txtCom" runat="server"/>
        快递单号：<input type="text" id="txtNum" runat="server"/>
        <asp:Button runat="server"  id="btnSearch" Text="查询" OnClick="btnSearch_Click" /><br /><br /><br />
        <asp:TextBox runat="server" ID="txtContent" TextMode="MultiLine" Width="400" Height="200">
        </asp:TextBox>
        <a href="http://m.kuaidi100.com/result.jsp?nu=880970867290403637" target="_blank">查询</a>
        <input type="button" id="btnTest" value="test" />
        <table border="1">
            <tr>
                <td>1</td><td>2</td>
            </tr>
            <tr>
                <td style="width:100px"   rowspan="2" colspan="2">1</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
