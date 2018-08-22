using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class DateTimeRangeValidatorAttribute : ValueValidatorAttribute
	{
		private DateTime lowerBound;

		private RangeBoundaryType lowerBoundType;

		private DateTime upperBound;

		private RangeBoundaryType upperBoundType;

		public DateTimeRangeValidatorAttribute(string upperBound) : this(DateTimeRangeValidatorAttribute.ConvertToISO8601Date(upperBound))
		{
		}

		public DateTimeRangeValidatorAttribute(DateTime upperBound) : this(default(DateTime), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public DateTimeRangeValidatorAttribute(string lowerBound, string upperBound) : this(DateTimeRangeValidatorAttribute.ConvertToISO8601Date(lowerBound), DateTimeRangeValidatorAttribute.ConvertToISO8601Date(upperBound))
		{
		}

		public DateTimeRangeValidatorAttribute(DateTime lowerBound, DateTime upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public DateTimeRangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this(DateTimeRangeValidatorAttribute.ConvertToISO8601Date(lowerBound), lowerBoundType, DateTimeRangeValidatorAttribute.ConvertToISO8601Date(upperBound), upperBoundType)
		{
		}

		public DateTimeRangeValidatorAttribute(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType)
		{
			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new DateTimeRangeValidator(this.lowerBound, this.lowerBoundType, this.upperBound, this.upperBoundType, base.Negated);
		}

		private static DateTime ConvertToISO8601Date(string iso8601DateString)
		{
			if (string.IsNullOrEmpty(iso8601DateString))
			{
				return default(DateTime);
			}
			DateTime result;
			try
			{
				result = DateTime.ParseExact(iso8601DateString, "s", CultureInfo.InvariantCulture);
			}
			catch (FormatException innerException)
			{
				throw new ArgumentException(Resources.ExceptionInvalidDate, "dateString", innerException);
			}
			return result;
		}
	}
}
