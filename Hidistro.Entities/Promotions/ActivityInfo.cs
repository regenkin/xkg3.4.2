using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Promotions
{
	public class ActivityInfo
	{
		private int _activitiesid;

		private string _activitiesname;

		private int? _activitiestype;

		private System.DateTime _starttime;

		private System.DateTime _endtime;

		private string _activitiesdescription;

		private int? _takeeffect;

		private int? _type;

		private string _membergrades = "0";

		private string _defualtGroup = "0";

		private string _customGroup = "0";

		private int _attendtime = 0;

		private int _attendtype;

		private bool _isallproduct = true;

		private decimal _meetmoney = 0m;

		private decimal _reductionmoney = 0m;

		public int MeetType
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<ActivityDetailInfo> Details
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

		public string ActivitiesName
		{
			get
			{
				return this._activitiesname;
			}
			set
			{
				this._activitiesname = value;
			}
		}

		public int? ActivitiesType
		{
			get
			{
				return this._activitiestype;
			}
			set
			{
				this._activitiestype = value;
			}
		}

		public System.DateTime StartTime
		{
			get
			{
				return this._starttime;
			}
			set
			{
				this._starttime = value;
			}
		}

		public System.DateTime EndTime
		{
			get
			{
				return this._endtime;
			}
			set
			{
				this._endtime = value;
			}
		}

		public string ActivitiesDescription
		{
			get
			{
				return this._activitiesdescription;
			}
			set
			{
				this._activitiesdescription = value;
			}
		}

		public int? TakeEffect
		{
			get
			{
				return this._takeeffect;
			}
			set
			{
				this._takeeffect = value;
			}
		}

		public int? Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		public string MemberGrades
		{
			get
			{
				return this._membergrades;
			}
			set
			{
				this._membergrades = value;
			}
		}

		public string DefualtGroup
		{
			get
			{
				return this._defualtGroup;
			}
			set
			{
				this._defualtGroup = value;
			}
		}

		public string CustomGroup
		{
			get
			{
				return this._customGroup;
			}
			set
			{
				this._customGroup = value;
			}
		}

		public int attendTime
		{
			get
			{
				return this._attendtime;
			}
			set
			{
				this._attendtime = value;
			}
		}

		public int attendType
		{
			get
			{
				return this._attendtype;
			}
			set
			{
				this._attendtype = value;
			}
		}

		public bool isAllProduct
		{
			get
			{
				return this._isallproduct;
			}
			set
			{
				this._isallproduct = value;
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
	}
}
