using Hishop.Weixin.MP.Domain;
using System;

namespace Hishop.Weixin.MP.Response
{
	public class ImageResponse : AbstractResponse
	{
		public Image Image
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Image;
			}
		}
	}
}
