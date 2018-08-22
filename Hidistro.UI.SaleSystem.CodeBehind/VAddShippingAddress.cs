using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VAddShippingAddress : VMemberTemplatedWebControl
	{
		private RegionSelector dropRegions;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Vaddshippingaddress.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
			PageTitle.AddSiteNameTitle("添加收货地址");
		}
	}
}
