using System;

namespace Hidistro.Entities.Promotions
{
	public class PointExChangeInfo
	{
		private int _id;

		private string _name;

		private string _membergrades = "0";

		private string _defualtgroup = "0";

		private string _customgroup = "0";

		private System.DateTime _begindate;

		private System.DateTime _enddate;

		private int _productnumber = 0;

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

		public string ImgUrl
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
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
				return this._defualtgroup;
			}
			set
			{
				this._defualtgroup = value;
			}
		}

		public string CustomGroup
		{
			get
			{
				return this._customgroup;
			}
			set
			{
				this._customgroup = value;
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

		public int ProductNumber
		{
			get
			{
				return this._productnumber;
			}
			set
			{
				this._productnumber = value;
			}
		}
	}
}
