using System;

namespace Hidistro.Entities.Promotions
{
	public class PointExChange_ExChangedInfo
	{
		private int _id;

		private int _exchangeid;

		private string _exchangename;

		private int _productid;

		private int _pointnumber = 0;

		private System.DateTime _date;

		private int _memberid;

		private string _membergrades = "0";

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

		public int exChangeId
		{
			get
			{
				return this._exchangeid;
			}
			set
			{
				this._exchangeid = value;
			}
		}

		public string exChangeName
		{
			get
			{
				return this._exchangename;
			}
			set
			{
				this._exchangename = value;
			}
		}

		public int ProductId
		{
			get
			{
				return this._productid;
			}
			set
			{
				this._productid = value;
			}
		}

		public int PointNumber
		{
			get
			{
				return this._pointnumber;
			}
			set
			{
				this._pointnumber = value;
			}
		}

		public System.DateTime Date
		{
			get
			{
				return this._date;
			}
			set
			{
				this._date = value;
			}
		}

		public int MemberID
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
	}
}
