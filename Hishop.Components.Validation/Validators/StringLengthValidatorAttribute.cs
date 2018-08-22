using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class StringLengthValidatorAttribute : ValueValidatorAttribute
	{
		private int lowerBound;

		private RangeBoundaryType lowerBoundType;

		private int upperBound;

		private RangeBoundaryType upperBoundType;

		public StringLengthValidatorAttribute(int upperBound) : this(0, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public StringLengthValidatorAttribute(int lowerBound, int upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public StringLengthValidatorAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType)
		{
			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new StringLengthValidator(this.lowerBound, this.lowerBoundType, this.upperBound, this.upperBoundType, base.Negated);
		}
	}
}
