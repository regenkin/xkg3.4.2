using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VExchangeDetails : VshopTemplatedWebControl
	{
		private int productId;

		private int exchangeId;

		private VshopTemplatedRepeater rptProductImages;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.WebControls.Literal litSalePoint;

		private System.Web.UI.WebControls.Literal litMarketPrice;

		private System.Web.UI.WebControls.Literal litShortDescription;

		private System.Web.UI.WebControls.Literal litSurplusTime;

		private System.Web.UI.WebControls.Literal litDescription;

		private System.Web.UI.WebControls.Literal litStock;

		private System.Web.UI.WebControls.Literal litSoldCount;

		private System.Web.UI.WebControls.Literal litConsultationsCount;

		private System.Web.UI.WebControls.Literal litReviewsCount;

		private System.Web.UI.WebControls.Literal litItemParams;

		private System.Web.UI.WebControls.Literal litEachCount;

		private Common_SKUSelector skuSelector;

		private Common_ExpandAttributes expandAttr;

		private System.Web.UI.WebControls.HyperLink linkDescription;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdHasCollected;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdCategoryId;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdProductId;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdEachCount;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdStock;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdIsActive;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdIsInRange;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdPoint;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdTemplateid;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdUserExchanged;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VExchangeDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId) || !int.TryParse(this.Page.Request.QueryString["exchangeId"], out this.exchangeId))
			{
				base.GotoResourceNotFound("");
			}
			this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litSalePoint = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePoint");
			this.litMarketPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarketPrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litSurplusTime = (System.Web.UI.WebControls.Literal)this.FindControl("litSurplusTime");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
			this.litEachCount = (System.Web.UI.WebControls.Literal)this.FindControl("litEachCount");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.linkDescription = (System.Web.UI.WebControls.HyperLink)this.FindControl("linkDescription");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
			this.hdHasCollected = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdHasCollected");
			this.hdCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdCategoryId");
			this.hdEachCount = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdEachCount");
			this.hdProductId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdProductId");
			this.hdStock = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdStock");
			this.hdIsActive = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdIsActive");
			this.hdIsInRange = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdIsInRange");
			this.hdPoint = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdPoint");
			this.hdTemplateid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdTemplateid");
			this.hdUserExchanged = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdUserExchanged");
			PointExChangeInfo pointExChangeInfo = PointExChangeHelper.Get(this.exchangeId);
			PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(this.exchangeId, this.productId);
			ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
			if (pointExChangeInfo != null && product != null && productInfo != null)
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				if (currentMember != null)
				{
					this.hdPoint.Value = currentMember.Points.ToString();
					if (MemberProcessor.CheckCurrentMemberIsInRange(pointExChangeInfo.MemberGrades, pointExChangeInfo.DefualtGroup, pointExChangeInfo.CustomGroup))
					{
						this.hdIsInRange.Value = "true";
					}
					else
					{
						this.hdIsInRange.Value = "false";
					}
				}
				if (pointExChangeInfo.EndDate < System.DateTime.Now)
				{
					this.litSurplusTime.Text = "已结束";
					this.hdIsActive.Value = "0";
				}
				else if (pointExChangeInfo.BeginDate > System.DateTime.Now)
				{
					this.litSurplusTime.Text = "未开始";
					this.hdIsActive.Value = "0";
				}
				else
				{
					this.hdIsActive.Value = "1";
					System.TimeSpan timeSpan = pointExChangeInfo.EndDate - System.DateTime.Now;
					if (timeSpan.Days > 1)
					{
						this.litSurplusTime.Text = string.Concat(new object[]
						{
							"还剩",
							timeSpan.Days,
							"天",
							timeSpan.Hours,
							"小时"
						});
					}
					else
					{
						this.litSurplusTime.Text = "还剩" + timeSpan.Hours + "小时";
					}
				}
				this.hdProductId.Value = this.productId.ToString();
				if (product == null)
				{
					base.GotoResourceNotFound("此商品已不存在");
				}
				if (product.SaleStatus != ProductSaleStatus.OnSale)
				{
					base.GotoResourceNotFound(ErrorType.前台商品下架, "此商品已下架");
				}
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					SlideImage[] source = new SlideImage[]
					{
						new SlideImage(product.ImageUrl1, locationUrl),
						new SlideImage(product.ImageUrl2, locationUrl),
						new SlideImage(product.ImageUrl3, locationUrl),
						new SlideImage(product.ImageUrl4, locationUrl),
						new SlideImage(product.ImageUrl5, locationUrl)
					};
					this.rptProductImages.DataSource = from item in source
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
				}
				string mainCategoryPath = product.MainCategoryPath;
				if (!string.IsNullOrEmpty(mainCategoryPath))
				{
					this.hdCategoryId.Value = mainCategoryPath.Split(new char[]
					{
						'|'
					})[0];
				}
				else
				{
					this.hdCategoryId.Value = "0";
				}
				this.litProdcutName.Text = product.ProductName;
				this.hdTemplateid.Value = product.FreightTemplateId.ToString();
				this.litSalePoint.Text = productInfo.PointNumber.ToString();
				if (product.MarketPrice.HasValue)
				{
					this.litMarketPrice.SetWhenIsNotNull(product.MarketPrice.GetValueOrDefault(0m).ToString("F2"));
				}
				this.litShortDescription.Text = product.ShortDescription;
				string text = product.Description;
				if (!string.IsNullOrEmpty(text))
				{
					text = System.Text.RegularExpressions.Regex.Replace(text, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + System.Web.HttpContext.Current.Server.HtmlEncode(product.ProductName) + "' src='$2' />", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				}
				if (this.litDescription != null)
				{
					this.litDescription.Text = text;
				}
				this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
				int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(this.exchangeId, this.productId);
				int num = (productInfo.ProductNumber - productExchangedCount >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
				this.litStock.Text = num.ToString();
				this.hdStock.Value = num.ToString();
				this.litEachCount.Text = productInfo.EachMaxNumber.ToString();
				this.hdEachCount.Value = productInfo.EachMaxNumber.ToString();
				this.skuSelector.ProductId = this.productId;
				if (this.expandAttr != null)
				{
					this.expandAttr.ProductId = this.productId;
				}
				if (this.linkDescription != null)
				{
					this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
				}
				int num2 = ProductBrowser.GetProductConsultationsCount(this.productId, false);
				this.litConsultationsCount.SetWhenIsNotNull(num2.ToString());
				num2 = ProductBrowser.GetProductReviewsCount(this.productId);
				this.litReviewsCount.SetWhenIsNotNull(num2.ToString());
				if (currentMember != null)
				{
					this.hdUserExchanged.Value = PointExChangeHelper.GetUserProductExchangedCount(this.exchangeId, this.productId, currentMember.UserId).ToString();
					bool flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
					this.hdHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
				}
				ProductBrowser.UpdateVisitCounts(this.productId);
				PageTitle.AddSiteNameTitle("积分商品");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text2 = "";
				if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
				{
					text2 = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
				}
				this.litItemParams.Text = string.Concat(new object[]
				{
					text2.Replace("|", "｜"),
					"|",
					masterSettings.GoodsName.Replace("|", "｜"),
					"|",
					masterSettings.GoodsDescription.Replace("|", "｜"),
					"$",
					Globals.HostPath(System.Web.HttpContext.Current.Request.Url).Replace("|", "｜"),
					product.ImageUrl1.Replace("|", "｜"),
					"|",
					product.ProductName.Replace("|", "｜"),
					"|",
					(product.ShortDescription == null) ? "" : product.ShortDescription.Replace("|", "｜"),
					"|",
					System.Web.HttpContext.Current.Request.Url
				});
			}
			else
			{
				System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
				System.Web.HttpContext.Current.Response.End();
			}
		}
	}
}
