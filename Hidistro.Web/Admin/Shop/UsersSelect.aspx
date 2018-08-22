<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="UsersSelect.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.UsersSelect" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .y3-prevshopname input[type=submit] {        
        line-height:17px;
        }
    </style>
    <script type="text/javascript">
        function seluser(obj) {
            var issel = $(obj).attr("issel");
            var userid=$(obj).attr("userid");
            var data = "posttype=sel&issel=" + issel + "&userid=" + userid+"&t="+(new Date()).getTime();
            $.ajax({
                url: "usersselect.aspx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.success == "1") {
                        if (issel == 1) {
                            $(obj).attr("issel", "0").attr("value", "选择").attr("class", "btn btn-primary btn-xs");
                        } else {
                            $(obj).attr("issel", "1").attr("value", "已选").attr("class", "btn btn-success btn-xs");
                        }
                    }
                }
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
                    <div class="y3-prevshopname"><div class="fr" style="margin-right:50px;">
                        用户名 <asp:TextBox ID="txtKey" runat="server" placeholder="输入用户名" MaxLength="50" CssClass="inputW150 mr10"></asp:TextBox> 
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" Text="查询" OnClick="btnSearch_Click" />
                        &nbsp;&nbsp;<input type="button" value="确定选择" class="btn btn-success btn-sm" style="line-height:16px" onclick="parent.$('#divmyIframeModal').modal('hide')" />
                    </div></div>
                        <asp:Repeater ID="rptList" runat="server">
                            <HeaderTemplate>
                    <table class="table y3-modaltable" style="margin-bottom:0;width:100%">
                        <thead>
                            <tr style="background:none;">
                                <th>昵称</th>
                                <th>手机</th>
                                <th>用户名</th>
                                <th>上级</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr>
                                <td><%# Eval("UserName")%></td>
                                <td><%# Eval("CellPhone")%></td>
                                <td><%# Eval("UserBindName")%></td>
                                <td><%# Eval("StoreName").ToString()==""?"主店": Eval("StoreName")%></td>
                                <td>
                                    <%#FormatOper(Eval("UserID"),adminname) %>
                                </td>
                            </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                        </tbody>
                    </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="divEmpty" runat="server" CssClass="alignc" Visible="false">无相关数据</asp:Panel>
                        <div class="page" style="border-top:1px solid #DDD;">
                            <div class="bottomPageNumber clearfix">
                                <div class="pageNumber">
                                    <div class="pagination">
                                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="5" />
                                    </div>
                                </div>
                            </div>
                        </div>
        </form>
</asp:Content>
