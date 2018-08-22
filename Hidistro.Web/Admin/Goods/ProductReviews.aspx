<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductReviews" MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_dlstPtReviews th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:left; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_dlstPtReviews td {margin:0px;border-left:0px;border-right:0px;vertical-align:top;}
        .username {margin-left:10px;}
         td{word-break: break-all;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>客户评论</h2>
            <%--<small>管理店铺的所有商品咨询，您可以查询或删除商品咨询</small>--%>
        </div>
        <div id="mytabl">
            <div class="page-box">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
            <div class="table-page">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">

                </ul>
                
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                    <div class="form-inline">
                        <div class="form-group mr20">
                            <label for="">商品名称</label>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize" placeholder="" />
                        </div>
                        <div class="form-group mr20">
                            <label for="">商品分类</label>
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="form-control resetSize"  />
                        </div>
                        <div class="form-group mr20">
                            <label for="">商家编码</label>
                            <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize" placeholder="" />                            
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" class="btn resetSize btn-primary" />
                        </div>
                    </div>
                    </div>
<%--                    <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>--%>
                    <UI:Grid ID="dlstPtReviews" runat="server" ShowHeader="true" CssClass="table mar table-bordered" AutoGenerateColumns="false" DataKeyNames="ReviewId" HeaderStyle-CssClass="table_title"   GridLines="None" Width="100%">
                        <Columns>
                            <asp:TemplateField SortExpression="ProductName" ItemStyle-Width="32%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                                <ItemStyle CssClass="Name" />
                                <ItemTemplate>                                
                                    <div style="float: left;"><div class="img fl mr10">
                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60"  Width="60" Height="60"/></div>
                                        <a href='<%#string.Format("/ProductDetails.aspx?productId={0}",Eval("productId"))%>' target="_blank">
                                            
                                            <asp:Literal ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' />
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="客户评论" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ReviewText") %>' CssClass="line"></asp:Label>
                                    <br />
                                    <span style="color: #999;">
                                    <Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ReviewDate") %>' runat="server"></Hi:FormatedTimeLabel>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName").ToString() %>' CssClass="username"></asp:Label>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="操作" ItemStyle-Width="10%" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemTemplate>
                                    <span class="submit_shanchu">
                                        <asp:Button ID="btnDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ReviewId")%>' Text="删除" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>确定要执行该删除操作吗？</strong><p>删除后将不可以恢复！</p>',this);" />
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </UI:Grid>
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
        </div>

    </form>	
</asp:Content>