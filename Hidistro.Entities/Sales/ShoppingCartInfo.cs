using Hidistro.Core;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	public class ShoppingCartInfo
	{
		private bool isSendGift;

		private decimal timesPoint = 1m;

		private System.Collections.Generic.IList<ShoppingCartItemInfo> lineItems;

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

		public int SendGiftPromotionId
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public string TemplateId
		{
			get;
			set;
		}

		public string SendGiftPromotionName
		{
			get;
			set;
		}

		public bool IsSendGift
		{
			get
			{
				bool result;
				foreach (ShoppingCartItemInfo current in this.lineItems)
				{
					if (current.IsSendGift)
					{
						result = true;
						return result;
					}
				}
				result = this.isSendGift;
				return result;
			}
			set
			{
				this.isSendGift = value;
			}
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

		public bool IsSendTimesPoint
		{
			get;
			set;
		}

		public decimal TimesPoint
		{
			get
			{
				return this.timesPoint;
			}
			set
			{
				this.timesPoint = value;
			}
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

		public int GetPointNumber
		{
			get;
			set;
		}

		public int MemberPointNumber
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<ShoppingCartItemInfo> LineItems
		{
			get
			{
				if (this.lineItems == null)
				{
					this.lineItems = new System.Collections.Generic.List<ShoppingCartItemInfo>();
				}
				return this.lineItems;
			}
		}

		public decimal Amount
		{
			get;
			set;
		}

		public decimal Exemption
		{
			get;
			set;
		}

		public decimal ShipCost
		{
			get;
			set;
		}

		public decimal Total
		{
			get;
			set;
		}

		public decimal Weight
		{
			get
			{
				decimal num = 0m;
				foreach (ShoppingCartItemInfo current in this.lineItems)
				{
					if (!current.IsfreeShipping)
					{
						num += current.GetSubWeight();
					}
				}
				return num;
			}
		}

		public decimal TotalWeight
		{
			get
			{
				decimal num = 0m;
				foreach (ShoppingCartItemInfo current in this.lineItems)
				{
					num += current.GetSubWeight();
				}
				return num;
			}
		}

		public decimal GetTotal()
		{
			return this.GetAmount() - this.ReducedPromotionAmount;
		}

		public int GetPoint()
		{
			return Globals.GetPoint(this.GetTotal());
		}

		public decimal GetAmount()
		{
			decimal num = 0m;
			foreach (ShoppingCartItemInfo current in this.lineItems)
			{
				num += current.SubTotal;
			}
			return num;
		}

		public int GetTotalPoint()
		{
			int num = 0;
			foreach (ShoppingCartItemInfo current in this.lineItems)
			{
				num += current.PointNumber * current.Quantity;
			}
			return num;
		}

		public int GetQuantity()
		{
			int num = 0;
			foreach (ShoppingCartItemInfo current in this.lineItems)
			{
				num += current.Quantity;
			}
			return num;
		}
	}
}
