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
	public class OrderStatisticsDetail : AdminPage
	{
		public int FXOrderNumber;

		public int BuyerNumber;

		public string FXSaleAmountFee = "0";

		public string FXBuyAvgPrice = "0";

		public string FXCommissionFee = "0";

		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		private int Top = 10;

		public int UserId;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected OrderStatisticsDetail() : base("m10", "tjp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
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
				this.BeginDate = new System.DateTime?(System.DateTime.Today.AddDays(-7.0));
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay >= 7)
					{
						this.btnWeekView.BorderColor = ((this.lastDay == 7) ? System.Drawing.ColorTranslator.FromHtml("#1CA47D") : System.Drawing.ColorTranslator.FromHtml(""));
						this.btnMonthView.BorderColor = ((this.lastDay == 30) ? System.Drawing.ColorTranslator.FromHtml("#1CA47D") : System.Drawing.ColorTranslator.FromHtml(""));
					}
					else
					{
						this.btnWeekView.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.btnMonthView.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					}
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
				int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
			}
			this.UserId = base.GetUrlIntParam("UserId");
		}

		private void ReBind_Url(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("BeginDate", this.BeginDate.ToString());
			nameValueCollection.Add("EndDate", this.EndDate.ToString());
			nameValueCollection.Add("Top", this.Top.ToString());
			nameValueCollection.Add("UserId", this.UserId.ToString());
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			OrderStatisticsQuery_UnderShop orderStatisticsQuery_UnderShop = new OrderStatisticsQuery_UnderShop();
			orderStatisticsQuery_UnderShop.BeginDate = this.BeginDate;
			orderStatisticsQuery_UnderShop.EndDate = this.EndDate;
			orderStatisticsQuery_UnderShop.Top = new int?(this.Top);
			orderStatisticsQuery_UnderShop.PageIndex = this.pager.PageIndex;
			orderStatisticsQuery_UnderShop.PageSize = this.pager.PageSize;
			orderStatisticsQuery_UnderShop.SortOrder = SortAction.Desc;
			orderStatisticsQuery_UnderShop.SortBy = "SaleAmountFee";
			orderStatisticsQuery_UnderShop.ShopLevel = new int?(1);
			orderStatisticsQuery_UnderShop.AgentId = new int?(base.GetUrlIntParam("UserId"));
			Globals.EntityCoding(orderStatisticsQuery_UnderShop, true);
			DbQueryResult orderStatisticReport_UnderShop = ShopStatisticHelper.GetOrderStatisticReport_UnderShop(orderStatisticsQuery_UnderShop);
			this.pager.TotalRecords = orderStatisticReport_UnderShop.TotalRecords;
			this.rptList.DataSource = orderStatisticReport_UnderShop.Data;
			this.rptList.DataBind();
			System.Data.DataRow orderStatisticReportGlobalByAgentID = ShopStatisticHelper.GetOrderStatisticReportGlobalByAgentID(orderStatisticsQuery_UnderShop);
			this.FXOrderNumber = base.GetFieldIntValue(orderStatisticReportGlobalByAgentID, "OrderNumber");
			this.BuyerNumber = base.GetFieldIntValue(orderStatisticReportGlobalByAgentID, "BuyerNumber");
			this.FXSaleAmountFee = base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "SaleAmountFee").ToString("N2");
			this.FXBuyAvgPrice = "0";
			if (base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "BuyerNumber") > 0m)
			{
				this.FXBuyAvgPrice = System.Math.Round(base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "SaleAmountFee") / base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "BuyerNumber"), 2).ToString("N2");
			}
			this.FXCommissionFee = base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "CommissionAmountFee").ToString("N2");
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
	}
}
