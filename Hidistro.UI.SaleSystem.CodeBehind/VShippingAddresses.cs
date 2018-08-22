using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VShippingAddresses : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptvShipping;

		private System.Web.UI.HtmlControls.HtmlAnchor aLinkToAdd;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Vshippingaddresses.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptvShipping = (VshopTemplatedRepeater)this.FindControl("rptvShipping");
			this.aLinkToAdd = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("aLinkToAdd");
			this.aLinkToAdd.HRef = Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx";
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
			{
				System.Web.UI.HtmlControls.HtmlAnchor expr_72 = this.aLinkToAdd;
				expr_72.HRef = expr_72.HRef + "?returnUrl=" + Globals.UrlEncode(this.Page.Request.QueryString["returnUrl"]);
			}
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			if (shippingAddresses != null)
			{
				this.rptvShipping.DataSource = shippingAddresses;
				this.rptvShipping.DataBind();
			}
			PageTitle.AddSiteNameTitle("收货地址");
		}
	}
}
