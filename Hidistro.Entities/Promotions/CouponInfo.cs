using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Promotions
{
	[HasSelfValidation]
	public class CouponInfo
	{
		public int CouponId
		{
			get;
			set;
		}

		public string CouponName
		{
			get;
			set;
		}

		public System.DateTime EndDate
		{
			get;
			set;
		}

		public System.DateTime BeginDate
		{
			get;
			set;
		}

		public decimal ConditionValue
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValCoupon", MessageTemplate = "可抵扣金额不能为空，金额大小0.01-1000万之间")]
		public decimal CouponValue
		{
			get;
			set;
		}

		public int StockNum
		{
			get;
			set;
		}

		public int ReceiveNum
		{
			get;
			set;
		}

		public int UsedNum
		{
			get;
			set;
		}

		public string MemberGrades
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

		public string ImgUrl
		{
			get;
			set;
		}

		public int ProductNumber
		{
			get;
			set;
		}

		public bool Finished
		{
			get;
			set;
		}

		public bool IsAllProduct
		{
			get;
			set;
		}

		public string CouponTypes
		{
			get;
			set;
		}

		public int maxReceivNum
		{
			get;
			set;
		}

		public CouponInfo()
		{
		}

		public CouponInfo(string name, System.DateTime closingTime, System.DateTime startTime, decimal amount, decimal discountValue)
		{
			this.CouponName = name;
			this.EndDate = closingTime;
			this.BeginDate = startTime;
			this.ConditionValue = amount;
			this.CouponValue = discountValue;
		}

		public CouponInfo(int couponId, string name, System.DateTime closingTime, System.DateTime startTime, decimal amount, decimal discountValue)
		{
			this.CouponId = couponId;
			this.CouponName = name;
			this.EndDate = closingTime;
			this.BeginDate = startTime;
			this.ConditionValue = amount;
			this.CouponValue = discountValue;
		}

		[SelfValidation(Ruleset = "ValCoupon")]
		public void CompareAmount(ValidationResults result)
		{
			decimal arg_07_0 = this.ConditionValue;
			if (this.CouponValue > this.ConditionValue)
			{
				result.AddResult(new ValidationResult("折扣值不能大于满足金额", this, "", "", null));
			}
		}
	}
}
