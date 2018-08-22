using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VSaleService : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litSaleService;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VSaleService.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litSaleService = (System.Web.UI.WebControls.Literal)this.FindControl("litSaleService");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.litSaleService.Text = masterSettings.SaleService;
			}
			PageTitle.AddSiteNameTitle("售后服务");
		}
	}
}
