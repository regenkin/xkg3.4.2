using System;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public class SingleViewButton : SingleButton
	{
		public string url
		{
			get;
			set;
		}

		public SingleViewButton() : base(ButtonType.view.ToString())
		{
		}
	}
}
