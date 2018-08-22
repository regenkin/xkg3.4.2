using System;

namespace Hidistro.Entities.Store
{
	public class ManagerInfo
	{
		public virtual int UserId
		{
			get;
			protected set;
		}

		public virtual int RoleId
		{
			get;
			set;
		}

		public virtual string UserName
		{
			get;
			set;
		}

		public virtual string Password
		{
			get;
			set;
		}

		public virtual string Email
		{
			get;
			set;
		}

		public virtual System.DateTime CreateDate
		{
			get;
			set;
		}

		public ManagerInfo()
		{
			this.CreateDate = System.DateTime.Now;
		}
	}
}
