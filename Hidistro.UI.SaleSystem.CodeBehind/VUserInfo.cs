using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VUserInfo : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlImage imglogo;

		private System.Web.UI.HtmlControls.HtmlContainerControl Nickname;

		private System.Web.UI.HtmlControls.HtmlControl WeixinHead;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				System.Web.UI.WebControls.Literal control = (System.Web.UI.WebControls.Literal)this.FindControl("txtUserBindName");
				System.Web.UI.HtmlControls.HtmlInputText control2 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
				System.Web.UI.HtmlControls.HtmlInputText control3 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtPhone");
				System.Web.UI.HtmlControls.HtmlInputText control4 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtEmail");
				System.Web.UI.HtmlControls.HtmlInputHidden control5 = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtUserName");
				System.Web.UI.HtmlControls.HtmlInputText control6 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtCardID");
				this.imglogo = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imglogo");
				this.Nickname = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("Nickname");
				this.WeixinHead = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("WeixinHead");
				if (!string.IsNullOrEmpty(currentMember.UserHead))
				{
					this.imglogo.Src = currentMember.UserHead;
				}
				this.Nickname.InnerText = currentMember.UserName;
				if (string.IsNullOrEmpty(currentMember.OpenId))
				{
					this.WeixinHead.Attributes.Add("noOpenId", "true");
				}
				control.SetWhenIsNotNull(currentMember.UserBindName);
				control5.SetWhenIsNotNull(currentMember.UserName);
				control2.SetWhenIsNotNull(currentMember.RealName);
				control3.SetWhenIsNotNull(currentMember.CellPhone);
				control4.SetWhenIsNotNull(currentMember.QQ);
				control6.SetWhenIsNotNull(currentMember.CardID);
			}
			PageTitle.AddSiteNameTitle("修改用户信息");
		}
	}
}
