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
	[PrivilegeCheck(Privilege.AddProductCategory)]
	public class CategoryEdit : AdminPage
	{
		protected string reurl = Globals.RequestQueryStr("reurl");

		protected int categoryid = Globals.RequestQueryNum("categoryId");

		protected string operatorName = "添加";

		protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;

		protected System.Web.UI.WebControls.TextBox txtCategoryName;

		protected UpImg uploader1;

		protected ProductCategoriesDropDownList dropCategories;

		protected ProductTypeDownList dropProductTypes;

		protected System.Web.UI.WebControls.TextBox txtSKUPrefix;

		protected System.Web.UI.WebControls.TextBox txtthird;

		protected System.Web.UI.WebControls.TextBox txtsecond;

		protected System.Web.UI.WebControls.TextBox txtfirst;

		protected System.Web.UI.HtmlControls.HtmlGenericControl liURL;

		protected System.Web.UI.WebControls.TextBox txtRewriteName;

		protected System.Web.UI.WebControls.TextBox txtPageKeyTitle;

		protected System.Web.UI.WebControls.TextBox txtPageKeyWords;

		protected System.Web.UI.WebControls.TextBox txtPageDesc;

		protected System.Web.UI.WebControls.Button btnSaveCategory;

		protected System.Web.UI.WebControls.Button btnSaveAddCategory;

		protected CategoryEdit() : base("m02", "spp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.reurl))
			{
				this.reurl = "managecategories.aspx";
			}
			this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
			this.btnSaveAddCategory.Click += new System.EventHandler(this.btnSaveAddCategory_Click);
			if (this.categoryid > 0)
			{
				this.operatorName = "编辑";
				this.btnSaveAddCategory.Visible = false;
			}
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			if (flag)
			{
				int categoryId = 0;
				int.TryParse(base.Request["parentCategoryId"], out categoryId);
				CategoryInfo category = CatalogHelper.GetCategory(categoryId);
				if (category != null)
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{ ");
					base.Response.Write(string.Format("\"SKUPrefix\":\"{0}\",\"f\":\"{1}\",\"s\":\"{2}\",\"t\":\"{3}\"", new object[]
					{
						category.SKUPrefix,
						category.FirstCommission,
						category.SecondCommission,
						category.ThirdCommission
					}));
					base.Response.Write("}");
					base.Response.End();
				}
			}
			if (!this.Page.IsPostBack)
			{
				if (this.categoryid > 0)
				{
					CategoryInfo category2 = CatalogHelper.GetCategory(this.categoryid);
					this.dropProductTypes.DataBind();
					this.dropProductTypes.SelectedValue = category2.AssociatedProductType;
					if (category2 == null)
					{
						base.GotoResourceNotFound();
						return;
					}
					Globals.EntityCoding(category2, false);
					this.BindCategoryInfo(category2);
					if (category2.Depth > 1)
					{
						this.liURL.Style.Add("display", "none");
						return;
					}
				}
				else
				{
					this.dropProductTypes.DataBind();
					this.dropCategories.DataBind();
				}
			}
		}

		private void BindCategoryInfo(CategoryInfo categoryInfo)
		{
			if (categoryInfo != null)
			{
				this.txtCategoryName.Text = categoryInfo.Name;
				this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
				this.txtSKUPrefix.Text = categoryInfo.SKUPrefix;
				this.txtRewriteName.Text = categoryInfo.RewriteName;
				this.txtPageKeyTitle.Text = categoryInfo.MetaTitle;
				this.txtPageKeyWords.Text = categoryInfo.MetaKeywords;
				this.txtPageDesc.Text = categoryInfo.MetaDescription;
				this.txtfirst.Text = (string.IsNullOrEmpty(categoryInfo.FirstCommission) ? "0" : categoryInfo.FirstCommission);
				this.txtsecond.Text = (string.IsNullOrEmpty(categoryInfo.SecondCommission) ? "0" : categoryInfo.SecondCommission);
				this.txtthird.Text = (string.IsNullOrEmpty(categoryInfo.ThirdCommission) ? "0" : categoryInfo.ThirdCommission);
				int? parentCategoryId = categoryInfo.ParentCategoryId;
				if (parentCategoryId.GetValueOrDefault() > 0)
				{
					bool arg_107_0 = parentCategoryId.HasValue;
				}
				this.uploader1.UploadedImageUrl = categoryInfo.IconUrl;
			}
		}

		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			if (this.categoryid > 0)
			{
				CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
				if (category == null)
				{
					this.ShowMsg("编缉商品分类错误,未知", false);
					return;
				}
				string arg_2D_0 = string.Empty;
				category.IconUrl = this.uploader1.UploadedImageUrl;
				category.Name = this.txtCategoryName.Text;
				category.SKUPrefix = this.txtSKUPrefix.Text;
				category.RewriteName = this.txtRewriteName.Text;
				category.MetaTitle = this.txtPageKeyTitle.Text;
				category.MetaKeywords = this.txtPageKeyWords.Text;
				category.MetaDescription = this.txtPageDesc.Text;
				category.AssociatedProductType = this.dropProductTypes.SelectedValue;
				category.Notes1 = "";
				category.Notes2 = "";
				category.Notes3 = "";
				if (category.Depth > 1)
				{
					CategoryInfo category2 = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
					if (string.IsNullOrEmpty(category.Notes1))
					{
						category.Notes1 = category2.Notes1;
					}
					if (string.IsNullOrEmpty(category.Notes2))
					{
						category.Notes2 = category2.Notes2;
					}
					if (string.IsNullOrEmpty(category.Notes3))
					{
						category.Notes3 = category2.Notes3;
					}
				}
				ValidationResults validationResults = Validation.Validate<CategoryInfo>(category, new string[]
				{
					"ValCategory"
				});
				string text = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
					{
						text += Formatter.FormatErrorMessage(current.Message);
					}
					this.ShowMsg(text, false);
					return;
				}
				CategoryActionStatus categoryActionStatus = CatalogHelper.UpdateCategory(category);
				if (categoryActionStatus == CategoryActionStatus.Success)
				{
					this.ShowMsgAndReUrl(this.operatorName + "成功", true, this.reurl);
					return;
				}
				if (categoryActionStatus == CategoryActionStatus.UpdateParentError)
				{
					this.ShowMsg("不能自己成为自己的上级分类", false);
					return;
				}
				this.ShowMsg(this.operatorName + "商品分类错误", false);
				return;
			}
			else
			{
				CategoryInfo category3 = this.GetCategory();
				if (category3 == null)
				{
					return;
				}
				if (CatalogHelper.AddCategory(category3) == CategoryActionStatus.Success)
				{
					this.ShowMsgAndReUrl("成功" + this.operatorName + "了商品分类", true, this.reurl);
					return;
				}
				this.ShowMsg(this.operatorName + "失败", false);
				return;
			}
		}

		private void btnSaveAddCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category == null)
			{
				return;
			}
			if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
			{
				this.ShowMsgAndReUrl(this.operatorName + "成功", true, "categoryedit.aspx");
				return;
			}
			this.ShowMsg(this.operatorName + "商品分类失败,未知错误", false);
		}

		private CategoryInfo GetCategory()
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			categoryInfo.IconUrl = this.uploader1.UploadedImageUrl;
			categoryInfo.Name = this.txtCategoryName.Text.Trim();
			categoryInfo.ParentCategoryId = this.dropCategories.SelectedValue;
			categoryInfo.SKUPrefix = this.txtSKUPrefix.Text.Trim();
			categoryInfo.AssociatedProductType = this.dropProductTypes.SelectedValue;
			if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
			{
				categoryInfo.RewriteName = this.txtRewriteName.Text.Trim();
			}
			else
			{
				categoryInfo.RewriteName = null;
			}
			categoryInfo.MetaTitle = this.txtPageKeyTitle.Text.Trim();
			categoryInfo.MetaKeywords = this.txtPageKeyWords.Text.Trim();
			categoryInfo.MetaDescription = this.txtPageDesc.Text.Trim();
			categoryInfo.Notes1 = "";
			categoryInfo.Notes2 = "";
			categoryInfo.Notes3 = "";
			categoryInfo.DisplaySequence = 0;
			if (categoryInfo.ParentCategoryId.HasValue)
			{
				CategoryInfo category = CatalogHelper.GetCategory(categoryInfo.ParentCategoryId.Value);
				if (category == null || category.Depth >= 5)
				{
					this.ShowMsg(string.Format("您选择的上级分类有误，商品分类最多只支持{0}级分类", 5), false);
					return null;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes1))
				{
					categoryInfo.Notes1 = category.Notes1;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes2))
				{
					categoryInfo.Notes2 = category.Notes2;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes3))
				{
					categoryInfo.Notes3 = category.Notes3;
				}
				if (string.IsNullOrEmpty(categoryInfo.RewriteName))
				{
					categoryInfo.RewriteName = category.RewriteName;
				}
			}
			if (string.IsNullOrEmpty(this.txtCategoryName.Text.Trim()))
			{
				this.ShowMsg("分类名称不能为空！", false);
				return null;
			}
			string text = Globals.RequestFormStr(this.txtfirst.ClientID.Replace("_", "$"));
			string text2 = Globals.RequestFormStr(this.txtsecond.ClientID.Replace("_", "$"));
			string text3 = Globals.RequestFormStr(this.txtthird.ClientID.Replace("_", "$"));
			if (string.IsNullOrEmpty(text))
			{
				text = "0";
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "0";
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "0";
			}
			categoryInfo.FirstCommission = text;
			categoryInfo.SecondCommission = text2;
			categoryInfo.ThirdCommission = text3;
			bool flag = false;
			if (System.Convert.ToDecimal(categoryInfo.FirstCommission) < 0m || System.Convert.ToDecimal(categoryInfo.FirstCommission) > 100m)
			{
				this.ShowMsg("输入的佣金格式不正确！", false);
				flag = true;
			}
			if (System.Convert.ToDecimal(categoryInfo.SecondCommission) < 0m || System.Convert.ToDecimal(categoryInfo.SecondCommission) > 100m)
			{
				this.ShowMsg("输入的佣金格式不正确！", false);
				flag = true;
			}
			if (System.Convert.ToDecimal(categoryInfo.ThirdCommission) < 0m || System.Convert.ToDecimal(categoryInfo.ThirdCommission) > 100m)
			{
				this.ShowMsg("输入的佣金格式不正确！", false);
				flag = true;
			}
			if (flag)
			{
				return null;
			}
			ValidationResults validationResults = Validation.Validate<CategoryInfo>(categoryInfo, new string[]
			{
				"ValCategory"
			});
			string text4 = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text4 += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text4, false);
				return null;
			}
			return categoryInfo;
		}
	}
}
