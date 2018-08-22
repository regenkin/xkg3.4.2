<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.EditManager" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
    <style>
        .col-xs-3{width:300px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
                    <h2>编辑管理员信息</h2>
    </div>

     <div class="mate-tabl">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" aria-expanded="false">基本信息</a></li>
                        <li role="presentation" class=""><a href='<%="EditManagerPassword.aspx?userId="+Page.Request.QueryString["userId"] %>' >修改密码</a></li>
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="profile">


           <!--表单-->
         <form id="thisForm" runat="server" class="form-horizontal" >
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label">用户名：</label>
                        <div class="col-xs-3">
                           <span class="form-control" style="border:none" ><asp:Literal ID="lblLoginNameValue" runat="server" /></span>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>所属部门：</label>
                        <div class="col-xs-3">
                            <Hi:RoleDropDownList ID="dropRole" runat="server"  CssClass="form-control" AllowNull="false" />
                        </div>
                    </div>
                     <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>邮件地址：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtprivateEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            <small class="help-block">请输入正确电子邮件，长度在1-256个字符以内</small>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-2 control-label">注册日期：</label>
                        <div class="col-xs-3">
                           <span class="form-control" style="border:none" ><Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" /> </span> 
                        </div>
                         
                    </div>
                     
                   
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                            <asp:Button ID="btnEditProfile" runat="server"  CssClass="btn btn-success" Text="保 存" />
                      
                        </div>
                    </div>




    
  
      

    </form>


                        </div>
                        <div role="tabpanel" class="tab-pane " id="messages">-</div>
                    </div>
  </div>


    <script>

        function InitValidators() {

            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtprivateEmail': {
                    validators: {
                        notEmpty: {
                            message: '请输入正确电子邮件，长度在6-256个字符以内 '
                        },
                        regexp: {
                            regexp: /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/,
                            message: '邮箱格式不正确！'
                        },
                        stringLength: {
                            min: 6,
                            max: 256,
                            message: '邮箱长度在6-256个字符以内 '
                        }
                    }
                }
            });

        }

        $(document).ready(function () {
            InitValidators();

        });


    </script>
</asp:Content>
