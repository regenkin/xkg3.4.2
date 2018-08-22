using System;

namespace Hidistro.Entities.Promotions
{
	public class PointExchangeChangedInfo
	{
		public int exChangeId
		{
			get;
			set;
		}

		public string exChangeName
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int PointNumber
		{
			get;
			set;
		}

		public int MemberID
		{
			get;
			set;
		}

		public int MemberGrades
		{
			get;
			set;
		}

		public System.DateTime Date
		{
			get;
			set;
		}
	}
}
