using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VNotice : VMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-vNotice.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = Globals.RequestQueryNum("type");
			int num2 = Globals.RequestQueryNum("readtype");
			System.Web.UI.HtmlControls.HtmlInputHidden htmlInputHidden = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidType");
			System.Web.UI.HtmlControls.HtmlInputHidden htmlInputHidden2 = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidReadtype");
			string title = "公告";
			if (num == 1)
			{
				htmlInputHidden.Value = "1";
				title = "消息";
			}
			if (num2 == 1)
			{
				htmlInputHidden2.Value = "1";
			}
			PageTitle.AddSiteNameTitle(title);
		}
	}
}
