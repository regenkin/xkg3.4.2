using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VDistributorCommission : VMemberTemplatedWebControl
	{
		private FormatedMoneyLabel lbltotalcommission;

		private FormatedMoneyLabel lblsurpluscommission;

		private FormatedMoneyLabel lblAlreadycommission;

		private FormatedMoneyLabel lblcommission;

		private FormatedMoneyLabel lblsaleprice;

		private FormatedMoneyLabel lbltwocommission;

		private FormatedMoneyLabel lblthreecommission;

		private FormatedMoneyLabel lbltwosaleprice;

		private FormatedMoneyLabel lblthreesaleprice;

		private System.Web.UI.WebControls.Literal litMsg;

		private System.Web.UI.WebControls.HyperLink hyrequest;

		private System.Web.UI.WebControls.Panel paneltwo;

		private System.Web.UI.WebControls.Panel panelthree;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorCommission.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
			if (userIdDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				this.lbltotalcommission = (FormatedMoneyLabel)this.FindControl("lbltotalcommission");
				this.lblsurpluscommission = (FormatedMoneyLabel)this.FindControl("lblsurpluscommission");
				this.lblAlreadycommission = (FormatedMoneyLabel)this.FindControl("lblAlreadycommission");
				this.lblcommission = (FormatedMoneyLabel)this.FindControl("lblcommission");
				this.lbltwocommission = (FormatedMoneyLabel)this.FindControl("lbltwocommission");
				this.lblthreecommission = (FormatedMoneyLabel)this.FindControl("lblthreecommission");
				this.lblsaleprice = (FormatedMoneyLabel)this.FindControl("lblsaleprice");
				this.lbltwosaleprice = (FormatedMoneyLabel)this.FindControl("lbltwosaleprice");
				this.lblthreesaleprice = (FormatedMoneyLabel)this.FindControl("lblthreesaleprice");
				this.paneltwo = (System.Web.UI.WebControls.Panel)this.FindControl("paneltwo");
				this.panelthree = (System.Web.UI.WebControls.Panel)this.FindControl("panelthree");
				this.litMsg = (System.Web.UI.WebControls.Literal)this.FindControl("litMsg");
				this.hyrequest = (System.Web.UI.WebControls.HyperLink)this.FindControl("hyrequest");
				PageTitle.AddSiteNameTitle("我的佣金");
				DataTable dataTable = DistributorsBrower.GetCurrentDistributorsCommosion(userIdDistributors.UserId);
				if (userIdDistributors != null && userIdDistributors.UserId > 0)
				{
					this.lblsurpluscommission.Money = userIdDistributors.ReferralBlance;
					this.lblAlreadycommission.Money = userIdDistributors.ReferralRequestBalance;
					if (userIdDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
					{
						this.paneltwo.Visible = false;
					}
					else if (userIdDistributors.DistributorGradeId == DistributorGrade.ThreeDistributor)
					{
						this.paneltwo.Visible = false;
						this.panelthree.Visible = false;
					}
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						this.lblcommission.Money = dataTable.Rows[0]["CommTotal"];
						this.lblsaleprice.Money = dataTable.Rows[0]["OrderTotal"];
					}
					if (userIdDistributors.DistributorGradeId == DistributorGrade.OneDistributor)
					{
						dataTable = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.TowDistributor);
						if (dataTable != null && dataTable.Rows.Count > 0)
						{
							this.lbltwocommission.Money = dataTable.Rows[0]["CommTotal"];
							this.lbltwosaleprice.Money = dataTable.Rows[0]["OrderTotal"];
						}
						dataTable = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.ThreeDistributor);
						if (dataTable != null && dataTable.Rows.Count > 0)
						{
							this.lblthreecommission.Money = dataTable.Rows[0]["CommTotal"];
							this.lblthreesaleprice.Money = dataTable.Rows[0]["OrderTotal"];
						}
					}
					if (userIdDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
					{
						dataTable = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.ThreeDistributor);
						if (dataTable != null && dataTable.Rows.Count > 0)
						{
							this.lblthreecommission.Money = dataTable.Rows[0]["CommTotal"];
							this.lblthreesaleprice.Money = dataTable.Rows[0]["OrderTotal"];
						}
					}
					this.lbltotalcommission.Money = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;
					if (DistributorsBrower.IsExitsCommionsRequest())
					{
						this.hyrequest.Text = "<span class='glyphicon glyphicon-credit-card'></span>您的申请正在审核当中……";
						this.hyrequest.Enabled = false;
					}
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					if (string.IsNullOrEmpty(currentMember.RealName) || string.IsNullOrEmpty(currentMember.CellPhone))
					{
						this.hyrequest.NavigateUrl = "UserInfo.aspx?edit=true&&returnUrl=" + Globals.UrlEncode(Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Vshop/RequestCommissions.aspx");
					}
				}
			}
		}
	}
}
