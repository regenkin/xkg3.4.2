<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="SMSSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.SMSSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_btnSaveSMSSettings { margin-left:16px;}
        #ctl00_ContentPlaceHolder1_btnTestSend { margin-left:16px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>短信发送设置</h2>
        </div>
        <div> 
            <div id="maindiv"  >
                <div class="form-group">
                    <label class="col-xs-2 control-label"></label>
                    <div class="col-xs-6">
                        第一次使用请先<a href="http://sms.kuaidiantong.cn/" target="frammain" >点击这里</a>注册账号获取短信接口的AppKey和AppSecret
                    </div>
                </div>      
                <div class="form-group">
                    <label for="inputEmail4" class="col-xs-2 control-label">Appkey：</label>
                    <div class="col-xs-4">
                        <div style="display:none;">
                             <span class="formitemtitle Pw_140">发送方式：</span>
                             <select id="ddlSms" name="ddlSms"></select>
                        </div>
                        <asp:TextBox runat="server" class="form-control" ID="txtAppkey"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail4" class="col-xs-2 control-label">Appsecret：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txtAppsecret"></asp:TextBox>

                    </div>
                </div>                
                <div class="form-group">
                    <div class="col-xs-offset-2">
                        <asp:Button runat="server" class="btn btn-success inputw100" ID="btnSaveSMSSettings" Text="保存" />
                    </div>
                </div>
                                        

                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">接收手机号：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txtTestCellPhone"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">测试内容：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txtTestSubject"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2">
                        <asp:Label ID="lbMsg" runat="server" Text="" ForeColor="Red" /><br />
                        <asp:Button runat="server" class="btn btn-success inputw100" ID="btnTestSend" OnClientClick="return TestCheck();"  Text="测试发送" />
                    </div>
                </div>
            </div>
        </div>

  <asp:HiddenField runat="server" ID="txtSelectedName" />
  <asp:HiddenField runat="server" ID="txtConfigData" />
  <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />   
<script type="text/javascript">
    $(document).ready(function () {
        pluginContainer = $("#pluginContainer");
        templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
        dropPlugins = $("#ddlSms");
        selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");

          // 绑定短信类型列表
          $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
          $.ajax({
              url: "PluginHandler.aspx?type=SMSSender&action=getlist",
              type: 'GET',
              async: false,
              dataType: 'json',
              timeout: 10000,
              success: function (resultData) {
                  //alert(resultData.qty);
                  if (resultData.qty == 0)
                      return;

                  $.each(resultData.items, function (i, item) {
                      if (item.FullName == $(selectedNameCtl).val())
                          $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                      else
                          $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                  });
              }
          });

          //$(dropPlugins).bind("change", function() { SelectPlugin("SMSSender"); });

          //$(dropPlugins).attr("disabled", "disabled");
 

          if ($(selectedNameCtl).val().length > 0) {
              SelectPlugin("SMSSender");
          }
      });

      function TestCheck() {
          $("#lbMsg").text("");

          if ($(dropPlugins).val() == "") {
              alert("请先选择发送方式并填写配置信息");
              return false;
          }
          if ($("#Appkey").val().length == 0) {
              alert("Appkey必须填");
              return false;
          }
          if ($("#Appsecret").val().length == 0) {
              alert("Appsecret必须填");
              return false;
          }
          if ($("#ctl00_contentHolder_txtTestCellPhone").val().length == 0) {
              alert("请输入接收手机号码");
              return false;
          }
          if ($("#ctl00_contentHolder_txtTestSubject").val().length == 0) {
              alert("请输入发送内容");
              return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }

      function Save() {
          if ($("#Appkey").val().length == 0) {
              alert("Appkey必须填");
              return false;
          }
          if ($("#Appsecret").val().length == 0) {
              alert("Appsecret必须填");
              return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }
</script>

</form>



  

</asp:Content>
