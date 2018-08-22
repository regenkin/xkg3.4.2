<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Shippers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.Shippers" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="page-header">
                    <h2>物流地址管理</h2>
    </div>
 <!--分页功能-->
           <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <a id="preview" class="btn btn-success">新增地址</a>
                    </div>

                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server"  ShowTotalPages="true" ID="pager" />
                      </div>
                    </div>
                    </div>
        </div>


               
             <form class="form-horizontal" id="thisForm" runat="server" >
       <div >
          
           <asp:Repeater ID="ShipperList"  runat="server" >
               <ItemTemplate>
           <div class="shipperBox">

               <div class="shipperTitle">
                   <a Class="shipperType" style="display:none" ><%#Eval("ShipperTag") %></a>
              
                <%#Eval("ShipperName") %> 
               </div>
               <div  class="shipperAddr"> <%# Hidistro.Entities.RegionHelper.GetFullRegion(int.Parse(Eval("RegionId").ToString()),"　") %><br /><%#Eval("Address") %> </div>
               <div  class="shipperFooter">
               <div class="shipperBtn">
                   <asp:LinkButton ID="LinkButton1"   CssClass="btn btn-info btn-xs" OnClick="EditShiper"  runat="server"  CommandArgument='<%#Eval("ShipperId") %>'  >修改</asp:LinkButton>
                   <asp:Button  ID="btnDelete" OnClick="DeleteShiper_Click"  CssClass="btn btn-danger btn-xs"  OnClientClick="return HiConform('<strong>确定要删除选择的地址吗？</strong><p>删除地址不可恢复！</p>',this)" runat="server" CommandName="Delete"    CommandArgument='<%#Eval("ShipperId") %>'  Text="删除"></asp:Button> 
                   <%-- <asp:Button ID="LBtnDel" runat="server" Text="删除" IsShow="true"   CssClass="btn btn-danger btn-xs"  CommandArgument='<%#Eval("ShipperId") %>'     OnClientClick="return HiConform('<strong>确定要删除选择的地址吗？</strong><p>删除地址不可恢复！</p>',this)" ToolTip="" /> --%>
               </div>
                   <%#Eval("TelPhone") %></div>

           </div>

           </ItemTemplate>
           </asp:Repeater>

          

           <div class="clearfix"></div>
       </div>


  






        


 <%--   <form runat="server"  id="thisForm">--%>
     <!--弹出窗口-->
     <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <div class="modal-title">
                            <asp:Literal ID="editType" runat="server" Text="新增地址" ></asp:Literal>
                        </div>
                    </div>
                    <div class="modal-body">

                     <asp:HiddenField ID="Task" Value="New" runat="server" />
                     <asp:HiddenField ID="ShipperId" Value="" runat="server" />

                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-3 control-label"><em>*</em>地址类型：</label>
                        <div class="col-xs-6">
                            <asp:CheckBoxList ID="txtShipperType" RepeatDirection="Horizontal"  RepeatLayout="Flow"   CssClass="form-control"  runat="server" >
                            <asp:ListItem Value="1"　style="margin-right:20px">　默认发货地址</asp:ListItem>　
                            <asp:ListItem Value="2">　默认退货地址</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-3 control-label"><em>*</em>发货人：</label>
                        <div class="col-xs-6">
                             <asp:TextBox ID="txtShipperName" CssClass="form-control" runat="server" />
                        </div>
                    </div>

            
                  <div class="form-group">
                        <label for="inputPassword3" class="col-xs-3 control-label"><em>*</em>发货地址：</label>
                        <div class="col-xs-9">
                            <Hi:RegionSelector runat="server" CssClass="Regions"  ID="ddlReggion" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-3 control-label"><em>*</em>详细地址：</label>
                        <div class="col-xs-6">
                            <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" />
                        </div>
                    </div>


                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-3 control-label"><em>*</em>联系电话：</label>
                        <div class="col-xs-6">
                            <asp:TextBox ID="txtTelPhone" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                         
                  <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10" style="padding-left:70px">
                             <asp:Button ID="btnSave" runat="server"   Text="　保存　"  CssClass="btn btn-success"    />
                             
                        </div>
                    </div>

               
                    </div>
                    <div class="modal-footer" style="padding-right:50px">
                        <%--<button type="button" onclick="MdPrint()" class="btn btn-primary" >确定</button>
                        <button type="button" class="btn btn-info" data-dismiss="modal">关闭</button>--%>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal-dialog -->
        </div>
    <!-- /.弹出END -->

     
     <script>

         $(function () {
             $('#preview').click(function () {

                
                 //清空相关数据，
                 $("#ctl00_ContentPlaceHolder1_Task").val("ADD");
                 $("#ctl00_ContentPlaceHolder1_ShipperId").val("");
                 $("#ctl00_ContentPlaceHolder1_txtAddress").val("");
                 $("#ctl00_ContentPlaceHolder1_txtTelPhone").val("");
                 $("#ctl00_ContentPlaceHolder1_txtShipperName").val("");
                 $("#ddlRegions3").val("");
                 $("#ddlRegions2").val("");
                 $("#ddlRegions1").val("");

                 $("#ctl00_ContentPlaceHolder1_txtShipperType_0").attr("checked", false);
                 $("#ctl00_ContentPlaceHolder1_txtShipperType_1").attr("checked", false);

                 $('#previewshow').modal('toggle').children().css({
                     width: '600px'

                 })
             });


             $(".shipperType").each(function () {
                 var t = $(this).text().trim();
                 if (t == "1") {
                     $(this).html("默认发货地址");
                     $(this).show();
                 }
                 else if (t == "2")
                 {
                     $(this).html("默认退货地址");
                     $(this).show();
                 }
                 else if (t == "3") {
                     $(this).html("默认发货/退货地址");
                     $(this).show();
                 }

             });

             $(".shipperBox").hover(function () {
                // alert($(this).find(".shipperBtn").length);
                 $(this).find(".shipperBtn").slideDown(200);
                // 

             }, function () {

                 $(this).find(".shipperBtn").slideUp(200);

             });



             $('#aspnetForm').formvalidation({
                 'ddlRegions2': {
                     validators: {
                         notEmpty: {
                             message: '地区未选择'
                         },
                         stringLength: {
                             min: 1,
                             max: 10,
                             message: '地区未选择'
                         }
                     }
                 },
                 'ctl00$ContentPlaceHolder1$txtShipperType': {
                     validators: {
                         notEmpty: {
                             message: '请选择地址类型'
                         },
                         stringLength: {
                             min: 1,
                             max: 10,
                             message: '请选择地址类型'
                         }
                     }
                 },
                 'ctl00$ContentPlaceHolder1$txtTelPhone': {
                     validators: {
                         notEmpty: {
                             message: '联系电话请填写'
                         },
                         tell: {
                             message: '请填写正确的手机或座机号码'
                         }
                     }
                 },
                 'ctl00$ContentPlaceHolder1$txtAddress': {
                     validators: {
                         notEmpty: {
                             message: '请填写详细地址，10字以上'
                         },
                         stringLength: {
                             min: 10,
                             max: 100,
                             message: '详细地址，10-100字！'
                         }
                     }
                 },
                 'ctl00$ContentPlaceHolder1$txtShipperName': {
                     validators: {
                         notEmpty: {
                             message: '发货人请填写，2-20个字符！'
                         },
                         stringLength: {
                             min: 2,
                             max: 20,
                             message: '发货人请填写，2-20个字符！'
                         }
                     }
                 }

             });




             if ($("#ctl00_ContentPlaceHolder1_Task").val() == "EDIT") {
                 $('#previewshow').modal('toggle').children().css({
                     width: '600px',
                     top: '170px'
                 });
             }
             
         });



        

        // ddlRegions3
     </script>
     </form>
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />

