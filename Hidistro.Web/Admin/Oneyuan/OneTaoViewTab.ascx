<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OneTaoViewTab.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.OneTaoViewTab" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<div id="mytabl" runat="server" ClientIDMode="Static"  style="margin-top:5px;">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li  id="LiViewTab1" ClientIDMode="Static" runat="server" ><a href="AddOneyuanInfo.aspx" id="ViewTab1" ClientIDMode="Static" runat="server">活动设置</a></li>
                    <li  id="LiViewTab2" ClientIDMode="Static" runat="server"><a href="OneTaoResult.aspx"  id="ViewTab2" ClientIDMode="Static" runat="server">中奖结果</a></li>
                    <li  id="LiViewTab3" ClientIDMode="Static" runat="server"><a href="OneTaoPartInList.aspx"  id="ViewTab3" ClientIDMode="Static" runat="server">参与记录</a></li>
                </ul>

                 <div class="page-box" id="pageSizeSet" runat="server">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
</div>
