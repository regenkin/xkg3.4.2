using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.settings
{
	public class ManageLogs : AdminPage
	{
		public int lastDay;

		private string FromDate = "";

		private string ToDate = "";

		protected Script Script5;

		protected Script Script7;

		protected Script Script6;

		protected Script Script9;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected LogsUserNameDropDownList dropOperationUserNames;

		protected ucDateTimePicker calFromDate;

		protected ucDateTimePicker calToDate;

		protected System.Web.UI.WebControls.Button btnQueryLogs;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.WebControls.Button btnDel;

		protected System.Web.UI.WebControls.Button btnClear;

		protected PageSize PageSize1;

		protected System.Web.UI.WebControls.Repeater dlstLog1;

		protected Pager pager;

		protected ManageLogs() : base("m09", "szp12")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnQueryLogs.Click += new System.EventHandler(this.btnQueryLogs_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropOperationUserNames.DataBind();
				this.BindLogs();
			}
		}

		protected void DeleteLog(object sender, System.EventArgs e)
		{
			long logId = 0L;
			if (!long.TryParse(((System.Web.UI.WebControls.Button)sender).CommandArgument, out logId))
			{
				this.ShowMsg("非正常删除！", true);
				return;
			}
			if (EventLogs.DeleteLog(logId))
			{
				this.BindLogs();
				this.ShowMsg("成功删除了单个操作日志", true);
				return;
			}
			this.ShowMsg("在删除过程中出现未知错误", false);
		}

		private void btnQueryLogs_Click(object sender, System.EventArgs e)
		{
			this.lastDay = 0;
			this.ReloadManagerLogs(true);
		}

		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OperationUserName", this.dropOperationUserNames.SelectedValue);
			if (this.lastDay > 0)
			{
				nameValueCollection.Add("FromDate", this.FromDate);
				nameValueCollection.Add("ToDate", this.ToDate);
			}
			else
			{
				if (this.calFromDate.SelectedDate.HasValue)
				{
					nameValueCollection.Add("FromDate", this.calFromDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
				}
				if (this.calToDate.SelectedDate.HasValue)
				{
					nameValueCollection.Add("ToDate", this.calToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
				}
			}
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
			base.ReloadPage(nameValueCollection);
		}

		private void DeleteCheck()
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的操作日志项", false);
				return;
			}
			int num = EventLogs.DeleteLogs(text);
			this.BindLogs();
			this.ShowMsg(string.Format("成功删除了{0}个操作日志", num), true);
		}

		public void BindLogs()
		{
			OperationLogQuery operationLogQuery = this.GetOperationLogQuery();
			DbQueryResult logs = EventLogs.GetLogs(operationLogQuery);
			this.dlstLog1.DataSource = logs.Data;
			this.dlstLog1.DataBind();
			this.SetSearchControl();
			this.pager.TotalRecords = logs.TotalRecords;
		}

		private void SetSearchControl()
		{
			if (!this.Page.IsCallback)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
				{
					this.dropOperationUserNames.SelectedValue = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
				}
				try
				{
					if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
					{
						this.calFromDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["FromDate"]));
					}
					if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
					{
						this.calToDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["ToDate"]));
					}
				}
				catch (System.Exception)
				{
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay == 30)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("#1CA47D");
						return;
					}
					if (this.lastDay == 7)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#1CA47D");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						return;
					}
					this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
				}
			}
		}

		private OperationLogQuery GetOperationLogQuery()
		{
			OperationLogQuery operationLogQuery = new OperationLogQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
			{
				operationLogQuery.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
			{
				operationLogQuery.FromDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
			{
				operationLogQuery.ToDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
			}
			operationLogQuery.Page.PageIndex = this.pager.PageIndex;
			operationLogQuery.Page.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
			{
				operationLogQuery.Page.SortBy = this.Page.Request.QueryString["SortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
			{
				operationLogQuery.Page.SortOrder = SortAction.Desc;
			}
			return operationLogQuery;
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
			this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#1CA47D");
			System.DateTime now = System.DateTime.Now;
			this.calToDate.SelectedDate = new System.DateTime?(now);
			this.calFromDate.SelectedDate = new System.DateTime?(now.AddDays(-6.0));
			this.ToDate = now.ToString("yyyy-MM-dd");
			this.FromDate = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.ReloadManagerLogs(true);
		}

		protected void Button4_Click(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.calToDate.SelectedDate = new System.DateTime?(now);
			this.calFromDate.SelectedDate = new System.DateTime?(now.AddMonths(-1));
			this.ToDate = now.ToString("yyyy-MM-dd");
			this.FromDate = now.AddMonths(-1).ToString("yyyy-MM-dd");
			this.lastDay = 30;
			this.ReloadManagerLogs(true);
		}

		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			this.DeleteCheck();
		}

		protected void btnClear_Click(object sender, System.EventArgs e)
		{
			if (EventLogs.DeleteAllLogs())
			{
				this.BindLogs();
				this.ShowMsg("成功删除了所有操作日志", true);
				return;
			}
			this.ShowMsg("在删除过程中出现未知错误", false);
		}
	}
}
