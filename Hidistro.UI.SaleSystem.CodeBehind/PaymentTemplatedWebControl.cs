using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class PaymentTemplatedWebControl : SimpleTemplatedWebControl
	{
		private readonly bool isBackRequest;

		protected PaymentNotify Notify;

		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		protected System.Collections.Generic.List<OrderInfo> orderlist;

		public PaymentTemplatedWebControl(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.isBackRequest)
			{
				if (!base.LoadHtmlThemedControl())
				{
					throw new SkinNotFoundException(this.SkinPath);
				}
				this.AttachChildControls();
			}
			this.DoValidate();
		}

		private void DoValidate()
		{
			System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
			Globals.Debuglog("订单支付：0-" + JsonConvert.SerializeObject(this.Notify), "_Debuglog.txt");
			if (this.isBackRequest)
			{
				this.Notify.ReturnUrl = Globals.FullPath("/pay/PaymentReturn_url.aspx") + "?" + this.Page.Request.Url.Query;
			}
			Globals.Debuglog("订单支付：1-" + JsonConvert.SerializeObject(this.Notify), "_Debuglog.txt");
			this.OrderId = this.Notify.GetOrderId();
			this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
			if (this.orderlist.Count != 0)
			{
				int modeId = 0;
				foreach (OrderInfo current in this.orderlist)
				{
					this.Amount += current.GetAmount();
					current.GatewayOrderId = this.Notify.GetGatewayOrderId();
					modeId = current.PaymentTypeId;
				}
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
				if (paymentMode == null)
				{
					this.ResponseStatus(true, "gatewaynotfound");
				}
				else
				{
					this.Notify.Finished += new System.EventHandler<FinishedEventArgs>(this.Notify_Finished);
					this.Notify.NotifyVerifyFaild += new System.EventHandler(this.Notify_NotifyVerifyFaild);
					this.Notify.Payment += new System.EventHandler(this.Notify_Payment);
					string configXml = HiCryptographer.Decrypt(paymentMode.Settings);
					this.Notify.VerifyNotify(30000, configXml);
				}
			}
		}

		private void Notify_Payment(object sender, System.EventArgs e)
		{
			this.UserPayOrder();
		}

		private void Notify_NotifyVerifyFaild(object sender, System.EventArgs e)
		{
			this.ResponseStatus(false, "verifyfaild");
		}

		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			if (e.IsMedTrade)
			{
				this.FinishOrder();
			}
			else
			{
				this.UserPayOrder();
			}
		}

		protected abstract void DisplayMessage(string status);

		private void ResponseStatus(bool success, string status)
		{
			if (this.isBackRequest)
			{
				this.Notify.WriteBack(System.Web.HttpContext.Current, success);
			}
			else
			{
				this.DisplayMessage(status);
			}
		}

		private void UserPayOrder()
		{
			foreach (OrderInfo current in this.orderlist)
			{
				if (current.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					this.ResponseStatus(true, "success");
					return;
				}
			}
			foreach (OrderInfo current in this.orderlist)
			{
				if (current.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(current))
				{
					current.OnPayment();
					this.ResponseStatus(true, "success");
				}
				else
				{
					this.ResponseStatus(false, "fail");
				}
			}
		}

		private void FinishOrder()
		{
			foreach (OrderInfo current in this.orderlist)
			{
				if (current.OrderStatus == OrderStatus.Finished)
				{
					this.ResponseStatus(true, "success");
					return;
				}
			}
			foreach (OrderInfo current in this.orderlist)
			{
				if (current.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && MemberProcessor.ConfirmOrderFinish(current))
				{
					this.ResponseStatus(true, "success");
				}
				else
				{
					this.ResponseStatus(false, "fail");
				}
			}
		}
	}
}
