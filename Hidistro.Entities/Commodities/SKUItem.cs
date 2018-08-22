using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class SKUItem : System.IComparable
	{
		private System.Collections.Generic.Dictionary<int, int> skuItems;

		private System.Collections.Generic.Dictionary<int, decimal> memberPrices;

		public System.Collections.Generic.Dictionary<int, int> SkuItems
		{
			get
			{
				System.Collections.Generic.Dictionary<int, int> arg_19_0;
				if ((arg_19_0 = this.skuItems) == null)
				{
					arg_19_0 = (this.skuItems = new System.Collections.Generic.Dictionary<int, int>());
				}
				return arg_19_0;
			}
		}

		public System.Collections.Generic.Dictionary<int, decimal> MemberPrices
		{
			get
			{
				System.Collections.Generic.Dictionary<int, decimal> arg_19_0;
				if ((arg_19_0 = this.memberPrices) == null)
				{
					arg_19_0 = (this.memberPrices = new System.Collections.Generic.Dictionary<int, decimal>());
				}
				return arg_19_0;
			}
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

		public decimal Weight
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public decimal CostPrice
		{
			get;
			set;
		}

		public decimal SalePrice
		{
			get;
			set;
		}

		public int CompareTo(object obj)
		{
			SKUItem sKUItem = obj as SKUItem;
			int result;
			if (sKUItem == null)
			{
				result = -1;
			}
			else if (sKUItem.SkuItems.Count != this.SkuItems.Count)
			{
				result = -1;
			}
			else
			{
				foreach (int current in sKUItem.SkuItems.Keys)
				{
					if (sKUItem.SkuItems[current] != this.SkuItems[current])
					{
						result = -1;
						return result;
					}
				}
				result = 0;
			}
			return result;
		}
	}
}
