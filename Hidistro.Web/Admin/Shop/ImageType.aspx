<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageType.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Shop.ImageType" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <script type="text/javascript">
        function ShowAddDiv(roleId, name, description) {
            //验证方法'
            //vilidsetings = {
            //    'ctl00$ContentPlaceHolder1$txt_AddImageTypeName': {
            //        validators: {
            //            notEmpty: {
            //                message: '填写分组名称'
            //            },
            //            stringLength: {
            //                min: 1,
            //                max: 20,
            //                message: '长度不能超过20个字符'
            //            }
            //        }
            //    }
            //};
            //arrytext = null;
            //formtype = "add";
            $('#<%=txt_AddImageTypeName.ClientID%>').val("");
            //DialogShowNew('添加图片分组', 'rolesetcmp', 'addImageType', 'ctl00_ContentPlaceHolder1_btn_AddImageType');
            $('#addImageType').modal('toggle').children().css({
                width: '520px',
                height: '260px'
            })
            $("#addImageType").modal({ show: true });
        }

        //function validatorForm() {
        //    $("#hform").find(":input").trigger("blur"); //触发验证
        //    var numError = $("#hform").find('.has-error').length;
        //    if (numError) return false; //验证未通过
        //    return true;
        //}
        //function beforeSubmit() {
        //    var diplay = $('#addImageType').attr('display');
        //    if (diplay == 'none') {
        //        return false;
        //    }
        //    else {
        //        return true;
        //    }
        //}
        function CheckForm() {
            var info = document.getElementById("ctl00_ContentPlaceHolder1_txt_AddImageTypeName");
            if ($.trim(info.value) == "") {
                ShowMsg("请填写分组名称！");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>图片分组管理</h2>
            <%--<small>查看不同图片分组的图片信息</small>--%>
        </div>

        <UI:Grid ID="ImageTypeList" runat="server" AutoGenerateColumns="false" ShowHeader="true"
            GridLines="None"
            HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
            DataKeyNames="CategoryId" Width="80%">
            <Columns>
                <asp:TemplateField HeaderText="分组名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <Hi:HtmlDecodeTextBox ID="ImageTypeName" runat="server" Text='<%# Bind("CategoryName") %>' CssClass="form-control" MaxLength="10" />
                    </ItemTemplate>
                </asp:TemplateField>
                <UI:SortImageColumn HeaderText="排序" ReadOnly="true" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left" />
                <asp:TemplateField HeaderText="操作" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <span class="submit_shanchu">
                          <%--  <Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" />--%>
                            <asp:Button runat="server" ID="lkbtnDelete"  CommandName="Delete" Text="删除" CssClass="btnLink" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后该分组下面的图片将会分配到默认分组中！</p>', this);" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>

        <div class="btn_bottom" style="margin-top: 20px;">
            <asp:Button ID="ImageTypeEdit" runat="server" Text="批量保存" CssClass="btn btn-primary bigsize" />
            <%--            <asp:Button ID="ImageTypeAdd" runat="server" OnClientClick="ShowAddDiv(); return false;"
                Text="添加分组" CssClass="btn btn-success bigsize" />--%>
            <input type="button" onclick="ShowAddDiv();" class="btn btn-success bigsize" value="添加分组" />
        </div>




        <div class="modal fade" id="addImageType">
            <div class="modal-dialog ">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">添加图片分组</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-xs-2 control-label">分组名称：</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="txt_AddImageTypeName" runat="server" CssClass="form-control" Width="300px" MaxLength="10"></asp:TextBox>
                                <small class="help-block">长度限制在10个字符以内</small>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_AddImageType" runat="server" Text="保存" CssClass="btn btn-success" OnClientClick="return CheckForm()" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>
    </form>
</asp:Content>
