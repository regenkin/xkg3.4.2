using System;

namespace Hishop.Weixin.MP.Request.Event
{
	public class SubscribeEventRequest : EventRequest
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
				return RequestEventType.Subscribe;
			}
			set
			{
			}
		}
	}
}
