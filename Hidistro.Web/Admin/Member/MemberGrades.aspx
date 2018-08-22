<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberGrades.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.MemberGrades" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .thCss{text-align:center;}
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>会员等级管理</h2>  
        </div>
        <div style="margin-bottom:10px;"> <a href="/Admin/member/AddMemberGrade.aspx" class="btn resetSize btn-primary">添加会员等级</a></div>
	  <!--数据列表区域-->
      <div id="datalist">
       	   
          
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th style="text-align:left">等级名称</th>
                                <th>会员人数</th>
                                <th>满足交易额</th>
                                <th>满足交易次数</th>
                                <th>会员折扣</th>
                                <th>&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Convert.ToBoolean(Eval("IsDefault")) == true ? Eval("Name")+" <span style=\"color:blue;\">(默认等级)" +"</span>" : Eval("Name")%></td>
                        <td style="text-align:center"><%# SelectUserCountGrades(Convert.ToInt32( Eval("GradeId"))) %></td>
                        <td style="text-align:center"><%# "￥"+Eval("TranVol","{0:F2}") %></td>
                        <td style="text-align:center"><%#Eval("TranTimes") %></td>
                        <td style="text-align:center"><%# Eval("Discount").ToString()!="100"?"现价×"+Eval("Discount")+"%":"-" %></td>
                        <td><asp:HyperLink ID="lkbViewAttribute" runat="server" CssClass="mr5" Text="编辑" NavigateUrl='<%# "AddMemberGrade.aspx?id="+Eval("GradeId")%>' ></asp:HyperLink><asp:Label ID="lblSplit" runat="server" Text="|"></asp:Label><asp:Button ID="btnDel" CssClass="btnLink" runat="server" Text="删除" CommandName="DeleteGrade" CommandArgument='<%#Eval("gradeId") %>' OnClientClick="return HiConform('<strong>确定要执行该删除操作吗？</strong><p>删除后将不可以恢复！</p>',this)" /></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
	</table>
                </FooterTemplate>
            </asp:Repeater>

        </div>
       </form>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#datalist').find('th').each(function () {
                $(this).addClass('thCss');
            });
        })
    </script>
</asp:Content>

