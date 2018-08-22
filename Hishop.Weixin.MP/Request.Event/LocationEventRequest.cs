using System;

namespace Hishop.Weixin.MP.Request.Event
{
	public class LocationEventRequest : EventRequest
	{
		public float Latitude
		{
			get;
			set;
		}

		public float Longitude
		{
			get;
			set;
		}

		public float Precision
		{
			get;
			set;
		}

		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Location;
			}
			set
			{
			}
		}
	}
}
