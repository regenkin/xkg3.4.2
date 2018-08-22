using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class WXConfigBindOK : AdminPage
	{
		public string BindSetDesc = "立即绑定";

		public string ChangeBindUrl = "";

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected System.Web.UI.WebControls.Button btnClearToken;

		protected System.Web.UI.WebControls.Literal txtUrl;

		protected System.Web.UI.WebControls.HiddenField hdfCopyUrl;

		protected System.Web.UI.WebControls.Literal txtToken;

		protected System.Web.UI.WebControls.HiddenField hdfCopyToken;

		protected System.Web.UI.WebControls.Label lbAppId;

		protected System.Web.UI.WebControls.TextBox txtAppSecret;

		protected System.Web.UI.WebControls.Button btnSave;

		protected WXConfigBindOK() : base("m06", "wxp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (string.IsNullOrEmpty(masterSettings.WeixinToken))
				{
					masterSettings.WeixinToken = this.CreateKey(8);
					SettingsManager.Save(masterSettings);
				}
				if (string.IsNullOrWhiteSpace(masterSettings.CheckCode))
				{
					masterSettings.CheckCode = this.CreateKey(20);
					SettingsManager.Save(masterSettings);
				}
				this.BindSetDesc = (string.IsNullOrEmpty(masterSettings.WeixinAppId) ? "立即绑定" : "查询详情");
				this.hdfCopyUrl.Value = (this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text));
				this.hdfCopyToken.Value = (this.txtToken.Text = masterSettings.WeixinToken);
				this.lbAppId.Text = masterSettings.WeixinAppId;
				this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
				int bindOpenIDAndNoUserNameCount = MemberHelper.GetBindOpenIDAndNoUserNameCount();
				this.ChangeBindUrl = ((bindOpenIDAndNoUserNameCount > 0) ? "WXConfigChangeBind.aspx" : "WXConfig.aspx");
			}
		}

		private string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(array);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(string.Format("{0:X2}", array[i]));
			}
			return stringBuilder.ToString();
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.WeixinAppSecret = this.txtAppSecret.Text.Trim();
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}

		protected void btnClearToken_Click(object sender, System.EventArgs e)
		{
			Globals.RefreshWeiXinToken();
			this.ShowMsgAndReUrl("刷新成功", true, "WXConfigBindOK.aspx");
		}
	}
}
