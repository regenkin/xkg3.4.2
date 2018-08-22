<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Goods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Goods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="icon" href="../../images/hi.ico" />
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/js/jquery.nicescroll.min.js"></script>
    <link rel="stylesheet" href="../css/common.css" />
    <script>
        $(function () {

            $('#mytabl > ul li').click(function () {
                $('#mytabl > ul li').removeClass('active');
                $(this).addClass('active');
                $(this).parent().next().children().removeClass('active');
                $(this).parent().next().children().eq($(this).index()).addClass('active');
            })
        })

        function gotovalue(obj, title, ShortDescription, ThumbnailUrl40) {
            window.parent.closeModal("myIframeModal", "txtContent", obj, title, ShortDescription, ThumbnailUrl40);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="mytabl">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">
                    <li class="active"><a href="#home">商品</a></li>
                    <li><a href="#profile">商品分类</a></li>

                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div class="tab-pane active">
                        <div class="form-inline">
                            <div class="form-group">
                                <label for="exampleInputName2">商品名称</label>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control" placeholder="输入商品名称" Width="200" />
                                  <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn btn-primary btn-sm" />
                            </div>

                        </div>
                       
                        <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                            GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="table_title" CssClass="table table-bordered table-hover">
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                            <Columns>

                                <asp:TemplateField ItemStyle-Width="60%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>

                                        <div style="float: left;">
                                            <span class="Name"><a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                                target="_blank">
                                                <%# Eval("ProductName") %></a></span> <span class="colorC" style="display: block"></span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="30%" HeaderText="创建时间" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                        <%# Eval("AddedDate") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="操作" ItemStyle-Width="10%" HeaderStyle-Width="95" HeaderStyle-CssClass=" td_left td_right_fff">
                                    <ItemTemplate>
                                        <button type="button" class="btn btn-default" value='<%="http://"+ Globals.DomainName+"/ProductDetails.aspx?productId=" %><%#Eval("ProductId") %>' onclick="gotovalue(this.value,'<%#Server.HtmlEncode(Eval("ProductName").ToString()) %>','<%#Server.HtmlEncode(Eval("ShortDescription").ToString()) %>','<%#Server.HtmlEncode(Eval("ThumbnailUrl40").ToString()) %>');">选取</button>

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
                    <div class="tab-pane">

                        <UI:Grid ID="grdTopCategries" DataKeyNames="CategoryId" runat="server" ShowHeader="true"
                            AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-hover"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="分类名称" HeaderStyle-Width="80%" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                        <a target="_blank" href='<%# "/ProductList.aspx?CategoryId="+Eval("CategoryId")%>'>
                                            <%#Eval("Name") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                                    <ItemTemplate>
                                          <button type="button" class="btn btn-default" value='<%= Request.Url.Host+"/ProductList.aspx?categoryId=" %><%#Eval("CategoryId") %>' onclick="gotovalue(this.value,'<%#Server.HtmlEncode(Eval("Name").ToString()) %>');">选取</button>

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </UI:Grid>
                    </div>

                </div>
            </div>

        </div>

    </form>
</body>
</html>
