
<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorSummary.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/echarts.js"></script>
      <style>

        .yejiItem{text-align:center;line-height:30px;float:left;margin:5px;padding:10px 60px;border-left:1px solid #b9caca}
            .yejiItem:first-child {border-left:0px
            }
            .yejiItem .money{color:#125acb;font-weight:bold;font-size:18px}
             .yejiItem .yejitxt{color:#444451;font-size:18px}
             .infodiv{float:left;}
             .infosdetail{width:250px;margin-left:10px;line-height:23px}
             .infostitle{width:90px; text-align:center;}
             .infosdetail ul label{width:90px;text-align:right;margin-right:10px;font-weight:normal;}
            .infosdetailLong{width:500px;}
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--
    <form runat="server" id="dsfdsf">
    </form>--%>


     <div class="member-info clearfix">
                    <div class="fl">
                        <p class="line">今日分销金额 ￥<%=FXValidOrderTotal.ToString("N2") %></p>
                        <div class="blank"></div>
                        <p class="line">业绩占比 <%=FXResultPercent.ToString("N2")%>%</p>
                        <div class="radius left">
                            <div class="bigsize">
                                <p>今日分销订单数</p>
                                <span><%=FXOrderNumber%></span>
                            </div>
                            <p>产生佣金</p>
                            <span>￥<%=FXCommissionFee.ToString("N2")%></span></div>
                    </div>
                    <div class="fr">
                        <p class="line">昨日分销金额 ￥<%=FXValidOrderTotal_Yesterday.ToString("N2")%></p>
                        <div class="blank"></div>
                        <p class="line">业绩占比 <%=FXResultPercent_Yesterday.ToString("N2")%>%</p>
                        <div class="radius right">
                            <div class="bigsize">
                                <p>昨日分销订单数</p>
                                <span><%=FXOrderNumber_Yesterday%></span>
                            </div>
                            <p>产生佣金</p>
                            <span>￥<%=FXCommissionFee_Yesterday.ToString("N2")%></span></div>
                    </div>
                </div>
     <!--业绩-->
        <%--<h3 class="templateTitle">业绩</h3>--%>
        <div class="set-switch  clearfix" style="height:120px">

            <div class="yejiItem">
             <div class="money" id="ReferralOrders" runat="server"><%=AgentNumber%></div>
             <div class="yejitxt">分销商总数</div>
            </div>

             <div class="yejiItem">
             <div class="money"  id="OrdersTotal" runat="server"><%=NewAgentNumber_Yesterday%></div>
             <div class="yejitxt">昨日新增分销商数</div>
            </div>

             <div class="yejiItem">
             <div class="money"  id="TotalReferral" runat="server">￥<%=FinishedDrawCommissionFee.ToString("N2")%></div>
             <div class="yejitxt">已提现佣金</div><%--全部--%>
             </div>

             <div class="yejiItem">
             <div class="money"  id="ReferralBlance" runat="server">￥<%=WaitDrawCommissionFee.ToString("N2")%></div>
             <div class="yejitxt" >待提现佣金</div><%--全部--%>
            </div>
   
        </div>

                <!--分销走势-->
      <div class="shop-chart">
                    <h3>分销走势</h3>
                    <div class="echarts-map" id="main"></div>
                </div>


     <script type="text/javascript">
         // Step:3 conifg ECharts's path, link to echarts.js from current page.
         // Step:3 为模块加载器配置echarts的路径，从当前页面链接到echarts.js，定义所需图表路径
         require.config({
             paths: {
                 echarts: '../js'
             }
         });

         // Step:4 require echarts and use it in the callback.
         // Step:4 动态加载echarts然后在回调函数中开始使用，注意保持按需加载结构定义图表路径
         require(
             [
                 'echarts',
                 'echarts/chart/bar',
                 'echarts/chart/line',
                 'echarts/chart/map'
             ],
             function (ec) {
                 //--- 折柱 ---
                 var myChart = ec.init(document.getElementById('main'));
                 myChart.setOption({
                     tooltip: {
                         trigger: 'axis'
                     },
                     legend: {
                         data: ['新增分销商数', '分销金额', '分销佣金']
                     },
                     calculable: true,
                     xAxis: [
                         {
                             type: 'category',
                             boundaryGap: false,
                             //data: ['2015-07-09', '2015-07-10', '2015-07-11', '2015-07-12', '2015-07-13', '2015-07-13', '2015-07-14']
                             data: [ <%=DateList%>]
                         }
                     ],
                     yAxis: [
                         {
                             type: 'value'
                         }
                     ],
                     series: [
                         {
                             name: '新增分销商数',
                             type: 'line',
                             //stack: '总量',
                             //data: [1200, 1322, 101, 134, 90, 230, 210]
                             data: [ <%=QtyList1%>]
                         },
                         {
                             name: '分销金额',
                             type: 'line',
                             //stack: '总量2',
                             //data: [1200, 1322, 101, 134, 90, 230, 210]
                             data: [ <%=QtyList2%>]
                         },
                         {
                             name: '分销佣金',
                             type: 'line',
                             //stack: '总量3',
                             //data: [1200, 1322, 101, 134, 90, 230, 210]
                             data: [ <%=QtyList3%>]
                         }
                     ]
                 });
             }
         );
    </script>
</asp:Content>
