<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_OneTaoSubmit.aspx.cs" Inherits="Hidistro.UI.Web.Pay.wx_OneTaoSubmit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>微信支付</title>
</head>
<body>
   <script type="text/javascript">
       var CheckValue="<%=CheckValue%>";
       if(CheckValue!=""){
           alert(CheckValue);
           //如果出错时，弹出提示
          // location.href = "/vshop/MyOneTao.aspx";
       }
       else
       {
           document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
               WeixinJSBridge.invoke('getBrandWCPayRequest', <%= pay_json %>,
               function(res){
                   // alert(JSON.stringify(res));
                   if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                       // alert("夺宝活动订单支付成功!");
                       location.href = "/vshop/OneTaoPaySuccess.aspx"; //支付成功，进入
                   }
                   else
                   {
                       alert("支付取消或者失败");
                       location.href = "/vshop/OneyuanList.aspx";
                   }
              
               });
       });
       
       }
       

 
       //新的支付方法 ，需要weixinset 同时 需要保持签名的
       //这里需要强调的是，下边config和chooseWXPay中的参数名为：nonceStr、timestamp要一直，否则就会一直报错：paySign加密错误
      <%-- wx.ready(function () {

           wx.error(function(res){
               // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
               alert("公众号信息验证失败:"+res.errMsg);
           });

           var pay_json=<%= pay_json %>;
           if(pay_json.package!=null){

               if(pay_json.package.indexOf("prepay_id=wx")>-1){
                   wx.chooseWXPay(pay_json);
               }
               else{
                   alert("微信支付参数错误！"+pay_json.package);
               }
           }
           else if(pay_json.msg!=null)
           {
               alert(pay_json.msg);
           }
           else{
               alert("微信支付未配置好！");
           }

       });
      --%>

       //支付成功回调函数
       //function Paysuccess(res)
       //{
       //    if (res.err_msg == "get_brand_wcpay_request:ok") {
       //        alert("订单支付成功!点击确认进入我的订单中心");
       //        location.href = "/vshop/OneTaoPaySuccess.aspx"; //支付成功，进入
       //    }
       //    else {
       //        alert("支付取消或者失败");
       //    }
       //}
     

</script>
</body>
</html>
