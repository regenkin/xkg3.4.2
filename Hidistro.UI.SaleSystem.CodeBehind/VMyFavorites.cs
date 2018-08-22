using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyFavorites : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptProducts;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyFavorites.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string url = this.Page.Request.QueryString["returnUrl"];
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
			{
				this.Page.Response.Redirect(url);
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.rptProducts.DataSource = ProductBrowser.GetFavorites(currentMember);
			this.rptProducts.DataBind();
			PageTitle.AddSiteNameTitle("我的收藏");
		}
	}
}
