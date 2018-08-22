using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class RedPagerActivityQuery : Pagination
	{
		public int RedPagerActivityId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}
	}
}
