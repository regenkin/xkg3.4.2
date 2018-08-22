using Hidistro.ControlPanel.Sales;
using Hidistro.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderItemStatusLabel : Literal
	{
		private string _DetailUrl = string.Empty;

		private string _OrderID = string.Empty;

		private string _SkuID = string.Empty;

		private int _Type;

		private int _OrderItemID;

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

		protected override void Render(HtmlTextWriter writer)
		{
			if (this._OrderItemID == 0 && (string.IsNullOrEmpty(this._OrderID) || string.IsNullOrEmpty(this._SkuID)))
			{
				base.Text = "";
			}
			else
			{
				string text = string.Empty;
				if (this._Type != 1)
				{
					RefundInfo byOrderIdAndProductID = RefundHelper.GetByOrderIdAndProductID(this._OrderID, 0, this._SkuID, this._OrderItemID);
					if (byOrderIdAndProductID != null)
					{
						switch (byOrderIdAndProductID.HandleStatus)
						{
						case RefundInfo.Handlestatus.Applied:
							text = "退款中";
							break;
						case RefundInfo.Handlestatus.Refunded:
							text = "已退款";
							break;
						case RefundInfo.Handlestatus.Refused:
							text = "拒绝退款";
							break;
						case RefundInfo.Handlestatus.NoneAudit:
							text = "退款待审核";
							break;
						case RefundInfo.Handlestatus.HasTheAudit:
							text = "退款已审核";
							break;
						case RefundInfo.Handlestatus.NoRefund:
							text = "待退款";
							break;
						case RefundInfo.Handlestatus.AuditNotThrough:
							text = "拒绝退款";
							break;
						case RefundInfo.Handlestatus.RefuseRefunded:
							text = "拒绝退款";
							break;
						}
					}
				}
				if (!string.IsNullOrEmpty(this._DetailUrl) && !string.IsNullOrEmpty(text))
				{
					base.Text = string.Concat(new string[]
					{
						"<a href='",
						this._DetailUrl,
						"' target='_blank'>",
						text,
						"</a>"
					});
				}
				else
				{
					base.Text = text;
				}
			}
			base.Render(writer);
		}
	}
}
