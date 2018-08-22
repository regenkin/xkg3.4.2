<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ExpressComputerpes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.ExpressComputerpes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
      <Hi:Script ID="Script9" runat="server" Src="/utility/globals.js"/> <%--//调用到快速翻页函数--%>
     <style>
       .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
                    <h2>物流公司</h2>
    </div>

        <form runat="server" id="thisForm">
     <!--搜索-->
     
          <div  class="form-horizontal" style="margin:20px 0px">        
            <div class="form-inline">公司名称：
            <asp:TextBox ID="txtcompany" runat="server" CssClass="form-control resetSize" ></asp:TextBox>
              <span>快递100Code：</span><asp:TextBox ID="txtKuaidi100Code" runat="server" CssClass="form-control resetSize" ></asp:TextBox>
           <span>淘宝Code：</span><span><asp:TextBox ID="txtTaobaoCode" runat="server" CssClass="form-control resetSize" ></asp:TextBox>
            <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="btn resetSize btn-primary"/> 
            </div>                            
          </div>

          

     <!--分页功能-->
           <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <a href="javascript:void(0)" onclick="javascript:ShowAddSKUValueDiv()" class="btn btn-success">添加物流公司</a>
                    </div>

                  
        </div>

             <!--数据块-->
            <UI:Grid ID="grdExpresscomputors"  runat="server" ShowHeader="true" DataKeyNames="Name" SortOrderBy="CreateDate"  AutoGenerateColumns="false" GridLines="None" Width="100%" 
                 HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered" >
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="物流公司">
                        <ItemTemplate><%#Eval("Name") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="快递100Code">
                        <ItemTemplate><%# Eval("Kuaidi100Code")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="淘宝Code">
                        <ItemTemplate><%# Eval("TaobaoCode")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="95">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
                              <div style="display:<%# newCmpName(Eval("New").ToString())%>">
	                           <span class="submit_bianji"><a href="javascript:void(0)" onclick="ShowEditSKUValueDiv('编辑',this)" class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandName="Delete" CommandArgument=<%# Eval("Name") %>   IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
                              </div>
                           </ItemTemplate>
                    </asp:TemplateField>                                         
            </Columns>
        </UI:Grid>

         

      <!--数据列表底部功能区域--> 
<div class="modal fade"  id="previewshow">
  <div class="modal-dialog">
    <div class="modal-content form-horizontal"  id="hform">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="modaltitle" style="text-align:left">添加物流公司</h4>
      </div>
      <div class="modal-body" >


           <input type="hidden" id="hdcomputers" runat="server" />
         <div class="form-group">
            <label  class="col-xs-2 control-label" style="width:25%">公司名称：</label>
            <div class="col-xs-3" style="width:70%">
            <asp:TextBox ID="txtAddCmpName" CssClass="form-control" runat="server" />
            </div>
           </div>
         <div class="form-group">
            <label for="inputPassword3" class="col-xs-2 control-label" style="width:25%">快递100Code</label>
              <div class="col-xs-3" style="width:70%">
                 <asp:TextBox ID="txtAddKuaidi100Code" CssClass="form-control" runat="server" />
           </div>
          </div>
          <div class="form-group"> 
              <label for="inputPassword3" class="col-xs-2 control-label" style="width:25%">淘宝Code：</label>
             <div class="col-xs-3" style="width:70%">
                <asp:TextBox ID="txtAddTaobaoCode" CssClass="form-control" runat="server"></asp:TextBox>
           </div>
         </div>

      </div>
      <div class="modal-footer">
       <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="btn  btn-success"/>　
          <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
            
            
    </form>

    <script>


        function ShowAddSKUValueDiv(opers, strname, strKuaidi100Code, strTaobaoCode) {
            //arrytext = null;
            $("#modaltitle").html("添加物流公司");
            $('#ctl00_ContentPlaceHolder1_hdcomputers').val("");
            $('#ctl00_ContentPlaceHolder1_txtAddCmpName').val("");
            $('#ctl00_ContentPlaceHolder1_txtAddKuaidi100Code').val("");
            $('#ctl00_ContentPlaceHolder1_txtAddTaobaoCode').val("");
            $()
            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '170px'
            })
        }

        function ShowEditSKUValueDiv(opers, link_obj) {

            arrytext = null;
            var strname = $(link_obj).parents("tr").find("td").eq(0).text();
            var strKuaidi100Code = $(link_obj).parents("tr").find("td").eq(1).text();
            var strTaobaoCode = $(link_obj).parents("tr").find("td").eq(2).text();


           $("#modaltitle").html("修改物流公司");
            $('#ctl00_ContentPlaceHolder1_hdcomputers').val(strname);
            $('#ctl00_ContentPlaceHolder1_txtAddCmpName').val(strname);
            $('#ctl00_ContentPlaceHolder1_txtAddKuaidi100Code').val(strKuaidi100Code);
            $('#ctl00_ContentPlaceHolder1_txtAddTaobaoCode').val(strTaobaoCode);

            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });
           
        }

        $(function () {

            //验证方法'
           var vilidsetings = {
            'ctl00$ContentPlaceHolder1$txtAddCmpName': {
                validators: {
                    notEmpty: {
                        message: '物流公司名称不允许为空!'
                    },
                    stringLength: {
                        min: 2,
                        max: 20,
                        message: '物流公司名称2-20个字！'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtAddKuaidi100Code': {
                validators: {
                    notEmpty: {
                        message: '快递100Code不允许为空！'
                    },
                    stringLength: {
                        min: 2,
                        max: 20,
                        message: '快递100Code 2-20字符'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtAddTaobaoCode': {
                validators: {
                    notEmpty: {
                        message: '淘宝code不允许为空！'
                    },
                    stringLength: {
                        min: 2,
                        max: 20,
                        message: '淘宝code 2-20字符'
                    }
                }
            }

        };

        $('#hform').formvalidation(vilidsetings);//绑定验证方法


        });
       


        //层对象弹出
        function DialogShowNew() {
               
        }

    </script>
</asp:Content>