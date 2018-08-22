<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="SuperiorSelect.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.SuperiorSelect" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .y3-prevshopname input[type=submit] {        
        line-height:17px;
        }
    </style>
    <script type="text/javascript">
        function setsuper(obj,uid,name){
            if(HiConform('<strong>确定要修改当前分销商的上级为【'+name+'】吗？</strong><p>修改后，可能无法还原或者会影响当前的统计数据！</p>',obj)){
                var data="posttype=update&userid=<%=userid%>&touserid="+uid+"&t="+(new Date().getTime())
                $.ajax({
                    url: "superiorselect.aspx",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            HiTipsShow("上级修改成功！","success",function(){location.reload()});
                        }else{
                           parent.ShowMsg(json.tips,false);
                        }
                    }
                })
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div class="y3-prevshopname">
                        <span>店铺名称：【<%=htmlStoreName %>】</span>
                        <span>当前所属上级：【<%=htmlSuperName %>】</span>
                    </div>
                    <div class="y3-prevshopname"><div class="fr" style="margin-right:50px;">
                        店铺名 <asp:TextBox ID="txtKey" runat="server" placeholder="输入店铺名/联系人" MaxLength="50" CssClass="inputW150 mr10"></asp:TextBox> 
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" Text="查询" OnClick="btnSearch_Click" />
                    </div></div>
                        <asp:Repeater ID="rptList" runat="server">
                            <HeaderTemplate>
                    <table class="table y3-modaltable" style="margin-bottom:0;width:100%">
                        <thead>
                            <tr style="background:none;">
                                <th>店铺名</th>
                                <th>联系人</th>
                                <th>联系方式</th>
                                <th>分销商等级</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr>
                                <td title="<%#Eval("StoreName") %>"><%# Hidistro.Core.Globals.SubStr(Eval("StoreName").ToString(),30,"...")%></td>
                                <td><%#Hidistro.Core.Globals.SubStr(Eval("RealName").ToString(),10,"")%></td>
                                <td><%# Eval("CellPhone")%></td>
                                <td><%#Eval("Name") %></td>
                                <td>
                                    <%#FormatOperBtn(Eval("userid"),Eval("StoreName")) %>
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
