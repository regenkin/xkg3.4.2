<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="MemberRegion.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.MemberRegion" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

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
                    <h2>会员地区分布</h2>
                </div>
        
                <div class="mapcharts clearfix">
                    <div class="map fl">
                        <div id="main"></div>
                    </div>
                    <div class="fr maptabl">
                        <table class="table">
                            <caption>TOP10省份</caption>
                            <thead>
                                <tr>
                                    <th>地区</th>
                                    <th>数量</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptList" runat="server">
             
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("RegionName" )%></td>
                                            <td><%# Eval("TotalRec" )%></td>
                                        </tr>
                                        
                                    </ItemTemplate>
                                </asp:Repeater>

                                 
                            </tbody>
                        </table>
                    </div>
                </div>
 <%--            </div>
       </div>
         
    </div>--%>
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
                title : {
                    text: '各地区会员分布数',
                    x:'center'
                },
                tooltip : {
                    trigger: 'item'
                },
                legend: {
                    orient: 'vertical',
                    x:'left',
                    data:['会员分布数']
                },
                dataRange: {
                    min: 0,
                    max: <%=MaxQty%>,
                    //max: 2500,
                    x: 'left',
                    y: 'bottom',
                    text:['高','低'],           // 文本，默认为数值文本
                    calculable : true
                },
                toolbox: {
                    show: false,
                    orient : 'vertical',
                    x: 'right',
                    y: 'center',
                    feature : {
                        mark : {show: true},
                        dataView : {show: true, readOnly: false},
                        restore : {show: true},
                        saveAsImage : {show: true}
                    }
                },
                roamController: {
                    show: false,
                    x: 'right',
                    mapTypeControl: {
                        'china': true
                    }
                },
                series : [
                    {
                        name: '会员分布数',
                        type: 'map',
                        mapType: 'china',
                        roam: false,
                        itemStyle:{
                            normal:{label:{show:true}},
                            emphasis:{label:{show:true}}
                        },
                        data: [
                            <%=QtyList1%>
                            //{name: '北京',value: Math.round(Math.random()*1000)},
                            //{name: '天津',value: Math.round(Math.random()*1000)},
                            //{name: '上海',value: Math.round(Math.random()*1000)},
                            //{name: '重庆',value: Math.round(Math.random()*1000)},
                            //{name: '河北',value: Math.round(Math.random()*1000)},
                            //{name: '河南',value: Math.round(Math.random()*1000)},
                            //{name: '云南',value: Math.round(Math.random()*1000)},
                            //{name: '辽宁',value: Math.round(Math.random()*1000)},
                            //{name: '黑龙江',value: Math.round(Math.random()*1000)},
                            //{name: '湖南',value: Math.round(Math.random()*1000)},
                            //{name: '安徽',value: Math.round(Math.random()*1000)},
                            //{name: '山东',value: Math.round(Math.random()*1000)},
                            //{name: '新疆',value: Math.round(Math.random()*1000)},
                            //{name: '江苏',value: Math.round(Math.random()*1000)},
                            //{name: '浙江',value: Math.round(Math.random()*1000)},
                            //{name: '江西',value: Math.round(Math.random()*1000)},
                            //{name: '湖北',value: Math.round(Math.random()*1000)},
                            //{name: '广西',value: Math.round(Math.random()*1000)},
                            //{name: '甘肃',value: Math.round(Math.random()*1000)},
                            //{name: '山西',value: Math.round(Math.random()*1000)},
                            //{name: '内蒙古',value: Math.round(Math.random()*1000)},
                            //{name: '陕西',value: Math.round(Math.random()*1000)},
                            //{name: '吉林',value: Math.round(Math.random()*1000)},
                            //{name: '福建',value: Math.round(Math.random()*1000)},
                            //{name: '贵州',value: Math.round(Math.random()*1000)},
                            //{name: '广东',value: Math.round(Math.random()*1000)},
                            //{name: '青海',value: Math.round(Math.random()*1000)},
                            //{name: '西藏',value: Math.round(Math.random()*1000)},
                            //{name: '四川',value: Math.round(Math.random()*1000)},
                            //{name: '宁夏',value: Math.round(Math.random()*1000)},
                            //{name: '海南',value: Math.round(Math.random()*1000)},
                            //{name: '台湾',value: Math.round(Math.random()*1000)},
                            //{name: '香港',value: Math.round(Math.random()*1000)},
                            //{name: '澳门',value: Math.round(Math.random()*1000)}
                        ]
                    }
                ]
            });
        }
    );
    </script>


<hr />

       

    </form>
</asp:Content>
