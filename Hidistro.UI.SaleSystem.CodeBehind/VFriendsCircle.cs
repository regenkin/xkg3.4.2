using ASPNET.WebControls;
using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Weibo;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VFriendsCircle : VshopTemplatedWebControl
	{
		private Pager pager;

		private VshopTemplatedRepeater refriendscircle;

		private System.Web.UI.WebControls.Literal ItemHtml;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VFriendsCircle.html";
			}
			base.OnInit(e);
		}

		private void refriendscircle_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Repeater repeater = e.Item.Controls[0].FindControl("ItemInfo") as System.Web.UI.WebControls.Repeater;
				System.Web.UI.WebControls.Literal literal = e.Item.Controls[0].FindControl("ItemHtml") as System.Web.UI.WebControls.Literal;
				DataRowView dataRowView = (DataRowView)e.Item.DataItem;
				if (dataRowView["ArticleType"].ToString() == "4")
				{
					System.Collections.Generic.IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(dataRowView["ArticleId"].ToString()));
					if (articleItems != null)
					{
						repeater.DataSource = articleItems;
						repeater.DataBind();
					}
				}
				else
				{
					literal.Text = "<div class='mate-ctx clear' >" + dataRowView["Memo"].ToString() + "</div>";
				}
			}
		}

		private void BindData()
		{
			ArticleQuery articleQuery = new ArticleQuery();
			articleQuery.SortBy = "ArticleId";
			articleQuery.SortOrder = SortAction.Asc;
			Globals.EntityCoding(articleQuery, true);
			articleQuery.PageIndex = this.pager.PageIndex;
			articleQuery.PageSize = this.pager.PageSize;
			articleQuery.IsShare = 1;
			DbQueryResult articleRequest = ArticleHelper.GetArticleRequest(articleQuery);
			this.refriendscircle.DataSource = articleRequest.Data;
			this.refriendscircle.DataBind();
			this.pager.TotalRecords = articleRequest.TotalRecords;
		}

		protected override void AttachChildControls()
		{
			this.refriendscircle = (VshopTemplatedRepeater)this.FindControl("refriendscircle");
			this.ItemHtml = (System.Web.UI.WebControls.Literal)this.FindControl("ItemHtml");
			this.pager = (Pager)this.FindControl("pager");
			this.refriendscircle.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.refriendscircle_ItemDataBound);
			this.BindData();
			PageTitle.AddSiteNameTitle("朋友圈素材");
		}
	}
}
