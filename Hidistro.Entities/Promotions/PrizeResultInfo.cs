using System;

namespace Hidistro.Entities.Promotions
{
	public class PrizeResultInfo
	{
		public int LogId
		{
			get;
			set;
		}

		public int GameId
		{
			get;
			set;
		}

		public int PrizeId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public System.DateTime PlayTime
		{
			get;
			set;
		}

		public bool IsUsed
		{
			get;
			set;
		}
	}
}
