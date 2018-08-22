<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="ReplyProductConsultations.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ReplyProductConsultations" %>
<%@ Register src="~/hieditor/ueditor/controls/ucUeditor.ascx" tagname="KindeditorControl" tagprefix="Kindeditor" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <style>
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    </style>
<form runat="server">
    <div class="page-header">
            <h2>客户咨询回复</h2>
            <small>管理员回复客户咨询</small>
        </div>
     <div class="form-horizontal">
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"> 咨询用户</label>
                <div class="col-xs-3" style="padding-top:5px;">
                      <asp:Literal ID="litUserName" runat="server"></asp:Literal>
                </div>
            </div>
         <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"> 咨询时间</label>
                <div class="col-xs-3" style="padding-top:5px;">
                   <Hi:FormatedTimeLabel ID="lblTime" runat="server"></Hi:FormatedTimeLabel>
                </div>
            </div> <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"> 咨询内容</label>
                <div class="col-xs-3" style="padding-top:5px;">
                   <asp:Literal ID="litConsultationText" runat="server"></asp:Literal>
                </div>
            </div> <div class="form-group">
                <label for="inputEmail3" class="col-xs-2 control-label"> 回复</label>
                <div class="col-xs-3">
                  <Kindeditor:KindeditorControl id="fckReplyText" runat="server" Width="550"  height="200" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-3 col-xs-10">

                    <asp:Button ID="btnReplyProductConsultation" Text="保 存" CssClass="btn btn-primary" runat="server" />
                   
                </div>
            </div>
         </div>
 
 
 
</form>
</asp:Content>