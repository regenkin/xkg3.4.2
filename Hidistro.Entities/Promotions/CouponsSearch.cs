using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class CouponsSearch : Pagination
	{
		public string CouponName
		{
			get;
			set;
		}

		public decimal? minValue
		{
			get;
			set;
		}

		public decimal? maxValue
		{
			get;
			set;
		}

		public System.DateTime? beginDate
		{
			get;
			set;
		}

		public System.DateTime? endDate
		{
			get;
			set;
		}

		public bool? Finished
		{
			get;
			set;
		}

		public int? SearchType
		{
			get;
			set;
		}
	}
}
