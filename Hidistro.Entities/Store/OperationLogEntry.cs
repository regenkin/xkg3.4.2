using System;

namespace Hidistro.Entities.Store
{
	public class OperationLogEntry
	{
		public long LogId
		{
			get;
			set;
		}

		public Privilege Privilege
		{
			get;
			set;
		}

		public string PageUrl
		{
			get;
			set;
		}

		public System.DateTime AddedTime
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string IpAddress
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}
	}
}
