using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VDistributorDescription : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litApplicationDescription;

		private System.Web.UI.HtmlControls.HtmlInputHidden litRMsg;

		private System.Web.UI.HtmlControls.HtmlInputHidden litIsEnable;

		protected string IsEnable = "0";

		protected string RMsg = "您未达到开店条件，最低消费金额需要达到{0}元才能申请,你当前累计消费金额为{1}元";

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VDistributorDesciption.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litIsEnable = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litIsEnable");
			this.litRMsg = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litRMsg");
			this.litApplicationDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litApplicationDescription");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.litApplicationDescription.Text = System.Web.HttpUtility.HtmlDecode(masterSettings.DistributorDescription);
			PageTitle.AddSiteNameTitle("分销说明信息");
			this.Page.Session["stylestatus"] = "2";
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
			if (userIdDistributors != null)
			{
				this.IsEnable = "1";
			}
			else
			{
				int finishedOrderMoney = masterSettings.FinishedOrderMoney;
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				this.RMsg = string.Format(this.RMsg, finishedOrderMoney, currentMember.Expenditure);
				if (currentMember.Expenditure >= finishedOrderMoney)
				{
					this.IsEnable = "2";
				}
			}
			this.litIsEnable.Value = this.IsEnable;
			this.litRMsg.Value = this.RMsg;
		}
	}
}
