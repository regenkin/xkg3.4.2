using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Sales
{
	public class CategoriesQuery : Pagination
	{
		public string Name
		{
			get;
			set;
		}
	}
}
