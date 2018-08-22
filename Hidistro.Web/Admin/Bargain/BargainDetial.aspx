<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BargainDetial.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Bargain.BargainDetial" MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }

        $(function () {
            var bargainId = getUrlParam("Id");
            var data = {};
            data.BargainId = bargainId;
            $.post("/api/VshopProcess.ashx?action=GetStatisticalData", data, function (json) {
                if (json.success=="1")
                {
                    $("#ctl00_ContentPlaceHolder1_lbNumberOfParticipants").html(json.NumberOfParticipants);
                    $("#ctl00_ContentPlaceHolder1_lbmemberNumber").html(json.SingleMember);
                    $("#ctl00_ContentPlaceHolder1_lbSaleNumber").html(json.ActivitySales);
                    $("#ctl00_ContentPlaceHolder1_lbStock").html(json.SurplusInventory);
                    $("#ctl00_ContentPlaceHolder1_lbSalePrice").html(json.AverageTransactionPrice);
                    $("#ctl00_ContentPlaceHolder1_lbStatus").html(json.ActiveState);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div>
            <div class="page-header">
                <h2>好友砍价<small style="display:inline;margin-left:20px;color:red">好友砍价商品不同时享受满减活动,不能使用优惠劵,不能使用积分抵扣订单金额 </small></h2>
            </div>
            <div class="blank">
                <a href="ManagerBargain.aspx?Type=0" class="btn btn-primary btn-sm inputw100">&lt;&lt; 返回</a>
            </div>
            <div class="shop-navigation pb100 clearfix">
                <div class="fl">
                    <div class="mobile-border">
                        <div class="mobile-d">
                            <div class="mobile-header">
                                <i></i>
                                <div class="mobile-title">好友砍价</div>
                            </div>
                            <div class="set-overflow">
                                <div style="min-height: 350px;">
                                    <div class="y3-shared-title">
                                        <p class="f-title">
                                            <asp:Label runat="server" ID="lbtitle"></asp:Label>
                                        </p>
                                        <div class="y3-shopimgname">
                                            <asp:Image runat="server" ID="productImage" ImageUrl="/utility/pics/none.gif" />
                                            <asp:Label runat="server" ID="lbproductName"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear-line">
                            <div class="mobile-footer"></div>
                        </div>
                    </div>
                </div>
                <div class="fl frwidth">
                    <div class="set-switch resetBorder">
                        <p class="mb10 borderSolidB pb5"><strong>活动设置：</strong></p>
                        <div class="form-horizontal clearfix">
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;分享标题：</label>
                                <div class="form-inline col-xs-9">
                                    <%-- <input type="text" class="form-control resetSize inputw300" disabled id="setdate" placeholder="该活动被分享的时候显示的默认标题">--%>
                                    <asp:TextBox ID="txtTitle" Enabled="false" runat="server" CssClass="form-control resetSize inputw300" placeholder="该活动被分享的时候显示的默认标题"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动时间：</label>
                                <div class="form-inline journal-query col-xs-9">
                                    <div class="form-group">
                                        <Hi:DateTimePicker runat="server" Enabled="false" CssClass="form-control resetSize" ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="开始时间" />
                                        &nbsp;&nbsp;至&nbsp;
                                        <Hi:DateTimePicker runat="server" Enabled="false" CssClass="form-control resetSize" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="结束时间" IsEnd="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动封面：</label>
                                <div class="form-inline journal-query col-xs-9">
                                    <asp:Image runat="server" ID="imgeProductName" Width="70px" Height="70px" />
                                    <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                                            建议尺寸：600 x 200 像素，小于300KB，支持jpg、gif、png格式
                                        </div>
                                </div>
                                
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">活动说明：</label>
                                <div class="form-inline journal-query col-xs-9">
                                    <div class="form-group">
                                        <%-- <textarea class="form-control inputtext" disabled placeholder="1、砍至底价后，请确认下单付款，不付款无效"></textarea>--%>
                                        <asp:TextBox runat="server" Enabled="false" ID="txtRemarks" CssClass="form-control inputtext" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="set-switch resetBorder">
                        <p class="mb10 borderSolidB pb5"><strong>商品设置：</strong></p>
                        <div class="form-horizontal clearfix">
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;选择奖品：</label>
                                <div class="form-inline col-xs-9 pt3">
                                    <span class="colorc">选择的多规格商品每个规格的价格必须相同</span>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername">&nbsp;&nbsp;</label>
                                <div class="form-inline col-xs-9">
                                    <div class="y3-prize-info clearfix">
                                        <%--  <div class="shop-img fl">
                                            <img src="http://fpoimg.com/60x60">
                                        </div>
                                        <div class="shop-username fl ml10">
                                            <p>苹果（Apple）iPhone 6 Plus A1524 16G版 4G手机</p>
                                        </div>
                                        <p class="fl ml20">现价：￥5188.00~￥5888.00</p>
                                        <p class="fl ml20">库存：1230</p>--%>
                                        <%= productInfoHtml %>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动库存：</label>
                                <div class="form-inline col-xs-9">
                                    <%--<input type="text" disabled class="form-control resetSize inputw100" id="setdate" value="1">--%>
                                    <asp:TextBox ID="txtTranNumber" Enabled="false" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    <span class="colorc">该商品参加砍价活动最大可销售数量</span>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;限购数量：</label>
                                <div class="form-inline col-xs-9">
                                    <%-- <input type="text" disabled class="form-control resetSize inputw100" id="setdate" value="1">--%>
                                    <asp:TextBox ID="txtPurchaseNumber" Enabled="false" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    <span class="colorc">同一个会员最多购买数量</span>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;初始价格：</label>
                                <div class="form-inline col-xs-9">
                                    <%--<input type="text" disabled class="form-control resetSize inputw100" id="setdate" value="1">&nbsp;元--%>
                                    <asp:TextBox ID="txtInitialPrice" Enabled="false" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp;元
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动底价：</label>
                                <div class="form-inline col-xs-9">
                                    <%--<input type="text" disabled class="form-control resetSize inputw100" id="setdate" value="1">&nbsp;元--%>
                                    <asp:TextBox ID="txtFloorPrice" Enabled="false" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp;元
                                        <span class="colorc">最终成交价格不会低于底价</span>
                                </div>
                            </div>
                              <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;是否分佣：</label>
                                <div class="form-inline col-xs-9">
                                     <asp:CheckBox runat="server" ID="ckIsCommission" Text="分佣" Checked="true" Enabled="false"/>    
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;砍价方式：</label>
                                <div class="form-inline col-xs-9">
                                    <div class="mb10">
                                        <label class="mr10 middle fl pt3">
                                            <%--  <input disabled type="radio">每次砍掉</label>--%>
                                            <asp:RadioButton ID="rbtBargainTypeOne" GroupName="BargainType" Enabled="false" Checked="true" runat="server" Text="每次砍掉" /></label>
                                        <%--           <input type="text" disabled class="form-control resetSize inputw100" id="setdate" value="1">&nbsp;元--%>
                                        <asp:TextBox ID="txtBargainTypeOneValue" runat="server" Enabled="false" CssClass="form-control resetSize inputw100"></asp:TextBox>&nbsp;元
                                    </div>
                                    <div>
                                        <label class="mr10 middle fl pt3">
                                            <asp:RadioButton ID="rbtBargainTypeTwo" Enabled="false" GroupName="BargainType" runat="server" Text="随机砍掉" />
                                        </label>
                                        <asp:TextBox ID="txtBargainTypeTwoValue1" runat="server" Enabled="false" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                        &nbsp;~&nbsp;
                                        <asp:TextBox ID="txtBargainTypeTwoValue2" runat="server" Enabled="false" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                        &nbsp;元
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="set-switch resetBorder">
                        <p class="mb10 borderSolidB pb5"><strong>活动统计：</strong></p>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">参与人数：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="colorc">
                                        <asp:Label runat="server" ID="lbNumberOfParticipants" Text="0"></asp:Label>人</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">下单会员：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="colorc">
                                        <asp:Label runat="server" ID="lbmemberNumber" Text="0"></asp:Label>位</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">活动销售：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="colorc">
                                        <asp:Label runat="server" ID="lbSaleNumber" Text="0"></asp:Label>件</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">剩余库存：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="colorc">
                                        <asp:Label runat="server" ID="lbStock" Text="0"></asp:Label>件</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">成交均价：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="colorc">￥<asp:Label runat="server" ID="lbSalePrice" Text="0"></asp:Label></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal clearfix">
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">活动状态：</label>
                                <div class="form-inline journal-query col-xs-9 pt3">
                                    <span class="red">
                                        <asp:Label runat="server" ID="lbStatus" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
