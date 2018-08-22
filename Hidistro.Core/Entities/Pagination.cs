using Hidistro.Core.Enums;
using System;

namespace Hidistro.Core.Entities
{
	public class Pagination
	{
		public int PageIndex
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public string SortBy
		{
			get;
			set;
		}

		public SortAction SortOrder
		{
			get;
			set;
		}

		public bool IsCount
		{
			get;
			set;
		}

		public int DeleteBeforeState
		{
			get;
			set;
		}

		public Pagination()
		{
			this.IsCount = true;
			this.PageSize = 10;
		}
	}
}
