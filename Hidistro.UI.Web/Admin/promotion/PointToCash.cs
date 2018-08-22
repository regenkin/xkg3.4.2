using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class PointToCash : AdminPage
	{
		protected bool enable;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_Rate;

		protected System.Web.UI.WebControls.TextBox txt_MaxAmount;

		protected System.Web.UI.WebControls.Button saveBtn;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.Button btnQuery;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		public PointToCash() : base("m08", "yxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			this.enable = masterSettings.PonitToCash_Enable;
			this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
			if (!base.IsPostBack)
			{
				this.txt_Rate.Text = masterSettings.PointToCashRate.ToString();
				this.txt_MaxAmount.Text = masterSettings.PonitToCash_MaxAmount.ToString("F2");
			}
		}

		private void BindData()
		{
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		protected void saveBtn_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_Rate.Text;
			string text2 = this.txt_MaxAmount.Text;
			int pointToCashRate = 0;
			decimal ponitToCash_MaxAmount = 0m;
			if (!this.bInt(text, ref pointToCashRate))
			{
				this.ShowMsg("请输入正确的抵现比例！", false);
				return;
			}
			if (!this.bDecimal(text2, ref ponitToCash_MaxAmount))
			{
				this.ShowMsg("请输入正确的单次最高抵现金额！", false);
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.PointToCashRate = pointToCashRate;
			masterSettings.PonitToCash_MaxAmount = ponitToCash_MaxAmount;
			SettingsManager.Save(masterSettings);
			this.enable = masterSettings.PonitToCash_Enable;
			this.ShowMsg("保存成功！", true);
		}
	}
}
