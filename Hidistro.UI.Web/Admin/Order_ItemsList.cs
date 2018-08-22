using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Order_ItemsList : System.Web.UI.UserControl
	{
		private OrderInfo order;

		protected System.Web.UI.WebControls.DataList dlstOrderItems;

		protected System.Web.UI.WebControls.Literal litWeight;

		protected System.Web.UI.WebControls.Literal lblAmoutPrice;

		protected System.Web.UI.WebControls.HyperLink hlkReducedPromotion;

		protected FormatedMoneyLabel lblTotalPrice;

		protected System.Web.UI.WebControls.Literal lblBundlingPrice;

		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		protected override void OnLoad(System.EventArgs e)
		{
			this.dlstOrderItems.DataSource = this.order.LineItems.Values;
			this.dlstOrderItems.DataBind();
			this.litWeight.Text = this.order.Weight.ToString();
			if (this.order.IsReduced)
			{
				this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
				this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
				this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.ReducedPromotionId
				});
			}
			if (this.order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
			}
			this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
		}
	}
}
