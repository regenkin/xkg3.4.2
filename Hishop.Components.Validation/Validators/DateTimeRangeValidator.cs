using System;

namespace Hishop.Components.Validation.Validators
{
	public class DateTimeRangeValidator : RangeValidator<DateTime>
	{
		public DateTimeRangeValidator(DateTime upperBound) : base(upperBound)
		{
		}

		public DateTimeRangeValidator(DateTime upperBound, bool negated) : base(upperBound, negated)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, DateTime upperBound) : base(lowerBound, upperBound)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, DateTime upperBound, bool negated) : base(lowerBound, upperBound, negated)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType) : base(lowerBound, lowerBoundType, upperBound, upperBoundType)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType, bool negated) : base(lowerBound, lowerBoundType, upperBound, upperBoundType, negated)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType, string messageTemplate) : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate)
		{
		}

		public DateTimeRangeValidator(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, negated)
		{
		}
	}
}
