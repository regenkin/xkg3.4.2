using ASPNET.WebControls;
using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class GetArticles : AdminPage
	{
		protected string ArticleTitle = string.Empty;

		private int pageno;

		protected int recordcount;

		protected int articletype;

		private string title = string.Empty;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected GetArticles() : base("m01", "dpp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.BindData(this.articletype, this.pageno, this.ArticleTitle);
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.Form["key"]))
			{
				this.ArticleTitle = base.Server.UrlDecode(this.Page.Request.Form["key"]);
			}
			string s = base.Request.Form["type"];
			string s2 = base.Request.QueryString["pageindex"];
			int.TryParse(s, out this.articletype);
			int.TryParse(s2, out this.pageno);
			if (this.pageno < 1)
			{
				this.pageno = 1;
			}
			switch (this.articletype)
			{
			case 1:
			case 2:
			case 4:
				return;
			case 3:
				return;
			}
            this.articletype = 0;
        }

		protected string FormatArticleShow(object articleId, object articletype, object title, object pubtime, object imgurl, object memo, object IsShare)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (articletype.ToString() == "2")
			{
				stringBuilder.AppendLine("<div class='single-mate mate-list'>");
				stringBuilder.AppendLine("    <div class='mate-inner'>");
				stringBuilder.AppendLine("          <h3>" + title + "</h3>");
				stringBuilder.AppendLine("           <span>" + System.DateTime.Parse(pubtime.ToString()).ToString("yyyy-MM-dd HH:mm") + "</span>");
				stringBuilder.AppendLine("         <div class='mate-img'>");
				stringBuilder.AppendLine("             <img src='" + imgurl + "' class='img-responsive'>");
				stringBuilder.AppendLine("          </div>");
				stringBuilder.AppendLine("         <p class='mate-info'>" + memo + "</p>");
				stringBuilder.AppendLine("     </div>");
				stringBuilder.AppendLine(" <div class='nav clearfix'>");
				stringBuilder.AppendLine("     <a class='one' href='../weixin/sendalledit.aspx?aid=" + articleId + "'>微信群发</a>");
				stringBuilder.AppendLine("     <a href='../weibo/letter.aspx?aid=" + articleId + "'>微博群发</a>");
				stringBuilder.AppendLine("     <a href='javascript:void(0)' onclick='ArticleView(" + articleId + ")'>预览</a>");
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"     <a href='javascript:void(0)' onclick='editOneArticle(",
					articleId,
					",",
					articletype.ToString(),
					")'>编辑</a>"
				}));
				stringBuilder.AppendLine("     <a href='javascript:void(0)' class='dropdown'>");
				stringBuilder.AppendLine("         <span id='dLabel' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>删除");
				stringBuilder.AppendLine("         </span>");
				stringBuilder.AppendLine("         <div class='dropdown-menu width' aria-labelledby='dLabel'>");
				stringBuilder.AppendLine("             <p class='dropdown-header'>确定删除吗？</p>");
				stringBuilder.AppendLine("             <button type='button' class='btn btn-danger marg' onclick='delOneArticle(" + articleId + ")'>删除</button>");
				stringBuilder.AppendLine("             <button type='button' class='btn btn-primary'>取消</button>");
				stringBuilder.AppendLine("         </div>");
				stringBuilder.AppendLine("     </a>");
				stringBuilder.AppendLine(" </div>");
				if ((bool)IsShare)
				{
					stringBuilder.AppendLine("<p class='distributor'>分销商</p>");
				}
				stringBuilder.AppendLine("</div>");
			}
			else if (articletype.ToString() == "4")
			{
				stringBuilder.AppendLine("<div class='many-mate mate-list'>");
				stringBuilder.AppendLine("    <div class='mate-inner top'>");
				stringBuilder.AppendLine("        <span>" + System.DateTime.Parse(pubtime.ToString()).ToString("yyyy-MM-dd HH:mm") + "</span>");
				stringBuilder.AppendLine("        <div class='mate-img'>");
				stringBuilder.AppendLine("            <img src='" + imgurl + "' class='img-responsive'>");
				stringBuilder.AppendLine("            <div class='title'>" + title + "</div>");
				stringBuilder.AppendLine("        </div>");
				if ((bool)IsShare)
				{
					stringBuilder.AppendLine("<p class='distributor'>分销商</p>");
				}
				stringBuilder.AppendLine("    </div>");
				System.Collections.Generic.IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(articleId.ToString()));
				foreach (ArticleItemsInfo current in articleItems)
				{
					stringBuilder.AppendLine("    <div class='mate-inner'>");
					stringBuilder.AppendLine("        <div class='child-mate'>");
					stringBuilder.AppendLine("            <div class='child-mate-title clearfix'>");
					stringBuilder.AppendLine("                <div class='title'>");
					stringBuilder.AppendLine("                    <h4>" + current.Title + "</h4>");
					stringBuilder.AppendLine("                </div>");
					stringBuilder.AppendLine("                <div class='img'>");
					stringBuilder.AppendLine("                    <img src='" + current.ImageUrl + "' class='img-responsive'>");
					stringBuilder.AppendLine("                </div>");
					stringBuilder.AppendLine("            </div>");
					stringBuilder.AppendLine("        </div>");
					stringBuilder.AppendLine("");
					stringBuilder.AppendLine("    </div>");
				}
				stringBuilder.AppendLine("    <div class='nav clearfix'>");
				stringBuilder.AppendLine("        <a class='one' href='../weixin/sendalledit.aspx?aid=" + articleId + "'>微信群发</a>");
				stringBuilder.AppendLine("        <a href='../weibo/letter.aspx?aid=" + articleId + "'>微博群发</a>");
				stringBuilder.AppendLine("        <a href='javascript:void(0)' onclick='ArticleView(" + articleId + ")'>预览</a>");
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"        <a href='javascript:void(0)' onclick='editOneArticle(",
					articleId,
					",",
					articletype.ToString(),
					")'>编辑</a>"
				}));
				stringBuilder.AppendLine("        <a href='javascript:void(0)' class='dropdown'>");
				stringBuilder.AppendLine("            <span id='dLabel' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>删除");
				stringBuilder.AppendLine("            </span>");
				stringBuilder.AppendLine("            <div class='dropdown-menu width' aria-labelledby='dLabel'>");
				stringBuilder.AppendLine("                <p class='dropdown-header'>确定删除吗？</p>");
				stringBuilder.AppendLine("                <button type='button' class='btn btn-danger marg' onclick='delOneArticle(" + articleId + ")'>删除</button>");
				stringBuilder.AppendLine("                <button type='button' class='btn btn-primary'>取消</button>");
				stringBuilder.AppendLine("            </div>");
				stringBuilder.AppendLine("        </a>");
				stringBuilder.AppendLine("    </div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}

		private void BindData(int articletype, int pageno, string title)
		{
			ArticleQuery articleQuery = new ArticleQuery();
			articleQuery.Title = title;
			articleQuery.ArticleType = articletype;
			if (articletype == 1)
			{
				articleQuery.ArticleType = 0;
				articleQuery.IsShare = 1;
			}
			articleQuery.SortBy = "PubTime";
			articleQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(articleQuery, true);
			articleQuery.PageIndex = pageno;
			articleQuery.PageSize = this.pager.PageSize;
			DbQueryResult articleRequest = ArticleHelper.GetArticleRequest(articleQuery);
			this.rptList.DataSource = articleRequest.Data;
			this.rptList.DataBind();
			int totalRecords = articleRequest.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			this.recordcount = totalRecords;
			if (this.pager.TotalRecords <= this.pager.PageSize)
			{
				this.pager.Visible = false;
			}
		}
	}
}
