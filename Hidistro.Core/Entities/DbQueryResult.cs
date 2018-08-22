using System;

namespace Hidistro.Core.Entities
{
	public class DbQueryResult
	{
		public int TotalRecords
		{
			get;
			set;
		}

		public object Data
		{
			get;
			set;
		}
	}
}
