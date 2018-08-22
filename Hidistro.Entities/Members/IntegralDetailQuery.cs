using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class IntegralDetailQuery : Pagination
	{
		public int UserId
		{
			get;
			set;
		}

		public int IntegralSourceType
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

		public int IntegralStatus
		{
			get;
			set;
		}
	}
}
