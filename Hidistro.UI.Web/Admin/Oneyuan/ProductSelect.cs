using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class ProductSelect : System.Web.UI.Page
	{
		protected int IsMultil;

		private string productName;

		private int? categoryId;

		private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(base.Request.QueryString["IsMultil"], out this.IsMultil);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.IsUnclassified = true;
				this.dropCategories.DataBind();
				this.BindProducts();
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			this.txtSearchText.Text = this.productName;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
		}

		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("SaleStatus", "1");
			this.ReloadPage(nameValueCollection);
		}

		protected void ReloadPage(System.Collections.Specialized.NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		private string GenericReloadUrl(System.Collections.Specialized.NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return base.Request.Url.AbsolutePath;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append(base.Request.Url.AbsolutePath).Append("?");
			foreach (string text in queryStrings.Keys)
			{
				string text2 = queryStrings[text].Trim().Replace("'", "");
				if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
				{
					stringBuilder.Append(text).Append("=").Append(base.Server.UrlEncode(text2)).Append("&");
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.productName,
				CategoryId = this.categoryId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				BrandId = null,
				SaleStatus = this.saleStatus
			};
			if (this.categoryId.HasValue && this.categoryId > 0)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.txtSearchText.Text = productQuery.Keywords;
			this.dropCategories.SelectedValue = productQuery.CategoryId;
			ProductHelper.GetProductNum();
			this.pager.TotalRecords = products.TotalRecords;
		}
	}
}
