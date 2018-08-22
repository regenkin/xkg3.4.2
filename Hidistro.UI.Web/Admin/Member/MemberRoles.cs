using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class MemberRoles : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected ucUeditor fkContent;

		protected System.Web.UI.WebControls.Button btnSaveImageFtp;

		protected MemberRoles() : base("m04", "hyp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveImageFtp.Click += new System.EventHandler(this.btnSaveImageFtp_Click);
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.fkContent.Text = masterSettings.MemberRoleContent;
			}
		}

		protected void btnSaveImageFtp_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.MemberRoleContent = this.fkContent.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功！", true);
		}
	}
}
