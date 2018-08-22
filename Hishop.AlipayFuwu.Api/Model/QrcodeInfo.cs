using System;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class QrcodeInfo
	{
		public codeInfo codeInfo
		{
			get;
			set;
		}

		public string codeType
		{
			get;
			set;
		}

		public int expireSecond
		{
			get;
			set;
		}

		public string showLogo
		{
			get;
			set;
		}
	}
}
