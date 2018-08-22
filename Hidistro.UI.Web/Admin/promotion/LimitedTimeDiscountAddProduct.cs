using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class LimitedTimeDiscountAddProduct : AdminPage
	{
		protected string actionName;

		protected int id;

		private int? categoryId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected LimitedTimeDiscountAddProduct() : base("m08", "yxp24")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.dropCategories.IsUnclassified = true;
				this.dropCategories.DataBind();
				this.DataBindDiscount();
			}
		}

		private void DataBindDiscount()
		{
			this.id = Globals.RequestQueryNum("id");
			if (this.id > 0)
			{
				LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(this.id);
				if (discountInfo != null)
				{
					this.actionName = discountInfo.ActivityName;
				}
			}
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.txtProductName.Text,
				ProductCode = "",
				CategoryId = this.dropCategories.SelectedValue,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence"
			};
			if (this.dropCategories.SelectedValue.HasValue && this.dropCategories.SelectedValue > 0)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.dropCategories.SelectedValue.Value).Path;
			}
			DbQueryResult discountProduct = LimitedTimeDiscountHelper.GetDiscountProduct(productQuery);
			this.grdProducts.DataSource = discountProduct.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = discountProduct.TotalRecords;
		}

		protected string GetDisable(string ActivityName, object limitedTimeDiscountId, int discountId)
		{
			if (!string.IsNullOrEmpty(ActivityName) && Globals.ToNum(limitedTimeDiscountId) != discountId)
			{
				return "disabled";
			}
			return "";
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.DataBindDiscount();
		}

		private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.DataBindDiscount();
		}

		protected string GetDisplayValue(object obj)
		{
			decimal d;
			if (!decimal.TryParse(obj.ToString(), out d))
			{
				return "none";
			}
			if (d > 0m)
			{
				return "";
			}
			return "none";
		}

		protected string GetDisplay(object obj)
		{
			if (!string.IsNullOrEmpty(obj.ToString()))
			{
				return "";
			}
			return "none";
		}
	}
}
