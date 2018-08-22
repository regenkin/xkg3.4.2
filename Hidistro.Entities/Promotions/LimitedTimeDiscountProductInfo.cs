using System;

namespace Hidistro.Entities.Promotions
{
	public class LimitedTimeDiscountProductInfo
	{
		public int LimitedTimeDiscountProductId
		{
			get;
			set;
		}

		public int LimitedTimeDiscountId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public decimal Discount
		{
			get;
			set;
		}

		public decimal Minus
		{
			get;
			set;
		}

		public int IsDehorned
		{
			get;
			set;
		}

		public int IsChamferPoint
		{
			get;
			set;
		}

		public decimal FinalPrice
		{
			get;
			set;
		}

		public System.DateTime BeginTime
		{
			get;
			set;
		}

		public System.DateTime EndTime
		{
			get;
			set;
		}

		public System.DateTime CreateTime
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}
	}
}
