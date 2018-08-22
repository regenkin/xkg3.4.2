using Hishop.AlipayFuwu.Api.Util;
using System;
using System.Collections.Generic;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class Articles
	{
		public string toUserId
		{
			get;
			set;
		}

		public string msgType
		{
			get;
			set;
		}

		public string createTime
		{
			get
			{
				return AliOHHelper.TransferToMilStartWith1970(System.DateTime.Now).ToString("F0");
			}
		}

		public MessageText text
		{
			get;
			set;
		}

		public System.Collections.Generic.List<article> articles
		{
			get;
			set;
		}
	}
}
