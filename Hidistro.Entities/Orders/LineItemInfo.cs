using System;

namespace Hidistro.Entities.Orders
{
	public class LineItemInfo
	{
		public int ID
		{
			get;
			set;
		}

		public string SkuId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public string SKU
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public int ShipmentQuantity
		{
			get;
			set;
		}

		public decimal ItemCostPrice
		{
			get;
			set;
		}

		public decimal ItemListPrice
		{
			get;
			set;
		}

		public decimal ItemAdjustedPrice
		{
			get;
			set;
		}

		public string ItemDescription
		{
			get;
			set;
		}

		public string ThumbnailsUrl
		{
			get;
			set;
		}

		public decimal ItemWeight
		{
			get;
			set;
		}

		public string SKUContent
		{
			get;
			set;
		}

		public int PromotionId
		{
			get;
			set;
		}

		public string PromotionName
		{
			get;
			set;
		}

		public decimal ReturnMoney
		{
			get;
			set;
		}

		public string MainCategoryPath
		{
			get;
			set;
		}

		public OrderStatus OrderItemsStatus
		{
			get;
			set;
		}

		public decimal ItemsCommission
		{
			get;
			set;
		}

		public decimal SecondItemsCommission
		{
			get;
			set;
		}

		public decimal ThirdItemsCommission
		{
			get;
			set;
		}

		public decimal ItemAdjustedCommssion
		{
			get;
			set;
		}

		public decimal ThirdCommission
		{
			get;
			set;
		}

		public decimal SecondCommission
		{
			get;
			set;
		}

		public decimal FirstCommission
		{
			get;
			set;
		}

		public bool IsSetCommission
		{
			get;
			set;
		}

		public int PointNumber
		{
			get;
			set;
		}

		public int ExchangeId
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public decimal DiscountAverage
		{
			get;
			set;
		}

		public string OrderID
		{
			get;
			set;
		}

		public bool IsAdminModify
		{
			get;
			set;
		}

		public int LimitedTimeDiscountId
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public decimal GetSubTotal()
		{
			return this.ItemAdjustedPrice * this.Quantity;
		}
	}
}
