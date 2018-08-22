using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.StatisticsReport;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Sales
{
	public class UserInCreaseStatistics : AdminPage
	{
		public string QtyList1 = "";

		public string DateList = "";

		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected UserInCreaseStatistics() : base("m10", "tjp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (base.GetUrlParam("BeginDate") == "")
			{
				string text = "";
				ShopStatisticHelper.StatisticsOrdersByRecDate(System.DateTime.Today, UpdateAction.AllUpdate, 0, out text);
			}
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (this.txtBeginDate.TextToDate.HasValue)
			{
				this.BeginDate = new System.DateTime?(this.txtBeginDate.TextToDate.Value);
			}
			if (this.txtEndDate.TextToDate.HasValue)
			{
				this.EndDate = new System.DateTime?(this.txtEndDate.TextToDate.Value);
			}
			this.lastDay = 0;
			this.ReBind_Url(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				this.BeginDate = new System.DateTime?(System.DateTime.Today.AddDays(-6.0));
				this.EndDate = new System.DateTime?(System.DateTime.Today);
				if (base.GetUrlParam("BeginDate") != "")
				{
					this.BeginDate = new System.DateTime?(System.DateTime.Parse(base.GetUrlParam("BeginDate")));
				}
				if (base.GetUrlParam("EndDate") != "")
				{
					this.EndDate = new System.DateTime?(System.DateTime.Parse(base.GetUrlParam("EndDate")));
				}
				this.txtBeginDate.TextToDate = this.BeginDate;
				this.txtEndDate.TextToDate = this.EndDate;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay >= 7)
					{
						this.btnWeekView.BorderColor = ((this.lastDay == 7) ? System.Drawing.ColorTranslator.FromHtml("#1CA47D") : System.Drawing.ColorTranslator.FromHtml(""));
						this.btnMonthView.BorderColor = ((this.lastDay == 30) ? System.Drawing.ColorTranslator.FromHtml("#1CA47D") : System.Drawing.ColorTranslator.FromHtml(""));
						return;
					}
					this.btnWeekView.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					this.btnMonthView.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					return;
				}
			}
			else
			{
				if (this.txtBeginDate.TextToDate.HasValue)
				{
					this.BeginDate = new System.DateTime?(this.txtBeginDate.TextToDate.Value);
				}
				if (this.txtEndDate.TextToDate.HasValue)
				{
					this.EndDate = new System.DateTime?(this.txtEndDate.TextToDate.Value);
				}
			}
		}

		private void ReBind_Url(bool isSearch)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{
				{
					"BeginDate",
					this.BeginDate.ToString()
				},
				{
					"EndDate",
					this.EndDate.ToString()
				},
				{
					"lastDay",
					this.lastDay.ToString()
				}
			});
		}

		private void BindData()
		{
			OrderStatisticsQuery orderStatisticsQuery = new OrderStatisticsQuery();
			OrderStatisticsQuery arg_1E_0 = orderStatisticsQuery;
			System.DateTime arg_12_0 = this.BeginDate.Value;
			arg_1E_0.BeginDate = this.txtBeginDate.TextToDate;
			OrderStatisticsQuery arg_3B_0 = orderStatisticsQuery;
			System.DateTime arg_2F_0 = this.EndDate.Value;
			arg_3B_0.EndDate = this.txtEndDate.TextToDate;
			orderStatisticsQuery.SortOrder = SortAction.Desc;
			orderStatisticsQuery.SortBy = "RecDate";
			Globals.EntityCoding(orderStatisticsQuery, true);
			System.Data.DataTable dtDist = ShopStatisticHelper.Member_GetInCreateReport(orderStatisticsQuery);
			this.lastDay = (orderStatisticsQuery.EndDate.Value - orderStatisticsQuery.BeginDate.Value).Days + 1;
			this.LoadTradeDataList(dtDist, this.BeginDate.Value, this.lastDay);
		}

		protected void btnWeekView_Click(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.BeginDate = new System.DateTime?(now.AddDays(-6.0));
			this.EndDate = new System.DateTime?(now);
			this.lastDay = 7;
			this.ReBind_Url(true);
		}

		protected void btnMonthView_Click(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.BeginDate = new System.DateTime?(now.AddDays(-29.0));
			this.EndDate = new System.DateTime?(now);
			this.lastDay = 30;
			this.ReBind_Url(true);
		}

		private void LoadTradeDataList(System.Data.DataTable dtDist, System.DateTime BeginDate, int Days)
		{
			this.DateList = "";
			int num = 0;
			for (int i = 0; i < Days; i++)
			{
				System.Data.DataRow[] array = dtDist.Select("RecDate='" + BeginDate.AddDays((double)i).ToString("yyyy-MM-dd") + "' ");
				if (array.Length > 0)
				{
					this.QtyList1 += base.GetFieldValue(array[0], "NewMemberNumber");
				}
				else
				{
					this.QtyList1 += "0";
				}
				this.DateList = this.DateList + "'" + BeginDate.AddDays((double)i).ToString("yyyy-MM-dd") + "'";
				if (num < Days - 1)
				{
					this.DateList += ",";
					this.QtyList1 += ",";
				}
				num++;
			}
		}
	}
}
