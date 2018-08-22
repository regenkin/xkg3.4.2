using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ShopMenu : AdminPage
	{
		protected bool _enable;

		protected System.Web.UI.WebControls.CheckBox ShopDefault;

		protected System.Web.UI.WebControls.CheckBox ActivityMenu;

		protected System.Web.UI.WebControls.CheckBox MemberDefault;

		protected System.Web.UI.WebControls.CheckBox DistributorsMenu;

		protected System.Web.UI.WebControls.CheckBox GoodsListMenu;

		protected System.Web.UI.WebControls.CheckBox GoodsCheck;

		protected System.Web.UI.WebControls.CheckBox GoodsType;

		protected System.Web.UI.WebControls.CheckBox BrandMenu;

		protected System.Web.UI.WebControls.RadioButtonList RadioType;

		protected System.Web.UI.WebControls.Button BtnSave;

		protected ShopMenu() : base("m01", "dpp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (!base.IsPostBack)
			{
				this.ShopDefault.Checked = masterSettings.ShopDefault;
				this.MemberDefault.Checked = masterSettings.MemberDefault;
				this.GoodsType.Checked = masterSettings.GoodsType;
				this.GoodsCheck.Checked = masterSettings.GoodsCheck;
				this.ActivityMenu.Checked = masterSettings.ActivityMenu;
				this.DistributorsMenu.Checked = masterSettings.DistributorsMenu;
				this.GoodsListMenu.Checked = masterSettings.GoodsListMenu;
				this.BrandMenu.Checked = masterSettings.BrandMenu;
				this.RadioType.SelectedValue = masterSettings.ShopMenuStyle;
			}
			this._enable = masterSettings.EnableShopMenu;
		}

		private void BtnSave_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (!this.ShopDefault.Checked && !this.MemberDefault.Checked && !this.GoodsType.Checked && !this.GoodsCheck.Checked)
			{
				this.ShowMsg("请至少选择一个显示店铺导航的页面", false);
				return;
			}
			masterSettings.ShopDefault = this.ShopDefault.Checked;
			masterSettings.MemberDefault = this.MemberDefault.Checked;
			masterSettings.GoodsType = this.GoodsType.Checked;
			masterSettings.GoodsCheck = this.GoodsCheck.Checked;
			masterSettings.ShopMenuStyle = this.RadioType.SelectedValue;
			masterSettings.ActivityMenu = this.ActivityMenu.Checked;
			masterSettings.DistributorsMenu = this.DistributorsMenu.Checked;
			masterSettings.GoodsListMenu = this.GoodsListMenu.Checked;
			masterSettings.BrandMenu = this.BrandMenu.Checked;
			this.ShowMsg("保存成功", true);
			SettingsManager.Save(masterSettings);
		}
	}
}
