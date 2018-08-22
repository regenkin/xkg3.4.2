using System;
using System.Globalization;

namespace Hidistro.Core.Configuration
{
	public class RolesConfiguration
	{
		private string member = "Member";

		private string systemAdmin = "SystemAdministrator";

		private string manager = "Manager";

		public string Member
		{
			get
			{
				return this.member;
			}
		}

		public string Manager
		{
			get
			{
				return this.manager;
			}
		}

		public string SystemAdministrator
		{
			get
			{
				return this.systemAdmin;
			}
		}

		public string RoleList()
		{
			return string.Format(CultureInfo.InvariantCulture, "^({0}|{1}|{2})$", new object[]
			{
				this.Member,
				this.SystemAdministrator,
				this.Manager
			});
		}
	}
}
