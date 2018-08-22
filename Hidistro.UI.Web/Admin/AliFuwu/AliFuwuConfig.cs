using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class AliFuwuConfig : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAliAppId;

		protected System.Web.UI.WebControls.TextBox txtAliFollowTitle;

		protected System.Web.UI.WebControls.Button btnSave;

		protected System.Web.UI.WebControls.Literal txtUrl;

		protected System.Web.UI.WebControls.HiddenField hdfCopyUrl;

		protected System.Web.UI.WebControls.Literal txtToken;

		protected System.Web.UI.WebControls.HiddenField hdfCopyToken;

		protected System.Web.UI.WebControls.Button RSACreat;

		protected System.Web.UI.WebControls.Literal txtRSA;

		protected System.Web.UI.WebControls.HiddenField RSAPublic;

		protected AliFuwuConfig() : base("m11", "fwp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = this.txtAliAppId.Text.Trim();
			string text2 = this.txtAliFollowTitle.Text.Trim();
			if (text.Length < 14)
			{
				this.ShowMsg("请输入正确的AppId信息！", false);
				return;
			}
			if (text2.Length < 2)
			{
				this.ShowMsg("关注消息，请不要留空，2个字符以上！", false);
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (masterSettings.AlipayAppid != text)
			{
				MemberHelper.ClearAllAlipayopenId();
				ScanHelp.ClearScanBind("ALIPAY");
			}
			masterSettings.AlipayAppid = text;
			masterSettings.AliOHFollowRelayTitle = text2;
			SettingsManager.Save(masterSettings);
			AlipayFuwuConfig.CommSetConfig(text, base.Server.MapPath("~/"), "GBK");
			this.ShowMsg("服务窗配置保存成功！", true);
			this.BindData();
		}

		private void BindData()
		{
			this.hdfCopyUrl.Value = (this.txtUrl.Text = string.Format("http://{0}/api/AliPayFuwuApi.ashx", base.Request.Url.Host));
			this.hdfCopyToken.Value = (this.txtToken.Text = string.Format("http://{0}/api/AliPayFuwuApi.ashx", base.Request.Url.Host));
			this.RSAPublic.Value = (this.txtRSA.Text = RsaKeyHelper.GetRSAKeyContent(base.Server.MapPath("~/config/rsa_public_key.pem"), true));
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.txtAliAppId.Text = masterSettings.AlipayAppid;
			this.txtAliFollowTitle.Text = masterSettings.AliOHFollowRelayTitle;
		}

		protected void RsaCreat_Click(object sender, System.EventArgs e)
		{
			string text = RsaKeyHelper.CreateRSAKeyFile(base.Server.MapPath("~/config/RSAGenerator/Rsa.exe"), base.Server.MapPath("~/config"), false);
			if (text == "success")
			{
				this.ShowMsg("新密钥对生成成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("新密钥对生成失败" + text, false);
		}
	}
}
