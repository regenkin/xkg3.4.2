using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ConcernUrl : AdminPage
	{
		protected bool enableGuidePageSet;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		private string action = Globals.RequestFormStr("action");

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtGuidePageSet;

		protected System.Web.UI.WebControls.Button btnSave;

		protected ConcernUrl() : base("m06", "wxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (this.action == "setenable")
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
					this.siteSettings.EnableGuidePageSet = (Globals.RequestFormNum("enable") == 1);
					SettingsManager.Save(this.siteSettings);
					base.Response.Write(s);
					base.Response.End();
				}
				else
				{
					string guidePageSet = this.siteSettings.GuidePageSet;
					if (guidePageSet.Length > 10)
					{
						this.txtGuidePageSet.Text = this.siteSettings.GuidePageSet;
					}
				}
			}
			this.enableGuidePageSet = this.siteSettings.EnableGuidePageSet;
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			this.siteSettings.GuidePageSet = this.txtGuidePageSet.Text.Trim();
			SettingsManager.Save(this.siteSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}
