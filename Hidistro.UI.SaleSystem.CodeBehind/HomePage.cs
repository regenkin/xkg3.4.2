using System;
using System.ComponentModel;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class HomePage : NewTemplatedWebControl
	{
		[System.ComponentModel.Bindable(true)]
		public string TempFilePath
		{
			get;
			set;
		}

		protected override void OnInit(System.EventArgs e)
		{
			this.TempFilePath = "Skin-HomePage.html";
			if (this.SkinName == null)
			{
				this.SkinName = this.TempFilePath;
			}
			base.OnInit(e);
		}
	}
}
