using System;

namespace Hidistro.Entities.Store
{
	public class SystemAuthorizationInfo
	{
		public SystemAuthorizationState state
		{
			get;
			set;
		}

		public string type
		{
			get;
			set;
		}

		public int DistributorCount
		{
			get;
			set;
		}

		public bool IsShowJixuZhiChi
		{
			get;
			set;
		}
	}
}
