using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberOrderReturn : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptOrders;

		private DataView ItemsDt = new DataView();

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrderReturn.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("退换货");
			OrderQuery query = new OrderQuery();
			this.rptOrders = (VshopTemplatedRepeater)this.FindControl("rptOrders");
			this.rptOrders.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptOrders_ItemDataBound);
			DataSet userOrderReturn = MemberProcessor.GetUserOrderReturn(Globals.GetCurrentMemberUserId(), query);
			this.ItemsDt = userOrderReturn.Tables[1].DefaultView;
			this.rptOrders.DataSource = userOrderReturn;
			this.rptOrders.DataBind();
		}

		private void rptOrders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litStyle");
				string str = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderId");
				this.ItemsDt.RowFilter = " OrderId='" + str + "'";
				if (this.ItemsDt.Count == 0)
				{
					literal.Text = "style=\"display:none;\"";
				}
			}
		}
	}
}
