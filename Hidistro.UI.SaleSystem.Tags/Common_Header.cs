using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Header : SimpleTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Header.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
