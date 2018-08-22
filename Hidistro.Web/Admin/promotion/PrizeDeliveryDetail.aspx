<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="PrizeDeliveryDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.PrizeDeliveryDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rows_title {
            background: #f2f2f2;
        }

        .Regions, .tdleft {
            text-align: left !important;
            padding-left: 15px!important;
        }

        .borderleft {
            border-left: 1px solid #ddd;
        }

        .pinfo {
            margin: 5px 20px;
            color: #4896f6;
        }

        .content-table .table tr {
            border: 1px solid #ddd;
        }

        .jpname {
            color: #777;
        }

        .Dstatus .btn {
            margin-bottom: 5px;
        }

        #ddlRegions1, #ddlRegions2 {
            margin-right: 3px;
        }

        .splittr {
            border: 0px !important;
        }

        .addwauto {
            width: auto !important;
            display: inline-block !important;
        }
        p{line-height:30px}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>奖品详情</h2>
    </div>


    <div class="content-table">
        <table class="table" style="table-layout: fixed">
            <tbody>
                <tr class="rows_title">
                    <th class="tdleft" width="25%">
                                            商品信息
                    </th>
                    <th class="tdleft" width="20%">活动名称
                    </th>
                    <th width="10%">中奖时间
                    </th>
                    <th width="10%">收货人</th>
                    <th width="15%">奖品</th>
                </tr>
                <tr>
                    <td class="tdleft">
                        <div class="img fl mr10">
                            <Hi:ListImage ID="txtImage" runat="server" ImageUrl="/utility/pics/none.gif" Width="60" Height="60" />
                        </div>
                        <div class="pinfo">
                            <asp:Literal ID="txtProductName" runat="server"></asp:Literal>
                        </div>
                    </td>
                    <td class="tdleft"><asp:Literal ID="txtGameTitle" runat="server"></asp:Literal><br />
                        <span class="jpname">[<asp:Literal ID="txtGameType" runat="server"></asp:Literal>]</span>
                    </td>
                    <td><asp:Literal ID="txtPlayTime" runat="server"></asp:Literal>
                    </td>
                    <td class="borderleft"><asp:Literal ID="txtDeliever" runat="server"></asp:Literal><br />
                        <asp:Literal ID="txtDTel" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="txtPrizeGrade" runat="server"></asp:Literal>
                    </td>

                </tr>
                <tr class="splittr">
                    <td colspan="5"></td>
                </tr>

                <tr class="rows_title">
                    <th class="tdleft" colspan="5">收货信息
                    </th>
                </tr>
                <tr>
                    <td class="tdleft" colspan="5">
                        <p>选择所在地：<asp:Literal ID="txtRegionName" runat="server"></asp:Literal></p>
                        <p>详细地址：<asp:Literal ID="txtAddress" runat="server"></asp:Literal></p>
                        <p>收货人姓名：<asp:Literal ID="txtReceiver" runat="server"></asp:Literal></p>
                        <p>联系电话：<asp:Literal ID="txtTel" runat="server"></asp:Literal></p>
                    </td>
                </tr>


                <tr class="splittr">
                    <td colspan="5"></td>
                </tr>

                <tr class="rows_title">
                    <th class="tdleft" colspan="5">物流信息
                    </th>
                </tr>
                <tr>
                    <td class="tdleft" colspan="5">&nbsp;&nbsp;
                         <p>物流公司：<asp:Literal ID="txtExpressName" runat="server"></asp:Literal></p>
                        <p>快递单号：<asp:Literal ID="txtCourierNumber" runat="server"></asp:Literal></p>
                    </td>
                </tr>

                <tr class="splittr">
                    <td colspan="5"></td>
                </tr>

                <tr class="rows_title">
                    <td colspan="5" class="tdleft">
                        当前状态：<asp:Literal ID="txtStatus" runat="server"></asp:Literal>
                    </td>
                </tr>


            </tbody>
        </table>
    </div>
</asp:Content>
