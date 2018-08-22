using ASPNET.WebControls;
using Hidistro.ControlPanel.FengXiao;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.FenXiao;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorUpdateList : AdminPage
	{
		protected string localUrl = string.Empty;

		protected string htmlMenuTitleAdd = string.Empty;

		protected string ArticleTitle = string.Empty;

		private int pageno;

		protected int recordcount;

		protected int sendType;

		private string title = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtKey;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected DistributorUpdateList() : base("m05", "fxp12")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.pageno = Globals.RequestQueryNum("pageindex");
			if (this.pageno < 1)
			{
				this.pageno = 1;
			}
			this.localUrl = base.Request.Url.ToString();
			if (!base.IsPostBack)
			{
				this.BindData(this.pageno, this.sendType);
			}
		}

		private void BindData(int pageno, int sendtype)
		{
			DistributorGradeCommissionQuery distributorGradeCommissionQuery = new DistributorGradeCommissionQuery();
			distributorGradeCommissionQuery.SortBy = "ID";
			distributorGradeCommissionQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(distributorGradeCommissionQuery, true);
			distributorGradeCommissionQuery.PageIndex = pageno;
			distributorGradeCommissionQuery.PageSize = this.pager.PageSize;
			string text = Globals.RequestQueryStr("starttime");
			string text2 = Globals.RequestQueryStr("endtime");
			string text3 = Globals.RequestQueryStr("title");
			if (!string.IsNullOrEmpty(text3))
			{
				distributorGradeCommissionQuery.Title = text3;
				this.txtKey.Text = text3;
			}
			try
			{
				if (!string.IsNullOrEmpty(text))
				{
					distributorGradeCommissionQuery.StartTime = new System.DateTime?(System.DateTime.Parse(text));
					this.calendarStartDate.Text = distributorGradeCommissionQuery.StartTime.Value.ToString("yyyy-MM-dd");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					distributorGradeCommissionQuery.EndTime = new System.DateTime?(System.DateTime.Parse(text2));
					this.calendarEndDate.Text = distributorGradeCommissionQuery.EndTime.Value.ToString("yyyy-MM-dd");
				}
			}
			catch
			{
			}
			DbQueryResult dbQueryResult = DistributorGradeCommissionHelper.DistributorGradeCommission(distributorGradeCommissionQuery);
			this.rptList.DataSource = dbQueryResult.Data;
			this.rptList.DataBind();
			int totalRecords = dbQueryResult.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			this.recordcount = totalRecords;
			if (this.pager.TotalRecords <= this.pager.PageSize)
			{
				this.pager.Visible = false;
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string s = this.txtKey.Text.Trim();
			string text = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarStartDate$txtDateTimePicker");
			string text2 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarEndDate$txtDateTimePicker");
			string url = string.Concat(new string[]
			{
				"DistributorUpdateList.aspx?title=",
				base.Server.UrlEncode(s),
				"&starttime=",
				text,
				"&endtime=",
				text2
			});
			base.Response.Redirect(url);
			base.Response.End();
		}
	}
}
