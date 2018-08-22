using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.AddProductCategory)]
	public class ProductEdit : ProductBasePage
	{
		protected int isnext = Globals.RequestQueryNum("isnext");

		protected string reurl = Globals.RequestQueryStr("reurl");

		protected int categoryid = Globals.RequestQueryNum("categoryId");

		protected string operatorName = "新增";

		protected int productId;

		private string thisUrl = string.Empty;

		protected Script Script2;

		protected Script Script1;

		protected Script Script4;

		protected Script Script3;

		protected System.Web.UI.WebControls.Literal litCategoryName;

		protected System.Web.UI.WebControls.HyperLink lnkEditCategory;

		protected ProductTypeDownList dropProductTypes;

		protected BrandCategoriesDropDownList dropBrandCategories;

		protected TrimTextBox txtProductName;

		protected TrimTextBox txtProductShortName;

		protected TrimTextBox txtDisplaySequence;

		protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;

		protected ProductTagsLiteral litralProductTag;

		protected TrimTextBox txtProductTag;

		protected TrimTextBox txtShortDescription;

		protected ProductFlashUpload ucFlashUpload1;

		protected TrimTextBox txtAttributes;

		protected TrimTextBox txtSkus;

		protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;

		protected TrimTextBox txtMarketPrice;

		protected TrimTextBox txtSalePrice;

		protected TrimTextBox txtMemberPrices;

		protected TrimTextBox txtStock;

		protected System.Web.UI.WebControls.HiddenField hdfSKUPrefix;

		protected TrimTextBox txtProductCode;

		protected TrimTextBox txtCostPrice;

		protected TrimTextBox txtUnit;

		protected TrimTextBox txtSku;

		protected TrimTextBox txtShowSaleCounts;

		protected System.Web.UI.WebControls.CheckBox cbIsSetCommission;

		protected TrimTextBox txtThirdCommission;

		protected TrimTextBox txtSecondCommission;

		protected TrimTextBox txtFirstCommission;

		protected TrimTextBox txtCubicMeter;

		protected TrimTextBox txtWeight;

		protected TrimTextBox txtFreightWeight;

		protected System.Web.UI.WebControls.RadioButton ChkisfreeShipping;

		protected System.Web.UI.WebControls.RadioButton rbtIsSetTemplate;

		protected FreightTemplateDownList FreightTemplateDownList1;

		protected System.Web.UI.WebControls.RadioButton radOnSales;

		protected System.Web.UI.WebControls.RadioButton radInStock;

		protected System.Web.UI.WebControls.RadioButton radUnSales;

		protected System.Web.UI.WebControls.Button btnNext;

		protected ucUeditor fckDescription;

		protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;

		protected System.Web.UI.WebControls.Button btnSave;

		protected System.Web.UI.HtmlControls.HtmlGenericControl spanJs;

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("gettype");
			if (a == "getcategorycommission")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"获取失败！\"}";
				this.categoryid = Globals.RequestFormNum("categoryId");
				if (this.categoryid > 0)
				{
					CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
					if (category != null)
					{
						s = string.Concat(new string[]
						{
							"{\"type\":\"1\",\"f\":\"",
							category.FirstCommission,
							"\",\"s\":\"",
							category.SecondCommission,
							"\",\"t\":\"",
							category.ThirdCommission,
							"\"}"
						});
					}
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			if (string.IsNullOrEmpty(this.reurl))
			{
				this.reurl = "productonsales.aspx";
			}
			this.thisUrl = base.Request.Url.ToString().ToLower();
			bool flag = !string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true";
			if (flag)
			{
				base.DoCallback();
				return;
			}
			this.productId = Globals.RequestQueryNum("productId");
			if (this.productId == 0 && this.isnext == 1)
			{
				this.thisUrl = this.thisUrl.Replace("isnext=1", "").Replace("&&", "&");
				base.Response.Redirect(this.thisUrl);
				base.Response.End();
			}
			if (this.productId > 0)
			{
				this.operatorName = "修改";
				string a2 = Globals.RequestFormStr("posttype");
				if (a2 == "updatecontent")
				{
					base.Response.ContentType = "application/json";
					string s2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
					string content = Globals.RequestFormStr("memo");
					string s3 = ProductHelper.UpdateProductContent(this.productId, content);
					if (Globals.ToNum(s3) > 0)
					{
						s2 = "{\"type\":\"1\",\"tips\":\"商品信息保存成功！\"}";
					}
					base.Response.Write(s2);
					base.Response.End();
				}
			}
			if (!this.Page.IsPostBack)
			{
				this.FreightTemplateDownList1.DataBind();
				System.Collections.Generic.IList<int> list = null;
				if (this.productId > 0)
				{
					System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs;
					ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out attrs, out list);
					if (productDetails == null)
					{
						base.GotoResourceNotFound();
						return;
					}
					if (this.categoryid > 0)
					{
						this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryid);
						this.ViewState["ProductCategoryId"] = this.categoryid;
						this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryid.ToString(System.Globalization.CultureInfo.InvariantCulture) + "&reurl=" + base.Server.UrlEncode(this.reurl);
					}
					else
					{
						this.litCategoryName.Text = CatalogHelper.GetFullCategory(productDetails.CategoryId);
						this.categoryid = productDetails.CategoryId;
						this.ViewState["ProductCategoryId"] = productDetails.CategoryId;
						this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + productDetails.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
					}
					System.Web.UI.WebControls.HyperLink expr_35F = this.lnkEditCategory;
					string navigateUrl = expr_35F.NavigateUrl;
					expr_35F.NavigateUrl = string.Concat(new string[]
					{
						navigateUrl,
						"&productId=",
						productDetails.ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"&reurl=",
						base.Server.UrlEncode(this.reurl)
					});
					this.litralProductTag.SelectedValue = list;
					if (list.Count > 0)
					{
						foreach (int current in list)
						{
							TrimTextBox expr_3F2 = this.txtProductTag;
							expr_3F2.Text = expr_3F2.Text + current.ToString() + ",";
						}
						this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
					}
					this.dropProductTypes.DataBind();
					this.dropBrandCategories.DataBind();
					this.LoadProduct(productDetails, attrs);
					return;
				}
				else if (this.categoryid > 0)
				{
					this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryid);
					this.ViewState["ProductCategoryId"] = this.categoryid;
					this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryid.ToString(System.Globalization.CultureInfo.InvariantCulture);
					this.dropProductTypes.DataBind();
					this.dropBrandCategories.DataBind();
					CategoryInfo category2 = CatalogHelper.GetCategory(this.categoryid);
					if (category2 != null)
					{
						this.txtFirstCommission.Text = category2.FirstCommission;
						this.txtSecondCommission.Text = category2.SecondCommission;
						this.txtThirdCommission.Text = category2.ThirdCommission;
						this.txtSku.Text = category2.SKUPrefix;
						this.txtProductCode.Text = category2.SKUPrefix;
						return;
					}
				}
				else
				{
					base.Response.Redirect("selectcategory.aspx");
					base.Response.End();
				}
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this.SubmitProduct("update");
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			this.SubmitProduct("next");
		}

		private void SubmitProduct(string opername)
		{
			string text = this.ucFlashUpload1.Value.Trim();
			this.ucFlashUpload1.Value = text;
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = new string[]
			{
				"",
				"",
				"",
				"",
				""
			};
			int num = 0;
			while (num < array.Length && num < 5)
			{
				array2[num] = array[num];
				num++;
			}
			if (this.categoryid == 0)
			{
				this.categoryid = (int)this.ViewState["ProductCategoryId"];
			}
			bool isSetCommission = !this.cbIsSetCommission.Checked;
			int showSaleCounts = 0;
			int displaySequence;
			decimal num2;
			decimal? num3;
			decimal? marketPrice;
			int stock;
			decimal? num4;
			decimal firstCommission;
			decimal secondCommission;
			decimal thirdCommission;
			decimal cubicMeter;
			decimal freightWeight;
			if (!this.ValidateConverts(this.txtProductName.Text.Trim(), this.chkSkuEnabled.Checked, out displaySequence, out num2, out num3, out marketPrice, out stock, out num4, out showSaleCounts, out firstCommission, out secondCommission, out thirdCommission, out cubicMeter, out freightWeight))
			{
				return;
			}
			Globals.Debuglog("商品规格：" + this.chkSkuEnabled.Checked.ToString(), "_Debuglog.txt");
			if (!this.chkSkuEnabled.Checked && num2 <= 0m)
			{
				this.ShowMsg("商品现价必须大于0", false);
				return;
			}
			string text2 = this.fckDescription.Text;
			if (this.ckbIsDownPic.Checked)
			{
				text2 = base.DownRemotePic(text2);
			}
			ProductInfo productInfo = new ProductInfo
			{
				ProductId = this.productId,
				CategoryId = this.categoryid,
				TypeId = this.dropProductTypes.SelectedValue,
				ProductName = this.txtProductName.Text.Trim().Replace("\\", ""),
				ProductShortName = this.txtProductShortName.Text.Trim(),
				ProductCode = this.txtProductCode.Text.Trim(),
				DisplaySequence = displaySequence,
				MarketPrice = marketPrice,
				Unit = this.txtUnit.Text.Trim(),
				ImageUrl1 = array2[0],
				ImageUrl2 = array2[1],
				ImageUrl3 = array2[2],
				ImageUrl4 = array2[3],
				ImageUrl5 = array2[4],
				ThumbnailUrl40 = array2[0].Replace("/images/", "/thumbs40/40_"),
				ThumbnailUrl60 = array2[0].Replace("/images/", "/thumbs60/60_"),
				ThumbnailUrl100 = array2[0].Replace("/images/", "/thumbs100/100_"),
				ThumbnailUrl160 = array2[0].Replace("/images/", "/thumbs160/160_"),
				ThumbnailUrl180 = array2[0].Replace("/images/", "/thumbs180/180_"),
				ThumbnailUrl220 = array2[0].Replace("/images/", "/thumbs220/220_"),
				ThumbnailUrl310 = array2[0].Replace("/images/", "/thumbs310/310_"),
				ThumbnailUrl410 = array2[0].Replace("/images/", "/thumbs410/410_"),
				ShortDescription = this.txtShortDescription.Text,
				IsfreeShipping = this.ChkisfreeShipping.Checked,
				Description = (!string.IsNullOrEmpty(text2) && text2.Length > 0) ? text2 : null,
				AddedDate = System.DateTime.Now,
				BrandId = this.dropBrandCategories.SelectedValue,
				FirstCommission = firstCommission,
				SecondCommission = secondCommission,
				ThirdCommission = thirdCommission,
				FreightTemplateId = this.ChkisfreeShipping.Checked ? 0 : this.FreightTemplateDownList1.SelectedValue,
				IsSetCommission = isSetCommission,
				CubicMeter = cubicMeter,
				FreightWeight = freightWeight
			};
			ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
			if (this.radInStock.Checked)
			{
				saleStatus = ProductSaleStatus.OnStock;
			}
			if (this.radUnSales.Checked)
			{
				saleStatus = ProductSaleStatus.UnSale;
			}
			if (this.radOnSales.Checked)
			{
				saleStatus = ProductSaleStatus.OnSale;
			}
			productInfo.SaleStatus = saleStatus;
			CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
			if (category != null)
			{
				productInfo.MainCategoryPath = category.Path + "|";
			}
			System.Collections.Generic.Dictionary<string, SKUItem> dictionary = null;
			System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
			if (this.chkSkuEnabled.Checked)
			{
				decimal minShowPrice = 0m;
				productInfo.HasSKU = true;
				dictionary = base.GetSkus(this.txtSkus.Text);
				if (dictionary == null)
				{
					this.ShowMsg("商品规格填写不完整！", false);
					return;
				}
				decimal[] minSalePrice = new decimal[]
				{
					79228162514264337593543950335m
				};
				foreach (SKUItem current in from sku in dictionary.Values
				where sku.SalePrice < minSalePrice[0]
				select sku)
				{
					minSalePrice[0] = current.SalePrice;
				}
				minShowPrice = minSalePrice[0];
				decimal[] maxSalePrice = new decimal[]
				{
					-79228162514264337593543950335m
				};
				foreach (SKUItem current2 in from sku in dictionary.Values
				where sku.SalePrice > maxSalePrice[0]
				select sku)
				{
					maxSalePrice[0] = current2.SalePrice;
				}
				decimal maxShowPrice = maxSalePrice[0];
				productInfo.MinShowPrice = minShowPrice;
				productInfo.MaxShowPrice = maxShowPrice;
			}
			else
			{
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>
				{
					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = this.txtSku.Text,
							SalePrice = num2,
							CostPrice = num3.HasValue ? num3.Value : 0m,
							Stock = stock,
							Weight = num4.HasValue ? num4.Value : 0m
						}
					}
				};
				if (this.txtMemberPrices.Text.Length > 0)
				{
					base.GetMemberPrices(dictionary["0"], this.txtMemberPrices.Text);
				}
				productInfo.MinShowPrice = num2;
				productInfo.MaxShowPrice = num2;
			}
			if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
			{
				attrs = base.GetAttributes(this.txtAttributes.Text);
			}
			ValidationResults validationResults = Validation.Validate<ProductInfo>(productInfo);
			if (!validationResults.IsValid)
			{
				this.ShowMsg(validationResults);
				return;
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
			{
				string text3 = this.txtProductTag.Text.Trim();
				string[] array3;
				if (text3.Contains(","))
				{
					array3 = text3.Split(new char[]
					{
						','
					});
				}
				else
				{
					array3 = new string[]
					{
						text3
					};
				}
				string[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					string value = array4[i];
					list.Add(System.Convert.ToInt32(value));
				}
			}
			if (this.productId > 0)
			{
				ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(this.productId);
				productInfo.SaleCounts = productBaseInfo.SaleCounts;
				productInfo.ShowSaleCounts = productBaseInfo.ShowSaleCounts;
			}
			else
			{
				productInfo.SaleCounts = 0;
				productInfo.ShowSaleCounts = showSaleCounts;
			}
			if (this.productId > 0)
			{
				ProductActionStatus productActionStatus = ProductHelper.UpdateProduct(productInfo, dictionary, attrs, list);
				if (productActionStatus == ProductActionStatus.Success)
				{
					this.litralProductTag.SelectedValue = list;
					if (opername == "next")
					{
						if (this.isnext != 1)
						{
							this.thisUrl = this.thisUrl.Replace("productid=" + this.productId, "productid=" + this.productId + "&isnext=1");
						}
						base.Response.Redirect(this.thisUrl);
						base.Response.End();
						return;
					}
					this.spanJs.InnerHtml = "<script>$('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').attr('disabled', 'true');setTimeout(function () { $('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').removeAttr('disabled'); }, 5000);</script>";
					this.ShowMsgAndReUrl(this.operatorName + "商品信息成功!", true, this.reurl);
					return;
				}
				else
				{
					if (productActionStatus == ProductActionStatus.AttributeError)
					{
						this.ShowMsg(this.operatorName + "商品失败，保存商品属性时出错", false);
						return;
					}
					if (productActionStatus == ProductActionStatus.DuplicateName)
					{
						this.ShowMsg(this.operatorName + "商品失败，商品名称不能重复", false);
						return;
					}
					if (productActionStatus == ProductActionStatus.DuplicateSKU)
					{
						this.ShowMsg(this.operatorName + "商品失败，商家编码不能重复", false);
						return;
					}
					if (productActionStatus == ProductActionStatus.SKUError)
					{
						this.ShowMsg(this.operatorName + "商品失败，商家编码不能重复", false);
						return;
					}
					if (productActionStatus == ProductActionStatus.OffShelfError)
					{
						this.ShowMsg(this.operatorName + "商品失败，子站没在零售价范围内的商品无法下架", false);
						return;
					}
					if (productActionStatus == ProductActionStatus.ProductTagEroor)
					{
						this.ShowMsg(this.operatorName + "商品失败，保存商品标签时出错", false);
						return;
					}
					this.ShowMsg(this.operatorName + "商品失败，未知错误", false);
					return;
				}
			}
			else
			{
				string text4 = ProductHelper.AddProductNew(productInfo, dictionary, attrs, list);
				int num5 = Globals.ToNum(text4);
				if (num5 > 0)
				{
					base.Response.Redirect("productedit.aspx?productid=" + num5 + "&isnext=1");
					base.Response.End();
					return;
				}
				this.ShowMsg(this.operatorName + "商品失败，" + text4, false);
				return;
			}
		}

		private bool ValidateConverts(string productname, bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight, out int showSaleCounts, out decimal firstCommission, out decimal secondCommission, out decimal thirdCommission, out decimal cubicMeter, out decimal freightWeight)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(productname))
			{
				text += Formatter.FormatErrorMessage("请输入商品名称");
			}
			if (!int.TryParse(this.txtShowSaleCounts.Text.Trim(), out showSaleCounts))
			{
				showSaleCounts = 0;
			}
			costPrice = null;
			marketPrice = null;
			weight = null;
			displaySequence = (stock = 0);
			salePrice = 0m;
			if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
			{
				text += Formatter.FormatErrorMessage("请正确填写商品排序");
			}
			decimal num;
			if (decimal.TryParse(this.txtMarketPrice.Text, out num))
			{
				marketPrice = new decimal?(num);
			}
			else
			{
				text += Formatter.FormatErrorMessage("请正确填写商品原价");
			}
			if (!skuEnabled)
			{
				if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品现价");
				}
				if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
				{
					decimal value;
					if (decimal.TryParse(this.txtCostPrice.Text, out value))
					{
						costPrice = new decimal?(value);
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的成本价");
					}
				}
				if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品库存");
				}
				if (!string.IsNullOrEmpty(this.txtWeight.Text))
				{
					decimal value2;
					if (decimal.TryParse(this.txtWeight.Text, out value2))
					{
						weight = new decimal?(value2);
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的重量");
					}
				}
			}
			else if (!string.IsNullOrEmpty(this.txtSalePrice.Text))
			{
				if (decimal.TryParse(this.txtMarketPrice.Text, out num))
				{
					salePrice = num;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写商品现价");
				}
			}
			if (this.txtThirdCommission.Text.Trim() == "")
			{
				thirdCommission = 0m;
			}
			else if (decimal.TryParse(this.txtThirdCommission.Text, out num))
			{
				thirdCommission = num;
			}
			else
			{
				thirdCommission = 0m;
				text += Formatter.FormatErrorMessage("请正确填写上二级佣金比例");
			}
			if (this.txtSecondCommission.Text.Trim() == "")
			{
				secondCommission = 0m;
			}
			else if (decimal.TryParse(this.txtSecondCommission.Text, out num))
			{
				secondCommission = num;
			}
			else
			{
				secondCommission = 0m;
				text += Formatter.FormatErrorMessage("请正确填写上一级佣金比例");
			}
			if (this.txtFirstCommission.Text.Trim() == "")
			{
				firstCommission = 0m;
			}
			else if (decimal.TryParse(this.txtFirstCommission.Text, out num))
			{
				firstCommission = num;
			}
			else
			{
				firstCommission = 0m;
				text += Formatter.FormatErrorMessage("请正确填写成交店铺佣金比例");
			}
			if (decimal.TryParse(this.txtCubicMeter.Text, out num))
			{
				cubicMeter = num;
			}
			else
			{
				cubicMeter = 0m;
			}
			if (decimal.TryParse(this.txtFreightWeight.Text, out num))
			{
				freightWeight = num;
			}
			else
			{
				freightWeight = 0m;
			}
			if (this.rbtIsSetTemplate.Checked && this.FreightTemplateDownList1.SelectedValue < 1)
			{
				text += Formatter.FormatErrorMessage("请选择运费模版");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}

		private void LoadProduct(ProductInfo product, System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs)
		{
			this.dropProductTypes.SelectedValue = product.TypeId;
			this.dropBrandCategories.SelectedValue = product.BrandId;
			bool isSetCommission = product.IsSetCommission;
			if (isSetCommission)
			{
				this.cbIsSetCommission.Checked = false;
				this.txtFirstCommission.Enabled = true;
				this.txtSecondCommission.Enabled = true;
				this.txtThirdCommission.Enabled = true;
				this.txtFirstCommission.Text = product.FirstCommission.ToString("F2");
				this.txtSecondCommission.Text = product.SecondCommission.ToString("F2");
				this.txtThirdCommission.Text = product.ThirdCommission.ToString("F2");
			}
			else
			{
				CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
				if (category != null)
				{
					this.txtFirstCommission.Text = category.FirstCommission;
					this.txtSecondCommission.Text = category.SecondCommission;
					this.txtThirdCommission.Text = category.ThirdCommission;
				}
			}
			this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
			this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
			this.txtProductShortName.Text = Globals.HtmlDecode(product.ProductShortName);
			this.txtProductCode.Text = product.ProductCode;
			this.txtUnit.Text = product.Unit;
			if (product.MarketPrice.HasValue)
			{
				this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
			}
			this.txtShortDescription.Text = product.ShortDescription;
			this.fckDescription.Text = product.Description;
			if (product.SaleStatus == ProductSaleStatus.OnSale)
			{
				this.radOnSales.Checked = true;
			}
			else if (product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.radUnSales.Checked = true;
			}
			else
			{
				this.radInStock.Checked = true;
			}
			this.ChkisfreeShipping.Checked = product.IsfreeShipping;
			string text = string.Concat(new string[]
			{
				product.ImageUrl1,
				",",
				product.ImageUrl2,
				",",
				product.ImageUrl3,
				",",
				product.ImageUrl4,
				",",
				product.ImageUrl5
			});
			this.ucFlashUpload1.Value = text.Replace(",,", ",").Replace(",,", ",").Trim(new char[]
			{
				','
			});
			if (attrs != null && attrs.Count > 0)
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("<xml><attributes>");
				foreach (int current in attrs.Keys)
				{
					stringBuilder.Append("<item attributeId=\"").Append(current.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(current).UsageMode).ToString()).Append("\" >");
					foreach (int current2 in attrs[current])
					{
						stringBuilder.Append("<attValue valueId=\"").Append(current2.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
					}
					stringBuilder.Append("</item>");
				}
				stringBuilder.Append("</attributes></xml>");
				this.txtAttributes.Text = stringBuilder.ToString();
			}
			if (product.HasSKU && product.Skus.Keys.Count > 0)
			{
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.Append("<xml><productSkus>");
				foreach (string current3 in product.Skus.Keys)
				{
					SKUItem sKUItem = product.Skus[current3];
					string text2 = string.Concat(new string[]
					{
						"<item skuCode=\"",
						sKUItem.SKU,
						"\" salePrice=\"",
						sKUItem.SalePrice.ToString("F2"),
						"\" costPrice=\"",
						(sKUItem.CostPrice > 0m) ? sKUItem.CostPrice.ToString("F2") : "",
						"\" qty=\"",
						sKUItem.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"\" weight=\"",
						(sKUItem.Weight > 0m) ? sKUItem.Weight.ToString("F2") : "",
						"\">"
					});
					text2 += "<skuFields>";
					foreach (int current4 in sKUItem.SkuItems.Keys)
					{
						string str = string.Concat(new string[]
						{
							"<sku attributeId=\"",
							current4.ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" valueId=\"",
							sKUItem.SkuItems[current4].ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" />"
						});
						text2 += str;
					}
					text2 += "</skuFields>";
					if (sKUItem.MemberPrices.Count > 0)
					{
						text2 += "<memberPrices>";
						foreach (int current5 in sKUItem.MemberPrices.Keys)
						{
							text2 += string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", current5.ToString(System.Globalization.CultureInfo.InvariantCulture), sKUItem.MemberPrices[current5].ToString("F2"));
						}
						text2 += "</memberPrices>";
					}
					text2 += "</item>";
					stringBuilder2.Append(text2);
				}
				stringBuilder2.Append("</productSkus></xml>");
				this.txtSkus.Text = stringBuilder2.ToString();
			}
			else
			{
				product.HasSKU = false;
			}
			SKUItem defaultSku = product.DefaultSku;
			this.txtSku.Text = product.SKU;
			this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
			this.txtCostPrice.Text = ((defaultSku.CostPrice > 0m) ? defaultSku.CostPrice.ToString("F2") : "");
			this.txtStock.Text = ProductHelper.GetProductSumStock(product.ProductId).ToString();
			this.txtWeight.Text = ((defaultSku.Weight > 0m) ? defaultSku.Weight.ToString("F2") : "");
			if (defaultSku.MemberPrices.Count > 0)
			{
				this.txtMemberPrices.Text = "<xml><gradePrices>";
				foreach (int current6 in defaultSku.MemberPrices.Keys)
				{
					TrimTextBox expr_7F8 = this.txtMemberPrices;
					expr_7F8.Text += string.Format("<grande id=\"{0}\" price=\"{1}\" />", current6.ToString(System.Globalization.CultureInfo.InvariantCulture), defaultSku.MemberPrices[current6].ToString("F2"));
				}
				TrimTextBox expr_859 = this.txtMemberPrices;
				expr_859.Text += "</gradePrices></xml>";
			}
			this.chkSkuEnabled.Checked = product.HasSKU;
			this.rbtIsSetTemplate.Checked = (product.FreightTemplateId > 0);
			this.txtShowSaleCounts.Text = product.ShowSaleCounts.ToString();
			this.txtCubicMeter.Text = product.CubicMeter.ToString("F2");
			this.txtFreightWeight.Text = product.FreightWeight.ToString("F2");
			this.FreightTemplateDownList1.SelectedValue = product.FreightTemplateId;
		}
	}
}
