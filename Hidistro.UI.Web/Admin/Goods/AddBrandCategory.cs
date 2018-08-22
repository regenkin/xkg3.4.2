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
	public class AddBrandCategory : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtBrandName;

		protected UpImg uploader1;

		protected System.Web.UI.WebControls.TextBox txtCompanyUrl;

		protected System.Web.UI.WebControls.TextBox txtReUrl;

		protected System.Web.UI.WebControls.TextBox txtkeyword;

		protected System.Web.UI.WebControls.TextBox txtMetaDescription;

		protected ucUeditor fckDescription;

		protected ProductTypesCheckBoxList chlistProductTypes;

		protected System.Web.UI.WebControls.Button btnSave;

		protected System.Web.UI.WebControls.Button btnAddBrandCategory;

		protected AddBrandCategory() : base("m02", "spp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnAddBrandCategory.Click += new System.EventHandler(this.btnAddBrandCategory_Click);
			if (!base.IsPostBack)
			{
				this.chlistProductTypes.DataBind();
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			if (string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
			{
				this.ShowMsg("请选择图片上传！", false);
				return;
			}
			brandCategoryInfo.Logo = this.uploader1.UploadedImageUrl;
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
			if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/Goods/BrandCategories.aspx"), true);
				return;
			}
			this.ShowMsg("添加品牌分类失败", true);
		}

		protected void btnAddBrandCategory_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			if (string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
			{
				this.ShowMsg("请上传图片！", false);
				return;
			}
			brandCategoryInfo.Logo = this.uploader1.UploadedImageUrl;
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
			if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
			{
				this.txtBrandName.Text = "";
				this.txtCompanyUrl.Text = "";
				this.txtkeyword.Text = "";
				this.txtMetaDescription.Text = "";
				this.txtReUrl.Text = "";
				this.fckDescription.Text = "";
				this.ShowMsg("成功添加品牌分类", true);
				return;
			}
			this.ShowMsg("添加品牌分类失败", true);
		}

		private BrandCategoryInfo GetBrandCategoryInfo()
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
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
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			brandCategoryInfo.ProductTypes = list;
			brandCategoryInfo.Description = ((!string.IsNullOrEmpty(this.fckDescription.Text) && this.fckDescription.Text.Length > 0) ? this.fckDescription.Text : null);
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