<style>
 .Regions select{height:34px;margin-right:3px;
  font-size: 14px;
  line-height: 1.42857143;
  color: #555;
  background-color: #fff;
  background-image: none;
  border: 1px solid #ccc;
  border-radius: 4px;
  -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
  box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
  -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
  -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
  transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
 }

.shipperBox{border:1px solid #aaa;width:300px;height:160px;float:left;margin-right:10px;margin-top:10px}
.shipperBox:hover{border:1px solid #9e14f2;} 
.shipperTitle{background:url(../images/userheader.jpg) 10px 50%  no-repeat;border:1px solid #ddd;line-height:40px;padding-left:30px;}
.shipperAddr{background:url(../images/cpositon.jpg) 10px 10px no-repeat;border-bottom:1px solid #ddd;line-height:20px;height:80px;padding:10px 30px}
 .shipperType{cursor:pointer;margin-top:-1px;margin-right:5px;font-size:12px;line-height:20px;float:right;padding:1px 10px;background:#c13ff5;color:#fff;border-bottom-right-radius:9px;border-bottom-left-radius:9px}
.shipperFooter{background:url(../images/phoneconn.jpg)  10px 50%   no-repeat;line-height:40px;padding-left:30px}
.shipperBtn{line-height:20px;float:right;padding:7px 15px 7px 7px;display:none}
shipperType:hover{color:#fff;background:#0f7ef7}

    </style>
</asp:Content>