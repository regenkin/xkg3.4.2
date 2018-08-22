using ASPNET.WebControls;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.StatisticsReport;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Sales
{
	public class ProductSaleRanking : AdminPage
	{
		private System.DateTime? BeginDate;

		private System.DateTime? EndDate;

		private int lastDay;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected ucDateTimePicker txtBeginDate;

		protected ucDateTimePicker txtEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnWeekView;

		protected System.Web.UI.WebControls.Button btnMonthView;

		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;

		protected ExportFormatRadioButtonList exportFormatRadioButtonList;

		protected System.Web.UI.WebControls.Button btnExport;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected ProductSaleRanking() : base("m10", "tjp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
				this.exportFieldsCheckBoxList.Items.Clear();
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("排名", "RankIndex"));
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("商品名称", "ProductName"));
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("销售量", "SaleQty"));
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("销售额", "SaleAmountFee"));
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("购买人数", "BuyerNumber"));
				this.exportFieldsCheckBoxList.Items.Add(new System.Web.UI.WebControls.ListItem("转化率", "ConversionRate"));
			}
		}

		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (this.exportFieldsCheckBoxList.SelectedItem == null)
			{
				this.ShowMsg("请选择需要导出的记录", false);
				return;
			}
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.exportFieldsCheckBoxList.Items)
			{
				if (listItem.Selected)
				{
					list.Add(listItem.Value);
					list2.Add(listItem.Text);
				}
			}
			System.Data.DataTable dataTable = ShopStatisticHelper.Product_GetStatisticReport_NoPage(new OrderStatisticsQuery
			{
				BeginDate = this.BeginDate,
				EndDate = this.EndDate
			}, list);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (string current in list2)
			{
				stringBuilder.Append(current + ",");
				if (current == list2[list2.Count - 1])
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("\r\n");
				}
			}
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				foreach (string current2 in list)
				{
					stringBuilder.Append(dataRow[current2]).Append(",");
					if (current2 == list[list2.Count - 1])
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("\r\n");
					}
				}
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			if (this.exportFormatRadioButtonList.SelectedValue == "csv")
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DataExport.csv");
				this.Page.Response.ContentType = "application/octet-stream";
			}
			else
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DataExport.txt");
				this.Page.Response.ContentType = "application/vnd.ms-word";
			}
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
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
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("BeginDate", this.BeginDate.ToString());
			nameValueCollection.Add("EndDate", this.EndDate.ToString());
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
			orderStatisticsQuery.PageIndex = this.pager.PageIndex;
			orderStatisticsQuery.PageSize = this.pager.PageSize;
			orderStatisticsQuery.SortOrder = SortAction.Desc;
			orderStatisticsQuery.SortBy = "SaleAmountFee";
			Globals.EntityCoding(orderStatisticsQuery, true);
			DbQueryResult dbQueryResult = ShopStatisticHelper.Product_GetStatisticReport(orderStatisticsQuery);
			this.rptList.DataSource = dbQueryResult.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
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
