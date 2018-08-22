using ControlPanel.WeiBo;
using Hidistro.Entities.Weibo;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Vshop
{
	public class ArticleShow : System.Web.UI.Page
	{
		protected int articleID;

		protected string htmlTitle = string.Empty;

		protected string htmlImageUrl = string.Empty;

		protected string htmlUrl = string.Empty;

		protected string htmlMemo = string.Empty;

		protected string htmlPubTime = string.Empty;

		protected string ArticleType = string.Empty;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string s = base.Request.QueryString["ID"];
			int.TryParse(s, out this.articleID);
			if (this.articleID > 0)
			{
				ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.articleID);
				if (articleInfo == null)
				{
					this.GoBack();
					return;
				}
				this.htmlTitle = articleInfo.Title;
				this.ArticleType = articleInfo.ArticleType.ToString().ToLower();
				this.htmlImageUrl = articleInfo.ImageUrl;
				this.htmlUrl = articleInfo.Url;
				this.htmlPubTime = articleInfo.PubTime.ToString("yyyy-MM-dd HH:mm");
				this.htmlMemo = articleInfo.Memo;
				if (this.ArticleType == "list")
				{
					this.rptList.DataSource = articleInfo.ItemsInfo;
					this.rptList.DataBind();
					return;
				}
			}
			else
			{
				this.GoBack();
			}
		}

		private void GoBack()
		{
			base.Response.Write("<script>history.go(-1)</script>");
			base.Response.End();
		}
	}
}
