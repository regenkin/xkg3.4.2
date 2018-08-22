using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class ActivitySearch : Pagination
	{
		public ActivityStatus status
		{
			get;
			set;
		}

		public System.DateTime? begin
		{
			get;
			set;
		}

		public System.DateTime? end
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
	}
}
