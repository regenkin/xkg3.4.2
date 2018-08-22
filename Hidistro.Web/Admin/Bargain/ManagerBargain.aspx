<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagerBargain.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Bargain.ManagerBargain" MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass {
            text-align: center;
        }

        .labelClass {
            margin-right: 10px;
        }

        .thCss {
            text-align: center;
        }

        .selectthis {
            border: 1px solid;
            border-color: #999999;
            color: #c93027;
            margin-right: 2px;
        }

            .selectthis:hover {
                border: 1px solid;
                border-color: #999999;
                color: #c93027;
                margin-right: 2px;
            }

        .aClass {
            border: 1px solid;
            border-color: #999999;
            color: #999999;
            margin-right: 2px;
        }

            .aClass:hover {
                border: 1px solid;
                border-color: #999999;
                color: #999999;
                margin-right: 2px;
            }

        #datalist td {
            word-break: break-all;
        }

        .title-table table thead tr th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: left;
            vertical-align: middle;
        }

        #datalist #ctl00_ContentPlaceHolder1_grdBargainList tbody tr td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            text-align: center;
            vertical-align: middle;
            border: none !important;
        }

        .table-bordered > tbody > tr > td {
            border: none;
            text-align: center;
        }

            .table-bordered > tbody > tr > td img {
                float: left;
                margin-right: 5px;
            }

            .table-bordered > tbody > tr > td:nth-of-type(2) {
                text-align: left;
            }

        .table-bordered > thead > tr > th {
            border: none;
        }

        .datetimepicker {
            top: 300px !important;
        }
    </style>
    <script src="../../Utility/swfupload/swfupload.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Utility/swfupload/DisLogoupload.js" type="text/javascript" charset="gbk"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //加载当前选中的tab
            var type = GetQueryString("Type");
            $(".nav-tabs > li").removeClass("active");
            $(".nav-tabs > li[dataID='" + type + "']").addClass("active");
            $(".nav-tabs > li[dataID='" + type + "'] > a").attr("href", "#");

            //全选按钮
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $("input[type='checkbox']").each(function () {
                    $(this).prop('checked', check);
                });
            });

            //加载状态的数量
            $.ajax({
                url: "/api/VshopProcess.ashx",
                type: "post",
                data: "action=GetBargainCount",
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        if (json.allCount > 0) {
                            $('.nav-tabs li[dataid="0"]').find("a").html($('.nav-tabs li[dataid="0"]').find("a").html() + "(" + json.allCount + ")");
                        }
                        if (json.ingCount > 0) {
                            $('.nav-tabs li[dataid="1"]').find("a").html($('.nav-tabs li[dataid="1"]').find("a").html() + "(" + json.ingCount + ")");
                        }
                        if (json.unbegunCount > 0) {
                            $('.nav-tabs li[dataid="3"]').find("a").html($('.nav-tabs li[dataid="3"]').find("a").html() + "(" + json.unbegunCount + ")");
                        }
                        if (json.endCount > 0) {
                            $('.nav-tabs li[dataid="2"]').find("a").html($('.nav-tabs li[dataid="2"]').find("a").html() + "(" + json.endCount + ")");
                        }
                    }
                }
            });
            $('#btnUpdateEndDate').click(function () {
                var bargainId = $("#hiddId").val();
                var endDate = $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').val();
                var startDate = $('#ctl00_ContentPlaceHolder1_calendarBeginDate_txtDateTimePicker').val();
                var end = new Date(Date.parse(endDate.replace(/-/g, "/")));
                var start = new Date(Date.parse(startDate.replace(/-/g, "/")));
                if (end == "Invalid Date") {
                    HiTipsShow("请输入有效的日期!", "error");
                    $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').focus();
                    return false;
                }

                if (start > end) {
                    HiTipsShow("结束日期必须大于开始日期!", "error");
                    $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').focus();
                    return false;
                }
                if (new Date() > end) {
                    HiTipsShow("结束日期必须大于当前时间!", "error");
                    $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').focus();
                    return false;
                }
                var data = {};
                data.BargainId = bargainId;
                data.EndDate = end.toLocaleString();
                $.post("/api/VshopProcess.ashx?action=UpdateBargainEndDate", data, function (json) {
                    if (json.success == "1") {
                        HiTipsShow("修改成功", "success", function () {
                            window.location.href = "ManagerBargain.aspx?Type=0";
                        });
                    }
                });
            });
        });

        ///修改结束时间
        function OpenEdit(id, endDate, beginDate) {
            $("#hiddId").val(id);
            $('#ctl00_ContentPlaceHolder1_calendarBeginDate_txtDateTimePicker').val(beginDate);
            $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').val(endDate);
            $('#divEditEndDate').modal('toggle').children().css({
                width: '500px', top: "100px"
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div>
            <div class="page-header">
                <h2>好友砍价管理<small style="display: inline; margin-left: 20px;color:red">好友砍价商品不同时享受满减活动,不能使用优惠劵,不能使用积分抵扣订单金额 </small></h2>
            </div>
            <div class="blank">
                <a href="AddBargain.aspx" class="btn btn-primary">新建砍价</a>
            </div>
            <div>
                <ul class="nav nav-tabs">
                    <li dataid="0" class="active"><a href="ManagerBargain.aspx?Type=0">
                        <asp:Literal ID="ListActive" Text="所有活动" runat="server"></asp:Literal></a></li>
                    <li dataid="1"><a href="ManagerBargain.aspx?Type=1">
                        <asp:Literal ID="Listfrozen" Text="进行中" runat="server"></asp:Literal></a></li>
                    <li dataid="3"><a href="ManagerBargain.aspx?Type=3">
                        <asp:Literal ID="Literal1" Text="未开始" runat="server"></asp:Literal></a></li>
                    <li dataid="2"><a href="ManagerBargain.aspx?Type=2">
                        <asp:Literal ID="Literal2" Text="已结束" runat="server"></asp:Literal></a></li>
                </ul>
                <div class="form-inline mb10">
                    <div class="set-switch">
                        <div class="form-inline  mb10">
                            <div class="form-group mr20" style="margin-left: 0px;">
                                <label for="sellshop1" class="ml10">活动标题：</label>
                                <asp:TextBox ID="txtTitle" CssClass="form-control resetSize" runat="server" />
                            </div>
                            <div class="form-group mr20" style="margin-left: 30px;">
                                <label for="txtProductName">商品名称：</label>
                                <asp:TextBox ID="txtProductName" CssClass="form-control resetSize" runat="server" />
                            </div>
                            <div class="form-group" style="margin-left: 110px; margin-top: 5px">
                                <asp:Button ID="btnSearchButton" runat="server" CssClass="btn resetSize btn-primary" Text="搜索" />
                            </div>
                            <div class="form-group mr20" style="margin-left: 30px; margin-top: 10px">
                                <a class="bl mb5" href="ManagerBargain.aspx?Type=0" style="cursor: pointer">清除条件</a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="title-table">
                    <div style="margin-bottom: 5px; margin-top: 10px;">
                        <div class="form-inline" id="pagesizeDiv" style="float: left; width: 100%; margin-bottom: 5px;">
                        </div>
                        <div class="page-box">
                            <div class="page fr">
                                <div class="form-group" style="margin-right: 0px; margin-left: 0px; background: #fff;">
                                    <label for="exampleInputName2">每页显示数量：</label>
                                    <UI:PageSize runat="server" ID="hrefPageSize" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="form-inline" style="text-align: left; margin-top: 5px; background: #fff;">
                        <label>

                            <input type="checkbox" id="selectAll" style="margin: 0px 0px 0px 15px" />

                            全选</label>
                        &nbsp;&nbsp;
                           <asp:Button ID="btnDeleteCheck" runat="server" Text="批量删除" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>活动删除后将不可恢复！</strong><p>确定要批量删除所选择的活动吗？</p>',this)" />
                    </div>
                    <div id="datalist">
                        <asp:Repeater ID="grdBargainList" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover mar table-bordered">
                                    <thead>
                                        <tr>
                                            <th style="width: 47px;"><span id="ctl00_ContentPlaceHolder1_grdMemberList_ctl01_label"></span></th>
                                            <th style="width: 310px">商品名称</th>
                                            <th style="text-align: center; width: 80px">初始价格</th>
                                            <th style="text-align: center; width: 80px">商品底价</th>
                                            <th style="text-align: center; width: 160px">活动时间</th>
                                            <th style="text-align: center; width: 70px">活动库存</th>
                                            <th style="text-align: center; width: 70px">成交数量</th>
                                            <th style="text-align: center; width: 70px">活动状态</th>
                                            <th style="text-align: center; width: 110px">操作</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr style="border-bottom: none;">
                                    <td>
                                        <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("Id") %>' /></td>
                                    <td>
                                        <img alt="商品图片" src='<%# (string.IsNullOrEmpty(Eval("ThumbnailUrl60").ToString())?"/utility/pics/none.gif":Eval("ThumbnailUrl60").ToString()) %>' style="height: 60px; width: 60px; border-width: 0px;" />
                                        <span style="width: 140px"><a target="_blank" title="点击打开活动页面" href="/BargainDetial.aspx?id=<%#Eval("Id") %>"><%# Eval("ProductName")%></a></span>
                                    </td>
                                    <td>￥<%#Eval("InitialPrice", "{0:F2}") %></td>
                                    <td>￥<%#Eval("FloorPrice", "{0:F2}") %></td>
                                    <td>自<%#Eval("BeginDate","{0:yyyy-MM-dd HH:mm:ss}")%><br>
                                        至<%# Eval("EndDate","{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                    <td><%#Eval("ActivityStock")%></td>
                                    <td><%#Eval("TranNumber")%></td>
                                    <td><%# Eval("bargainstatus")%></td>
                                    <td>
                                        <p>
                                            <a href='<%# string.Format("BargainDetial.aspx?Id={0}", Eval("Id"))%>' class="btn btn-warning resetSize mb5 inputw50">查看</a>
                                        </p>
                                        <p>
                                            <%# GetEditHtml(Eval("id"),Eval("bargainstatus"),Eval("EndDate"),Eval("BeginDate"))%>
                                        </p>
                                        <p>
                                            <asp:Button ID="lkDelete" Visible='<%# GetStatus(Eval("bargainstatus"))%>' IsShow="true" runat="server" Text="删除" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" CssClass="btn btn-danger resetSize mb5 inputw50" OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" />
                                        </p>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
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
            </div>


            <div class="modal fade" id="divEditEndDate">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" style="text-align: left">编辑结束时间</h4>
                        </div>
                        <div class="modal-body form-horizontal">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>开始时间：</label>
                                <div class="col-xs-6">
                                    <Hi:DateTimePicker runat="server" CssClass="form-control resetSize" ID="calendarBeginDate" Enabled="false" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="结束时间" IsEnd="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>结束时间：</label>
                                <div class="col-xs-6">
                                    <Hi:DateTimePicker runat="server" CssClass="form-control resetSize" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="结束时间" IsEnd="true" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="hidden" id="hiddId" value="" style="display: none;" />
                            <input type="button" id="btnUpdateEndDate" class="btn btn-primary" value="确定修改" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
