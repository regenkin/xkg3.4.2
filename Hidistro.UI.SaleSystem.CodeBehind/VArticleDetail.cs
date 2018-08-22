using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Entities.Weibo;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VArticleDetail : VshopTemplatedWebControl
	{
		protected int itemID = 0;

		protected int singleID = 0;

		protected string htmlTitle = string.Empty;

		private HiImage imgUrl;

		private System.Web.UI.WebControls.Literal litContent;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VArticleDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.itemID = Globals.RequestQueryNum("iid");
			this.singleID = Globals.RequestQueryNum("sid");
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			if (this.singleID > 0)
			{
				ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.singleID);
				if (articleInfo != null)
				{
					this.htmlTitle = articleInfo.Title;
					this.imgUrl.ImageUrl = articleInfo.ImageUrl;
					this.litContent.Text = articleInfo.Content;
				}
				else
				{
					base.GotoResourceNotFound("");
				}
			}
			else if (this.itemID > 0)
			{
				ArticleItemsInfo articleItemsInfo = ArticleHelper.GetArticleItemsInfo(this.itemID);
				if (articleItemsInfo != null)
				{
					this.htmlTitle = articleItemsInfo.Title;
					this.imgUrl.ImageUrl = articleItemsInfo.ImageUrl;
					this.litContent.Text = articleItemsInfo.Content;
				}
				else
				{
					base.GotoResourceNotFound("");
				}
			}
			else
			{
				base.GotoResourceNotFound("");
			}
			PageTitle.AddSiteNameTitle(this.htmlTitle);
		}
	}
}
