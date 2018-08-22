using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductReview : VMemberTemplatedWebControl
	{
		private int productId;

		private VshopTemplatedRepeater rptProducts;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.WebControls.Literal litSalePrice;

		private System.Web.UI.WebControls.Literal litSoldCount;

		private System.Web.UI.WebControls.Literal litShortDescription;

		private System.Web.UI.HtmlControls.HtmlImage productImage;

		private System.Web.UI.WebControls.HyperLink productLink;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.WebControls.Literal litPID;

		private System.Web.UI.WebControls.Literal litSkuId;

		private System.Web.UI.WebControls.Literal litItemid;

		private System.Web.UI.WebControls.Literal litOrderId;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductReview.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.productImage = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("productImage");
			this.productLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("productLink");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.litPID = (System.Web.UI.WebControls.Literal)this.FindControl("litPID");
			this.litSkuId = (System.Web.UI.WebControls.Literal)this.FindControl("litSkuId");
			this.litItemid = (System.Web.UI.WebControls.Literal)this.FindControl("litItemid");
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			string text = this.Page.Request["OrderId"];
			string arg = "";
			this.litSkuId.Text = Globals.RequestQueryStr("skuid");
			this.litItemid.Text = Globals.RequestQueryNum("itemid").ToString();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (!string.IsNullOrEmpty(text))
			{
				this.litOrderId.Text = text;
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
				if (orderInfo != null && orderInfo.ReferralUserId > 0)
				{
					arg = "&&ReferralId=" + orderInfo.ReferralUserId;
				}
			}
			else
			{
				LineItemInfo latestOrderItemByProductIDAndUserID = ProductBrowser.GetLatestOrderItemByProductIDAndUserID(this.productId, currentMember.UserId);
				if (latestOrderItemByProductIDAndUserID != null)
				{
					text = latestOrderItemByProductIDAndUserID.OrderID;
					this.Page.Response.Redirect(string.Concat(new object[]
					{
						"ProductReview.aspx?OrderId=",
						text,
						"&ProductId=",
						latestOrderItemByProductIDAndUserID.ProductId,
						"&skuid=",
						latestOrderItemByProductIDAndUserID.SkuId,
						"&itemid=",
						latestOrderItemByProductIDAndUserID.ID
					}));
					this.Page.Response.End();
				}
				if (Globals.GetCurrentDistributorId() > 0)
				{
					arg = "&&ReferralId=" + Globals.GetCurrentDistributorId().ToString();
				}
			}
			ProductInfo product = ProductBrowser.GetProduct(currentMember, this.productId);
			this.litProdcutName.SetWhenIsNotNull(product.ProductName);
			this.litSalePrice.SetWhenIsNotNull(product.MinSalePrice.ToString("F2"));
			this.litShortDescription.SetWhenIsNotNull(product.ShortDescription);
			this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
			this.productImage.Src = product.ThumbnailUrl180;
			this.litPID.Text = this.productId.ToString();
			this.productLink.NavigateUrl = "/ProductDetails.aspx?ProductId=" + product.ProductId + arg;
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.productId = this.productId;
			productReviewQuery.IsCount = true;
			productReviewQuery.PageIndex = pageIndex;
			productReviewQuery.PageSize = pageSize;
			productReviewQuery.SortBy = "ReviewId";
			productReviewQuery.SortOrder = SortAction.Desc;
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			DbQueryResult productReviews = ProductBrowser.GetProductReviews(productReviewQuery);
			this.rptProducts.DataSource = productReviews.Data;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(productReviews.TotalRecords.ToString());
			PageTitle.AddSiteNameTitle("商品评价");
		}
	}
}
