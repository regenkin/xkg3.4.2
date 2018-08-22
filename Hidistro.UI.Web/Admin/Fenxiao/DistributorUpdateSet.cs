using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorUpdateSet : AdminPage
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected Hidistro.UI.Common.Controls.Style Style1;

		protected Script Script4;

		protected System.Web.UI.WebControls.CheckBox cbIsAddCommission;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected DistributorUpdateSet() : base("m05", "fxp12")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("posttype");
			if (!(a == "save"))
			{
				bool flag = this.siteSettings.IsAddCommission == 1;
				this.cbIsAddCommission.Checked = flag;
				if (flag)
				{
					this.calendarStartDate.Text = this.siteSettings.AddCommissionStartTime;
					this.calendarEndDate.Text = this.siteSettings.AddCommissionEndTime;
				}
				else
				{
					System.DateTime now = System.DateTime.Now;
					this.calendarStartDate.Text = now.ToString("yyyy-MM-dd");
					this.calendarEndDate.Text = now.AddMonths(2).ToString("yyyy-MM-dd");
				}
				this.rptList.DataSource = DistributorGradeBrower.GetAllDistributorGrade();
				this.rptList.DataBind();
				return;
			}
			int num = Globals.RequestFormNum("isadd");
			base.Response.ContentType = "application/json";
			string s = "{\"type\":\"0\",\"tips\":\"修改失败，请输入正确的参数！\"}";
			if (num == 1)
			{
				string addCommissionStartTime = Globals.RequestFormStr("starttime");
				string addCommissionEndTime = Globals.RequestFormStr("endtime");
				string value = Globals.RequestFormStr("data");
				JArray jArray = (JArray)JsonConvert.DeserializeObject(value);
				try
				{
					using (System.Collections.Generic.IEnumerator<JToken> enumerator = jArray.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							JObject jObject = (JObject)enumerator.Current;
							int num2 = Globals.ToNum(jObject["gradeid"].ToString());
							decimal num3 = decimal.Parse(jObject["addcommission"].ToString());
							if (num2 > 0 && num3 >= 0m)
							{
								DistributorGradeBrower.SetAddCommission(num2, num3);
							}
						}
					}
					this.siteSettings.IsAddCommission = 1;
					this.siteSettings.AddCommissionStartTime = addCommissionStartTime;
					this.siteSettings.AddCommissionEndTime = addCommissionEndTime;
					Globals.EntityCoding(this.siteSettings, true);
					SettingsManager.Save(this.siteSettings);
					s = "{\"type\":\"1\",\"tips\":\"修改成功！\"}";
				}
				catch
				{
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			if (DistributorGradeBrower.ClearAddCommission())
			{
				this.siteSettings.IsAddCommission = 0;
				this.siteSettings.AddCommissionStartTime = null;
				this.siteSettings.AddCommissionEndTime = null;
				Globals.EntityCoding(this.siteSettings, true);
				SettingsManager.Save(this.siteSettings);
				s = "{\"type\":\"1\",\"tips\":\"成功关闭分销商升级奖励！\"}";
			}
			base.Response.Write(s);
			base.Response.End();
		}
	}
}
