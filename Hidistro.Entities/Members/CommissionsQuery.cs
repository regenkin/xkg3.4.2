using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class CommissionsQuery : Pagination
	{
		public int UserId
		{
			get;
			set;
		}

		public int ReferralUserId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string OrderNum
		{
			get;
			set;
		}

		public string StartTime
		{
			get;
			set;
		}

		public string EndTime
		{
			get;
			set;
		}
	}
}
