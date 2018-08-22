using System;

namespace Hidistro.Entities.Promotions
{
	public class ActivityDetailInfo
	{
		private int _id;

		private int _activitiesid;

		private decimal _meetmoney = 0m;

		private decimal _reductionmoney = 0m;

		private bool _bfreeshipping = false;

		private int _integral;

		private int _couponid = 0;

		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		public int MeetNumber
		{
			get;
			set;
		}

		public int ActivitiesId
		{
			get
			{
				return this._activitiesid;
			}
			set
			{
				this._activitiesid = value;
			}
		}

		public decimal MeetMoney
		{
			get
			{
				return this._meetmoney;
			}
			set
			{
				this._meetmoney = value;
			}
		}

		public decimal ReductionMoney
		{
			get
			{
				return this._reductionmoney;
			}
			set
			{
				this._reductionmoney = value;
			}
		}

		public bool bFreeShipping
		{
			get
			{
				return this._bfreeshipping;
			}
			set
			{
				this._bfreeshipping = value;
			}
		}

		public int Integral
		{
			get
			{
				return this._integral;
			}
			set
			{
				this._integral = value;
			}
		}

		public int CouponId
		{
			get
			{
				return this._couponid;
			}
			set
			{
				this._couponid = value;
			}
		}
	}
}
