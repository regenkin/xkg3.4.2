using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class CouponItemInfoQuery : Pagination
	{
		public int? CouponId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string CounponName
		{
			get;
			set;
		}

		public int? CouponStatus
		{
			get;
			set;
		}
	}
}
