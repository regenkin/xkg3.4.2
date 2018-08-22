<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MemberGaiKuang.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.MemberGaiKuang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/echarts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  
                <div class="page-header">
                    <h2>店铺会员概况</h2>
                </div>
                <div class="member-info clearfix">
                    <div class="fl">
                        <p class="line">活跃会员数<%=ActiveUserQty %></p>
                        <div class="blank"></div>
                        <p class="line">休眠会员数<%=SleepUserQty %></p>
                        <div class="radius left">
                            <div class="bigsize">
                                <p>成交会员</p>
                                <span><%=SuccessTradeUserQty %></span>
                            </div>
                            <p>昨日新增</p>
                            <span><%=SuccessTradeUserQty_Yesterday %></span>
                        </div>
                    </div>
                    <div class="fr">
                        <p class="line">收藏会员数<%=CollectUserQty %></p>
                        <div class="blank"></div>
                        <p class="line">加购物车会员数<%=CartUserQty %></p>
                        <div class="radius right">
                            <div class="bigsize">
                                <p>潜在会员</p>
                                <span><%=PotentialUserQty %></span>
                            </div>
                            <p>昨日新增</p>
                            <span><%=PotentialUserQty_Yesterday %></span>
                        </div>
                    </div>
                </div>
                <div class="membership">
                    <p>会员总数<span><%=MemberQty %></span></p>
                    <div class="lineBox"></div>
                    <div class="triangle"></div>
                </div>
                <div class="memberChart clearfix">
                    <div class="chartLedt fl" id="radiuschart"></div>
                    <div class="chartRight fr" id="linechart"></div>
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
            'echarts/chart/pie',
            'echarts/chart/funnel',
            'echarts/chart/bar',
            'echarts/chart/line',
            'echarts/chart/map'
        ],
        function (ec) {
            //--- 折柱 ---
            var myChart = ec.init(document.getElementById('radiuschart'));
            myChart.setOption({
                title : {
                    text: '分组数据',
                    x:'center'
                },
                tooltip : {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    orient : 'vertical',
                    x: 'left',
                    data: [<%=MemberGradeList%>]
                    //data:['普通会员','高级会员','VIP会员','视频广告','搜索引擎']
                },
                calculable : true,
                series : [
                    {
                        name:'会员类别',
                        type:'pie',
                        radius : '55%',
                        center: ['50%', '60%'],
                        data: [
                            <%=QtyList_Grade%>
                            //{value:335, name:'普通会员'},
                            //{value:310, name:'高级会员'},
                            //{value:234, name:'VIP会员'},
                            //{value:135, name:'视频广告'},
                            //{value:1548, name:'搜索引擎'}
                        ]
                    }
                ]
            });
            var myChart1 = ec.init(document.getElementById('linechart'));
            myChart1.setOption({
                title : {
                    text: '会员分布地区(前9名)',
                    subtext: '数据来自网络'
                },
                tooltip : {
                    trigger: 'axis'
                },
                legend: {
                    data: ['区域统计']
                },
                calculable : true,
                xAxis : [
                    {
                        type : 'value',
                        boundaryGap : [0, 0.01]
                    }
                ],
                yAxis : [
                    {
                        type: 'category',
                        data: [<%=RegionNameList%>]
                        //data : ['湖南','湖北','美国','印度','中国','世界人口(万)']
                    }
                ],
                series : [
                    {
                        name:'区域统计',
                        type: 'bar',
                        data: [<%=RegionQtyList%>]
                        //data:[18203, 23489, 29034, 104970, 131744, 630230]
                    }
                ]
            });            
        }
    );
    </script>


</asp:Content>
