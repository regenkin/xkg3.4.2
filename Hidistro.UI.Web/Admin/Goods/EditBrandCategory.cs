using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class EditBrandCategory : AdminPage
	{
		private int brandId;

		protected System.Web.UI.WebControls.TextBox txtBrandName;

		protected UpImg uploader1;

		protected System.Web.UI.WebControls.TextBox txtCompanyUrl;

		protected System.Web.UI.WebControls.TextBox txtReUrl;

		protected System.Web.UI.WebControls.TextBox txtkeyword;

		protected System.Web.UI.WebControls.TextBox txtMetaDescription;

		protected ucUeditor fckDescription;

		protected ProductTypesCheckBoxList chlistProductTypes;

		protected System.Web.UI.WebControls.Button btnUpdateBrandCategory;

		protected EditBrandCategory() : base("m02", "spp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpdateBrandCategory.Click += new System.EventHandler(this.btnUpdateBrandCategory_Click);
			if (!this.Page.IsPostBack)
			{
				this.chlistProductTypes.DataBind();
				this.loadData();
			}
		}

		private void loadData()
		{
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
			if (brandCategory == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.ViewState["Logo"] = brandCategory.Logo;
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (brandCategory.ProductTypes.Contains(int.Parse(listItem.Value)))
				{
					listItem.Selected = true;
				}
			}
			this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
			this.txtCompanyUrl.Text = brandCategory.CompanyUrl;
			this.txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
			this.txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
			this.txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
			this.fckDescription.Text = brandCategory.Description;
			if (!string.IsNullOrEmpty(brandCategory.Logo))
			{
				this.uploader1.UploadedImageUrl = brandCategory.Logo;
			}
		}

		protected void btnUpdateBrandCategory_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			if (string.IsNullOrEmpty(brandCategoryInfo.Logo))
			{
				this.ShowMsg("请上传一张品牌LOGO图片", false);
				return;
			}
			if (string.IsNullOrEmpty(brandCategoryInfo.BrandName))
			{
				this.ShowMsg("请输入品牌名称！", false);
				return;
			}
			if (string.IsNullOrEmpty(brandCategoryInfo.Description))
			{
				this.ShowMsg("请输入品牌介绍！", false);
				return;
			}
			if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/Goods/BrandCategories.aspx"), true);
				return;
			}
			this.ShowMsg("编辑品牌分类失败", true);
		}

		private BrandCategoryInfo GetBrandCategoryInfo()
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			brandCategoryInfo.BrandId = this.brandId;
			brandCategoryInfo.Logo = this.uploader1.UploadedImageUrl;
			brandCategoryInfo.BrandName = Globals.HtmlEncode(this.txtBrandName.Text.Trim());
			if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
			{
				brandCategoryInfo.CompanyUrl = this.txtCompanyUrl.Text.Trim();
			}
			else
			{
				brandCategoryInfo.CompanyUrl = null;
			}
			brandCategoryInfo.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
			brandCategoryInfo.MetaKeywords = Globals.HtmlEncode(this.txtkeyword.Text.Trim());
			brandCategoryInfo.MetaDescription = Globals.HtmlEncode(this.txtMetaDescription.Text.Trim());
			brandCategoryInfo.Description = ((!string.IsNullOrEmpty(this.fckDescription.Text) && this.fckDescription.Text.Length > 0) ? this.fckDescription.Text : null);
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			brandCategoryInfo.ProductTypes = list;
			return brandCategoryInfo;
		}

		private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
		{
			ValidationResults validationResults = Validation.Validate<BrandCategoryInfo>(brandCategory, new string[]
			{
				"ValBrandCategory"
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
