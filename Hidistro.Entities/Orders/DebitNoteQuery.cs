using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class DebitNoteQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}
	}
}
