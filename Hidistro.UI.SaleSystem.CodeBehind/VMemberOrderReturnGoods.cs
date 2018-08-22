using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberOrderReturnGoods : VMemberTemplatedWebControl
	{
		private string orderId;

		private System.Web.UI.WebControls.Literal litOrderId;

		private System.Web.UI.WebControls.Literal litOrderDate;

		private VshopTemplatedRepeater rptOrderProducts;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrderReturnGoods.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderDate = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderDate");
			this.rptOrderProducts = (VshopTemplatedRepeater)this.FindControl("rptOrderProducts");
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			this.litOrderId.Text = this.orderId;
			this.litOrderDate.Text = orderInfo.OrderDate.ToString();
			this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
			this.rptOrderProducts.DataBind();
			PageTitle.AddSiteNameTitle("退换货商品");
		}
	}
}
