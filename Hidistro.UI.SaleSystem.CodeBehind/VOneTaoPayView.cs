using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VOneTaoPayView : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlImage ProductImg;

		private System.Web.UI.WebControls.Literal litProdcutAttr;

		private System.Web.UI.WebControls.Literal litActivityID;

		private System.Web.UI.WebControls.Literal litReachType;

		private System.Web.UI.WebControls.Literal litPrice;

		private System.Web.UI.WebControls.Literal litBuyNum;

		private System.Web.UI.WebControls.Literal litPayPrice;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.HtmlControls.HtmlContainerControl PayWaytxt;

		private System.Web.UI.HtmlControls.HtmlContainerControl PayBtn;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VOneTaoPayView.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = Globals.RequestQueryStr("Pid");
			if (string.IsNullOrEmpty(text))
			{
				base.GotoResourceNotFound("");
			}
			OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, text, "");
			if (addParticipant == null)
			{
				base.GotoResourceNotFound("");
			}
			string activityId = addParticipant.ActivityId;
			OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(activityId);
			if (oneyuanTaoInfoById == null)
			{
				base.GotoResourceNotFound("");
			}
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litProdcutAttr = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutAttr");
			this.litActivityID = (System.Web.UI.WebControls.Literal)this.FindControl("litActivityID");
			this.litReachType = (System.Web.UI.WebControls.Literal)this.FindControl("litReachType");
			this.litPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litPrice");
			this.litBuyNum = (System.Web.UI.WebControls.Literal)this.FindControl("litBuyNum");
			this.litPayPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litPayPrice");
			this.ProductImg = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("ProductImg");
			this.PayWaytxt = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("PayWaytxt");
			this.PayBtn = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("PayBtn");
			this.litProdcutName.Text = oneyuanTaoInfoById.ProductTitle;
			this.litProdcutAttr.Text = addParticipant.SkuIdStr;
			this.litActivityID.Text = activityId;
			this.litReachType.Text = "";
			this.litPrice.Text = oneyuanTaoInfoById.EachPrice.ToString("F2");
			this.litBuyNum.Text = addParticipant.BuyNum.ToString();
			this.litPayPrice.Text = addParticipant.TotalPrice.ToString("F2");
			this.ProductImg.Src = oneyuanTaoInfoById.ProductImg;
			if (oneyuanTaoInfoById.ReachType == 1)
			{
				this.litReachType.Text = "满足参与人数自动开奖";
			}
			else if (oneyuanTaoInfoById.ReachType == 2)
			{
				this.litReachType.Text = "活动到期自动开奖";
			}
			else
			{
				this.litReachType.Text = "活动到期时且满足参与人数自动开奖";
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string str = "商家尚未开启网上支付功能！";
			this.PayBtn.Visible = false;
			if (!this.Page.Request.UserAgent.ToLower().Contains("micromessenger") && masterSettings.EnableAlipayRequest)
			{
				str = "支付宝手机支付";
				this.PayBtn.Visible = true;
			}
			else if (masterSettings.EnableWeiXinRequest && this.Page.Request.UserAgent.ToLower().Contains("micromessenger"))
			{
				str = "微信支付";
				this.PayBtn.Visible = true;
			}
			this.PayWaytxt.InnerText = "当前可用支付方式：" + str;
			this.PayWaytxt.Attributes.Add("Pid", text);
			PageTitle.AddSiteNameTitle("结算支付");
		}
	}
}
