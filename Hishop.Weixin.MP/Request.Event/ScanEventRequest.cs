using System;

namespace Hishop.Weixin.MP.Request.Event
{
	public class ScanEventRequest : EventRequest
	{
		public string EventKey
		{
			get;
			set;
		}

		public string Ticket
		{
			get;
			set;
		}

		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Scan;
			}
			set
			{
			}
		}
	}
}
