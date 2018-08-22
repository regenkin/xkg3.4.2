using System;

namespace Hidistro.Entities.Promotions
{
	public enum CanPrizeError
	{
		可以玩,
		会员等级不在此活动范围内,
		积分不够,
		超过每人每天限次,
		超过每人最多限次,
		此抽奖活动不在有效期内 = 7,
		此抽奖活动还没开始
	}
}
