using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VNoticeDetail : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litTitle;

		private System.Web.UI.WebControls.Literal litPubTime;

		private System.Web.UI.WebControls.Literal litMemo;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-vNoticeDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = Globals.RequestQueryStr("type");
			int num = Globals.RequestQueryNum("id");
			this.litTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litTitle");
			this.litPubTime = (System.Web.UI.WebControls.Literal)this.FindControl("litPubTime");
			this.litMemo = (System.Web.UI.WebControls.Literal)this.FindControl("litMemo");
			NoticeInfo noticeInfo = NoticeBrowser.GetNoticeInfo(num);
			if (noticeInfo != null)
			{
				int sendType = noticeInfo.SendType;
				string text = "公告";
				if (sendType == 1)
				{
					text = "消息";
				}
				this.litTitle.Text = noticeInfo.Title;
				System.DateTime dateTime = noticeInfo.PubTime.HasValue ? noticeInfo.PubTime.Value : noticeInfo.AddTime;
				this.litPubTime.Text = string.Concat(new string[]
				{
					"<span>",
					dateTime.ToString("yyyy-MM-dd"),
					"</span><span><i class='glyphicon glyphicon-time'></i>",
					dateTime.ToString("HH:mm"),
					"</span>"
				});
				this.litMemo.Text = string.Concat(new object[]
				{
					noticeInfo.Memo,
					"<p class=\"lookall\"><a href=\"notice.aspx?type=",
					sendType,
					"\">更多",
					text,
					"&gt;&gt;</a></p>"
				});
				PageTitle.AddSiteNameTitle(noticeInfo.Title);
				if (!(a == "view"))
				{
					if (noticeInfo.IsPub == 1)
					{
						MemberInfo currentMember = MemberProcessor.GetCurrentMember();
						NoticeBrowser.ViewNotice(currentMember.UserId, num);
					}
					else
					{
						System.Web.HttpContext.Current.Response.Write("文章未发布！");
						System.Web.HttpContext.Current.Response.End();
					}
				}
			}
			else
			{
				System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
				System.Web.HttpContext.Current.Response.End();
			}
		}
	}
}
