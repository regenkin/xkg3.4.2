using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
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
	public class WXConfig : AdminPage
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected System.Web.UI.WebControls.Literal txtUrl;

		protected System.Web.UI.WebControls.HiddenField hdfCopyUrl;

		protected System.Web.UI.WebControls.Literal txtToken;

		protected System.Web.UI.WebControls.HiddenField hdfCopyToken;

		protected System.Web.UI.WebControls.TextBox txtAppId;

		protected System.Web.UI.WebControls.TextBox txtAppSecret;

		protected System.Web.UI.WebControls.Button btnSave;

		protected WXConfig() : base("m06", "wxp01")
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
				if (!string.IsNullOrWhiteSpace(masterSettings.WeixinAppId) && string.IsNullOrEmpty(base.Request.QueryString["reset"]))
				{
					base.Response.Redirect("WXConfigBindOK.aspx");
				}
				this.hdfCopyUrl.Value = (this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text));
				this.hdfCopyToken.Value = (this.txtToken.Text = masterSettings.WeixinToken);
				this.txtAppId.Text = masterSettings.WeixinAppId;
				this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
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
			if (masterSettings.WeixinAppId != this.txtAppId.Text.Trim())
			{
				WeiXinHelper.ClearWeiXinMediaID();
				MemberHelper.ClearAllOpenId();
				ScanHelp.ClearScanBind("WX");
			}
			masterSettings.WeixinAppId = this.txtAppId.Text.Trim();
			masterSettings.WeixinAppSecret = this.txtAppSecret.Text.Trim();
			SettingsManager.Save(masterSettings);
			Globals.RefreshWeiXinToken();
			this.ShowMsgAndReUrl("修改成功", true, "WXConfigBindOK.aspx");
		}
	}
}
