using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Settings
{
	public class MenuInfo
	{
		public int MenuId
		{
			get;
			set;
		}

		public int ParentMenuId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public string ShopMenuPic
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<MenuInfo> SubMenus
		{
			get;
			set;
		}
	}
}
