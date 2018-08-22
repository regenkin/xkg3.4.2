using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class SubStoreCommissions : AdminPage
	{
		private int userid;

		protected string StartTime = "";

		protected string EndTime = "";

		private int ReferralUserId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Repeater reCommissions;

		protected Pager pager;

		protected SubStoreCommissions() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (int.TryParse(this.Page.Request.QueryString["ReferralUserId"], out this.ReferralUserId) && int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
				{
					this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
				{
					this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
				}
				this.BindData();
				return;
			}
			base.GotoResourceNotFound();
		}

		private void BindData()
		{
			CommissionsQuery commissionsQuery = new CommissionsQuery();
			commissionsQuery.UserId = this.userid;
			commissionsQuery.EndTime = this.EndTime;
			commissionsQuery.StartTime = this.StartTime;
			commissionsQuery.PageIndex = this.pager.PageIndex;
			commissionsQuery.ReferralUserId = this.ReferralUserId;
			commissionsQuery.PageSize = this.pager.PageSize;
			commissionsQuery.SortOrder = SortAction.Desc;
			commissionsQuery.SortBy = "CommId";
			Globals.EntityCoding(commissionsQuery, true);
			DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(commissionsQuery, "");
			this.reCommissions.DataSource = commissionsWithStoreName.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
		}
	}
}
