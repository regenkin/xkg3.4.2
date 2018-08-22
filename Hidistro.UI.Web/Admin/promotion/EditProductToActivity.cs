using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class EditProductToActivity : AdminPage
	{
		protected ProductSaleStatus _status = ProductSaleStatus.All;

		protected int _id;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblJoin;

		protected System.Web.UI.WebControls.Label lbsaleNumber;

		protected System.Web.UI.WebControls.Label lbwareNumber;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minPrice;

		protected System.Web.UI.WebControls.TextBox txt_maxPrice;

		protected System.Web.UI.WebControls.Button btnQuery;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected EditProductToActivity() : base("m08", "yxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && !this.bInt(base.Request["id"].ToString(), ref this._id))
			{
				this._id = 0;
			}
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			if (!base.IsPostBack)
			{
				this.BindProducts(this._id);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.BindProducts(this._id);
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private void BindProducts(int couponId)
		{
			System.Data.DataTable selectedProducts = this.GetSelectedProducts(couponId);
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
				SaleStatus = this._status,
				minPrice = minPrice,
				maxPrice = maxPrice,
				selectQuery = string.Format("productId in (select productId from Hishop_Activities_Product where ActivitiesId={0})", this._id)
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			dataTable.Columns.Add("setStatus");
			if (dataTable != null && selectedProducts != null && dataTable.Rows.Count > 0 && selectedProducts.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int num = int.Parse(dataTable.Rows[i]["ProductId"].ToString());
					System.Data.DataRow[] array = selectedProducts.Select(string.Format("ProductId={0}", num));
					if (array.Length > 0)
					{
						dataTable.Rows[i]["setStatus"] = ((array[0]["status"].ToString() == "0") ? "暂停" : "恢复");
					}
				}
			}
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = products.TotalRecords;
			this.lblJoin.Text = ((selectedProducts != null) ? selectedProducts.Rows.Count.ToString() : "0");
			this.setInSaleAndStock();
		}

		private void setInSaleAndStock()
		{
			System.Data.DataTable productNum = ProductHelper.GetProductNum();
			this.lbsaleNumber.Text = productNum.Rows[0]["OnSale"].ToString();
			this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
		}

		private System.Data.DataTable GetSelectedProducts(int actId)
		{
			return ActivityHelper.QueryProducts(actId);
		}
	}
}
