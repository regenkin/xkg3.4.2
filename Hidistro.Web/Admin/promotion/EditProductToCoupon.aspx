<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProductToCoupon.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.EditProductToCoupon" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectBtnCss{width:200px;height:40px;}
    </style>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    $(this).prop('checked', check);
                });
            });

            $('#saveAllBtn').click(function () {
                saveAll();
            });

            $('.content-table table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            });
            var tableTitle = $('.title-table').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.title-table').css({
                        position: 'fixed',
                        top: '58px'
                    })
                }
                if ($(document).scrollTop() + $('.title-table').height() + 58 <= tableTitle) {
                    $('.title-table').removeAttr('style');
                }
            });
        });

        function winqrcode(url) {
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + url);
            $('#divqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divqrcode").modal({ show: true });
        }
        function closeModal(obj) {
            $("#" + obj).modal('hide');
            location.reload();
        }

        function saveAll() {
            window.location.href = "CouponsList.aspx";
        }

        function stopSingle(productId) {
            var couponId = $('#txt_coupon').val();
            var data = {
                actType: '0',
                id: couponId,
                products: productId,
                type: 0
            };
            $.ajax({
                type: "post",
                url: "EditProductHandler.ashx",
                data: data,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        window.location.reload();
                    }
                    else {
                        ShowMsg("暂停商品失败（" + data.data + ")");
                    }
                }
            });
        }

        function openSingle(productId) {
            var couponId = $('#txt_coupon').val();
            var data = {
                actType: '0',
                id: couponId,
                products: productId,
                type: 1
            };
            $.ajax({
                type: "post",
                url: "EditProductHandler.ashx",
                data: data,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        window.location.reload();
                    }
                    else {
                        ShowMsg("恢复商品失败（" + data.data + ")");
                    }
                }
            });
        }

        function removeSingle(productId) {
            var couponId = $('#txt_coupon').val();
            var data = {
                actType: '0',
                id: couponId,
                products: productId,
                type: 2
            };
            $.ajax({
                type: "post",
                url: "EditProductHandler.ashx",
                data: data,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        window.location.reload();
                    }
                    else {
                        ShowMsg("移除商品失败（" + data.data + ")");
                    }
                }
            });
        }

        function BatchStop() {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                stopSingle(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchOpen() {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                openSingle(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchRemove() {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                removeSingle(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>优惠券商品选择</h2>
        </div>
        <input type="hidden" id="txt_coupon" value="<%=couponId%>" />
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist">
                <li id="tabHeader_coupondsjoin" role="presentation" class="active">
                    <a href="EditProductToCoupon.aspx?id=<%=couponId%>">已加入(<asp:Label runat="server" ID="lblJoin" Text="0"></asp:Label>)</a>
                </li>
                <li id="tabHeader_couponds" role="presentation">
                    <a href="AddProductToCoupon.aspx?id=<%=couponId%>">出售中(<asp:Label runat="server" ID="lbsaleNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_memberCouponds" role="presentation" >
                    <a href="AddProductToCoupon_stock.aspx?id=<%=couponId%>">仓库中(<asp:Label runat="server" ID="lbwareNumber" Text="0"></asp:Label>)</a>
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

        <div class="set-switch" style="margin-top:10px;">
        <div class="form-inline">
            <label>商品名称:</label>
            <asp:TextBox ID="txt_name" Width="100" runat="server" CssClass="form-control resetSize mr20" />

            <label>现价格区间:</label>
            <asp:TextBox ID="txt_minPrice" Width="100" runat="server" CssClass="form-control resetSize" />
            <label>至</label>
            <asp:TextBox ID="txt_maxPrice" Width="100" runat="server" CssClass="form-control resetSize mr20" />
            <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary resetSize" />
        </div>
        </div>

        <div class="form-inline" style="margin-bottom: 10px; margin-top:-10px;">
            <table style="width:100%;margin-top:5px;">
                <tr>
                    <td width="77%">
                        <ul>
                            <li class="batchHandleButton">
                                <input type="checkbox" name="selectAll" id="selectAll" /> 全选
                                <button type="button" class="btn btn-warning resetSize" onclick="BatchStop()" style="margin-left:20px;">
                                    批量暂停
                                </button>

                                <button type="button" class="btn btn-success resetSize" onclick="BatchOpen()" style="margin-left:10px;">
                                    批量恢复
                                </button>
                                                                                              
                                <button type="button" class="btn btn-danger resetSize" onclick="BatchRemove()" style="margin-left:10px;">
                                    批量移除
                                </button>                                
                            </li>
                        </ul>
                    </td>
                    
                </tr>
            </table>
        </div>

        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="50%">商品名称</th>
                            <th width="10%" style="text-align:left;">价格</th>
                            <th width="40%"></th>
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
                                    <td width="50%">
                                        
                                        <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("ProductId") %>' />

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
                                    <td width="10%" style="text-align:left;">
                                        <p>原价：<span><%#Eval("MarketPrice", "{0:f2}")%></span></p>
                                        <p>现价：<span style="color:#F60;"><%# Eval("SalePrice", "{0:f2}")%></span></p>
                                    </td>                                  
                                    <td style="text-align:center; width:40%;">
                                        <button type="button" onclick='<%#string.Format("stopSingle({0});", Eval("ProductId"))%>'
                                            class="btn btn-warning resetSize" id="stopBtn" <%#string.Format("style=\" display :{0}\"", Eval("canstopStatus"))%>>                                            
                                            暂停                                            
                                        </button>
                                        <button type="button" onclick='<%#string.Format("openSingle({0});", Eval("ProductId"))%>'
                                            class="btn btn-success resetSize" id="openBtn" <%#string.Format("style=\" display :{0}\"", Eval("canopenStatus"))%>>                                            
                                            恢复                                            
                                        </button>

                                        <button type="button" onclick='<%#string.Format("removeSingle({0});", Eval("ProductId"))%>'
                                            class="btn btn-danger resetSize" id="removeBtn" style="margin-left:20px;">                                            
                                            移除                                            
                                        </button>
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
        <div style="height:50px;"></div>

        <div class="footer-btn navbar-fixed-bottom autow">
            <div style="text-align:center;margin-right:100px;">
                <input type="button" id="saveAllBtn" class="btn btn-primary" value="完 成" />
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>   
    </form>
</asp:Content>
