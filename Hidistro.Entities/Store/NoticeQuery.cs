using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Store
{
	public class NoticeQuery : Pagination
	{
		public string Title
		{
			get;
			set;
		}

		public string Author
		{
			get;
			set;
		}

		public System.DateTime? StartTime
		{
			get;
			set;
		}

		public System.DateTime? EndTime
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public int SendType
		{
			get;
			set;
		}

		public int? IsNotShowRead
		{
			get;
			set;
		}

		public bool? IsDistributor
		{
			get;
			set;
		}

		public int? IsPub
		{
			get;
			set;
		}

		public int? IsDel
		{
			get;
			set;
		}
	}
}
