<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="UserInCreaseStatistics.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.UserInCreaseStatistics" %>

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
      <form id="form1" runat="server"  >
        <div class="page-header">
            <h2>会员增长统计</h2>
        </div> 
        <div class="set-switch">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-xs-1 pad resetSize control-label" for="setdate">操作时间：</label>
                    <div class="form-inline journal-query">
                        <div class="form-group">                        
                            <uc1:ucDateTimePicker runat="server" ID="txtBeginDate" class="form-control resetSize" />
                            <%-- <input type="text" class="form-control resetSize" id="setdate" placeholder="创建日期">--%>
                            &nbsp;&nbsp;至&nbsp;
                             <uc1:ucDateTimePicker runat="server" ID="txtEndDate" class="form-control resetSize" /> 
                            <%-- <input type="text" class="form-control resetSize" placeholder="创建日期">--%>
                        </div>

                          <asp:Button ID="btnSearch" runat="server" Text="查询" class="btn resetSize btn-primary" />
     
                        <div class="form-group">
                            <label for="exampleInputName2">查询日期</label>
 
                            <asp:Button ID="btnWeekView" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="btnWeekView_Click"   />
                            <asp:Button ID="btnMonthView" runat="server" class="btn resetSize btn-default" Text="最近1个月" OnClick="btnMonthView_Click" />

                        </div>
                      <%--  <p class="form-group reportForm"><span class="glyphicon glyphicon-th"></span></p>
                        <a href="javascript:void(0)">下载报表</a>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="shop-chart">
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
                tooltip : {
                    trigger: 'axis'
                },
                legend: {
                    data:['新增会员数']
                },
                calculable : true,
                xAxis : [
                    {
                        type : 'category',
                        boundaryGap : false,
                        //data : ['2015-07-09','2015-07-10','2015-07-11','2015-07-12','2015-07-13','2015-07-13','2015-07-14']
                        data: [ <%=DateList %>]
                    }
                ],
                yAxis : [
                    {
                        type : 'value'
                    }
                ],
                series : [
                    {
                        name:'会员数',
                        type:'line',
                        //data:[600, 1322, 101, 134, 90, 230, 210]
                        data: [ <%=QtyList1%>]
                    } 
                ]
            });
        }
    );
    </script>

     </form>

     
</asp:Content>
