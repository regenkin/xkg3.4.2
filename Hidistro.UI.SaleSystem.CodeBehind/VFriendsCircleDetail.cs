using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Entities.Weibo;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VFriendsCircleDetail : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Repeater TopCtx;

		private System.Web.UI.WebControls.Repeater ItemCtx;

		protected int MaterialID = 0;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VFriendsCircleDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(Globals.RequestQueryStr("id"), out this.MaterialID))
			{
				base.GotoResourceNotFound("");
			}
			else
			{
				ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.MaterialID);
				string title = articleInfo.Title;
				System.DateTime now = System.DateTime.Now;
				this.TopCtx = (System.Web.UI.WebControls.Repeater)this.FindControl("TopCtx");
				this.ItemCtx = (System.Web.UI.WebControls.Repeater)this.FindControl("ItemCtx");
				System.Collections.Generic.List<ArticleInfo> list = new System.Collections.Generic.List<ArticleInfo>();
				list.Add(articleInfo);
				this.TopCtx.DataSource = list;
				this.TopCtx.DataBind();
				if (articleInfo.ArticleType == ArticleType.List)
				{
					System.Collections.Generic.IList<ArticleItemsInfo> itemsInfo = articleInfo.ItemsInfo;
					this.ItemCtx.DataSource = itemsInfo;
					this.ItemCtx.DataBind();
				}
				PageTitle.AddSiteNameTitle(title);
			}
		}
	}
}
