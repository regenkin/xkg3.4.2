using System;

namespace Hidistro.Entities.VShop
{
	public class OrderRedPagerInfo
	{
		public string OrderID
		{
			get;
			set;
		}

		public int RedPagerActivityId
		{
			get;
			set;
		}

		public string RedPagerActivityName
		{
			get;
			set;
		}

		public int MaxGetTimes
		{
			get;
			set;
		}

		public int AlreadyGetTimes
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

		public int ExpiryDays
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}
	}
}
