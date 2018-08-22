using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProgressCheck : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptOrderReturns;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProgressCheck.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string title = "历史记录";
			int num = Globals.RequestQueryNum("type");
			if (num != 1)
			{
				num = 0;
				title = "售后列表";
			}
			PageTitle.AddSiteNameTitle(title);
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.rptOrderReturns = (VshopTemplatedRepeater)this.FindControl("rptOrderReturns");
			this.rptOrderReturns.DataSource = ShoppingProcessor.GetOrderReturnTable(currentMember.UserId, "", num);
			this.rptOrderReturns.DataBind();
		}
	}
}
