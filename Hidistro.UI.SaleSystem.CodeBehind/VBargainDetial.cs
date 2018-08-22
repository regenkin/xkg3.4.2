using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
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
	public class VBargainDetial : VshopTemplatedWebControl
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

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddEndDate;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPurchaseNumber;

		private Common_SKUSelector skuSelector;

		private VshopTemplatedRepeater rptProductImages;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideTitle;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideImgUrl;

		private System.Web.UI.HtmlControls.HtmlInputHidden hideDesc;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VBargainDetial.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["id"], out this.bargainId))
			{
				base.GotoResourceNotFound("");
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(this.bargainId, currentMember.UserId);
				if (bargainDetialInfo != null)
				{
					this.Page.Response.Redirect(string.Concat(new object[]
					{
						"InviteBargainDetial.aspx?bargainId=",
						this.bargainId,
						"&bargainDetialId=",
						bargainDetialInfo.Id,
						"&ReferralId=",
						this.Page.Request.QueryString["ReferralId"]
					}));
				}
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
			this.hiddEndDate = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddEndDate");
			this.hiddPurchaseNumber = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddPurchaseNumber");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
			this.hideTitle = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideTitle");
			this.hideImgUrl = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideImgUrl");
			this.hideDesc = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hideDesc");
			bool flag = false;
			BargainInfo bargainInfo = BargainHelper.GetBargainInfo(this.bargainId);
			if (bargainInfo != null)
			{
				PageTitle.AddSiteNameTitle(bargainInfo.Title);
				this.litFloorPrice.Text = bargainInfo.FloorPrice.ToString("F2");
				this.litFloorPrice1.Text = bargainInfo.FloorPrice.ToString("F2");
				this.litSalePrice.Text = bargainInfo.InitialPrice.ToString("f2");
				this.litPurchaseNumber.Text = (bargainInfo.ActivityStock - bargainInfo.TranNumber).ToString();
				this.litParticipantNumber.Text = BargainHelper.HelpBargainCount(this.bargainId).ToString();
				this.hiddEndDate.Value = bargainInfo.EndDate.ToString("yyyy:MM:dd:HH:mm:ss");
				this.hiddPurchaseNumber.Value = bargainInfo.PurchaseNumber.ToString();
				this.litStock.Text = bargainInfo.PurchaseNumber.ToString();
				this.hideTitle.Value = bargainInfo.Title;
				this.hideDesc.Value = bargainInfo.Remarks.Replace("\n", " ").Replace("\r", "");
				string activityCover = bargainInfo.ActivityCover;
				string str = string.Empty;
				System.Uri url = System.Web.HttpContext.Current.Request.Url;
				if (!activityCover.StartsWith("http"))
				{
					str = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
				}
				if (bargainInfo.ProductId > 0)
				{
					this.skuSelector.ProductId = bargainInfo.ProductId;
					if (currentMember != null)
					{
						flag = ProductBrowser.CheckHasCollect(currentMember.UserId, bargainInfo.ProductId);
					}
					this.hiddHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
					ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
					if (productDetails == null)
					{
						this.Context.Response.Write("<script>alert('该商品不存在!');location.href='/default.aspx';</script>");
						this.Context.Response.End();
					}
					else
					{
						this.hiddProductId.Value = bargainInfo.ProductId.ToString();
						this.litProdcutName.Text = productDetails.ProductName;
						this.litShortDescription.Text = productDetails.ProductShortName;
						this.hideImgUrl.Value = (string.IsNullOrEmpty(productDetails.ThumbnailUrl60) ? (str + activityCover) : (str + productDetails.ThumbnailUrl60));
						this.litProductDesc.Text = productDetails.Description;
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
						int num = ProductBrowser.GetProductConsultationsCount(bargainInfo.ProductId, false);
						this.litProductConsultationTotal.SetWhenIsNotNull(num.ToString());
						num = ProductBrowser.GetProductReviewsCount(bargainInfo.ProductId);
						this.litProductCommentTotal.SetWhenIsNotNull(num.ToString());
					}
				}
			}
		}
	}
}
