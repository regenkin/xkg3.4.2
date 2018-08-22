<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMemberGrade.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.AddMemberGrade" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <script src="/admin/js/bootstrapSwitch.js" type="text/javascript"></script>
    <link href="/admin/css/bootstrapSwitch.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {


            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtRankName': {
                    validators: {
                        notEmpty: {
                            message: "会员等级名称不能为空，长度限制在20字符以内"
                        },
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '会员等级名称不能为空，长度限制在20字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_tradeVol': {
                    validators: {
                        notEmpty: {
                            message: "请输入满足交易额"
                        },
                        regexp: {
                            regexp: /^[0-9]+\.{0,1}[0-9]{0,2}$/,
                            message: '请输入数字'
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txt_tradeTimes': {
                    validators: {
                        notEmpty: {
                            message: "请输入满足交易次数"
                        },
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: '请输入数字'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtValue': {
                    validators: {
                        notEmpty: {
                            message: '等级折扣为不能为空，且是数字'
                        },
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: '请输入数字'
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="thisForm" runat="server" class="form-horizontal">

        <div class="page-header">
            <h2><%=htmlOperName %>会员等级</h2>
        </div>
        <div>

            <div class="form-group" style="margin-left: 5px; margin-bottom: 30px;">
                <div style="font-size: 16px;"><span style="color: red; margin-right: 10px; font-weight: 900;">|</span>基本信息 </div>
                <div class="splitLine"></div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>会员等级名称：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtRankName" CssClass="form-control inputw150" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>会员折扣：</label>
                <div class="col-xs-4">
                    现价×<asp:TextBox ID="txtValue" CssClass="form-control inl ml5" Width="90px" runat="server" /> %
                <br />
                    <small>输入90表示打9折，100%表示不打折.</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>设为默认：</label>
                <div id="radioDiv" class="col-xs-4">
                    <div class="switch" id="mySwitch">
                        <input type="checkbox" id="cbIsDefault" runat="server" />
                    </div>
                </div>
            </div>

            <div class="form-group" style="margin-left: 5px; margin-bottom: 30px;">
                <div style="font-size: 16px;"><span style="color: red; margin-right: 10px; font-weight: 900;">|</span>自动升级 </div>
                <div class="splitLine"></div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>满足交易额：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txt_tradeVol" CssClass="form-control inputw150 inl" runat="server" />
                    元
                </div>
            </div>
            <div class="form-group" style="margin-bottom: 5px; margin-top: -10px;">
                <label for="inputEmail1" class="col-xs-2 control-label"></label>
                <div class="col-xs-4">
                    或
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>满足交易次数：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txt_tradeTimes" CssClass="form-control inputw150" runat="server" />
                </div>
            </div>

            <div class="form-group" style="display: none;">
                <label for="inputEmail1" class="col-xs-2 control-label">备注：</label>
                <div class="col-xs-4" style="margin-left: 5px;">
                    <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine" CssClass="form-group"
                        Width="450" Height="120"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-xs-offset-2">
                    <asp:Button runat="server" ID="btnSubmitMemberRanks" class="btn btn-success inputw100 ml20"
                        Text="确定" />
                </div>
            </div>
        </div>
    </form>


</asp:Content>

