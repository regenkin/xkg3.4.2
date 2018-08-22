using System;

namespace Hishop.MeiQia.Api.Domain
{
	public class Token
	{
		public string access_token
		{
			get;
			set;
		}

		public int expires_in
		{
			get;
			set;
		}
	}
}
