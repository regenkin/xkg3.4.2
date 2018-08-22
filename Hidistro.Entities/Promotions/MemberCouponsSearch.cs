using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class MemberCouponsSearch : Pagination
	{
		public string CouponName
		{
			get;
			set;
		}

		public string OrderNo
		{
			get;
			set;
		}

		public int MemberId
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}
	}
}
