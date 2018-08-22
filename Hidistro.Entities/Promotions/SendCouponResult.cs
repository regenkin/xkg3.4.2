using System;

namespace Hidistro.Entities.Promotions
{
	public enum SendCouponResult
	{
		正常领取,
		优惠券已结束,
		会员不在此活动范内,
		已领完,
		此用户已到领取上限,
		其它错误
	}
}
