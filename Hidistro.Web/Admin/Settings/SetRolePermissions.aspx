<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true"
    CodeBehind="SetRolePermissions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.SetRolePermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        $(function () {
           
            $('.checkboxlist .classa input').click(function () {
                if ($(this)[0].checked) {
                    $(this).parents('.checkboxlist').find('input').prop('checked', true);
                } else {
                    $(this).parents('.checkboxlist').find('input').prop('checked', false);
                }
            });
            $('.checkboxlist .two input').click(function () {
                if ($(this).next().get(0)) {
                    if ($(this)[0].checked) {
                        $(this).parents('.titlecheck').next().find('input').prop('checked', true);
                    } else {
                        $(this).parents('.titlecheck').next().find('input').prop('checked', false);
                    }
                }
                var chekLen = $(this).parents('.twoinerlist').find('input:checked').size(),
                    allLen = $(this).parents('.twoinerlist').find('input').size();
                if (chekLen == allLen) {
                    $(this).parents('.twoinerlist').prev().find('strong').prev().prop('checked', true);
                } else {
                    $(this).parents('.twoinerlist').prev().find('strong').prev().prop('checked', false);
                }
                var allCheckLen = $(this).parents('.checkboxlist').find('.two input:checked').size();
                allAllLen = $(this).parents('.checkboxlist').find('.two input').size();
                if (allCheckLen == allAllLen) {
                    $(this).parents('.checkboxlist').find('.classa input').prop('checked', true);
                } else {
                    $(this).parents('.checkboxlist').find('.classa input').prop('checked', false);
                }
            });
            $("#btnSubmit").click(function () { SaveDate(); });
            $("#cancelSubmit").click(function () { location.href = 'Roles.aspx'; });

           $("input[value='m04_hyp09']").parent().hide();
        })

        function SaveDate() {
            var selectChecks = $("input[name='permissions']:checked");
            var selectValue = '';
            selectChecks.each(function () {
                selectValue += $(this).val() + ',';
            });
            if (selectValue != '') {
                selectValue = selectValue.substr(0, selectValue.length - 1);
            }

            $.ajax({
                url: 'SaveRolePermissionData.ashx',
                type: 'post',
                dataType: 'json',
                data: { 'roleId': $("#txtRoleId").val(), 'rolePermissions': selectValue },
                success: function (data) {
                    if (data.status == '1') {
                       ShowMsgAndReUrl(data.Desciption, true, 'Roles.aspx');
                    } else {
                        ShowMsg(data.Desciption, false);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" style="margin-top: 20px; margin-bottom: 60px;">

        <div class="page-header">
            <h2>部门权限配置</h2>
        </div>
        <div class="setadmintable">
            <table width="100%" class="table" border="1">
                <thead>
                    <tr>
                        <th width="20%">部门名称</th>
                        <th width="80%">权限列表</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:Literal ID="ltRoleName" runat="server"></asp:Literal></td>
                        
                        <td class="border">
                            <asp:Literal ID="ltHtml" runat="server"></asp:Literal>
                           
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="footer-btn navbar-fixed-bottom">
            <input type="button" class="btn btn-success" id="btnSubmit" value="保存" style="width: 100px;" />
            <input type="button" class="btn btn-success" id="cancelSubmit" value="取消" style="width: 100px;" />
        </div>

   
    </form>

</asp:Content>
