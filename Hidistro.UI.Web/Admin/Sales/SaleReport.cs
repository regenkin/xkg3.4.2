using Hidistro.ControlPanel.VShop;
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
	public class SaleReport : AdminPage
	{
		public decimal BuyerAvgPrice;

		public decimal OrderNumber;

		public decimal BuyerNumber;

		public decimal SaleAmountFee;

		public int NewMemberNumber;

		public int NewAgentNumber;

		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		public string DateListA = "";

		public string DateListB = "";

		public string QtyListA1 = "";

		public string QtyListA2 = "";

		public string QtyListB1 = "";

		public string QtyListB2 = "";

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected SaleReport() : base("m10", "tjp01")
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
			if (!base.IsPostBack)
			{
				this.LoadParameters();
				this.BindData();
			}
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
			System.DateTime dateTime = System.DateTime.Today.AddDays(-6.0);
			if (this.txtBeginDate.TextToDate.HasValue)
			{
				dateTime = System.DateTime.Parse(this.txtBeginDate.TextToDate.Value.ToString());
			}
			System.DateTime dateTime2 = System.DateTime.Now;
			if (this.txtEndDate.TextToDate.HasValue)
			{
				dateTime2 = System.DateTime.Parse(this.txtEndDate.TextToDate.Value.ToString());
			}
			System.Data.DataRow order_Member_CountInfo = ShopStatisticHelper.GetOrder_Member_CountInfo(dateTime, dateTime2);
			System.Data.DataTable saleReport = ShopStatisticHelper.GetSaleReport(dateTime, dateTime2);
			int num = (dateTime2 - dateTime).Days;
			this.OrderNumber = base.GetFieldDecimalValue(order_Member_CountInfo, "OrderNumber");
			this.BuyerNumber = base.GetFieldDecimalValue(order_Member_CountInfo, "BuyerNumber");
			this.SaleAmountFee = base.GetFieldDecimalValue(order_Member_CountInfo, "SaleAmountFee");
			this.BuyerAvgPrice = 0m;
			if (this.BuyerNumber > 0m)
			{
				this.BuyerAvgPrice = System.Math.Round(this.SaleAmountFee / this.BuyerNumber, 2);
			}
			this.NewMemberNumber = base.GetFieldIntValue(order_Member_CountInfo, "NewMemberNumber");
			this.NewAgentNumber = base.GetFieldIntValue(order_Member_CountInfo, "NewAgentNumber");
			this.DateListA = "";
			this.DateListB = "";
			int num2 = 0;
			num = saleReport.Rows.Count;
			foreach (System.Data.DataRow dataRow in saleReport.Rows)
			{
				this.DateListA = this.DateListA + "'" + System.Convert.ToDateTime(dataRow["RecDate"].ToString()).ToString("yyyy-MM-dd") + "'";
				this.QtyListA1 += base.GetFieldIntValue(dataRow, "OrderNumber").ToString();
				this.QtyListA2 += base.GetFieldDecimalValue(dataRow, "SaleAmountFee").ToString();
				this.QtyListB1 += base.GetFieldIntValue(dataRow, "NewMemberNumber").ToString();
				this.QtyListB2 += base.GetFieldIntValue(dataRow, "NewAgentNumber").ToString();
				if (num2 < num - 1)
				{
					this.DateListA += ",";
					this.QtyListA1 += ",";
					this.QtyListA2 += ",";
					this.DateListB += ",";
					this.QtyListB1 += ",";
					this.QtyListB2 += ",";
				}
				num2++;
			}
			this.DateListB = this.DateListA;
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

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			bool arg_13_0 = this.txtBeginDate.TextToDate.HasValue;
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			this.txtBeginDate.TextToDate = new System.DateTime?(System.DateTime.Today.AddDays(-3.0));
		}
	}
}
