<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="NoticeList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.NoticeList" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>消息公告管理</h2>
    </div>
    <div id="mytabl">
        <div class="table-page">
            <ul class="nav nav-tabs">
                <li <%if (sendType == 0)
                      { %> class="active" <%} %>>
                    <a href="NoticeList.aspx"><span>公告管理</span></a></li>
                <li <%if (sendType == 1)
                      { %> class="active" <%} %>>
                    <a href="NoticeList.aspx?type=1"><span>消息管理</span></a></li>
            </ul>
        </div>
    </div>


    <form runat="server">
        <div class="tab-content">
            <div class="tab-pane active">
                <div class="set-switch">
                    <div class="form-inline">
                        <div class="form-group mr20">
                            <label for="ctl00_ContentPlaceHolder1_txtTitle">公告标题：</label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control resetSize inputw150"></asp:TextBox>
                            <label for="ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker">发布时间：</label>
                            <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw100" />
                            至
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw100" />
                            <label for="ctl00_ContentPlaceHolder1_txtUserName">&nbsp;创建用户：</label>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                            <span  <%if (sendType == 1)
                      { %> style="display:none;"<%} %>>
                            <label for="ctl00_ContentPlaceHolder1_ddlState">状态：</label>
                            <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control resetSize">
                                <asp:ListItem Text="所有公告" Value=""></asp:ListItem>
                                <asp:ListItem Text="待发布" Value="0"></asp:ListItem>
                                <asp:ListItem Text="已发布" Value="1"></asp:ListItem>
                            </asp:DropDownList></span>&nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn resetSize btn-primary" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="checkbox">
            <label>
                <input id="selAll" class="allselect" type="checkbox">全选</label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDel" runat="server" CssClass="btn btn-danger resetSize" Text="批量删除" OnClick="btnDel_Click"/>&nbsp;&nbsp;
            <asp:Button ID="btnPub" runat="server" CssClass="btn btn-primary resetSize" Text="批量发布" Visible="false"/>&nbsp;&nbsp;
            <asp:HyperLink ID="hlinkAdd" runat="server" CssClass="btn btn-success resetSize"></asp:HyperLink>
        </div>

        <div class="datalist mt5">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th> </th>
                                <th style="width: 325px;">标题</th>
                                <th>创建用户</th>
                                <th>创建时间</th>
                                <th>发送对象</th>
                                <th>发送状态</th>
                                <th>最后发布时间</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><input name="cbNoticeGroup" type="checkbox" value='<%#Eval("ID") %>' /></td>
                        <td><%# Eval("Title") %></td>
                        <td><%# Eval("Author") %></td>
                        <td><%# Eval("AddTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td><%# FormatSendTo(Eval("SendTo"),Eval("ID")) %></td>
                        <td><%# FormatIsPub(Eval("IsPub")) %></td>
                        <td><%# Eval("PubTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td>
                            <span class="submit_shanchu">
                               <input type="button" value="预览" class="btnLink" onclick="NoticeView('<%#Eval("ID") %>')" />
                                <asp:Button ID="btnPub" runat="server" Text="发布" CssClass="btnLink" CommandName="pub" CommandArgument='<%#Eval("Id") %>' OnClientClick="return HiConform('<strong>确定要发布选择的内容吗？</strong><p>发布后内容将不可修改！</p>',this)" />
                                <asp:HyperLink ID="hpLinkEdit" runat="server" Text="修改" Visible="false"></asp:HyperLink>
                                <asp:Button ID="btnDel" runat="server" CommandName="delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return HiConform('<strong>确定要删除当前内容吗？</strong><p>删除后将不可恢复,并且用户也看不到该内容！</p>',this);" Text="删除" CssClass="btnLink" />
                            </span>
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
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </form>
    <script type="text/javascript">
        function checkform(objId) {
            var info = $.trim($('#' + objId).val());
            if (info == "") {
                ShowMsg("请输入标签名称", false);
                return false;
            }
        }
        $('#selAll').click(function () {
            if ($(this).prop("checked")) {
                $('.datalist input[type="checkbox"]').prop('checked', true);
            } else {
                $('.datalist input[type="checkbox"]').prop('checked', false);
            }
        });
        function NoticeView(id) {
            var content = '<iframe src="/vshop/NoticeDetail.aspx?type=view&Id=' + id + '" id="ifmMobile" width="100%" scrolling="no" frameborder="0"></iframe>';
            MobileContentShow('<%=(sendType==1?"消息发送":"公告") %>展示页', content);
        }
    </script>
</asp:Content>
