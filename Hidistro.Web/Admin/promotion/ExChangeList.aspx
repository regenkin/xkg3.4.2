<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExChangeList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.ExChangeList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">        .activediv {
   z-index:1;     }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            var tableTitle = $('.activediv').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.activediv').css({
                        position: 'fixed',
                        top: '58px',
                        borderBottom: '1px solid #ccc',
                        boxShadow: '0 1px 3px #ccc',
                        width: '1020px',
                        zindex:1
                    })
                }
                //if ($(document).scrollTop() + $('.activediv').height() + 58 <= tableTitle) {
                if ($(document).scrollTop() + 58 <= tableTitle) {
                    $('.activediv').attr("style", "background-color: rgb(255, 255, 255);");
                }
            });
        })

        $(document).ready(function () {
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    $(this).prop('checked', check);
                });
            });

            var status = getUrlParam("status");
            $('#nav li').eq(status).siblings().removeClass('active').end().addClass('active');
        });

        function setDel(id, obj) {       
            var flag = true;
            if (id == null) {
                id = "";
                $('input[name="CheckBoxGroup"]').each(function () {
                    if (!flag) return;
                    if ($(this).prop('checked')) {
                        id += "," + $(this).val();                      
                    }
                });
                if (id.length > 1) {
                    id = id.substr(1);
                }
                else {
                    ShowMsg('请选择商品！', false);
                    flag = false;
                    return;
                }
            }
            if (HiConform("删除积分兑换活动，是否继续？", obj)) {
                flag = true;
            }
            else {
                flag = false;
            }
            if (flag) {
                $('#<%=txt_ids.ClientID%>').val(id);
                $('#<%=DelBtn.ClientID%>').click();
            }

        }


        //获取url中的参数
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
            <h2>积分兑换</h2>
            <%--<small>积分兑换商品：在该页面设置积分兑换商品，在微信商城页面就可以兑换积分商品。</small>--%>
        </div>
        <div class="blank" style="text-align: left;">
            <a href="PointExchange.aspx" class="btn btn-primary">添加积分兑换活动</a>
            <a href="../Member/setScore_sign.aspx" class="btn btn-primary" style="margin-left:20px;">会员积分设置</a>
        </div>

        <div class="activediv"  style="background-color:#fff;">

      
        <div class="set-switch">
        <div class="form-inline">
            <label>商品名称：</label>
            <asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txt_name" placeholder="商品名称" Width="110px"></asp:TextBox>
            <asp:Button CssClass="btn btn-primary resetSize mr20" ID="btnSeach" runat="server" Text="查询"/>
            <a href="../trade/manageorder.aspx" class="btn btn-primary resetSize">订单列表</a>          
        </div>
        </div>
        
        <div class="play-tabs">
            <div class="table-page">
                <ul class="nav nav-tabs" role="tablist" id="nav">
                    <li role="presentation" class="active">
                        <a href="ExChangeList.aspx?status=0">所有活动(<asp:Label runat="server" ID="lblAll" Text="0"></asp:Label>)</a>
                    </li>
                    <li role="presentation" >
                        <a href="ExChangeList.aspx?status=1">进行中(<asp:Label runat="server" ID="lblIn" Text="0"></asp:Label>)</a>
                    </li>
                    <li role="presentation">
                        <a href="ExChangeList.aspx?status=2">已结束(<asp:Label runat="server" ID="lblEnd" Text="0"></asp:Label>)</a>
                    </li>
                    <li role="presentation">
                        <a href="ExChangeList.aspx?status=3">未开始(<asp:Label runat="server" ID="lblUnBegin" Text="0"></asp:Label>)</a>
                    </li>                    
                </ul>

                <div class="page-box" style="margin-right: 15px;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="PageSize1" />
                    </div>
                </div>
            </div>
            </div>
        </div>

        <div style="margin-bottom: 10px; margin-top:10px;margin-left:5px">
            <input type="checkbox" id="selectAll" /> 全选

           <%-- <button type="button" class="btn btn-danger resetSize" onclick="setDel();" style="margin-left:20px;">
                批量删除
            </button>--%>

            <asp:Button ID="btnDelete" runat="server"  style="margin-left:20px;" Text="批量删除" CssClass="btn resetSize btn-danger" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？<p>删除后不可恢复！</p></strong>', this);" />


            <div style="display: none;">
                <asp:Button runat="server" ID="DelBtn"/>
                <asp:TextBox runat="server" ID="txt_ids"></asp:TextBox>
            </div>
        </div>
              </div>

        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="2%"></th>
                            <th width="15%">活动名称</th>
                            <th width="15%">可兑换商品（件）</th>
                            <th width="15%">可兑换商品发放总量</th>
                            <th width="15%">已兑换商品总数</th>
                            <th width="15%">状态</th>                            
                            <th width="23%">操作</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="content-table">
                <table class="table">
                    <tbody>
                        <asp:Repeater ID="grdProducts" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="2%">
                                        <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("Id") %>' />
                                    </td>
                                    <td width="15%">
                                        <%#Eval("Name") %>
                                    </td>

                                    <td width="15%">
                                        <%#Eval("ProductNumber") %>
                                    </td>

                                    <td width="15%">
                                        <%#Eval("TotalNumber") %>
                                    </td>

                                    <td width="15%">
                                        <%#Eval("ExChangedNumber") %>
                                    </td>

                                    <td width="15%">
                                        <%#Eval("sStatus") %>
                                    </td>
                                                                      

                                    <td style="text-align: left; width: 23%;">
                                        <button type="button" onclick='<%#string.Format("window.location.href=\"EditProductToExchange.aspx?id={0}\"", Eval("id"))%>' class="btn btn-warning btn-sm" name="updateBtn" <%#string.Format("{0}", Eval("canChkStatus").ToString())%>>
                                            修改商品                                            
                                        </button>
                                        <button type="button" onclick='<%#string.Format("window.location.href=\"PointExchange.aspx?id={0}\"", Eval("id"))%>' class="btn btn-info btn-sm" name="selectBtn" style="margin-left:10px;">
                                            查看详情                                            
                                        </button>

                                        <span class="submit_jiage" style="margin-left:10px;">
                                            <a href="PointExchange.aspx?id=<%#Eval("id")%>" name="edit" style='display:<%#Eval("canChkStatus").ToString()=="disabled"?"none":""%>' >编辑</a>
                                        </span>
                                        <span class="submit_jiage" style="margin-left:10px;">
                                          <%--  <a href="javascript:;" onclick='<%#string.Format("setDel({0},this);", Eval("id"))%>' name="del">删除</a>--%>
                                            <asp:Button ID="btnDel" CommandName="Delete" CommandArgument='<%# Eval("id") %>' runat="server" CssClass="btnLink pad" Text="删除" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后活动不可恢复！</p>',this);" />
                                        </span>
                                        <%--<button type="button" onclick='<%#string.Format("setDel({0},this);", Eval("id"))%>' class="btn btn-link btn-sm" name="del">
                                            删除                    
                                        </button>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>


    </form>
</asp:Content>
