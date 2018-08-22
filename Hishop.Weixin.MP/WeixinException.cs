using System;

namespace Hishop.Weixin.MP
{
	public class WeixinException : Exception
	{
		public WeixinException()
		{
		}

		public WeixinException(string message) : base(message)
		{
		}

		public WeixinException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
