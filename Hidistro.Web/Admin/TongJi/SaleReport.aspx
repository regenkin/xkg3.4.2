<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="SaleReport.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.SaleReport" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/common.css" />
    <script src="../js/echarts.js"></script>
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">

                <div class="page-header">
                    <h2>经营简报</h2>
                </div>
  
  
                <div class="set-switch">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-1 pad resetSize control-label" for="setdate">操作时间：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                    <%--<input type="text" id="setdate" placeholder="创建日期">--%>
                                    <uc1:ucDateTimePicker runat="server" ID="txtBeginDate" CssClass="form-control resetSize" />
                                    &nbsp;&nbsp;至&nbsp;
                                    <%--<input type="text" class="form-control resetSize" placeholder="创建日期">--%>
                                    <uc1:ucDateTimePicker runat="server" ID="txtEndDate" CssClass="form-control resetSize" />
                                </div>
                                <%--<button type="submit" class="btn resetSize btn-primary">查询</button>--%>
                                <asp:Button ID="btnSearch" runat="server" Text="查询"  CssClass="btn resetSize btn-primary" />
                                <%--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="读" />
&nbsp;<asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="写" />--%>
                                <div class="form-group">
                                    <label for="exampleInputName2">查询日期</label>
                                    <asp:Button ID="btnWeekView"  runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="btnWeekView_Click"   />
                                    <asp:Button ID="btnMonthView" runat="server" class="btn resetSize btn-default" Text="最近1个月" OnClick="btnMonthView_Click" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <h4 class="statisticalTitle mb15">经营情况</h4>
                <div class="statisticalTable">
                    <table class="table" width="100%">
                        <thead>
                            <tr>
                                <th>客单价</th>
                                <th>订单数</th>
                                <th>购买人数</th>
                                <th>订单金额</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>￥<%= BuyerAvgPrice.ToString("N2") %> </td>
                                <td><%= OrderNumber%></td>
                                <td><%= BuyerNumber%></td>
                                <td>￥<%= SaleAmountFee.ToString("N2") %></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="shop-chart">
                    <div class="echarts-map" id="main"></div>
                </div>
                <h4 class="statisticalTitle mb15">会员情况</h4>
                <div class="statisticalTable">
                    <table class="table" width="100%">
                        <thead>
                            <tr>
                                <th>新增会员数</th>
                                <th>新增分销商数</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><%= NewMemberNumber%></td>
                                <td><%= NewAgentNumber%></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="shop-chart">
                    <div class="echarts-map" id="main1"></div>
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
                tooltip : {
                    trigger: 'axis'
                },
                legend: {
                    data:['订单数','订单金额']
                },
                calculable : true,
                xAxis : [
                    {
                        type : 'category',
                        boundaryGap : false,
                        //data : ['2015-07-19','2015-07-10','2015-07-11','2015-07-12','2015-07-13','2015-07-13','2015-07-14']
                        data: [ <%=DateListA %>]
                    }
                ],
                yAxis : [
                    {
                        type : 'value'
                    }
                ],
                series : [
                    {
                        name:'订单数',
                        type:'line',
                        //data:[600, 1322, 101, 134, 90, 230, 210]
                        data: [<%=QtyListA1 %>]
                    },
                    {
                        name:'订单金额',
                        type:'line',
                        //data:[150, 232, 201, 154, 190, 330, 410]
                        data: [<%=QtyListA2 %>]
                    }
                ]
            });

            //会员情况
            var myChart1 = ec.init(document.getElementById('main1'));
            myChart1.setOption({
                tooltip : {
                    trigger: 'axis'
                },
                legend: {
                    data:['新增会员','新增经销商']
                },
                calculable : true,
                xAxis : [
                    {
                        type : 'category',
                        boundaryGap : false,
                        //data : ['2015-07-09','2015-07-10','2015-07-11','2015-07-12','2015-07-13','2015-07-13','2015-07-14']
                        data: [ <%=DateListB %>]
                    }
                ],
                yAxis : [
                    {
                        type : 'value'
                    }
                ],
                series : [
                    {
                        name: '新增会员',
                        type:'line',
                        //data:[600, 1322, 101, 134, 90, 230, 210]
                        data: [<%=QtyListB1 %>]
                    },
                    {
                        name: '新增经销商',
                        type:'line',
                        //data:[150, 232, 201, 154, 190, 330, 410]
                        data: [<%=QtyListB2 %>]
                    }
                ]
            });
        }
    );
    </script>
 
    </form>
</asp:Content>
