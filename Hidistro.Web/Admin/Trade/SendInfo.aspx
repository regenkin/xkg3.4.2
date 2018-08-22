<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SendInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.SendInfo" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>发货明细</h2>
            <asp:Label ID="lblStatus"
                runat="server" Style="display: none;" Text="1"></asp:Label>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs" style="display: none">
                    <li id="liOper0">
                        <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>待生成明细单</span></asp:HyperLink></li>
                    <li id="liOper1">
                        <asp:HyperLink ID="hlinkNotPay" runat="server"><span>已生成明细单</span></asp:HyperLink></li>
                    <li id="liOper2">
                        <asp:HyperLink ID="hlinkYetPay" runat="server"><span>历史明细单</span></asp:HyperLink></li>
                </ul>
            </div>
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                        <strong>智能合并订单</strong>
                        <p>开启智能合并以后，系统会自动将收货信息一样且运费模板相同的订单合并成一个包裹发货</p>
                        <div id="guidepageEnable" class="switch-btn off" onclick="setEnable(this)">
                            已关闭
                <i></i>
                        </div>
                    </div>
                </div>
            </div>

        </div>



        <div class="orderManagement">
            <div class="tableHeader mb10">
                <table width="100%">
                    <thead>
                        <tr>
                            <th width="270">商品信息</th>
                            <th width="91">单价</th>
                            <th width="91">数量</th>
                            <th width="91">售后</th>
                            <th width="119">实付金额</th>
                            <th width="119">收货人</th>
                            <th width="119">收货地区</th>
                            <th width="120">指定物流</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="select-page nomarg clearfix">
                <div class="form-horizontal fl">
                    <div class="form-group mar forced">
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" id="selAll" class="allselect">全选</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" class="btn btn-primary resetSize" value="批量备注" onclick="RemarkOrderFun('0', '', '', '', '')" />
                            <asp:Button ID="btnDeleteCheck" runat="server" Text="批量删除" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>订单删除后将进入订单回收站！</strong><p>确定要批量删除所选择的订单吗？</p>',this)" />
                        </div>
                    </div>
                </div>
                <div class="page fr">
                    <div class="form-group">
                        <label>每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                        <input type="hidden" id="hidOrderId" runat="server" />
                    </div>
                </div>
            </div>



            <div class="orderManagementList orderManagementListOther">
                <ul>

                    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">


                        <ItemTemplate>
                            <li>
                                <div class="listTitle">
                                    <label>
                                        <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />
                                        订单编号：<%#Eval("OrderId") %><%#Globals.GetBarginShow(Eval("BargainDetialId")) %><asp:Literal
                                            ID="group" runat="server" Text='<%#  Eval("GroupBuyId")!=DBNull.Value&& Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' /></label>&nbsp;<span><Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                                runat="server"></Hi:FormatedTimeLabel></span>
                                    <span class="ok">
                                        <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                                            runat="server" />
                                    </span><span class="ml10 logistics" tid="<%#Eval("ExpressCompanyAbb") %>"><%--物流公司：未指定--%></span>
                                    <div style="float: right; padding-right: 10px; width: 26px" data-toggle="tooltip" data-placement="left" title="<%#Server.HtmlEncode(Eval("ManagerRemark").ToString()) %>" onclick="RemarkOrderFun('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Server.HtmlEncode(Eval("ManagerMark").ToString()) %>','<%#Server.HtmlEncode(Eval("ManagerRemark").ToString()) %>')">
                                        <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" />
                                    </div>
                                </div>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td width="550">
                                                <div class="listInfoLeft">
                                                    <asp:Repeater ID="rptSubList" runat="server">
                                                        <ItemTemplate>
                                                            <div class="orderInfolist clearfix">
                                                                <div class="orderImg fl clearfix">
                                                                    <div class="img fl">
                                                                        <Hi:ListImage runat="server" DataField="ThumbnailsUrl" Width="60" Height="60" />
                                                                        <%-- <img src="<%#Eval("ThumbnailsUrl") %>" style="width:60px;height:60px;" alt="">--%>
                                                                    </div>
                                                                    <div class="imgInfo fl">
                                                                        <p class="setColor"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductId") %>" target="_blank"><%# Eval("ItemDescription")%></a></p>
                                                                        <p><%--颜色：黑色 尺码：110--%><%#Eval("SKUContent") %></p>
                                                                    </div>
                                                                </div>
                                                                 <p class="fl orderP"><%#Eval("Type").ToString()=="0"? "￥"+Eval("ItemListPrice","{0:f2}"):Eval("PointNumber")+"积分" %></p>
                                                                <p class="fl orderP"><%#Eval("Quantity") %></p>
                                                                <p class="fl orderP setColor">
                                                                    <Hi:OrderItemStatusLabel ID="lblOrderItemStatus" OrderStatusCode='<%# Eval("OrderItemsStatus") %>'
                                                                        runat="server" />
                                                                </p>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </td>
                                            <td width="470">
                                                <div class="listInfoRight clearfix">
                                                    <div class="paymentAmount fl">
                                                        <p>
                                                            <strong>￥<Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                                                                <span class="Name" style="margin-left: -8px;"></strong>
                                                            &nbsp;
                                <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'
                                    Text="修改价格" Visible="false"></asp:HyperLink></span>
                                                    
                                                    <br>
                                                            (含运费￥<%#Eval("AdjustedFreight","{0:f2}") %>)
                                                        </p>
                                                        <span class="setColor"><%#Eval("PaymentType") %></span>
                                                    </div>
                                                    <div class="username fl">
                                                        <p>
                                                            <%#Eval("ShipTo") %><Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                                                            <br>
                                                            <%#Eval("CellPhone") %>
                                                        </p>
                                                        <%-- <span class="setColor glyphicon glyphicon-comment"></span>--%>
                                                        <span data-toggle="tooltip" title="<%#Eval("Remark") %>" style="<%#Eval("Remark").ToString().Trim()==""?"display:none":""%>">
                                                            <img src="../images/xi.gif" border="0"></span>
                                                    </div>
                                                    <div class="shopSource fl">
                                                        <p><%#Eval("ShippingRegion").ToString().Replace("，","<br>")%></p>
                                                    </div>
                                                    <div class="operation fr">
                                                        <%#Eval("ExpressCompanyName") %>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </li>
                        </ItemTemplate>

                    </asp:Repeater>


                </ul>
            </div>
        </div>

        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>


        
        <div class="modal fade" id="RemarkOrder">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">修改备注</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单号：</label><div class="col-xs-8 exitpa" id="spanOrderId"></div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">提交时间：</label><div class="col-xs-8 exitpa" id="lblOrderDateForRemark"></div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单实收款(元)：</label><div class="col-xs-8 exitpa" style="color: red; font-weight: bold;">
                                <Hi:FormatedMoneyLabel
                                    ID="lblOrderTotalForRemark" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label" style="padding-top: 0px;line-height:22px"><em>*</em>标志：</label>
                            <Hi:OrderRemarkImageRadioButtonList
                                runat="server" ID="orderRemarkImageForRemark" Width="238" />
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label">备注：</label><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                                Width="300" Height="50" />
                            <label class="col-xs-3 control-label"> </label><small>(备注信息仅后台可见)</small>
                        </div>

                        <div class="modal-footer">
                            <input type="text" id="txtcategoryId" runat="server" class="hide" />
                            <asp:Button ID="btnRemark" runat="server" Text="保存" CssClass="btn btn-success" OnClientClick="return ModifyMemo()" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function ShowOrderState() {
            var status = $("#ctl00_ContentPlaceHolder1_lblStatus").html();
            $("#liOper" + status).attr("class", "active");
        }
        $(document).ready(function () {
            ShowOrderState();
            $.ajax({
                url: "sendinfo.aspx",
                type: "post",
                data: "isCallback=GetOrdersStates",
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        if (json.allcount > 0) {
                            $("#liOper0").find("span").html($("#liOper0").find("span").html() + "(" + json.allcount + ")");
                            if (json.waibuyerpaycount > 0) {
                                $("#liOper1").find("span").html($("#liOper1").find("span").html() + "(" + json.waibuyerpaycount + ")");
                            }
                            if (json.buyalreadypaidcount > 0) {
                                $("#liOper2").find("span").html($("#liOper2").find("span").html() + "(" + json.buyalreadypaidcount + ")");
                            }
                            if (json.sellalreadysentcount > 0) {
                                $("#liOper3").find("span").html($("#liOper3").find("span").html() + "(" + json.sellalreadysentcount + ")");
                            }
                        }
                    }
                    $(".nav.nav-tabs").show();
                }
            });
            $("[data-toggle='tooltip']").tooltip({ html: false });
        });

        var arrytempstr = null;
        function setArryText(keyname, keyvalue) {
            var temptex = "\"" + keyname + "\":\"" + escape(keyvalue) + "\"";
            if (arrytempstr != null) {
                arrytempstr = arrytempstr.substr(0, arrytempstr.length - 1);
                arrytempstr += "," + temptex + "}";

            } else {
                arrytempstr = "{" + temptex + "}"
            }
            arrytext = $.parseJSON(arrytempstr);
        }

        function ModifyMemo() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
                    $radioId = $("input[type='radio'][name='ctl00$ContentPlaceHolder1$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        ShowMsg('请选择标志图标', false);
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_ContentPlaceHolder1_txtRemark", $("#ctl00_ContentPlaceHolder1_txtRemark").val());
                    break;
            };
            return true;
        }
        function RemarkOrderFun(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            if (OrderId != "0") {
                $(".isordersbatch").hide();
                arrytext = null;
                formtype = "remark";
                $("#ctl00_ContentPlaceHolder1_lblOrderTotalForRemark").html(OrderTotal);
                $("#ctl00_ContentPlaceHolder1_hidOrderId").val(OrderId);
                $("#spanOrderId").html(OrderId);
                $("#lblOrderDateForRemark").html(OrderDate);
                for (var i = 0; i <= 5; i++) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).value == managerMark) {
                        setArryText("ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i, "true");
                        $("#ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).click();//.attr("checked","checked");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).attr("checked", false);
                    }
                }
                setArryText("ctl00_ContentPlaceHolder1_txtRemark", managerRemark);
                $("#ctl00_ContentPlaceHolder1_txtRemark").val(managerRemark);
                //DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'ctl00_ContentPlaceHolder1_btnRemark');

                $('#RemarkOrder').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                $("#RemarkOrder").modal({ show: true });
            } else {
                //批量
                var IsTrue = false;
                var obj = document.getElementsByName("CheckBoxGroup");

                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].checked == true) {
                        IsTrue = true;
                        break;
                    }
                }
                if (!IsTrue) {
                    ShowMsg('请选择您要操作的选项!', false);
                    return false;
                }
                $("#ctl00_ContentPlaceHolder1_hidOrderId").val(OrderId);
                $(".isordersbatch").hide();
                $('#RemarkOrder').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                $("#RemarkOrder").modal({ show: true });
            }
        }
    </script>
</asp:Content>
