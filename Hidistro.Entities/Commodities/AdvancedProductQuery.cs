using System;

namespace Hidistro.Entities.Commodities
{
	public class AdvancedProductQuery : ProductQuery
	{
		public bool IncludeOnSales
		{
			get;
			set;
		}

		public bool IncludeUnSales
		{
			get;
			set;
		}

		public bool IncludeInStock
		{
			get;
			set;
		}
	}
}
