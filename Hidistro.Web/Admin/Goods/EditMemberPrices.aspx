<%@ Page Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="EditMemberPrices.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditMemberPrices" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function loadSkuPrice() {
            if (!checkPrice())
                return false;

            var skuPriceXml = "<xml><skuPrices>";
            $.each($(".SkuPriceRow"), function () {
                var skuId = $(this).attr("skuId");
                var costPrice = $("#tdCostPrice_" + skuId).val();
                var salePrice = $("#tdSalePrice_" + skuId).val();
                var itemXml = String.format("<item skuId=\"{0}\" costPrice=\"{1}\" salePrice=\"{2}\">", skuId, costPrice, salePrice);
                itemXml += "<skuMemberPrices>";

                $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function (rowIndex, rowItem) {
                    var id = $(this).attr("id");
                    var gradeId = id.substring(0, id.indexOf("_"));
                    var memberPrice = $(this).val();
                    if (memberPrice != "")
                        itemXml += String.format("<priceItme gradeId=\"{0}\" memberPrice=\"{1}\" \/>", gradeId, memberPrice);
                });

                itemXml += "<\/skuMemberPrices>";
                itemXml += "<\/item>";
                skuPriceXml += itemXml;
            });
            skuPriceXml += "<\/skuPrices><\/xml>";
            $("#ctl00_ContentPlaceHolder1_txtPrices").val(skuPriceXml);
            return true;
        }

        function checkPrice() {
            var validated = true;
            var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

            $.each($(".SkuPriceRow"), function () {
                var skuId = $(this).attr("skuId");
                var costPrice = $("#tdCostPrice_" + skuId).val();
                var salePrice = $("#tdSalePrice_" + skuId).val();

                // 检查必填项是否填了
                if (salePrice.length == 0) {
                    alert("商品规格的一口价为必填项！");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                if (!exp.test(salePrice)) {
                    alert("商品规格的一口价输入有误");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                var num = parseFloat(salePrice);
                if (num > 10000000 || num <= 0) {
                    alert("商品规格的一口价超出了系统表示范围！");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                if (costPrice.length > 0) {
                    // 检查输入的是否是有效的金额
                    if (!exp.test(costPrice)) {
                        alert("商品规格的成本价输入有误！");
                        $("#tdCostPrice_" + skuId).focus();
                        validated = false;
                        return false;
                    }

                    // 检查金额是否超过了系统范围
                    var num = parseFloat(costPrice);
                    if (num > 10000000 || num < 0) {
                        alert("商品规格的成本价超出了系统表示范围！");
                        $("#tdCostPrice_" + skuId).focus();
                        validated = false;
                        return false;
                    }
                }

                $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function (rowIndex, rowItem) {
                    var id = $(this).attr("id");
                    var memberPrice = $(this).val();
                    if (memberPrice.length > 0) {
                        // 检查输入的是否是有效的金额
                        if (!exp.test(memberPrice)) {
                            alert("商品规格的会员等级价输入有误！");
                            $(this).focus();
                            validated = false;
                            return false;
                        }

                        // 检查金额是否超过了系统范围
                        var num = parseFloat(memberPrice);
                        if (!((num >= 0.01) && (num <= 10000000))) {
                            alert("商品规格的会员等级价超出了系统表示范围！");
                            $(this).focus();
                            validated = false;
                            return false;
                        }
                    }
                    if (validated == false)
                        return false;
                });
                if (validated == false)
                    return false;
            });

            return validated;
        }
       function CloseModal() {
           window.parent.closeModal("divEditBaseInfo");
       }

    
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" style="width: 650px;">
          <div class="page-header">
            <h2>批量修改商品会员零售价格</h2>
            <small>如果会员等级价没填，系统会自动按等级折扣计算；您可以对已选的这些商品直接调价或按公式调价，也可以手工输入您想要的价格后在页底处保存设置</small>
        </div>
        <div class="set-switch">
            <div class="form-inline mb10">
                <div class="form-group mr20">
                    <label for="sellshop1">直接调价: </label>
                   <Hi:MemberPriceDropDownList ID="ddlMemberPrice" runat="server" AllowNull="false" CssClass="form-control" /> = <asp:TextBox ID="txtTargetPrice" CssClass="form-control"  runat="server" Width="80px" />        
                </div>

                <div class="form-group">
                    <label for="sellshop3"></label>
                    <asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>
            <div class="form-inline">
                <div class="form-group mr20">
                    <label for="sellshop4">公式调价:</label>
                   <Hi:MemberPriceDropDownList ID="ddlMemberPrice2" runat="server" AllowNull="false"  CssClass="form-control" /> = <Hi:MemberPriceDropDownList ID="ddlSalePrice" runat="server"  CssClass="form-control"  AllowNull="false" />
                </div>
                <div class="form-group mr20">
                    
               <Hi:OperationDropDownList ID="ddlOperation" runat="server" AllowNull="false"  CssClass="form-control" /><asp:TextBox ID="txtOperationPrice" CssClass="form-control"  runat="server" Width="80px" />
                </div>
                <div class="form-group">
                    <label for="sellshop3"></label>
                    <asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>

        </div>
           <Hi:GridSkuMemberPriceTable runat="server" />
         <div class="form-group">
            <div class="col-xs-offset-4 col-xs-10">
                 <Hi:TrimTextBox runat="server" ID="txtPrices" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
                <button type="button" class="btn btn-default" onclick="CloseModal();">关闭</button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSavePrice" runat="server" OnClientClick="return loadSkuPrice();" Text="保存设置" CssClass="btn btn-success" />

            </div>
        </div>
 
        </form>
</asp:Content>
 
