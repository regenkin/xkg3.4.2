using System;

namespace Hishop.Weixin.MP.Request.Event
{
	public class MassendJobFinishEventRequest : EventRequest
	{
		public string Status
		{
			get;
			set;
		}

		public string TotalCount
		{
			get;
			set;
		}

		public string FilterCount
		{
			get;
			set;
		}

		public string SentCount
		{
			get;
			set;
		}

		public string ErrorCount
		{
			get;
			set;
		}

		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.MASSSENDJOBFINISH;
			}
			set
			{
			}
		}
	}
}
