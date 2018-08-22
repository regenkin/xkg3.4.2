<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="HuiTou.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.HuiTou" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/echarts.js"></script>
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
                <div class="page-header">
                    <h2>老顾客回头率</h2>
                </div>
                <div class="set-switch">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-1 pad resetSize control-label" for="setdate">操作时间：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                    <%--<input type="text" id="setdate" placeholder="创建日期">--%>
                                    <uc1:ucDateTimePicker ID="txtBeginDate" runat="server" CssClass="form-control resetSize" />                                
                                    &nbsp;&nbsp;至&nbsp;
                                    <%--<input type="text" class="form-control resetSize" placeholder="创建日期">--%>
                                    <uc1:ucDateTimePicker ID="txtEndDate" runat="server" CssClass="form-control resetSize" />
                                   
                                </div>
                                <asp:Button ID="btnSearch" runat="server"  CssClass="btn resetSize btn-primary" Text="查询" />
<%--                                <button type="submit" class="btn resetSize btn-primary">查询</button>--%>
                                <div class="form-group">
                                    <label for="exampleInputName2">查询日期</label>
                                    <asp:Button ID="btnWeekView" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="btnWeekView_Click"   />
                                    <asp:Button ID="btnMonthView" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="btnMonthView_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <h3 class="templateTitle">购买人数</h3>
                <div class="orderList mb20">
                    <ul class="clearfix">
                        <li class="noborder">
                            <div class="number"><%=BuyerNumber%></div>
                            <p>总购买人数</p>
                        </li>
                        <li>
                            <div class="number"><%=OldMember%></div>
                            <p>老顾客数</p>
                        </li>
                        <li>
                            <div class="number"><%=OldMemberPercent%>%</div>
                            <p>老顾客占比</p>
                        </li>
                    </ul>
                </div>
                <h3 class="templateTitle">新老顾客对比</h3>
                 <div id="main" class="newmap"></div>

<hr />
        
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
            myChart.setOption(
                option = {
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    legend: {
                        data: ['当日购买人数', '老会员', '新会员'],
                        //data: [ '老会员', '新会员'],
                        selectedMode: false
                    },

                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            //data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                            data: [<%=DateList%>]
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value'
                        }
                    ],
                    series: [
                        {
                            name: '当日购买人数',
                            type: 'bar',
                            //data: [320, 332, 301, 334, 390, 330, 320]
                            data: [<%=QtyListAll%>]
                          
                        },
                        {
                            name: '老会员',
                            type: 'bar',
                            stack: '广告',
                            //data: [120, 132, 101, 134, 90, 230, 210]
                            data: [<%=QtyListOld%>]
                            
                        },
                        {
                            name: '新会员',
                            type: 'bar',
                            stack: '广告',
                            //data: [220, 182, 191, 234, 290, 330, 310]
                            data: [<%=QtyListNew%>]
                        }
                    ]
                }
                );
        }
    );
    </script>

    </form>
</asp:Content>
