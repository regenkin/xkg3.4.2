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
	public class VRequestCommissions : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litmaxmoney;

		private System.Web.UI.HtmlControls.HtmlInputText txtaccount;

		private System.Web.UI.HtmlControls.HtmlInputText txtAccountName;

		private System.Web.UI.HtmlControls.HtmlInputText txtmoney;

		private System.Web.UI.HtmlControls.HtmlInputText txtmoneyweixin;

		private System.Web.UI.HtmlControls.HtmlAnchor requestcommission;

		private System.Web.UI.HtmlControls.HtmlAnchor requestcommission1;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidmoney;

		private System.Web.UI.HtmlControls.HtmlSelect accoutType;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RequestCommissions.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			DistributorsInfo nowCurrentDistributors = DistributorsBrower.GetNowCurrentDistributors(Globals.GetCurrentMemberUserId());
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (nowCurrentDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				PageTitle.AddSiteNameTitle("申请提现");
				this.accoutType = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("accoutType");
				this.litmaxmoney = (System.Web.UI.WebControls.Literal)this.FindControl("litmaxmoney");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				if (masterSettings.DrawPayType.Contains("0"))
				{
					this.accoutType.Items.Add(new System.Web.UI.WebControls.ListItem("微信钱包", "0"));
				}
				if (masterSettings.DrawPayType.Contains("1"))
				{
					this.accoutType.Items.Add(new System.Web.UI.WebControls.ListItem("支付宝", "1"));
				}
				if (masterSettings.DrawPayType.Contains("2"))
				{
					this.accoutType.Items.Add(new System.Web.UI.WebControls.ListItem("线下转帐", "2"));
				}
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("litAlipayBtn");
				literal.Text = "display:none";
				if (masterSettings.DrawPayType.Contains("3"))
				{
					literal.Text = "";
				}
				this.txtAccountName = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtAccountName");
				this.txtaccount = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtaccount");
				this.txtmoney = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtmoney");
				this.txtmoneyweixin = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtmoneyweixin");
				this.hidmoney = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidmoney");
				this.requestcommission = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("requestcommission");
				this.requestcommission1 = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("requestcommission1");
				this.txtaccount.Value = nowCurrentDistributors.RequestAccount;
				this.txtAccountName.Value = currentMember.RealName;
				decimal referralBlance = nowCurrentDistributors.ReferralBlance;
				this.litmaxmoney.Text = referralBlance.ToString("F2");
				decimal num = 0m;
				if (decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out num) && num > 0m)
				{
					this.txtmoney.Attributes["placeholder"] = "请输入大于等于" + num + "元的金额";
					this.txtmoneyweixin.Attributes["placeholder"] = "最低提现金额" + num + "元的金额";
					this.hidmoney.Value = num.ToString();
				}
				if (DistributorsBrower.IsExitsCommionsRequest())
				{
					this.requestcommission.Disabled = true;
					this.requestcommission.InnerText = "您的申请正在审核当中";
					this.requestcommission1.Disabled = true;
					this.requestcommission1.InnerText = "您的申请正在审核当中";
					literal.Text = "display:none";
					System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)this.FindControl("litWechatBtn");
					literal2.Text = "display:none";
				}
				else
				{
					this.requestcommission.Attributes.Add("onclick", "return RequestCommissions(3)");
					this.requestcommission1.Attributes.Add("onclick", "return RequestCommissions(1)");
				}
			}
		}
	}
}
