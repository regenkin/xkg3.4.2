using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VOneTaoPaySuccess : VMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VOneTaoPaySuccess.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("支付结果");
		}
	}
}
