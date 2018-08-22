using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class ShareActList : AdminPage
	{
		protected ShareActivityStatus status;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Label lblAll;

		protected System.Web.UI.WebControls.Label lblIn;

		protected System.Web.UI.WebControls.Label lblEnd;

		protected System.Web.UI.WebControls.Label lblUnBegin;

		protected PageSize PageSize1;

		protected System.Web.UI.WebControls.Button lkDelete;

		protected System.Web.UI.WebControls.Button DelBtn;

		protected System.Web.UI.WebControls.TextBox txt_Ids;

		protected System.Web.UI.WebControls.Repeater grdDate;

		protected Pager pager1;

		protected ShareActList() : base("m08", "yxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSeach.Click += new System.EventHandler(this.btnSeach_Click);
			this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
			this.lkDelete.Click += new System.EventHandler(this.lkDelete_Click);
			this.grdDate.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdDate_ItemCommand);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("status"))
			{
				int num = 0;
				if (base.Request["status"].ToString().bInt(ref num))
				{
					this.status = (ShareActivityStatus)num;
				}
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			string text = this.txt_name.Text.Trim();
			DbQueryResult dbQueryResult = ShareActHelper.Query(new ShareActivitySearch
			{
				status = this.status,
				IsCount = true,
				PageIndex = this.pager1.PageIndex,
				PageSize = this.pager1.PageSize,
				SortBy = "Id",
				SortOrder = SortAction.Desc,
				CouponName = text
			});
			System.Data.DataTable dataSource = (System.Data.DataTable)dbQueryResult.Data;
			this.grdDate.DataSource = dataSource;
			this.grdDate.DataBind();
			this.pager1.TotalRecords = dbQueryResult.TotalRecords;
			this.CountTotal(text);
		}

		private void CountTotal(string name)
		{
			ShareActivitySearch shareActivitySearch = new ShareActivitySearch();
			shareActivitySearch.status = ShareActivityStatus.All;
			shareActivitySearch.IsCount = true;
			shareActivitySearch.PageIndex = this.pager1.PageIndex;
			shareActivitySearch.PageSize = this.pager1.PageSize;
			shareActivitySearch.SortBy = "Id";
			shareActivitySearch.SortOrder = SortAction.Desc;
			shareActivitySearch.CouponName = name;
			DbQueryResult dbQueryResult = ShareActHelper.Query(shareActivitySearch);
			System.Data.DataTable dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblAll.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			shareActivitySearch.status = ShareActivityStatus.In;
			dbQueryResult = ShareActHelper.Query(shareActivitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblIn.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			shareActivitySearch.status = ShareActivityStatus.End;
			dbQueryResult = ShareActHelper.Query(shareActivitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblEnd.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			shareActivitySearch.status = ShareActivityStatus.unBegin;
			dbQueryResult = ShareActHelper.Query(shareActivitySearch);
			dataTable = (System.Data.DataTable)dbQueryResult.Data;
			this.lblUnBegin.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		protected void DelBtn_Click(object sender, System.EventArgs e)
		{
		}

		private void grdDate_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (string.IsNullOrEmpty(e.CommandArgument.ToString()))
				{
					return;
				}
				ShareActHelper.Delete(int.Parse(e.CommandArgument.ToString()));
				this.BindData();
			}
		}

		protected void lkDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择活动！", false);
				return;
			}
			if (text.Length > 1)
			{
				text = text.TrimStart(new char[]
				{
					','
				}).TrimEnd(new char[]
				{
					','
				});
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				ShareActHelper.Delete(int.Parse(s));
			}
			this.ShowMsg("批量删除成功！", true);
			this.BindData();
		}
	}
}
