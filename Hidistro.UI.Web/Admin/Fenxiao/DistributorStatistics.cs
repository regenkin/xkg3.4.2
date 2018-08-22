using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorStatistics : AdminPage
	{
		private string StartTime = "";

		private string EndTime = "";

		public int lastDay;

		private int i;

		private int rows;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnQueryLogs;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.WebControls.Repeater reDistributor;

		protected Pager pager;

		protected DistributorStatistics() : base("m05", "fxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.reDistributor.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
			this.BindData();
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
				{
					this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
					this.calendarStartDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.StartTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
				{
					this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
					this.calendarEndDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.EndTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay == 30)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FF00CC");
						return;
					}
					if (this.lastDay == 7)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FF00CC");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						return;
					}
					this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					return;
				}
			}
			else
			{
				if (this.calendarStartDate.SelectedDate.HasValue)
				{
					this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
				}
				if (this.calendarEndDate.SelectedDate.HasValue)
				{
					this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
				}
			}
		}

		private void reDistributor_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litph");
				this.i++;
				this.rows = (this.pager.PageIndex - 1) * this.pager.PageSize + this.i;
				if (this.rows == 1)
				{
					literal.Text = "<img src=\"../images/0001.gif\"></img>";
					return;
				}
				if (this.rows == 2)
				{
					literal.Text = "<img src=\"../images/0002.gif\"></img>";
					return;
				}
				if (this.rows == 3)
				{
					literal.Text = "<img src=\"../images/0003.gif\"></img>";
					return;
				}
				literal.Text = (int.Parse(literal.Text) + this.rows).ToString();
			}
		}

		protected void Button4_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
			this.lastDay = 30;
			this.ReBind(true);
		}

		protected void Button1_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.ReBind(true);
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("StartTime", this.StartTime);
			nameValueCollection.Add("EndTime", this.EndTime);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			DbQueryResult distributorsRankings = VShopHelper.GetDistributorsRankings(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex);
			this.reDistributor.DataSource = distributorsRankings.Data;
			this.reDistributor.DataBind();
			this.pager.TotalRecords = distributorsRankings.TotalRecords;
		}

		protected void btnQueryLogs_Click(object sender, System.EventArgs e)
		{
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			this.lastDay = 0;
			this.ReBind(true);
		}
	}
}
