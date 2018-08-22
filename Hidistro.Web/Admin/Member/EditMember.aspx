<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMember.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.EditMember" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ctl00_ContentPlaceHolder1_rsddlRegion select{width:150px;float:left;margin-right:5px}
        #bindSysUser{cursor:pointer}
    </style>
    <script type="text/javascript">

        
        $(document).ready(function () {
            $('#datadiv').find('select').each(function () {
                $(this).removeClass();
                $(this).addClass('form-control');
            });
            InitValidators();

            $("#bindSysUser").click(function () {

                $("#BindShow").formvalidation({
                    'submit': '#ctl00_ContentPlaceHolder1_BindCheck',
                    'ctl00$ContentPlaceHolder1$txtBindName': {
                        validators: {
                            notEmpty: {
                                message: '用户名不能为空，6-20个字符！'
                            },
                            stringLength: {
                                min: 6,
                                max: 20,
                                message: '用户名不能为空，6-20个字符！'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtUserPassword': {
                        validators: {
                            notEmpty: {
                                message: '密码不能为空，6-20个字符！'
                            },
                            stringLength: {
                                min: 6,
                                max: 20,
                                message: '密码不能为空，6-20个字符！'
                            }
                        }
                    }
                });



                $('#BindShow').modal('toggle').children().css({
                    width: '400px', top: "170px"
                });

                $('#BindShow').on('hidden.bs.modal', function (e) {
                    window.location.reload();
                })
               
              

            });


        });

        function InitValidators() {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtRealName': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '姓名长度在20个字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtprivateEmail': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 256,
                            message: '请输入正确电子邮件，长度在1-256个字符以内'
                        },
                        regexp: {
                            regexp: /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/,
                            message: ''
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txtAddress': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: '街道地址必须控制在100个字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtQQ': {
                    validators: {
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: 'QQ号长度限制在3-20个字符之间，只能输入数字'
                        }
                        ,
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtCellPhone': {
                    validators: {
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: '手机号码长度限制在3-20个字符之间,只能输入数字'
                        }
                        ,
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtCardID': {
                    validators: {
                        stringLength: {
                            min: 15,
                            max: 18,
                            message: '只能输入15位或者18位身份证号码'
                        }
                        ,
                        regexp: {
                            regexp: /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/,
                            message: ''
                        }
                    }
                }
            });
          }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="thisForm" runat="server" class="form-horizontal">

        <div class="modal fade" id="BindShow">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left" >绑定系统用户名</h4>
      </div>
        <div class="modal-body form-horizontal" >
            <div class="form-group">
                
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>用户名：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="txtBindName"   CssClass="form-control  inputw120"    runat="server" ></asp:TextBox>
                        </div>
                </div>

            <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>密码：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="txtUserPassword" TextMode="Password"  CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>

          
           <input type="hidden" id="PSWUserIds" value=""  runat="server" />
        </div>
      <div class="modal-footer">
          <asp:Button ID="BindCheck"  class="btn btn-primary"   Text="确定" runat="server" />
        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

        <div id="datadiv">
            <div class="page-header">
                <h2>编辑会员信息</h2>
                <small>编辑会员各项信息资料</small>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">昵称：</label>
                <div class="col-xs-4" style="margin-top:5px;">
                    <asp:Literal ID="lblLoginNameValue" runat="server"></asp:Literal>                
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">系统用户名：</label>
                <div class="col-xs-4" style="margin-top:5px;" id="BindDiv">
                    <asp:Literal ID="LitUserBindName" runat="server"></asp:Literal>                
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">会员等级：</label>
                <div class="col-xs-4">
                    <Hi:MemberGradeDropDownList ID="drpMemberRankList" runat="server" AllowNull="false" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">姓名：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtRealName" CssClass="form-control" runat="server" />
                    <small class="help-block">姓名长度在20个字符以内</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">电子邮件地址：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtprivateEmail" CssClass="form-control" runat="server" />
                    <small class="help-block">请输入正确电子邮件，长度在1-256个字符以内</small>
                </div>
            </div>
            <div class="form-group hi">
                <label for="inputEmail1" class="col-xs-2 control-label">详细地址：</label>
                <div class="col-xs-6">
                    <Hi:RegionSelector runat="server" ID="rsddlRegion" />           
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">街道地址：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" />
                    <small class="help-block">街道地址必须控制在100个字符以内</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">QQ：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtQQ" CssClass="form-control" runat="server" />
                    <small class="help-block">QQ号长度限制在3-20个字符之间，只能输入数字</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">手机号码：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCellPhone" CssClass="form-control" runat="server" />
                    <small class="help-block">手机号码长度限制在3-20个字符之间,只能输入数字</small>
                </div>
            </div>
             <div class="form-group">
                <label for="txtCardID" class="col-xs-2 control-label">身份证号码：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCardID" CssClass="form-control" runat="server" />
                    <small class="help-block">只能输入15位或者18位身份证号码</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">注册日期：</label>
                <div class="col-xs-4" style="margin-top: 5px;">
                    <Hi:FormatedTimeLabel ID="lblRegsTimeValue"  runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">总消费金额：</label>
                <div class="col-xs-4" style="margin-top: 5px;">
                    <asp:Literal ID="lblTotalAmountValue" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-2 marginl">
                    <asp:Button runat="server" ID="btnEditUser" class="btn btn-success inputw100"
                        Text="确定" />
                </div>
            </div>
        </div>
</form>

</asp:Content>

