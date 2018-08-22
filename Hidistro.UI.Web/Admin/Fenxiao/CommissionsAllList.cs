using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class CommissionsAllList : AdminPage
	{
		private string OrderId = "";

		private string StoreName = "";

		private string StartTime = "";

		private string EndTime = "";

		protected decimal CurrentTotal;

		protected string CurrentStoreName = "";

		public int lastDay;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected System.Web.UI.WebControls.TextBox txtOrderId;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnQueryLogs;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.WebControls.Repeater reCommissions;

		protected Pager pager;

		protected CommissionsAllList() : base("m05", "fxp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			CommissionsQuery commissionsQuery = new CommissionsQuery();
			commissionsQuery.StoreName = this.StoreName;
			commissionsQuery.OrderNum = this.OrderId;
			commissionsQuery.EndTime = this.EndTime;
			commissionsQuery.StartTime = this.StartTime;
			commissionsQuery.PageIndex = this.pager.PageIndex;
			commissionsQuery.PageSize = this.pager.PageSize;
			commissionsQuery.SortOrder = SortAction.Desc;
			commissionsQuery.SortBy = "CommId";
			Globals.EntityCoding(commissionsQuery, true);
			DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(commissionsQuery, "0");
			this.reCommissions.DataSource = commissionsWithStoreName.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
			System.DateTime fromdatetime;
			if (!System.DateTime.TryParse(this.StartTime, out fromdatetime))
			{
				fromdatetime = System.DateTime.Parse("2015-01-01");
			}
			this.CurrentTotal = DistributorsBrower.GetUserCommissions(0, fromdatetime, this.EndTime, this.StoreName, commissionsQuery.OrderNum, "");
		}

		protected void rptypelist_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (string.IsNullOrEmpty(this.StoreName) && (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem))
			{
				System.Web.UI.WebControls.Repeater repeater = e.Item.FindControl("reCommissionsChild") as System.Web.UI.WebControls.Repeater;
				System.Data.DataRowView dataRowView = (System.Data.DataRowView)e.Item.DataItem;
				int num = System.Convert.ToInt32(dataRowView.Row["ReferralUserId"]);
				string text = dataRowView.Row["OrderId"].ToString();
				if (num > 0 && text != "")
				{
					CommissionsQuery commissionsQuery = new CommissionsQuery();
					commissionsQuery.ReferralUserId = num;
					commissionsQuery.StoreName = "";
					commissionsQuery.OrderNum = text;
					commissionsQuery.EndTime = "";
					commissionsQuery.StartTime = "";
					commissionsQuery.PageIndex = 1;
					commissionsQuery.PageSize = 1000;
					commissionsQuery.SortOrder = SortAction.Desc;
					commissionsQuery.SortBy = "CommId";
					Globals.EntityCoding(commissionsQuery, true);
					DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(commissionsQuery, "3");
					repeater.DataSource = commissionsWithStoreName.Data;
					repeater.DataBind();
				}
			}
		}

		protected string getNextName(string uid, string rid, string rpath)
		{
			string result = "原上级店铺";
			if (uid == rid || string.IsNullOrEmpty(rpath))
			{
				result = "成交店铺";
			}
			else if (uid == rpath)
			{
				result = "上一级分销商";
			}
			else if (rpath.Contains("|"))
			{
				string[] array = rpath.Split(new char[]
				{
					'|'
				});
				if (array[0] == uid)
				{
					result = "上二级分销商";
				}
				if (array[1] == uid)
				{
					result = "上一级分销商";
				}
			}
			return result;
		}

		protected void Button1_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.OrderId = this.txtOrderId.Text;
			this.StoreName = this.txtStoreName.Text;
			this.ReBind(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
				this.txtStoreName.Text = this.StoreName;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
				{
					this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
				}
				this.txtOrderId.Text = this.OrderId;
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

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.OrderId);
			nameValueCollection.Add("StoreName", this.StoreName);
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

		protected void Button4_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
			this.lastDay = 30;
			this.OrderId = this.txtOrderId.Text;
			this.StoreName = this.txtStoreName.Text;
			this.ReBind(true);
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
			this.OrderId = this.txtOrderId.Text;
			this.StoreName = this.txtStoreName.Text;
			this.lastDay = 0;
			this.ReBind(true);
		}
	}
}
