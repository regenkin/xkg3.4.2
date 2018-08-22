<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SendAll.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.SendAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>function clicks() {
    var ShipperName = ['姓名', '姓名', '姓名', '姓名', '姓名'];
    var SizeShipperName = ['282,157,133,58', '282,157,133,58', '282,157,133,58', '282,157,133,58', '282,157,133,58'];
    var CellPhone = ['13800138000', '13800138000', '13800138000', '13800138000', '13800138000'];
    var SizeCellPhone = ['281,295,154,58', '281,295,154,58', '281,295,154,58', '281,295,154,58', '281,295,154,58'];
    var Address = ['详细地址测试', '详细地址测试', '详细地址测试', '详细地址测试', '详细地址测试'];
    var SizeAddress = ['357,116,358,69', '357,116,358,69', '357,116,358,69', '357,116,358,69', '357,116,358,69'];
    var Zipcode = ['555555', '555555', '555555', '555555', '555555'];
    var SizeZipcode = ['392,323,127,58', '392,323,127,58', '392,323,127,58', '392,323,127,58', '392,323,127,58'];
    var Province = ['浙江省', '浙江省', '浙江省', '浙江省', '浙江省'];
    var SizeProvnce = ['330,179,134,60', '330,179,134,60', '330,179,134,60', '330,179,134,60', '330,179,134,60'];
    var City = ['宁波市', '宁波市', '宁波市', '宁波市', '宁波市'];
    var SizeCity = ['330,278,131,60', '330,278,131,60', '330,278,131,60', '330,278,131,60', '330,278,131,60'];
    var District = ['江东区', '江东区', '江东区', '江东区', '江东区'];
    var SizeDistrict = ['330,374,122,60', '330,374,122,60', '330,374,122,60', '330,374,122,60', '330,374,122,60'];
    var OrderId = ['订单号：201509091156365', '订单号：201509098685213', '订单号：201509092394383', '订单号：201509091543245', '订单号：201509010022093'];
    var SizeOrderId = ['168,457,191,69', '168,457,191,69', '168,457,191,69', '168,457,191,69', '168,457,191,69'];
    var ShipitemInfos = ['规格 数量1货号 :', '规格 数量1货号 :', '规格 数量1货号 :', '规格 数量1货号 :', '规格规格：22 适合年龄：33  数量1货号 :23377-6规格尺码：37 颜色：白色  数量1货号 :7-1'];
    var SizeitemInfos = ['214,458,228,81', '214,458,228,81', '214,458,228,81', '214,458,228,81', '214,458,228,81'];
    var SiteName = ['vshop', 'vshop', 'vshop', 'vshop', 'vshop'];
    var SizeSiteName = ['305,184,296,58', '305,184,296,58', '305,184,296,58', '305,184,296,58', '305,184,296,58'];
    var ShipTelPhone = ['', '', '', '', ''];
    var SizeShipTelPhone = ['0,0,0,0', '0,0,0,0', '0,0,0,0', '0,0,0,0', '0,0,0,0'];
    var ShipCellPhone = ['18684696503', '18684696503', '18684696503', '18684696503', '18684696503'];
    var SizeShipCellPhone = ['160,306,137,60', '160,306,137,60', '160,306,137,60', '160,306,137,60', '160,306,137,60'];
    var ShipZipCode = ['12345', '12345', '12345', '12345', '12345'];
    var SizeShipZipCode = ['71,108,155,60', '71,108,155,60', '71,108,155,60', '71,108,155,60', '71,108,155,60'];
    var ShipAddress = ['ceshi地址', 'ceshi地址', 'ceshi地址', 'ceshi地址', 'ceshi地址'];
    var ShipSizeAddress = ['247,119,361,67', '247,119,361,67', '247,119,361,67', '247,119,361,67', '247,119,361,67'];
    var ShipProvince = ['上海', '上海', '上海', '上海', '上海'];
    var ShipSizeProvnce = ['215,181,129,63', '215,181,129,63', '215,181,129,63', '215,181,129,63', '215,181,129,63'];
    var ShipCity = ['上海市', '上海市', '上海市', '上海市', '上海市'];
    var ShipSizeCity = ['215,275,131,63', '215,275,131,63', '215,275,131,63', '215,275,131,63', '215,275,131,63'];
    var ShipDistrict = ['卢湾区', '卢湾区', '卢湾区', '卢湾区', '卢湾区'];
    var ShipSizeDistrict = ['215,369,126,63', '215,369,126,63', '215,369,126,63', '215,369,126,63', '215,369,126,63'];
    var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    try {
        for (var i = 0; i < 5; ++i) {
            showdiv();
            LODOP.SET_PRINT_PAGESIZE(1, 2280, 1270, "");
            LODOP.SET_PRINT_STYLE("FontSize", 12);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            LODOP.ADD_PRINT_TEXT(SizeShipperName[i].split(',')[0], SizeShipperName[i].split(',')[1], SizeShipperName[i].split(',')[2], SizeShipperName[i].split(',')[3], ShipperName[0]);
            LODOP.ADD_PRINT_TEXT(SizeCellPhone[i].split(',')[0], SizeCellPhone[i].split(',')[1], SizeCellPhone[i].split(',')[2], SizeCellPhone[i].split(',')[3], CellPhone[0]);
            LODOP.ADD_PRINT_TEXT(SizeAddress[i].split(',')[0], SizeAddress[i].split(',')[1], SizeAddress[i].split(',')[2], SizeAddress[i].split(',')[3], Address[0]);
            LODOP.ADD_PRINT_TEXT(SizeZipcode[i].split(',')[0], Zipcode[0]);
            LODOP.ADD_PRINT_TEXT(SizeProvnce[i].split(',')[0], SizeProvnce[i].split(',')[1], SizeProvnce[i].split(',')[2], SizeProvnce[i].split(',')[3], Province[0]);
            LODOP.ADD_PRINT_TEXT(SizeCity[i].split(',')[0], SizeCity[i].split(',')[1], SizeCity[i].split(',')[2], SizeCity[i].split(',')[3], City[0]);
            LODOP.ADD_PRINT_TEXT(SizeDistrict[i].split(',')[0], SizeDistrict[i].split(',')[1], SizeDistrict[i].split(',')[2], SizeDistrict[i].split(',')[3], District[0]);
            LODOP.ADD_PRINT_TEXT(SizeOrderId[i].split(',')[0], SizeOrderId[i].split(',')[1], SizeOrderId[i].split(',')[2], SizeOrderId[i].split(',')[3], OrderId[i]);
            LODOP.ADD_PRINT_TEXT(SizeitemInfos[i].split(',')[0], SizeitemInfos[i].split(',')[1], SizeitemInfos[i].split(',')[2], SizeitemInfos[i].split(',')[3], ShipitemInfos[i]);
            LODOP.ADD_PRINT_TEXT(SizeSiteName[i].split(',')[0], SizeSiteName[i].split(',')[1], SizeSiteName[i].split(',')[2], SizeSiteName[i].split(',')[3], SiteName[i]);
            LODOP.ADD_PRINT_TEXT(SizeShipTelPhone[i].split(',')[0], SizeShipTelPhone[i].split(',')[1], SizeShipTelPhone[i].split(',')[2], SizeShipTelPhone[i].split(',')[3], ShipTelPhone[i]);
            LODOP.ADD_PRINT_TEXT(SizeShipCellPhone[i].split(',')[0], SizeShipCellPhone[i].split(',')[1], SizeShipCellPhone[i].split(',')[2], SizeShipCellPhone[i].split(',')[3], ShipCellPhone[i]);
            LODOP.ADD_PRINT_TEXT(SizeShipZipCode[i].split(',')[0], SizeShipZipCode[i].split(',')[1], SizeShipZipCode[i].split(',')[2], SizeShipZipCode[i].split(',')[3], ShipZipCode[i]);
            LODOP.ADD_PRINT_TEXT(ShipSizeAddress[i].split(',')[0], ShipSizeAddress[i].split(',')[1], ShipSizeAddress[i].split(',')[2], ShipSizeAddress[i].split(',')[3], ShipAddress[i]);
            LODOP.ADD_PRINT_TEXT(ShipSizeProvnce[i].split(',')[0], ShipSizeProvnce[i].split(',')[1], ShipSizeProvnce[i].split(',')[2], ShipSizeProvnce[i].split(',')[3], ShipProvince[i]);
            LODOP.ADD_PRINT_TEXT(ShipSizeCity[i].split(',')[0], ShipSizeCity[i].split(',')[1], ShipSizeCity[i].split(',')[2], ShipSizeCity[i].split(',')[3], ShipCity[i]);
            LODOP.ADD_PRINT_TEXT(ShipSizeDistrict[i].split(',')[0], ShipSizeDistrict[i].split(',')[1], ShipSizeDistrict[i].split(',')[2], ShipSizeDistrict[i].split(',')[3], ShipDistrict[i]);
            LODOP.PRINT();
        }
        setTimeout("hidediv()", 3000);
    } catch (e) {
        alert("请先安装打印控件！");
        hidediv();
        return false;
    }
}
        setTimeout("clicks()", 1000);
    </script>
</asp:Content>
