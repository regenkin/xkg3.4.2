using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Goods : AdminPage
	{
		private string productName;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected Grid grdProducts;

		protected Pager pager;

		protected Grid grdTopCategries;

		protected Goods() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!base.IsPostBack)
			{
				this.BindProducts();
				this.BindData();
			}
		}

		private void BindData()
		{
			this.grdTopCategries.DataSource = CatalogHelper.GetSequenceCategories();
			this.grdTopCategries.DataBind();
		}

		private void BindProducts()
		{
			this.LoadParameters();
			ProductSaleStatus saleStatus = ProductSaleStatus.All;
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.productName,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SaleStatus = saleStatus
			};
			this.txtSearchText.Text = productQuery.Keywords;
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = products.TotalRecords;
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void ReloadProductOnSales(bool isSearch)
		{
			this.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{
				{
					"productName",
					Globals.UrlEncode(this.txtSearchText.Text.Trim())
				}
			});
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

		protected new void ReloadPage(System.Collections.Specialized.NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
		}
	}
}
