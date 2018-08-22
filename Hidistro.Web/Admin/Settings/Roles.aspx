<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true"
    CodeBehind="Roles.aspx.cs" Inherits="Hidistro.UI.Web.Admin.settings.Roles" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%--<%@ Import Namespace="Hidistro.Core" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />--%>
    <style>
        .table_title {
            background: #f2f2f2;
        }

        table td, th {
            text-align: center;
        }

        .control-label {
            width: 110px;
        }
        .submitlist {
         width:40px;
         float:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>部门管理</h2>
    </div>
    <button class="btn btn-success" onclick="AddNewRoles()">添加新部门</button>
    <form id="thisForm" runat="server" style="margin-top: 20px">
        <UI:Grid ID="grdGroupList" runat="server" AutoGenerateColumns="false" ShowHeader="true"
            DataKeyNames="RoleId" CssClass="table table-hover mar table-bordered" GridLines="None"
            HeaderStyle-CssClass="table_title">
            <Columns>
                <asp:TemplateField HeaderText="部门名称" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="lblRoleName" Text='<%#Eval("RoleName")%>' runat="server" />
                        <%# Convert.ToBoolean(Eval("IsDefault"))?"<span style=\"color:blue;\">(所有权限)":"" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="职能说明" ItemStyle-Width="50%" ItemStyle-CssClass="tdleft"
                    HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <div class="tdleft">
                            <asp:Literal ID="lblRoleDesc" Text='<%#Eval("Description") %>' runat="server"></asp:Literal>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="220px" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemTemplate>
                        <span class="submitlist submit_shanchu">&nbsp;</span>
                        <span class="submitlist submit_shanchu">
                      <%--      <Hi:ImageLinkButton runat="server" IsShow="true" ID="DeleteImageLinkButton1" CommandName="Delete"
                                Text="删除" />--%>
                             <asp:Button ID="DeleteImageLinkButton1" runat="server" Text="删除"    Class="btnLink pad"  CommandName="Delete"    OnClientClick="return HiConform('<strong>确定要删除选择的部门吗？</strong><p>删除后不可恢复！</p>',this)" ToolTip="" /> 
                        </span>
                        <span class="submitlist submit_bianji"><a href="javascript:ShowEditDiv('<%# Eval("RoleId")%>','<%# Eval("RoleName")%>','<%#  Eval("Description")%>','<%#Convert.ToBoolean(Eval("IsDefault"))?"1":"0" %>');">
                            编辑</a></span>
                        
                        <span class="submitlist submit_shanchu" style="<%# Convert.ToBoolean(Eval("IsDefault"))?"display:none; ": "" %>">
                            <a href="SetRolePermissions.aspx?roleId=<%# Eval("RoleId")%>">权限</a></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>



        <!--编辑部门-->

        <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal" id="hform">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span
                            aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">添加新部门</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="htxtRoleId" runat="server" />
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><span style="color: red">*</span>部门名称：</label>
                            <div class="col-xs-3" style="width: 72%">
                                <asp:TextBox ID="txtAddRoleName" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                                <small class="help-block">部门名称不能为空,长度限制在60个字符以内</small>
                            </div>

                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><span style="color: red">*</span>所有权限：</label>

                            <div class="col-xs-3" style="width: 72%">
                              <label><input id="rdFalse" name="rdIsDefault" type="radio" value="false" checked="true" />否</label>
                                <label><input id="rdTrue" name="rdIsDefault" type="radio" value="true" />是</label>
                               
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">职能说明：</label>
                            <div class="col-xs-3" style="width: 72%">
                                <asp:TextBox ID="txtRoleDesc" runat="server" CssClass="form-control" Height="81px"
                                    TextMode="MultiLine" Width="300px"></asp:TextBox>
                                <small class="help-block">说明部门具备哪些职能，长度限制在100个字符以内</small>
                            </div>

                        </div>


                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmitRoles" runat="server" Text="确 定" CssClass="btn  btn-success" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



    </form>



    <script>

        var formtype = "";
        var vilidsetings = {
            'ctl00$ContentPlaceHolder1$txtAddRoleName': {
                validators: {
                    notEmpty: {
                        message: '部门名称不能为空,长度限制在60个字符以内'
                    },
                    stringLength: {
                        min: 1,
                        max: 60,
                        message: '部门名称不能为空,长度限制在60个字符以内'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtRoleDesc': {
                validators: {
                    stringLength: {
                        min: 0,
                        max: 30,
                        message: '职能说明的长度限制在100个字符以内'
                    }
                }
            }

        };

        $('#hform').formvalidation(vilidsetings);//绑定验证方法

        function AddNewRoles() {

            $("#modaltitle").html("添加新部门");
            $('#ctl00_ContentPlaceHolder1_txtAddRoleName').val("");
            $('#ctl00_ContentPlaceHolder1_txtRoleDesc').val("");
            $('#ctl00_ContentPlaceHolder1_htxtRoleId').val("");

            $("#rdTrue").prop("checked", false);
            showModel();
        }


        function ShowEditDiv(roleId, name, description, IsDefault) {
          
            $("#modaltitle").html("修改部门");
            $('#ctl00_ContentPlaceHolder1_txtAddRoleName').val(name);
            $('#ctl00_ContentPlaceHolder1_txtRoleDesc').val(description);
            $('#ctl00_ContentPlaceHolder1_htxtRoleId').val(roleId);

            if (IsDefault=="1") {
                $("#rdTrue").prop("checked", true);
            }

            showModel();
        }


        function showModel() {

            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });
        }


        // alert(123);

    </script>
</asp:Content>
