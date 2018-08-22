using System;

namespace Hishop.Plugins.Integration
{
	public abstract class IntegrateProvider
	{
		public static IntegrateProvider Instance(string applicationType, string configStr)
		{
			Type type = Type.GetType(applicationType);
			IntegrateProvider integrateProvider = Activator.CreateInstance(type) as IntegrateProvider;
			integrateProvider.Init(configStr);
			return integrateProvider;
		}

		protected abstract void Init(string configStr);

		public abstract void Register(string username, int gender, string password, string email, string regip, string qq, string msn);

		public abstract void ChangePassword(string username, string password);

		public abstract void Login(string username, string password, string returnUrl);

		public abstract void AdminLogin(string username, string password, string returnUrl);

		public abstract void Logout();

		public abstract int GetUserID(string username);

		public abstract void DeleteUser(int userId);
	}
}
