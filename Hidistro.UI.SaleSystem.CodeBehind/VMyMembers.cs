using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyMembers : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserId;

		private System.Web.UI.WebControls.Literal litMysubMember;

		private System.Web.UI.WebControls.Literal litMysubFirst;

		private System.Web.UI.WebControls.Literal litMysubSecond;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPageIndex;

		private VshopTemplatedRepeater rpMyMemberList;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyMembers.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("店铺会员");
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
			if (userIdDistributors == null)
			{
				this.Context.Response.Redirect("/default.aspx");
				this.Context.Response.End();
			}
			this.litUserId = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId");
			this.litUserId.Text = userIdDistributors.UserId.ToString();
			this.litMysubMember = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubMember");
			this.litMysubFirst = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubFirst");
			this.litMysubSecond = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubSecond");
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
			this.rpMyMemberList = (VshopTemplatedRepeater)this.FindControl("rpMyMemberList");
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
			this.hiddTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddTotal");
			this.hiddPageIndex = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddPageIndex");
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				int referralUserId = int.Parse(this.Page.Request.QueryString["UserID"]);
				int num = 0;
				DataTable membersByUserId = MemberProcessor.GetMembersByUserId(referralUserId, pageIndex, pageSize, out num);
				this.hiddTotal.Value = num.ToString();
				this.hiddPageIndex.Value = pageIndex.ToString();
				this.rpMyMemberList.DataSource = membersByUserId;
				this.rpMyMemberList.DataBind();
			}
		}
	}
}
