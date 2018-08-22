using System;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class UserStatisticsInfo
	{
		public long RegionId
		{
			get;
			set;
		}

		public string RegionName
		{
			get;
			set;
		}

		public int Usercounts
		{
			get;
			set;
		}

		public decimal AllUserCounts
		{
			get;
			set;
		}

		public decimal Percentage
		{
			get
			{
				decimal result;
				if (this.AllUserCounts != 0m)
				{
					result = this.Usercounts / this.AllUserCounts * 100m;
				}
				else
				{
					result = 0m;
				}
				return result;
			}
		}

		public decimal Lenth
		{
			get
			{
				return this.Percentage * 4m;
			}
		}
	}
}
