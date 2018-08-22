using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Promotions
{
	public class VoteInfo
	{
		private long _voteid;

		private string _votename;

		private bool _isbackup = false;

		private int _maxcheck;

		private string _imageurl;

		private System.DateTime _startdate;

		private System.DateTime _enddate;

		private string _description;

		private string _membergrades;

		private string _defualtgroup;

		private string _customgroup;

		private bool _isMultiCheck = false;

		public int VoteCounts
		{
			get;
			set;
		}

		public long VoteId
		{
			get
			{
				return this._voteid;
			}
			set
			{
				this._voteid = value;
			}
		}

		public string VoteName
		{
			get
			{
				return this._votename;
			}
			set
			{
				this._votename = value;
			}
		}

		public bool IsBackup
		{
			get
			{
				return this._isbackup;
			}
			set
			{
				this._isbackup = value;
			}
		}

		public int MaxCheck
		{
			get
			{
				return this._maxcheck;
			}
			set
			{
				this._maxcheck = value;
			}
		}

		public string ImageUrl
		{
			get
			{
				return this._imageurl;
			}
			set
			{
				this._imageurl = value;
			}
		}

		public System.DateTime StartDate
		{
			get
			{
				return this._startdate;
			}
			set
			{
				this._startdate = value;
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

		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
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

		public bool IsMultiCheck
		{
			get
			{
				return this._isMultiCheck;
			}
			set
			{
				this._isMultiCheck = value;
			}
		}

		public System.Collections.Generic.IList<VoteItemInfo> VoteItems
		{
			get;
			set;
		}

		public int VoteAttends
		{
			get;
			set;
		}
	}
}
