using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorApplySet : AdminPage
	{
		public string tabnum = "0";

		public string productHtml = "";

		protected Hidistro.UI.Common.Controls.Style Style1;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox radioCommissionon;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox radiorequeston;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox radioDistributorApplicationCondition;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox HasConditions;

		protected System.Web.UI.HtmlControls.HtmlInputText txtrequestmoney;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox HasProduct;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSave;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddProductId;

		protected ucUeditor fckDescription;

		protected System.Web.UI.WebControls.Button Button1;

		protected DistributorApplySet() : base("m05", "fxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.tabnum = base.Request.QueryString["tabnum"];
				if (string.IsNullOrEmpty(this.tabnum))
				{
					this.tabnum = "0";
				}
				this.txtrequestmoney.Value = masterSettings.FinishedOrderMoney.ToString();
				this.fckDescription.Text = masterSettings.DistributorDescription;
				this.radioDistributorApplicationCondition.Checked = masterSettings.DistributorApplicationCondition;
				this.radiorequeston.Checked = true;
				if (!masterSettings.IsRequestDistributor)
				{
					this.radiorequeston.Checked = false;
				}
				this.radioCommissionon.Checked = false;
				if (masterSettings.EnableCommission)
				{
					this.radioCommissionon.Checked = true;
				}
				if (masterSettings.FinishedOrderMoney > 0)
				{
					this.HasConditions.Checked = true;
				}
				this.HasProduct.Checked = masterSettings.EnableDistributorApplicationCondition;
				string distributorProducts = masterSettings.DistributorProducts;
				if (!string.IsNullOrEmpty(distributorProducts))
				{
					this.hiddProductId.Value = distributorProducts;
				}
				if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && masterSettings.DistributorProductsDate.Contains("|"))
				{
					if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate.Split(new char[]
					{
						'|'
					})[0]))
					{
						this.calendarStartDate.SelectedDate = new System.DateTime?(System.Convert.ToDateTime(masterSettings.DistributorProductsDate.Split(new char[]
						{
							'|'
						})[0]));
					}
					if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate.Split(new char[]
					{
						'|'
					})[1]))
					{
						this.calendarEndDate.SelectedDate = new System.DateTime?(System.Convert.ToDateTime(masterSettings.DistributorProductsDate.Split(new char[]
						{
							'|'
						})[1]));
					}
				}
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			this.tabnum = "0";
			int num = 0;
			if (this.radioDistributorApplicationCondition.Checked && !this.HasProduct.Checked && !this.HasConditions.Checked)
			{
				this.ShowMsg("请选择分销商申请条件", false);
				return;
			}
			if (this.HasConditions.Checked && (!int.TryParse(this.txtrequestmoney.Value.Trim(), out num) || num < 1))
			{
				this.ShowMsg("累计消费金额必须为大于0的整数金额", false);
				return;
			}
			if (this.HasProduct.Checked && string.IsNullOrEmpty(this.hiddProductId.Value))
			{
				this.ShowMsg("请选择指定商品", false);
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.DistributorApplicationCondition = this.radioDistributorApplicationCondition.Checked;
			masterSettings.IsRequestDistributor = this.radiorequeston.Checked;
			masterSettings.EnableCommission = this.radioCommissionon.Checked;
			masterSettings.FinishedOrderMoney = num;
			masterSettings.EnableDistributorApplicationCondition = this.HasProduct.Checked;
			if (this.HasProduct.Checked)
			{
				masterSettings.DistributorProducts = this.hiddProductId.Value;
			}
			else
			{
				masterSettings.DistributorProducts = "";
			}
			string text = "";
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				text = this.calendarStartDate.SelectedDate.Value.ToString();
			}
			text += "|";
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				text += this.calendarEndDate.SelectedDate.Value.ToString();
			}
			masterSettings.DistributorProductsDate = text;
			SettingsManager.Save(masterSettings);
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["Admin-Product"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Value = null;
				httpCookie.Expires = System.DateTime.Now.AddYears(-1);
				System.Web.HttpContext.Current.Response.Cookies.Set(httpCookie);
			}
			this.ShowMsgAndReUrl("修改成功", true, "DistributorApplySet.aspx");
		}

		protected void btnSave_Description(object sender, System.EventArgs e)
		{
			this.tabnum = "1";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.DistributorDescription = this.fckDescription.Text.Trim();
			SettingsManager.Save(masterSettings);
			this.ShowMsg("分销说明修改成功", true);
		}
	}
}
