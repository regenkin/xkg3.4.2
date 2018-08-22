using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddProductType : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtTypeName;

		protected BrandCategoriesCheckBoxList chlistBrand;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.WebControls.Button btnAddProductType;

		protected AddProductType() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddProductType.Click += new System.EventHandler(this.btnAddProductType_Click);
			if (!base.IsPostBack)
			{
				this.chlistBrand.DataBind();
			}
		}

		private void btnAddProductType_Click(object sender, System.EventArgs e)
		{
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeName = this.txtTypeName.Text.Trim();
			productTypeInfo.Remark = this.txtRemark.Text;
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistBrand.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			productTypeInfo.Brands = list;
			if (!this.ValidationProductType(productTypeInfo))
			{
				return;
			}
			int num = ProductTypeHelper.AddProductType(productTypeInfo);
			if (num > 0)
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/goods/AddAttribute.aspx?typeId=" + num), true);
				return;
			}
			this.ShowMsg("添加商品类型失败", false);
		}

		private bool ValidationProductType(ProductTypeInfo productType)
		{
			ValidationResults validationResults = Validation.Validate<ProductTypeInfo>(productType, new string[]
			{
				"ValProductType"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
