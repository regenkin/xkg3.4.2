using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Config : AdminPage
	{
		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected System.Web.UI.WebControls.TextBox txtSiteName;

		protected System.Web.UI.WebControls.TextBox txtShopTel;

		protected System.Web.UI.WebControls.TextBox txtShopIntroduction;

		protected System.Web.UI.WebControls.Button btnSave;

		protected Config() : base("m01", "dpp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtSiteName.Text = masterSettings.SiteName;
				this.txtShopIntroduction.Text = masterSettings.ShopIntroduction;
				this.hidpic.Value = masterSettings.DistributorLogoPic;
				this.txtShopTel.Text = masterSettings.ShopTel;
				if (!System.IO.File.Exists(base.Server.MapPath(masterSettings.DistributorLogoPic)))
				{
					this.hidpic.Value = "http://fpoimg.com/70x70";
				}
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string text = this.txtSiteName.Text.Trim();
			if (text.Length < 1 || text.Length > 10)
			{
				this.ShowMsg("请填写您的店铺名称，长度在10个字符以内", false);
				return;
			}
			string text2 = this.txtShopTel.Text.Trim();
			if (text2.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(text2, "(^[0-9]{3,4}\\-[0-9]{7,8}(\\-[0-9]{2,4})?$)|(^[0-9]{7,8}$)|(^[0−9]3,4[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)|(13\\d{9}$)|(15[01235-9]\\d{8}$)|(1[4978][0-9]\\d{8}$)|(^400(\\-)?[0-9]{3,4}(\\-)?[0-9]{3,4}$)"))
			{
				this.ShowMsg("请填写正确的手机或座机号码", false);
				return;
			}
			string text3 = this.txtShopIntroduction.Text.Trim();
			if (text3.Length > 60)
			{
				this.ShowMsg("店铺介绍的长度不能超过60个字符", false);
				return;
			}
			masterSettings.SiteName = text;
			masterSettings.ShopIntroduction = text3;
			masterSettings.ShopTel = text2;
			SettingsManager.Save(masterSettings);
			if (!string.IsNullOrEmpty(this.hidpicdel.Value))
			{
				string[] array = this.hidpicdel.Value.Split(new char[]
				{
					'|'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text4 = array2[i];
					if (!(text4 == "http://fpoimg.com/70x70"))
					{
						string path = text4;
						path = base.Server.MapPath(path);
						if (System.IO.File.Exists(path))
						{
							System.IO.File.Delete(path);
						}
					}
				}
			}
			this.hidpicdel.Value = "";
			this.ShowMsg("保存成功!", true);
		}
	}
}
