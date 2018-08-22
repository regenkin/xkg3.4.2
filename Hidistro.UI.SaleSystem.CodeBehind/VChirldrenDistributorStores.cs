using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VChirldrenDistributorStores : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rpdistributor;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddTotal;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VChirldrenDistributorStores.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("下级分销商");
			this.rpdistributor = (VshopTemplatedRepeater)this.FindControl("rpdistributor");
			this.hiddTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddTotal");
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.PageIndex = 0;
			distributorsQuery.PageSize = 10;
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(), true);
			if (currentDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				distributorsQuery.GradeId = 3;
				int userId = 0;
				if (int.TryParse(this.Page.Request.QueryString["UserId"], out userId))
				{
					distributorsQuery.UserId = userId;
				}
				distributorsQuery.ReferralPath = userId.ToString();
				int num = 0;
				DataTable threeDistributors = DistributorsBrower.GetThreeDistributors(distributorsQuery, out num);
				this.hiddTotal.Value = num.ToString();
				this.rpdistributor.DataSource = threeDistributors;
				this.rpdistributor.DataBind();
			}
		}
	}
}
