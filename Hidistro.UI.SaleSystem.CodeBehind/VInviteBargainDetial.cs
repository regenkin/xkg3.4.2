using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VInviteBargainDetial : VshopTemplatedWebControl
	{
		private int bargainId;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.WebControls.Literal litShortDescription;

		private System.Web.UI.WebControls.Literal litSalePrice;

		private System.Web.UI.WebControls.Literal litFloorPrice;

		private System.Web.UI.WebControls.Literal litFloorPrice1;

		private System.Web.UI.WebControls.Literal litPurchaseNumber;

		private System.Web.UI.WebControls.Literal litParticipantNumber;

		private System.Web.UI.WebControls.Literal litProductDesc;

		private System.Web.UI.WebControls.Literal litProductConsultationTotal;

		private System.Web.UI.WebControls.Literal litProductCommentTotal;

		private System.Web.UI.WebControls.Literal litStock;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddHasCollected;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddProductId;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideTitle;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideImgUrl;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideDesc;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddEndDate;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPurchaseNumber;

		private Common_SKUSelector skuSelector;

		private VshopTemplatedRepeater rptProductImages;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VInviteBargainDetial.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["bargainId"], out this.bargainId))
			{
				base.GotoResourceNotFound("");
			}
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litFloorPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litFloorPrice");
			this.litFloorPrice1 = (System.Web.UI.WebControls.Literal)this.FindControl("litFloorPrice1");
			this.litPurchaseNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litPurchaseNumber");
			this.litParticipantNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litParticipantNumber");
			this.litProductDesc = (System.Web.UI.WebControls.Literal)this.FindControl("litProductDesc");
			this.litProductConsultationTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litProductConsultationTotal");
			this.litProductCommentTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litProductCommentTotal");
			this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
			this.hiddHasCollected = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddHasCollected");
			this.hiddProductId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddProductId");
			this.hideTitle = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideTitle");
			this.hideImgUrl = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideImgUrl");
			this.hideDesc = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideDesc");
			this.hiddEndDate = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddEndDate");
			this.hiddPurchaseNumber = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddPurchaseNumber");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			bool flag = false;
			BargainInfo bargainInfo = BargainHelper.GetBargainInfo(this.bargainId);
			if (bargainInfo != null)
			{
				this.hideTitle.Value = bargainInfo.Title;
				this.hideDesc.Value = bargainInfo.Remarks;
				System.Uri url = System.Web.HttpContext.Current.Request.Url;
				string activityCover = bargainInfo.ActivityCover;
				string str = string.Empty;
				if (!activityCover.StartsWith("http"))
				{
					str = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
				}
				int num = Globals.RequestQueryNum("bargainDetialId");
				if (num > 0)
				{
					BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(num);
					if (currentMember == null || (currentMember != null && bargainDetialInfo != null && bargainDetialInfo.UserId != currentMember.UserId))
					{
						System.Web.HttpContext.Current.Response.Redirect(string.Concat(new object[]
						{
							"HelpBargainDetial.aspx?bargainId=",
							this.bargainId,
							"&bargainDetialId=",
							num
						}));
						System.Web.HttpContext.Current.Response.End();
					}
				}
				PageTitle.AddSiteNameTitle(bargainInfo.Title);
				this.litFloorPrice.Text = bargainInfo.FloorPrice.ToString("F2");
				this.litFloorPrice1.Text = bargainInfo.FloorPrice.ToString("F2");
				this.litPurchaseNumber.Text = (bargainInfo.ActivityStock - bargainInfo.TranNumber).ToString();
				this.litParticipantNumber.Text = BargainHelper.HelpBargainCount(this.bargainId).ToString();
				this.hiddEndDate.Value = bargainInfo.EndDate.ToString("yyyy:MM:dd:HH:mm:ss");
				this.hiddPurchaseNumber.Value = bargainInfo.PurchaseNumber.ToString();
				this.litStock.Text = bargainInfo.PurchaseNumber.ToString();
				if (bargainInfo.ProductId > 0)
				{
					this.skuSelector.ProductId = bargainInfo.ProductId;
					if (currentMember != null)
					{
						flag = ProductBrowser.CheckHasCollect(currentMember.UserId, bargainInfo.ProductId);
					}
					this.hiddHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
					ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
					this.hiddProductId.Value = bargainInfo.ProductId.ToString();
					this.litProdcutName.Text = productDetails.ProductName;
					this.litShortDescription.Text = productDetails.ShortDescription;
					this.litSalePrice.Text = productDetails.MinSalePrice.ToString("f2");
					this.litProductDesc.Text = productDetails.Description;
					this.hideImgUrl.Value = (string.IsNullOrEmpty(productDetails.ThumbnailUrl60) ? (str + activityCover) : (str + productDetails.ThumbnailUrl60));
					if (this.rptProductImages != null)
					{
						string locationUrl = "javascript:;";
						SlideImage[] source = new SlideImage[]
						{
							new SlideImage(productDetails.ImageUrl1, locationUrl),
							new SlideImage(productDetails.ImageUrl2, locationUrl),
							new SlideImage(productDetails.ImageUrl3, locationUrl),
							new SlideImage(productDetails.ImageUrl4, locationUrl),
							new SlideImage(productDetails.ImageUrl5, locationUrl)
						};
						this.rptProductImages.DataSource = from item in source
						where !string.IsNullOrWhiteSpace(item.ImageUrl)
						select item;
						this.rptProductImages.DataBind();
					}
					int num2 = ProductBrowser.GetProductConsultationsCount(bargainInfo.ProductId, false);
					this.litProductConsultationTotal.SetWhenIsNotNull(num2.ToString());
					num2 = ProductBrowser.GetProductReviewsCount(bargainInfo.ProductId);
					this.litProductCommentTotal.SetWhenIsNotNull(num2.ToString());
				}
			}
			else
			{
				System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
				System.Web.HttpContext.Current.Response.End();
			}
		}
	}
}
