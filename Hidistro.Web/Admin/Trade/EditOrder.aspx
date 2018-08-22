<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="EditOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.EditOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .listInfoLeft {
            width: 730px;
        }

        .orderManagementList {
            width: 880px;margin-bottom: 0px;
        }

        #divEdit {
            max-height: 400px;
            overflow-y: auto;
        }

        .borderred {
            border: 1px solid red;
        }
        .orderInfolist.clearfix input {
            width: 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div id="divEdit">
            订单原价（不含运费）<span style="color: red">￥<asp:Literal ID="litOrderGoodsTotalPrice" runat="server"></asp:Literal></span>
            <div class="orderManagementList">
                <ul>
                    <li>
                        <div class="listTitle">
                            <span style="width: 285px; display: inline-block; margin-left: 2px;">商品信息</span>
                            <span style="width: 110px; display: inline-block; margin-left: 2px;">单价</span>
                            <span style="width: 90px; display: inline-block; margin-left: 2px;">数量</span>
                            <span style="width: 115px; display: inline-block; margin-left: 2px;">涨价或优惠</span>
                            <span style="width: 100px; display: inline-block; margin-left: 2px;">小计</span>
                            <span style="width: 145px; display: inline-block; margin-left: 2px; text-align: center">运费</span>
                        </div>
                        <table id="tbShow">
                            <tbody>
                                <tr>
                                    <td width="730">
                                        <div class="listInfoLeft">
                                            <asp:Repeater ID="rptItemList" runat="server">
                                                <ItemTemplate>
                                                    <div class="orderInfolist clearfix">
                                                        <span style="width: 285px; display: inline-block;"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductID") %>" target="_blank" data-toggle="tooltip" data-placement="right" title="<%#Server.HtmlEncode(Eval("ItemDescription").ToString()) %>"><%# Hidistro.Core.Globals.SubStr(Eval("ItemDescription").ToString(),38,"...") %></a></span>
                                                        <span style="width: 110px; display: inline-block;">￥<%# Eval("ItemListPrice","{0:f2}") %></span>
                                                        <span style="width: 90px; display: inline-block;"><%# Eval("Quantity") %></span>
                                                        <span style="width: 115px; display: inline-block;">
                                                            <input type="hidden" name="discountAverage" value="<%#Eval("DiscountAverage") %>" />
                                                            <input type="hidden" name="skuid" value="<%#Eval("ID") %>" />
                                                            <input type="hidden" name="itemtotalprice" value="<%#(decimal.Parse(Eval("ItemListPrice").ToString())*(int.Parse(Eval("Quantity").ToString()))).ToString("f2") %>" />
                                                            <%#FormatAdjustedCommssion(Eval("Type"),Eval("ItemAdjustedCommssion")) %>
                                                        </span>
                                                        <span style="width: 100px; display: inline-block;">￥<span id="showitemprice"><%# Eval("ItemAdjustedPrice","{0:f2}") %></span></span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </td>
                                    <td width="150">
                                        <div class="clearfix" style="text-align: center">
                                            <p>
                                                <asp:Literal ID="litLogistic" runat="server" Text="快递"></asp:Literal></p>
                                            <p>
                                                <asp:TextBox ID="txtAdjustedFreight" runat="server" Width="75" CssClass="alignc" MaxLength="7"></asp:TextBox>
                                            </p>
                                            <p>直接输入</p>
                                            <p>运费金额</p>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <p class="pt5">买家实付：<span id="spItemTotalPrice"><asp:Literal ID="litItemTotalPrice" runat="server"></asp:Literal></span> <span id="spAddPrice"></span>+ <span id="spAdjustedFreight"></span>= ￥<span id="spOrderTotal"><asp:Literal ID="litOrderTotal" runat="server"></asp:Literal></span></p>
                        <p style="color: #999">买家实付 = 小计 + 涨价或优惠 + 运费</p>
                        <p style="color: #999">订单优惠的<span id="spAdjustedCommssion"><asp:Literal ID="litAdjustedCommssion" runat="server"></asp:Literal></span>元已均摊到每个商品，并计算在每个商品的优惠中（积分兑换商品除外）</p>
                    </li>
                </ul>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-success inputw100" id="btnConfirm">确定</button>
            <button type="button" class="btn btn-default inputw100" id="btnCancel">取消</button>
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            iframeHeight = 300;
            var lineCount = $(".orderInfolist.clearfix").length;
            if (lineCount > 2 && lineCount < 5) {
                iframeHeight += (lineCount - 2) * 60;
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            } else if (lineCount < 3) {
                parent.SetObjHeight("#divmyIframeModalIframe", iframeHeight);
            }
            GetCountPriceShow(null);
            $("input[name='adjustedcommssion']").blur(function () {

                var temp = $(this).val();
                var t = temp.charAt(0);
                if (temp == "" || temp == "-") {
                    temp = "0";
                } else {
                    if ('' != temp.replace(/\d{1,}\.{0,1}\d{0,}/, '')) {
                        temp = temp.match(/\d{1,}\.{0,1}\d{0,}/) == null ? '' : temp.match(/\d{1,}\.{0,1}\d{0,}/);
                    }
                    if (temp == "") {
                        temp = "0";
                    }
                }
                
                $(this).val((t == '-' ? "-" : "") + Number(temp).toFixed(2));
                GetCountPriceShow(this);

            })
            $("#ctl00_ContentPlaceHolder1_txtAdjustedFreight").keyup(function () {
                var temp = $(this).val();
                if ('' != temp.replace(/\d{1,}\.{0,1}\d{0,}/, '')) {
                    temp = temp.match(/\d{1,}\.{0,1}\d{0,}/) == null ? '' : temp.match(/\d{1,}\.{0,1}\d{0,}/);
                }
                $(this).val(temp);
                GetCountPriceShow(this);
            })
            $("#btnConfirm").click(function () {
                var adjustedFreight = parseFloat($("#ctl00_ContentPlaceHolder1_txtAdjustedFreight").val());
                var jdata = "";
                $(".orderInfolist").each(function () {
                    var itemadd = parseFloat($(this).find("input[name='adjustedcommssion']").val());
                    var skuid = $(this).find("input[name='skuid']").val()
                    if (jdata == "") {
                        jdata = "{\"skuid\":\"" + skuid + "\",\"adjustedcommssion\":" + itemadd + "}";
                    } else {
                        jdata += ",{\"skuid\":\"" + skuid + "\",\"adjustedcommssion\":" + itemadd + "}";
                    }
                });
                var jsonTips = "[{\"o\":\"<%=orderId%>\",\"f\":" + adjustedFreight + ",\"data\":[" + jdata + "]}]";
                //alert(jsonTips); return false;
                $.ajax({
                    url: "editorder.aspx",
                    type: "post",
                    data: "posttype=updateorder&data=" + jsonTips,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            parent.HiTipsShow(json.tips, "success", function () {
                                //parent.$('#divmyIframeModal').modal('hide');
                                parent.location.href = "<%=ReUrl%>";
                            })
                        } else {
                            parent.HiTipsShow(json.tips, "fail");
                        }
                    }
                });
            })
            $("#btnCancel").click(function () {
                parent.$('#divmyIframeModal').modal('hide')
            })
            $("[data-toggle='tooltip']").tooltip({ html: false });
        })
        function GetCountPriceShow(obj) {
            if (obj != null && $(obj).val() == "-") {
                $(obj).val(0);
            }
            var itemListTotalPrice = 0.00;
            var itemAddPrice = 0.00;
            var adjustedFreight = parseFloat($("#ctl00_ContentPlaceHolder1_txtAdjustedFreight").val());
            $(".orderInfolist").each(function () {
                var itemval = parseFloat($(this).find("input[name='itemtotalprice']").val());
                var itemadd = parseFloat($(this).find("input[name='adjustedcommssion']").val());
                var discountAverage = parseFloat($(this).find("input[name='discountAverage']").val());
                itemAddPrice += itemadd;
                var tempcompare = itemval + itemadd - discountAverage;
                itemListTotalPrice += (tempcompare);
                if (tempcompare < 0) {
                    $(obj).val(0);
                    GetCountPriceShow(null);
                    return false;
                }
                var html = (tempcompare).toFixed(2);
                $(this).find("span[id='showitemprice']").html(html);
            });
            var adjustedCommssion = parseFloat($("#spAdjustedCommssion").html());
            itemAddPrice -= adjustedCommssion;
            //$("#spItemTotalPrice").html(itemListTotalPrice.toFixed(2));
            $("#spAddPrice").html((itemAddPrice > 0 ? " + " : " - ") + Math.abs(itemAddPrice).toFixed(2));

            itemListTotalPrice += adjustedFreight;
            if (itemListTotalPrice > 0) {
                $("#spOrderTotal").html(itemListTotalPrice.toFixed(2));
                $("#btnConfirm").removeAttr("disabled");
            } else {
                $("#btnConfirm").attr("disabled", "disabled");
                parent.HiTipsShow("请输入合法的数值！", "error", function () {
                    if (obj != null) {
                        $(obj).val(0.00);
                        GetCountPriceShow(null);
                        $(obj).addClass("borderred");
                        setTimeout(function () {
                            $(obj).removeClass("borderred");
                        }, 2000);
                    }
                });
            }
            //$("#spAdjustedCommssion").html((itemAddPrice > 0 ? "涨价的" : "优惠的") + Math.abs(itemAddPrice).toFixed(2))
            $("#spAdjustedFreight").html(adjustedFreight.toFixed(2));
        }
    </script>
</asp:Content>
