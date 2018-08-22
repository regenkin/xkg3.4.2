using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VCommissionDetails : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater vshopcommssion;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VCommissionDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(), true);
			if (currentDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				this.vshopcommssion = (VshopTemplatedRepeater)this.FindControl("vshopcommssion");
				CommissionsQuery commissionsQuery = new CommissionsQuery();
				commissionsQuery.StartTime = (commissionsQuery.EndTime = "");
				commissionsQuery.PageIndex = 1;
				commissionsQuery.PageSize = 100000;
				commissionsQuery.UserId = Globals.GetCurrentMemberUserId();
				DbQueryResult commissions = DistributorsBrower.GetCommissions(commissionsQuery);
				if (commissions.TotalRecords > 0)
				{
					this.vshopcommssion.DataSource = commissions.Data;
					this.vshopcommssion.DataBind();
				}
			}
		}
	}
}
