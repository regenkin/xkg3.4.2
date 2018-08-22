using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class BalanceDrawRequestQuery : Pagination
	{
		public string StoreName
		{
			get;
			set;
		}

		public string RequestTime
		{
			get;
			set;
		}

		public string CheckTime
		{
			get;
			set;
		}

		public string IsCheck
		{
			get;
			set;
		}

		public string RequestStartTime
		{
			get;
			set;
		}

		public string RequestEndTime
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}
	}
}
