using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberComment : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptOrderItemList;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberComment.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
			PageTitle.AddSiteNameTitle("评价列表");
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 10;
			}
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.PageIndex = pageIndex;
			orderQuery.PageSize = pageSize;
			orderQuery.SortBy = "Id";
			orderQuery.SortOrder = SortAction.Desc;
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			int orderItemsStatus = 5;
			this.rptOrderItemList = (VshopTemplatedRepeater)this.FindControl("rptOrderItemList");
			DbQueryResult orderMemberComment = ProductBrowser.GetOrderMemberComment(orderQuery, currentMemberUserId, orderItemsStatus);
			this.rptOrderItemList.DataSource = orderMemberComment.Data;
			this.rptOrderItemList.DataBind();
			this.txtTotal.SetWhenIsNotNull(orderMemberComment.TotalRecords.ToString());
		}
	}
}
