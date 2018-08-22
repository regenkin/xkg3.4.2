using System;

namespace Hidistro.Entities.VShop
{
	public class LotteryTicketInfo : LotteryActivityInfo
	{
		public string GradeIds
		{
			get;
			set;
		}

		public int MinValue
		{
			get;
			set;
		}

		public string InvitationCode
		{
			get;
			set;
		}

		public System.DateTime OpenTime
		{
			get;
			set;
		}

		public bool IsOpened
		{
			get;
			set;
		}
	}
}
