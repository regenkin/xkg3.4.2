using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class SiteNameLabel : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = SettingsManager.GetMasterSettings(true).SiteName;
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = "请设置店铺名称";
			}
			base.Render(writer);
		}
	}
}
