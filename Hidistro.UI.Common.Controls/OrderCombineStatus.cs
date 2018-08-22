using Hidistro.ControlPanel.Sales;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderCombineStatus : Literal
	{
		private bool _IsShowToUser;

		private string _DetailUrl = string.Empty;

		private string _OrderID = string.Empty;

		private string _SkuID = string.Empty;

		private int _Type;

		private int _OrderItemID;

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

		public object OrderItemsStatusCode
		{
			get
			{
				return this.ViewState["OrderItemsStatusCode"];
			}
			set
			{
				this.ViewState["OrderItemsStatusCode"] = value;
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

		public string DetailUrl
		{
			get
			{
				return this._DetailUrl;
			}
			set
			{
				this._DetailUrl = value;
			}
		}

		public string OrderID
		{
			get
			{
				return this._OrderID;
			}
			set
			{
				this._OrderID = value;
			}
		}

		public string SkuID
		{
			get
			{
				return this._SkuID;
			}
			set
			{
				this._SkuID = value;
			}
		}

		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		public int OrderItemID
		{
			get
			{
				return this._OrderItemID;
			}
			set
			{
				this._OrderItemID = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = this.GetOrderCombineStatus((OrderStatus)this.OrderStatusCode, (OrderStatus)this.OrderItemsStatusCode);
			base.Render(writer);
		}

		public string GetOrderCombineStatus(OrderStatus orderStatusCode, OrderStatus OrderItemsStatusCode)
		{
			string text;
			if (this.Gateway != null && this.Gateway.ToString() == "hishop.plugins.payment.podrequest" && orderStatusCode == OrderStatus.WaitBuyerPay)
			{
				text = "等待发货";
			}
			else if (this._IsShowToUser && orderStatusCode == OrderStatus.SellerAlreadySent)
			{
				text = "等待收货";
			}
			else
			{
				text = OrderInfo.GetOrderStatusName(orderStatusCode);
			}
			if (this._OrderItemID == 0 && (string.IsNullOrEmpty(this._OrderID) || string.IsNullOrEmpty(this._SkuID)))
			{
				text = (text ?? "");
			}
			else
			{
				string text2 = string.Empty;
				if (this._Type != 1)
				{
					RefundInfo byOrderIdAndProductID = RefundHelper.GetByOrderIdAndProductID(this._OrderID, 0, this._SkuID, this._OrderItemID);
					if (byOrderIdAndProductID != null)
					{
						switch (byOrderIdAndProductID.HandleStatus)
						{
						case RefundInfo.Handlestatus.Applied:
							text2 = "退款中";
							break;
						case RefundInfo.Handlestatus.Refunded:
							text2 = "已退款";
							break;
						case RefundInfo.Handlestatus.Refused:
							text2 = "拒绝退款";
							break;
						case RefundInfo.Handlestatus.NoneAudit:
							text2 = "退款待审核";
							break;
						case RefundInfo.Handlestatus.HasTheAudit:
							text2 = "退款已审核";
							break;
						case RefundInfo.Handlestatus.NoRefund:
							text2 = "待退款";
							break;
						case RefundInfo.Handlestatus.AuditNotThrough:
							text2 = "拒绝退款";
							break;
						case RefundInfo.Handlestatus.RefuseRefunded:
							text2 = "拒绝退款";
							break;
						}
					}
				}
				if (!string.IsNullOrEmpty(this._DetailUrl) && !string.IsNullOrEmpty(text2))
				{
					if (text2 == "已退款")
					{
						text = string.Concat(new string[]
						{
							"<a style=\"margin-top:10px;background-color:#FFBB66; color:#fff\" href='",
							this._DetailUrl,
							"' target='_blank'>",
							text2,
							"</a>"
						});
					}
					else
					{
						text = string.Concat(new string[]
						{
							text,
							"<br/><a style=\"margin-top:10px;background-color:#FFBB66; color:#fff\" href='",
							this._DetailUrl,
							"' target='_blank'>",
							text2,
							"</a>"
						});
					}
				}
				else
				{
					text = text + "<br/>" + text2;
				}
			}
			return text;
		}
	}
}
