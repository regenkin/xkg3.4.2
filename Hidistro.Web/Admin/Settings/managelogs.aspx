<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageLogs.aspx.cs" Inherits="Hidistro.UI.Web.Admin.settings.ManageLogs" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
       <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
      <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <Hi:Script ID="Script9" runat="server" Src="/utility/globals.js"/> <%--//调用到快速翻页函数--%>
    <style>
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
                    <h2>操作日志</h2>
                    <small>查看各管理员的历史操作记录</small>
  
    </div>


    <form runat="server" id="thisForm">

         <!--搜索-->
        
        <div class="set-switch">
                    <div class="form-horizontal clearfix">
                        <div class="form-group setmargin">
                            <label class="col-xs-1 pad control-label resetSize" for="pausername">操作人：</label>
                            <div class="form-inline">
                                <Hi:LogsUserNameDropDownList ID="dropOperationUserNames" CssClass="form-control resetSize" runat="server" Width="152"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-1 pad control-label resetSize" for="setdate">操作时间：</label>
                            <div class="form-inline journal-query">
                               <div class="form-group">
                                 <%--  <asp:TextBox ID="calFromDate" CssClass="form-control resetSize inputw150" runat="server"  placeholder="创建日期" />--%>
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calFromDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;至&nbsp;
                                    <Hi:DateTimePicker CalendarType="EndDate" ID="calToDate" runat="server" CssClass="form-control resetSize inputw150" />
                                  <%-- <asp:TextBox ID="calToDate" CssClass="form-control resetSize input-ssm start_datetime1" runat="server"  placeholder="创建日期" />--%>
                                </div>
                                <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" />
                                <div class="form-group">
                                    <label for="exampleInputName2">快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click" />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>





         <!--批量操作块,清空日志，页码-->
              <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <div class="form-group mar forced">
                            <div class="checkbox">
                                <label><input type="checkbox" onclick="SelectAllNew(this)">全选</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnDel" runat="server" CssClass="btn resetSize btn-danger" Text="批量删除" OnClick="btnDel_Click" OnClientClick="return HiConform('<strong>删除后将不可恢复！</strong><p>确定要批量删除所选择的日志记录吗？</p>',this);" />
                                <asp:Button ID="btnClear" runat="server" CssClass="btn resetSize btn-warning" Text="清空日志" OnClick="btnClear_Click" OnClientClick="return HiConform('<strong>清空后将不可恢复！</strong><p>确定要清空所有操作日志吗？</p>',this);" />
                            </div>
                        </div>
                    </div>
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="PageSize1"  />
                        </div>
                    </div>
                </div>

      
     

         <!--数据列表-->
     <asp:Repeater ID="dlstLog1"  runat="server" >
         
         <HeaderTemplate>
             <div class="journal-tab">
             <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <thead>
                            <tr>
                                <th width="100">用户</th>
                                <th  width="150">IP</th>
                                <th>详情</th>
                                <th  width="200">操作时间</th>
                                <th  width="100">操作</th>
                            </tr>
                        </thead>
                        <tbody>
         </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
          <td  style="text-align:left">
  
          <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("LogId") %>'>　
              <asp:Literal runat="server" ID="litUserName" Text='<%#Eval("UserName")%>' /></td>
          <td align="center"><asp:Literal runat="server" ID="litIp" Text=' <%#Eval("IPAddress")%>' /></td>
          <td  style="text-align:left;word-wrap:break-word;"><div>
           <span style="display:block">页面地址：<asp:Literal runat="server" ID="litPageUrl" Text=' <%#Eval("PageUrl")%>' /></span>
          日志的详细描述：<abbr class="colorG"><asp:Literal runat="server" ID="litDescription" Text=' <%#Eval("Description")%>' /></abbr>
              </div>
              </td>
          <td align="center"><Hi:FormatedTimeLabel runat="server" ID="time" Time='<%# Eval("AddedTime")%>' /></td>
          <td align="center">
              <asp:Button ID="btnDelOne" runat="server" OnClientClick="return HiConform('<strong>删除后将不可恢复！</strong><p>确定要删除该条日志吗？</p>',this);" CssClass="btnLink"  CommandArgument='<%#Eval("LogId") %>' OnClick="DeleteLog" Text="删除" />
          </td>
        </tr>
     </ItemTemplate>
     <FooterTemplate>
         </tbody>
     </table>
     </div>
     </FooterTemplate>
     </asp:Repeater>

         <!--数据列表底部功能区域-->
  
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <div class="form-group mar forced">
                            <div class="checkbox">
                                <label><input type="checkbox" name="selall"   onclick="javascript: SelectAllNew(this);">全选</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <input value="批量删除" onclick="$('#ctl00_ContentPlaceHolder1_btnDel').click()" class="btn resetSize btn-danger" type="button">
                            </div>
                        </div>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>
    </form>

    <script>

        function SelectAllNew(obj) {
            $("[name=CheckBoxGroup]").prop("checked", $(obj).get(0).checked);
            //.attr("checked", $(obj).get(0).checked);//这种方式会有异常，只能执行一次，
        }

        $(function () {
            $(".start_datetime1").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm-dd',
                autoclose: true,
                weekStart: 1,
                minView: 2
            });


        });


    </script>

</asp:Content>
