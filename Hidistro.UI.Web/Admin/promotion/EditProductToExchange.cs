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
	public class EditProductToExchange : AdminPage
	{
		protected ProductSaleStatus status = ProductSaleStatus.All;

		protected int exchangeId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lbSelectNumber;

		protected System.Web.UI.WebControls.Label lbsaleNumber;

		protected System.Web.UI.WebControls.Label lbwareNumber;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minPrice;

		protected System.Web.UI.WebControls.TextBox txt_maxPrice;

		protected System.Web.UI.WebControls.Button btnQuery;

		protected System.Web.UI.WebControls.Button btnBatchStop;

		protected System.Web.UI.WebControls.Button btnBatchOpen;

		protected System.Web.UI.WebControls.Button btnBatchRemove;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected EditProductToExchange() : base("m08", "yxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && !this.bInt(base.Request["id"].ToString(), ref this.exchangeId))
			{
				this.exchangeId = 0;
			}
			if (!base.IsPostBack)
			{
				this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
				this.BindProducts(this.exchangeId);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.BindProducts(this.exchangeId);
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private void BindProducts(int eId)
		{
			if (this.exchangeId == 0)
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
				maxPrice = maxPrice,
				selectQuery = string.Format(" ProductId  in (  select ProductId from Hishop_PointExChange_Products where exChangeId={0})", this.exchangeId)
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			dataTable.Columns.Add("ProductNumber");
			dataTable.Columns.Add("PointNumber");
			dataTable.Columns.Add("eachMaxNumber");
			dataTable.Columns.Add("status");
			dataTable.Columns.Add("seledStatus");
			dataTable.Columns.Add("canSelStatus");
			dataTable.Columns.Add("canChkStatus");
			System.Data.DataTable products2 = PointExChangeHelper.GetProducts(this.exchangeId);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
				{
					string str = dataTable.Rows[i]["ProductId"].ToString();
					System.Data.DataRow[] array = products2.Select(" productId='" + str + "'");
					if (array.Length > 0)
					{
						dataTable.Rows[i]["ProductNumber"] = array[0]["ProductNumber"];
						dataTable.Rows[i]["PointNumber"] = array[0]["PointNumber"];
						dataTable.Rows[i]["eachMaxNumber"] = array[0]["eachMaxNumber"];
						dataTable.Rows[i]["status"] = array[0]["status"];
					}
					else
					{
						dataTable.Rows.RemoveAt(i);
					}
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
			this.lbSelectNumber.Text = ((products2 != null) ? products2.Rows.Count.ToString() : "0");
			this.setInSaleAndStock();
		}

		private void setInSaleAndStock()
		{
			System.Data.DataTable productNum = ProductHelper.GetProductNum();
			this.lbsaleNumber.Text = productNum.Rows[0]["OnSale"].ToString();
			this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
		}

		protected void grdProducts_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			string commandName = e.CommandName;
			if (commandName == "Renew")
			{
				if (!string.IsNullOrEmpty(base.Request["id"].ToString()))
				{
					int num = int.Parse(base.Request["id"]);
					string productIds = e.CommandArgument.ToString();
					bool flag = PointExChangeHelper.SetProductsStatus(num, 0, productIds);
					if (flag)
					{
						this.ShowMsg("恢复商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("恢复商品失败", false);
					return;
				}
			}
			else if (commandName == "Pause")
			{
				if (!string.IsNullOrEmpty(base.Request["id"].ToString()))
				{
					int num2 = int.Parse(base.Request["id"]);
					string productIds2 = e.CommandArgument.ToString();
					bool flag2 = PointExChangeHelper.SetProductsStatus(num2, 1, productIds2);
					if (flag2)
					{
						this.ShowMsg("暂停商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("暂停商品失败", false);
					return;
				}
			}
			else if (commandName == "Delete" && !string.IsNullOrEmpty(base.Request["id"].ToString()))
			{
				int num3 = int.Parse(base.Request["id"]);
				string text = e.CommandArgument.ToString();
				if (!string.IsNullOrEmpty(text))
				{
					int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(num3, int.Parse(text));
					if (productExchangedCount > 0)
					{
						this.ShowMsg("该商品已经存在兑换记录，不能移除!", false);
						return;
					}
					bool flag3 = PointExChangeHelper.DeleteProducts(num3, text);
					if (flag3)
					{
						this.ShowMsg("删除商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("删除商品失败", false);
				}
			}
		}

		protected void btnBatchStop_Click(object sender, System.EventArgs e)
		{
			if (base.Request.Form["CheckBoxGroup"] != null)
			{
				string productIds = base.Request.Form["CheckBoxGroup"].ToString();
				if (!string.IsNullOrEmpty(base.Request["id"].ToString()))
				{
					int num = int.Parse(base.Request["id"]);
					bool flag = PointExChangeHelper.SetProductsStatus(num, 1, productIds);
					if (flag)
					{
						this.ShowMsg("暂停商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("暂停商品失败", false);
					return;
				}
			}
			else
			{
				this.ShowMsg("请先选择商品", false);
			}
		}

		protected void btnBatchOpen_Click(object sender, System.EventArgs e)
		{
			if (base.Request.Form["CheckBoxGroup"] != null)
			{
				string productIds = base.Request.Form["CheckBoxGroup"].ToString();
				if (!string.IsNullOrEmpty(base.Request["id"].ToString()))
				{
					int num = int.Parse(base.Request["id"]);
					bool flag = PointExChangeHelper.SetProductsStatus(num, 0, productIds);
					if (flag)
					{
						this.ShowMsg("恢复商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("恢复商品失败", false);
					return;
				}
			}
			else
			{
				this.ShowMsg("请先选择商品", false);
			}
		}

		protected void btnBatchRemove_Click(object sender, System.EventArgs e)
		{
			if (base.Request.Form["CheckBoxGroup"] != null)
			{
				string productIds = base.Request.Form["CheckBoxGroup"].ToString();
				if (!string.IsNullOrEmpty(base.Request["id"].ToString()))
				{
					int num = int.Parse(base.Request["id"]);
					bool flag = PointExChangeHelper.DeleteProducts(num, productIds);
					if (flag)
					{
						this.ShowMsg("移除商品成功", true);
						this.BindProducts(this.exchangeId);
						return;
					}
					this.ShowMsg("移除商品失败", false);
					return;
				}
			}
			else
			{
				this.ShowMsg("请先选择商品", false);
			}
		}
	}
}
