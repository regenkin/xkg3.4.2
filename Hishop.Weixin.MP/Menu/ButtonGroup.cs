using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public class ButtonGroup
	{
		public List<BaseButton> button
		{
			get;
			set;
		}

		public ButtonGroup()
		{
			this.button = new List<BaseButton>();
		}
	}
}
