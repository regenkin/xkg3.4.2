using Hidistro.Core.Enums;
using System;

namespace Hidistro.Entities.Members
{
	public class DistributorsInfo : MemberInfo
	{
		public string StoreName
		{
			get;
			set;
		}

		public DistributorGrade DistributorGradeId
		{
			get;
			set;
		}

		public int DistriGradeId
		{
			get;
			set;
		}

		public int? ParentUserId
		{
			get;
			set;
		}

		public string ReferralPath
		{
			get;
			set;
		}

		public int ReferralOrders
		{
			get;
			set;
		}

		public decimal ReferralBlance
		{
			get;
			set;
		}

		public decimal ReferralRequestBalance
		{
			get;
			set;
		}

		public System.DateTime CreateTime
		{
			get;
			set;
		}

		public System.DateTime? AccountTime
		{
			get;
			set;
		}

		public int ReferralStatus
		{
			get;
			set;
		}

		public string Logo
		{
			get;
			set;
		}

		public string BackImage
		{
			get;
			set;
		}

		public string StoreDescription
		{
			get;
			set;
		}

		public string RequestAccount
		{
			get;
			set;
		}

		public decimal OrdersTotal
		{
			get;
			set;
		}

		public string StoreCard
		{
			get;
			set;
		}

		public System.DateTime CardCreatTime
		{
			get;
			set;
		}
	}
}
