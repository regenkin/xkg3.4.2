using System;

namespace Hidistro.Entities.Members
{
	public class DistributorGradeCommissionInfo
	{
		public int Id
		{
			get;
			set;
		}

		public int CommType
		{
			get;
			set;
		}

		public int ReferralUserId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public decimal Commission
		{
			get;
			set;
		}

		public System.DateTime PubTime
		{
			get;
			set;
		}

		public string OperAdmin
		{
			get;
			set;
		}

		public string Memo
		{
			get;
			set;
		}

		public string OrderID
		{
			get;
			set;
		}

		public decimal OldCommissionTotal
		{
			get;
			set;
		}
	}
}
