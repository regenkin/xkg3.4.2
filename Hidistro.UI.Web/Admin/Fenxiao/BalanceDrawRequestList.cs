using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class BalanceDrawRequestList : AdminPage
	{
		private string RequestStartTime = "";

		private string StoreName = "";

		private string RequestEndTime = "";

		private int lastDay;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected ucDateTimePicker txtRequestStartTime;

		protected ucDateTimePicker txtRequestEndTime;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.WebControls.Repeater reBalanceDrawRequest;

		protected Pager pager;

		protected BalanceDrawRequestList() : base("m05", "fxp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			if (this.txtRequestStartTime.SelectedDate.HasValue)
			{
				this.RequestStartTime = this.txtRequestStartTime.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.txtRequestEndTime.SelectedDate.HasValue)
			{
				this.RequestEndTime = this.txtRequestEndTime.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			this.lastDay = 0;
			this.ReBind(true);
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("StoreName", this.txtStoreName.Text);
			nameValueCollection.Add("RequestStartTime", this.RequestStartTime);
			nameValueCollection.Add("RequestEndTime", this.RequestEndTime);
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
				{
					this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
					this.txtRequestStartTime.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.RequestStartTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
				{
					this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
					this.txtRequestEndTime.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.RequestEndTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay == 30)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("#1CA47D");
					}
					else if (this.lastDay == 7)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#1CA47D");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					}
					else
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					}
				}
				this.txtStoreName.Text = this.StoreName;
				return;
			}
			this.StoreName = this.txtStoreName.Text;
			if (this.txtRequestStartTime.SelectedDate.HasValue)
			{
				this.RequestStartTime = this.txtRequestStartTime.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.txtRequestEndTime.SelectedDate.HasValue)
			{
				this.RequestEndTime = this.txtRequestEndTime.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
		}

		protected void Button1_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.RequestEndTime = now.ToString("yyyy-MM-dd");
			this.RequestStartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.ReBind(true);
		}

		protected void Button4_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.RequestEndTime = now.ToString("yyyy-MM-dd");
			this.RequestStartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
			this.lastDay = 30;
			this.ReBind(true);
		}

		private void BindData()
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.RequestTime = "";
			balanceDrawRequestQuery.CheckTime = "CheckTime";
			balanceDrawRequestQuery.RequestStartTime = this.RequestStartTime;
			balanceDrawRequestQuery.RequestEndTime = this.RequestEndTime;
			balanceDrawRequestQuery.StoreName = this.StoreName;
			balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			balanceDrawRequestQuery.SortOrder = SortAction.Desc;
			balanceDrawRequestQuery.SortBy = "RequestTime";
			balanceDrawRequestQuery.IsCheck = "2";
			Globals.EntityCoding(balanceDrawRequestQuery, true);
			DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(balanceDrawRequestQuery);
			this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
			this.reBalanceDrawRequest.DataBind();
			this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
		}
	}
}
