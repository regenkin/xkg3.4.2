using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class DistributorGradeQuery : Pagination
	{
		public int GradeId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}
	}
}
