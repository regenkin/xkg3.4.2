using ASPNET.WebControls;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
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
	public class OrderStatistics : AdminPage
	{
		public int OrderNumber;

		public int FXOrderNumber;

		public string SaleAmountFee = "0";

		public string FXSaleAmountFee = "0";

		public string FXResultPercent = "0";

		public string FXCommissionFee = "0";

		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		private int Top = 10;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected System.Web.UI.WebControls.DropDownList ddlTop;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected OrderStatistics() : base("m10", "tjp02")
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
				this.Top = 10;
				if (base.GetUrlParam("BeginDate") != "")
				{
					this.BeginDate = new System.DateTime?(System.DateTime.Parse(base.GetUrlParam("BeginDate")));
				}
				if (base.GetUrlParam("EndDate") != "")
				{
					this.EndDate = new System.DateTime?(System.DateTime.Parse(base.GetUrlParam("EndDate")));
				}
				if (base.GetUrlParam("Top") != "")
				{
					this.Top = int.Parse(base.GetUrlParam("Top"));
				}
				this.txtBeginDate.TextToDate = this.BeginDate;
				this.txtEndDate.TextToDate = this.EndDate;
				this.ddlTop.SelectedValue = this.Top.ToString();
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
				this.Top = int.Parse(this.ddlTop.SelectedValue);
				int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
			}
		}

		private void ReBind_Url(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("BeginDate", this.BeginDate.ToString());
			nameValueCollection.Add("EndDate", this.EndDate.ToString());
			nameValueCollection.Add("Top", this.Top.ToString());
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			OrderStatisticsQuery orderStatisticsQuery = new OrderStatisticsQuery();
			orderStatisticsQuery.BeginDate = this.BeginDate;
			orderStatisticsQuery.EndDate = this.EndDate;
			orderStatisticsQuery.Top = new int?(this.Top);
			orderStatisticsQuery.PageIndex = this.pager.PageIndex;
			orderStatisticsQuery.PageSize = this.pager.PageSize;
			orderStatisticsQuery.SortOrder = SortAction.Desc;
			orderStatisticsQuery.SortBy = "SaleAmountFee";
			Globals.EntityCoding(orderStatisticsQuery, true);
			DbQueryResult orderStatisticReport = ShopStatisticHelper.GetOrderStatisticReport(orderStatisticsQuery);
			System.DateTime beginDate = System.DateTime.Today.AddDays(-6.0);
			if (this.txtBeginDate.TextToDate.HasValue)
			{
				beginDate = this.txtBeginDate.TextToDate.Value;
			}
			System.DateTime endDate = System.DateTime.Now;
			if (this.txtEndDate.TextToDate.HasValue)
			{
				endDate = this.txtEndDate.TextToDate.Value;
			}
			System.Data.DataRow order_Member_CountInfo = ShopStatisticHelper.GetOrder_Member_CountInfo(beginDate, endDate);
			this.OrderNumber = base.GetFieldIntValue(order_Member_CountInfo, "OrderNumber");
			this.SaleAmountFee = base.GetFieldDecimalValue(order_Member_CountInfo, "SaleAmountFee").ToString("N2");
			this.FXOrderNumber = base.GetFieldIntValue(order_Member_CountInfo, "FXOrderNumber");
			this.FXSaleAmountFee = base.GetFieldDecimalValue(order_Member_CountInfo, "FXSaleAmountFee").ToString("N2");
			this.FXResultPercent = "0";
			if (base.GetFieldDecimalValue(order_Member_CountInfo, "SaleAmountFee") > 0m)
			{
				this.FXResultPercent = System.Math.Round(base.GetFieldDecimalValue(order_Member_CountInfo, "FXSaleAmountFee") / base.GetFieldDecimalValue(order_Member_CountInfo, "SaleAmountFee") * 100m, 2).ToString("N2");
			}
			this.FXCommissionFee = base.GetFieldDecimalValue(order_Member_CountInfo, "FXCommissionFee").ToString("N2");
			this.rptList.DataSource = orderStatisticReport.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = orderStatisticReport.TotalRecords;
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

		protected void ddlTop_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.ReBind_Url(true);
		}
	}
}
