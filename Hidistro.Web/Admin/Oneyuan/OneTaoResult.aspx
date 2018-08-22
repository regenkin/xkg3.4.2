<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="OneTaoResult.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.OneTaoResult" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" TagName="ViewTab" Src="~/Admin/Oneyuan/OneTaoViewTab.ascx" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #mainInfo { margin-top: 10px; display:none}
        .prizetb{width:100%;margin-top:20px}
        .prizetb td,th{padding-left:20px;line-height:30px}
         .prizetb tbody tr{background:#fff;border:1px solid #ccc}
         .prizetb thead tr{background:none}
         .redtd{ background: #DA4453; color: #fff; padding: 10px 20px; border-radius: 6px}
          .Graytd{ background: #c6c4c4; color: #fff; padding: 10px 20px; border-radius: 6px}
         .txtCenter{text-align:center}
         .frt{background:#F2F8FC;float: left; border: 1px solid #ccc; width: 800px; padding: 10px; border-radius: 6px}
         .flt{float: left; margin-right: 10px}
         .prizeUl {}
          .prizeUl  li{width:180px;height:50px;display:inline-block;margin-right:10px;margin-bottom:10px}
           .prizeUl  li img{float:left;margin-right:5px;width:50px;height:50px;border-radius:25px}
            .prizeUl  li p{line-height:23px;height:23px;overflow:hidden}
            .red{color:#19a7dd;font-weight:bold}
             .blue{color:#103ba3;font-weight:bold}
    </style>
    <script>

        <%=DataJson%>;

        $(function () {
            if (DataJson != null) {

                if (!DataJson.HasCalculate) {
                    $("#errid").html("活动暂未开奖！");
                    return;
                }
                else if (DataJson.HasCalculate && !DataJson.IsSuccess && typeof(PrizeCountInfo) != "undefined") {
                    $("#errid").html("活动开奖异常，开奖计算异常，订单数据不足，无法开出指定数量的中奖号！");
                }
                else if (DataJson.HasCalculate && !DataJson.IsSuccess) {
                    var htmlrs = '活动结束了，活动未达到开奖条件，未开奖成功！<br/>';
                    htmlrs += "活动标题：" + DataJson.Title + "，已参与份数：" + DataJson.FinishedNum + "份,需要满足份数：" + DataJson.ReachNum + "份";
                    $("#errid").html(htmlrs);
                    return;
                }

                if (DataJson.HasCalculate && DataJson.IsSuccess) {

                    //成功开奖时
                    $("#errid").hide();
                   
                    var listhtml = '';

                    if (typeof (WinInfo) != "undefined" && WinInfo != null) {

                        for (var l = 0; l < WinInfo.length; l++) {
                            listhtml += '<li><img src="' + WinInfo[l].UserHead + '" /><p  class="blue">' + WinInfo[l].UserName + '</p><p class="red">' + WinInfo[l].PrizeNum + '</p></li>';
                        }

                        $("#listUser").html(listhtml);
                    }

                    if (listhtml!="") {
                         $("#succeId").show();
                    }

                };

                if (typeof (PrizeCountInfo) != "undefined" && PrizeCountInfo != null) {
                    $("#mainInfo").show();

                    if (DataJson.PrizeTime == null) {
                        DataJson.PrizeTime = PrizeCountInfo[0].BuyTime;
                    }

                    //如果有开奖记录
                    var tdhtml = '<tr><td class="redtd txtCenter"  colspan="4">截止开奖时间【' + DataJson.PrizeTime + '】<br/>最后50条全站购买记录</td></tr>';
                    for (var i = 0; i < PrizeCountInfo.length; i++) {
                        PrizeCountInfo[i].BuyTime = PrizeCountInfo[i].BuyTime.replace(" ", "T");
                        var NowDate = new Date(Date.parse(PrizeCountInfo[i].BuyTime));
                     
                        tdhtml += "<tr><td>" + NowDate.Format("yyyy-MM-dd") + "</td><td>" + NowDate.Format("hh:mm:ss.S") + "</td><td>" + NowDate.Format("hmmssSSS") * 1 + "</td><td>" + PrizeCountInfo[i].UserName + "</td></tr>";

                        if (PrizeCountInfo[i].PrizeLuckInfo != null) {
                            var pa = PrizeCountInfo[i].PrizeLuckInfo.split(",");
                            //10000003,5999395112,10,2
                            if (pa[0].indexOf("重复") == -1) {
                                tdhtml += '<tr><td class="redtd"  colspan="4">';
                            }
                            else {
                                tdhtml += '<tr><td class="Graytd"  colspan="4">'; //重复的灰色
                            }
                            
                           tdhtml +=  '最后' + (i + 1) + '条取值的求和数值得：' + pa[1] + '（上面' + (i + 1) + '条购买记录时间取值相加之和）<br/>' +
                           '中奖号码　= （' + pa[1] + ' ÷ ' + pa[2] + '）取余数(' + pa[3] + ') + 10000001<br/>' +
                           '　　　　　= ' + pa[0] +
                           '</td></tr>';
                        }

                    }

                    $("#prizePlanel").html(tdhtml);
                }

            }
            else {
                $("#errid").html("活动信息不存在");
            }
        });


        Date.prototype.Format = function (fmt) { //author: meizz 
            var o = {
                "M+": this.getMonth() + 1, //月份 
                "d+": this.getDate(), //日 
                "h+": this.getHours(), //小时 
                "m+": this.getMinutes(), //分 
                "s+": this.getSeconds(), //秒 
                "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
                "S+": this.getMilliseconds() //毫秒 
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }

        

       

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2 id="txtEditInfo" runat="server">查看一元夺宝
        </h2>
    </div>
    <Hi:ViewTab ID="ViewTab1" runat="server"></Hi:ViewTab>
    <div style="border: 1px solid #ccc; line-height: 30px; padding: 20px; font-size: 16px; border-radius: 6px" id="errid">
    </div>

    <div id="succeId" style="margin-top: 10px;display:none">
         <div class="flt" style="padding-top:10px" >中奖号码：</div>
        <div class="frt" style="background:none;border:none">
            <ul class="prizeUl" id="listUser">
               
            </ul>
        </div>

    </div>

    <div id="mainInfo" style="clear:both">
        <div  class="flt">计算详情：</div>
        <div  class="frt">
            <div style="background: #DA4453; color: #fff; padding: 10px 20px; border-radius: 6px">
                <p style="font-size: 18px">计算公式</p>
                <p style="padding: 5px 0px; font-size: 16px">中奖号码 = （求和数值 ÷ 参与人次）取余数 + 10000001</p>
                <p>
                    说明：其中求和数值 = 截止该商品开奖时间点前最后50条全站购买记录，第二个中奖号码取最后51条全站购买记录，
                     <br />
                    依次类推，第N个中奖号码取最后49+N条全站购买记录；不取重复中奖号码
                </p>
            </div>

            <table class="prizetb">
                <thead>
                <tr>

                    <th width="25%">购买日期</th>
                    <th width="25%">时间</th>
                    <th width="25%">取值</th>
                    <th width="25%">会员昵称</th>
                </tr>
                </thead>
                
                <tbody id="prizePlanel">
                
                </tbody>

            </table>


        </div>



    </div>


</asp:Content>
