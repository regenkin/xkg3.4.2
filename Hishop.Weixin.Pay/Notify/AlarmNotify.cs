using System;

namespace Hishop.Weixin.Pay.Notify
{
	public class AlarmNotify : NotifyObject
	{
		public string AppId
		{
			get;
			set;
		}

		public int ErrorType
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string AlarmContent
		{
			get;
			set;
		}

		public long TimeStamp
		{
			get;
			set;
		}

		public string AppSignature
		{
			get;
			set;
		}

		public string SignMethod
		{
			get;
			set;
		}
	}
}
