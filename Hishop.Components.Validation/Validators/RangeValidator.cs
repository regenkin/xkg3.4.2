using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class RangeValidator<T> : ValueValidator<T> where T : IComparable
	{
		private RangeChecker<T> rangeChecker;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.RangeValidatorNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.RangeValidatorNegatedDefaultMessageTemplate;
			}
		}

		public RangeValidator(T upperBound) : this(default(T), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		protected RangeValidator(T upperBound, bool negated) : this(default(T), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive, negated)
		{
		}

		public RangeValidator(T lowerBound, T upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		protected RangeValidator(T lowerBound, T upperBound, bool negated) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive, negated)
		{
		}

		public RangeValidator(T lowerBound, RangeBoundaryType lowerBoundType, T upperBound, RangeBoundaryType upperBoundType) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null)
		{
		}

		protected RangeValidator(T lowerBound, RangeBoundaryType lowerBoundType, T upperBound, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null, negated)
		{
		}

		public RangeValidator(T lowerBound, RangeBoundaryType lowerBoundType, T upperBound, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, false)
		{
			this.rangeChecker = new RangeChecker<T>(lowerBound, lowerBoundType, upperBound, upperBoundType);
		}

		public RangeValidator(T lowerBound, RangeBoundaryType lowerBoundType, T upperBound, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateRangeValidator(lowerBound, lowerBoundType, upperBound, upperBoundType);
			this.rangeChecker = new RangeChecker<T>(lowerBound, lowerBoundType, upperBound, upperBoundType);
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool flag = objectToValidate == null;
			bool flag2 = !flag && !this.rangeChecker.IsInRange(objectToValidate);
			if (flag || flag2 != base.Negated)
			{
				base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
			}
		}

		protected override string GetMessage(object objectToValidate, string key)
		{
			return string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[]
			{
				objectToValidate,
				key,
				base.Tag,
				this.rangeChecker.LowerBound,
				this.rangeChecker.LowerBoundType,
				this.rangeChecker.UpperBound,
				this.rangeChecker.UpperBoundType
			});
		}
	}
	public class RangeValidator : RangeValidator<IComparable>
	{
		public RangeValidator(IComparable upperBound) : this(null, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public RangeValidator(IComparable lowerBound, IComparable upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null)
		{
		}

		public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null, negated)
		{
		}

		public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, false)
		{
		}

		public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, negated)
		{
		}
	}
}
