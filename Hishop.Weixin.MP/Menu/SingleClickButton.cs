using System;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public class SingleClickButton : SingleButton
	{
		public string key
		{
			get;
			set;
		}

		public SingleClickButton() : base(ButtonType.click.ToString())
		{
		}
	}
}
