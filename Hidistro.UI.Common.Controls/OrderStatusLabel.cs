using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderStatusLabel : Literal
	{
		private bool _IsShowToUser;

		public object OrderStatusCode
		{
			get
			{
				return this.ViewState["OrderStatusCode"];
			}
			set
			{
				this.ViewState["OrderStatusCode"] = value;
			}
		}

		public object Gateway
		{
			get;
			set;
		}

		public bool IsShowToUser
		{
			get
			{
				return this._IsShowToUser;
			}
			set
			{
				this._IsShowToUser = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			OrderStatus orderStatus = (OrderStatus)this.OrderStatusCode;
			if (this.Gateway != null && this.Gateway.ToString() == "hishop.plugins.payment.podrequest" && orderStatus == OrderStatus.WaitBuyerPay)
			{
				base.Text = "等待发货";
			}
			else if (this._IsShowToUser && orderStatus == OrderStatus.SellerAlreadySent)
			{
				base.Text = "等待收货";
			}
			else
			{
				base.Text = OrderInfo.GetOrderStatusName(orderStatus);
			}
			base.Render(writer);
		}
	}
}
