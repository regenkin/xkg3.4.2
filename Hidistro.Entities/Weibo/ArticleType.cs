using Hidistro.Entities.VShop;
using System;

namespace Hidistro.Entities.Weibo
{
	public enum ArticleType
	{
		[EnumShowText("文本")]
		Text = 1,
		[EnumShowText("单图文")]
		News,
		[EnumShowText("多图文")]
		List = 4
	}
}
