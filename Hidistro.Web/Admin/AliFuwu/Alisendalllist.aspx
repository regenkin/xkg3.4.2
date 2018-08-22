<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Alisendalllist.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliFuwu.Alisendalllist" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="page-header">
        <h2>服务窗群发记录</h2>
    </div>
    <div class="form-inline mb10">
        <div class="form-group">
            <a class="btn btn-success resetSize" href="sendalledit.aspx">服务窗群发</a>
        </div>
    </div>

      <div class="datalist">
        <form runat="server">            
                <asp:Repeater ID="rptList" runat="server" >
                    <HeaderTemplate>
                        <table class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th style="text-align:center;width:360px;">群发标题</th>
                                    <th style="width:100px; text-align:center;">发送粉丝数</th>
                                    <th style="width:159px; text-align:center;">时间</th>
                                    <th style="width:100px; text-align:center;">状态</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("Title") %> </td>
                            <td><%#Eval("SendCount") %></td>
                            <td style="text-align:center;"><%# Eval("SendTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                            <td style="text-align:center;" title="<%#Server.HtmlEncode(Eval("ReturnJsonData").ToString()) %>"><%# Eval("SendState").ToString()=="1"?"成功":"" %><%# Eval("SendState").ToString()=="2"?"<span style='color:red'>失败</span>":"" %><%# Eval("SendState").ToString()=="0"?"待发送":"" %></td>
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
                                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                                    </div>
                                </div>
                            </div>
                        </div>
            <div class="blank5 clearfix">
            </div>
        </form>
    </div>
</asp:Content>
