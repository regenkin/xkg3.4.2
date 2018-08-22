using System;

namespace Hidistro.Entities.Members
{
	public class MemberClientSet
	{
		public int ClientTypeId
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

		public int LastDay
		{
			get;
			set;
		}

		public string ClientChar
		{
			get;
			set;
		}

		public decimal ClientValue
		{
			get;
			set;
		}
	}
}
