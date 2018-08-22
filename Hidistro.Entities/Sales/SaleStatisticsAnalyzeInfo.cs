using System;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class SaleStatisticsAnalyzeInfo
	{
		public decimal OrderTotals
		{
			get;
			set;
		}

		public int OrderCounts
		{
			get;
			set;
		}

		public int OrderUserCounts
		{
			get;
			set;
		}

		public int VisitCounts
		{
			get;
			set;
		}

		public int UserCounts
		{
			get;
			set;
		}
	}
}
