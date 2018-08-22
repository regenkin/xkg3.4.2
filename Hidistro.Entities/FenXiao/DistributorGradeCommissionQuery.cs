using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.FenXiao
{
	public class DistributorGradeCommissionQuery : Pagination
	{
		public string Title
		{
			get;
			set;
		}

		public System.DateTime? StartTime
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
