using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductSearch : VshopTemplatedWebControl
	{
		private int categoryId;

		private string keyWord;

		private HiImage imgUrl;

		private System.Web.UI.WebControls.Literal litContent;

		private VshopTemplatedRepeater rptProducts;

		private VshopTemplatedRepeater rptCategories;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vProductSearch.html";
			}
			base.OnInit(e);
		}

		private void rptCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litpromotion");
				if (!string.IsNullOrEmpty(literal.Text))
				{
					literal.Text = "<img src='" + literal.Text + "'></img>";
				}
				else
				{
					literal.Text = "<img src='/Storage/master/default.png'></img>";
				}
			}
		}

		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
			this.Page.Session["stylestatus"] = "4";
			this.rptCategories.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
			System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
			this.rptCategories.DataSource = maxSubCategories;
			this.rptCategories.DataBind();
			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}
