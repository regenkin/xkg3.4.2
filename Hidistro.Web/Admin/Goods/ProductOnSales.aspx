<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true"
    CodeBehind="ProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ProductOnSales" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dropBatchOperation").bind("change", function () { SelectOperation(''); });
        });

        function SelectOperation(obj) {
            var Operation = $("#dropBatchOperation").val();
            if (obj != "")
                Operation = obj;
            var productIds = GetProductId();
            if (productIds.length > 0) {

                switch (Operation) {
                    case "1":
                        formtype = "onsale";
                        arrytext = null;
                        DialogShow("商品上架", "productonsale", "divOnSaleProduct", "ctl00_contentHolder_btnUpSale");
                        break;
                    case "2":
                        formtype = "unsale";
                        arrytext = null;
                        $("#divUnSaleProduct").modal('toggle').children().css({
                            width: 500 + 'px',
                            height: 300 + 'px'
                        })

                        break;
                    case "3":
                        formtype = "instock";
                        arrytext = null;
                        DialogShow("商品入库", "productinstock", "divInStockProduct", "ctl00_contentHolder_btnInStock");
                        break;
                    case "5":
                        formtype = "setFreeShip";
                        arrytext = null;
                        $("#divSetFreeShip").modal('toggle').children().css({
                            width: 500 + 'px',
                            height: 300 + 'px'
                        })
                        //$("#divSetFreeShip").modal({ show: true });
                        break;
                    case "6":
                        formtype = "cancelFreeShip";
                        arrytext = null;
                        DialogShow("取消包邮", "cancelFreeShip", "divCancelFreeShip", "ctl00_contentHolder_btnCancelFreeShip");
                        break;
                    case "4":
                    case "10":
                        $("#modaltitle").text("调整基本信息");
                        $("#MyEidtBaseInfoIframe").attr("src", "EditBaseInfo.aspx?ProductIds=" + productIds+"&reurl="+encodeURIComponent(location.href));
                        $('#divEditBaseInfo').modal('toggle').children().css({
                            width: '820px',
                            height: '550px'
                        })
                        $("#divEditBaseInfo").modal({ show: true });
                        break;
                    case "11":
                        $("#modaltitle").text("调整显示销售数量");
                        $("#MyEidtBaseInfoIframe").attr("src", "EditSaleCounts.aspx?ProductIds=" + productIds + "&reurl=" + encodeURIComponent(location.href));
                        $('#divEditBaseInfo').modal('toggle').children().css({
                            width: '820px',
                            height: '550px'
                        })
                        $("#divEditBaseInfo").modal({ show: true });
                        break;
                    case "12":
                        $("#modaltitle").text("调整库存");
                        $("#MyEidtBaseInfoIframe").attr("src", "EditStocks.aspx?ProductIds=" + productIds + "&reurl=" + encodeURIComponent(location.href));
                        $('#divEditBaseInfo').modal('toggle').children().css({
                            width: '820px',
                            height: '550px'
                        })
                        $("#divEditBaseInfo").modal({ show: true });
                        break;
                    case "13":
                        $("#modaltitle").text("调整会员价");
                        $("#MyEidtBaseInfoIframe").attr("src", "EditMemberPrices.aspx?ProductIds=" + productIds + "&reurl=" + encodeURIComponent(location.href));
                        $('#divEditBaseInfo').modal('toggle').children().css({
                            width: '820px',
                            height: '550px'
                        })

                        break;
                    case "15":
                        formtype = "tag";
                        setArryText('ctl00_contentHolder_txtProductTag', "");
                        DialogShow("设置商品标签", "producttag", "divTagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
                        break;
                    case "16":
                        formtype = "SetFreightTemplate";
                        $("#divSetFreightTemplate").modal('toggle').children().css({
                            width: 500 + 'px',
                            height: 300 + 'px'
                        })

                        break;

                }
            }
            $("#dropBatchOperation").val("");
        }
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
            //location.reload();

        }
        function GetProductId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                HiTipsShow("请选择商品", "warning")
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "相关商品");
        }

        function validatorForm() {
            switch (formtype) {
                case "tag":
                    if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                        alert("请选择商品标签");
                        return false;
                    }
                    break;
                case "onsale":
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                    break;
                case "unsale":
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                    break;
                case "instock":
                case "setFreeShip":
                case "cancelFreeShip":
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                    break;
            };
            return true;
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
                if($(document).scrollTop()+$('.title-table').height()+58 <= tableTitle){
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
        function resetform() {
            document.getElementById("aspnetForm").reset();
        }
        function copyurl(obj) {
         
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
          
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>出售中的商品</h2>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li class="active"><a href="#home"><asp:Literal ID="LitOnSale" runat="server"></asp:Literal></a></li>
                    <li><a href="ProductOnStock.aspx"><asp:Literal ID="LitOnStock" runat="server"></asp:Literal></a></li>
                    <li><a href="ProductZero.aspx"><asp:Literal ID="LitZero" runat="server"></asp:Literal></a></li>
                </ul>
                <div class="page-box">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">

                    <div class="set-switch">
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">商品名称：</label>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize inputw150" placeholder="" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop2">商品分类：</label>
                                <Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control resetSize inputw150" runat="server" NullToDisplay="请选择商品分类"
                                    Width="150" />
                            </div>
                            <div class="form-group">
                                <label for="sellshop3">品牌：</label>
                                <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" CssClass="form-control resetSize inputw150" NullToDisplay="请选择品牌"
                                    Width="150" />
                            </div>
                        </div>
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop4">商家编码：</label>
                                <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize inputw150" placeholder="" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop5">创建时间：</label>
                                <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                            </div>
                            <div class="form-group">
                                <label for="sellshop6">&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                            </div>
                        </div>
                        <div class="reset-search">
                            <a class="bl mb5" onclick="resetform();" style="cursor: pointer">清除条件</a>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn resetSize btn-primary" />
                        </div>
                    </div>


<%--                    <div class="select-page clearfix" style="margin-top: 20px;">
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
                                        <th width="15%">创建时间</th>
                                        <th width="7%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="8">
                                            <div class="mb10 table-operation">
                                                <input type="checkbox" id="sells1" class="allselect">
                                                <label for="sells1">全选</label>
                                                <button type="button" class="btn resetSize btn-primary" onclick="SelectOperation('2');">下架</button>
                                                
                                                <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="btn resetSize btn-danger" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后商品将进入回收站，分销商的相关商品也将被删除！</p>', this);" />
                                                &nbsp;︱
                                                            <button type="button" class="btn resetSize btn-primary" onclick="SelectOperation('5');">设置包邮</button>
                                                <button type="button" class="btn resetSize btn-primary" onclick="SelectOperation('16');">设置运费模板</button>
                                                &nbsp;︱&nbsp;
                                                         <select id="dropBatchOperation" class="form-control resetSize autow inl inputw150">
                                                             <option value="">更多操作..</option>
                                                             <%--    <option value="1">商品上架</option>--%>
                                                             <%-- <option value="2">商品下架</option>--%>
                                                             <%--  <option value="3">商品入库</option>--%>
                                                             <%--  <option value="5">设置包邮</option>--%>
                                                             <%--  <option value="6">取消包邮</option>--%>
                                                             <option value="10">调整基本信息</option>
                                                             <option value="11">调整显示销售数量</option>
                                                             <option value="12">调整库存</option>
                                                             <option value="13">调整会员价</option>
                                                             <%-- <option value="15">调整商品关联标签</option>--%>
                                                         </select>

                                                <asp:HyperLink Target="_blank" Visible="false" runat="server" ID="btnDownTaobao" Text="下载淘宝商品" />
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
                                                        <p class="mb5"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductId") %>" target="_blank" style="width: auto; display: inline;" title="<%#Server.HtmlEncode(Eval("ProductName").ToString()) %>"><%#Hidistro.Core.Globals.SubStr(Eval("ProductName").ToString(),68,"...") %></a></p>
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
                                                <td width="15%"><%#Eval("AddedDate") %></td>
                                                <td width="7%">
                                                    <p>
                                                        <a href="<%#"ProductEdit.aspx?productId="+Eval("ProductId")%>&reurl=<%=LocalUrl %>">编辑</a>  &nbsp;| &nbsp;<a href="<%#"ProductEdit.aspx?productId="+Eval("ProductId")%>&isnext=1&reurl=<%=LocalUrl %>">详情</a>
                                                            
                                                    </p>
                                                    <p>
                                                        <asp:Button ID="btnUnSaleProduct" CommandName="UnSaleProduct" CommandArgument='<%# Eval("ProductId") %>' runat="server" CssClass="btnLink pad" Text="下架" OnClientClick="return HiConform('<strong>确定要下架所选商品吗？</strong><p>下架后商品将不在前台显示！</p>',this);" />&nbsp;︱ 
                                                        <asp:Button ID="btnDel" CommandName="Delete" CommandArgument='<%# Eval("ProductId") %>' runat="server" CssClass="btnLink pad" Text="删除" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后商品将进入回收站，分销商的相关商品也将被删除！</p>',this);" />
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
                    <%-- 上架商品--%>
                    <div id="divOnSaleProduct" style="display: none;">
                        <div class="frame-content">
                            <p>
                                <em>确定要上架商品？上架后商品将前台出售</em>
                            </p>
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
                    <%-- 下架商品--%>
                    <div class="modal fade" id="divUnSaleProduct">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">下架商品</h4>
                                </div>
                                <div class="modal-body">
                                    <p>
                                        确定要下架所选商品吗？下架后商品将不在前台显示！
                                    </p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                    <asp:Button ID="btnUnSale" runat="server" Text="下架商品" CssClass="btn btn-primary" />
                                </div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>

                    <%-- 入库商品--%>
                    <div id="divInStockProduct" style="display: none;">
                        <div class="frame-content">
                        </div>
                    </div>
                    <%-- 设置包邮--%>

                    <div class="modal fade" id="divSetFreeShip">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">设置包邮</h4>
                                </div>
                                <div class="modal-body">
                                    <p>
                                        确定要设置这些商品包邮？ 
                                    </p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                    <asp:Button ID="btnSetFreeShip" runat="server" Text="设置包邮" CssClass="btn btn-primary" />
                                </div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <%-- 设置运费模板--%>

                    <div class="modal fade" id="divSetFreightTemplate">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">设置运费模板</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-inline">
                                        <div class="form-group mr20">
                                            <label for="sellshop4">选择运费模板：</label>
                                            <Hi:FreightTemplateDownList ID="FreightTemplateDownList1" CssClass="form-control" runat="server" NullToDisplay="--请选择运费模板--" />
                                        </div>

                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                    <asp:Button ID="BtnTemplate" runat="server" Text="确定" CssClass="btn btn-primary" />
                                </div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <%-- 取消包邮--%>
                    <div id="divCancelFreeShip" style="display: none;">
                        <div class="frame-content">
                            <p>
                                <em>确定要取消这些商品的包邮？</em>
                            </p>
                        </div>
                    </div>
                    <%-- 商品标签--%>
                    <div id="divTagsProduct" style="display: none;">
                        <div class="frame-content">
                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                        </div>
                    </div>
                    <%--调整信息--%>
                    <div class="modal fade" id="divEditBaseInfo">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title" id="modaltitle">调整信息</h4>
                                </div>
                                <div class="modal-body">
                                    <iframe id="MyEidtBaseInfoIframe" width="780" height="400"></iframe>
                                </div>
                                <div class="modal-footer">
                                </div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                </div>
                <div class="tab-pane"></div>
                <div class="tab-pane"></div>

            </div>
        </div>

        <div style="display: none">
            <asp:Button ID="btnUpdateProductTags" runat="server" Text="调整商品标签" CssClass="submit_DAqueding" />
            <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
            <asp:Button ID="btnInStock" runat="server" Text="入库商品" CssClass="submit_DAqueding" />

            <asp:Button ID="btnUpSale" runat="server" Text="上架商品" CssClass="submit_DAqueding" />

            <asp:Button ID="btnCancelFreeShip" runat="server" Text="取消包邮" CssClass="submit_DAqueding" />
        </div>
    </form>
</asp:Content>

