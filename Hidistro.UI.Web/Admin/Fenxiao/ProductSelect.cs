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

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class ProductSelect : System.Web.UI.Page
	{
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
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdProducts.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
			this.grdProducts.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.grdProducts_ItemDataBound);
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

		private void grdProducts_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
				{
					string value = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "productid").ToString();
					System.Web.UI.WebControls.Button button = e.Item.FindControl("btnCheck") as System.Web.UI.WebControls.Button;
					System.Web.UI.HtmlControls.HtmlControl htmlControl = e.Item.FindControl("trId") as System.Web.UI.HtmlControls.HtmlControl;
					if (masterSettings.DistributorProducts.Contains(value))
					{
						button.Text = "取消";
						button.Attributes.CssStyle.Remove("background-color");
						button.Attributes.CssStyle.Remove("border-color");
						button.Attributes.CssStyle.Add("background-color", "#5cb85c");
						button.Attributes.CssStyle.Add("border-color", "#4cae4c");
						htmlControl.Attributes.Add("class", "selRow");
						return;
					}
					button.Text = "选择";
					button.Attributes.CssStyle.Remove("background-color");
					button.Attributes.CssStyle.Remove("border-color");
					button.Attributes.CssStyle.Add("background-color", "#286090");
					button.Attributes.CssStyle.Add("border-color", "#204d74");
					htmlControl.Attributes.Remove("class");
				}
			}
		}

		private void grdProducts_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "check")
			{
				string text = e.CommandArgument.ToString();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
				{
					string text2 = "";
					if (masterSettings.DistributorProducts.Contains(text))
					{
						string[] array = masterSettings.DistributorProducts.Split(new char[]
						{
							','
						});
						for (int i = 0; i < array.Length; i++)
						{
							string text3 = array[i];
							if (!text3.Equals(text))
							{
								text2 = text2 + text3 + ",";
							}
						}
						if (text2.Length > 0)
						{
							text2 = text2.Substring(0, text2.Length - 1);
						}
					}
					else
					{
						text2 = masterSettings.DistributorProducts + "," + text;
					}
					masterSettings.DistributorProducts = text2;
				}
				else
				{
					masterSettings.DistributorProducts = text;
				}
				SettingsManager.Save(masterSettings);
				this.BindProducts();
			}
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
