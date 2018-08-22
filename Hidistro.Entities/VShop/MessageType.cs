using System;

namespace Hidistro.Entities.VShop
{
	public enum MessageType
	{
		[EnumShowText("文本")]
		Text = 1,
		[EnumShowText("单图文")]
		News,
		[EnumShowText("多图文")]
		List = 4
	}
}
