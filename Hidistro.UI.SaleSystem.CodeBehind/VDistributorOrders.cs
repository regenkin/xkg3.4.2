using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VDistributorOrders : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater vshoporders;

		private System.Web.UI.WebControls.Literal litfinishnum;

		private System.Web.UI.WebControls.Literal litallnum;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorOrders.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
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
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
			if (userIdDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				this.vshoporders = (VshopTemplatedRepeater)this.FindControl("vshoporders");
				this.litfinishnum = (System.Web.UI.WebControls.Literal)this.FindControl("litfinishnum");
				this.litallnum = (System.Web.UI.WebControls.Literal)this.FindControl("litallnum");
				PageTitle.AddSiteNameTitle("店铺订单");
				int num = 0;
				int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("status"), out num);
				orderQuery.UserId = new int?(Globals.GetCurrentMemberUserId());
				if (num != 5)
				{
					orderQuery.Status = OrderStatus.Finished;
					this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
					orderQuery.Status = OrderStatus.All;
					this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
				}
				else
				{
					this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
					orderQuery.Status = OrderStatus.Finished;
					this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
				}
				this.vshoporders.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.vshoporders_ItemDataBound);
				DataSet distributorOrder = DistributorsBrower.GetDistributorOrder(orderQuery);
				this.vshoporders.DataSource = distributorOrder;
				this.vshoporders.DataBind();
			}
		}

		private void vshoporders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			DataView dataView = (DataView)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderItems");
			DataTable dataTable = dataView.ToTable();
			object obj = dataTable.Compute("sum(ItemAdjustedCommssion)", "OrderItemsStatus<>9 and OrderItemsStatus<>10 and IsAdminModify=0");
			decimal d = (obj != System.DBNull.Value) ? ((decimal)obj) : 0m;
			obj = dataTable.Compute("sum(itemsCommission)", "OrderItemsStatus<>9 and OrderItemsStatus<>10");
			decimal d2 = (obj != System.DBNull.Value) ? ((decimal)obj) : 0m;
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litCommission");
			literal.Text = (d2 - d).ToString("F2");
		}
	}
}
