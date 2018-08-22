using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class COD : AdminPage
	{
		protected bool _enable;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected COD() : base("m09", "szp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this._enable = masterSettings.EnablePodRequest;
			}
		}
	}
}
