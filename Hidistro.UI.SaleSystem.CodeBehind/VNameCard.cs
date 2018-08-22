using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VNameCard : VshopTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VNameCard.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				System.Web.UI.HtmlControls.HtmlInputText control = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
				System.Web.UI.HtmlControls.HtmlInputText control2 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtPhone");
				System.Web.UI.HtmlControls.HtmlInputText control3 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtMicroSignal");
				control.SetWhenIsNotNull(currentMember.RealName);
				control2.SetWhenIsNotNull(currentMember.CellPhone);
				control3.SetWhenIsNotNull(currentMember.MicroSignal);
			}
			PageTitle.AddSiteNameTitle("我的名片");
		}
	}
}
