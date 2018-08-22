<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LimitedTimeDiscountList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.LimitedTimeDiscountList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style type="text/css">
          #ctl00_ContentPlaceHolder1_grdLimitedTimeDiscountList th {
              margin: 0px;
              border-left: 0px;
              border-right: 0px;
              background-color: #F7F7F7;
              text-align: center;
              vertical-align: middle;
          }

          #ctl00_ContentPlaceHolder1_grdLimitedTimeDiscountList td {
              margin: 0px;
              border-left: 0px;
              border-right: 0px;
              vertical-align: middle;
          }
      </style>
     <script type="text/javascript">
         $(document).ready(function () {
             //tip显示
             $("[data-toggle='tooltip']").tooltip({ html: false });
             var status = getUrlParam("status");
             $('#nav li').eq(status).siblings().removeClass('active').end().addClass('active');
         });

         ///暂停活动，恢复活动
         function ChangeStatus(status, id) {
             $.ajax({
                 type: "post",
                 url: "LimitedTimeDiscountHandler.ashx",
                 data: { id: id, status: status, action: "ChangeStatus" },
                 dataType: "json",
                 success: function (data) {
                     if (data.msg == "success") {
                         HiTipsShow("状态修改成功", "success", function () {
                             document.location.href = document.location.href;
                         });
                     }
                     else {
                         HiTipsShow(data.msg, "error");
                     }
                 },
                 error: function () {
                     HiTipsShow("访问服务器出错!", "error");
                 }
             });
         }

         function getUrlParam(name) {
             var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
             var r = window.location.search.substr(1).match(reg);  //匹配目标参数
             if (r != null) return unescape(r[2]); return null; //返回参数值
         }
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>限时折扣</h2>
        </div>
        <div class="blank" style="text-align: left;">
            <a href="EditLimitedTimeDiscount.aspx" class="btn btn-primary">添加限时折扣</a>
        </div>
        <div class="activediv" style="background-color: #fff">
            <div class="play-tabs">
                <div class="table-page">
                    <ul class="nav nav-tabs" role="tablist" id="nav">
                        <li role="presentation" class="active">
                            <a href="LimitedTimeDiscountList.aspx?status=0">所有活动(<asp:Label runat="server" ID="lblAll" Text="0"></asp:Label>)</a>
                        </li>
                        <li role="presentation">
                            <a href="LimitedTimeDiscountList.aspx?status=1">进行中(<asp:Label runat="server" ID="lblIn" Text="0"></asp:Label>)</a>
                        </li>
                        <li role="presentation">
                            <a href="LimitedTimeDiscountList.aspx?status=2">已结束(<asp:Label runat="server" ID="lblEnd" Text="0"></asp:Label>)</a>
                        </li>
                        <li role="presentation">
                            <a href="LimitedTimeDiscountList.aspx?status=3">未开始(<asp:Label runat="server" ID="lblUnBegin" Text="0"></asp:Label>)</a>
                        </li>
                    </ul>
                    <div class="page-box" style="margin-right: 15px;">
                        <div class="page fr">
                            <div class="form-group">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="hrefPageSize" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="set-switch" style="margin-top: 10px;">
                <div class="form-inline">
                    <asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txtActivityName" placeholder="活动名称" Width="110px"></asp:TextBox>
                    <Hi:DateTimePicker runat="server" ReadOnly="false" CssClass="form-control resetSize" ID="calendarStartDate" PlaceHolder="开始时间" Width="110" />
                    至
                    <Hi:DateTimePicker runat="server" CssClass="form-control resetSize mr20" ID="calendarEndDate" PlaceHolder="结束时间" Width="110" />
                    <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询" OnClick="btnSeach_Click" />
                </div>
            </div>
        </div>
        <div style="margin-top:5px;">
             <UI:Grid ID="grdLimitedTimeDiscountList"  runat="server" ShowHeader="true" AutoGenerateColumns="false"
                 DataKeyNames="LimitedTimeDiscountId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered" GridLines="None" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="活动名称" SortExpression="ActivityName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                         <ItemStyle Width="120" />
                        <ItemTemplate>
                             <%# Eval("ActivityName")%><br />
                             <div data-toggle="tooltip" data-placement="left">
                                 <%# GetDescription(Eval("Description").ToString()) %>
                             </div> 
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="活动时间" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="180" />
                        <ItemTemplate>
                            自<%# Eval("BeginTime","{0:yyyy-MM-dd HH:mm:ss}")%><br />至<%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}")%> 
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="优惠对象" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="150" />
                        <ItemTemplate>
                            <%--<div data-toggle="tooltip" data-placement="left" title="<%# GetMemberGarde( Eval("ApplyMembers").ToString(),Eval("DefualtGroup").ToString(),Eval("CustomGroup").ToString())%>">--%>
                            <div data-toggle="tooltip" data-placement="left"  >
                                  <%# GetMemberGarde( Eval("ApplyMembers").ToString(),Eval("DefualtGroup").ToString(),Eval("CustomGroup").ToString()).Length>30? GetMemberGarde( Eval("ApplyMembers").ToString(),Eval("DefualtGroup").ToString(),Eval("CustomGroup").ToString()).Substring(0,28)+"..":GetMemberGarde( Eval("ApplyMembers").ToString(),Eval("DefualtGroup").ToString(),Eval("CustomGroup").ToString())%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="活动商品" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="100" />
                        <ItemTemplate>
                            已加入<%# Eval("productCount")%> 
                        </ItemTemplate>
                    </asp:TemplateField>
                                                                                                
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="230" />
                        <ItemTemplate>
                            <span >
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/EditLimitedTimeDiscount.aspx?id={0}", Eval("LimitedTimeDiscountId")))%>')" >编辑</a>
                            </span>|
                             <span >
                                 <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete"  CssClass="btnLink pad"  OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" /> 
                            </span><br />
                            <span style="margin-top:20px">
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/LimitedTimeDiscountProduct.aspx?id={0}", Eval("LimitedTimeDiscountId")))%>')" >修改商品</a>
                            </span><br />
                            <span>
                                <a href="javascript:ChangeStatus('<%# Eval("Status").ToString() %>','<%# Eval("LimitedTimeDiscountId").ToString() %>');" ><%# GetStatusHtml( Eval("Status").ToString()) %></a>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </ui:grid>
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" style="width: auto">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
            </div>                        
        </div>
    </form>
</asp:Content>
