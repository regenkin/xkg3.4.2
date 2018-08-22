using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddProductToPointExchange : AdminPage
	{
		protected ProductSaleStatus status = ProductSaleStatus.OnSale;

		protected int eId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lbSelectNumber;

		protected System.Web.UI.WebControls.Label lbsaleNumber;

		protected System.Web.UI.WebControls.Label lbwareNumber;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minPrice;

		protected System.Web.UI.WebControls.TextBox txt_maxPrice;

		protected System.Web.UI.WebControls.Button btnQuery;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected AddProductToPointExchange() : base("m08", "yxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && !this.bInt(base.Request["id"].ToString(), ref this.eId))
			{
				this.eId = 0;
			}
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			if (!base.IsPostBack)
			{
				this.BindProducts(this.eId);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.BindProducts(this.eId);
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private void BindProducts(int exchangeId)
		{
			if (exchangeId == 0)
			{
				return;
			}
			string text = this.txt_name.Text;
			string text2 = this.txt_minPrice.Text;
			string text3 = this.txt_maxPrice.Text;
			decimal? minPrice = null;
			decimal? maxPrice = null;
			decimal value = 0m;
			if (!this.bDecimal(text2, ref value))
			{
				minPrice = null;
			}
			else
			{
				minPrice = new decimal?(value);
			}
			if (!this.bDecimal(text3, ref value))
			{
				maxPrice = null;
			}
			else
			{
				maxPrice = new decimal?(value);
			}
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = text,
				ProductCode = "",
				CategoryId = null,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = null,
				BrandId = null,
				EndDate = null,
				TypeId = null,
				SaleStatus = this.status,
				minPrice = minPrice,
				maxPrice = maxPrice
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			System.Data.DataTable products2 = PointExChangeHelper.GetProducts(this.eId);
			dataTable.Columns.Add("ProductNumber");
			dataTable.Columns.Add("PointNumber");
			dataTable.Columns.Add("eachMaxNumber");
			dataTable.Columns.Add("seledStatus");
			dataTable.Columns.Add("canSelStatus");
			dataTable.Columns.Add("canChkStatus");
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["ProductNumber"] = 0;
					dataTable.Rows[i]["PointNumber"] = 0;
					dataTable.Rows[i]["eachMaxNumber"] = 0;
				}
			}
			if (products2 != null)
			{
				if (dataTable.Rows.Count > 0 && products2.Rows.Count > 0)
				{
					for (int j = 0; j < dataTable.Rows.Count; j++)
					{
						int num = int.Parse(dataTable.Rows[j]["ProductId"].ToString());
						if (products2.Select(string.Format("ProductId={0}", num)).Length > 0)
						{
							dataTable.Rows[j]["seledStatus"] = "''";
							dataTable.Rows[j]["canSelStatus"] = "none";
							dataTable.Rows[j]["canChkStatus"] = "disabled";
							PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(exchangeId, num);
							if (productInfo != null)
							{
								dataTable.Rows[j]["ProductNumber"] = productInfo.ProductNumber.ToString();
								dataTable.Rows[j]["PointNumber"] = productInfo.PointNumber.ToString();
								dataTable.Rows[j]["eachMaxNumber"] = productInfo.EachMaxNumber.ToString();
							}
						}
						else
						{
							dataTable.Rows[j]["seledStatus"] = "none";
							dataTable.Rows[j]["canSelStatus"] = "''";
							dataTable.Rows[j]["canChkStatus"] = string.Empty;
						}
					}
				}
				else if (dataTable.Rows.Count > 0)
				{
					for (int k = 0; k < dataTable.Rows.Count; k++)
					{
						dataTable.Rows[k]["seledStatus"] = "none";
						dataTable.Rows[k]["canSelStatus"] = "''";
						dataTable.Rows[k]["canChkStatus"] = string.Empty;
					}
				}
			}
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = products.TotalRecords;
			this.lbsaleNumber.Text = products.TotalRecords.ToString();
			this.lbSelectNumber.Text = ((products2 != null) ? products2.Rows.Count.ToString() : "0");
			this.setInStock();
		}

		private void setInStock()
		{
			System.Data.DataTable productNum = ProductHelper.GetProductNum();
			this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
		}

		private System.Data.DataTable GetSelectedProducts(int exchangeId)
		{
			return CouponHelper.GetCouponProducts(exchangeId);
		}
	}
}
