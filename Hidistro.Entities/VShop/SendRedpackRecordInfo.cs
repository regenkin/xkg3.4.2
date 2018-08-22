using System;

namespace Hidistro.Entities.VShop
{
	public class SendRedpackRecordInfo
	{
		public int ID
		{
			get;
			set;
		}

		public int BalanceDrawRequestID
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}

		public string OpenID
		{
			get;
			set;
		}

		public int Amount
		{
			get;
			set;
		}

		public string ActName
		{
			get;
			set;
		}

		public string Wishing
		{
			get;
			set;
		}

		public string ClientIP
		{
			get;
			set;
		}

		public bool IsSend
		{
			get;
			set;
		}

		public System.DateTime SendTime
		{
			get;
			set;
		}
	}
}
