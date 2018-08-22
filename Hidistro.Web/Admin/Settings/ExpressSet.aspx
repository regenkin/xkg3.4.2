<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="ExpressSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.ExpressSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>物流跟踪配置</h2>
    </div>
    <form runat="server" id="thisForm" class="form-horizontal">
        <div>
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label">快递100 key：</label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtKey" runat="server" CssClass="form-control resetSize inputw350" Style="display: inline-block;" Text=""></asp:TextBox>
                    <a href="http://www.kuaidi100.com/openapi/applyapi.shtml" style="margin: inherit" target="_blank">去申请</a>
                    <p>快递100跟踪物流状态时使用的参数，可以去快递100申请该key.</p>
                   <%-- <p>添加了快递100友情链接的网站：http://你申请的网站域名/default.aspx</p>--%>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>物流查询地址：</label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control resetSize inputw350" Text="http://m.kuaidi100.com/index_all.html?postid={numberId}&amp;type={companyCode}&amp;callbackurl={callbackurl}" ></asp:TextBox>
                    <p>在没有填写快递100 key时，系统将访问该地址查询物流信息。</p>
                    <span style="color: red">其中{numberId}为运单号码参数，修改地址时，请原样粘贴</span>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-2 col-xs-10 marginl">
                    <asp:Button runat="server" ID="btnSave" class="btn btn-success inputw100" Text="保存" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
