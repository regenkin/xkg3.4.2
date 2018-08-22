using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductDescription : VshopTemplatedWebControl
	{
		private int productId;

		private System.Web.UI.WebControls.Literal litDescription;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductDescription.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
			this.litDescription.Text = product.Description;
			PageTitle.AddSiteNameTitle("商品描述");
		}
	}
}
