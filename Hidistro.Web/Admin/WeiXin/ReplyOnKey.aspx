<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ReplyOnKey.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ReplyOnKey" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>自动回复</h2>
    </div>
    <div class="form-inline mb10">
        <div class="form-group">
            <a class="btn btn-success resetSize" href="replyedit.aspx?type=subscribe">首次关注</a>
        </div>
        <div class="form-group">
            <a class="btn btn-success resetSize" href="replyedit.aspx">添加回复规则</a>
        </div>
        <div class="form-group">
        <small>"无匹配回复"仅在未开启多客服设置才生效</small></div>
    </div>
    <div class="datalist">
        <form runat="server">
            
                <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" >
                    <HeaderTemplate>
                        <table class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th style="text-align:center;width:360px;">自动回复内容</th>
                                    <th style="width:100px; text-align:center;">回复类型</th>
                                    <th style="width:100px; text-align:center;">匹配类型</th>
                                    <th style="text-align:center;">关键词</th>
                                    <th style="width:159px; text-align:center;">最后修改时间</th>
                                    <th style="width:100px; text-align:center;">最后修改人</th>
                                    <th style="width:100px; text-align:center;">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#GetTitleShow( Eval("MessageTypeName"),Eval("ArticleID"),Eval("Id")) %> </td>
                            <td><%# GetReplyTypeName(Eval("ReplyType")) %></td>
                            <td style="text-align:center;"><%# Eval("MatchType").ToString()=="Like"?"模糊匹配":"精确匹配" %></td>
                            <td><%# Eval("Keys") %></td>
                            <td style="text-align:center;"><%# Eval("LastEditDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                            <td style="text-align:center;"><%# Eval("LastEditor") %></td>
                            <td style="text-align:center;"><span class="submit_bianji">
                                <a href="replyedit.aspx?<%#(((int)Eval("ReplyType")).ToString()==((int)Hidistro.Entities.VShop.ReplyType.Subscribe).ToString())?"type=subscribe&":"" %>id=<%#Eval("Id") %>" class="">编辑</a>
                            </span>
                            <span class="submit_shanchu">
                                <%--<Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true" CommandName="Delete" runat="server"  CommandArgument='<%#Eval("Id") %>'/>--%>
                                  <asp:Button ID="lkDelete" CssClass="btnLink" CommandArgument='<%#Eval("Id") %>'  IsShow="true" runat="server" Text="删除" CommandName="Delete"  OnClientClick="return HiConform('<strong>确定要删除选择的自动回复吗？</strong><p>删除自动回复不可恢复！</p>',this)" />
                            </span></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
	</table>
                    </FooterTemplate>

                </asp:Repeater>



            <div class="blank5 clearfix">
            </div>
        </form>
    </div>
    <script type="text/javascript" src="../weixin/GetImagesMsgId.aspx"></script>
</asp:Content>
