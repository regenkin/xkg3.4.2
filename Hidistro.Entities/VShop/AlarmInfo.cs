using System;

namespace Hidistro.Entities.VShop
{
	public class AlarmInfo
	{
		public int AlarmNotifyId
		{
			get;
			set;
		}

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

		public System.DateTime TimeStamp
		{
			get;
			set;
		}

		public AlarmInfo()
		{
			this.TimeStamp = System.DateTime.Now;
		}
	}
}
