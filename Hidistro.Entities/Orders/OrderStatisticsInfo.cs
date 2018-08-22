using System;
using System.Data;

namespace Hidistro.Entities.Orders
{
	public class OrderStatisticsInfo
	{
		public DataTable OrderTbl
		{
			get;
			set;
		}

		public int TotalCount
		{
			get;
			set;
		}

		public decimal TotalOfPage
		{
			get;
			set;
		}

		public decimal ProfitsOfPage
		{
			get;
			set;
		}

		public decimal TotalOfSearch
		{
			get;
			set;
		}

		public decimal ProfitsOfSearch
		{
			get;
			set;
		}
	}
}
