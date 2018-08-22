using System;

namespace Hidistro.Entities.Promotions
{
	public class MemberCouponsInfo
	{
		private int _id;

		private int _couponid;

		private int? _memberid;

		private System.DateTime? _receivedate;

		private System.DateTime? _useddate;

		private string _orderno;

		private int? _status;

		private string _couponname;

		private decimal? _conditionvalue;

		private System.DateTime? _begindate;

		private System.DateTime? _enddate;

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

		public int? MemberId
		{
			get
			{
				return this._memberid;
			}
			set
			{
				this._memberid = value;
			}
		}

		public System.DateTime? ReceiveDate
		{
			get
			{
				return this._receivedate;
			}
			set
			{
				this._receivedate = value;
			}
		}

		public System.DateTime? UsedDate
		{
			get
			{
				return this._useddate;
			}
			set
			{
				this._useddate = value;
			}
		}

		public string OrderNo
		{
			get
			{
				return this._orderno;
			}
			set
			{
				this._orderno = value;
			}
		}

		public int? Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
			}
		}

		public string CouponName
		{
			get
			{
				return this._couponname;
			}
			set
			{
				this._couponname = value;
			}
		}

		public decimal? ConditionValue
		{
			get
			{
				return this._conditionvalue;
			}
			set
			{
				this._conditionvalue = value;
			}
		}

		public System.DateTime? BeginDate
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

		public System.DateTime? EndDate
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

		public bool IsAllProduct
		{
			get;
			set;
		}

		public decimal CouponValue
		{
			get;
			set;
		}

		public MemberCouponsInfo()
		{
		}

		public MemberCouponsInfo(int couponId)
		{
			this._couponid = couponId;
		}
	}
}
