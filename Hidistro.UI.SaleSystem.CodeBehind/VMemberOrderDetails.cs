using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberOrderDetails : VMemberTemplatedWebControl
	{
		private string orderId;

		private System.Web.UI.WebControls.Literal litShipTo;

		private System.Web.UI.WebControls.Literal litPhone;

		private System.Web.UI.WebControls.Literal litAddress;

		private System.Web.UI.WebControls.Literal litShipToDate;

		private System.Web.UI.WebControls.Literal litShippingCost;

		private System.Web.UI.WebControls.Literal litCounponPrice;

		private System.Web.UI.WebControls.Literal litRedPagerAmount;

		private System.Web.UI.WebControls.Literal litBuildPrice;

		private System.Web.UI.WebControls.Literal litDisCountPrice;

		private System.Web.UI.WebControls.Literal litExemption;

		private System.Web.UI.WebControls.Literal litPointToCash;

		private System.Web.UI.WebControls.Literal litOrderId;

		private System.Web.UI.WebControls.Literal litActualPrice;

		private System.Web.UI.WebControls.Literal litOrderDate;

		private System.Web.UI.WebControls.Literal litRemark;

		private OrderStatusLabel litOrderStatus;

		private System.Web.UI.WebControls.Literal litTotalPrice;

		private System.Web.UI.WebControls.Literal litPayTime;

		private System.Web.UI.WebControls.HyperLink hlinkGetRedPager;

		private System.Web.UI.HtmlControls.HtmlInputHidden orderStatus;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtOrderId;

		private VshopTemplatedRepeater rptOrderProducts;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrderDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litPhone");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderDate = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderDate");
			this.litOrderStatus = (OrderStatusLabel)this.FindControl("litOrderStatus");
			this.rptOrderProducts = (VshopTemplatedRepeater)this.FindControl("rptOrderProducts");
			this.litTotalPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litTotalPrice");
			this.litPayTime = (System.Web.UI.WebControls.Literal)this.FindControl("litPayTime");
			this.hlinkGetRedPager = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkGetRedPager");
			this.orderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("orderStatus");
			this.txtOrderId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtOrderId");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.litShipToDate = (System.Web.UI.WebControls.Literal)this.FindControl("litShipToDate");
			this.litShippingCost = (System.Web.UI.WebControls.Literal)this.FindControl("litShippingCost");
			this.litCounponPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litCounponPrice");
			this.litRedPagerAmount = (System.Web.UI.WebControls.Literal)this.FindControl("litRedPagerAmount");
			this.litExemption = (System.Web.UI.WebControls.Literal)this.FindControl("litExemption");
			this.litPointToCash = (System.Web.UI.WebControls.Literal)this.FindControl("litPointToCash");
			this.litBuildPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litBuildPrice");
			this.litDisCountPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litDisCountPrice");
			this.litActualPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litActualPrice");
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			this.litShipTo.Text = orderInfo.ShipTo;
			this.litPhone.Text = orderInfo.CellPhone;
			this.litAddress.Text = orderInfo.ShippingRegion + orderInfo.Address;
			if (orderInfo.BargainDetialId > 0)
			{
				this.litOrderId.Text = this.orderId + "<span class='text-danger'> 【砍价】</span>";
			}
			else
			{
				this.litOrderId.Text = this.orderId;
			}
			this.litOrderDate.Text = orderInfo.OrderDate.ToString();
			this.litTotalPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
			this.litOrderStatus.OrderStatusCode = orderInfo.OrderStatus;
			this.litOrderStatus.Gateway = orderInfo.Gateway;
			this.litPayTime.SetWhenIsNotNull(orderInfo.PayDate.HasValue ? orderInfo.PayDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
			OrderRedPagerInfo orderRedPagerInfo = OrderRedPagerBrower.GetOrderRedPagerInfo(this.orderId);
			if (orderRedPagerInfo != null && orderRedPagerInfo.MaxGetTimes > orderRedPagerInfo.AlreadyGetTimes)
			{
				this.hlinkGetRedPager.NavigateUrl = "/vshop/GetRedShare.aspx?orderid=" + this.orderId;
				this.hlinkGetRedPager.Visible = true;
			}
			this.orderStatus.SetWhenIsNotNull(((int)orderInfo.OrderStatus).ToString());
			this.txtOrderId.SetWhenIsNotNull(this.orderId.ToString());
			decimal d = orderInfo.CouponValue;
			if (d > 0m)
			{
				this.litCounponPrice.Text = "-¥" + d.ToString("F2");
			}
			else
			{
				this.litCounponPrice.Text = " ¥" + d.ToString("F2").Trim(new char[]
				{
					'-'
				});
			}
			d = orderInfo.RedPagerAmount;
			if (d > 0m)
			{
				this.litRedPagerAmount.Text = "<div><span class=\"span-r-80\">代金券抵扣：</span>- ¥" + d.ToString("F2") + "</div>";
			}
			d = orderInfo.GetAdjustCommssion();
			if (d > 0m)
			{
				this.litDisCountPrice.Text = "<div><span class=\"span-r-80\">价格调整：</span>- ¥" + d.ToString("F2") + "</div>";
			}
			else
			{
				this.litDisCountPrice.Text = "<div><span class=\"span-r-80\">价格调整：</span> &nbsp;¥" + d.ToString("F2").Trim(new char[]
				{
					'-'
				}) + "</div>";
			}
			d = orderInfo.PointToCash;
			if (d > 0m)
			{
				this.litPointToCash.Text = "<div><span class=\"span-r-80\">积分抵现：</span>- ¥" + d.ToString("F2") + "</div>";
			}
			d = orderInfo.DiscountAmount;
			if (d > 0m)
			{
				this.litExemption.Text = "<div><span class=\"span-r-80\">优惠减免：</span>- ¥" + d.ToString("F2") + "</div>";
			}
			this.litShippingCost.Text = orderInfo.AdjustedFreight.ToString("F2");
			this.litShipToDate.SetWhenIsNotNull(orderInfo.ShipToDate);
			this.litBuildPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
			this.litActualPrice.SetWhenIsNotNull(orderInfo.TotalPrice.ToString("F2"));
			this.litRemark.SetWhenIsNotNull(orderInfo.Remark);
			this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
			this.rptOrderProducts.DataBind();
			PageTitle.AddSiteNameTitle("订单详情");
		}
	}
}
