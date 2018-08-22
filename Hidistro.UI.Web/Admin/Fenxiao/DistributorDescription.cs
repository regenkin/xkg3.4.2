using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorDescription : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected ucUeditor htmlfkContent;

		protected System.Web.UI.WebControls.Button Button2;

		protected DistributorDescription() : base("m05", "fxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.htmlfkContent.Text = masterSettings.DistributorDescription;
			}
		}

		protected void btnSave_fkContent(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.DistributorDescription = this.htmlfkContent.Text.Trim();
			SettingsManager.Save(masterSettings);
			this.ShowMsg("分销说明修改成功", true);
		}
	}
}
