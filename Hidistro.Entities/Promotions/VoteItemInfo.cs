using System;

namespace Hidistro.Entities.Promotions
{
	public class VoteItemInfo
	{
		private long _voteitemid;

		private long _voteid;

		private string _voteitemname;

		private int _itemcount = 0;

		public long VoteItemId
		{
			get
			{
				return this._voteitemid;
			}
			set
			{
				this._voteitemid = value;
			}
		}

		public decimal Percentage
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

		public string VoteItemName
		{
			get
			{
				return this._voteitemname;
			}
			set
			{
				this._voteitemname = value;
			}
		}

		public int ItemCount
		{
			get
			{
				return this._itemcount;
			}
			set
			{
				this._itemcount = value;
			}
		}
	}
}
