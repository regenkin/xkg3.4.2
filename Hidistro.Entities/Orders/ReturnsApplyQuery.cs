using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class ReturnsApplyQuery : Pagination
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

		public string ReturnsId
		{
			get;
			set;
		}
	}
}
