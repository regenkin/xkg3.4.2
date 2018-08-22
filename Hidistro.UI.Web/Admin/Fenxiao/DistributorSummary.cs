using Hidistro.ControlPanel.VShop;
using Hidistro.Entities.StatisticsReport;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorSummary : AdminPage
	{
		public decimal SaleAmountFee;

		public decimal FXValidOrderTotal;

		public decimal FXResultPercent;

		public int FXOrderNumber;

		public decimal FXCommissionFee;

		public decimal SaleAmountFee_Yesterday;

		public decimal FXValidOrderTotal_Yesterday;

		public decimal FXResultPercent_Yesterday;

		public int FXOrderNumber_Yesterday;

		public decimal FXCommissionFee_Yesterday;

		public int AgentNumber;

		public int NewAgentNumber_Yesterday;

		public decimal FinishedDrawCommissionFee;

		public decimal WaitDrawCommissionFee;

		public string QtyList1 = "";

		public string QtyList2 = "";

		public string QtyList3 = "";

		public string DateList = "";

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralOrders;

		protected System.Web.UI.HtmlControls.HtmlGenericControl OrdersTotal;

		protected System.Web.UI.HtmlControls.HtmlGenericControl TotalReferral;

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralBlance;

		protected DistributorSummary() : base("m05", "fxp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.DateTime arg_05_0 = System.DateTime.Today;
			System.DateTime beginDate = System.DateTime.Today.AddDays(-6.0);
			if (!base.IsPostBack)
			{
				string text = "";
				ShopStatisticHelper.StatisticsOrdersByRecDate(System.DateTime.Today, UpdateAction.AllUpdate, 0, out text);
				this.LoadData();
				this.LoadTradeDataList(beginDate, 7);
			}
		}

		private void LoadData()
		{
			System.DateTime today = System.DateTime.Today;
			System.DateTime dateTime = today.AddDays(-1.0);
			System.Data.DataRow dataRow = ShopStatisticHelper.Distributor_GetGlobal(today);
			if (dataRow != null)
			{
				this.SaleAmountFee = base.GetFieldDecimalValue(dataRow, "ValidOrderTotal");
				this.FXValidOrderTotal = base.GetFieldDecimalValue(dataRow, "FXValidOrderTotal");
				this.FXOrderNumber = base.GetFieldIntValue(dataRow, "FXOrderNumber");
				this.FXCommissionFee = base.GetFieldDecimalValue(dataRow, "FXSumCommission");
				this.FXResultPercent = 0m;
				if (this.SaleAmountFee > 0m && this.FXValidOrderTotal > 0m)
				{
					this.FXResultPercent = System.Convert.ToDecimal(this.FXValidOrderTotal / this.SaleAmountFee * 100m);
				}
			}
			dataRow = ShopStatisticHelper.Distributor_GetGlobal(dateTime);
			if (dataRow != null)
			{
				this.SaleAmountFee_Yesterday = base.GetFieldDecimalValue(dataRow, "ValidOrderTotal");
				this.FXValidOrderTotal_Yesterday = base.GetFieldDecimalValue(dataRow, "FXValidOrderTotal");
				this.FXOrderNumber_Yesterday = base.GetFieldIntValue(dataRow, "FXOrderNumber");
				this.FXCommissionFee_Yesterday = base.GetFieldDecimalValue(dataRow, "FXSumCommission");
				this.FXResultPercent_Yesterday = 0m;
				if (this.SaleAmountFee_Yesterday > 0m && this.FXValidOrderTotal_Yesterday > 0m)
				{
					this.FXResultPercent_Yesterday = System.Convert.ToDecimal(this.FXValidOrderTotal_Yesterday / this.SaleAmountFee_Yesterday * 100m);
				}
			}
			dataRow = ShopStatisticHelper.Distributor_GetGlobalTotal(dateTime);
			if (dataRow != null)
			{
				this.AgentNumber = base.GetFieldIntValue(dataRow, "DistributorNumber");
				this.NewAgentNumber_Yesterday = base.GetFieldIntValue(dataRow, "NewAgentNumber");
				this.FinishedDrawCommissionFee = base.GetFieldDecimalValue(dataRow, "FinishedDrawCommissionFee");
				this.WaitDrawCommissionFee = base.GetFieldDecimalValue(dataRow, "WaitDrawCommissionFee");
			}
		}

		private void LoadTradeDataList(System.DateTime BeginDate, int Days)
		{
			System.Data.DataTable trendDataList_FX = ShopStatisticHelper.GetTrendDataList_FX(BeginDate, Days);
			this.DateList = "";
			int num = 0;
			foreach (System.Data.DataRow dataRow in trendDataList_FX.Rows)
			{
				this.DateList = this.DateList + "'" + System.Convert.ToDateTime(dataRow["RecDate"].ToString()).ToString("yyyy-MM-dd") + "'";
				this.QtyList1 += base.GetFieldIntValue(dataRow, "NewAgentCount");
				this.QtyList2 += base.GetFieldDecimalValue(dataRow, "FXAmountFee");
				this.QtyList3 += base.GetFieldDecimalValue(dataRow, "FXCommisionFee");
				if (num < Days - 1)
				{
					this.DateList += ",";
					this.QtyList1 += ",";
					this.QtyList2 += ",";
					this.QtyList3 += ",";
				}
				num++;
			}
		}
	}
}
