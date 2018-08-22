<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Default" 
    CodeBehind="Default.aspx.cs" 
 MasterPageFile="~/Admin/AdminNew.Master"  %>

<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/admin/js/ZeroClipboard.min.js"></script>  <%--必须在echarts.js前引用--%>
    <script src="/admin/js/echarts.js"></script>


 <script type="text/javascript">
     $(document).ready(function () {
         //var copy = new ZeroClipboard(document.getElementById("hrefUrl"), {
         var copy = new ZeroClipboard(document.getElementById("hrefUrl"), {
             moviePath: "/admin/js/ZeroClipboard.swf"
         });
         copy.on('complete', function (client, args) {
             HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
         });
     });


     function copyurl(obj) {
         // alert(document.getElementById(obj));
         var copy = new ZeroClipboard(document.getElementById(obj), {
             moviePath: "/admin/js/ZeroClipboard.swf"
         });
         copy.on('complete', function (client, args) {

             HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
         });
     }

</script>
      

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          
    <form runat="server" id="aspForm">
                <div class="shopuser clearfix">
                    <div class="fl shopuser-img clearfix">
                        <div class="img fl">
                            <img src="/Admin/Shop/Public/images/80x80.png" runat="server" id="imgLogo">
                        </div>
                        <h2 class="fl"><asp:Literal runat="server" ID="lbShopName" ></asp:Literal> </h2>
                    </div>
                    <div class="shop-link fr">
                        <a class="btn btn-success mr20 btn-sm" href="/admin/goods/selectcategory.aspx">发布商品</a>
                        <%--<a class="btn btn-primary btn-sm" href="<%=showUrl %>" >访问店铺</a>--%>
                        <a class="btn btn-primary btn-sm" href="javascript:void(0)" >访问店铺</a>
                         <div class="vechar">
                             <p>手机扫码访问：</p>
                             <img alt="加载中..." 
                             src="http://s.jiathis.com/qrcode.php?url=<%=showUrl%>" />
                             <p>
                                 <input type="text" id='txtShopUrl'  name="txtShopUrl"  value='<%=showUrl %>' disabled="" style="display: none">
                                 <%--<button type="button" class="btn btn-primary" data-clipboard-target="txtSholUrl" id="hrefUrl">复制</button>--%>
                                 <a  href="#"  id="hrefUrl"  class="my_clip_button" data-clipboard-target="txtShopUrl">复制页面链接</a>
                                 <a class="fr" href="<%=showUrl %>"  target="_blank"  >电脑上查看</a>
                             </p>
                         </div>

                    </div>
                </div>
                <div class="data-shop">
                    <div class="datashop-top">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="javascript:void(0)">店铺实时数据</a></li>
                        </ul>
                    </div>
                    <div class="data-list clearfix">
                        <div class="data-box fl">
                            <h4>待发货订单</h4>
                            <p><a href="trade/BuyerAlreadyPaid.aspx"><%=WaitSendOrderQty %></a></p>
                        </div>
                        <div class="data-box fl">
                            <h4>今日订单数</h4>
                            <p class="resetlh"><a href="trade/manageorder.aspx?PageSize=10&OrderType=0&OrderStatus=0&StartDate=<%=DateTime.Today.ToString("yyyy-MM-dd")%>&EndDate=<%=DateTime.Today.ToString("yyyy-MM-dd")%>"><%=OrderQty_Today %></a></p>
                            <h4 class="resetbor">昨日订单数：<%=OrderQty_Yesterday %></h4>
                        </div>
                        <div class="data-box fl">
                            <h4>今日成交额</h4>
                            <p class="resetlh">￥<%=OrderAmountFee_Today %></p>
                            <h4 class="resetbor">昨日成交额：￥<%=OrderAmountFee_Yesterday %></h4>
                        </div>
                        <div class="data-box fl">
                            <h4>售后订单</h4>
                            <p ><%--<asp:Label  ID="lbServiceOrderQty2" runat="server">0</asp:Label>--%>  
                                <asp:HyperLink ID="lbServiceOrderQty" runat="server" NavigateUrl="trade/returnsapply.aspx" >0</asp:HyperLink>
                            </p>
                            <%--<p class="setcolor"><a href="trade/returnsapply.aspx"><%=ServiceOrderQty %></a></p>--%>
                        </div>
                        <div class="shopnumber-list fl">
                            <ul>
                                <li><span>商品数：</span><a href="goods/productonsales.aspx"><%=GoodsQty %></a></li>
                                <li><span>会员数：</span><a href="member/managemembers.aspx"><%=MemberQty %></a></li>
                                <li class="noborder"><span>分销商数：</span><a href="Fenxiao/distributorlist.aspx"><%=DistributorQty %></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="shop-chart">
                    <h3>店铺经营趋势</h3>
                    <div class="echarts-map" id="main"></div>
                </div>
                <div class="shopdatatable clearfix">
                    <div class="shopdatatable-left fl">
                        <table class="table table-hover">
                            <caption>分销商佣金排名<p><span class="active">近7天</span><%--<span>30天</span>--%></p></caption>
                            <thead>
                                <tr>
                                    <th>店铺名称</th>
                                    <th>销售订单数</th>
                                    <th>佣金</th>
                                    <th>排名</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptDistributor" runat="server" >
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("StoreName")%></td>
                                            <td><%# Eval("Ordernums")%></td>
                                            
                                            <td><%# Convert.ToDecimal(Eval("CommTotalSum")).ToString("N2") %></td>
                                            <td><%# Eval("rownum")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div class="shopdatatable-right fr">
                        <table class="table">
                            <caption>会员消费排名<p><span class="active">近7天</span><%--<span>30天</span>--%></p></caption>
                            <thead>
                                <tr>
                                <th>会员名</th>
                                <th>购买订单数</th>
                                <th>交易额</th>
                                <th>排名</th>
                                </tr>
                            </thead>
                            <tbody>
                               <asp:Repeater ID="rptMember" runat="server" >
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("UserName")%></td>
                                            <td><%# Eval("OrderQty")%></td>
                                            <td><%# Convert.ToDecimal( Eval("ValidOrderTotal")).ToString("N2") %></td>
                                            <td><%# Eval("Rank")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                    </div>
                </div>
     
  
     <script type="text/javascript">
         // Step:3 conifg ECharts's path, link to echarts.js from current page.
         // Step:3 为模块加载器配置echarts的路径，从当前页面链接到echarts.js，定义所需图表路径
         require.config({
             paths: {
                 echarts: '/admin/js'
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
                         data: ['订单数', '新增分销商数', '新增会员数'],
                         selectedMode: false
                     },
                     calculable: true,
                     xAxis: [
                         {
                             type: 'category',
                             boundaryGap: false,
                             data: [ <%=DateList %>]
                        //data : ['2015-07-09','2015-07-10','2015-07-11','2015-07-12','2015-07-13','2015-07-13','2015-07-14']
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
                    }
                ],
                series: [
                    {
                        name: '订单数',
                        type: 'line',
                        //stack: '总量',
                        data: [ <%=QtyList1%>]
                        //data: [  1200, 1322, 101, 134, 90, 230, 210]
                    },
                    {
                        name: '新增分销商数',
                        type: 'line',
                        //stack: '总量',
                        data: [ <%=QtyList2%>]
                    },
                    {
                        name: '新增会员数',
                        type: 'line',
                        //stack: '总量',
                        data: [ <%=QtyList3%>]
                    }
                ]
            });
        }
    );
    </script>
<script>
    $(function () {
        $('.shopdatatable caption p span').click(function () {
            $(this).parent().find('span').removeClass('active');
            $(this).addClass('active');
        })
        $('.shop-link > a:last').click(function (event) {
            if ($('.shop-link .vechar').css('display') == 'none') {
                $('.shop-link .vechar').show();
            } else {
                $('.shop-link .vechar').hide();
            }
            event.stopPropagation();
        })
        $(document).click(function () {
            $('.shop-link .vechar').hide();
        })
    })
</script>

         
  
        </form>

</asp:Content>
 