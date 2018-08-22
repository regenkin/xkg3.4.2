using System;

namespace Hishop.Weixin.MP.Request
{
	public class VoiceRequest : AbstractRequest
	{
		public string MediaId
		{
			get;
			set;
		}

		public string Format
		{
			get;
			set;
		}
	}
}
