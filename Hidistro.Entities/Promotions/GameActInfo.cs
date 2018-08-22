using System;

namespace Hidistro.Entities.Promotions
{
	public class GameActInfo
	{
		private int _gameid;

		private eGameType _gametype;

		private string _gamename;

		private System.DateTime _begindate;

		private System.DateTime _enddate;

		private string _decription;

		private string _membergrades = "0";

		private int _usepoint = 0;

		private int _givepoint = 0;

		private bool _bonlynotwinner = false;

		private int _attendtimes = 0;

		private string _imgurl = "0";

		private int _status = 0;

		private string _unwindecrip;

		private int _createstep = 1;

		public int GameId
		{
			get
			{
				return this._gameid;
			}
			set
			{
				this._gameid = value;
			}
		}

		public eGameType GameType
		{
			get
			{
				return this._gametype;
			}
			set
			{
				this._gametype = value;
			}
		}

		public string GameName
		{
			get
			{
				return this._gamename;
			}
			set
			{
				this._gamename = value;
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

		public string Decription
		{
			get
			{
				return this._decription;
			}
			set
			{
				this._decription = value;
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

		public int usePoint
		{
			get
			{
				return this._usepoint;
			}
			set
			{
				this._usepoint = value;
			}
		}

		public int GivePoint
		{
			get
			{
				return this._givepoint;
			}
			set
			{
				this._givepoint = value;
			}
		}

		public bool bOnlyNotWinner
		{
			get
			{
				return this._bonlynotwinner;
			}
			set
			{
				this._bonlynotwinner = value;
			}
		}

		public int attendTimes
		{
			get
			{
				return this._attendtimes;
			}
			set
			{
				this._attendtimes = value;
			}
		}

		public string ImgUrl
		{
			get
			{
				return this._imgurl;
			}
			set
			{
				this._imgurl = value;
			}
		}

		public int status
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

		public string unWinDecrip
		{
			get
			{
				return this._unwindecrip;
			}
			set
			{
				this._unwindecrip = value;
			}
		}

		public int CreateStep
		{
			get
			{
				return this._createstep;
			}
			set
			{
				this._createstep = value;
			}
		}
	}
}
