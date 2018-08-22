using System;

namespace Hidistro.Entities.Promotions
{
	public class CouponEdit
	{
		public int? totalNum
		{
			get;
			set;
		}

		public int? maxReceivNum
		{
			get;
			set;
		}

		public System.DateTime? begin
		{
			get;
			set;
		}

		public System.DateTime? end
		{
			get;
			set;
		}
	}
}
