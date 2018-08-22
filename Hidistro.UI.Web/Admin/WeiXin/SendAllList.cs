using ASPNET.WebControls;
using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.Entities.WeiXin;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class SendAllList : AdminPage
	{
		protected string htmlMenuTitleAdd = string.Empty;

		protected string ArticleTitle = string.Empty;

		private int pageno;

		protected int recordcount;

		protected int articletype;

		private string title = string.Empty;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected SendAllList() : base("m06", "wxp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.pageno = Globals.RequestQueryNum("pageindex");
				if (this.pageno < 1)
				{
					this.pageno = 1;
				}
				this.BindData(this.pageno);
				if (this.pageno == 1)
				{
					WeiXinHelper.DelOldSendAllList();
				}
			}
		}

		private void BindData(int pageno)
		{
			SendAllQuery sendAllQuery = new SendAllQuery();
			sendAllQuery.SortBy = "ID";
			sendAllQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(sendAllQuery, true);
			sendAllQuery.PageIndex = pageno;
			sendAllQuery.PageSize = this.pager.PageSize;
			DbQueryResult sendAllRequest = WeiXinHelper.GetSendAllRequest(sendAllQuery, 0);
			this.rptList.DataSource = sendAllRequest.Data;
			this.rptList.DataBind();
			int totalRecords = sendAllRequest.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			this.recordcount = totalRecords;
			if (this.pager.TotalRecords <= this.pager.PageSize)
			{
				this.pager.Visible = false;
			}
		}
	}
}
