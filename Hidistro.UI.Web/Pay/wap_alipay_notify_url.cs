using Hidistro.ControlPanel.OutPay.App;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wap_alipay_notify_url : System.Web.UI.Page
	{
		protected System.Collections.Generic.List<OrderInfo> orderlist;

		protected string OrderId;

		private void DoOneTao(string out_trade_no, string trade_status, string trade_no, System.Collections.Generic.SortedDictionary<string, string> sPara)
		{
			if (trade_status == "TRADE_SUCCESS" || trade_status == "TRADE_FINISHED")
			{
				OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, out_trade_no, "");
				if (addParticipant == null)
				{
					base.Response.Write("success");
					Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
					return;
				}
				addParticipant.PayTime = new System.DateTime?(System.DateTime.Now);
				addParticipant.PayWay = "alipay";
				addParticipant.PayNum = trade_no;
				addParticipant.Remark = "订单已支付：支付金额为￥" + sPara["total_fee"];
				if (!addParticipant.IsPay && OneyuanTaoHelp.SetPayinfo(addParticipant))
				{
					OneyuanTaoHelp.SetOneyuanTaoFinishedNum(addParticipant.ActivityId, 0);
				}
				else
				{
					Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
				}
			}
			else
			{
				Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
			}
			base.Response.Write("success");
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count > 0)
			{
				Notify notify = new Notify();
				bool flag = notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]);
				if (flag)
				{
					string text = base.Request.Form["out_trade_no"];
					string text2 = base.Request.Form["trade_no"];
					string text3 = base.Request.Form["trade_status"];
					if (!string.IsNullOrEmpty(text) && text.StartsWith("B"))
					{
						this.DoOneTao(text, text3, text2, requestPost);
						base.Response.End();
					}
					if (text3 == "TRADE_SUCCESS" || text3 == "TRADE_FINISHED")
					{
						this.OrderId = text;
						this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
						if (this.orderlist.Count == 0)
						{
							base.Response.Write("success");
							return;
						}
						foreach (OrderInfo current in this.orderlist)
						{
							current.GatewayOrderId = text2;
						}
						foreach (OrderInfo current2 in this.orderlist)
						{
							if (current2.OrderStatus == OrderStatus.BuyerAlreadyPaid)
							{
								base.Response.Write("success");
								return;
							}
						}
						foreach (OrderInfo current3 in this.orderlist)
						{
							if (current3.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(current3))
							{
								current3.OnPayment();
								base.Response.Write("success");
							}
						}
					}
					base.Response.Write("success");
					return;
				}
				base.Response.Write("fail");
				return;
			}
			else
			{
				base.Response.Write("无通知参数");
			}
		}

		private System.Collections.Generic.SortedDictionary<string, string> GetRequestPost()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.Form[allKeys[i]]);
			}
			return sortedDictionary;
		}
	}
}
