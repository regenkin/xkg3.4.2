using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.StatisticsReport
{
	public class OrderStatisticsQuery_UnderShop : Pagination
	{
		public int? AgentId
		{
			get;
			set;
		}

		public int? ShopLevel
		{
			get;
			set;
		}

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
	}
}
