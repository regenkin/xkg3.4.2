using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VUserLogin : VshopTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserLogin.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("用户登录");
		}
	}
}
