using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberOrders : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptOrders;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrders.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
			PageTitle.AddSiteNameTitle("会员订单");
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
			int num = 0;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("status"), out num);
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.PageIndex = pageIndex;
			orderQuery.PageSize = pageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			if (num == 1)
			{
				orderQuery.Status = OrderStatus.WaitBuyerPay;
			}
			else if (num == 2)
			{
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
			}
			else if (num == 3)
			{
				orderQuery.Status = OrderStatus.SellerAlreadySent;
			}
			else if (num == 4)
			{
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
			}
			this.rptOrders = (VshopTemplatedRepeater)this.FindControl("rptOrders");
			DbQueryResult userOrderByPage = MemberProcessor.GetUserOrderByPage(Globals.GetCurrentMemberUserId(), orderQuery);
			this.txtTotal.SetWhenIsNotNull(userOrderByPage.TotalRecords.ToString());
			this.rptOrders.DataSource = userOrderByPage.Data;
			this.rptOrders.DataBind();
		}
	}
}
