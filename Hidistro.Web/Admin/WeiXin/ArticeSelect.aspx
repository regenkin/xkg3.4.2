<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticeSelect.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ArticeSelect" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/htmlc; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/js/jquery.nicescroll.min.js"></script>
    <link rel="stylesheet" href="../css/common.css" />
    <script src="/admin/js/Framenew.js"></script>
    <script>
        $(function () {

            $('#mytabl > ul li').click(function () {
                $('#mytabl > ul li').removeClass('active');
                $(this).addClass('active');
                $(this).parent().next().children().removeClass('active');
                $(this).parent().next().children().eq($(this).index()).addClass('active');
            })
        })

        function gotovalue(ArticleId) {
            window.parent.closeModal("MyPictureIframe", ArticleId);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="mytabl">
                <div class="tab-content">
                    <div class="tab-pane active">
                        <div class="form-inline mb5">
                            <div class="form-group">
                                <label for="exampleInputName2">图文名称</label>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control" placeholder="输入图文名称" Width="200" />
                                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                        <asp:Repeater ID="rptList" runat="server">
                            <HeaderTemplate>
                                <table class="table table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>标题</th>
                                            <th style="width:80px">图文类型</th>
                                            <th style="width:150px">创建时间</th>
                                            <th style="width:85px">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <span class="Name"><a href='/vshop/ArticleShow.aspx?id=<%#Eval("ArticleId")%>'
                                                target="_blank">
                                                <%# Eval("Title") %></a></span> <span class="colorC" style="display: block"></span>
                                        </div>
                                    </td>
                                    <td><%# Eval("ArticleType").ToString()=="2"?"单图文":"多图文" %></td>
                                    <td><%# Eval("PubTime") %></td>
                                    <td>
                                        <button type="button" class="btn btn-default" value='<%#Eval("Url") %>' onclick="gotovalue('<%#Eval("ArticleId") %>')">选取</button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
	</table>
                            </FooterTemplate>

                        </asp:Repeater>

                        <div class="page">
                            <div class="bottomPageNumber clearfix">
                                <div class="pageNumber">
                                    <div class="pagination">
                                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="3" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

        </div>

    </form>
</body>
</html>
