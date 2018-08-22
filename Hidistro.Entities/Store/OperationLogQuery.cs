using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Store
{
	public class OperationLogQuery
	{
		public Pagination Page
		{
			get;
			set;
		}

		public System.DateTime? FromDate
		{
			get;
			set;
		}

		public System.DateTime? ToDate
		{
			get;
			set;
		}

		public string OperationUserName
		{
			get;
			set;
		}

		public OperationLogQuery()
		{
			this.Page = new Pagination();
		}
	}
}
