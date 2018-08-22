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
	public class LimitedTimeDiscountProduct : AdminPage
	{
		protected string actionName;

		protected int id;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected LimitedTimeDiscountProduct() : base("m08", "yxp24")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdProducts.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
			if (!base.IsPostBack)
			{
				this.dropCategories.IsUnclassified = true;
				this.dropCategories.DataBind();
				this.DataBindDiscount();
			}
		}

		private void grdProducts_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "MoveProduct")
			{
				object commandArgument = e.CommandArgument;
				if (!string.IsNullOrEmpty(commandArgument.ToString()))
				{
					bool flag = LimitedTimeDiscountHelper.DeleteDiscountProduct(commandArgument.ToString());
					if (flag)
					{
						this.ShowMsgAndReUrl("移除成功", true, "LimitedTimeDiscountProduct.aspx?id=" + Globals.RequestQueryNum("id"));
					}
				}
			}
			if (e.CommandName == "Stop")
			{
				int num = 0;
				if (int.TryParse(e.CommandArgument.ToString(), out num))
				{
					LimitedTimeDiscountProductInfo discountProductInfoById = LimitedTimeDiscountHelper.GetDiscountProductInfoById(num);
					int status = (discountProductInfoById.Status == 3) ? 1 : 3;
					if (!string.IsNullOrEmpty(num.ToString()))
					{
						bool flag2 = LimitedTimeDiscountHelper.ChangeDiscountProductStatus(num.ToString(), status);
						if (flag2)
						{
							this.ShowMsgAndReUrl("状态修改成功", true, "LimitedTimeDiscountProduct.aspx?id=" + Globals.RequestQueryNum("id"));
						}
					}
				}
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
				SortOrder = SortAction.Desc
			};
			if (this.dropCategories.SelectedValue.HasValue && this.dropCategories.SelectedValue > 0)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.dropCategories.SelectedValue.Value).Path;
			}
			DbQueryResult discountProducted = LimitedTimeDiscountHelper.GetDiscountProducted(productQuery, this.id);
			this.grdProducts.DataSource = discountProducted.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = discountProducted.TotalRecords;
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

		protected string GetStatus(string status)
		{
			string result = "";
			if (status != null)
			{
				if (!(status == "1"))
				{
					if (!(status == "2"))
					{
						if (status == "3")
						{
							result = "已暂停";
						}
					}
					else
					{
						result = "删除";
					}
				}
				else
				{
					result = "进行中";
				}
			}
			return result;
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.DataBindDiscount();
		}
	}
}
