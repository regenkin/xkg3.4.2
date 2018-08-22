using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class RelativeDateTimeValidator : ValueValidator<DateTime>
	{
		private RangeChecker<DateTime> rangeChecker;

		private int lowerBound;

		private DateTimeUnit lowerUnit;

		private int upperBound;

		private DateTimeUnit upperUnit;

		private RelativeDateTimeGenerator generator;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.RelativeDateTimeNegatedDefaultMessageTemplate;
			}
		}

		internal int LowerBound
		{
			get
			{
				return this.lowerBound;
			}
		}

		internal DateTimeUnit LowerUnit
		{
			get
			{
				return this.lowerUnit;
			}
		}

		internal RangeBoundaryType LowerBoundType
		{
			get
			{
				return this.rangeChecker.LowerBoundType;
			}
		}

		internal int UpperBound
		{
			get
			{
				return this.upperBound;
			}
		}

		internal DateTimeUnit UpperUnit
		{
			get
			{
				return this.upperUnit;
			}
		}

		internal RangeBoundaryType UpperBoundType
		{
			get
			{
				return this.rangeChecker.UpperBoundType;
			}
		}

		internal RelativeDateTimeGenerator Generator
		{
			get
			{
				return this.generator;
			}
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate, negated)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, false)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, negated)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate)
		{
		}

		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate, negated)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit) : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit, bool negated) : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, false)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, null, negated)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, messageTemplate, false)
		{
		}

		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);
			this.lowerBound = lowerBound;
			this.lowerUnit = lowerUnit;
			this.upperBound = upperBound;
			this.upperUnit = upperUnit;
			this.generator = new RelativeDateTimeGenerator();
			DateTime now = DateTime.Now;
			DateTime dateTime = this.generator.GenerateBoundDateTime(lowerBound, lowerUnit, now);
			DateTime dateTime2 = this.generator.GenerateBoundDateTime(upperBound, upperUnit, now);
			this.rangeChecker = new RangeChecker<DateTime>(dateTime, lowerBoundType, dateTime2, upperBoundType);
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate == null)
			{
				base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
				return;
			}
			base.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}

		protected override void DoValidate(DateTime objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (this.rangeChecker.IsInRange(objectToValidate) == base.Negated)
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
				this.lowerBound,
				this.LowerUnit,
				this.upperBound,
				this.UpperUnit
			});
		}
	}
}
