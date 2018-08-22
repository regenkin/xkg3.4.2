using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Commodities
{
	public class GroupBuyQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public int State
		{
			get;
			set;
		}
	}
}
