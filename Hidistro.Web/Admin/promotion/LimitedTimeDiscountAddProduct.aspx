<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="LimitedTimeDiscountAddProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.LimitedTimeDiscountAddProduct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagPrefix="Hi" TagName="SetMemberRange" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .nav-bar-r {
            width: 1020px;
            position: relative;
            left: 50%;
            margin-left: -390px;
            color: #fff;
        }

        #discount0, #minus0 {
            border-radius: 4px;
            border: 1px solid #ccc;
            color: #000;
        }

        .discount, .minus, .inputCss {
            border-radius: 4px;
            border: 1px solid #ccc;
            color: #000;
        }
        .divDiscountCss {
            display: inline-block;
            padding: 3px 5px;
            border: 1px solid rgb(65,154,134);
        }
    </style>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        var selectCount = 0;
        $(document).ready(function () {
            //复制商品路径
            $('.content-table table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            });
            $('input[name="CheckBoxGroup"]').each(function () {
                var check = $(this).prop('checked');
                if (check) {
                    selectCount = selectCount + 1;
                }
            });
            $("#selectCount").html(selectCount);

            //全选按钮
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                selectCount = 0;
                $('input[name="CheckBoxGroup"]').each(function () {
                    if ($(this).prop('disabled') == false) {
                        $(this).prop('checked', check);
                        var id = $(this).val();
                        if (check) {
                            selectCount = selectCount + 1;
                            $("#divDiscount" + id).show();
                            $("#divShowDiscount" + id).hide();

                        }
                        else {
                            selectCount = 0;
                            $("#divDiscount" + id).hide();
                            $("#divShowDiscount" + id).show();
                        }
                    }
                });
                $("#selectCount").html(selectCount);
            });
            //单选
            $('input[name="CheckBoxGroup"]').click(function () {
                var check = $(this).prop('checked');
                var id = $(this).val();
                if (check) {
                    selectCount = selectCount + 1;
                    $("#divDiscount" + id).show();
                    $("#divShowDiscount" + id).hide();
                }
                else {
                    selectCount = selectCount - 1;
                    $("#divDiscount" + id).hide();
                    $("#divShowDiscount" + id).show();
                }
                $("#selectCount").html(selectCount);
            })

            //减元
            $("input.minus").blur(function () {
                //产品编号
                var id = $(this).attr("data");
                //折扣清空
                $("#discount" + id).val("");
                //减去价格
                var minusPrice = $(this).val();
                if (parseFloat(minusPrice) <= 0) {
                    ShowMsg('请输入大于0的数字！', false);
                    return;
                }
                if (minusPrice != "" && minusPrice != null) {
                    if (!checkFloat(minusPrice)) {
                        ShowMsg('请输入数字！', false);
                        return;
                    }
                }
                if (id != "0") {//不是批量设置
                    //原价
                    var price = $("#SalePrice" + id).html();
                    if (parseFloat(price) - parseFloat(minusPrice) > 0) {
                        var finalPrice = parseFloat(price) - parseFloat(minusPrice);
                        $("#finalprice" + id).val(Number(finalPrice).toFixed(2));
                    }
                    else {
                        ShowMsg('请输入比原价小的数字！', false);
                    }
                }
            });

            //折扣
            $("input.discount").blur(function () {
                //产品编号
                var id = $(this).attr("data");

                //减元清空
                $("#minus" + id).val("");
                //折扣
                var discount = $(this).val();
                if (discount != "" && discount != null) {
                    if (!checkFloat(discount)) {
                        ShowMsg('请输入数字！', false);
                        return;
                    }
                }
                if (parseFloat(discount) <= 0 || parseFloat(discount) > 9.9) {
                    ShowMsg('请输入大于0到9.9的数字！', false);
                    return;
                }
                if (id != "0") {//不是批量设置
                    var price = $("#SalePrice" + id).html();  //原价
                    var finalPrice = parseFloat(price) * (discount / 10);
                    $("#finalprice" + id).val(Number(finalPrice).toFixed(2));
                }
            });
            //批量设置
            $("#SetProduct").on("click", function () {
                //判断是否有选中的产品
                if (selectCount < 1) {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }

                var minus = parseFloat($("#minus0").val());
                if (minus > 0) {//减去价格
                    //遍历
                    var number = 0;
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        var id = $(this).val();
                        if (check) {
                            var price = parseFloat($("#SalePrice" + id).html());
                            if (price - minus <= 0) {
                                number = number + 1;
                            }
                        }
                    });
                    if (number > 0) {
                        ShowMsg('请输入比原价小的数字！', false);
                        return;
                    }
                    else {
                        $('input[name="CheckBoxGroup"]').each(function () {
                            var check = $(this).prop('checked');
                            var id = $(this).val();
                            if (check) {
                                var price = parseFloat($("#SalePrice" + id).html());
                                if (price - minus <= 0) {
                                    number = number + 1;
                                }
                                if (price - minus > 0) {
                                    $("#discount" + id).val("");
                                    $("#minus" + id).val(minus);
                                    $("#finalprice" + id).val(Number(price - minus).toFixed(2));
                                }
                            }
                        });
                    }
                }
                var discount = $("#discount0").val();
                if (parseFloat(discount) > 0 && parseFloat(discount) < 10) {//折扣
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        //数据
                        var id = $(this).val();
                        if (check) {
                            //赋值
                            $("#discount" + id).val(discount);
                            $("#minus" + id).val("");
                            var price = $("#SalePrice" + id).html();  //原价
                            var finalPrice = parseFloat(price) * (discount / 10);
                            $("#finalprice" + id).val(Number(finalPrice).toFixed(2));
                        }
                    });
                }
            });

            //去角
            $("#Dehorned").on("click", function () {
                $('input[name="CheckBoxGroup"]').each(function () {
                    var check = $(this).prop('checked');
                    //数据
                    var id = $(this).val();
                    if (check) {
                        var finalPrice = $("#finalprice" + id).val();
                        var index = finalPrice.indexOf('.');
                        if (index > 0) {
                            var price = parseFloat(finalPrice.substring(index + 1, index + 2));
                            $("#finalprice" + id).val(Number(parseFloat(finalPrice) - (price / 10)).toFixed(2));
                        }
                    }
                });
            });

            //去角分
            $("#ChamferPoint").on("click", function () {
                $('input[name="CheckBoxGroup"]').each(function () {
                    var check = $(this).prop('checked');
                    //数据
                    var id = $(this).val();
                    if (check) {
                        var finalprice = $("#finalprice" + id).val();
                        $("#finalprice" + id).val(Number(parseInt(finalprice)).toFixed(2))
                    }
                });
            });
            //隐藏其他活动商品
            $("#hideOtherProduct").on("click", function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    var limitDiscountId = getParam("id");
                    var discountId = $(this).attr("data");
                    if (check) {
                        if (discountId != "" && discountId != limitDiscountId) {
                            $('tr[id="' + discountId + '"]').hide();
                        }
                    }
                    else {
                        if (discountId != "" && discountId != limitDiscountId) {
                            $('tr[id="' + discountId + '"]').show();
                        }
                    }
                });
            });

            //隐藏本活动商品
            $("#hideThisProduct").on("click", function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    var limitDiscountId = getParam("id");
                    var discountId = $(this).attr("data");
                    if (check) {
                        if (discountId == limitDiscountId) {
                            $('tr[id="' + limitDiscountId + '"]').hide();
                        }
                    }
                    else {
                        $('tr[id="' + limitDiscountId + '"]').show();
                    }
                });
            });
        });

        function checkFloat(str) {
            var re = /^[0-9]+.?[0-9]*$/; //判断字符串是否为数字 //判断正整数 /^[1-9]+[0-9]*]*$/ 
            if (!re.test(str)) {
                return false;
            }
            else {
                return true;
            }
        }

        function saveData() {
            var count = 0;
            var num=0;
            var discountProductList = "";
            var limitedTimeDiscountId = getUrlParam("id");
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    var productId = $(this).val();
                    var discount = $("#discount" + productId).val();
                    var minus = $("#minus" + productId).val();
                    var finalprice = $("#finalprice" + productId).val();
                    if (finalprice == "" || finalprice == null) {
                        count = count + 1;
                    }
                    if (parseFloat(finalprice) <= 0) {
                        num = num + 1;
                    }
                    discountProductList += limitedTimeDiscountId + "^" + productId + "^" + discount + "^" + minus + "^" + finalprice + ",";
                }
            });
            if (selectCount < 1) {
                ShowMsg('请选择活动商品！', false);
                return;
            }

            if (num > 0) {
                ShowMsg('已选商品的最终价格必须大于零！', false);
                return;
            }

            if (count > 0) {
                ShowMsg('请设置已选商品的折扣信息！', false);
                return;
            }

            console.log(discountProductList);
            $.ajax({
                type: "post",
                url: "LimitedTimeDiscountHandler.ashx",
                data: { id: limitedTimeDiscountId, discountProductList: discountProductList, action: "SaveDiscountProduct" },
                dataType: "json",
                success: function (data) {
                    if (data.msg == "success") {
                        HiTipsShow("添加成功", "success", function () {
                            document.location.href = document.location.href;
                        });
                    }
                    else {
                        HiTipsShow(data.msg, "error");
                    }
                },
                error: function () {
                    HiTipsShow("访问服务器出错!", "error");
                }
            });
        }

        //确认按钮事件
        function saveDataAndNext() {
            var actionProductNumber = 0;
            //如果没有选择数据
            $('input[name="CheckBoxGroup"]').each(function () {
                var limitDiscountId = getParam("id");
                var discountId = $(this).attr("data");
                if (limitDiscountId == discountId) {
                    actionProductNumber = actionProductNumber + 1;
                }
            });
            if (selectCount < 1) {
                if (actionProductNumber > 0) {
                    window.location.href = "LimitedTimeDiscountProduct.aspx?id=<%=id %>";
                }
                else {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }
            }
            else {
                var count = 0;
                var discountProductList = "";
                var limitedTimeDiscountId = getUrlParam("id");
                $('input[name="CheckBoxGroup"]').each(function () {
                    if ($(this).prop('checked')) {
                        var productId = $(this).val();
                        var discount = $("#discount" + productId).val();
                        var minus = $("#minus" + productId).val();
                        var finalprice = $("#finalprice" + productId).val();
                        if (finalprice == "" || finalprice == null || parseFloat(finalprice) == 0.0) {
                            count = count + 1;
                        }
                        discountProductList += limitedTimeDiscountId + "^" + productId + "^" + discount + "^" + minus + "^" + finalprice + ",";
                    }
                });

                if (count > 0) {
                    ShowMsg('请设置已选商品的折扣信息！', false);
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "LimitedTimeDiscountHandler.ashx",
                    data: { id: limitedTimeDiscountId, discountProductList: discountProductList, action: "SaveDiscountProduct" },
                    dataType: "json",
                    success: function (data) {
                        if (data.msg == "success") {
                            HiTipsShow("添加成功", "success", function () {
                                document.location.href = "LimitedTimeDiscountProduct.aspx?id=" + limitedTimeDiscountId;
                            });
                        }
                        else {
                            HiTipsShow(data.msg, "error");
                        }
                    },
                    error: function () {
                        HiTipsShow("访问服务器出错!", "error");
                    }
                });
            }
        }

        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
        //二维码
        function winqrcode(url) {
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + url);
            $('#divqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divqrcode").modal({ show: true });
        }
        //复制路径
        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>限时折扣 > <%=actionName %> > 添加活动商品</h2>
        </div>
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist" id="nav">
                <li role="presentation" class="active">
                    <a href="LimitedTimeDiscountAddProduct.aspx?id=<%=id %>">添加活动商品</a>
                </li>
                <li role="presentation">
                    <a href="LimitedTimeDiscountProduct.aspx?id=<%=id %>">已选活动商品</a>
                </li>
            </ul>
            <div class="page-box" style="margin-right: 15px;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
        </div>
        <div class="set-switch" style="margin-top: 10px;">
            <div class="form-inline" style="margin-top: 5px; margin-bottom: 5px; vertical-align: central;">
                商品名称:<asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txtProductName" placeholder="商品名称" Width="200px"></asp:TextBox>
                商品分类:<Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control resetSize inputw150" runat="server" NullToDisplay="请选择商品分类" Width="200" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询" OnClick="btnSeach_Click" />
            </div>
        </div>
        <div class="set-switch" style="margin-top: 10px;">
            <div style="margin-left: 15px">
                <input type="checkbox" id="selectAll" /><label for="selectAll">&nbsp;全选</label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="checkbox" id="hideOtherProduct" /><label for="hideOtherProduct">&nbsp;隐藏其他活动商品</label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="checkbox" id="hideThisProduct" /><label for="hideThisProduct">&nbsp;隐藏本活动商品</label>
            </div>
            <div class="sell-table" style="margin-top: 10px;">
                <div class="title-table">
                    <table class="table">
                        <thead>
                            <tr>
                                <th width="5%"></th>
                                <th width="30%">商品名称</th>
                                <th width="10%" style="text-align: left;">已参与活动</th>
                                <th width="10%" style="text-align: center;">剩余库存</th>
                                <th width="10%" style="text-align: center;">价格</th>
                                <th width="35%" style="text-align: center;">折扣信息</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="content-table">
                    <table class="table">
                        <tbody>
                            <asp:Repeater ID="grdProducts" runat="server">
                                <ItemTemplate>
                                    <tr id="<%# Eval("LimitedTimeDiscountId") %>">
                                        <td width="5%">
                                            <label>
                                                <input type="checkbox" name="CheckBoxGroup" data="<%# Eval("LimitedTimeDiscountId") %>" value="<%# Eval("productws") %>" <%# GetDisable(Eval("ActivityName").ToString(),Eval("LimitedTimeDiscountId"),id)%> /></label></td>
                                        <td width="30%">
                                            <div class="img fl mr10">
                                                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" Width="60" Height="60" />
                                            </div>
                                            <div class="shop-info">
                                                <p class="mb5" id="productname<%# Eval("productws") %>"><%# Eval("ProductName") %></p>
                                                <a class="er" href="javascript:void(0)" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("productws")%>');"></a>
                                                <input type="text" id='urldata<%#  Eval("productws") %>' placeholder="" name='urldata<%# Eval("productws") %>' value='<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("productws")%>' disabled="" style="display: none">
                                                <a class="fz" href="javascript:void(0)" data-clipboard-target='urldata<%# Eval("productws") %>' id='url<%# Eval("productws") %>' onclick="copyurl(this.id);"></a>
                                            </div>
                                        </td>
                                        <td width="10%" style="text-align: center;">
                                            <%#Eval("ActivityName")%>
                                        </td>
                                        <td width="10%" style="text-align: center;">
                                            <%# Eval("Stock") %>
                                        </td>
                                        <td width="10%" style="text-align: right;">
                                            <p><span style="color: #F60;">￥<span id="SalePrice<%# Eval("productws") %>"><%#Globals.ToNum(Eval("SkuNum"))>1?Eval("MaxShowPrice", "{0:f2}"): Eval("SalePrice", "{0:f2}")%></span></span></p>
                                            <span><%# Globals.ToNum(Eval("SkuNum"))>1?"<p style='text-align:center'><span style='background-color:rgb(153,153,153);padding:2px 5px;color:#fff;'>多规格</span></p>":""  %></span>
                                        </td>
                                        <td width="35%" style="text-align: center;">
                                            <div style="display: none" id="divDiscount<%# Eval("productws") %>">
                                               <span class="divDiscountCss">打<input type="text" name="discount" data="<%# Eval("productws") %>" id="discount<%# Eval("productws") %>" class="discount" style="width: 40px" value="<%# Eval("Discount", "{0:f2}")%>" />折</span>
                                                        或<span class="divDiscountCss">减<input type="text" name="minus" id="minus<%# Eval("productws") %>" data="<%# Eval("productws") %>" class="minus" style="width: 40px" value="<%# Eval("Minus", "{0:f2}")%>" />元</span>
                                                       <span class="divDiscountCss">最终价<input type="text" name="finalprice" id="finalprice<%# Eval("productws") %>" style="width: 60px" class="inputCss" value="<%# Eval("FinalPrice", "{0:f2}")%>" />元</span>
                                            </div>
                                            <div style='display: <%# GetDisplay(Eval("ActivityName"))%>' id="divShowDiscount<%# Eval("productws") %>">
                                                <span style="display: <%# GetDisplayValue(Eval("Discount")) %>">打<%# Eval("Discount","{0:f2}")%>折</span>
                                                <span style="display: <%# GetDisplayValue(Eval("Minus")) %>">减<%# Eval("Minus","{0:f2}")%>员</span>
                                                &nbsp;&nbsp;<span style="display: <%# GetDisplayValue(Eval("FinalPrice")) %>">最终价:<%# Eval("FinalPrice","{0:f2}")%>元</span>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="footer-btn navbar-fixed-bottom autow">
            <div class="clearfix nav-bar-r clearfix">
                <div class="fl">
                    <button type="button" class="btn btn-primary">已选<span id="selectCount">0<span></button>
                    <input type="text" id="discount0" class="discount" data="0" style="width: 40px" value="" />&nbsp;折或减&nbsp;<input type="text" id="minus0" data="0" class="minus" style="width: 40px" value="" />
                    <button type="button" class="btn btn-primary" id="SetProduct">批量设置</button>
                    <button type="button" class="btn btn-primary" id="Dehorned">去角</button>
                    <button type="button" class="btn btn-primary" id="ChamferPoint">去角分</button>
                </div>
                <div class="fr">
                    <button type="button" class="btn btn-primary" onclick="saveData();">加入活动</button>
                    <button type="button" class="btn btn-primary" onclick="saveDataAndNext();">确定</button>
                </div>
            </div>
        </div>
        <%-- 商品二维码--%>
        <div class="modal fade" id="divqrcode">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">商品二维码</h4>
                    </div>
                    <div class="modal-body" style="text-align: center">
                        <image id="imagecode" src=""></image>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
