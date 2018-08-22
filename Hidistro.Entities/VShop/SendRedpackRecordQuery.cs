using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class SendRedpackRecordQuery : Pagination
	{
		public int ID
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}

		public int BalanceDrawRequestID
		{
			get;
			set;
		}

		public bool IsSend
		{
			get;
			set;
		}
	}
}
