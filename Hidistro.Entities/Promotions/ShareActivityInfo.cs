using System;

namespace Hidistro.Entities.Promotions
{
	public class ShareActivityInfo
	{
		private int _id;

		private int _couponid;

		private System.DateTime _begindate;

		private System.DateTime _enddate;

		private decimal _meetvalue;

		private int _couponnumber = 1;

		private string _ActivityName;

		private string _ImgUrl;

		private string _ShareTitle;

		private string _Description;

		public string CouponName
		{
			get;
			set;
		}

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

		public System.DateTime BeginDate
		{
			get
			{
				return this._begindate;
			}
			set
			{
				this._begindate = value;
			}
		}

		public System.DateTime EndDate
		{
			get
			{
				return this._enddate;
			}
			set
			{
				this._enddate = value;
			}
		}

		public decimal MeetValue
		{
			get
			{
				return this._meetvalue;
			}
			set
			{
				this._meetvalue = value;
			}
		}

		public int CouponNumber
		{
			get
			{
				return this._couponnumber;
			}
			set
			{
				this._couponnumber = value;
			}
		}

		public string ActivityName
		{
			get
			{
				return this._ActivityName;
			}
			set
			{
				this._ActivityName = value;
			}
		}

		public string ImgUrl
		{
			get
			{
				return this._ImgUrl;
			}
			set
			{
				this._ImgUrl = value;
			}
		}

		public string ShareTitle
		{
			get
			{
				return this._ShareTitle;
			}
			set
			{
				this._ShareTitle = value;
			}
		}

		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				this._Description = value;
			}
		}
	}
}
