using System;

namespace Hidistro.Entities.Promotions
{
	public class GameWinningPool
	{
		public int WinningPoolId
		{
			get;
			set;
		}

		public int GameId
		{
			get;
			set;
		}

		public int Number
		{
			get;
			set;
		}

		public int GamePrizeId
		{
			get;
			set;
		}

		public int IsReceive
		{
			get;
			set;
		}
	}
}
