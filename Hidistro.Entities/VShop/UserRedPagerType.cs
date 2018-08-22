using System;

namespace Hidistro.Entities.VShop
{
	public enum UserRedPagerType
	{
		[EnumShowText("所有")]
		All,
		[EnumShowText("可用")]
		Usable,
		[EnumShowText("已过期")]
		Expiry
	}
}
