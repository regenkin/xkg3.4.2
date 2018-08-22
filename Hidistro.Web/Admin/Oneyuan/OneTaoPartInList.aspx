<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="OneTaoPartInList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.OneTaoPartInList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" TagName="ViewTab" Src="~/Admin/Oneyuan/OneTaoViewTab.ascx" %> 
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        .tableRow{border:1px solid #ccc}
        .tableRow tr td{padding-left:5px!important;vertical-align:middle!important}
        .tableRow tr:hover{background:#def3e1}
        .table th{text-align:left!important;padding-left:5px!important}
    </style>
    <script>

        $(function () {

            $(".ShowPrizeNums").click(function () {

                $next = $(this).next();
                HiTipsShow($next.html().trim(), "confirmII",function(){},"夺宝号码");
            });

        })

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
        <h2 id="txtEditInfo" runat="server">查看一元夺宝
        </h2>
    </div>
    <Hi:ViewTab ID="ViewTab1"  runat="server"></Hi:ViewTab>
    <form runat="server">
         <div class="set-switch">   
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop4">会员昵称：</label>
                                <asp:TextBox ID="txtUserName" CssClass="form-control  resetSize  inputw150" runat="server" />
                            </div>
                            <div class="form-group mr20">
                              
                                <asp:CheckBox ID="ShowIsPrize" runat="server" style="vertical-align:middle" OnCheckedChanged="ShowIsPrize_CheckedChanged1" AutoPostBack="true"  />
                                 <label for="ShowIsPrize"> 只显示中奖会员
                                </label>
                            </div>
                       
                             <div class="form-group" style="margin-left :30px">
                            <asp:Button ID="btnSearchButton" runat="server" Text="查询"  CssClass="btn btn-primary  resetSize " /> 
         
                            </div>
                        </div>
         </div>

     <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="5%"></th>
                                        <th width="8%">会员昵称</th>
                                        <th width="6%">会员等级</th>
                                        <th width="8%">联系电话</th>
                                        <th width="5%">购买份数</th>
                                        <th width="15%">参与时间</th>
                                        <th width="10%">中奖号码</th>
                                        <th width="10%" style="text-align:center">操作</th>
                                    </tr>
                                </thead>
                                <tbody  class="tableRow">
                                   
                                    <asp:Repeater ID="Datalist" runat="server" OnItemDataBound="rptypelist_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                        <td >
                                           <Hi:ListImage ID="ListImage1" runat="server" DataField="UserHead"  Width="50" Height="50"/>
                                        </td>
                                        <td><%# Eval("UserName") %></td>
                                        <td><%# Eval("Name") %></td>
                                        <td ><%# Eval("CellPhone") %></td>
                                        <td><%# Eval("BuyNum") %></td>
                                        <td ><%# Eval("BuyTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                        <td><asp:Literal ID="PrizeNum" runat="server" /></td>
                                      
                                         <td >
                                            <a class="btn btn-primary btn-xs ShowPrizeNums"  >夺宝号码</a>
                                             <span style="display:none">
                                                 <asp:Literal ID="AllPrizeNum" runat="server" /> 
                                             </span>
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
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager"  />
                                </div>
                            </div>
                        </div>
                    </div>
        </form>
</asp:Content>
