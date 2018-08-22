using System;

namespace Hidistro.Entities.Promotions
{
	public class PrizeResultViewInfo : PrizeResultInfo
	{
		public PrizeType PrizeType
		{
			get;
			set;
		}

		public int GivePoint
		{
			get;
			set;
		}

		public string GiveCouponId
		{
			get;
			set;
		}

		public string GiveShopBookId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public PrizeGrade PrizeGrade
		{
			get;
			set;
		}

		public string PrizeName
		{
			get;
			set;
		}

		public string Prize
		{
			get;
			set;
		}
	}
}
