using Hidistro.Core;
using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class PurchaseOrderItemUpdateHyperLink : HyperLink
	{
		public object PurchaseStatusCode
		{
			get
			{
				object result;
				if (this.ViewState["purchaseStatusCode"] == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewState["purchaseStatusCode"];
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["purchaseStatusCode"] = value;
				}
			}
		}

		public object PurchaseOrderId
		{
			get
			{
				object result;
				if (this.ViewState["PurchaseOrderId"] == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewState["PurchaseOrderId"];
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["PurchaseOrderId"] = value;
				}
			}
		}

		public object DistorUserId
		{
			get
			{
				object result;
				if (this.ViewState["DistorUserId"] == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewState["DistorUserId"];
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["DistorUserId"] = value;
				}
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.PurchaseOrderId.ToString().ToUpper().StartsWith("MP"))
			{
				OrderStatus orderStatus = (OrderStatus)this.PurchaseStatusCode;
				if (orderStatus != OrderStatus.WaitBuyerPay)
				{
					base.Visible = false;
					base.Text = string.Empty;
				}
				else
				{
					base.NavigateUrl = string.Concat(new object[]
					{
						Globals.ApplicationPath,
						"/admin/purchaseOrder/ChangePurchaseOrderItems.aspx?PurchaseOrderId=",
						this.PurchaseOrderId,
						"&DistorUserId=",
						this.DistorUserId
					});
				}
			}
			else
			{
				base.Visible = false;
				base.Text = string.Empty;
			}
			base.Render(writer);
		}
	}
}
