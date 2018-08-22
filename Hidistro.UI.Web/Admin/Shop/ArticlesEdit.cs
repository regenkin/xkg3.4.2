using ControlPanel.WeiBo;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ArticlesEdit : AdminPage
	{
		protected string articleJson;

		protected int MaterialID;

		protected string htmlOperName = "新增";

		protected string htmlArticleTitle = "单条图文标题";

		protected string htmlImgUrl = string.Empty;

		protected string htmlUrl = string.Empty;

		protected string htmlLinkTypeName = "查看全文";

		protected string htmlMemo = "摘要";

		protected string htmlLinkType = "1";

		protected string htmlAddJs = string.Empty;

		protected string htmlDate = System.DateTime.Now.ToString("M月d日");

		protected string ReUrl = string.Empty;

		protected ucUeditor fkContent;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox IsShare;

		protected ArticlesEdit() : base("m01", "dpp06")
		{
		}

		[PrivilegeCheck(Privilege.Summary)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(base.Request.QueryString["id"], out this.MaterialID);
			string arg_36_0 = base.Request.QueryString["cmd"];
			this.ReUrl = base.Request.QueryString["reurl"];
			if (string.IsNullOrEmpty(this.ReUrl))
			{
				this.ReUrl = "articles.aspx";
			}
			string a = base.Request.Form["posttype"];
			if (!base.IsPostBack)
			{
				if (a == "addsinglearticle")
				{
					base.Response.ContentType = "application/json";
					string text = base.Request.Form["linkUrl"];
					string text2 = base.Request.Form["title"];
					string text3 = base.Request.Form["img"];
					string memo = base.Request.Form["memo"];
					string content = base.Request.Form["content"];
					string s = base.Request.Form["linkType"];
					string value = base.Request.Form["IsShare"];
					int num = 1;
					int.TryParse(s, out num);
					string s2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
					if (string.IsNullOrEmpty(text2))
					{
						s2 = "{\"type\":\"0\",\"tips\":\"请填写标题！\"}";
						base.Response.Write(s2);
						base.Response.End();
					}
					if (string.IsNullOrEmpty(text3))
					{
						s2 = "{\"type\":\"0\",\"tips\":\"请选择封面图片！\"}";
						base.Response.Write(s2);
						base.Response.End();
					}
					if (num != 1 && string.IsNullOrEmpty(text))
					{
						s2 = "{\"type\":\"0\",\"tips\":\"请设置链接地址！\"}";
						base.Response.Write(s2);
						base.Response.End();
					}
					ArticleInfo articleInfo = new ArticleInfo();
					articleInfo.ArticleId = this.MaterialID;
					articleInfo.Url = text;
					articleInfo.Title = text2;
					if (string.IsNullOrEmpty(value))
					{
						articleInfo.IsShare = false;
					}
					else
					{
						articleInfo.IsShare = bool.Parse(value);
					}
					articleInfo.ImageUrl = text3;
					articleInfo.Memo = memo;
					articleInfo.ArticleType = ArticleType.News;
					articleInfo.PubTime = System.DateTime.Now;
					articleInfo.Content = content;
					articleInfo.LinkType = (LinkType)num;
					if (articleInfo.ArticleId > 0)
					{
						if (ArticleHelper.UpdateSingleArticle(articleInfo))
						{
							s2 = "{\"type\":\"1\",\"id\":\"" + articleInfo.ArticleId + "\",\"tips\":\"单图文修改成功！\"}";
						}
					}
					else
					{
						int num2 = ArticleHelper.AddSingerArticle(articleInfo);
						if (num2 > 0)
						{
							s2 = "{\"type\":\"1\",\"id\":\"" + num2 + "\",\"tips\":\"单图文新增成功！\"}";
						}
					}
					base.Response.Write(s2);
					base.Response.End();
					return;
				}
				if (this.MaterialID > 0)
				{
					this.htmlOperName = "编辑";
					ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(this.MaterialID);
					if (articleInfo2 != null)
					{
						if (articleInfo2.ArticleType == ArticleType.List)
						{
							base.Response.Redirect("multiarticlesedit.aspx?id=" + this.MaterialID);
							base.Response.End();
							return;
						}
						this.htmlArticleTitle = articleInfo2.Title;
						this.htmlImgUrl = articleInfo2.ImageUrl;
						this.htmlUrl = articleInfo2.Url;
						this.htmlMemo = articleInfo2.Memo;
						this.htmlDate = articleInfo2.PubTime.ToString("M月d日");
						this.fkContent.Text = articleInfo2.Content;
						this.IsShare.Checked = articleInfo2.IsShare;
						this.htmlAddJs = "BindPicData('" + this.htmlImgUrl + "');";
						this.htmlLinkType = ((int)articleInfo2.LinkType).ToString();
						if (this.htmlLinkType != "1")
						{
							this.htmlAddJs += "$('#urlData').show();";
						}
						this.htmlLinkTypeName = articleInfo2.LinkType.ToShowText();
					}
				}
			}
		}
	}
}
