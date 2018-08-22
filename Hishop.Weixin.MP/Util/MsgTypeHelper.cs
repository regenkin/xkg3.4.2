using System;
using System.Xml.Linq;

namespace Hishop.Weixin.MP.Util
{
	public static class MsgTypeHelper
	{
		public static RequestMsgType GetMsgType(XDocument doc)
		{
			return MsgTypeHelper.GetMsgType(doc.Root.Element("MsgType").Value);
		}

		public static RequestMsgType GetMsgType(string str)
		{
			return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), str, true);
		}
	}
}
