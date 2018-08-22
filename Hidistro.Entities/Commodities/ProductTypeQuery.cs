using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Commodities
{
	public class ProductTypeQuery : Pagination
	{
		public string TypeName
		{
			get;
			set;
		}
	}
}
