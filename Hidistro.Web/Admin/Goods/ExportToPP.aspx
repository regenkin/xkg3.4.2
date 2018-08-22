<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ExportToPP.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ExportToPP" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table_title {
            background: #f2f2f2;
        }

        table th {
            text-align: center;
        }

        input[type="checkbox"], input[type="radio"] {
            margin: 0;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#ctl00_contentHolder_radCategory").bind("click", function () { $("#liCategory").show(); $("#liLine").hide(); });
            $("#ctl00_contentHolder_radLine").bind("click", function () { $("#liLine").show(); $("#liCategory").hide(); });
            $(".start_datetime1").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm-dd',
                autoclose: true,
                weekStart: 1,
                minView: 2
            });
        });
        function checkSearch() {
            var r = $("#ctl00_ContentPlaceHolder1_chkOnSales").is(":checked") || $("#ctl00_ContentPlaceHolder1_chkInStock").is(":checked");
            if (!r) {
                ShowMsg("商品类别至少选择一项", false);
                return false
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>批量导出商品</h2>
            <small></small>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li><a href="ExportToTB.aspx">导出为淘宝数据包</a></li>
                <li class="active"><a href="exporttopp.aspx">导出为拍拍数据包</a></li>

            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">

                    <div class="form-inline">
                        <div class="form-group">
                            <label for="exampleInputName2">商品名称</label>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize" placeholder="" />

                        </div>
                        <div class="form-group">
                            <label for="exampleInputEmail2">商品分类</label>
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control resetSize" runat="server" NullToDisplay="--请选择商品分类--"
                                Width="180" />
                        </div>
                        <div class="form-group">
                            <label>商家编码</label>

                            <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize" />

                        </div>

                        <div style="margin-top: 10px;">

                            <div class="form-group">
                                <label for="exampleInputName2">创建时间</label>

                                <uc1:ucDateTimePicker runat="server" ID="calendarStartDate" CssClass="form-control resetSize input-ssm start_datetime1"
                                    PlaceHolder="开始时间" />
                                &nbsp;&nbsp;至&nbsp;
                                   <uc1:ucDateTimePicker runat="server" ID="calendarEndDate" CssClass="form-control resetSize input-ssm start_datetime1"
                                       PlaceHolder="结束时间" />

                            </div>

                            <div class="form-group" style="display: none">
                                <label for="">淘宝状态</label>

                                <asp:DropDownList ID="dpTaoBao" runat="server" Width="150">
                                    <asp:ListItem Value="-1" Selected="True">-请选择淘宝状态-</asp:ListItem>
                                    <asp:ListItem Value="1">已制作</asp:ListItem>
                                    <asp:ListItem Value="0">未制作</asp:ListItem>
                                </asp:DropDownList>


                            </div>
                            <div class="form-group">
                                <label for="">包含(至少选择一项)</label>
                                <div class="checkbox rsertRadio">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkOnSales" Text="出售中的商品" Checked="true" />
                                    </label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkUnSales" Text="已下架的商品" Visible="false" />

                                    </label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkInStock" Text="仓库中的商品" />&nbsp;&nbsp;&nbsp;
                                    </label>
                                </div>


                            </div>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn resetSize btn-primary"  OnClientClick="return checkSearch()" />
                        </div>

                    </div>


                    <div style="margin-top: 10px;">
                        <div class="set-switch">
                            <div class="form-inline">
                                <div class="form-group">
                                    <label for="exampleInputName2">导出数量</label>
                                    <asp:Label runat="server" ID="lblTotals"></asp:Label>件&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="form-group">
                                    <label>导出版本</label>
                                    <asp:DropDownList runat="server" ID="dropExportVersions" CssClass="form-control resetSize"></asp:DropDownList>&nbsp;&nbsp;
                                </div>
                                <div class="form-group">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkExportStock" />&nbsp;导出库存数量&nbsp;&nbsp;</label>
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputName2"></label>
                                    <asp:Button runat="server" ID="btnExport" Text="导 出" CssClass="btn resetSize btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="dataarea mainwidth">
                        <!--搜索-->


                        <!--数据列表区域-->
                        <div class="datalist">
                            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId" CssClass="table table-bordered table-hover"
                                SortOrder="Desc" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                <Columns>
                                    <asp:TemplateField HeaderText="商品">
                                        <ItemTemplate>
                                            <div style="float: left; margin-right: 10px;">
                                                <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                                </a>
                                            </div>
                                            <div style="float: left;">
                                                <span class="Name"><a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                                <span class="colorC" style="display: block">商家编码：<%# Eval("ProductCode") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="库存" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <Hi:MoneyColumnForAdmin HeaderText="原价" ItemStyle-Width="80" DataField="MarketPrice" />

                                    <Hi:MoneyColumnForAdmin HeaderText="现价" ItemStyle-Width="80" DataField="SalePrice" />
                                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="80" HeaderStyle-CssClass=" td_left td_right_fff">
                                        <ItemTemplate>
                                            <span class="submit_shanchu">
                                                <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-primary" CommandName="Remove" Text="不导出" /></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </UI:Grid>
                            <div class="blank12 clearfix"></div>
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

                </div>
                <div class="tab-pane">
                </div>
            </div>
        </div>


    </form>
</asp:Content>
