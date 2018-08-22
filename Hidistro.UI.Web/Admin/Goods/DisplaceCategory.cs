using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.goods
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class DisplaceCategory : AdminPage
	{
		protected ProductCategoriesDropDownList dropCategoryFrom;

		protected ProductCategoriesDropDownList dropCategoryTo;

		protected System.Web.UI.WebControls.Button btnSaveCategory;

		protected DisplaceCategory() : base("m02", "spp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategoryFrom.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
					{
						this.dropCategoryFrom.SelectedValue = new int?(value);
					}
				}
			}
		}

		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			if (!this.dropCategoryFrom.SelectedValue.HasValue || !this.dropCategoryTo.SelectedValue.HasValue)
			{
				this.ShowMsgToTarget("请选择需要转移商品的分类或需要转移至的商品分类！", false, "parent");
				return;
			}
			if (this.dropCategoryFrom.SelectedValue.Value == this.dropCategoryTo.SelectedValue.Value)
			{
				this.ShowMsgToTarget("请选择不同的商品分类进行转移！", false, "parent");
				return;
			}
			if (CatalogHelper.DisplaceCategory(this.dropCategoryFrom.SelectedValue.Value, this.dropCategoryTo.SelectedValue.Value) == 0)
			{
				this.ShowMsgToTarget("此分类下没有可以转移的商品！", false, "parent");
				return;
			}
			HiCache.Remove("DataCache-Categories");
			HiCache.Remove("DataCache-CategoryList");
			this.ShowMsgToTarget("商品批量转移成功！", true, "parent");
		}
	}
}
