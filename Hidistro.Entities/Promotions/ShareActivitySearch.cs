using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class ShareActivitySearch : Pagination
	{
		public string CouponName
		{
			get;
			set;
		}

		public ShareActivityStatus status
		{
			get;
			set;
		}
	}
}
