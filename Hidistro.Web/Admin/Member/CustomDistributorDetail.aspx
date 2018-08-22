<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CustomDistributorDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.CustomDistributorDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass {
            text-align: center;
        }

        .labelClass {
            margin-right: 10px;
        }

        .thCss {
            text-align: center;
        }

        .selectthis {
            border: 1px solid;
            border-color: #999999;
            color: #c93027;
            margin-right: 2px;
        }

            .selectthis:hover {
                border: 1px solid;
                border-color: #999999;
                color: #c93027;
                margin-right: 2px;
            }

        .aClass {
            border: 1px solid;
            border-color: #999999;
            color: #999999;
            margin-right: 2px;
        }

            .aClass:hover {
                border: 1px solid;
                border-color: #999999;
                color: #999999;
                margin-right: 2px;
            }

        #datalist td {
            word-break: break-all;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: center;
            vertical-align: middle;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            text-align: center;
            vertical-align: middle;
        }

        .table-bordered > thead > tr > th {
            border: none;
        }
    </style>
    <script>
        function IsShowDiv(isShow) {
            if (isShow == "true") {
                $("#setMemberDiv").css('display', 'block');
                $("#addMembersDiv").css('display', 'none');
            } else {
                $("#setMemberDiv").css('display', 'none');
                $("#addMembersDiv").css('display', 'block');
            }
        }

        $(document).ready(function () {
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $("input[type='checkbox']").each(function () {
                    $(this).prop('checked', check);
                });
            });
        });

        function DefualtGroup(clientType) {
            var url = "/admin/Member/CustomDistributorDetail.aspx?GroupId=<%=currentGroupId%>";
            if (clientType != "") {
                url += "&clientType=" + clientType;
            }
            location.href = url;
        }

        function AddMembers() {
            location.href = "/Admin/member/CustomDistributorAddMembers.aspx?GroupId=<%=currentGroupId%>";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div>
            <div class="page-header">
                <h2>分组管理><asp:Literal runat="server" ID="GroupName"></asp:Literal></h2>
            </div>
            <div id="addMembersDiv">
                <a class="btn btn-primary btn-sm" href="#" onclick="AddMembers()">批量添加会员<span class="glyphicon glyphicon-plus" aria-hidden="true"></span></a>
                <a class="btn btn-primary btn-sm" href="#" onclick="IsShowDiv('true')">手动添加会员<span class="glyphicon glyphicon-plus" aria-hidden="true"></span></a>
            </div>
            <div id="setMemberDiv" style="display: none; width: 30%;">
                <Hi:TrimTextBox runat="server" Rows="4" Columns="50" ID="txtUserNames" TextMode="MultiLine" />
                <small>手动添加会员，多个会员时，一行一个用户名</small>
                <asp:Button runat="server" class="btn btn-primary" ID="BtnAddMembers" Text="加入分组" />
                <a class="btn btn-default" href="#" onclick="IsShowDiv('false')">取消</a>
            </div>
            <br />
            <div>
                <ul class="nav nav-tabs">
                    <li dataid="normal" <%if (clientType == "") { %>class="active"<%} %>><a href="#" onclick="DefualtGroup('')">
                        <asp:Literal ID="litAll" Text="所有会员" runat="server"></asp:Literal></a></li>
                    <li dataid="new" <%if (clientType == "new") { %>class="active"<%} %>><a href="#" onclick="DefualtGroup('new')">
                        <asp:Literal ID="litNew" Text="新会员" runat="server"></asp:Literal></a></li>
                    <li dataid="activy" <%if (clientType == "activy")
                                          { %>class="active"<%} %>><a href="#" onclick="DefualtGroup('activy')">
                        <asp:Literal ID="litActivy" Text="活跃会员" runat="server"></asp:Literal></a></li>
                    <li dataid="sleep" <%if (clientType == "sleep")
                                         { %>class="active"<%} %>><a href="#" onclick="DefualtGroup('sleep')">
                        <asp:Literal ID="litSleep" Text="沉睡会员" runat="server"></asp:Literal></a></li>
                </ul>
                <div class="title-table">
                    <div style="margin-bottom: 5px; margin-top: 10px;">
                        <div class="form-inline" id="pagesizeDiv" style="float: left; width: 100%; margin-bottom: 5px;">
                        </div>
                        <div class="page-box">
                            <div class="page fr">
                                <div class="form-group" style="margin-right: 0px; margin-left: 0px; background: #fff;">
                                    <label for="exampleInputName2">每页显示数量：</label>
                                    <UI:PageSize runat="server" ID="hrefPageSize" />
                                </div>
                            </div>
                        </div>
                        <div class="pageNumber" style="float: right; height: 29px; margin-bottom: 5px; display: none;">
                            <label>每页显示数量：</label>
                            <div class="pagination" style="display: none;">
                                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                            </div>
                        </div>
                        <div class="form-inline" style="text-align: left; margin-top: 5px; background: #fff;">
                            <label>
                                <input type="checkbox" id="selectAll" style="margin: 0px 0px 0px 17px" />
                                全选</label>
                            <asp:Button ID="lkbDelectCheck" runat="server" Text="批量移除" class="btn resetSize btn-danger" IsShow="true" OnClientClick="return HiConform('<p>确认要批量移除用户吗？</p>', this);" />
                        </div>
                    </div>
                </div>
                <div id="datalist">
                    <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                        DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                        GridLines="None" Width="100%">
                        <Columns>
                            <UI:CheckBoxColumn CellWidth="50" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="微信头像" ItemStyle-Width="80" SortExpression="UserName" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <img alt="头像" src="<%# Eval("UserHead") %>" style="height: 45px; width: 45px; border-width: 0px;" />
                                    <input type="text" value="<%# Eval("UserId") %>" style="display: none;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="昵称/手机" ItemStyle-Width="112" SortExpression="UserName" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <p>
                                        <asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName").ToString()==""?"未设置":Eval("UserName") %>' />
                                    </p>
                                    <p><%# Eval("CellPhone").ToString()==""?"未绑定":Eval("CellPhone") %></p>
                                    <asp:HiddenField Value='<%# Eval("CellPhone")%>' runat="server" ID="hidCellPhone" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="微信OpenID" ItemStyle-Width="135" ShowHeader="true">
                                <ItemTemplate>
                                    <%# Eval("OpenID").ToString()==""?"未绑定":Eval("OpenID")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="用户名" ShowHeader="true" ItemStyle-Width="110">
                                <ItemTemplate>
                                    <%# Eval("UserBindName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="会员等级" ItemStyle-Width="106" ShowHeader="true">
                                <ItemTemplate>
                                    <asp:Literal ID="lblGradeName" runat="server" Text='<%# Eval("GradeName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="上级" ItemStyle-Width="99" SortExpression="Balance">
                                <ItemTemplate>
                                    <%# Eval("StoreName").ToString()==""?"主店": Eval("StoreName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="订单数/消费额" ItemStyle-HorizontalAlign="Center" ShowHeader="true" ItemStyle-Width="106">
                                <ItemTemplate>
                                    <p>
                                        <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/trade/manageorder.aspx?UserId={0}",Server.UrlEncode(Eval("UserId").ToString()))) %>' style="text-decoration: underline; color: #F60;">
                                            <asp:Label ID="lblOrderNumberBandField" class="order-span" Text='<%# Eval("OrderCount") %>' runat="server"></asp:Label>
                                        </a>
                                    </p>
                                    <p style="color: #F60;">
                                        ￥<%# Eval("OrderTotal").ToString ()==""?"0.00":Convert.ToDouble ( Eval("OrderTotal")).ToString ("f2") %>
                                    </p>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="注册时间" HeaderStyle-HorizontalAlign="Center" SortExpression="GradeName" ItemStyle-Width="124">
                                <ItemTemplate>
                                    <itemtemplate><%# Eval("CreateDate","{0:yyyy-MM-dd<br>HH:mm:ss}")%></itemtemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="border_top border_bottom" HeaderStyle-Width="95">
                                <ItemStyle CssClass="spanD spanN actionBtn" />
                                <ItemTemplate>
                                    <p>
                                        <span class="submit_bianji"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}", Eval("UserId")))%>'>详情</a></span>
                                        <span class="submit_shanchu">
                                        <asp:Button ID="btnDel" CssClass="btnLink" runat="server" Text="删除" CommandName="Delete" CommandArgument='<%#Eval("UserId") %>' OnClientClick="return HiConform('<p>确定要移除选择的用户吗？</p>',this)" /></span>
                                        <input id="hdUserId" type="hidden" value="<%# Eval("UserID") %>" />
                                    </p>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </UI:Grid>
                </div>
            </div>
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" style="width: auto">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
