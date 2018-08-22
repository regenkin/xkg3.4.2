<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ProductOnDeleted.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ProductOnDeleted" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function deleteProduct(productId) {

            $("#ctl00_ContentPlaceHolder1_currentProductId").val(productId);
            $("#divdeleteProduct").modal({ show: true });
        }
        $(function () {
            $('.allselect').change(function () {
                if ($(this)[0].checked) {
                    $('.content-table input[type="checkbox"]').prop('checked', true);
                } else {
                    $('.content-table input[type="checkbox"]').prop('checked', false);
                }
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
            $('.content-table table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            })
        })

        function deleteProducts() {
            var v_str = "";
            arrytext = null;
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                ShowMsg("请先选择商品！",false);
                return false;
            }
            $("#ctl00_ContentPlaceHolder1_currentProductId").val(v_str.substring(0, v_str.length - 1));
            $("#divdeleteProduct").modal({ show: true });
        }

        function resetform() {
            document.getElementById("aspnetForm").reset();
        }

        function SetPenetrationStatus(checkobj) {
            if (checkobj.checked) {
                $("#ctl00_ContentPlaceHolder1_hdPenetrationStatus").val("1");
            } else {
                $("#ctl00_ContentPlaceHolder1_hdPenetrationStatus").val("0");
            }
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
    <script src="../js/ZeroClipboard.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="table-page">
            <div class="page-header">
                <h2>商品回收站</h2>
                <small>删除的商品会先放入回收站，可以在这里找回误删的商品或者将商品彻底删除</small>
            </div>
            <div class="page-box">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
        </div>
        <div class="set-switch">
            <div class="form-inline mb10">
                <div class="form-group mr20 isetwidth">
                    <label for="sellshop1">商品名称：</label>
                    <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize inputw150" placeholder="" />
                </div>
                <div class="form-group mr20 isetwidth">
                    <label for="sellshop2">商品分类：</label>
                    <Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control resetSize" runat="server" NullToDisplay="请选择商品分类"/>
                </div>
                <div class="form-group isetwidth">
                    <label for="sellshop3">品牌：</label>
                    <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" CssClass="form-control resetSize inputw150" NullToDisplay="请选择品牌"
                        Width="150" />
                </div>
            </div>
            <div class="form-inline">
                <div class="form-group mr20 isetwidth">
                    <label for="sellshop4">商家编码：</label>
                    <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize inputw150" placeholder="" />
                </div>
                <div class="form-group mr20 isetwidth">
                    <label for="sellshop5">创建时间：</label>
                      <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                </div>
                <div class="form-group isetwidth">
                    <label for="sellshop6">&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                       <Hi:DateTimePicker CalendarType="StartDate" ID="calendarEndDate" runat="server" CssClass="form-control resetSize inputw150" />
                </div>
            </div>
            <div class="reset-search">
                <a class="bl mb5" onclick="resetform();" style="cursor: pointer">清除条件</a>
                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn resetSize btn-primary" />
            </div>
        </div>

<%--        <div class="select-page clearfix" style="margin-top: 20px;">
        </div>--%>
        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="30%">商品名称</th>
                            <th width="10%" style="text-align: left;text-indent: 10px;">价格</th>
                            <th width="10%">总库存</th>
                            <th width="10%">总销量</th>
                            <th width="10%">商家编码</th>
                            <th width="8%">排序</th>
                            <th width="14%">创建时间</th>
                            <th width="8%"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="8">
                                <div class="mb10 table-operation">
                                    <input type="checkbox" id="sells1" class="allselect">
                                    <label for="sells1">全选</label>
                                    <a href="javascript:void(0)" onclick="deleteProducts();" class="btn resetSize btn-danger">彻底删除</a>
                                    &nbsp;︱
                                                <asp:LinkButton runat="server" ID="btnUpShelf" CssClass="btn resetSize btn-primary" Text="还原到出售中" />
                                    <span style="display: none">
                                        <asp:LinkButton runat="server" ID="btnOffShelf" Text="还原到下架区" /></span>

                                    &nbsp;︱&nbsp;
                                       <asp:LinkButton runat="server" ID="btnInStock" Text="还原到仓库里" CssClass="btn resetSize btn-primary" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="content-table">
                <table class="table">
                    <tbody>
                        <asp:Repeater ID="grdProducts" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="30%">
                                        <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("ProductId") %>' />

                                        <div class="img fl mr10">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60"  Width="60" Height="60"/>
                                        </div>
                                        <div class="shop-info">
                                            <p class="mb5" title="<%#Server.HtmlEncode(Eval("ProductName").ToString()) %>"><%#Hidistro.Core.Globals.SubStr(Eval("ProductName").ToString(),68,"...") %></p>
                                            <a class="er" title="点击查看商品二维码" href="javascript:void(0)" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>');"></a>
                                            <input type="text" id='urldata<%# Eval("ProductId") %>' placeholder="" name='urldata<%# Eval("ProductId") %>' value='<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>' disabled="" style="display: none">
                                            <a class="fz" title="点击复制商品链接" href="javascript:void(0)" data-clipboard-target='urldata<%# Eval("ProductId") %>' id='url<%# Eval("ProductId") %>' onclick="copyurl(this.id);"></a>
                                        </div>
                                    </td>
                                    <td width="10%" style="text-align: left;text-indent: 10px;">
                                        <p>原价：<span><%#Eval("MarketPrice", "{0:f2}")%></span></p>
                                        <p>现价：<span><%# Eval("SalePrice", "{0:f2}")%></span></p>
                                    </td>
                                    <td width="10%"><%# Eval("Stock") %></td>
                                    <td width="10%"><%# Eval("SaleCounts") %></td>
                                    <td width="10%"><%#Eval("ProductCode") %></td>
                                    <td width="8%"><%#Eval("DisplaySequence") %></td>
                                    <td width="14%"><%#Eval("AddedDate") %></td>
                                    <td width="8%">

                                        <p>
                                            <a href="javascript:void(0)" onclick="deleteProduct('<%# Eval("ProductId") %>');">彻底删除</a>
                                        </p>

                                        <p>
                                            <Hi:ImageLinkButton ID="UpShelf" CommandName="UpShelf" CommandArgument='<%# Eval("ProductId") %>' runat="server" Text="还原到出售中" IsShow="false" />
                                        </p>
                                        <p>
                                            <Hi:ImageLinkButton ID="InStock" CommandName="InStock" CommandArgument='<%# Eval("ProductId") %>' runat="server" Text="还原到仓库里" IsShow="false" />
                                        </p>

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

        <%--彻底删除--%>
        <div class="modal fade" id="divdeleteProduct">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">彻底删除</h4>
                    </div>
                    <div class="modal-body">
                        <p>
                            是否删除图片：<asp:CheckBox ID="chkDeleteImage" Text="删除图片" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <asp:Button ID="btnOK" runat="server" Text="彻底删除" CssClass="btn btn-primary" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>


        <div style="display: none">

            <input type="hidden" id="hdPenetrationStatus" value="1" runat="server" />
            <input runat="server" type="hidden" id="currentProductId" />
        </div>

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
