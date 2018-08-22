using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VQRcode : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal liturl;

		private System.Web.UI.WebControls.Literal litimage;

		private System.Web.UI.WebControls.Literal litstorename;

		private System.Web.UI.WebControls.Literal litgotourl;

		private System.Web.UI.WebControls.Literal litItemParams;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VQRcode.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litimage = (System.Web.UI.WebControls.Literal)this.FindControl("litimage");
			this.litgotourl = (System.Web.UI.WebControls.Literal)this.FindControl("litgotourl");
			this.liturl = (System.Web.UI.WebControls.Literal)this.FindControl("liturl");
			this.litstorename = (System.Web.UI.WebControls.Literal)this.FindControl("litstorename");
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
			{
				this.liturl.Text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Default.aspx?ReferralId=" + this.Page.Request.QueryString["ReferralId"];
				this.litgotourl.Text = this.liturl.Text;
				this.litstorename.Text = DistributorsBrower.GetCurrentDistributors(int.Parse(this.Page.Request.QueryString["ReferralId"]), true).StoreName;
			}
			this.litimage.Text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";
			PageTitle.AddSiteNameTitle(this.litstorename.Text + "店铺二维码");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string text = "";
			if (!string.IsNullOrEmpty(masterSettings.ShopSpreadingCodePic))
			{
				text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.ShopSpreadingCodePic;
			}
			this.litItemParams.Text = string.Concat(new string[]
			{
				text,
				"|",
				masterSettings.ShopSpreadingCodeName,
				"|",
				masterSettings.ShopSpreadingCodeDescription.Replace("|", "｜")
			});
		}
	}
}
