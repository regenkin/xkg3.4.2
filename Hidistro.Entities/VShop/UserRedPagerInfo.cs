using System;

namespace Hidistro.Entities.VShop
{
	public class UserRedPagerInfo
	{
		public int RedPagerID
		{
			get;
			set;
		}

		public decimal Amount
		{
			get;
			set;
		}

		public string OrderID
		{
			get;
			set;
		}

		public string RedPagerActivityName
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}

		public decimal OrderAmountCanUse
		{
			get;
			set;
		}

		public System.DateTime CreateTime
		{
			get;
			set;
		}

		public System.DateTime ExpiryTime
		{
			get;
			set;
		}

		public bool IsUsed
		{
			get;
			set;
		}
	}
}
