using Hidistro.Core;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VRequestDistributorFinish : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litDescirption;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RequestDistributorFinish.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("去看店铺");
			this.litDescirption = (System.Web.UI.WebControls.Literal)this.FindControl("litDescirption");
			this.litDescirption.Text = SettingsManager.GetMasterSettings(false).DistributorDescription;
			if (!this.Page.IsPostBack)
			{
				System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["SelectProcutId"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					string json = Globals.UrlDecode(httpCookie.Value);
					JObject source = JObject.Parse(json);
					System.Collections.Generic.List<int> productList = (from s in source.Values()
					select System.Convert.ToInt32(s)).ToList<int>();
					DistributorsBrower.AddDistributorProductId(productList);
					System.Web.HttpContext.Current.Response.Cookies["SelectProcutId"].Value = null;
				}
			}
		}
	}
}
