using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class BalanceDrawApplyErrorList : AdminPage
	{
		private string RequestStartTime = "";

		private string RequestEndTime = "";

		private string StoreName = "";

		protected System.Web.UI.WebControls.HiddenField hSerialID;

		protected System.Web.UI.WebControls.TextBox SignalrefuseMks;

		protected System.Web.UI.WebControls.Button Button3;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.LinkButton Frist;

		protected System.Web.UI.WebControls.LinkButton Second;

		protected System.Web.UI.WebControls.Repeater reCommissions;

		protected Pager pager;

		protected BalanceDrawApplyErrorList() : base("m05", "fxp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.Button3.Click += new System.EventHandler(this.Button3_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void Button3_Click(object sender, System.EventArgs e)
		{
			int[] array = new int[]
			{
				int.Parse(this.hSerialID.Value)
			};
			if (array[0] != 0)
			{
				int balanceDrawRequestIsCheckStatus = DistributorsBrower.GetBalanceDrawRequestIsCheckStatus(array[0]);
				if (balanceDrawRequestIsCheckStatus == -1 || balanceDrawRequestIsCheckStatus == 2)
				{
					this.ShowMsg("当前项数据不可以驳回，操作终止！", false);
					return;
				}
				if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(array, -1, this.SignalrefuseMks.Text, null))
				{
					this.ShowMsg("申请已驳回！", true);
					this.LoadParameters();
					this.BindData();
					return;
				}
				this.ShowMsg("申请驳回失败，请再次尝试", false);
			}
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			this.ReBind(true);
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("StoreName", this.txtStoreName.Text);
			nameValueCollection.Add("RequestStartTime", this.RequestStartTime);
			nameValueCollection.Add("RequestEndTime", this.RequestEndTime);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
				{
					this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
				{
					this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
				{
					this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
					this.calendarStartDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.RequestStartTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
				{
					this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
					this.calendarEndDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.RequestEndTime));
				}
				this.txtStoreName.Text = this.StoreName;
				return;
			}
			this.StoreName = this.txtStoreName.Text;
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
		}

		private void BindData()
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.RequestTime = "";
			balanceDrawRequestQuery.CheckTime = "";
			balanceDrawRequestQuery.StoreName = this.StoreName;
			balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			balanceDrawRequestQuery.SortOrder = SortAction.Desc;
			balanceDrawRequestQuery.SortBy = "SerialID";
			balanceDrawRequestQuery.RequestEndTime = this.RequestEndTime;
			balanceDrawRequestQuery.RequestStartTime = this.RequestStartTime;
			balanceDrawRequestQuery.IsCheck = "";
			balanceDrawRequestQuery.UserId = "";
			string[] extendChecks = new string[]
			{
				"3"
			};
			Globals.EntityCoding(balanceDrawRequestQuery, true);
			DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(balanceDrawRequestQuery, extendChecks);
			this.reCommissions.DataSource = balanceDrawRequest.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
			System.Data.DataTable drawRequestNum = DistributorsBrower.GetDrawRequestNum(new int[]
			{
				0,
				1
			});
			if (drawRequestNum.Rows.Count > 0)
			{
				this.Frist.Text = "待发放(" + drawRequestNum.Rows[0]["num"].ToString() + ")";
			}
			this.Second.Text = "发放异常(" + this.pager.TotalRecords + ")";
		}

		protected void Frist_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("BalanceDrawApplyList.aspx");
		}
	}
}
