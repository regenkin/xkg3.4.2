using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductList : VshopTemplatedWebControl
	{
		private int categoryId;

		private string keyWord;

		private string pIds;

		private HiImage imgUrl;

		private System.Web.UI.WebControls.Literal litContent;

		private VshopTemplatedRepeater rptProducts;

		private VshopTemplatedRepeater rptCategories;

		private VshopTemplatedRepeater rptCategoryList;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdfKeyword;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			int num = Globals.RequestQueryNum("isLimitedTimeDiscountId");
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.pIds = this.Page.Request.QueryString["pIds"];
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
			}
			this.hdfKeyword = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdfKeyword");
			this.hdfKeyword.Value = this.keyWord;
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
			this.rptCategoryList = (VshopTemplatedRepeater)this.FindControl("rptCategoryList");
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			string text = this.Page.Request.QueryString["sort"];
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "DisplaySequence";
			}
			string text2 = this.Page.Request.QueryString["order"];
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = "desc";
			}
			int pageNumber;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageNumber))
			{
				pageNumber = 1;
			}
			int maxNum;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out maxNum))
			{
				maxNum = 20;
			}
			System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
			this.rptCategories.DataSource = maxSubCategories;
			this.rptCategories.DataBind();
			DataSet categoryList = CategoryBrowser.GetCategoryList();
			this.rptCategoryList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCategoryList_ItemDataBound);
			this.rptCategoryList.DataSource = categoryList;
			this.rptCategoryList.DataBind();
			int num2;
			this.rptProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, pageNumber, maxNum, out num2, text, text2, this.pIds, num == 1);
			this.rptProducts.DataBind();
			this.txtTotalPages.SetWhenIsNotNull(num2.ToString());
			string title = "商品列表";
			if (this.categoryId > 0)
			{
				CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
				if (category != null)
				{
					title = category.Name;
				}
			}
			PageTitle.AddSiteNameTitle(title);
		}

		private void rptCategoryList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			DataView dataView = (DataView)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "SubCategories");
			DataRowView dataRowView = (DataRowView)e.Item.DataItem;
			int num = System.Convert.ToInt32(dataRowView["CategoryId"]);
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litPlus");
			if (dataView == null || dataView.ToTable().Rows.Count == 0)
			{
				literal.Visible = false;
			}
			else
			{
				literal.Visible = true;
			}
		}
	}
}
