using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManyService : AdminPage
	{
		protected bool enableManyService;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		private string action = Globals.RequestFormStr("action");

		protected ManyService() : base("m06", "wxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack && this.action == "setenable")
			{
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
				try
				{
					this.siteSettings.OpenManyService = (Globals.RequestFormNum("enable") == 1);
					SettingsManager.Save(this.siteSettings);
				}
				catch
				{
					s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				}
				base.Response.Write(s);
				base.Response.End();
			}
			this.enableManyService = this.siteSettings.OpenManyService;
		}
	}
}
