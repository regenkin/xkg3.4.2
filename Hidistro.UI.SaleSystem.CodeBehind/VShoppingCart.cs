using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VShoppingCart : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptCartProducts;

		private VshopTemplatedRepeater rptCartPointProducts;

		private System.Web.UI.WebControls.Literal litTotal;

		private System.Web.UI.HtmlControls.HtmlGenericControl divShowTotal;

		private System.Web.UI.HtmlControls.HtmlAnchor aLink;

		private System.Collections.Generic.List<ShoppingCartInfo> cart;

		private System.Collections.Generic.List<ShoppingCartInfo> cartPoint;

		private decimal ReductionMoneyALL = 0m;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VShoppingCart.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptCartProducts = (VshopTemplatedRepeater)this.FindControl("rptCartProducts");
			this.rptCartProducts.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
			this.rptCartPointProducts = (VshopTemplatedRepeater)this.FindControl("rptCartPointProducts");
			this.litTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litTotal");
			this.divShowTotal = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("divShowTotal");
			this.aLink = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("aLink");
			this.Page.Session["stylestatus"] = "0";
			this.cart = ShoppingCartProcessor.GetShoppingCartAviti(0);
			this.cartPoint = ShoppingCartProcessor.GetShoppingCartAviti(1);
			if (this.cart != null)
			{
				this.rptCartProducts.DataSource = this.cart;
				this.rptCartProducts.DataBind();
			}
			else
			{
				System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)this.FindControl("products");
				panel.Visible = false;
			}
			if (this.cartPoint != null)
			{
				this.rptCartPointProducts.DataSource = this.cartPoint;
				this.rptCartPointProducts.DataBind();
			}
			else
			{
				System.Web.UI.WebControls.Panel panel2 = (System.Web.UI.WebControls.Panel)this.FindControl("pointproducts");
				panel2.Visible = false;
			}
			if (this.cart != null || this.cartPoint != null)
			{
				this.aLink.HRef = "/Vshop/SubmmitOrder.aspx";
			}
			else
			{
				System.Web.UI.WebControls.Panel panel3 = (System.Web.UI.WebControls.Panel)this.FindControl("divEmpty");
				panel3.Visible = true;
				System.Web.UI.WebControls.Panel panel4 = (System.Web.UI.WebControls.Panel)this.FindControl("pannelGo");
				panel4.Visible = false;
				System.Web.UI.HtmlControls.HtmlInputHidden htmlInputHidden = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdIsShow");
				htmlInputHidden.Value = "1";
			}
			decimal d = 0m;
			if (this.cart != null)
			{
				foreach (ShoppingCartInfo current in this.cart)
				{
					d += current.GetAmount();
				}
			}
			int num = 0;
			if (this.cartPoint != null)
			{
				foreach (ShoppingCartInfo current in this.cartPoint)
				{
					num += current.GetTotalPoint();
				}
			}
			PageTitle.AddSiteNameTitle("购物车");
			string text = string.Empty;
			decimal d2 = d - this.ReductionMoneyALL;
			if (d2 > 0m)
			{
				text = "￥" + d2.ToString("F2");
			}
			if (num > 0)
			{
				text = text + "+" + num.ToString() + "积分";
			}
			this.litTotal.Text = text;
		}

		private void rptCartProducts_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
			}
		}
	}
}
