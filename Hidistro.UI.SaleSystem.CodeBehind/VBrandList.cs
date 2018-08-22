using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VBrandList : VshopTemplatedWebControl
	{
		private VshopTemplatedRepeater rptBrands;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-vbrandList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.rptBrands = (VshopTemplatedRepeater)this.FindControl("rptBrands");
			this.rptBrands.DataSource = CategoryBrowser.GetBrandCategories();
			this.rptBrands.DataBind();
			PageTitle.AddSiteNameTitle("品牌列表");
		}
	}
}
