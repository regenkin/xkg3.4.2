using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VFinishOrder : VMemberTemplatedWebControl
	{
		private string orderId;

		private System.Web.UI.WebControls.Literal litOrderId;

		private System.Web.UI.WebControls.Literal litOrderTotal;

		private System.Web.UI.WebControls.Literal literalOrderTotal;

		private System.Web.UI.WebControls.Literal litMessage;

		private System.Web.UI.HtmlControls.HtmlInputHidden litPaymentType;

		private System.Web.UI.WebControls.Literal litHelperText;

		private System.Web.UI.HtmlControls.HtmlAnchor btnToPay;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VFinishOrder.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			System.Collections.Generic.List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(this.orderId);
			decimal num = 0m;
			if (orderMarkingOrderInfo.Count == 0)
			{
				this.Page.Response.Redirect("/Vshop/MemberOrders.aspx?status=0");
			}
			bool flag = true;
			foreach (OrderInfo current in orderMarkingOrderInfo)
			{
				num += current.GetTotal();
				foreach (LineItemInfo current2 in current.LineItems.Values)
				{
					if (current2.Type == 0)
					{
						flag = false;
					}
					foreach (LineItemInfo current3 in current.LineItems.Values)
					{
						if (!ProductHelper.GetProductHasSku(current3.SkuId, current3.Quantity))
						{
							current.OrderStatus = OrderStatus.Closed;
							current.CloseReason = "库存不足";
							OrderHelper.UpdateOrder(current);
							System.Web.HttpContext.Current.Response.Write("<script>alert('库存不足，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
							System.Web.HttpContext.Current.Response.End();
							return;
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && orderMarkingOrderInfo[0].Gateway == "hishop.plugins.payment.offlinerequest")
			{
				this.litMessage = (System.Web.UI.WebControls.Literal)this.FindControl("litMessage");
				this.litMessage.SetWhenIsNotNull(SettingsManager.GetMasterSettings(false).OffLinePayContent);
			}
			this.btnToPay = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("btnToPay");
			if (!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && orderMarkingOrderInfo[0].Gateway == "hishop.plugins.payment.weixinrequest")
			{
				this.btnToPay.Visible = true;
				this.btnToPay.HRef = "~/pay/wx_Submit.aspx?orderId=" + this.orderId;
			}
			if (!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.podrequest" && orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.offlinerequest" && orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.weixinrequest")
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderMarkingOrderInfo[0].PaymentTypeId);
				string attach = "";
				string showUrl = string.Format("http://{0}/vshop/", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.orderId, num, "订单支付", "订单号-" + this.orderId, orderMarkingOrderInfo[0].EmailAddress, orderMarkingOrderInfo[0].OrderDate, showUrl, Globals.FullPath("/pay/PaymentReturn_url.aspx"), Globals.FullPath("/pay/PaymentNotify_url.aspx"), attach);
				paymentRequest.SendRequest();
			}
			else
			{
				this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
				this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
				this.literalOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("literalOrderTotal");
				this.litPaymentType = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litPaymentType");
				int num2 = 0;
				this.litPaymentType.SetWhenIsNotNull("0");
				if (int.TryParse(this.Page.Request.QueryString["PaymentType"], out num2))
				{
					this.litPaymentType.SetWhenIsNotNull(num2.ToString());
				}
				this.litOrderId.SetWhenIsNotNull(this.orderId);
				if (flag)
				{
					this.litOrderTotal.SetWhenIsNotNull("您需要支付：¥" + num.ToString("F2"));
				}
				this.literalOrderTotal.SetWhenIsNotNull("订单金额：<span style='color:red'>¥" + num.ToString("F2") + "</span>");
				this.litHelperText = (System.Web.UI.WebControls.Literal)this.FindControl("litHelperText");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.litHelperText.SetWhenIsNotNull(masterSettings.OffLinePayContent);
				PageTitle.AddSiteNameTitle("下单成功");
			}
		}
	}
}
