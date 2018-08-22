using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class StringLengthValidator : ValueValidator<string>
	{
		private RangeChecker<int> rangeChecker;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.StringLengthValidatorNegatedDefaultMessageTemplate;
			}
		}

		public StringLengthValidator(int upperBound) : this(0, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public StringLengthValidator(int upperBound, bool negated) : this(0, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive, negated)
		{
		}

		public StringLengthValidator(int lowerBound, int upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{
		}

		public StringLengthValidator(int lowerBound, int upperBound, bool negated) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive, negated)
		{
		}

		public StringLengthValidator(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null)
		{
		}

		public StringLengthValidator(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null, negated)
		{
		}

		public StringLengthValidator(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, false)
		{
			this.rangeChecker = new RangeChecker<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
		}

		public StringLengthValidator(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
		{
			this.rangeChecker = new RangeChecker<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate != null)
			{
				if (this.rangeChecker.IsInRange(objectToValidate.Length) == base.Negated)
				{
					base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
					return;
				}
			}
			else
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
}
