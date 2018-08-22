using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class SaleStatisticsQuery : Pagination
	{
		public string QueryKey
		{
			get;
			set;
		}

		public System.DateTime? StartDate
		{
			get;
			set;
		}

		public System.DateTime? EndDate
		{
			get;
			set;
		}
	}
}
