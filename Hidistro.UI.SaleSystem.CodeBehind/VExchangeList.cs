using ControlPanel.Promotions;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
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
	public class VExchangeList : VshopTemplatedWebControl
	{
		private int id;

		private System.Web.UI.WebControls.Literal litPoints;

		private System.Web.UI.WebControls.Image imgCover;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideImgUrl;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideTitle;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideDesc;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddisInRange;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddUserId;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VExchangeList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["id"], out this.id))
			{
				base.GotoResourceNotFound("");
			}
			PointExChangeInfo pointExChangeInfo = PointExChangeHelper.Get(this.id);
			if (pointExChangeInfo != null)
			{
				this.hideImgUrl = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideImgUrl");
				this.hideTitle = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideTitle");
				this.hideDesc = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideDesc");
				this.hideTitle.Value = pointExChangeInfo.Name;
				this.hideDesc.Value = "活动时间：" + pointExChangeInfo.BeginDate.ToString("yyyy-MM-dd HH:mm:ss") + "至" + pointExChangeInfo.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
				System.Uri url = System.Web.HttpContext.Current.Request.Url;
				string imgUrl = pointExChangeInfo.ImgUrl;
				string str = string.Empty;
				if (!string.IsNullOrEmpty(imgUrl))
				{
					if (!imgUrl.StartsWith("http"))
					{
						str = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
					}
					this.hideImgUrl.Value = str + imgUrl;
				}
				PageTitle.AddSiteNameTitle(pointExChangeInfo.Name);
				this.imgCover = (System.Web.UI.WebControls.Image)this.FindControl("imgCover");
				if (!string.IsNullOrEmpty(pointExChangeInfo.ImgUrl))
				{
					this.imgCover.ImageUrl = pointExChangeInfo.ImgUrl;
				}
				else
				{
					this.imgCover.Visible = false;
				}
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
				if (currentMember != null)
				{
					this.hiddisInRange = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddisInRange");
					this.hiddUserId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddUserId");
					this.litPoints.Text = currentMember.Points.ToString();
					this.hiddUserId.Value = currentMember.UserId.ToString();
					if (MemberProcessor.CheckCurrentMemberIsInRange(pointExChangeInfo.MemberGrades, pointExChangeInfo.DefualtGroup, pointExChangeInfo.CustomGroup))
					{
						this.hiddisInRange.Value = "true";
					}
					else
					{
						this.hiddisInRange.Value = "false";
					}
				}
				else
				{
					this.litPoints.Text = "请先登录";
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
