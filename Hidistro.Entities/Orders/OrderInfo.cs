using Hidistro.Entities.Promotions;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Orders
{
	public class OrderInfo
	{
		private System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems;

		public static event System.EventHandler<System.EventArgs> Created;

		public static event System.EventHandler<System.EventArgs> Payment;

		public static event System.EventHandler<System.EventArgs> Deliver;

		public static event System.EventHandler<System.EventArgs> Refund;

		public static event System.EventHandler<System.EventArgs> Closed;

		public int BargainDetialId
		{
			get;
			set;
		}

		public System.Collections.Generic.Dictionary<string, LineItemInfo> LineItems
		{
			get
			{
				if (this.lineItems == null)
				{
					this.lineItems = new System.Collections.Generic.Dictionary<string, LineItemInfo>();
				}
				return this.lineItems;
			}
		}

		public string OrderId
		{
			get;
			set;
		}

		public string OrderMarking
		{
			get;
			set;
		}

		public string GatewayOrderId
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public ClientShortType ClientShortType
		{
			get;
			set;
		}

		public string ReferralPath
		{
			get;
			set;
		}

		public OrderMark? ManagerMark
		{
			get;
			set;
		}

		public string ManagerRemark
		{
			get;
			set;
		}

		public string Gateway
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "订单折扣不能为空，金额大小负1000万-1000万之间")]
		public decimal AdjustedDiscount
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public string CloseReason
		{
			get;
			set;
		}

		public System.DateTime OrderDate
		{
			get;
			set;
		}

		public System.DateTime? PayDate
		{
			get;
			set;
		}

		public System.DateTime? ShippingDate
		{
			get;
			set;
		}

		public System.DateTime? FinishDate
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string QQ
		{
			get;
			set;
		}

		public string Wangwang
		{
			get;
			set;
		}

		public string MSN
		{
			get;
			set;
		}

		public string ShippingRegion
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public string ShipTo
		{
			get;
			set;
		}

		public string TelPhone
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string ShipToDate
		{
			get;
			set;
		}

		public int ShippingModeId
		{
			get;
			set;
		}

		public string ModeName
		{
			get;
			set;
		}

		public int RealShippingModeId
		{
			get;
			set;
		}

		public string RealModeName
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}

		public decimal Freight
		{
			get;
			set;
		}

		public decimal AdjustedFreight
		{
			get;
			set;
		}

		public decimal DiscountAmount
		{
			get;
			set;
		}

		public string ActivitiesName
		{
			get;
			set;
		}

		public string ActivitiesId
		{
			get;
			set;
		}

		public string ShipOrderNumber
		{
			get;
			set;
		}

		public bool IsPrinted
		{
			get;
			set;
		}

		public decimal Weight
		{
			get
			{
				decimal num = 0m;
				foreach (LineItemInfo current in this.LineItems.Values)
				{
					num += current.ItemWeight * current.ShipmentQuantity;
				}
				return num;
			}
			set
			{
			}
		}

		public string ExpressCompanyName
		{
			get;
			set;
		}

		public string ExpressCompanyAbb
		{
			get;
			set;
		}

		public int PaymentTypeId
		{
			get;
			set;
		}

		public string PaymentType
		{
			get;
			set;
		}

		public decimal PayCharge
		{
			get;
			set;
		}

		public RefundStatus RefundStatus
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public string RefundRemark
		{
			get;
			set;
		}

		public OrderStatus DeleteBeforeState
		{
			get;
			set;
		}

		public decimal TotalPrice
		{
			get
			{
				return this.GetTotal();
			}
		}

		public decimal PointToCash
		{
			get;
			set;
		}

		public int PointExchange
		{
			get;
			set;
		}

		public int Points
		{
			get;
			set;
		}

		public int ReducedPromotionId
		{
			get;
			set;
		}

		public string ReducedPromotionName
		{
			get;
			set;
		}

		public decimal ReducedPromotionAmount
		{
			get;
			set;
		}

		public bool IsReduced
		{
			get;
			set;
		}

		public int SentTimesPointPromotionId
		{
			get;
			set;
		}

		public string SentTimesPointPromotionName
		{
			get;
			set;
		}

		public decimal TimesPoint
		{
			get;
			set;
		}

		public bool IsSendTimesPoint
		{
			get;
			set;
		}

		public int FreightFreePromotionId
		{
			get;
			set;
		}

		public string FreightFreePromotionName
		{
			get;
			set;
		}

		public bool IsFreightFree
		{
			get;
			set;
		}

		public int GroupBuyId
		{
			get;
			set;
		}

		public int CountDownBuyId
		{
			get;
			set;
		}

		public int BundlingID
		{
			get;
			set;
		}

		public int? BundlingNum
		{
			get;
			set;
		}

		public decimal NeedPrice
		{
			get;
			set;
		}

		public GroupBuyStatus GroupBuyStatus
		{
			get;
			set;
		}

		public string CouponName
		{
			get;
			set;
		}

		public string CouponCode
		{
			get;
			set;
		}

		public decimal CouponAmount
		{
			get;
			set;
		}

		public decimal CouponValue
		{
			get;
			set;
		}

		public string RedPagerActivityName
		{
			get;
			set;
		}

		public int? RedPagerID
		{
			get;
			set;
		}

		public decimal RedPagerOrderAmountCanUse
		{
			get;
			set;
		}

		public decimal RedPagerAmount
		{
			get;
			set;
		}

		public decimal BundlingPrice
		{
			get;
			set;
		}

		public decimal FirstCommission
		{
			get;
			set;
		}

		public decimal SecondCommission
		{
			get;
			set;
		}

		public decimal ThirdCommission
		{
			get;
			set;
		}

		public decimal Tax
		{
			get;
			set;
		}

		public string InvoiceTitle
		{
			get;
			set;
		}

		public int ReferralUserId
		{
			get;
			set;
		}

		public string Sender
		{
			get;
			set;
		}

		public string OldAddress
		{
			get;
			set;
		}

		public int SplitState
		{
			get;
			set;
		}

		public int ItemCount
		{
			get;
			set;
		}

		public OrderInfo()
		{
			this.OrderStatus = OrderStatus.WaitBuyerPay;
			this.RefundStatus = RefundStatus.None;
		}

		public int GetTotalPointNumber()
		{
			int num = 0;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.Type == 1)
				{
					num += current.PointNumber;
				}
			}
			return num;
		}

		public decimal GetTotal()
		{
			decimal d = this.GetAmount();
			d -= this.GetTotalDiscountAverage();
			d -= this.GetReturnAmount();
			d -= this.GetAdjustCommssion();
			return d + this.AdjustedFreight;
		}

		public virtual decimal GetCostPrice()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				num += current.ItemCostPrice * current.ShipmentQuantity;
			}
			return num;
		}

		public virtual decimal GetTotalDiscountAverage()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				num += current.DiscountAverage;
			}
			return num;
		}

		public virtual decimal GetAdjustCommssion()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.Type == 0)
				{
					num += current.ItemAdjustedCommssion;
				}
			}
			return num;
		}

		public virtual decimal GetTotalCommssion()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.OrderItemsStatus != OrderStatus.Refunded && current.OrderItemsStatus != OrderStatus.Returned)
				{
					num += current.ItemsCommission;
				}
			}
			return num;
		}

		public virtual decimal GetSecondTotalCommssion()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.OrderItemsStatus != OrderStatus.Refunded && current.OrderItemsStatus != OrderStatus.Returned)
				{
					num += current.SecondItemsCommission;
				}
			}
			return num;
		}

		public virtual decimal GetThirdTotalCommssion()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.OrderItemsStatus != OrderStatus.Refunded && current.OrderItemsStatus != OrderStatus.Returned)
				{
					num += current.ThirdItemsCommission;
				}
			}
			return num;
		}

		public virtual decimal GetProfit()
		{
			return this.GetTotal() - this.RefundAmount - this.GetCostPrice();
		}

		public int GetGroupBuyProductQuantity()
		{
			int result;
			if (this.GroupBuyId > 0)
			{
				using (System.Collections.Generic.Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = this.LineItems.Values.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						LineItemInfo current = enumerator.Current;
						result = current.Quantity;
						return result;
					}
				}
			}
			result = 0;
			return result;
		}

		public decimal GetAmount()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.Type == 0)
				{
					num += current.GetSubTotal();
				}
			}
			return num;
		}

		public decimal GetReturnAmount()
		{
			decimal num = 0m;
			foreach (LineItemInfo current in this.LineItems.Values)
			{
				if (current.Type == 0 && (current.OrderItemsStatus == OrderStatus.Returned || current.OrderItemsStatus == OrderStatus.Refunded))
				{
					num += current.ReturnMoney;
				}
			}
			return num;
		}

		public bool CheckAction(OrderActions action)
		{
			bool result;
			if (this.OrderStatus == OrderStatus.Finished || this.OrderStatus == OrderStatus.Closed)
			{
				result = false;
			}
			else
			{
				switch (action)
				{
				case OrderActions.BUYER_PAY:
				case OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS:
				case OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE:
				case OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE:
				case OrderActions.SELLER_CONFIRM_PAY:
				case OrderActions.SELLER_MODIFY_TRADE:
				case OrderActions.SELLER_CLOSE:
				case OrderActions.SUBSITE_SELLER_MODIFY_GIFTS:
					result = (this.OrderStatus == OrderStatus.WaitBuyerPay);
					break;
				case OrderActions.BUYER_CONFIRM_GOODS:
				case OrderActions.SELLER_FINISH_TRADE:
					result = (this.OrderStatus == OrderStatus.SellerAlreadySent);
					break;
				case OrderActions.SELLER_SEND_GOODS:
					result = (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.OrderStatus == OrderStatus.WaitBuyerPay && this.Gateway == "hishop.plugins.payment.podrequest"));
					break;
				case OrderActions.SELLER_REJECT_REFUND:
					result = (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.SellerAlreadySent);
					break;
				case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
				case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
				case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
				case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
					result = (this.OrderStatus == OrderStatus.WaitBuyerPay || this.OrderStatus == OrderStatus.BuyerAlreadyPaid);
					break;
				case OrderActions.SUBSITE_CREATE_PURCHASEORDER:
					result = (this.GroupBuyId > 0 && this.GroupBuyStatus == GroupBuyStatus.Success && this.OrderStatus == OrderStatus.BuyerAlreadyPaid);
					break;
				default:
					result = false;
					break;
				}
			}
			return result;
		}

		public static string GetOrderStatusName(OrderStatus orderStatus)
		{
			string result = "-";
			switch (orderStatus)
			{
			case OrderStatus.WaitBuyerPay:
				result = "等待付款";
				break;
			case OrderStatus.BuyerAlreadyPaid:
				result = "等待发货";
				break;
			case OrderStatus.SellerAlreadySent:
				result = "已发货";
				break;
			case OrderStatus.Closed:
				result = "已关闭";
				break;
			case OrderStatus.Finished:
				result = "交易成功";
				break;
			case OrderStatus.ApplyForRefund:
				result = "申请退款";
				break;
			case OrderStatus.ApplyForReturns:
				result = "申请退货";
				break;
			case OrderStatus.ApplyForReplacement:
				result = "申请换货";
				break;
			case OrderStatus.Refunded:
				result = "已退款";
				break;
			case OrderStatus.Returned:
				result = "已退货";
				break;
			default:
				if (orderStatus == OrderStatus.History)
				{
					result = "历史订单";
				}
				break;
			}
			return result;
		}

		public static void OnCreated(OrderInfo order)
		{
			if (OrderInfo.Created != null)
			{
				OrderInfo.Created(order, new System.EventArgs());
			}
		}

		public void OnCreated()
		{
			if (OrderInfo.Created != null)
			{
				OrderInfo.Created(this, new System.EventArgs());
			}
		}

		public static void OnPayment(OrderInfo order)
		{
			if (OrderInfo.Payment != null)
			{
				OrderInfo.Payment(order, new System.EventArgs());
			}
		}

		public void OnPayment()
		{
			if (OrderInfo.Payment != null)
			{
				OrderInfo.Payment(this, new System.EventArgs());
			}
		}

		public static void OnDeliver(OrderInfo order)
		{
			if (OrderInfo.Deliver != null)
			{
				OrderInfo.Deliver(order, new System.EventArgs());
			}
		}

		public void OnDeliver()
		{
			if (OrderInfo.Deliver != null)
			{
				OrderInfo.Deliver(this, new System.EventArgs());
			}
		}

		public static void OnRefund(OrderInfo order)
		{
			if (OrderInfo.Refund != null)
			{
				OrderInfo.Refund(order, new System.EventArgs());
			}
		}

		public void OnRefund()
		{
			if (OrderInfo.Refund != null)
			{
				OrderInfo.Refund(this, new System.EventArgs());
			}
		}

		public static void OnClosed(OrderInfo order)
		{
			if (OrderInfo.Closed != null)
			{
				OrderInfo.Closed(order, new System.EventArgs());
			}
		}

		public void OnClosed()
		{
			if (OrderInfo.Closed != null)
			{
				OrderInfo.Closed(this, new System.EventArgs());
			}
		}
	}
}
