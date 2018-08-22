using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToTB : AdminPage
	{
		private string _productName;

		private string _productCode;

		private int? _categoryId;

		private System.DateTime? _startDate;

		private System.DateTime? _endDate;

		private bool _includeOnSales;

		private bool _includeUnSales;

		private bool _includeInStock;

		private string _isMakeTaobao;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.TextBox txtSKU;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.DropDownList dpTaoBao;

		protected System.Web.UI.WebControls.CheckBox chkOnSales;

		protected System.Web.UI.WebControls.CheckBox chkUnSales;

		protected System.Web.UI.WebControls.CheckBox chkInStock;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Label lblTotals;

		protected System.Web.UI.WebControls.DropDownList dropExportVersions;

		protected System.Web.UI.WebControls.CheckBox chkExportStock;

		protected System.Web.UI.WebControls.Button btnExport;

		protected Grid grdProducts;

		protected Pager pager;

		protected ExportToTB() : base("m02", "spp05")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.grdProducts.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdProducts_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
			}
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindExporter();
			}
		}

		private void grdProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Remove")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int num = (int)this.grdProducts.DataKeys[rowIndex].Value;
				string text = (string)this.ViewState["RemoveProductIds"];
				if (string.IsNullOrEmpty(text))
				{
					text = num.ToString();
				}
				else
				{
					text = text + "," + num.ToString();
				}
				this.ViewState["RemoveProductIds"] = text;
				this.BindProducts();
			}
		}

		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (!this._includeUnSales && !this._includeOnSales && !this._includeInStock)
			{
				this.ShowMsg("至少要选择包含一个商品状态", false);
				return;
			}
			string selectedValue = this.dropExportVersions.SelectedValue;
			if (string.IsNullOrEmpty(selectedValue) || selectedValue.Length == 0)
			{
				this.ShowMsg("请选择一个导出版本", false);
				return;
			}
			bool flag = false;
			bool @checked = this.chkExportStock.Checked;
			bool flag2 = true;
			string text = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ((System.Web.HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + System.Web.HttpContext.Current.Request.Url.Port)) + Globals.ApplicationPath;
			string applicationPath = Globals.ApplicationPath;
			System.Data.DataSet exportProducts = ProductHelper.GetExportProducts(this.GetQuery(), flag, @checked, (string)this.ViewState["RemoveProductIds"]);
			ExportAdapter exporter = TransferHelper.GetExporter(selectedValue, new object[]
			{
				exportProducts,
				flag,
				@checked,
				flag2,
				text,
				applicationPath
			});
			exporter.DoExport();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReSearchProducts();
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindProducts();
			}
		}

		private void BindExporter()
		{
			this.dropExportVersions.Items.Clear();
			this.dropExportVersions.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.Collections.Generic.Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "淘宝助理");
			foreach (string current in exportAdapters.Keys)
			{
				this.dropExportVersions.Items.Add(new System.Web.UI.WebControls.ListItem(exportAdapters[current], current));
			}
		}

		private void BindProducts()
		{
			if (!this._includeUnSales && !this._includeOnSales && !this._includeInStock)
			{
				this.ShowMsg("至少要选择包含一个商品状态", false);
				return;
			}
			AdvancedProductQuery query = this.GetQuery();
			DbQueryResult exportProducts = ProductHelper.GetExportProducts(query, (string)this.ViewState["RemoveProductIds"]);
			this.grdProducts.DataSource = exportProducts.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = exportProducts.TotalRecords;
			this.lblTotals.Text = exportProducts.TotalRecords.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		private AdvancedProductQuery GetQuery()
		{
			AdvancedProductQuery advancedProductQuery = new AdvancedProductQuery
			{
				Keywords = this._productName,
				ProductCode = this._productCode,
				CategoryId = this._categoryId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SaleStatus = ProductSaleStatus.OnSale,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this._startDate,
				EndDate = this._endDate,
				IncludeInStock = this._includeInStock,
				IncludeOnSales = this._includeOnSales,
				IncludeUnSales = this._includeUnSales,
				IsMakeTaobao = new int?(System.Convert.ToInt32(this._isMakeTaobao))
			};
			if (this._categoryId.HasValue)
			{
				advancedProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(this._categoryId.Value).Path;
			}
			Globals.EntityCoding(advancedProductQuery, true);
			return advancedProductQuery;
		}

		private void ReSearchProducts()
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection
			{
				{
					"productName",
					Globals.UrlEncode(this.txtSearchText.Text.Trim())
				},
				{
					"productCode",
					Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim()))
				},
				{
					"pageSize",
					this.pager.PageSize.ToString()
				},
				{
					"includeOnSales",
					this.chkOnSales.Checked.ToString()
				},
				{
					"includeUnSales",
					this.chkUnSales.Checked.ToString()
				},
				{
					"includeInStock",
					this.chkInStock.Checked.ToString()
				},
				{
					"isMakeTaobao",
					this.dpTaoBao.SelectedValue
				}
			};
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void LoadParameters()
		{
			this._productName = this.txtSearchText.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this._productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				this.txtSearchText.Text = this._productName;
			}
			this._productCode = this.txtSKU.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this._productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
				this.txtSKU.Text = this._productCode;
			}
			this._categoryId = this.dropCategories.SelectedValue;
			int value;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]) && int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this._categoryId = new int?(value);
				this.dropCategories.SelectedValue = this._categoryId;
			}
			this._startDate = this.calendarStartDate.SelectedDate;
			System.DateTime value2;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["startDate"], out value2))
			{
				this._startDate = new System.DateTime?(value2);
				this.calendarStartDate.SelectedDate = this._startDate;
			}
			this._endDate = this.calendarEndDate.SelectedDate;
			System.DateTime value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["endDate"], out value3))
			{
				this._endDate = new System.DateTime?(value3);
				this.calendarEndDate.SelectedDate = this._endDate;
			}
			this._includeOnSales = this.chkOnSales.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeOnSales"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeOnSales"], out this._includeOnSales);
				this.chkOnSales.Checked = this._includeOnSales;
			}
			this._includeUnSales = this.chkUnSales.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeUnSales"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeUnSales"], out this._includeUnSales);
				this.chkUnSales.Checked = this._includeUnSales;
			}
			this._includeInStock = this.chkInStock.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeInStock"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeInStock"], out this._includeInStock);
				this.chkInStock.Checked = this._includeInStock;
			}
			this._isMakeTaobao = this.dpTaoBao.SelectedValue;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isMakeTaobao"]))
			{
				this._isMakeTaobao = this.Page.Request.QueryString["isMakeTaobao"];
				this.dpTaoBao.SelectedValue = this.Page.Request.QueryString["isMakeTaobao"];
			}
		}
	}
}
