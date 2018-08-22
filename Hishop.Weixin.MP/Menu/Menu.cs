using System;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public class Menu
	{
		public ButtonGroup menu
		{
			get;
			set;
		}

		public Menu()
		{
			this.menu = new ButtonGroup();
		}
	}
}
