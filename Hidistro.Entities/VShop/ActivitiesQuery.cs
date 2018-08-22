using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class ActivitiesQuery : Pagination
	{
		public string State
		{
			get;
			set;
		}

		public string ActivitiesName
		{
			get;
			set;
		}
	}
}
