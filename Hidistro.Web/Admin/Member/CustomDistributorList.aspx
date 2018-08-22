<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CustomDistributorList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.CustomDistributorList" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>会员分组设置</h2>
    </div>
    <div id="mytabl">
        <div class="table-page">
            <ul class="nav nav-tabs">
                <li>
                    <a href="UserGroupSet.aspx"><span>自动分组</span></a></li>
                <li class="active">
                    <a href="CustomDistributorList.aspx"><span>手动分组</span></a></li>
            </ul>
        </div>
    </div>
    <form runat="server"><a class="btn btn-success resetSize" href="#" onclick="ShowGroups('add',null,this)">添加分组</a>
        <div class="datalist mt5">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>分组名称</th>
                                <th>会员人数</th>
                                <th>新会员</th>
                                <th>活跃会员</th>
                                <th>沉睡会员</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("GroupName") %></td>
                        <td><%# Eval("UserCount") %></td>
                        <asp:Literal ID="ltMemberNumList" runat="server"></asp:Literal>
                        <td>
                            <span class="submit_shanchu">
                                <a href="javascript:void(0)" onclick="ShowGroups('update',<%# Eval("Id") %>,this)">编辑</a> 
                                <a href='<%# Hidistro.Core.Globals.GetAdminAbsolutePath(string.Format("/member/CustomDistributorDetail.aspx?GroupId={0}", Eval("id")))%>'>管理会员</a>
                                <asp:Button ID="btnDel" runat="server" CommandName="delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return HiConform('<strong>确定要删除当前分组吗？</strong><p>删除后该分组下面的用户记录都将删除！</p>',this);" Text="删除" CssClass="btnLink" />
                            </span>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
	</table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        
        <%--修改分组名--%>
        <div class="modal fade" id="updategroup_div">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">编辑分组</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label"><em>*</em>分组名称：</label><asp:TextBox ID="txtgroupname" CssClass="form-control inputw200" runat="server" MaxLength="20" onkeyup="$(this).val(getStrbylen($(this).val(),8))" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="hidden" id="hdgroupId" runat="server" />
                        <asp:Button ID="btnupdategroup" runat="server" Text="修改" CssClass="btn btn-success" OnClientClick="return checkform('ctl00_ContentPlaceHolder1_txtgroupname')" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>
        
        <%--添加分组名--%>
        <div class="modal fade" id="addgroup_div">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">添加分组</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label"><em>*</em>分组名称：</label><asp:TextBox ID="txtaddgroupname" CssClass="form-control inputw200" runat="server" MaxLength="20" onkeyup="$(this).val(getStrbylen($(this).val(),16))" />
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnaddgroup" runat="server" Text="添加" CssClass="btn btn-success"  OnClientClick="return checkform('ctl00_ContentPlaceHolder1_txtaddgroupname')"/>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>
        </form>
    <script type="text/javascript">
        var formtype = "";
        function ShowGroups(oper, groupId, link_obj) {
            arrytext = null;
            if (oper == "add") {
                formtype = "add";
                $("#ctl00_ContentPlaceHolder1_txtaddgroupname").val("");

                $('#addgroup_div').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
            } else {
                formtype = "edite";
                var groupName = $(link_obj).parents("tr").find("td").eq(0).text();
                $("#ctl00_ContentPlaceHolder1_hdgroupId").val(groupId);
                $("#ctl00_ContentPlaceHolder1_txtgroupname").val(groupName);
                $('#updategroup_div').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
            }
        }
        function checkform(objId) {
            var info = $.trim($('#'+objId).val());
            if (info == "") {
                ShowMsg("请输入分组名称",false);
                return false;
            }
        }        
    </script>
</asp:Content>
