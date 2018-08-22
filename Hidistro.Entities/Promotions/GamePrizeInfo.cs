using System;

namespace Hidistro.Entities.Promotions
{
	public class GamePrizeInfo
	{
		public int PrizeId
		{
			get;
			set;
		}

		public int GameId
		{
			get;
			set;
		}

		public PrizeGrade PrizeGrade
		{
			get;
			set;
		}

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

		public string GriveShopBookPicUrl
		{
			get;
			set;
		}

		public int PrizeCount
		{
			get;
			set;
		}

		public int PrizeRate
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

		public int IsLogistics
		{
			get;
			set;
		}

		public string PrizeImage
		{
			get;
			set;
		}
	}
}
