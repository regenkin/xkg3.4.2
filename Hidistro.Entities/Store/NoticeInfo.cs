using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Store
{
	public class NoticeInfo
	{
		public int Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Memo
		{
			get;
			set;
		}

		public string Author
		{
			get;
			set;
		}

		public System.DateTime AddTime
		{
			get;
			set;
		}

		public int IsPub
		{
			get;
			set;
		}

		public System.DateTime? PubTime
		{
			get;
			set;
		}

		public int SendType
		{
			get;
			set;
		}

		public int SendTo
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<NoticeUserInfo> NoticeUserInfo
		{
			get;
			set;
		}
	}
}
