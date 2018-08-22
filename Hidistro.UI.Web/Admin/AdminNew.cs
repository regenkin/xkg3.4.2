using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AdminNew : System.Web.UI.MasterPage
	{
		protected string htmlWebTitle = string.Empty;

		protected int CurrentUserId;

		protected System.Web.UI.WebControls.ContentPlaceHolder head;

		protected System.Web.UI.WebControls.Literal topMenu;

		protected System.Web.UI.WebControls.Literal litSitename;

		protected System.Web.UI.WebControls.Literal litUsername;

		protected System.Web.UI.WebControls.ContentPlaceHolder ContentPlaceHolder1;

		protected System.Web.UI.WebControls.Literal leftMenu;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
			if (currentManager != null)
			{
				this.CurrentUserId = currentManager.UserId;
			}
			else
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/admin/Login.aspx", true);
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.litSitename.Text = masterSettings.SiteName;
			this.htmlWebTitle = masterSettings.SiteName;
			if (this.Page.IsPostBack)
			{
				return;
			}
			AdminPage adminPage = this.Page as AdminPage;
			Navigation navigation = Navigation.GetNavigation(true);
			this.topMenu.Text = navigation.RenderTopMenu(adminPage.ModuleId);
			this.leftMenu.Text = navigation.RenderLeftMenu(adminPage.ModuleId, adminPage.PageId);
			this.litUsername.Text = currentManager.UserName;
		}
	}
}
