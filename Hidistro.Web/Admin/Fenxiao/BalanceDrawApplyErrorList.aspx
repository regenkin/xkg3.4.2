<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BalanceDrawApplyErrorList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.BalanceDrawApplyErrorList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Import Namespace="Hidistro.ControlPanel.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script>
     $(function () {
        $('.allselect').change(function () {
                 $('.content-table input[type="checkbox"]').prop('checked', $(this)[0].checked);
        });
        var tableTitle = $('.title-table').offset().top - 58;
        $(window).scroll(function () {
            if ($(document).scrollTop() >= tableTitle) {
                $('.title-table').css({
                    position: 'fixed',
                    top: '58px'
                })
            }
            if ($(document).scrollTop() + $('.title-table').height() + 58 <= tableTitle) {
                $('.title-table').removeAttr('style');
            }
        });
});


     function ShowRefuseSignalShow(sid, obj) {

         var rowParentTd = $(obj).parents("tr").children("td");
         $('#RefuseSignalShow').modal('toggle').children().css({
             width: '500px', top: "170px"
         });

         $("#ctl00_ContentPlaceHolder1_SignalrefuseMks").val(rowParentTd.eq(5).html().trim())
         $("#ctl00_ContentPlaceHolder1_hSerialID").val(sid);
         // alert($("#ctl00_ContentPlaceHolder1_hSerialID").val())
        // return false;
     }


 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form  runat="server">
         <!--申请驳回-->
  <div class="modal fade" id="RefuseSignalShow">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left" >申请驳回</h4>
      </div>
          <asp:HiddenField ID="hSerialID" Value="" runat="server" />
          <div class="set-switch form-horizontal" style="margin-bottom:0px">
              <div class="form-group" style="margin-bottom:0px">
                  <label class="col-xs-3 control-label" style="text-align:left;width:95px"><em>*</em>驳回理由：</label>
                  <div class="col-xs-9" style="width:400px">
                       <asp:TextBox  ID="SignalrefuseMks" TextMode="MultiLine" CssClass="form-control  inputw120"  Height="100"  runat="server" ></asp:TextBox>
                  <small>驳回理由必需填写</small>     
                  </div>
              </div>
        <div style="clear:both"> </div>
        </div>
     
      <div class="modal-footer">
          <asp:Button ID="Button3"  class="btn btn-primary" Text="确定"  runat="server"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
      </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
  </div><!-- /.modal -->


     <!--标题-->
         <div class="page-header">
            <h2>提现申请列表</h2>
         </div>
    
     <div class="set-switch">
                    <div class="form-horizontal clearfix">
                      

                        <div class="form-inline  mr20">
                            
                            
                            <div class="form-inline journal-query">
                            <div class="form-group">
                                <label　 for="sellshop1">　店铺名：</label>
                                <asp:TextBox  ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                             </div>
                                
                                <div class="form-group" style="padding-left:4px">
                                    <label  for="setdate">时间范围：</label>
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw100" />&nbsp;至&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw100" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnSearchButton" runat="server" class="btn resetSize btn-primary" Text="查询"/>&nbsp;&nbsp; <%--OnClick="btnQueryLogs_Click"--%> 
                            </div>
                        </div>
                    </div>
                </div>

          <!--数据tab-->
 <div class="play-tabs" style="padding-bottom:3px">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="">
                            <asp:LinkButton ID="Frist" Text="待发放(0)"  runat="server" OnClick="Frist_Click" ></asp:LinkButton>
                        <li role="presentation"  class="active"><asp:LinkButton ID="Second" Text="发放异常(0)" runat="server"  ></asp:LinkButton></li>
                    </ul>
 </div>
             
        <!--数据列表-->
             <div>

                 <div class="title-table">
                            <table class="table" style="margin-bottom:0px">
                                <thead>
                                    <tr>
                                        <th width="30"></th>
                                        <th width="100" style="text-align:left">店铺名称/手机</th>
                                        
                                        <th width="50">提现金额</th>
                                        <th width="50">账号类型</th>
                                        <th width="100">账号/收款人</th>
                                        <th width="300">支付异常</th>
                                        <th width="100"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                     </div>

                      <div class="content-table">
                      <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <tbody>
<%-- OnItemDataBound="rptList_ItemDataBound"--%>
        <asp:Repeater ID="reCommissions"  runat="server"  ><%--OnItemCommand="rptList_ItemCommand"--%>
     <ItemTemplate>
      <tr  class="td_bg">
                    <td width="30">&nbsp;<input name="CheckBoxGroup" type="checkbox" title="<%#Eval("IsCheck") %>" value='<%#Eval("SerialID") %>' /></td>
                              <td width="100" style="text-align:left">
                                  &nbsp;&nbsp;<%# Eval("StoreName")%>&nbsp;<br />
                                   &nbsp;&nbsp;<%# Eval("CellPhone")%></td>
                                <td width="50">￥<%# Eval("Amount", "{0:F2}")%></td>
                                <td width="50" IsCheck='<%# Eval("IsCheck")%>' title="<%# Eval("RequestType")%>"><%# VShopHelper.GetCommissionPayType(Eval("RequestType").ToString()) %>&nbsp;
                                </td>
                                  <td width="100" Userid="<%# Eval("UserId")%>">
                                      <p><%# Eval("MerchantCode") %>&nbsp;</p>
                                       <p><%# Eval("AccountName") %>&nbsp;</p>
                                </td>
                                 <td width="300" style="text-align:left;color:red">
                                     <%# Eval("Remark") %>&nbsp;
                                </td>
                                 
                                <td width="100">
                                                   <p>
                                   <a onclick="javascript:ShowRefuseSignalShow(<%# Eval("SerialID")%>,this)" class="btn btn-default btn-xs">驳回</a>
                                                   </p>
                                </td>
        </tr>
     </ItemTemplate>
 </asp:Repeater>
         </tbody>
     </table>
     </div>

    

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       <a onclick="javascript:history.go(-1)" class="btn btn-primary">返回</a>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="20" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>

             </div>
        </form>
</asp:Content>
