<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequestSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.BalanceDrawRequestSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <Hi:Style ID="Style1"  runat="server" Href="/admin/css/bootstrapSwitch.css" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/bootstrapSwitch.js" />
    <style>

        #ctl00_ContentPlaceHolder1_CheckRealName, #ctl00_ContentPlaceHolder1_DrawPayType{margin-top:6px;display:block}
        #ctl00_ContentPlaceHolder1_CheckRealName label,#ctl00_ContentPlaceHolder1_DrawPayType label{margin-right:20px}
    </style>
    <script>

        function checkForm() {

            if ($("#ctl00_ContentPlaceHolder1_DrawPayType_0")[0].checked == false &&
                $("#ctl00_ContentPlaceHolder1_DrawPayType_1")[0].checked == false &&
                $("#ctl00_ContentPlaceHolder1_DrawPayType_2")[0].checked == false &&
                $("#ctl00_ContentPlaceHolder1_DrawPayType_3")[0].checked == false
                ) {
                HiTipsShow("至少选择一种提现支付方式", 'error');
                return false;
            }
            var m=$("#ctl00_ContentPlaceHolder1_txtApplySet").val().trim();
            if (!/^\d+$/.test(m) || m=="" || m<1)
            {
                HiTipsShow("最低提现金额不能少于数值1，请填整数值！", 'error');
                return false;
            }

            return true;
        }

        function selpay(value,obj) {

           
            if (value == 0) {
                if ($(obj)[0].checked) {
                    $("#ctl00_ContentPlaceHolder1_weipaypanel").show();
                } else {
                    $("#ctl00_ContentPlaceHolder1_weipaypanel").hide();
                }
            }
            else if (value == 1) {
                if ($(obj)[0].checked) {
                    $("#ctl00_ContentPlaceHolder1_alipaypanel").show();
                } else {
                    $("#ctl00_ContentPlaceHolder1_alipaypanel").hide();
                }
            }

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <div class="page-header">
                    <h2>提现设置</h2>
    </div>
    <form runat="server" >
    <div class="form-horizontal" >

         <!--表单-->
        <div class="form-group has-feedback">
                        <label class="col-xs-2 control-label"><em>*</em>提现支持账户：</label>
                        <div class="col-xs-7">

                            <asp:CheckBoxList ID="DrawPayType" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                          <asp:ListItem Value="1" onclick="selpay(1,this)">支付宝</asp:ListItem>  　　
                        <asp:ListItem  Value="0"  onclick="selpay(0,this)" >微信支付</asp:ListItem>
                        <asp:ListItem  Value="2">线下转账</asp:ListItem>
                        <asp:ListItem  Value="3">微信红包</asp:ListItem>
                            </asp:CheckBoxList>
                            <small>如需使用支付宝或微信支付自动转账，请先设置好支付宝和微信支付的账号信息；线下转账方式是分销商提现时，填写自己的收款帐户信息，管理员再根据信息手工转账
</small>
                        </div>

                    </div>



                    <div class="form-group" id="alipaypanel" runat="server">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>支付宝批量转账：</label>
                        <div class="col-xs-7">
                            <div class="switch">
                                <input id="alipayCheck" type="checkbox" name="radioCommission" runat="server"/>
                            </div>
                            <small>开启以后，可以实现支付宝批量发放提现，自动转账。
                                <a href="https://b.alipay.com/order/appInfo.htm?salesPlanCode=2011052500326597&channel=ent" target="_blank">开通支付宝批量付款</a>
                            </small>
                        </div>
                    </div>

        <div id="weipaypanel" runat="server">
                    <div class="form-group" >
                        <label for="inputEmail3" class="col-xs-2 control-label">微信支付批量转账：</label>
                        <div class="col-xs-5">
                            <div class="switch" id="mySwitch">
                                <input id="weixinPayCheck" type="checkbox" name="radiorequest" runat="server"/>
                            </div>
                            <small>开启以后，可以实现微信支付批量发放提现，自动转账。</small>
                        </div>
                    </div>

         <div class="form-group has-feedback">
                        <label class="col-xs-2 control-label">姓名校验类型：</label>
                        <div class="col-xs-7">

                            <asp:RadioButtonList ID="CheckRealName"  runat="server"  RepeatDirection="Horizontal"  RepeatLayout="Flow">
                        <asp:ListItem Value="1">校验真实姓名</asp:ListItem>  　　
                        <asp:ListItem  Value="2" Selected="True"　>仅当用户是实名用户时校验真实姓名</asp:ListItem>
                        <asp:ListItem  Value="0">不校验真实姓名</asp:ListItem>

                            </asp:RadioButtonList>
                        </div>
                    </div>
</div>
        <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>单次最低提现金额：</label>
                        <div class="col-xs-5">
                               <asp:TextBox ID="txtApplySet" CssClass="form-control" Text="1" style="width:100px" runat="server"></asp:TextBox>
                            <small>设置分销商每次提现的最低金额，单位：元</small>
                        </div>
                    </div>
                    


                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                         <asp:Button ID="btnSave" runat="server" OnClientClick="return checkForm();"
                        Text="保存" CssClass="btn btn-success inputw100"  OnClick="btnSave_Click"  />
                        </div>
                    </div>

    </div>
    </form>
</asp:Content>
