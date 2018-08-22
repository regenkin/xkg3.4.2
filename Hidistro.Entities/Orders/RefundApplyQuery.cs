using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class RefundApplyQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}

		public int? HandleStatus
		{
			get;
			set;
		}
	}
}
