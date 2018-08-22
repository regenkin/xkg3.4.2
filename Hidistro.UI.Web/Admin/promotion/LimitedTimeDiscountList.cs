using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class LimitedTimeDiscountList : AdminPage
	{
		protected int status;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblAll;

		protected System.Web.UI.WebControls.Label lblIn;

		protected System.Web.UI.WebControls.Label lblEnd;

		protected System.Web.UI.WebControls.Label lblUnBegin;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtActivityName;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected Grid grdLimitedTimeDiscountList;

		protected Pager pager1;

		protected LimitedTimeDiscountList() : base("m08", "yxp24")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdLimitedTimeDiscountList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdLimitedTimeDiscountList_RowDeleting);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("status"))
			{
				if (!base.Request["status"].ToString().bInt(ref this.status))
				{
					this.status = 0;
				}
			}
			else
			{
				this.status = 0;
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected string GetStatusHtml(string status)
		{
			if (status == "3")
			{
				return "恢复活动";
			}
			return "暂停活动";
		}

		private void grdLimitedTimeDiscountList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int id = (int)this.grdLimitedTimeDiscountList.DataKeys[e.RowIndex].Value;
			if (!LimitedTimeDiscountHelper.UpdateDiscountStatus(id, DiscountStatus.Delete))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的活动", true);
		}

		private void CountTotal()
		{
			ActivitySearch activitySearch = new ActivitySearch();
			activitySearch.status = ActivityStatus.All;
			activitySearch.IsCount = true;
			activitySearch.PageIndex = this.pager1.PageIndex;
			activitySearch.PageSize = this.pager1.PageSize;
			activitySearch.SortBy = "LimitedTimeDiscountId";
			activitySearch.SortOrder = SortAction.Desc;
			DbQueryResult discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(activitySearch);
			System.Data.DataTable dataTable = (System.Data.DataTable)discountQuery.Data;
			this.lblAll.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.In;
			discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(activitySearch);
			dataTable = (System.Data.DataTable)discountQuery.Data;
			this.lblIn.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.End;
			discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(activitySearch);
			dataTable = (System.Data.DataTable)discountQuery.Data;
			this.lblEnd.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			activitySearch.status = ActivityStatus.unBegin;
			discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(activitySearch);
			dataTable = (System.Data.DataTable)discountQuery.Data;
			this.lblUnBegin.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
		}

		private void BindData()
		{
			string text = this.txtActivityName.Text;
			System.DateTime? selectedDate = this.calendarStartDate.SelectedDate;
			System.DateTime? selectedDate2 = this.calendarEndDate.SelectedDate;
			DbQueryResult discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(new ActivitySearch
			{
				status = (ActivityStatus)this.status,
				IsCount = true,
				PageIndex = this.pager1.PageIndex,
				PageSize = this.pager1.PageSize,
				SortBy = "LimitedTimeDiscountId",
				SortOrder = SortAction.Desc,
				Name = text,
				begin = selectedDate,
				end = selectedDate2
			});
			System.Data.DataTable dataSource = (System.Data.DataTable)discountQuery.Data;
			this.grdLimitedTimeDiscountList.DataSource = dataSource;
			this.grdLimitedTimeDiscountList.DataBind();
			this.pager1.TotalRecords = discountQuery.TotalRecords;
			this.CountTotal();
		}

		public string GetMemberGarde(string applyMembers, string defualtGroup, string customGroup)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (!string.IsNullOrEmpty(applyMembers))
			{
				System.Collections.Generic.IList<MemberGradeInfo> memberGrades;
				if (applyMembers == "0")
				{
					memberGrades = MemberHelper.GetMemberGrades();
				}
				else
				{
					memberGrades = MemberHelper.GetMemberGrades(applyMembers);
				}
				if (memberGrades != null && memberGrades.Count > 0)
				{
					foreach (MemberGradeInfo current in memberGrades)
					{
						stringBuilder.Append(current.Name + ",");
					}
				}
			}
			if (!string.IsNullOrEmpty(customGroup))
			{
				System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList;
				if (customGroup == "0")
				{
					customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
				}
				else
				{
					customGroupingList = CustomGroupingHelper.GetCustomGroupingList(customGroup);
				}
				if (customGroupingList != null && customGroupingList.Count > 0)
				{
					foreach (CustomGroupingInfo current2 in customGroupingList)
					{
						stringBuilder.Append(current2.GroupName + ",");
					}
				}
			}
			if (!string.IsNullOrEmpty(defualtGroup))
			{
				if (defualtGroup.IndexOf('1') > -1)
				{
					stringBuilder.Append("新会员,");
				}
				if (defualtGroup.IndexOf('2') > -1)
				{
					stringBuilder.Append("活跃会员,");
				}
				if (defualtGroup.IndexOf('3') > -1)
				{
					stringBuilder.Append("沉睡会员,");
				}
			}
			if (!string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
			}
			return "";
		}

		protected string GetDescription(string description)
		{
			if (description.Length > 8)
			{
				return description.Substring(0, 8) + "..";
			}
			return description;
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}
	}
}
