using System;

namespace Hidistro.Entities.Promotions
{
	public class CouponItemInfo
	{
		public int CouponId
		{
			get;
			set;
		}

		public string ClaimCode
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public System.DateTime GenerateTime
		{
			get;
			set;
		}

		public int? CouponStatus
		{
			get;
			set;
		}

		public System.DateTime? UsedTime
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public CouponItemInfo()
		{
		}

		public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, System.DateTime generateTime)
		{
			this.CouponId = couponId;
			this.ClaimCode = claimCode;
			this.UserId = userId;
			this.UserName = username;
			this.EmailAddress = emailAddress;
			this.GenerateTime = generateTime;
		}
	}
}
