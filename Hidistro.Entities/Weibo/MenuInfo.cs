using System;

namespace Hidistro.Entities.Weibo
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
	}
}
