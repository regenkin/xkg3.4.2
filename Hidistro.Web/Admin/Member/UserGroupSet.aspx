<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGroupSet.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.UserGroupSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <style type="text/css">
        h2{font-size:15px; font-weight:bold;}
  
        .splitLine{ background-color:#cccccc; height:1px; margin-top:5px; }
    </style>
    <script>

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
    <div class="page-header">
        <h2>会员分组设置</h2>
    </div>
    <div id="mytabl">
        <div class="table-page">
            <ul class="nav nav-tabs">
                <li class="active">
                    <a href="UserGroupSet.aspx"><span>自动分组</span></a></li>
                <li>
                    <a href="CustomDistributorList.aspx"><span>手动分组</span></a></li>
            </ul>
        </div>
    </div>
           
              
          
                    <div class="form-group" style="margin-left: 50px; margin-top:50px">
                        <h2 style="color:green;"> <span style="color:red;margin-right: 5px;font-weight :900;">|</span>新会员 </h2>
                        <div class="splitLine"></div>
                    </div>
                    <div class="form-inline" style="margin-left: 100px; margin-bottom:5px;">
                      注册以后没有购买过的会员
                    </div>
                   

                    <div class="form-group" style="margin-left: 50px; margin-top:20px;">
                     <h2 style="color:green;">   <span style="color:red;margin-right: 5px;font-weight :900;">|</span>活跃会员 </h2>
                        <div class="splitLine"></div>
                    </div>

                    <div class="form-inline" style="margin-left: 100px; margin-bottom: 5px;">    
                         <label> 最近</label>   
                         <input id="txt_time" class="form-control resetSize" style="width :60px;" type="text" runat="server" />
                        <label>天(含)内有成交的会员</label>       
                    </div>

                <div class="form-group" style="margin-left: 50px; margin-top: 20px;">
                    <h2 style="color:green;"> <span style="color:red;margin-right: 5px;font-weight :900;">|</span>沉睡会员</h2>
                    <div class="splitLine"></div>
                </div>

  
                <div class="form-inline" style="margin-left: 100px; margin-bottom: 5px;">
                               <label> 最近</label>   
                      <label id="lbTime">180</label>  
                        <label>天(不含)内没有成交的会员</label>     
               </div>
         
                <div class="btn_bottom" style="margin-top: 5px; text-align: center; width:60%;">
                    <asp:Button ID="btnSaveClientSettings" runat="server" Text="保 存" CssClass="btn btn-success inputw100"
                                OnClientClick="return validForm()" />
               </div>
          
    </form>

     <script type="text/javascript">
         $(function () {
             $("#lbTime").html($("#<%=txt_time.ClientID%>").val());
             $("#<%=txt_time.ClientID%>").bind("keyup", function () {
                 $("#<%=txt_time.ClientID%>").val($("#<%=txt_time.ClientID%>").val().replace(/[^\d]/g, ''));
                 if (/^[0-9]*$/.test($(this).val())) {
                     $("#lbTime").html($(this).val());
                 }
             })
         })

         function validForm()
         {
             if (parseInt($("#<%=txt_time.ClientID%>").val()) < 1) {
                 ShowMsg("请输入大于1的整数！", false);
                 return false;
             }
             if ($("#<%=txt_time.ClientID%>").val().length > 3) {
                 ShowMsg("间隔时间不能超过999天！", false);
                 return false;
             }
             if ($("#<%=txt_time.ClientID%>").val().length > 0)
             {
                 return true;
             }
          
             ShowMsg("请输入0-999数字！", false);
             return false;
         }
     </script>
</asp:Content>
