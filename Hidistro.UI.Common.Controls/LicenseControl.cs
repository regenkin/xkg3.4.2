using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class LicenseControl : WebControl
	{
		private readonly string renderFormat = "<a href=\"http://www.pufang.net\">Pufufang技术支持</a>";

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CopyrightLicenser.CheckCopyright())
			{
				writer.Write(string.Format(this.renderFormat, SettingsManager.GetMasterSettings(false).SiteUrl));
			}
		}
	}
}
