using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VGetRedPager : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden hdId;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdUserId;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VGetRedPager.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string value = System.Web.HttpContext.Current.Request.QueryString.Get("id");
			string value2 = System.Web.HttpContext.Current.Request.QueryString.Get("userid");
			if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2))
			{
				this.hdId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdID");
				this.hdUserId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdUserid");
				this.hdId.Value = value;
				this.hdUserId.Value = value2;
			}
			PageTitle.AddSiteNameTitle("获取优惠券");
		}
	}
}
