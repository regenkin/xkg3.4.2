using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class MembersIntegralDetail : AdminPage
	{
		private int userId;

		private System.DateTime? startDate;

		private System.DateTime? endDate;

		public string clientType;

		public int IntegralStatus = -1;

		public string ValidSmsNum = "0";

		protected Script Script5;

		protected Script Script6;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.DropDownList drIntegralStatus;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected PageSize hrefPageSize;

		protected Pager pager;

		protected Grid grdMemberList;

		protected Pager pager1;

		public MembersIntegralDetail() : base("m04", "hyp10")
		{
		}

		private void BindConsultation()
		{
			IntegralDetailQuery integralDetailQuery = new IntegralDetailQuery();
			integralDetailQuery.PageIndex = this.pager.PageIndex;
			integralDetailQuery.UserId = this.userId;
			integralDetailQuery.SortBy = this.grdMemberList.SortOrderBy;
			integralDetailQuery.PageSize = this.pager.PageSize;
			integralDetailQuery.IntegralStatus = this.IntegralStatus;
			integralDetailQuery.StartTime = this.startDate;
			integralDetailQuery.EndTime = this.endDate;
			if (this.grdMemberList.SortOrder.ToLower() == "desc")
			{
				integralDetailQuery.SortOrder = SortAction.Desc;
			}
			DbQueryResult integralDetail = MemberHelper.GetIntegralDetail(integralDetailQuery);
			this.grdMemberList.DataSource = integralDetail.Data;
			this.grdMemberList.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = integralDetail.TotalRecords);
		}

		private void grdMemberList_ReBindData(object sender)
		{
			this.BindConsultation();
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.BindData();
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
			{
				nameValueCollection.Add("userId", this.Page.Request.QueryString["userId"].ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			nameValueCollection.Add("IntegralStatus", this.drIntegralStatus.SelectedValue.ToString());
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		protected void BindData()
		{
			if (!this.Page.IsPostBack)
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["userId"], out num))
				{
					this.userId = num;
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
				{
					this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
				{
					this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IntegralStatus"]))
				{
					this.IntegralStatus = int.Parse(this.Page.Request.QueryString["IntegralStatus"]);
				}
				this.calendarStartDate.SelectedDate = this.startDate;
				this.calendarEndDate.SelectedDate = this.endDate;
				this.drIntegralStatus.SelectedValue = this.IntegralStatus.ToString();
				IntegralDetailQuery integralDetailQuery = new IntegralDetailQuery();
				integralDetailQuery.PageIndex = this.pager.PageIndex;
				integralDetailQuery.UserId = this.userId;
				integralDetailQuery.SortBy = this.grdMemberList.SortOrderBy;
				integralDetailQuery.PageSize = this.pager.PageSize;
				integralDetailQuery.IntegralStatus = this.IntegralStatus;
				integralDetailQuery.StartTime = this.startDate;
				integralDetailQuery.EndTime = this.endDate;
				if (this.grdMemberList.SortOrder.ToLower() == "desc")
				{
					integralDetailQuery.SortOrder = SortAction.Desc;
				}
				DbQueryResult integralDetail = MemberHelper.GetIntegralDetail(integralDetailQuery);
				this.grdMemberList.DataSource = integralDetail.Data;
				this.grdMemberList.DataBind();
				this.pager1.TotalRecords = (this.pager.TotalRecords = integralDetail.TotalRecords);
				return;
			}
			this.startDate = this.calendarStartDate.SelectedDate;
			this.endDate = this.calendarEndDate.SelectedDate;
			this.IntegralStatus = int.Parse(this.drIntegralStatus.SelectedValue);
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
