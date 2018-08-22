using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class ExChangeSearch : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public bool? bFinished
		{
			get;
			set;
		}

		public ExchangeStatus status
		{
			get;
			set;
		}
	}
}
