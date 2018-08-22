using System;

namespace Hidistro.Entities.VShop
{
	public enum LocationType
	{
		[EnumShowText("投票")]
		Vote = 1,
		[EnumShowText("活动")]
		Activity,
		[EnumShowText("首页")]
		Home,
		[EnumShowText("分类页")]
		Category,
		[EnumShowText("购物车")]
		ShoppingCart,
		[EnumShowText("会员中心")]
		OrderCenter,
		[EnumShowText("链接")]
		Link = 8,
		[EnumShowText("电话")]
		Phone,
		[EnumShowText("地址")]
		Address,
		[EnumShowText("品牌")]
		Brand = 12
	}
}
