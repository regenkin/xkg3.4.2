using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class LotteryActivityQuery : Pagination
	{
		public LotteryActivityType ActivityType
		{
			get;
			set;
		}
	}
}
