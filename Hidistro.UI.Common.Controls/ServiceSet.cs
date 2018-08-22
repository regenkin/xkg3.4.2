using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ServiceSet : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (masterSettings.EnableSaleService)
			{
				base.Text = masterSettings.ServiceMeiQia;
			}
			base.Render(writer);
		}
	}
}
