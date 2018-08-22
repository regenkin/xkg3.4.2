using ControlPanel.WeiBo;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class MultiArticlesEdit : AdminPage
	{
		protected string articleJson;

		protected int MaterialID;

		protected string htmlOperName = "新增";

		protected string htmlLinkType = "1";

		protected string htmlUrl = string.Empty;

		protected string htmlLinkTypeName = "阅读原文";

		protected string ReUrl = string.Empty;

		protected string htmlAddJs = string.Empty;

		protected ucUeditor fkContent;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox IsShare;

		protected MultiArticlesEdit() : base("m01", "dpp06")
		{
		}

		[PrivilegeCheck(Privilege.Summary)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.ReUrl = Globals.RequestQueryStr("reurl");
			if (string.IsNullOrEmpty(this.ReUrl))
			{
				this.ReUrl = "articles.aspx";
			}
			int.TryParse(Globals.RequestQueryStr("id"), out this.MaterialID);
			string a = Globals.RequestQueryStr("cmd");
			if (a == "add")
			{
				base.Response.ContentType = "application/json";
				string value = Globals.RequestFormStr("MultiArticle");
				string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
				System.Collections.Generic.List<ArticleList> list = JsonConvert.DeserializeObject<System.Collections.Generic.List<ArticleList>>(value);
				if (list != null && list.Count > 0)
				{
					int num = 0;
					ArticleInfo articleInfo = new ArticleInfo();
					System.Collections.Generic.List<ArticleItemsInfo> list2 = new System.Collections.Generic.List<ArticleItemsInfo>();
					System.DateTime now = System.DateTime.Now;
					string text = string.Empty;
					foreach (ArticleList current in list)
					{
						if (current.Title == "")
						{
							text = "标题不能为空!";
							break;
						}
						if (current.ImageUrl == "")
						{
							text = "请选择一张封面!";
							break;
						}
						if (current.LinkType == LinkType.ArticleDetail && current.Content == "")
						{
							text = "请输入内容!";
							break;
						}
						if (current.LinkType != LinkType.ArticleDetail && current.Url == "")
						{
							text = "请选择或输入自定义链接!";
							break;
						}
						if (current.Status != "del")
						{
							if (num == 0)
							{
								articleInfo.Title = current.Title;
								articleInfo.ArticleType = ArticleType.List;
								articleInfo.Content = current.Content;
								articleInfo.ImageUrl = current.ImageUrl;
								articleInfo.Url = current.Url;
								articleInfo.LinkType = current.LinkType;
								articleInfo.Memo = "";
								articleInfo.ArticleId = this.MaterialID;
								articleInfo.PubTime = now;
								articleInfo.IsShare = current.IsShare;
								num++;
							}
							else
							{
								ArticleItemsInfo articleItemsInfo = current;
								articleItemsInfo.PubTime = now;
								list2.Add(articleItemsInfo);
								num++;
							}
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						s = "{\"type\":\"0\",\"tips\":\"" + text + "\"}";
						base.Response.Write(s);
						base.Response.End();
					}
					articleInfo.ItemsInfo = list2;
					if (articleInfo.ArticleId > 0)
					{
						bool flag = ArticleHelper.UpdateMultiArticle(articleInfo);
						if (flag)
						{
							s = "{\"type\":\"1\",\"id\":\"" + articleInfo.ArticleId + "\",\"tips\":\"多图素材修改成功！\"}";
						}
					}
					else
					{
						int num2 = ArticleHelper.AddMultiArticle(articleInfo);
						if (num2 > 0)
						{
							s = "{\"type\":\"1\",\"id\":\"" + num2 + "\",\"tips\":\"多图素材新增成功！\"}";
						}
					}
					base.Response.Write(s);
					base.Response.End();
					return;
				}
			}
			else if (this.MaterialID > 0)
			{
				this.htmlOperName = "编辑";
				ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(this.MaterialID);
				if (articleInfo2 != null)
				{
					if (articleInfo2.ArticleType == ArticleType.News)
					{
						base.Response.Redirect("articlesedit.aspx?id=" + this.MaterialID);
						base.Response.End();
						return;
					}
					System.Collections.Generic.IList<ArticleItemsInfo> itemsInfo = articleInfo2.ItemsInfo;
					itemsInfo.Insert(0, new ArticleItemsInfo
					{
						ArticleId = this.MaterialID,
						Title = articleInfo2.Title,
						ImageUrl = articleInfo2.ImageUrl,
						Url = articleInfo2.Url,
						Content = articleInfo2.Content,
						LinkType = articleInfo2.LinkType,
						Id = 0,
						IsShare = articleInfo2.IsShare
					});
					this.IsShare.Checked = articleInfo2.IsShare;
					this.htmlLinkType = ((int)articleInfo2.LinkType).ToString();
					if (this.htmlLinkType != "1")
					{
						this.htmlAddJs = "$('#urlData').show();";
					}
					this.htmlLinkTypeName = articleInfo2.LinkType.ToShowText();
					System.Collections.Generic.List<ArticleList> list3 = new System.Collections.Generic.List<ArticleList>();
					int num3 = 1;
					foreach (ArticleItemsInfo current2 in itemsInfo)
					{
						list3.Add(new ArticleList
						{
							Id = current2.Id,
							Title = current2.Title,
							Url = current2.Url,
							ImageUrl = current2.ImageUrl,
							Content = current2.Content,
							BoxId = num3++.ToString(),
							LinkType = current2.LinkType,
							Status = "",
							IsShare = current2.IsShare
						});
					}
					this.articleJson = JsonConvert.SerializeObject(list3);
					return;
				}
			}
			else
			{
				this.articleJson = "''";
			}
		}
	}
}
