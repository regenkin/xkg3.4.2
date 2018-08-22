using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Notify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_Pay : System.Web.UI.Page
	{
		protected System.Collections.Generic.List<OrderInfo> orderlist;

		protected string OrderId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			NotifyClient notifyClient;
			if (masterSettings.EnableSP)
			{
				notifyClient = new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
			}
			else
			{
				notifyClient = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
			}
			PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
			if (payNotify == null)
			{
				return;
			}
			this.OrderId = payNotify.PayInfo.OutTradeNo;
			if (this.OrderId.StartsWith("B"))
			{
				this.DoOneTao(this.OrderId, payNotify.PayInfo);
				base.Response.End();
			}
			this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
			if (this.orderlist.Count == 0)
			{
				base.Response.Write("success");
				return;
			}
			foreach (OrderInfo current in this.orderlist)
			{
				current.GatewayOrderId = payNotify.PayInfo.TransactionId;
			}
			this.UserPayOrder();
		}

		private void DoOneTao(string Pid, PayInfo PayInfo)
		{
			OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, Pid, "");
			if (addParticipant == null)
			{
				base.Response.Write("success");
				return;
			}
			addParticipant.PayTime = new System.DateTime?(System.DateTime.Now);
			addParticipant.PayWay = "weixin";
			addParticipant.PayNum = Pid;
			addParticipant.Remark = "订单已支付：支付金额为￥" + PayInfo.TotalFee.ToString();
			if (!addParticipant.IsPay && OneyuanTaoHelp.SetPayinfo(addParticipant))
			{
				OneyuanTaoHelp.SetOneyuanTaoFinishedNum(addParticipant.ActivityId, 0);
			}
			else
			{
				Globals.Debuglog(JsonConvert.SerializeObject(PayInfo), "_Debuglog.txt");
			}
			base.Response.Write("success");
		}

		private void UserPayOrder()
		{
			foreach (OrderInfo current in this.orderlist)
			{
				if (current.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					base.Response.Write("success");
					return;
				}
			}
			foreach (OrderInfo current2 in this.orderlist)
			{
				if (current2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(current2))
				{
					current2.OnPayment();
					base.Response.Write("success");
				}
			}
		}
	}
}
