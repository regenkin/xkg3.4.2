<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="PrizeList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.PrizeList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="ControlPanel.Promotions" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rows_title {
            background: #f2f2f2;
        }

        .Regions, .tdleft {
            text-align: left !important;
            padding-left: 15px;
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

        dred {
            color: red;
        }
    </style>
    <script>
        //$(document).ready(function () {
        //    var tableTitle = $('.activediv').offset().top - 58;
        //    $(window).scroll(function () {
        //        if ($(document).scrollTop() >= tableTitle) {
        //            $('.activediv').css({
        //                position: 'fixed',
        //                top: '58px',
        //                borderBottom: '1px solid #ccc',
        //                boxShadow: '0 1px 3px #ccc',
        //                width: '1020px',
        //            })
        //        }
        //        //if ($(document).scrollTop() + $('.activediv').height() + 58 <= tableTitle) {
        //        if ($(document).scrollTop() + 58 <= tableTitle) {
        //            $('.activediv').attr("style", "background-color: rgb(255, 255, 255);");
        //        }
        //    });
        //})

        var ShowTabNum = "<%=ShowTabNum%>"; //当前显示页

        function resetform() {
            document.getElementById("aspnetForm").reset();
        }
        $(function () {
            $("#tabul").find("li").eq(ShowTabNum).addClass("active");

            $("select[selectset=regions]").addClass("form-control  resetSize addwauto"); //区域选择

            $(".Dstatus").each(function () {
                var rowStatus = $(this).attr("Dstatus");
                var addBnt = '<input type="button" class="btn btn-xs btn-info viewinfo" value="查看详情">';

                if (rowStatus == "0") {
                    addBnt += '<br/><input type="button" class="btn btn-xs btn-info mdaddr" value="修改地址"　 />';
                }
                else if (rowStatus == "1") {
                    addBnt += '<br/><input type="button" class="btn btn-xs btn-info mdaddr" value="修改地址"　 />';
                    addBnt += '<br/><input type="button" class="btn btn-xs btn-success sendPrize" value="　发货　"　 />';
                }
                else if (rowStatus == "2") {
                    addBnt += '<br/><input type="button" class="btn btn-xs btn-info sendPrize" value="修改发货"　 />';
                    addBnt += '<br/><input type="button" class="btn btn-xs btn-success confirmDeliver" value="确认收货"　 />';
                }
                $(this).html(addBnt);
            });





            //查看详情
            $(".viewinfo").click(function () {
                var $parent = $(this).parent();
                var LogId = $parent.attr("LogId");
                var pid = $parent.attr("pid");
                window.location.href = "PrizeDeliveryDetail.aspx?LogId=" + LogId+"&pid="+pid;
            })

            //修改发货地址
            $(".mdaddr").click(function () {
                ReSetLogId(this);

                var $parent = $(this).parent();
                var $parentPrev = $parent.prev().prev();

                $("#ctl00_ContentPlaceHolder1_txtPrizeAddress").val($parent.attr("address"));
                var selregiony = $parent.attr("reggionpath");
                var regions = selregiony.split(",");
                selregiony = regions[regions.length - 1];

                $("#addressRegion").attr("src", "hiRegionSelect.aspx?selid=" + selregiony); //重新定位


                var nametel = $parentPrev.text().trim().split(" ");
                $("#ctl00_ContentPlaceHolder1_txtPrizeReceiver").val(nametel[0].trim());
                $("#ctl00_ContentPlaceHolder1_txtPrizeTel").val(nametel[nametel.length - 1].trim());


                $('#MDAddress').modal('toggle').children().css({
                    width: '700px'
                });

            })


            $("#MDAddress").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_AddrBtn',
                'ctl00$ContentPlaceHolder1$txtPrizeReceiver': {
                    validators: {
                        notEmpty: {
                            message: '请填写联系人'
                        },
                        stringLength: {
                            min: 2,
                            max: 30,
                            message: '联系人不准确！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtPrizeTel': {
                    validators: {
                        notEmpty: {
                            message: '请填写联系电话！'
                        },
                        tell: {
                            message: '请填写正确的联系电话！'
                        }

                    }
                },
                'ctl00$ContentPlaceHolder1$txtPrizeAddress': {
                    validators: {
                        notEmpty: {
                            message: '请填写详细的地址'
                        },
                        stringLength: {
                            min: 6,
                            max: 50,
                            message: '地址信息不够详细'
                        }
                    }
                }
            });



            //发货登记修改
            $(".sendPrize").click(function () {
                ReSetLogId(this);
                var $parent = $(this).parent();

                $("#ctl00_ContentPlaceHolder1_txtPrizeCourierNumber").val($parent.attr("CourierNumber"));
                $("#ctl00_ContentPlaceHolder1_txtPrizeExpressName").val($parent.attr("ExpressName"));
                $("#ctl00_ContentPlaceHolder1_txtDeliveryTime").val($parent.attr("DeliveryTime"));

                $("#sendPanel").modal('toggle').children().css({
                    width: '400px'
                });;
            })

            $("#sendPanel").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_SendBtn',
                'ctl00$ContentPlaceHolder1$txtPrizeExpressName': {
                    validators: {
                        notEmpty: {
                            message: '请选择快递公司'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtPrizeCourierNumber': {
                    validators: {
                        notEmpty: {
                            message: '请填写快递单号'
                        },
                        stringLength: {
                            min: 6,
                            max: 30,
                            message: '快递单号信息不准确，至少6位以上'
                        }
                    }
                }
            });

            //收货确认
            $(".confirmDeliver").click(function () {
                ReSetLogId(this);


                HiTipsShow("确认标记此奖品已收货吗？", "confirm", "ctl00_ContentPlaceHolder1_ConfirmSend");
            })

        });

        function checkAddrForm() {
            $("#ctl00_ContentPlaceHolder1_HideRegion").val("");
            $("#ctl00_ContentPlaceHolder1_HideRegion").val($("#addressRegion").contents().find("#regionSelectorValue").val());
            if ($("#ctl00_ContentPlaceHolder1_HideRegion").val() == "") {
                //HiTipsShow("所有地未选择！", "error")
                //return false;
            }
            return true;
        };

        function SelectAllNew(obj) {
            $("[name=CheckBoxGroup]").prop("checked", $(obj).get(0).checked);

        }

        function DelSel() {

            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                HiTipsShow("没有数据选择！", "error");
                return;
            }

            HiTipsShow("确认删除选择项，删除后中奖记录及发货记录会同时删除？", "confirm", "ctl00_ContentPlaceHolder1_batchDel");

        }

        function ReSetLogId(obj) {
            $("#ctl00_ContentPlaceHolder1_txtLogID").val("");
            $("#ctl00_ContentPlaceHolder1_txtDeliverID").val(""); //清空相关值，以免串值
            $("#ctl00_ContentPlaceHolder1_txtDeliverStaus").val("");

            var $parent = $(obj).parent();
            $("#ctl00_ContentPlaceHolder1_txtLogID").val($parent.attr("LogId"));
            $("#ctl00_ContentPlaceHolder1_txtDeliverID").val($parent.attr("PrizeId")); //清空相关值，以免串值
            $("#ctl00_ContentPlaceHolder1_txtDeliverStaus").val($parent.attr("Dstatus"));
            $("#ctl00_ContentPlaceHolder1_txtLogPid").val($parent.attr("Pid"));

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>实物奖品发货管理</h2>
        <small>商品奖品发货管理。积分和优惠券奖品会自动发放到中奖会员账户中</small>
    </div>

    <form runat="server">
        <asp:HiddenField ID="txtLogID" Value="" runat="server" />
        <asp:HiddenField ID="txtLogPid" Value="" runat="server" />
        <asp:HiddenField ID="txtDeliverID" Value="" runat="server" />
        <asp:HiddenField ID="txtDeliverStaus" Value="" runat="server" />

        <!--收货确认提交-->
        <asp:Button ID="ConfirmSend" Style="display: none" runat="server" />

        <!--批量删除确认-->
        <asp:Button ID="batchDel" Style="display: none" runat="server" />

        <!--修改地址-->
        <div class="modal fade" id="MDAddress">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">修改奖品收货地址</h4>
                    </div>
                    <div class="modal-body form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>收货人姓名：</label>
                            <div class="col-xs-7">
                                <asp:TextBox ID="txtPrizeReceiver" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>联系电话：</label>
                            <div class="col-xs-7">
                                <asp:TextBox ID="txtPrizeTel" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group  form-horizontal">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>选择所在地：</label>
                            <div class="col-xs-9 ">
                                <iframe id="addressRegion" width="100%" height="30px" scrolling="no" frameborder="0" style="overflow: hidden; padding-top: 3px; border: 0px" src="hiRegionSelect.aspx"></iframe>
                                <asp:HiddenField ID="HideRegion" runat="server" Value="" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>详细地址：</label>
                            <div class="col-xs-9">
                                <asp:TextBox ID="txtPrizeAddress" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="AddrBtn" OnClientClick="return checkAddrForm()" class="btn btn-primary" Text="确定修改" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



        <!--发货信息-->
        <div class="modal fade" id="sendPanel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" style="text-align: left">发货信息</h4>
                    </div>
                    <div class="modal-body form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-3 control-label"><em>*</em>物流公司：</label>
                            <div class="col-xs-8">
                                <asp:DropDownList ID="txtPrizeExpressName" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-3 control-label"><em>*</em>快递单号：</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="txtPrizeCourierNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <asp:HiddenField ID="txtDeliveryTime" Value="" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="SendBtn" class="btn btn-primary" Text="保存" runat="server" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->



        <div id="mytabl">
            <!-- Nav tabs -->

            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="activediv" style="background-color: #fff">
                        <div class="table-page">
                            <ul class="nav nav-tabs" id="tabul">
                                <li>
                                    <asp:LinkButton ID="ListAll" Text="所有奖品(10)" runat="server" CommandName="0" OnClick="tabClick"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="ListWaitAddr" Text="待填写收货地址(0)" runat="server" CommandName="1" OnClick="tabClick"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="ListWaitSend" Text="待发货(0)" runat="server" CommandName="2" OnClick="tabClick"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="ListHasSend" Text="已发货(0)" runat="server" CommandName="3" OnClick="tabClick"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="ListHasReceive" Text="已收货(0)" runat="server" CommandName="4" OnClick="tabClick"></asp:LinkButton></li>
                            </ul>
                            <div class="page-box">
                                <div class="page fr">
                                    <div class="form-group">
                                        <label for="exampleInputName2">每页显示数量：</label>
                                        <UI:PageSize runat="server" ID="hrefPageSize" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="set-switch">

                            <div class="form-inline mb10">
                                <div class="form-group mr20">
                                    <label for="sellshop1">活动名称：</label>
                                    <asp:TextBox ID="txtTitle" CssClass="form-control resetSize inputw150" runat="server" />
                                </div>
                                <div class="form-group mr20">
                                    <label for="sellshop1">收货人：</label>
                                    <asp:TextBox ID="txtReceiver" CssClass="form-control resetSize inputw150" runat="server" />
                                </div>
                                <div class="form-group mr20">
                                    <label for="sellshop2">开奖时间：</label>
                                    <div class="form-group">
                                        <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                                        &nbsp;&nbsp;至&nbsp;&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                                        &nbsp;&nbsp;&nbsp;
                                    </div>
                                </div>

                            </div>
                            <div class="form-inline">
                                <div class="form-group mr20">
                                    <label for="sellshop4">奖品名称：</label>
                                    <asp:TextBox ID="txtProductName" CssClass="form-control  resetSize  inputw150" runat="server" />
                                </div>
                                <div class="form-group mr20">
                                    <label for="sellshop5">收货人区域：</label>
                                    <Hi:RegionSelector runat="server" ID="SelReggion" />
                                </div>

                            </div>
                            <div class="reset-search" style="right: 110px; top: 50px">
                                <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="btn btn-primary  resetSize " OnClick="btnSearchButton_Click" />
                                <asp:Button ID="btnExportButton" runat="server" Text="导出Excel" CssClass="btn btn-primary  resetSize " OnClick="btnExportButton_Click" />
                            </div>
                        </div>

                        <div class="form-inline" style="margin-top: 10px; margin-bottom: 10px; margin-left: 5px; margin-left: 10px">
                            <input type="checkbox" id="sells1" class="allselect" onclick="SelectAllNew(this)">
                            全选
                        <input type="button" value="批量删除" class="btn resetSize btn-danger" onclick="DelSel()" style="margin-left: 20px;" />
                        </div>
                    </div>
                    <div class="sell-table">
                        <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="25%">奖品信息</th>
                                        <th width="20%" style="text-align: left">活动名称</th>
                                        <th width="10%">收货人</th>
                                        <th width="10%">奖品等级</th>
                                        <th width="15%">操作</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="content-table">
                            <table class="table" style="table-layout: fixed">
                                <tbody>
                                    <asp:Repeater ID="reDistributor" runat="server">
                                        <ItemTemplate>
                                            <tr class="rows_title">
                                                <th class="tdleft" width="25%">&nbsp;&nbsp;
                                            <input type="checkbox" name="CheckBoxGroup"   value="<%# Eval("LogId")%>">&nbsp;&nbsp;&nbsp;
                                            中奖编号：<%# (int)Eval("Ptype")==1? Eval("PrizeNums").ToString().Remove(Eval("PrizeNums").ToString().Length-1):Eval("WinTime","{0:yyyy-MM-dd-}")+Eval("LogId").ToString() %></th>
                                                <th class="tdleft" width="20%">
                                                    <%# Eval("WinTime","{0:yyyy-MM-dd HH:mm:ss}")%>
                                                </th>
                                                <th width="10%" style="color: #ed4e1a">
                                                   <%# GameHelper.GetPrizesDeliveStatus(Eval("status").ToString(), Eval("IsLogistics").ToString() , Eval("PrizeType").ToString(),Eval("GameType").ToString())%>
                                                </th>
                                                <th width="10%"></th>
                                                <th class="tdleft" width="15%"></th>
                                            </tr>
                                            <tr>
                                                <td class="tdleft">
                                                    <div class="img fl mr10">
                                                        <%-- <img src="/utility/pics/none.gif" style="height:60px;width:60px;border-width:0px;">--%>
                                                        <Hi:ListImage ID="ListImage3" runat="server" DataField="ThumbnailUrl100" Width="60" Height="60" />
                                                    </div>
                                                    <div class="pinfo">
                                                        <%#Eval("ProductName") %><br>

                                                        <span style="color: #888"><%#Eval("SkuIdStr") %></span>
                                                    </div>
                                                </td>
                                                <td class="tdleft">
                                                    <%#Eval("Title") %><br />
                                                    <span class="jpname">[<%# GameHelper.GetGameTypeName(Eval("GameType").ToString()) %>]

                                               <dred> <%# (int)Eval("Ptype")==1?Eval("PrizeNums").ToString().Remove(Eval("PrizeNums").ToString().Length-1):""%></dred>

                                                    </span>
                                                </td>
                                                <td class="borderleft">
                                                    <%# Eval("Receiver")%><br />
                                                    <%# Eval("Tel") %>
                                                </td>
                                                <td>
                                                    <%#   GameHelper.GetPrizeGradeName(Eval("PrizeGrade").ToString()) %>
                                                </td>
                                                <td class="Dstatus" dstatus="<%#Eval("status") %>"
                                                    logid="<%#Eval("LogId") %>" prizeid="<%#Eval("Id") %>"
                                                    reggionpath="<%#Eval("ReggionPath") %>"
                                                    address="<%#Eval("Address") %>"
                                                    pid="<%#Eval("Pid") %>"
                                                    expressname="<%#Eval("ExpressName") %>"
                                                    couriernumber="<%#Eval("CourierNumber") %>"
                                                    deliverytime="<%#Eval("DeliveryTime","{0:yyyy-MM-dd HH:mm:ss}") %>">-
                                                </td>

                                            </tr>
                                            <tr class="splittr">
                                                <td colspan="5"></td>
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
                                <div class="pagination" style="float: right">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
                <div class="tab-pane"></div>

            </div>
        </div>






    </form>

</asp:Content>
