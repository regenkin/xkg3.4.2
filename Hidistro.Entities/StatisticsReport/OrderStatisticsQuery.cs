using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.StatisticsReport
{
	public class OrderStatisticsQuery : Pagination
	{
		public int? Top
		{
			get;
			set;
		}

		public System.DateTime? BeginDate
		{
			get;
			set;
		}

		public System.DateTime? EndDate
		{
			get;
			set;
		}

		public bool IsNoPage
		{
			get;
			set;
		}
	}
}
