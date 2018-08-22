using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderItemsStatus : Literal
	{
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

		public object OrderId
		{
			get
			{
				return this.ViewState["OrderId"];
			}
			set
			{
				this.ViewState["OrderId"] = value;
			}
		}

		public object ProductId
		{
			get
			{
				return this.ViewState["ProductId"];
			}
			set
			{
				this.ViewState["ProductId"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = this.GetOrderItemStatus((OrderStatus)this.OrderStatusCode);
			base.Render(writer);
		}

		public string GetOrderItemStatus(OrderStatus orderitem)
		{
			string result = "";
			switch (orderitem)
			{
			case OrderStatus.BuyerAlreadyPaid:
				result = string.Concat(new object[]
				{
					"<a class=\"btn-have\" href=\"RequestReturn.aspx?orderId=",
					this.OrderId,
					"&ProductId=",
					this.ProductId,
					"\">申请退款</a>"
				});
				break;
			case OrderStatus.SellerAlreadySent:
				result = string.Concat(new object[]
				{
					"<a class=\"btn-have\" href=\"RequestReturn.aspx?orderId=",
					this.OrderId,
					"&ProductId=",
					this.ProductId,
					"\">申请退货</a>"
				});
				break;
			case OrderStatus.ApplyForRefund:
				result = "<a class=\"btn-have\">退款审核中</a>";
				break;
			case OrderStatus.ApplyForReturns:
				result = "<a class=\"btn-have\">退货中</a>";
				break;
			case OrderStatus.Refunded:
				result = "<a class=\"btn-have\">退款完成</a>";
				break;
			case OrderStatus.Returned:
				result = "<a class=\"btn-have\">退货完成</a>";
				break;
			}
			return result;
		}
	}
}
