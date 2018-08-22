using Hishop.Weixin.MP.Domain;
using System;

namespace Hishop.Weixin.MP.Response
{
	public class MusicResponse : AbstractResponse
	{
		public Music Music
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Music;
			}
		}
	}
}
