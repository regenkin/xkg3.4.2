using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public class SubMenu : BaseButton
	{
		public List<SingleButton> sub_button
		{
			get;
			set;
		}

		public SubMenu()
		{
			this.sub_button = new List<SingleButton>();
		}

		public SubMenu(string name) : this()
		{
			base.name = name;
		}
	}
}
