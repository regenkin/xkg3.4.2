using System;
using System.Xml.Linq;

namespace Hishop.Weixin.MP.Util
{
	public static class EventTypeHelper
	{
		public static RequestEventType GetEventType(XDocument doc)
		{
			return EventTypeHelper.GetEventType(doc.Root.Element("Event").Value);
		}

		public static RequestEventType GetEventType(string str)
		{
			return (RequestEventType)Enum.Parse(typeof(RequestEventType), str, true);
		}
	}
}
