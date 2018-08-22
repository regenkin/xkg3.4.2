using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class OrderQuery : Pagination
	{
		public enum OrderType
		{
			NormalProduct = 1,
			GroupBuy
		}

		public OrderStatus Status
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string ShipTo
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string ShipId
		{
			get;
			set;
		}

		public System.DateTime? StartDate
		{
			get;
			set;
		}

		public System.DateTime? EndDate
		{
			get;
			set;
		}

		public int? PaymentType
		{
			get;
			set;
		}

		public int? GroupBuyId
		{
			get;
			set;
		}

		public int? ShippingModeId
		{
			get;
			set;
		}

		public int? IsPrinted
		{
			get;
			set;
		}

		public int? RegionId
		{
			get;
			set;
		}

		public OrderQuery.OrderType? Type
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public int? ReferralUserId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public OrderStatus? OrderItemsStatus
		{
			get;
			set;
		}

		public string Gateway
		{
			get;
			set;
		}
	}
}
