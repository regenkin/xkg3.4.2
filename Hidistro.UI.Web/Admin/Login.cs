using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Login : System.Web.UI.Page
	{
		protected string htmlWebTitle = string.Empty;

		private string verifyCodeKey = "VerifyCode";

		protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;

		protected System.Web.UI.WebControls.TextBox txtAdminName;

		protected System.Web.UI.WebControls.TextBox txtAdminPassWord;

		protected System.Web.UI.WebControls.TextBox txtCode;

		protected System.Web.UI.WebControls.Button btnAdminLogin;

		protected SmallStatusMessage lblStatus;

		private string ReferralLink
		{
			get
			{
				return this.ViewState["ReferralLink"] as string;
			}
			set
			{
				this.ViewState["ReferralLink"] = value;
			}
		}

		private bool CheckVerifyCode(string verifyCode)
		{
			return base.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			if (flag)
			{
				string verifyCode = base.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				base.Response.Write("}");
				base.Response.End();
			}
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				this.htmlWebTitle = masterSettings.SiteName;
				System.Uri urlReferrer = this.Context.Request.UrlReferrer;
				if (urlReferrer != null)
				{
					this.ReferralLink = urlReferrer.ToString();
				}
				this.txtAdminName.Focus();
			}
		}

		private void btnAdminLogin_Click(object sender, System.EventArgs e)
		{
			if (!Globals.CheckVerifyCode(this.txtCode.Text.Trim()))
			{
				this.ShowMessage("验证码不正确");
				return;
			}
			ManagerInfo manager = ManagerHelper.GetManager(this.txtAdminName.Text);
			if (manager == null)
			{
				this.ShowMessage("无效的用户信息");
				return;
			}
			if (manager.Password != HiCryptographer.Md5Encrypt(this.txtAdminPassWord.Text))
			{
				this.ShowMessage("密码不正确");
				return;
			}
			this.WriteCookie(manager);
			this.Page.Response.Redirect("Default.aspx", true);
		}

		private void WriteCookie(ManagerInfo userToLogin)
		{
			RoleInfo role = ManagerHelper.GetRole(userToLogin.RoleId);
			System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(1, userToLogin.UserId.ToString(), System.DateTime.Now, System.DateTime.Now.AddDays(1.0), true, string.Format("{0}_{1}", role.RoleId, role.IsDefault));
			string value = System.Web.Security.FormsAuthentication.Encrypt(ticket);
			System.Web.HttpCookie cookie = new System.Web.HttpCookie(string.Format("{0}{1}", Globals.DomainName, System.Web.Security.FormsAuthentication.FormsCookieName), value);
			System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
		}

		private void ShowMessage(string msg)
		{
			this.lblStatus.Text = msg;
			this.lblStatus.Success = false;
			this.lblStatus.Visible = true;
		}

		private void ChangeShowCopyRight(string showcopyright)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.ShowCopyRight = showcopyright;
			SettingsManager.Save(masterSettings);
		}

		//protected override void Render(System.Web.UI.HtmlTextWriter writer)
		//{
		//	SystemAuthorizationInfo systemAuthorization = SystemAuthorizationHelper.GetSystemAuthorization(true);
		//	if (systemAuthorization == null)
		//	{
		//		return;
		//	}
		//	this.ChangeShowCopyRight(systemAuthorization.IsShowJixuZhiChi ? "0" : "1");
		//	switch (systemAuthorization.state)
		//	{
		//	case SystemAuthorizationState.已过授权有效期:
		//		writer.Write(SystemAuthorizationHelper.noticeMsg);
		//		return;
		//	case SystemAuthorizationState.未经官方授权:
		//		writer.Write(SystemAuthorizationHelper.licenseMsg);
		//		return;
		//	default:
		//		base.Render(writer);
		//		return;
		//	}
		//}
	}
}
