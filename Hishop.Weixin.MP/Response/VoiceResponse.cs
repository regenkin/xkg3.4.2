using Hishop.Weixin.MP.Domain;
using System;

namespace Hishop.Weixin.MP.Response
{
	public class VoiceResponse : AbstractResponse
	{
		public Voice Voice
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Voice;
			}
		}
	}
}
