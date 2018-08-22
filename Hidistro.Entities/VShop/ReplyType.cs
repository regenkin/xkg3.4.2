using System;

namespace Hidistro.Entities.VShop
{
	public enum ReplyType
	{
		None,
		[EnumShowText("关注时回复")]
		Subscribe,
		[EnumShowText("关键字回复")]
		Keys,
		[EnumShowText("无匹配回复")]
		NoMatch = 4,
		[EnumShowText("大转盘")]
		Wheel = 8,
		[EnumShowText("刮刮卡")]
		Scratch = 16,
		[EnumShowText("砸金蛋")]
		SmashEgg = 32,
		[EnumShowText("微抽奖")]
		Ticket = 64,
		[EnumShowText("微投票")]
		Vote = 128,
		[EnumShowText("微报名")]
		SignUp = 256,
		[EnumShowText("微专题")]
		Topic = 512
	}
}
