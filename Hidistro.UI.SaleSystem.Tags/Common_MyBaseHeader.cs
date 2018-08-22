using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MyBaseHeader : VshopTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/Skin-Common_MyBaseHeader.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
