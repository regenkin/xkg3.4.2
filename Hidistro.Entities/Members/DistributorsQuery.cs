using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class DistributorsQuery : Pagination
	{
		public int UserId
		{
			get;
			set;
		}

		public int GradeId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string StoreName1
		{
			get;
			set;
		}

		public int ReferralStatus
		{
			get;
			set;
		}

		public System.DateTime StartTime
		{
			get;
			set;
		}

		public System.DateTime EndTime
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string MicroSignal
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string ReferralPath
		{
			get;
			set;
		}
	}
}
