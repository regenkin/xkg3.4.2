using System;

namespace Hidistro.Entities.Orders
{
	public class OrderSplitInfo
	{
		public int Id
		{
			get;
			set;
		}

		public int OrderIDNum
		{
			get;
			set;
		}

		public string OldOrderId
		{
			get;
			set;
		}

		public string ItemList
		{
			get;
			set;
		}

		public System.DateTime UpdateTime
		{
			get;
			set;
		}

		public decimal AdjustedFreight
		{
			get;
			set;
		}
	}
}
