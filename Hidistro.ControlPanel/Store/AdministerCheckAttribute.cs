using System;

namespace Hidistro.ControlPanel.Store
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
	public class AdministerCheckAttribute : Attribute
	{
		private bool administratorOnly;

		public bool AdministratorOnly
		{
			get
			{
				return this.administratorOnly;
			}
		}

		public AdministerCheckAttribute()
		{
		}

		public AdministerCheckAttribute(bool administratorOnly)
		{
			this.administratorOnly = administratorOnly;
		}
	}
}
