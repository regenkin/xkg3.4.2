using System;

namespace Hidistro.Entities.Promotions
{
	public class GameInfo
	{
		public int GameId
		{
			get;
			set;
		}

		public GameType GameType
		{
			get;
			set;
		}

		public string GameTitle
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public System.DateTime BeginTime
		{
			get;
			set;
		}

		public System.DateTime EndTime
		{
			get;
			set;
		}

		public string ApplyMembers
		{
			get;
			set;
		}

		public string DefualtGroup
		{
			get;
			set;
		}

		public string CustomGroup
		{
			get;
			set;
		}

		public int NeedPoint
		{
			get;
			set;
		}

		public int GivePoint
		{
			get;
			set;
		}

		public bool OnlyGiveNotPrizeMember
		{
			get;
			set;
		}

		public PlayType PlayType
		{
			get;
			set;
		}

		public string NotPrzeDescription
		{
			get;
			set;
		}

		public string GameUrl
		{
			get;
			set;
		}

		public string GameQRCodeAddress
		{
			get;
			set;
		}

		public GameStatus Status
		{
			get;
			set;
		}

		public string KeyWork
		{
			get;
			set;
		}

		public float PrizeRate
		{
			get;
			set;
		}

		public string WinningPool
		{
			get;
			set;
		}

		public int LimitEveryDay
		{
			get;
			set;
		}

		public int MaximumDailyLimit
		{
			get;
			set;
		}

		public int MemberCheck
		{
			get;
			set;
		}
	}
}
