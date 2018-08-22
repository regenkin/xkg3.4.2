using System;

namespace Hidistro.Entities.VShop
{
	public class RedPagerActivityInfo
	{
		public int RedPagerActivityId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public decimal MinOrderAmount
		{
			get;
			set;
		}

		public int MaxGetTimes
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public int ExpiryDays
		{
			get;
			set;
		}

		public decimal ItemAmountLimit
		{
			get;
			set;
		}

		public decimal OrderAmountCanUse
		{
			get;
			set;
		}

		public bool IsOpen
		{
			get;
			set;
		}
	}
}
