using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VActivity : VMemberTemplatedWebControl
	{
		private HiImage img;

		private System.Web.UI.WebControls.Literal litDescription;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vActivity.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int activityId;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("id"), out activityId);
			ActivityInfo activity = VshopBrowser.GetActivity(activityId);
			if (activity == null)
			{
				base.GotoResourceNotFound("");
			}
			this.img = (HiImage)this.FindControl("img");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.img.ImageUrl = activity.PicUrl;
			this.litDescription.Text = activity.Description;
			PageTitle.AddSiteNameTitle("微报名");
		}
	}
}
