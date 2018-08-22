using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ViewOneTao : VMemberTemplatedWebControl
	{
		private int productId;

		private string Vaid = "";

		private VshopTemplatedRepeater rptProductImages;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.WebControls.Literal litProdcutTag;

		private System.Web.UI.WebControls.Literal litSalePrice;

		private System.Web.UI.WebControls.Literal litMarketPrice;

		private System.Web.UI.WebControls.Literal litShortDescription;

		private System.Web.UI.WebControls.Literal litDescription;

		private System.Web.UI.WebControls.Literal litStock;

		private System.Web.UI.WebControls.Literal litSoldCount;

		private System.Web.UI.WebControls.Literal litState;

		private System.Web.UI.WebControls.Literal litMinNum;

		private System.Web.UI.WebControls.Literal litPrizeNum;

		private System.Web.UI.WebControls.Literal litFinished;

		private System.Web.UI.WebControls.Literal litConsultationsCount;

		private System.Web.UI.WebControls.Literal litMaxtxt;

		private System.Web.UI.WebControls.Literal litBuytxt;

		private System.Web.UI.WebControls.Literal litReviewsCount;

		private System.Web.UI.WebControls.Literal litItemParams;

		private System.Web.UI.WebControls.Literal litActivityId;

		private System.Web.UI.HtmlControls.HtmlControl PrizeTime;

		private System.Web.UI.HtmlControls.HtmlControl buyNum;

		private System.Web.UI.HtmlControls.HtmlControl SaveBtn;

		private System.Web.UI.HtmlControls.HtmlControl ViewtReview;

		private System.Web.UI.HtmlControls.HtmlControl Prizeprogress;

		private Common_ExpandAttributes expandAttr;

		private Common_SKUSelector skuSelector;

		private System.Web.UI.HtmlControls.HtmlContainerControl NomachMember;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ViewOneTao.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.Vaid = Globals.RequestQueryStr("vaid");
			if (string.IsNullOrEmpty(this.Vaid))
			{
				base.GotoResourceNotFound("");
			}
			OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(this.Vaid);
			if (oneyuanTaoInfoById == null)
			{
				base.GotoResourceNotFound("");
			}
			this.productId = oneyuanTaoInfoById.ProductId;
			ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
			if (product == null)
			{
				base.GotoResourceNotFound("");
			}
			OneTaoState oneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
			this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litProdcutTag = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutTag");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litMarketPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarketPrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
			this.litActivityId = (System.Web.UI.WebControls.Literal)this.FindControl("litActivityId");
			this.litState = (System.Web.UI.WebControls.Literal)this.FindControl("litState");
			this.PrizeTime = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("PrizeTime");
			this.buyNum = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("buyNum");
			this.SaveBtn = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("SaveBtn");
			this.ViewtReview = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("ViewtReview");
			this.litMaxtxt = (System.Web.UI.WebControls.Literal)this.FindControl("litMaxtxt");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.NomachMember = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("NomachMember");
			this.litMinNum = (System.Web.UI.WebControls.Literal)this.FindControl("litMinNum");
			this.litPrizeNum = (System.Web.UI.WebControls.Literal)this.FindControl("litPrizeNum");
			this.litFinished = (System.Web.UI.WebControls.Literal)this.FindControl("litFinished");
			this.Prizeprogress = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("Prizeprogress");
			this.litBuytxt = (System.Web.UI.WebControls.Literal)this.FindControl("litBuytxt");
			this.litPrizeNum.Text = oneyuanTaoInfoById.ReachNum.ToString();
			this.litFinished.Text = oneyuanTaoInfoById.FinishedNum.ToString();
			int num = oneyuanTaoInfoById.ReachNum - oneyuanTaoInfoById.FinishedNum;
			if (num < 0)
			{
				num = 0;
			}
			this.litMinNum.Text = num.ToString();
			float num2 = (float)(100 * oneyuanTaoInfoById.FinishedNum / oneyuanTaoInfoById.ReachNum);
			this.Prizeprogress.Attributes.Add("style", "width:" + num2.ToString("F0") + "%");
			this.ViewtReview.Attributes.Add("href", "ProductReview.aspx?ProductId=" + oneyuanTaoInfoById.ProductId.ToString());
			if (this.expandAttr != null)
			{
				this.expandAttr.ProductId = this.productId;
			}
			this.skuSelector.ProductId = this.productId;
			if (product != null)
			{
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
				this.litShortDescription.Text = product.ShortDescription;
			}
			int num3 = OneyuanTaoHelp.MermberCanbuyNum(oneyuanTaoInfoById.ActivityId, Globals.GetCurrentMemberUserId());
			this.buyNum.Attributes.Add("max", num3.ToString());
			this.litBuytxt.Text = string.Concat(new object[]
			{
				"限购",
				oneyuanTaoInfoById.EachCanBuyNum,
				"份，每份价格￥",
				oneyuanTaoInfoById.EachPrice.ToString("F2")
			});
			this.litMaxtxt.Text = "您已订购<di>" + (oneyuanTaoInfoById.EachCanBuyNum - num3).ToString() + "</di>份";
			if (num3 == 0 || oneTaoState != OneTaoState.进行中 || !MemberHelper.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup, this.CurrentMemberInfo.UserId))
			{
				this.buyNum.Attributes.Add("disabled", "disabled");
				this.SaveBtn.Visible = false;
			}
			string text;
			if (oneyuanTaoInfoById.FitMember == "0" || oneyuanTaoInfoById.CustomGroup == "0")
			{
				text = "全部会员";
			}
			else
			{
				text = "部分会员";
			}
			text = "适用会员：" + text;
			if (oneyuanTaoInfoById.FitMember != "0" && !MemberHelper.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup, this.CurrentMemberInfo.UserId))
			{
				text = "会员等级不符合活动要求";
				this.NomachMember.Attributes.Add("CanBuy", "false");
			}
			this.NomachMember.InnerHtml = text;
			string productName = product.ProductName;
			string text2 = product.Description;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = System.Text.RegularExpressions.Regex.Replace(text2, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + System.Web.HttpContext.Current.Server.HtmlEncode(productName) + "' src='$2' />", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			}
			if (this.litDescription != null)
			{
				this.litDescription.Text = text2;
			}
			this.litProdcutName.Text = productName;
			this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
			this.litActivityId.Text = oneyuanTaoInfoById.ActivityId;
			if (oneyuanTaoInfoById.ReachType == 1)
			{
				this.litActivityId.Text = "活动结束前满足总需份数，自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
			}
			else if (oneyuanTaoInfoById.ReachType == 2)
			{
				this.litActivityId.Text = "活动到期自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
			}
			else if (oneyuanTaoInfoById.ReachType == 3)
			{
				this.litActivityId.Text = "到开奖时间并满足总需份数，自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
			}
			this.PrizeTime.Attributes.Add("PrizeTime", oneyuanTaoInfoById.EndTime.ToString("G"));
			this.litState.Text = oneTaoState.ToString();
			if (oneTaoState == OneTaoState.已开奖)
			{
				IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
				isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("LitDataJson");
				System.Collections.Generic.IList<LuckInfo> winnerLuckInfoList = OneyuanTaoHelp.getWinnerLuckInfoList(oneyuanTaoInfoById.ActivityId, "");
				if (winnerLuckInfoList != null)
				{
					literal.Text = "var LitDataJson=" + JsonConvert.SerializeObject(winnerLuckInfoList, new JsonConverter[]
					{
						isoDateTimeConverter
					});
				}
				else
				{
					literal.Text = "var LitDataJson=null";
				}
			}
			System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)this.FindControl("litJs");
			string title = oneyuanTaoInfoById.Title;
			string activityDec = oneyuanTaoInfoById.ActivityDec;
			System.Uri url = this.Context.Request.Url;
			string text3 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
			string text4 = oneyuanTaoInfoById.ProductImg;
			if (text4 == "/utility/pics/none.gif")
			{
				text4 = oneyuanTaoInfoById.HeadImgage;
			}
			literal2.Text = string.Concat(new string[]
			{
				"<script>wxinshare_title=\"",
				this.Context.Server.HtmlEncode(title.Replace("\n", " ").Replace("\r", "")),
				"\";wxinshare_desc=\"",
				this.Context.Server.HtmlEncode(activityDec.Replace("\n", " ").Replace("\r", "")),
				"\";wxinshare_link=location.href;wxinshare_imgurl=\"",
				text3,
				text4,
				"\"</script>"
			});
			PageTitle.AddSiteNameTitle("一元夺宝商品详情");
		}
	}
}
