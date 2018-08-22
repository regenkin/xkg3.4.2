using System;
using System.Collections.Generic;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class NavItem
	{
		public string SpanName;

		public string Class;

		public string ID;

		public Dictionary<string, NavPageLink> PageLinks = new Dictionary<string, NavPageLink>();
	}
}
