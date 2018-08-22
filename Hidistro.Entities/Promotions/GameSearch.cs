using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class GameSearch : Pagination
	{
		public string Status
		{
			get;
			set;
		}

		public int? GameType
		{
			get;
			set;
		}

		public System.DateTime? BeginTime
		{
			get;
			set;
		}

		public System.DateTime? EndTime
		{
			get;
			set;
		}
	}
}
