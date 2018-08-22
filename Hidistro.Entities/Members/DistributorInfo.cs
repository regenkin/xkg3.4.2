using System;

namespace Hidistro.Entities.Members
{
	public class DistributorInfo : MemberInfo
	{
		public System.DateTime CreateTime
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string DistributorGradeId
		{
			get;
			set;
		}
	}
}
