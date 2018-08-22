using System;

namespace Hidistro.Entities.Promotions
{
	public class LimitedTimeDiscountInfo
	{
		public int LimitedTimeDiscountId
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		public System.DateTime BeginTime
		{
			get;
			set;
		}

		public System.DateTime EndTime
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int LimitNumber
		{
			get;
			set;
		}

		public string ApplyMembers
		{
			get;
			set;
		}

		public string DefualtGroup
		{
			get;
			set;
		}

		public string CustomGroup
		{
			get;
			set;
		}

		public System.DateTime CreateTime
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}
	}
}
