using System;

namespace Hidistro.Entities.Promotions
{
	public class CouponInfo_MemberWeiXin : CouponInfo
	{
		public string ValidDays
		{
			get;
			set;
		}

		public string OpenId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}
	}
}
