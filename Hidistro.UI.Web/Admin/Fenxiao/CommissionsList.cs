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
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class CommissionsList : AdminPage
	{
		protected int userid;

		protected string StartTime = "";

		protected string EndTime = "";

		protected decimal CurrentTotal;

		protected string CurrentStoreName = "";

		protected string StoreDisplay = "active";

		protected string FristDisplay = "";

		protected string SecondDisplay = "";

		protected string OtherDisplay = "";

		private string subLevel = "0";

		public int lastDay;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnQueryLogs;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.WebControls.Literal storeSum;

		protected System.Web.UI.WebControls.Literal fristSum;

		protected System.Web.UI.WebControls.Literal secondSum;

		protected System.Web.UI.WebControls.Literal OtherSum;

		protected System.Web.UI.WebControls.LinkButton Store;

		protected System.Web.UI.WebControls.LinkButton Frist;

		protected System.Web.UI.WebControls.LinkButton Second;

		protected System.Web.UI.WebControls.LinkButton Other;

		protected System.Web.UI.WebControls.Repeater reCommissions;

		protected System.Web.UI.WebControls.Repeater SubCommissions;

		protected System.Web.UI.WebControls.Repeater otherCommissions;

		protected Pager pager;

		protected CommissionsList() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
			{
				this.BindData();
				return;
			}
			this.Page.Response.Redirect("DistributorList.aspx");
		}

		protected string getNextName(string StoreName, string uid, string rid, string rpath)
		{
			string result = "店铺销售";
			if (uid == rid || string.IsNullOrEmpty(rpath))
			{
				result = "店铺销售";
			}
			else if (uid == rpath)
			{
				result = StoreName + "（下一级）";
			}
			else if (rpath.Contains("|"))
			{
				string[] array = rpath.Split(new char[]
				{
					'|'
				});
				if (array[0] == uid)
				{
					result = StoreName + "（下二级）";
				}
				if (array[1] == uid)
				{
					result = StoreName + "（下一级）";
				}
			}
			else
			{
				result = rpath;
			}
			return result;
		}

		private void BindData()
		{
			CommissionsQuery commissionsQuery = new CommissionsQuery();
			commissionsQuery.UserId = int.Parse(this.Page.Request.QueryString["UserId"]);
			commissionsQuery.EndTime = this.EndTime;
			commissionsQuery.StartTime = this.StartTime;
			commissionsQuery.PageIndex = this.pager.PageIndex;
			commissionsQuery.PageSize = this.pager.PageSize;
			commissionsQuery.SortOrder = SortAction.Desc;
			commissionsQuery.SortBy = "CommId";
			Globals.EntityCoding(commissionsQuery, true);
			DbQueryResult dbQueryResult;
			if (this.subLevel == "0")
			{
				dbQueryResult = VShopHelper.GetCommissionsWithStoreName(commissionsQuery, "5");
				this.reCommissions.DataSource = dbQueryResult.Data;
				this.reCommissions.DataBind();
			}
			else if (this.subLevel == "45")
			{
				dbQueryResult = VShopHelper.GetCommissionsWithStoreName(commissionsQuery, "4");
				this.otherCommissions.DataSource = dbQueryResult.Data;
				this.otherCommissions.DataBind();
			}
			else
			{
				dbQueryResult = VShopHelper.GetSubDistributorsContribute(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex, this.userid, int.Parse(this.subLevel));
				this.SubCommissions.DataSource = dbQueryResult.Data;
				this.SubCommissions.DataBind();
			}
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
			System.DateTime fromdatetime;
			if (!System.DateTime.TryParse(this.StartTime, out fromdatetime))
			{
				fromdatetime = System.DateTime.Parse("2015-01-01");
			}
			this.CurrentTotal = DistributorsBrower.GetUserCommissions(commissionsQuery.UserId, fromdatetime, this.EndTime, null, null, "");
			decimal userCommissions = DistributorsBrower.GetUserCommissions(commissionsQuery.UserId, fromdatetime, this.EndTime, null, null, "0");
			decimal userCommissions2 = DistributorsBrower.GetUserCommissions(commissionsQuery.UserId, fromdatetime, this.EndTime, null, null, "1");
			decimal userCommissions3 = DistributorsBrower.GetUserCommissions(commissionsQuery.UserId, fromdatetime, this.EndTime, null, null, "2");
			decimal num = this.CurrentTotal - userCommissions - userCommissions2 - userCommissions3;
			this.storeSum.Text = userCommissions.ToString("f2");
			this.fristSum.Text = userCommissions2.ToString("f2");
			this.secondSum.Text = userCommissions3.ToString("f2");
			this.OtherSum.Text = num.ToString("f2");
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(commissionsQuery.UserId);
			this.CurrentStoreName = userIdDistributors.StoreName;
		}

		protected void Button1_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.ReBind(true);
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["subLevel"]))
				{
					this.subLevel = base.Server.UrlDecode(this.Page.Request.QueryString["subLevel"]);
				}
				else
				{
					this.subLevel = "0";
				}
				if (this.subLevel == "1")
				{
					this.FristDisplay = "active";
					this.SecondDisplay = "";
					this.OtherDisplay = "";
					this.StoreDisplay = "";
				}
				else if (this.subLevel == "2")
				{
					this.FristDisplay = "";
					this.SecondDisplay = "active";
					this.StoreDisplay = "";
					this.OtherDisplay = "";
					this.subLevel = "2";
				}
				else if (this.subLevel == "45")
				{
					this.FristDisplay = "";
					this.SecondDisplay = "";
					this.StoreDisplay = "";
					this.OtherDisplay = "active";
				}
				else
				{
					this.FristDisplay = "";
					this.SecondDisplay = "";
					this.OtherDisplay = "";
					this.StoreDisplay = "active";
					this.subLevel = "0";
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

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserId", this.Page.Request.QueryString["UserId"]);
			nameValueCollection.Add("StartTime", this.StartTime);
			nameValueCollection.Add("EndTime", this.EndTime);
			nameValueCollection.Add("subLevel", this.subLevel);
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
			this.ReBind(true);
		}

		protected void Second_Click(object sender, System.EventArgs e)
		{
			this.StoreDisplay = "";
			this.FristDisplay = "";
			this.SecondDisplay = "active";
			this.OtherDisplay = "";
			this.subLevel = "2";
			this.ReBind(true);
		}

		protected void Other_Click(object sender, System.EventArgs e)
		{
			this.StoreDisplay = "";
			this.FristDisplay = "";
			this.SecondDisplay = "";
			this.OtherDisplay = "active";
			this.subLevel = "45";
			this.ReBind(true);
		}

		protected void Store_Click(object sender, System.EventArgs e)
		{
			this.StoreDisplay = "active";
			this.FristDisplay = "";
			this.SecondDisplay = "";
			this.OtherDisplay = "";
			this.subLevel = "0";
			this.ReBind(true);
		}

		protected void Frist_Click(object sender, System.EventArgs e)
		{
			this.FristDisplay = "active";
			this.SecondDisplay = "";
			this.StoreDisplay = "";
			this.OtherDisplay = "";
			this.subLevel = "1";
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
			this.lastDay = 0;
			this.ReBind(true);
		}
	}
}
