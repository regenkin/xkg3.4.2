<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="CustomDistributorStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.CustomDistributorStatistics" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="~/hieditor/ueditor/controls/ucUeditor.ascx" tagname="KindeditorControl" tagprefix="Kindeditor" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.allselect').change(function () {
                $('.table-bordered input[type="checkbox"]').prop('checked', $(this)[0].checked);
            });

            //添加数据验证
            $("#addCustomDistributorStatistic").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_btnSaveComm',
                'ctl00$ContentPlaceHolder1$txtStoreName': {
                    validators: {
                        notEmpty: {
                            message: '店铺名称不能为空'
                        },
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '店铺名称2-20个字符'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtOrderNum': {
                    validators: {
                        notEmpty: {
                            message: '销售订单数不能为空'
                        },
                        regexp: {
                            regexp: /^\d{1,6}$/,
                            message: '请填写正确的销售订单数！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtCommTotal': {
                    validators: {
                        notEmpty: {
                            message: '佣金不能为空'
                        },
                        regexp: {
                            regexp: /^[0-9]+(\.[0-9]+)?$/,
                            message: '佣金只能输入整数型数值'
                        }
                    }
                }
            });
        });

        function UpdateRow(orderId)
        {
            $('#addCustomDistributorStatistic').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#addCustomDistributorStatistic").modal({ show: true });
            $("#ctl00_ContentPlaceHolder1_hiddid").val(orderId);
            
            $.ajax({
                url: "/API/VshopProcess.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: {
                    action: "GetCustomDistributorStatistic",
                    orderId: orderId,
                },
                success: function (resultData) {
                    if (resultData.success) {
                        $("#ctl00_ContentPlaceHolder1_hiddid").val(orderId);
                        $("#uploader1_uploadedImageUrl").val(resultData.logo);
                        var imghtml="<img src='"+resultData.logo+"' id='uploader1_image' style='width: 50px; height: 22.7059px; background-color: rgb(255, 255, 255);'>";
                        $("#uploader1_preview").append(imghtml);
                        $("#ctl00_ContentPlaceHolder1_txtStoreName").val(resultData.storename);
                        $("#ctl00_ContentPlaceHolder1_txtOrderNum").val(resultData.ordernums);
                        $("#ctl00_ContentPlaceHolder1_txtCommTotal").val(resultData.commtotalsum);
                    }
                    else {
                        alert_h("加载失败");
                    }
                }
            });
        }

        function AddNewRow() {
            $("#ctl00_ContentPlaceHolder1_txtStoreName").val();
            $("#ctl00_ContentPlaceHolder1_txtOrderNum").val();
            $("#ctl00_ContentPlaceHolder1_txtCommTotal").val();
            $('#addCustomDistributorStatistic').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#addCustomDistributorStatistic").modal({ show: true });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" >
    <div class="page-header">
        <h2>自定义排行榜<small style="display:inline;margin-left:10px">设置虚拟的分销店铺销售、分佣数据，仅参与分销商排行，不会影响其他统计数据</small></h2>
    </div>
    <input type="button" class="btn btn-success" onclick="AddNewRow()" value="添加分销商排名">
    <br />
    <br />    
           <table style="border-collapse:collapse;" class="table table-hover mar table-bordered" cellspacing="0"  border="0">
               <thead>
                          <tr class="table_title">
                              <th style="width:80px"><input type="checkbox" id="checkBoxGroupAll" class="allselect"/></th><th style="width:120px">店铺logo</th><th style="width:240px">店铺名称</th><th style="width:240px">销售订单数</th><th style="width:150px">佣金</th><th>操作</th>
                          </tr>
               </thead>
          </table>
           <table style="border-collapse:collapse;" id="tableCustom" class="table table-hover mar table-bordered" cellspacing="0"  border="0">
            <tbody>
              <asp:Repeater ID="repCustomDistributorStatisticList" runat="server" OnItemCommand="repCustomDistributorStatisticList_ItemCommand"  >
                  <ItemTemplate>
                              <tr class="table_title">
                                  <td style="width:80px"> <input type="checkbox"  name="CheckBoxGroup" value='<%# DataBinder.Eval(Container.DataItem, "Id")%>'/>  </td>
                                  <td style="width:120px"> <img style="width:100px;height:60px" src="<%# DataBinder.Eval(Container.DataItem, "logo")%>"  /></td>
                                  <td style="width:240px"> <%# DataBinder.Eval(Container.DataItem, "storename")%> </td>
                                  <td style="width:240px"> <%# DataBinder.Eval(Container.DataItem, "ordernums")%> </td>
                                  <td style="width:150px"> <%# DataBinder.Eval(Container.DataItem, "commtotalsum","{0:f2}")%> </td>
                                  <td>  
                                      <input type="button" id="btnUpdate" class="btnLink pad" value="编辑" onclick="UpdateRow('<%# DataBinder.Eval(Container.DataItem, "Id")%>')" />
                                     <%-- <asp:Button runat="server" Text="编辑" class="btnLink pad" CommandName="Edit"   CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' IsShow="true" /> --%>
                                      <asp:Button runat="server" Text="删除" class="btnLink pad" CommandName="Delete"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'   /> 
                                  </td>
                              </tr>
                  </ItemTemplate>
                 </asp:Repeater>
              </tbody>
           </table>
           <br />
           <asp:Button ID="lkbDelectSelect" OnClick="lkbDelectSelect_Click" runat="server" Text="批量删除" class="btn btn-success"   style="background-color: #286090;border-color: #204d74;"  
                IsShow="true" OnClientClick="return HiConform('<strong>确认删除选择的分销商排名！</strong><p>是否继续？</p>', this);" /> 
           <!--添加排行榜--->
           <div class="modal fade" id="addCustomDistributorStatistic" >
                <div class="modal-dialog">
                    <div class="modal-content form-horizontal">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">添加分销商排名</h4>
                        </div>
                        <div class="modal-body form-horizontal">
                                 <div class="form-group"> <label for="inputEmail3" class="col-xs-4 control-label">店铺logo: </label>
                                      <div class="col-xs-6">
                                            <Hi:UpImg runat="server" ID="uploader1" IsNeedThumbnail="false" UploadType="brand"  />
                                            <label style="color:#808080;font-size:12px;">建议尺寸：650 x 200 像素，<br />小于300KB，支持.jpg、.gif、.png格式</label> 
                                      </div>
                                </div>
                                <div class="form-group"><label for="inputEmail3" class="col-xs-4 control-label"> 店铺名称: </label> <div class="col-xs-6"><asp:TextBox ID="txtStoreName" MaxLength="20" runat="server" CssClass="form-control resetSize inputw150" Width="243" /> </div>
                                </div>
                                <div class="form-group"><label for="inputEmail3" class="col-xs-4 control-label">销售订单数: </label> <div class="col-xs-6"><asp:TextBox ID="txtOrderNum" runat="server" MaxLength="6" CssClass="form-control resetSize inputw150" Width="243" /> </div>
                                </div>
                                <div class="form-group"> <label for="inputEmail3" class="col-xs-4 control-label">佣金:</label> <div class="col-xs-6"><asp:TextBox ID="txtCommTotal" runat="server" MaxLength="8" CssClass="form-control resetSize inputw150" Width="243" /></div>  </div>
                        </div>
                        <div class="modal-footer">
                            <asp:HiddenField runat="server" ID="hiddid" />
                            <asp:Button runat="server" ID="btnSaveComm" CssClass="btn btn-primary" Text="保存"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        </div>
                    </div>
                </div>
            </div>
       </form>
</asp:Content>