using System;

namespace Hishop.Weixin.Pay.Domain
{
	public class PayRequestInfo
	{
		public string appId
		{
			get;
			set;
		}

		public string package
		{
			get;
			set;
		}

		public string timeStamp
		{
			get;
			set;
		}

		public string nonceStr
		{
			get;
			set;
		}

		public string paySign
		{
			get;
			set;
		}

		public string signType
		{
			get
			{
				return "MD5";
			}
		}
	}
}
