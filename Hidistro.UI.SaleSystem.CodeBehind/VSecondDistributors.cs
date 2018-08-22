using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
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
	public class VSecondDistributors : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Panel twodistributor;

		private System.Web.UI.WebControls.Panel onedistributor;

		private VshopTemplatedRepeater rpdistributor;

		private System.Web.UI.WebControls.Literal litUserId;

		private System.Web.UI.WebControls.Literal litMysubMember;

		private System.Web.UI.WebControls.Literal litMysubFirst;

		private System.Web.UI.WebControls.Literal litMysubSecond;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPageIndex;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VSecondDistributors.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("下级分销商");
			this.litMysubMember = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubMember");
			this.litMysubFirst = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubFirst");
			this.litMysubSecond = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubSecond");
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
			this.litUserId = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId");
			this.litUserId.Text = userIdDistributors.UserId.ToString();
			DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
			if (distributorsSubStoreNum != null || distributorsSubStoreNum.Rows.Count > 0)
			{
				this.litMysubMember.Text = distributorsSubStoreNum.Rows[0]["memberCount"].ToString();
				this.litMysubFirst.Text = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
				this.litMysubSecond.Text = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
			}
			else
			{
				this.litMysubMember.Text = "0";
				this.litMysubFirst.Text = "0";
				this.litMysubSecond.Text = "0";
			}
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
				distributorsQuery.GradeId = 2;
				int gradeId = 0;
				if (int.TryParse(this.Page.Request.QueryString["gradeId"], out gradeId))
				{
					distributorsQuery.GradeId = gradeId;
				}
				distributorsQuery.ReferralPath = currentDistributors.UserId.ToString();
				distributorsQuery.UserId = currentDistributors.UserId;
				int num = 0;
				DataTable downDistributors = DistributorsBrower.GetDownDistributors(distributorsQuery, out num);
				this.hiddTotal.Value = num.ToString();
				this.rpdistributor.DataSource = downDistributors;
				this.rpdistributor.DataBind();
			}
		}
	}
}
