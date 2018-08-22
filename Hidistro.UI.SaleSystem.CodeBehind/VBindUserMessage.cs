using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VBindUserMessage : VshopTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VBindUserMessage.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (!string.IsNullOrEmpty(currentMember.UserBindName))
			{
				this.Page.Response.Redirect("/vshop/MemberCenter.aspx", true);
			}
			PageTitle.AddSiteNameTitle("用户绑定帐号");
		}
	}
}
