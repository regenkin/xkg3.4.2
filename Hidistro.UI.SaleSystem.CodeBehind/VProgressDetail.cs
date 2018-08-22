using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProgressDetail : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptOrderReturns;

		private VshopTemplatedRepeater rptOrderReturnsLog;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProgressDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("进度明细");
			if (this.Page.Request.QueryString["ReturnsId"] != null)
			{
				OrderQuery orderQuery = new OrderQuery();
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				this.rptOrderReturns = (VshopTemplatedRepeater)this.FindControl("rptOrderReturns");
				this.rptOrderReturnsLog = (VshopTemplatedRepeater)this.FindControl("rptOrderReturnsLog");
				this.rptOrderReturns.DataSource = ShoppingProcessor.GetOrderReturnTable(currentMember.UserId, this.Page.Request.QueryString["ReturnsId"], 10);
				this.rptOrderReturns.DataBind();
			}
		}
	}
}
