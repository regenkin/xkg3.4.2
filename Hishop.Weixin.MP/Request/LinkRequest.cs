using System;

namespace Hishop.Weixin.MP.Request
{
	public class LinkRequest : AbstractRequest
	{
		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}
	}
}
