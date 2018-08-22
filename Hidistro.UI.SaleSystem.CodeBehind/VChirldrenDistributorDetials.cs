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
	public class VChirldrenDistributorDetials : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater vshoporders1;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal1;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum1;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ChirldrenDistributorDetials.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotal1 = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum1 = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
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
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["distributorId"], out value))
			{
				orderQuery.ReferralUserId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["ReferralId"], out value2))
			{
				orderQuery.UserId = new int?(value2);
			}
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
				this.vshoporders1 = (VshopTemplatedRepeater)this.FindControl("vshoporders");
				PageTitle.AddSiteNameTitle("店铺订单");
				orderQuery.UserId = new int?(Globals.GetCurrentMemberUserId());
				orderQuery.Status = OrderStatus.Finished;
				this.vshoporders1.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.vshoporders1_ItemDataBound);
				DataSet distributorOrderByDetials = DistributorsBrower.GetDistributorOrderByDetials(orderQuery);
				this.vshoporders1.DataSource = distributorOrderByDetials;
				this.vshoporders1.DataBind();
			}
		}

		private void vshoporders1_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
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
