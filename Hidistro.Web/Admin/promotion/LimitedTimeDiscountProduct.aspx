<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LimitedTimeDiscountProduct.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.LimitedTimeDiscountProduct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagPrefix="Hi" TagName="SetMemberRange" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            })

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
                            $("#edit" + id).show();
                            $("#show" + id).hide();
                        }
                        else {
                            selectCount = 0;
                            $("#edit" + id).hide();
                            $("#show" + id).show();
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
                    $("#edit" + id).show();
                    $("#show" + id).hide();
                }
                else {
                    selectCount = selectCount - 1;
                    $("#edit" + id).hide();
                    $("#show" + id).show();
                }
                $("#selectCount").html(selectCount);
            })

            //减元
            $("input.minus").blur(function () {
                //产品编号
                var id = $(this).attr("data");
                //折扣清空
                $("#Discount" + id).val("");
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
                        $("#FinalPrice" + id).val(Number(finalPrice).toFixed(2));
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
                $("#Minus" + id).val("");
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
                    $("#FinalPrice" + id).val(Number(finalPrice).toFixed(2));
                }
            });


            //批量设置
            $("#SetProduct").on("click", function () {
                //判断是否有选中的产品
                if (selectCount < 1) {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }

                var minus = parseFloat($("#Minus0").val());
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
                                    $("#Discount" + id).val("");
                                    $("#Minus" + id).val(minus);
                                    $("#FinalPrice" + id).val(Number(price - minus).toFixed(2));
                                }
                            }
                        });
                    }
                }
                var discount = $("#Discount0").val();
                if (parseFloat(discount) > 0 && parseFloat(discount) < 10) {//折扣
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        //数据
                        var id = $(this).val();
                        if (check) {
                            //赋值
                            $("#Discount" + id).val(discount);
                            $("#Minus" + id).val("");
                            var price = $("#SalePrice" + id).html();  //原价
                            var finalPrice = parseFloat(price) * (discount / 10);
                            $("#FinalPrice" + id).val(Number(finalPrice).toFixed(2));
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
                        var finalprice = $("#FinalPrice" + id).val();
                        var index = finalprice.indexOf('.');
                        if (index > 0) {
                            var price = parseFloat(finalprice.substring(index + 1, index + 2));
                            $("#FinalPrice" + id).val(Number((parseFloat(finalprice) - (price / 10))).toFixed(2));
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
                        var finalprice = $("#FinalPrice" + id).val();
                        $("#FinalPrice" + id).val(Number(parseInt(finalprice)).toFixed(2));
                    }
                });
            });
            $(".btnSave").on("click", function () {
                var productId = $(this).attr("data");
                //验证 
                var minus = parseFloat($("#Minus" + productId).val());
                var discount = parseFloat($("#Discount" + productId).val());
                if (minus > 0) {
                    if (!checkFloat(minus)) {
                        ShowMsg('请输入数字！', false);
                        return;
                    }
                }
                else {
                    if (!checkFloat(discount)) {
                        ShowMsg('请输入数字！', false);
                        return;
                    }
                    if (parseFloat(discount) < 0 || parseFloat(discount) > 9.9) {
                        ShowMsg('请输入大于0到9.9的数字！', false);
                        return;
                    }
                }
                var finalPrice = parseFloat($("#FinalPrice" + productId).val());
                if (finalPrice <= 0) {
                    ShowMsg('已选商品的最终价格必须大于零！', false);
                    return;
                }
                //保存数据
                console.log("limitedTimeDiscountId:" + limitedTimeDiscountId + " discount: " + discount + ",finalPrice:" + finalPrice);
                var limitedTimeDiscountId = getUrlParam("id");
                $.ajax({
                    type: "post",
                    url: "LimitedTimeDiscountHandler.ashx",
                    data: { LimitedTimeDiscountId: limitedTimeDiscountId, ProductId: productId, FinalPrice: finalPrice, Discount: discount, Minus: minus, action: "SaveDiscountProductInfo" },
                    dataType: "json",
                    success: function (data) {
                        if (data.msg == "success") {
                            HiTipsShow("修改成功", "success", function () {
                                document.location.href = document.location.href;
                            });
                        }
                        else {
                            HiTipsShow("参数错误", "error");
                        }
                    },
                    error: function () {
                        HiTipsShow("访问服务器出错!", "error");
                    }
                });
            });

            //批量移除活动
            $("#btnMoves").on("click", function () {
                //判断是否有选中的产品
                if (selectCount < 1) {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }
                var flag = HiConform('<strong>您确认把商品移出当前活动！</strong><p>是否继续？</p>', this);
                if (flag) {
                    var ids = "";
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        //数据
                        var id = $(this).val();
                        var limitedTimeDiscountProductId = $(this).attr("data");
                        if (check) {
                            ids = ids + limitedTimeDiscountProductId + ",";
                        }
                    });
                    ids = ids.substring(0, ids.length - 1);
                    $.ajax({
                        type: "post",
                        url: "LimitedTimeDiscountHandler.ashx",
                        data: { limitedTimeDiscountProductIds: ids, action: "DeleteDiscountProduct" },
                        dataType: "json",
                        success: function (data) {
                            if (data.msg == "success") {
                                HiTipsShow("移除成功", "success", function () {
                                    document.location.href = document.location.href;
                                });
                            }
                            else {
                                HiTipsShow("参数错误", "error");
                            }
                        },
                        error: function () {
                            HiTipsShow("访问服务器出错!", "error");
                        }
                    });
                }
            });

            //批量暂停
            $("#btnStops").on("click", function () {
                //判断是否有选中的产品
                if (selectCount < 1) {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }
                var flag = HiConform('<strong>您确认要暂停活动中的商品！</strong><p>是否继续？</p>', this);
                if (flag) {
                    var ids = "";
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        //数据
                        var id = $(this).val();
                        var limitedTimeDiscountProductId = $(this).attr("data");
                        if (check) {
                            ids = ids + limitedTimeDiscountProductId + ",";
                        }
                    });
                    ids = ids.substring(0, ids.length - 1);
                    var status = 3;
                    $.ajax({
                        type: "post",
                        url: "LimitedTimeDiscountHandler.ashx",
                        data: { id: ids, status: status, action: "ChangeDiscountProductStatus" },
                        dataType: "json",
                        success: function (data) {
                            if (data.msg == "success") {
                                HiTipsShow("状态修改成功", "success", function () {
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
            });

            //批量重启
            $("#btnRestart").on("click", function () {
                //判断是否有选中的产品
                if (selectCount < 1) {
                    ShowMsg('请选择活动商品！', false);
                    return;
                }
                var flag = HiConform('<strong>您确认要重启活动中的商品！</strong><p>是否继续？</p>', this);
                if (flag) {
                    var ids = "";
                    $('input[name="CheckBoxGroup"]').each(function () {
                        var check = $(this).prop('checked');
                        //数据
                        var id = $(this).val();
                        var limitedTimeDiscountProductId = $(this).attr("data");
                        if (check) {
                            ids = ids + limitedTimeDiscountProductId + ",";
                        }
                    });
                    ids = ids.substring(0, ids.length - 1);
                    var status = 1;
                    $.ajax({
                        type: "post",
                        url: "LimitedTimeDiscountHandler.ashx",
                        data: { id: ids, status: status, action: "ChangeDiscountProductStatus" },
                        dataType: "json",
                        success: function (data) {
                            if (data.msg == "success") {
                                HiTipsShow("状态修改成功", "success", function () {
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
            });
        });

        //验证
        function checkFloat(str) {
            var re = /^[0-9]+.?[0-9]*$/; //判断字符串是否为数字 //判断正整数 /^[1-9]+[0-9]*]*$/ 
            if (!re.test(str)) {
                return false;
            }
            else {
                return true;
            }
        }

        //移出活动
        function DeleteDiscountProduct(limitedTimeDiscountProductId, obj) {
            var flag = HiConform('<strong>您确认把商品移除当前活动！</strong><p>是否继续？</p>', obj);
            if (flag) {
                $.ajax({
                    type: "post",
                    url: "LimitedTimeDiscountHandler.ashx",
                    data: { limitedTimeDiscountProductIds: limitedTimeDiscountProductId, action: "DeleteDiscountProduct" },
                    dataType: "json",
                    success: function (data) {
                        if (data.msg == "success") {
                            HiTipsShow("状态修改成功", "success", function () {
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
        }

        //暂停活动，恢复活动
        function stop(id, status, obj) {
            var flag = HiConform('<strong>您确认要修改当前活动的状态！</strong><p>是否继续？</p>', obj);
            if (flag) {
                var sta = 1;
                if (status == 3) {
                    sta = 1;
                }
                else {
                    sta = 3;
                }
                $.ajax({
                    type: "post",
                    url: "LimitedTimeDiscountHandler.ashx",
                    data: { id: id, status: sta, action: "ChangeDiscountProductStatus" },
                    dataType: "json",
                    success: function (data) {
                        if (data.msg == "success") {
                            HiTipsShow("状态修改成功", "success", function () {
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
        }
        //编辑按钮事件
        function edit(productId) {
            $("#spanSave" + productId).show();
            selectCount = selectCount + 1;
            $('#ck' + productId).attr("checked", "checked");
            $("#edit" + productId).show();
            $("#show" + productId).hide();
            $("#selectCount").html(selectCount);
        }

        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }

        function saveData() {
            var discountProductList = "";
            var limitedTimeDiscountId = getUrlParam("id");
            var count = 0;
            var num = 0;
            //判断是否有选中的产品
            if (selectCount < 1) {
                ShowMsg('请选择活动商品！', false);
                return;
            }

            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    var productId = $(this).val();
                    var limitedTimeDiscountProductId = $(this).attr("data");
                    var discount = $("#Discount" + productId).val();
                    var minus = $("#Minus" + productId).val();
                    var finalprice = $("#FinalPrice" + productId).val();
                    if (finalprice == "" || finalprice == null) {
                        count = count + 1;
                    }
                    if (parseFloat(finalprice) <= 0) {
                        num = num + 1;
                    }
                    discountProductList += limitedTimeDiscountProductId + "^" + productId + "^" + discount + "^" + minus + "^" + finalprice + ",";
                }
            });

            if (count > 0) {
                ShowMsg('请设置已选商品的折扣信息！', false);
                return;
            }

            if (num > 0) {
                ShowMsg('已选商品的最终价格必须大于零！', false);
                return;
            }

            $.ajax({
                type: "post",
                url: "LimitedTimeDiscountHandler.ashx",
                data: { LimitedTimeDiscountId: limitedTimeDiscountId, discountProductList: discountProductList, action: "UpdateDiscountProductList" },
                dataType: "json",
                success: function (data) {
                    if (data.msg == "success") {
                        HiTipsShow("修改成功", "success", function () {
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

        function winqrcode(url) {
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + url);
            $('#divqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divqrcode").modal({ show: true });
        }

        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }
    </script>
    <style>
        .nav-bar-r {
            width: 1020px;
            position: relative;
            left: 50%;
            margin-left: -390px;
            color: #fff;
        }

        #Discount0, #Minus0 {
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
        display:inline-block;padding:3px 5px;border:1px solid rgb(65,154,134);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>限时折扣 > <%=actionName %> > 已选活动商品</h2>
        </div>
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist" id="nav">
                <li role="presentation">
                    <a href="LimitedTimeDiscountAddProduct.aspx?id=<%=id %> ">添加活动商品</a>
                </li>
                <li role="presentation" class="active">
                    <a href="LimitedTimeDiscountProduct.aspx?id=<%=id %> ">已选活动商品</a>
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
                商品分类:<Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control resetSize inputw150" runat="server" NullToDisplay="请选择商品分类" Width="200px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询" OnClick="btnSeach_Click" />
            </div>
        </div>
        <div class="set-switch" style="margin-top: 10px;">
            <div style="margin-top: 10px; margin-left: 15px">
                <input type="checkbox" id="selectAll" /><label for="selectAll"> 全选</label>
                <input type="button" class="btn resetSize btn-primary" id="btnMoves" value="移出活动" />&nbsp;&nbsp;
                        <input type="button" id="btnStops" class="btn resetSize btn-primary" value="暂停" />&nbsp;&nbsp;
                        <input type="button" id="btnRestart" class="btn resetSize btn-primary" value="重启" />
            </div>
            <div class="sell-table" style="margin-top: 10px;">
                <div class="title-table">
                    <table class="table">
                        <thead>
                            <tr>
                                <th width="5%"></th>
                                <th width="30%">商品名称</th>
                                <th width="7%">价格</th>
                                <th width="35%">折扣信息</th>
                                <th width="7%" style="text-align: left;">状态</th>
                                <th width="14%">操作</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="content-table">
                    <table class="table">
                        <tbody>
                            <asp:Repeater ID="grdProducts" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td width="5%">
                                            <label>
                                                <input type="checkbox" name="CheckBoxGroup" id="ck<%# Eval("ProductId") %>" value="<%# Eval("ProductId") %>" data="<%# Eval("LimitedTimeDiscountProductId") %>" /></label></td>
                                        <td width="30%">
                                            <div class="img fl mr10">
                                                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" Width="60" Height="60" />
                                            </div>
                                            <div class="shop-info">
                                                <p class="mb5"><%# Eval("ProductName") %></p>
                                                <a class="er" href="javascript:void(0)" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>');"></a>
                                                <input type="text" id='urldata<%# Eval("ProductId") %>' placeholder="" name='urldata<%# Eval("ProductId") %>' value='<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>' disabled="" style="display: none">
                                                <a class="fz" href="javascript:void(0)" data-clipboard-target='urldata<%# Eval("ProductId") %>' id='url<%# Eval("ProductId") %>' onclick="copyurl(this.id);"></a>
                                            </div>
                                        </td>
                                        <td width="7%" style="text-align: right;">
                                            <p><span style="color: #F60;">￥<span id="SalePrice<%# Eval("ProductId") %>"><%# Eval("SalePrice", "{0:f2}")%></span></span></p>
                                        </td>
                                        <td width="35%">
                                            <div id="show<%# Eval("ProductId") %>">
                                                <span style="display: <%# GetDisplayValue(Eval("Discount")) %>">打<%# Eval("Discount","{0:f2}")%>折</span>
                                                <span style="display: <%# GetDisplayValue(Eval("Minus")) %>">减<%# Eval("Minus","{0:f2}")%>元</span>
                                                &nbsp;&nbsp;<span>最终价:<%# Eval("FinalPrice","{0:f2}")%>元</span>
                                            </div>
                                            <div id="edit<%# Eval("ProductId") %>" style="display: none">
                                                <span class="divDiscountCss">打<input style="margin:0 5px;width:40px;" id="Discount<%# Eval("ProductId") %>" type="text" value="<%# Eval("Discount","{0:f2}")%>" data="<%# Eval("ProductId") %>" name="discount" class="discount" value="" />折</span>
                                                或 <span class="divDiscountCss">减<input style="margin:0 5px;width:40px;color:#000;" id="Minus<%# Eval("ProductId") %>" type="text" value="<%# Eval("Minus","{0:f2}")%>" data="<%# Eval("ProductId") %>" name="minus" class="minus" value="" />元</span>
                                                <span class="divDiscountCss">最终价:<input style="margin:0 5px;width:40px;" id="FinalPrice<%# Eval("ProductId") %>" type="text" value="<%# Eval("FinalPrice","{0:f2}")%>" name="inputCss" class="inputCss" value="" />元</span>
                                                <span id="spanSave<%# Eval("ProductId") %>" style="display: none">
                                                    <input type="button" id="btnSave<%# Eval("ProductId") %>" class="btnSave" value="OK" data="<%# Eval("ProductId") %>" /></span>
                                            </div>
                                        </td>
                                        <td width="7%" style="text-align: center;"><%# GetStatus(Eval("Status").ToString()) %></td>
                                        <td width="14%" style="text-align: right;">
                                            <asp:Button ID="btnMove" CommandName="MoveProduct" CommandArgument='<%# Eval("LimitedTimeDiscountProductId") %>' runat="server" CssClass="btnLink pad" Text="移出" OnClientClick="return HiConform('<strong>您确认把商品移除当前活动！</strong><p>是否继续？</p>',this);" />
                                            &nbsp;&nbsp;<a href="javascript:edit(<%# Eval("ProductId")%>)" id="btnEdit">编辑</a>
                                            &nbsp;&nbsp;
                                                <asp:Button ID="btnStopAndRestart" runat="server" CommandName="Stop" CssClass="btnLink pad" CommandArgument='<%# Eval("LimitedTimeDiscountProductId") %>' Text='<%# Eval("Status").ToString()=="3"?"重启":"暂停" %>' OnClientClick="return HiConform('<strong>您确认要改变活动中的商品状态！</strong><p>是否继续？</p>',this);" />
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
                    <input type="text" id="Discount0" class="discount" data="0" style="width: 40px" value="" />&nbsp;折或减&nbsp;<input type="text" id="Minus0" data="0" class="minus" style="width: 40px" value="" />
                    <button type="button" class="btn btn-primary" id="SetProduct">批量设置</button>
                    <button type="button" class="btn btn-primary" id="Dehorned">去角</button>
                    <button type="button" class="btn btn-primary" id="ChamferPoint">去角分</button>
                </div>
                <div class="fr">
                    <button type="button" class="btn btn-primary" onclick="saveData();">确定</button>
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
