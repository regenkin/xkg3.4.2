using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VRequestReturn : VshopTemplatedWebControl
	{
		private string orderId;

		private string ProductId;

		private string SKuId;

		private System.Web.UI.WebControls.Literal litimage;

		private System.Web.UI.WebControls.Literal litname;

		private System.Web.UI.WebControls.Literal litItemAdjustedPrice;

		private System.Web.UI.WebControls.Literal litQuantity;

		private VshopTemplatedRepeater rptOrderProducts;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidorderid;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidproductid;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidskuid;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidOrderStatus;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidorderitemid;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VRequestReturn.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidOrderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("OrderStatus");
			this.hidskuid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("skuid");
			this.hidorderid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("orderid");
			this.hidorderitemid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("orderitemid");
			this.hidproductid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("productid");
			this.orderId = this.Page.Request.QueryString["orderId"].Trim();
			this.SKuId = Globals.RequestQueryStr("skuid").Trim();
			if (string.IsNullOrEmpty(this.SKuId))
			{
				int id = Globals.RequestQueryNum("ID");
				LineItemInfo lineItemInfo = OrderSplitHelper.GetLineItemInfo(id, this.orderId);
				if (lineItemInfo != null)
				{
					this.SKuId = lineItemInfo.SkuId;
					this.hidorderitemid.Value = id.ToString();
				}
			}
			this.hidorderid.Value = this.orderId;
			this.hidskuid.Value = this.SKuId;
			this.litimage = (System.Web.UI.WebControls.Literal)this.FindControl("litimage");
			this.litname = (System.Web.UI.WebControls.Literal)this.FindControl("litname");
			this.litItemAdjustedPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litItemAdjustedPrice");
			this.litQuantity = (System.Web.UI.WebControls.Literal)this.FindControl("litQuantity");
			this.rptOrderProducts = (VshopTemplatedRepeater)this.FindControl("rptOrderProducts");
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			this.hidOrderStatus.Value = ((int)orderInfo.OrderStatus).ToString();
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			bool flag = false;
			foreach (LineItemInfo current in orderInfo.LineItems.Values)
			{
				if (current.SkuId.ToString() == this.SKuId)
				{
					this.litimage.Text = "<image src=\"" + current.ThumbnailsUrl + "\"></image>";
					this.litname.Text = current.ItemDescription;
					this.litItemAdjustedPrice.Text = current.ItemAdjustedPrice.ToString("0.00");
					this.litQuantity.Text = current.Quantity.ToString();
					this.hidproductid.Value = current.ProductId.ToString();
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				base.GotoResourceNotFound("此订单商品不存在");
			}
			PageTitle.AddSiteNameTitle("申请退货");
		}
	}
}
