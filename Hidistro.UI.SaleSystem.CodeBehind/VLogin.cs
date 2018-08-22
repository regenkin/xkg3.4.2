using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VLogin : VshopTemplatedWebControl
	{
		private string sessionId;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VLogin.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.sessionId = this.Page.Request.QueryString["sessionId"];
			if (string.IsNullOrEmpty(this.sessionId))
			{
				this.Page.Response.Redirect("Default.aspx");
			}
			else
			{
				MemberInfo memberInfo = MemberProcessor.GetMember(this.sessionId);
				if (memberInfo == null || memberInfo.SessionEndTime < System.DateTime.Now)
				{
					this.Page.Response.Redirect("Default.aspx");
				}
				else
				{
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					if (currentMember != null)
					{
						if (string.IsNullOrEmpty(currentMember.OpenId) && currentMember.UserId != memberInfo.UserId)
						{
							currentMember.OpenId = memberInfo.OpenId;
							MemberProcessor.UpdateMember(currentMember);
							MemberProcessor.Delete(memberInfo.UserId);
							memberInfo = currentMember;
						}
					}
					base.setLogin(memberInfo.UserId);
					MemberProcessor.SetMemberSessionId(memberInfo.SessionId, System.DateTime.Now, memberInfo.OpenId);
					if (!string.IsNullOrEmpty(memberInfo.UserName) && memberInfo.UserName != memberInfo.OpenId)
					{
						this.Page.Response.Redirect("Default.aspx");
					}
					else
					{
						PageTitle.AddSiteNameTitle("登录");
					}
				}
			}
		}
	}
}
