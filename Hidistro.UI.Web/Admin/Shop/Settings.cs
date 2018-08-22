using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Settings : AdminPage
	{
		protected ImageLinkButton cancel;

		protected System.Web.UI.WebControls.TextBox txtAccess_Token;

		protected System.Web.UI.WebControls.Button btnAdd;

		protected Settings() : base("m07", "wbp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtAccess_Token.Text = masterSettings.Access_Token;
			}
		}

		protected void cancel_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.Access_Token = "";
			this.txtAccess_Token.Text = "";
			SettingsManager.Save(masterSettings);
			this.ShowMsg("解除授权成功", true);
		}

		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.Access_Token = this.txtAccess_Token.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}
