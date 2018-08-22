<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="RefoundList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.RefoundList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" TagName="ViewTab" Src="~/Admin/Oneyuan/OneTaoViewTab.ascx" %> 
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
        .tableRow{border:1px solid #ccc}
        .tableRow tr td{padding-left:5px!important;vertical-align:middle!important}
        .tableRow tr:hover{background:#def3e1}
        .table th{text-align:left!important;padding-left:5px!important}
    </style>


      <script>

          function AView(obj)
          {
            
              var remark = $(obj).attr("remark");

              if ($('#RemarkShow').length == 0) {
                  AddDialog("RemarkShow", "");
              }

              $('#RemarkShow').find(".info").html(remark);
              $('#RemarkShow').modal('toggle').children().css({width: '400px' });
          }

          function DoRefund(pid,PayWay)
          {
              //OneyungRefund.aspx
              if (PayWay == "") {
                  HiTipsShow("当前记录未支付，异常数据！", "warning");
                  return;
              }

              if (PayWay == "alipay") {

                  HiTipsShow("确定退款？", "confirmII", function () {
                      window.open("../OutPay/OneyungRefund.aspx?pids="+pid);
                  });

              }
              else if (PayWay == "weixin") {
                  HiTipsShow("确定退款？", "confirmII", function () {
                      var DataJson = { action: "WeiXinRefund", Pid: pid };
                      AjaxPost(DataJson);
                  });
              }
              
          }


          function BatchRefund()
          {
              if ($("input[name=CheckBoxGroup]:checked").length < 1) {
                  HiTipsShow("没有记录被选择", "warning");
                  return;
              }

              var Aids = [];
              var payWay = "";
              var iserr = false;
              $("input[name=CheckBoxGroup]:checked").each(function () {
                  var IsRefund = $(this).attr("IsRefund");
                  var tempPayway = $(this).attr("PW");

                  if (payWay.trim() == "") {
                      payWay = tempPayway;
                  }
                  else {  
                      if (payWay != tempPayway) {
                          iserr=true;
                          return;
                      }
                  }

                  if (IsRefund == "False") {
                      Aids.push($(this).val());
                  }
                  else {
                      $(this).prop("checked", false);
                  }
              });


              if (iserr) {

                  HiTipsShow("您选择的记录中同时存在微信支付与支付宝付款两种方式，无法批量退款！", "warning");
                  return;
              }

              if (Aids.length == 0) {
                  HiTipsShow("没有符合退款条件的记录被选择！", "warning");
                  return;
              }

              var idstr = Aids.join(","); //可删除的AID
              var DataJson = { action: "WeiXinBacthRefund", Pids: idstr };

              HiTipsShow("确定批量退款，当前选择了" + Aids.length + "条退款记录？", "confirmII", function () {
                  
                  if (payWay == "alipay") {
                      $('#PayWait').modal({ backdrop: 'static', keyboard: false,show:true});
                    //  $('#PayWait').modal("toggle");
                      window.open("../OutPay/OneyungRefund.aspx?pids=" + idstr);
                  } else
                  {
                      $('#PayWeixin').modal({ backdrop: 'static', keyboard: false, show: true });
                      AjaxPost(DataJson, function () {
                          $('#PayWeixin').modal('toggle');
                      });
                  }
                 
              });

          }


          function AjaxPost(PostData,callback) {
              var url = "RefoundList.aspx";
              $.ajax({
                  type: "post",
                  url: url,
                  data: PostData,
                  async: false,
                  dataType: "json",
                  success: function (data) {

                      if (callback) {
                          callback();
                      }

                      if (data.state) {
                          HiTipsShow(data.msg, "success", function () {
                              window.location.reload(); //重新加载当前页
                          });

                      } else {
                          HiTipsShow(data.msg, "error", function () {
                              window.location.reload(); //重新加载当前页
                          });
                      };
                  },
                  error: function () {
                      HiTipsShow("访问服务器异常！", "error");
                  }
              });




          }



     $(function () {

            var tabNum = getParam("state");
            if (tabNum != "")
            {
                var $tab = $(".nav-tabs li");
                $tab.removeClass("active");
                $tab.eq(tabNum).addClass("active");
            }
         

            $("#sells1").click(function () {
                $("input[name=CheckBoxGroup]").prop("checked", $(this).prop("checked"));
            });
     });

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
        <h2 id="txtEditInfo" runat="server">一元夺宝退款
        </h2>
    </div>
      <form runat="server">

             <div id="mytabl" style="margin-top:10px">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li class="active"><a href="RefoundList.aspx?state=0"><asp:Literal ID="ListWait" Text="待退款(10)" runat="server"></asp:Literal></a></li>
                    <li><a href="RefoundList.aspx?state=1"><asp:Literal ID="ListEnd" Text="已退款(0)"  runat="server"></asp:Literal></a></li>
                </ul>
                <div class="page-box">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
               <div class="tab-pane active">

                    <div class="set-switch">
                        
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop4">活动标题：</label>
                                <asp:TextBox ID="txtTitle" CssClass="form-control  resetSize  inputw100" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop4">用户昵称：</label>
                                <asp:TextBox ID="txtUserName" CssClass="form-control  resetSize  inputw100" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop4">联系电话：</label>
                                <asp:TextBox ID="txtPhone" CssClass="form-control  resetSize  inputw150" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop5">支付方式：</label>
                            <asp:DropDownList ID="txtPayWay" CssClass="form-control  resetSize " runat="server" >
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="alipay">支付宝</asp:ListItem>
                                    <asp:ListItem Value="weixin">微信支付</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                 
                             <div class="form-group" style="margin-left :20px; margin-top:2px">
                            <asp:Button ID="btnSearchButton" runat="server" Text="查询"  CssClass="btn btn-primary  resetSize " /> 
                              </div> 
                           
                        </div>
                    </div>

               </div>

            </div>
        </div>
 

         <div class="mb10 table-operation" style="padding-left:6px">
                                                <input type="checkbox" id="sells1" class="allselect">
                                                <label for="sells1">全选</label>
                                                 <a  class="btn resetSize btn-danger"  onclick="BatchRefund()">批量退款</a>
                                                  </div>
         <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="2%"></th>
                                        <th width="6%">昵称</th>
                                        <th width="10%">手机号</th>
                                        <th width="15%">活动标题</th>
                                        <th width="7%">结束时间</th>
                                        <th width="6%">支付类型</th>
                                        <th width="4%">金额</th>
                                         <th width="8%">退款流水号</th>
                                         <th width="5%">退款状态</th>
                                        <th width="9%" style="text-align:center" id="actionTd" runat="server">操作</th>
                                    </tr>
                                </thead>
                                <tbody  class="tableRow">
                                   
                                    <asp:Repeater ID="Datalist" runat="server">
                                        <ItemTemplate>

                                            <tr>
                                       <td style="vertical-align:middle"><input name="CheckBoxGroup" class="fl" type="checkbox" PW='<%# Eval("PayWay") %>' IsRefund='<%# Eval("IsRefund") %>'  value='<%# Eval("Pid") %>'  /></td>
                                        <td >
                                             <%# Eval("UserName") %>
                                        </td>
                                        <td> <%# Eval("CellPhone") %></td>
                                        <td ><%# Eval("Title") %>
                                        </td>
                                        <td> <%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}").ToString().Replace(" ","<br>") %></td>
                                        <td ><%# Eval("PayWay") %></td>
                                        <td><%# Eval("TotalPrice","{0:F2}") %></td>
                                        <td ><%# Eval("RefundNum") %></td>
                                        <td ><%# Eval("ASate") %></td>
                                         <td >
                                            <%# Eval("ActionBtn") %>
                                        </td>
                                     </tr>

                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
</div>
          <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="20" ID="pager"  />
                                </div>
                            </div>
                        </div>
                    </div>

     </form>  


     <!--支付宝支付等待-->
        <div class="modal fade" id="PayWait">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left" id="batchTitle">支付宝退款中</h4>
                    </div>
                    <input type="hidden" id="AliPayRealName" value="" />
                    <input type="hidden" id="AliPayUser" value="" />
                    <div class="set-switch form-horizontal" style="padding: 20px; text-align: center">

                        <input type="button" class="btn btn-info" id="payError" value="　支付遇到问题　" />
                        <input type="button" id="paySuccessAll" class="btn btn-success" value="　　支付完成　　" />

                    </div>

                    <div style="clear: both">
                    </div>

                    <div class="modal-footer">
                    </div>

                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>



     <!--支付宝支付等待-->
        <div class="modal fade" id="PayWeixin">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left" id="batchTitle">微信批量退款中</h4>
                    </div>
        
                    <div class="set-switch form-horizontal" style="padding: 20px; text-align: center">

                        正在退款处理中.....
                       
                    </div>

                    <div style="clear: both">
                    </div>

                    <div class="modal-footer">
                    </div>

                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>


        <!-- /.modal -->
</asp:Content>
