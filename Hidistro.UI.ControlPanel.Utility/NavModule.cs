using System;
using System.Collections.Generic;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class NavModule
	{
		public bool IsDivide;

		public string Title;

		public string Class;

		public string Link;

		public string ID;

		public Dictionary<string, NavItem> ItemList = new Dictionary<string, NavItem>();
	}
}
