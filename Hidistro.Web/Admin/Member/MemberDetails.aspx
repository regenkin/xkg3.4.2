<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberDetails.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.MemberDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="thisForm" runat="server" class="form-horizontal">
    <div class="areacolumn clearfix">
        <div>
            <div class="page-header">
                <h2>查看“<asp:Literal runat="server" ID="litUserName" />”会员信息</h2>
                <small>会员账户的详细信息</small>
            </div>
            <div class="formitem clearfix">
                <div class="form-group" style="display:none;">
                    <label for="inputEmail1" class="col-xs-2 control-label">推广链接：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="lblUserLink" runat="server" CssClass="form-control"  />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">会员等级：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litGrade" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">姓名：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litRealName" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">绑定用户：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litUserBindName" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">电子邮件地址：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">详细地址：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litAddress" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>


                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">QQ：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litQQ" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">微信OpenId：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litOpenId" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="litAlipayOpenid" class="col-xs-2 control-label">服务窗OpenId：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litAlipayOpenid" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>


                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">手机号码：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litCellPhone" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>
                
                <div class="form-group">
                    <label for="txtCardID" class="col-xs-2 control-label">身份证号码：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtCardID" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">注册日期：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="litCreateDate" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group" style="width:110%">
                    <div class="col-xs-offset-2 marginl">
                        <asp:Button runat="server" ID="btnEdit" class="btn btn-success bigsize"
                            Text="编辑此会员" /> &nbsp;&nbsp;<input type="button" value="返回" class="btn btn-default bigsize" onclick="history.go(-1)" />
                    </div>
                </div>
            </div>

        </div>
    </div>
        </form>
</asp:Content>
