using Hishop.Weixin.MP.Handler;
using System;

namespace Hishop.Weixin.MP.Test
{
	public class A : RequestHandler
	{
		public A(string xml) : base(xml)
		{
		}

		public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
		{
			return null;
		}
	}
}
