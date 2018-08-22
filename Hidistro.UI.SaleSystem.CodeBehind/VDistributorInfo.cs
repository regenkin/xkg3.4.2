using Hidistro.Core;
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
	public class VDistributorInfo : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litBackImg;

		private System.Web.UI.HtmlControls.HtmlInputText txtstorename;

		private System.Web.UI.HtmlControls.HtmlInputText txtStoreTel;

		private System.Web.UI.HtmlControls.HtmlTextArea txtdescription;

		private System.Web.UI.HtmlControls.HtmlInputText txtacctount;

		private System.Web.UI.HtmlControls.HtmlImage imglogo;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdlogo;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("店铺消息");
			this.imglogo = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imglogo");
			this.hdlogo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdlogo");
			this.txtstorename = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtstorename");
			this.txtdescription = (System.Web.UI.HtmlControls.HtmlTextArea)this.FindControl("txtdescription");
			this.txtacctount = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtacctount");
			this.txtStoreTel = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtStoreTel");
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
			if (userIdDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				if (userIdDistributors != null)
				{
					if (!string.IsNullOrEmpty(userIdDistributors.Logo))
					{
						this.imglogo.Src = userIdDistributors.Logo;
					}
					this.hdlogo.Value = userIdDistributors.Logo;
					this.txtstorename.Value = userIdDistributors.StoreName;
					this.txtdescription.Value = userIdDistributors.StoreDescription;
					this.txtacctount.Value = userIdDistributors.RequestAccount;
					this.txtStoreTel.Value = currentMember.CellPhone;
				}
			}
		}
	}
}
