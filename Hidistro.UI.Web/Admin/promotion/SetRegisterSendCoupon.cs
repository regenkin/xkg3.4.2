using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SetRegisterSendCoupon : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.DropDownList ddlCouponList;

		protected ucDateTimePicker ucDateBegin;

		protected ucDateTimePicker ucDateEnd;

		protected System.Web.UI.WebControls.Button btnSave;

		protected bool IsEnble
		{
			get;
			private set;
		}

		protected SetRegisterSendCoupon() : base("m08", "yxp17")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindDdlCouponList();
				this.BindDate();
			}
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.BtnSave);
		}

		private void BindDate()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.IsEnble = masterSettings.IsRegisterSendCoupon;
			try
			{
				this.ddlCouponList.SelectedValue = masterSettings.RegisterSendCouponId.ToString();
			}
			catch (System.Exception)
			{
			}
			this.ucDateBegin.Text = (masterSettings.RegisterSendCouponBeginTime.HasValue ? masterSettings.RegisterSendCouponBeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
			this.ucDateEnd.Text = (masterSettings.RegisterSendCouponEndTime.HasValue ? masterSettings.RegisterSendCouponEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
		}

		private void BindDdlCouponList()
		{
			System.Data.DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(System.DateTime.Now, new CouponType?(CouponType.活动赠送));
			if (unFinishedCoupon != null)
			{
				foreach (System.Data.DataRow dataRow in unFinishedCoupon.Rows)
				{
					this.ddlCouponList.Items.Add(new System.Web.UI.WebControls.ListItem
					{
						Text = dataRow["CouponName"].ToString(),
						Value = dataRow["CouponId"].ToString()
					});
				}
			}
		}

		protected void BtnSave(object sender, System.EventArgs e)
		{
			this.IsEnble = bool.Parse(base.Request["txtIsEnble"]);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.IsRegisterSendCoupon = this.IsEnble;
			masterSettings.RegisterSendCouponId = int.Parse(this.ddlCouponList.SelectedValue);
			masterSettings.RegisterSendCouponBeginTime = this.ucDateBegin.SelectedDate;
			masterSettings.RegisterSendCouponEndTime = this.ucDateEnd.SelectedDate;
			try
			{
				SettingsManager.Save(masterSettings);
				this.ShowMsg("保存成功！", true);
			}
			catch (System.Exception)
			{
				this.ShowMsg("保存失败！", false);
			}
		}
	}
}
