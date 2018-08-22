using System;

namespace Hishop.Weixin.MP.Request
{
	public class VideoRequest : AbstractRequest
	{
		public string MediaId
		{
			get;
			set;
		}

		public string ThumbMediaId
		{
			get;
			set;
		}
	}
}
