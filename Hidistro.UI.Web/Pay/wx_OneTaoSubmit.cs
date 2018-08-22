using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_OneTaoSubmit : System.Web.UI.Page
	{
		public string pay_json = "{\"msg\":\"非正常访问!\"}";

		public int shareid;

		public string CheckValue = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			if (string.IsNullOrEmpty(text))
			{
				this.pay_json = "{\"msg\":\"订单参数错误!\"}";
				return;
			}
			OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, text, "");
			if (addParticipant == null)
			{
				this.pay_json = "{\"msg\":\"订单不存在了!\"}";
				return;
			}
			OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(addParticipant.ActivityId);
			if (oneyuanTaoInfoById == null)
			{
				this.pay_json = "{\"msg\":\"活动已取消，禁止参与!\"}";
				return;
			}
			OneTaoState oneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
			if (oneTaoState != OneTaoState.进行中)
			{
				this.pay_json = "{\"msg\":\"您来晚了，活动已结束!\"}";
				return;
			}
			if (oneTaoState != OneTaoState.进行中)
			{
				this.pay_json = "{\"msg\":\"您来晚了，活动已结束!\"}";
				return;
			}
			if (oneyuanTaoInfoById.ReachType == 1 && oneyuanTaoInfoById.FinishedNum + addParticipant.BuyNum > oneyuanTaoInfoById.ReachNum)
			{
				this.pay_json = "{\"msg\":\"活动已满员，您来晚了!\"}";
				return;
			}
			decimal totalPrice = addParticipant.TotalPrice;
			PackageInfo packageInfo = new PackageInfo();
			packageInfo.Body = "一元夺宝。当前活动编号：" + addParticipant.ActivityId;
			packageInfo.NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host);
			packageInfo.OutTradeNo = text;
			packageInfo.TotalFee = (int)(totalPrice * 100m);
			if (packageInfo.TotalFee < 1m)
			{
				packageInfo.TotalFee = 1m;
			}
			string openId = "";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				openId = currentMember.OpenId;
			}
			else
			{
				this.pay_json = "{\"msg\":\"用户OPENID不存在，无法支付!\"}";
			}
			packageInfo.OpenId = openId;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			PayClient payClient;
			if (masterSettings.EnableSP)
			{
				payClient = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
			}
			else
			{
				payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
			}
			if (!payClient.checkSetParams(out this.CheckValue))
			{
				return;
			}
			if (!payClient.checkPackage(packageInfo, out this.CheckValue))
			{
				return;
			}
			PayRequestInfo payRequestInfo = payClient.BuildPayRequest(packageInfo);
			this.pay_json = this.ConvertPayJson(payRequestInfo);
			if (!payRequestInfo.package.ToLower().StartsWith("prepay_id=wx"))
			{
				this.CheckValue = payRequestInfo.package;
			}
		}

		public string ConvertPayJson(PayRequestInfo req)
		{
			string str = "{";
			str = str + "\"appId\":\"" + req.appId + "\",";
			str = str + "\"timeStamp\":\"" + req.timeStamp + "\",";
			str = str + "\"nonceStr\":\"" + req.nonceStr + "\",";
			str = str + "\"package\":\"" + req.package + "\",";
			str = str + "\"signType\":\"" + req.signType + "\",";
			str = str + "\"paySign\":\"" + req.paySign + "\"";
			return str + "}";
		}
	}
}
