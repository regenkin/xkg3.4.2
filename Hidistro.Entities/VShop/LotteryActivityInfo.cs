using System;
using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	public class LotteryActivityInfo
	{
		public int ActivityId
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		public int ActivityType
		{
			get;
			set;
		}

		public string ActivityKey
		{
			get;
			set;
		}

		public System.DateTime StartTime
		{
			get;
			set;
		}

		public System.DateTime EndTime
		{
			get;
			set;
		}

		public string ActivityDesc
		{
			get;
			set;
		}

		public string ActivityPic
		{
			get;
			set;
		}

		public System.Collections.Generic.List<PrizeSetting> PrizeSettingList
		{
			get;
			set;
		}

		public string PrizeSetting
		{
			get;
			set;
		}

		public int MaxNum
		{
			get;
			set;
		}
	}
}
