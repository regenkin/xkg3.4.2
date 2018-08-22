using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Letter : AdminPage
	{
		protected string htmlJs = string.Empty;

		protected int LocalArticleID = Globals.RequestQueryNum("aid");

		protected Letter() : base("m07", "wbp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack && this.LocalArticleID > 0)
			{
				ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.LocalArticleID);
				if (articleInfo != null)
				{
					this.htmlJs = string.Concat(new object[]
					{
						"closeModal('#MyPictureIframe', 'txtContent', '",
						articleInfo.Url,
						"', '",
						base.Server.HtmlEncode(articleInfo.Title),
						"', '",
						base.Server.HtmlEncode(articleInfo.Memo),
						"', '",
						base.Server.HtmlEncode(articleInfo.ImageUrl),
						"', ",
						articleInfo.ArticleId,
						")"
					});
				}
			}
		}
	}
}
