using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class UserRedPagerQuery : Pagination
	{
		public int RedPagerID
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}

		public bool IsUsed
		{
			get;
			set;
		}

		public UserRedPagerType Type
		{
			get;
			set;
		}
	}
}
