using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class WeixinPay : AdminPage
	{
		protected bool _enable;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected Hidistro.UI.Common.Controls.Style Style1;

		protected Script Script1;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblAppId;

		protected System.Web.UI.WebControls.Label lblAppSecret;

		protected System.Web.UI.WebControls.TextBox txt_mch_id;

		protected System.Web.UI.WebControls.TextBox txt_key;

		protected System.Web.UI.HtmlControls.HtmlGenericControl alipaypanel;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox EnableSP;

		protected System.Web.UI.WebControls.TextBox Main_AppId;

		protected System.Web.UI.WebControls.TextBox Main_Mch_ID;

		protected System.Web.UI.WebControls.TextBox Main_PayKey;

		protected System.Web.UI.WebControls.Button btnSave;

		protected WeixinPay() : base("m06", "wxp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.siteSettings.WeixinAppId) && !string.IsNullOrEmpty(this.siteSettings.WeixinAppSecret))
				{
					this.lblAppId.Text = this.siteSettings.WeixinAppId;
					this.lblAppSecret.Text = this.siteSettings.WeixinAppSecret;
				}
				else
				{
					this.lblAppSecret.Text = (this.lblAppId.Text = "<a href='../weixin/wxconfig.aspx'>去设置</a>");
					this.btnSave.Visible = false;
				}
				this.txt_key.Text = this.siteSettings.WeixinPartnerKey;
				this.txt_mch_id.Text = this.siteSettings.WeixinPartnerID;
				this.Main_PayKey.Text = this.siteSettings.Main_PayKey;
				this.Main_Mch_ID.Text = this.siteSettings.Main_Mch_ID;
				this.Main_AppId.Text = this.siteSettings.Main_AppId;
				this.EnableSP.Checked = this.siteSettings.EnableSP;
			}
			this._enable = this.siteSettings.EnableWeiXinRequest;
		}

		protected void Unnamed_Click(object sender, System.EventArgs e)
		{
			this.saveData();
		}

		private void saveData()
		{
			if (string.IsNullOrEmpty(this.txt_key.Text.Trim()) && !this.EnableSP.Checked)
			{
				this.ShowMsg("请输入Key！", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txt_mch_id.Text.Trim()))
			{
				this.ShowMsg("请输入商户号mch_id！", false);
				return;
			}
			if (this.EnableSP.Checked)
			{
				if (string.IsNullOrEmpty(this.Main_AppId.Text.Trim()))
				{
					this.ShowMsg("请输入服务商公众号！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.Main_Mch_ID.Text.Trim()))
				{
					this.ShowMsg("请输入服务商商户号mch_id！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.Main_PayKey.Text.Trim()))
				{
					this.ShowMsg("请输入服务商KEY！", false);
					return;
				}
			}
			else
			{
				this.Main_PayKey.Text = "";
				this.Main_Mch_ID.Text = "";
				this.Main_AppId.Text = "";
			}
			this.siteSettings.WeixinPartnerKey = this.txt_key.Text.Trim();
			this.siteSettings.WeixinPartnerID = this.txt_mch_id.Text.Trim();
			this.siteSettings.Main_PayKey = this.Main_PayKey.Text.Trim();
			this.siteSettings.Main_Mch_ID = this.Main_Mch_ID.Text.Trim();
			this.siteSettings.Main_AppId = this.Main_AppId.Text.Trim();
			this.siteSettings.EnableSP = this.EnableSP.Checked;
			SettingsManager.Save(this.siteSettings);
			this.ShowMsg("保存成功！", true);
		}
	}
}
