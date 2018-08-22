using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VBalanceDrawRequesList : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater vshopbalancedraw;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BalanceDrawRequesList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(true);
			if (currentDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				this.vshopbalancedraw = (VshopTemplatedRepeater)this.FindControl("vshopbalancedraw");
				BalanceDrawRequestQuery query = new BalanceDrawRequestQuery
				{
					PageIndex = 1,
					PageSize = 100000,
					UserId = currentDistributors.UserId.ToString(),
					SortBy = "SerialID",
					RequestTime = "",
					RequestStartTime = "",
					RequestEndTime = "",
					CheckTime = "",
					IsCheck = "",
					SortOrder = SortAction.Desc
				};
				DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(query, null);
				if (balanceDrawRequest.TotalRecords > 0)
				{
					this.vshopbalancedraw.DataSource = balanceDrawRequest.Data;
					this.vshopbalancedraw.DataBind();
				}
			}
		}
	}
}
