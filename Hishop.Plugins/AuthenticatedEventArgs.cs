using System;

namespace Hishop.Plugins
{
	public class AuthenticatedEventArgs : EventArgs
	{
		public string OpenId
		{
			get;
			private set;
		}

		public AuthenticatedEventArgs(string openId)
		{
			this.OpenId = openId;
		}
	}
}
