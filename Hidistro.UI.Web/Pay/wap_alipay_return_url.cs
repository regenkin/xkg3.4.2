using Hidistro.ControlPanel.OutPay.App;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wap_alipay_return_url : System.Web.UI.Page
	{
		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.Write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\"/>");
			System.Collections.Generic.SortedDictionary<string, string> requestGet = this.GetRequestGet();
			if (requestGet.Count <= 0)
			{
				base.Response.Write("<p style=\"font-size:16px;\">参数为空，支付异常！</p>");
				return;
			}
			Notify notify = new Notify();
			bool flag = notify.Verify(requestGet, base.Request.QueryString["notify_id"], base.Request.QueryString["sign"]);
			if (!flag)
			{
				base.Response.Write("<p style=\"font-size:16px;\">签名验证失败，可能支付密钥已经被修改</p>");
				return;
			}
			this.OrderId = base.Request.QueryString["out_trade_no"];
			string arg_91_0 = base.Request.QueryString["trade_no"];
			string arg_A7_0 = base.Request.QueryString["trade_status"];
			System.Collections.Generic.List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
			if (orderMarkingOrderInfo.Count == 0)
			{
				base.Response.Write("<p style=\"font-size:16px;\">找不到对应的订单，你付款的订单可能已经被删除</p>");
				return;
			}
			if (base.Request.QueryString["trade_status"] == "TRADE_FINISHED" || base.Request.QueryString["trade_status"] == "TRADE_SUCCESS")
			{
				this.Amount = decimal.Parse(base.Request.QueryString["total_fee"]);
				this.UserPayOrder();
				return;
			}
			base.Response.Write(string.Format("<p style=\"font-size:16px;color:#ff0000;\">支付失败，<br><a href=\"{0}\">查看订单</a></p>", "Vshop/MemberOrders.aspx?Status=3"));
		}

		private void UserPayOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}<br><a href=\"{2}\">查看订单</a></p>", this.OrderId, this.Amount.ToString("F"), "Vshop/MemberOrders.aspx?Status=3"));
			}
		}

		public System.Collections.Generic.SortedDictionary<string, string> GetRequestGet()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection queryString = base.Request.QueryString;
			string[] allKeys = queryString.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.QueryString[allKeys[i]]);
			}
			return sortedDictionary;
		}
	}
}
