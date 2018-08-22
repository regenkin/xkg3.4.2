using System;

namespace Hidistro.Entities.Store
{
	public class RoleInfo
	{
		public virtual int RoleId
		{
			get;
			set;
		}

		public virtual string RoleName
		{
			get;
			set;
		}

		public virtual string Description
		{
			get;
			set;
		}

		public virtual bool IsDefault
		{
			get;
			set;
		}
	}
}
