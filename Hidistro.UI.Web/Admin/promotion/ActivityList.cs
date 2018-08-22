using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class ActivityList : AdminPage
	{
		protected int _status;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblAll;

		protected System.Web.UI.WebControls.Label lblIn;

		protected System.Web.UI.WebControls.Label lblEnd;

		protected System.Web.UI.WebControls.Label lblUnBegin;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Button btnDelete;

		protected System.Web.UI.WebControls.Button DelBtn;

		protected Grid grdCoupondsList;

		protected Pager pager1;

		protected ActivityList() : base("m08", "yxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdCoupondsList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdCoupondsList_RowDeleting);
			this.btnSeach.Click += new System.EventHandler(this.btnSeach_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("status"))
			{
				if (!base.Request["status"].ToString().bInt(ref this._status))
				{
					this._status = 0;
				}
			}
			else
			{
				this._status = 0;
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void DelRows()
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCoupondsList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					int id = System.Convert.ToInt32(this.grdCoupondsList.DataKeys[gridViewRow.RowIndex].Value);
					ActivityHelper.Delete(id);
				}
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的活动", true);
		}

		private void grdCoupondsList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int id = (int)this.grdCoupondsList.DataKeys[e.RowIndex].Value;
			if (!ActivityHelper.Delete(id))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的活动", true);
		}

		private void BindData()
		{
			string text = this.txt_name.Text;
			System.DateTime? begin = null;
			System.DateTime? end = null;
			begin = this.calendarStartDate.SelectedDate;
			end = this.calendarEndDate.SelectedDate;
			DbQueryResult dbQueryResult = ActivityHelper.Query(new ActivitySearch
			{
				status = (ActivityStatus)this._status,
				IsCount = true,
				PageIndex = this.pager1.PageIndex,
				PageSize = this.pager1.PageSize,
				SortBy = "ActivitiesId",
				SortOrder = SortAction.Desc,
				Name = text,
				begin = begin,
				end = end
			});
			System.Data.DataTable dataTable = (System.Data.DataTable)dbQueryResult.Data;
			if (dataTable != null)
			{
				dataTable.Columns.Add("sStatus");
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						System.DateTime t = System.DateTime.Parse(dataTable.Rows[i]["StartTime"].ToString());
						System.DateTime t2 = System.DateTime.Parse(dataTable.Rows[i]["EndTime"].ToString());
						if (t > System.DateTime.Now)
						{
							dataTable.Rows[i]["sStatus"] = "未开始";
						}
						else if (t2 < System.DateTime.Now)
						{
							dataTable.Rows[i]["sStatus"] = "已结束";
						}
						else
						{
							dataTable.Rows[i]["sStatus"] = "进行中";
						}
					}
				}
			}
			this.grdCoupondsList.DataSource = dataTable;
			this.grdCoupondsList.DataBind();
			this.pager1.TotalRecords = dbQueryResult.TotalRecords;
			this.CountTotal();
		}

		private void CountTotal()
		{
			ActivitySearch activitySearch = new ActivitySearch();
			activitySearch.status = ActivityStatus.All;
			activitySearch.IsCount = true;
			activitySearch.PageIndex = this.pager1.PageIndex;
			activitySearch.PageSize = this.pager1.PageSize;
			activitySearch.SortBy = "ActivitiesId";
			activitySearch.SortOrder = SortAction.Desc;
			DbQueryResult dbQueryResult = ActivityHelper.Query(activitySearch);
			System.Data.DataTable dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblAll.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.In;
			dbQueryResult = ActivityHelper.Query(activitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblIn.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.End;
			dbQueryResult = ActivityHelper.Query(activitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblEnd.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.unBegin;
			dbQueryResult = ActivityHelper.Query(activitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblUnBegin.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCoupondsList.Rows)
			{
				if (gridViewRow.RowIndex >= 0)
				{
					System.Web.UI.WebControls.CheckBox checkBox = gridViewRow.Cells[0].Controls[0] as System.Web.UI.WebControls.CheckBox;
					if (checkBox.Checked)
					{
						list.Add(int.Parse(this.grdCoupondsList.DataKeys[gridViewRow.RowIndex].Value.ToString()));
					}
				}
			}
			if (list.Count <= 0)
			{
				this.ShowMsg("请至少选择一条要删除的数据！", false);
				return;
			}
			foreach (int current in list)
			{
				ActivityHelper.Delete(current);
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的活动", true);
		}

		protected void Unnamed_Click(object sender, System.EventArgs e)
		{
			this.DelRows();
		}
	}
}
