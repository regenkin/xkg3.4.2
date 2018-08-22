using System;

namespace Hidistro.Entities.VShop
{
	public class FeedBackInfo
	{
		public int FeedBackNotifyID
		{
			get;
			set;
		}

		public string AppId
		{
			get;
			set;
		}

		public System.DateTime TimeStamp
		{
			get;
			set;
		}

		public string OpenId
		{
			get;
			set;
		}

		public string MsgType
		{
			get;
			set;
		}

		public string FeedBackId
		{
			get;
			set;
		}

		public string TransId
		{
			get;
			set;
		}

		public string Reason
		{
			get;
			set;
		}

		public string Solution
		{
			get;
			set;
		}

		public string ExtInfo
		{
			get;
			set;
		}

		public FeedBackInfo()
		{
			this.TimeStamp = System.DateTime.Now;
		}
	}
}
