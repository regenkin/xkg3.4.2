using System;

namespace Hishop.Weixin.Pay.Lib
{
	public class WxPayException : Exception
	{
		public WxPayException(string msg) : base(msg)
		{
		}
	}
}
