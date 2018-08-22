using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Installer
{
	public class Succeed : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.Literal txtUrl;

		protected System.Web.UI.WebControls.Literal txtToken;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["callback"]))
			{
				try
				{
					Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
					configuration.AppSettings.Settings.Remove("Installer");
					configuration.Save();
					base.Response.Write("true");
				}
				catch (System.Exception ex)
				{
					base.Response.Write(ex.Message);
				}
				base.Response.End();
				return;
			}
			if (base.Request.UrlReferrer == null || base.Request.UrlReferrer.OriginalString.IndexOf("Install.aspx") < 0)
			{
				base.Response.Redirect("default.aspx");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(masterSettings.WeixinToken))
			{
				masterSettings.WeixinToken = this.CreateKey(8);
				SettingsManager.Save(masterSettings);
			}
			this.txtToken.Text = masterSettings.WeixinToken;
			this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
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
	}
}
