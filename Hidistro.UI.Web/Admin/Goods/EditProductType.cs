using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.EditProductType)]
	public class EditProductType : AdminPage
	{
		private int typeId;

		protected System.Web.UI.WebControls.TextBox txtTypeName;

		protected BrandCategoriesCheckBoxList chlistBrand;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.WebControls.Button btnEditProductType;

		protected EditProductType() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
			{
				int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
			}
			this.btnEditProductType.Click += new System.EventHandler(this.btnEditProductType_Click);
			if (!this.Page.IsPostBack)
			{
				this.chlistBrand.DataBind();
				ProductTypeInfo productType = ProductTypeHelper.GetProductType(this.typeId);
				if (productType == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.txtTypeName.Text = productType.TypeName;
				this.txtRemark.Text = productType.Remark;
				foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistBrand.Items)
				{
					if (productType.Brands.Contains(int.Parse(listItem.Value)))
					{
						listItem.Selected = true;
					}
				}
			}
		}

		private void btnEditProductType_Click(object sender, System.EventArgs e)
		{
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeId = this.typeId;
			productTypeInfo.TypeName = this.txtTypeName.Text;
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
			if (ProductTypeHelper.UpdateProductType(productTypeInfo))
			{
				this.ShowMsg("修改成功", true);
			}
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
