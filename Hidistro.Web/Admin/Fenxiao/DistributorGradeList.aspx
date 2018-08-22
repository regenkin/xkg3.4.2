<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorGradeList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorGradeList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .DefaultCss {color:#0094ff
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
            <h2>分销商等级管理</h2>
   </div>
     <div class="form-horizontal sell-table">
                        <a href="EditDistributorGrade.aspx" class="btn btn-info">添加分销商等级</a>
     </div>
<form runat="server">

  <div class="sell-table" style="margin-top:20px">
    <table class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>等级名称</th>
                            <th>分销商人数</th>
                            <th>佣金满足点</th>
                            <th>成交店铺佣金奖励</th>
                            <th>上一级佣金奖励</th>
                            <th>上二级佣金奖励</th>
                            <th>操作 　</th>
                        </tr>
                    </thead>
                    <tbody>
                       <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                    <ItemTemplate>
                            <tr>
                                <td>
                                    &nbsp;<%# Eval("Name")%><%# Eval("IsDefault").ToString()=="True"?"<span class='DefaultCss'>（默认等级）</span>":"" %></td>
                                <td style="text-align:center" >
                               <asp:Literal ID="GradeSum" Text="300" runat="server"></asp:Literal>
                               </td>
                                <td>&nbsp;￥<%# Eval("CommissionsLimit", "{0:F2}")%></td>
                                <td><%#FormatCommissionRise(Eval("FirstCommissionRise"))%></td>
                                <td><%#FormatCommissionRise(Eval("SecondCommissionRise"))%></td>
                                <td><%#FormatCommissionRise(Eval("ThirdCommissionRise"))%></td>
                             <%--   <td><%#Eval("Description")%>&nbsp;</td>--%>
                                <td width="188">
                                    <span class="submit_bianji"><a style="cursor:pointer" href="EditDistributorGrade.aspx?id=<%# Eval("GradeId")%>&reurl=<%=LocalUrl %>">
                                        编辑</a></span>                                        
                                        <span class="submit_shanchu">
                                           <%-- <asp:LinkButton ID="lbtnDel" runat="server" CommandArgument='<%#Eval("gradeid") %>' CommandName="del">删除</asp:LinkButton> --%>
                                             <asp:Button ID="lbtnDel" CssClass="btnLink" CommandArgument='<%#Eval("gradeid") %>'  runat="server" Text="删除" CommandName="del"  OnClientClick="return HiConform('<strong>确定要删除选择的分销商等级吗？</strong><p>删除分销商等级不可恢复！</p>',this)" />
                                        </span>
                                </td>
                            </tr>
                    </ItemTemplate>
                     

                        
                </asp:Repeater>
                    </tbody>
                </table>
      
                             <div style="margin-left:10px;" >  
                                 <small style="color:red;">
                                     说明：分销商佣金奖励，指的是分销商在享受商品佣金的基础上，额外增加的佣金比例，最终分销商享受的佣金百分比=商品佣金百分比+佣金奖励百分比。
                                  </small>

                             </div>
    </div>
</form>
</asp:Content>
