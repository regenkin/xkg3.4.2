using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ValidationService : AdminPage
	{
		protected bool enableValidationService;

		protected bool enableIsAutoToLogin;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		private string action = Globals.RequestFormStr("action");

		protected ValidationService() : base("m06", "wxp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a;
			if (!base.IsPostBack && (a = this.action) != null)
			{
				if (!(a == "setenable"))
				{
					if (a == "setautologin")
					{
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
						try
						{
							this.siteSettings.IsAutoToLogin = (Globals.RequestFormNum("enable") == 1);
							if (this.siteSettings.IsAutoToLogin && !this.siteSettings.IsValidationService)
							{
								s = "{\"type\":\"0\",\"tips\":\"操作失败，需要先开启微信授权登录！\"}";
							}
							else
							{
								SettingsManager.Save(this.siteSettings);
							}
						}
						catch
						{
							s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
						}
						base.Response.Write(s);
						base.Response.End();
					}
				}
				else
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
					try
					{
						this.siteSettings.IsValidationService = (Globals.RequestFormNum("enable") == 1);
						if (!this.siteSettings.IsValidationService)
						{
							this.siteSettings.IsAutoToLogin = false;
						}
						SettingsManager.Save(this.siteSettings);
					}
					catch
					{
						s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
					}
					base.Response.Write(s);
					base.Response.End();
				}
			}
			this.enableValidationService = this.siteSettings.IsValidationService;
			this.enableIsAutoToLogin = this.siteSettings.IsAutoToLogin;
		}
	}
}
