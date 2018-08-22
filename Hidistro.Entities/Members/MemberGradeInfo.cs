using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Members
{
	public class MemberGradeInfo
	{
		public int GradeId
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValMemberGrade", MessageTemplate = "会员等级名称不能为空，长度限制在60个字符以内")]
		public string Name
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValMemberGrade", MessageTemplate = "备注的长度限制在100个字符以内")]
		public string Description
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 2147483647, RangeBoundaryType.Inclusive, Ruleset = "ValMemberGrade", MessageTemplate = "满足积分为大于等于0的整数")]
		public int Points
		{
			get;
			set;
		}

		public bool IsDefault
		{
			get;
			set;
		}

		[RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValMemberGrade", MessageTemplate = "等级折扣只能是1-100之间的整数")]
		public int Discount
		{
			get;
			set;
		}

		public double? TranVol
		{
			get;
			set;
		}

		public int? TranTimes
		{
			get;
			set;
		}
	}
}
