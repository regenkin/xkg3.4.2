<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AddManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.settings.AddManager" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
       <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
      <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
      <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
     <script>
         function ShowSecondMenuLeft(firstnode, secondurl, threeurl) {
             window.parent.ShowMenuLeft(firstnode, secondurl, threeurl);
             art.dialog.close();
         }
   </script>
    <style>
        table{margin:15px 5px;width:100%;line-height:30px}
        /*body{font-size:12px}*/
        em{color:red}
        .col-xs-3{width:300px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
                    <h2>增加管理员</h2>
    </div>

    <form id="thisForm" runat="server" class="form-horizontal" >
        <asp:HiddenField ID="hidpic" runat="server" />
     <asp:HiddenField ID="hidpicdel" runat="server" />
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em >*</em>用户名：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-2 control-label"><em >*</em>密码：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server"/>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-2 control-label"><em >*</em>确认密码：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtPasswordagain" TextMode="Password" CssClass="form-control" runat="server"/>
                        </div>
                    </div>
                     
        <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em >*</em>邮箱地址：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                        </div>
                    </div>

                 <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em >*</em>所属于部门：</label>
                        <div class="col-xs-3">
                            <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" CssClass="form-control" />
                        </div>
                    </div>
                   
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                             <asp:Button ID="btnSave" runat="server" OnClientClick=""   Text="添加"  CssClass="btn btn-success" OnClick="btnSave_Click"  />
                        </div>
                    </div>




    
  
      

    </form>


    <script>

        $(function () {


            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtUserName': {
                    validators: {
                        notEmpty: {
                            message: '3-20个字符，支持汉字、字母、数字等组合'
                        },
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: '3-20个字符，支持汉字、字母、数字等组合'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtPassword': {
                    validators: {
                        notEmpty: {
                            message: '密码为3-20个字符，可由英文‘数字及符号组成'
                        },
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: '密码为3-20个字符，可由英文‘数字及符号组成'
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txtPasswordagain': {
                    validators: {
                        notEmpty: {
                            message: '重复密码不能为空'
                        },
                        repeatPass: {
                            message: '密码与上次输入不符'
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txtEmail': {
                    validators: {
                        notEmpty: {
                            message: '邮箱不能为空'
                        },
                        regexp: {
                            regexp: /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/,
                            message: '请输入有效的邮箱地址'
                        }
                    }
                }

            });


            

        });

    </script>

</asp:Content>
