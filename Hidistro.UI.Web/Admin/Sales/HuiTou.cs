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
	public class HuiTou : AdminPage
	{
		public int BuyerNumber;

		public int OldMember;

		public string OldMemberPercent = "0";

		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		private int Top = 10;

		public string QtyListOld = "";

		public string QtyListNew = "";

		public string QtyListAll = "";

		public string DateList = "";

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected HuiTou() : base("m10", "tjp03")
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
					"Top",
					this.Top.ToString()
				},
				{
					"lastDay",
					this.lastDay.ToString()
				}
			});
		}

		private void BindData()
		{
			System.Data.DataSet order_Member_Rebuy = ShopStatisticHelper.GetOrder_Member_Rebuy(this.BeginDate.Value, this.EndDate.Value);
			System.Data.DataTable dataTable = order_Member_Rebuy.Tables[0];
			foreach (System.Data.DataRow drOne in dataTable.Rows)
			{
				this.OldMember += base.GetFieldIntValue(drOne, "OldBuy");
				this.BuyerNumber += base.GetFieldIntValue(drOne, "totalBuy");
			}
			this.OldMemberPercent = "0";
			if (this.BuyerNumber > 0)
			{
				this.OldMemberPercent = (System.Convert.ToDecimal(this.OldMember) / System.Convert.ToDecimal(this.BuyerNumber) * 100m).ToString("N2");
			}
			System.Data.DataTable dataTable2 = order_Member_Rebuy.Tables[1];
			System.Data.DataTable dataTable3 = new System.Data.DataTable();
			dataTable3.Columns.Add("PayDate", typeof(string));
			dataTable3.Columns.Add("NewMemberQty", typeof(int));
			dataTable3.Columns.Add("OldMemberQty", typeof(int));
			int num = (this.EndDate.Value - this.BeginDate.Value).Days + 1;
			for (int i = 0; i < num; i++)
			{
				System.Data.DataRow dataRow = dataTable3.NewRow();
				dataRow["PayDate"] = this.BeginDate.Value.AddDays((double)i).ToString("yyyy-MM-dd");
				dataRow["OldMemberQty"] = 0;
				dataRow["NewMemberQty"] = 0;
				System.Data.DataRow[] array = dataTable2.Select("gpDate='" + this.BeginDate.Value.AddDays((double)i).ToString("yyyy-MM-dd") + "' ");
				if (array.Length > 0)
				{
					dataRow["OldMemberQty"] = array[0]["OldBuy"];
					dataRow["NewMemberQty"] = (int.Parse(array[0]["TotalBuy"].ToString()) - int.Parse(array[0]["OldBuy"].ToString())).ToString();
				}
				dataTable3.Rows.Add(dataRow);
				this.DateList = this.DateList + "'" + System.Convert.ToDateTime(dataRow["PayDate"].ToString()).ToString("yyyy-MM-dd") + "'";
				this.QtyListNew += base.GetFieldIntValue(dataRow, "NewMemberQty").ToString();
				this.QtyListOld += base.GetFieldDecimalValue(dataRow, "OldMemberQty").ToString();
				int fieldIntValue = base.GetFieldIntValue(dataRow, "NewMemberQty");
				int fieldIntValue2 = base.GetFieldIntValue(dataRow, "OldMemberQty");
				this.QtyListAll += (fieldIntValue + fieldIntValue2).ToString();
				if (i < num - 1)
				{
					this.DateList += ",";
					this.QtyListNew += ",";
					this.QtyListOld += ",";
					this.QtyListAll += ",";
				}
			}
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
