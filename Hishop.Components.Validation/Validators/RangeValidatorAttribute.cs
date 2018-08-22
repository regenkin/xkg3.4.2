using Hishop.Components.Validation.Properties;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class RangeValidatorAttribute : ValueValidatorAttribute
	{
		private IComparable lowerBound;

		private RangeBoundaryType lowerBoundType;

		private IComparable upperBound;

		private RangeBoundaryType upperBoundType;

		internal IComparable LowerBound
		{
			get
			{
				return this.lowerBound;
			}
		}

		internal RangeBoundaryType LowerBoundType
		{
			get
			{
				return this.lowerBoundType;
			}
		}

		internal IComparable UpperBound
		{
			get
			{
				return this.upperBound;
			}
		}

		internal RangeBoundaryType UpperBoundType
		{
			get
			{
				return this.upperBoundType;
			}
		}

        public RangeValidatorAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(double lowerBound, RangeBoundaryType lowerBoundType, double upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(long lowerBound, RangeBoundaryType lowerBoundType, long upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(float lowerBound, RangeBoundaryType lowerBoundType, float upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        public RangeValidatorAttribute(Type boundType, string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this(RangeValidatorAttribute.ConvertBound(boundType, lowerBound, "lowerBound"), lowerBoundType, RangeValidatorAttribute.ConvertBound(boundType, upperBound, "upperBound"), upperBoundType)
		{
		}

		private RangeValidatorAttribute(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType)
		{
			ValidatorArgumentsValidatorHelper.ValidateRangeValidator(lowerBound, lowerBoundType, upperBound, upperBoundType);
			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		private static IComparable ConvertBound(Type boundType, string bound, string boundParameter)
		{
			if (boundType == null)
			{
				throw new ArgumentNullException("boundType");
			}
			if (!typeof(IComparable).IsAssignableFrom(boundType))
			{
				throw new ArgumentException(Resources.ExceptionBoundTypeNotIComparable, "boundType");
			}
			if (bound == null)
			{
				return null;
			}
			IComparable result;
			if (boundType == typeof(DateTime))
			{
				try
				{
					result = DateTime.ParseExact(bound, "s", CultureInfo.InvariantCulture);
					return result;
				}
				catch (FormatException innerException)
				{
					throw new ArgumentException(Resources.ExceptionInvalidDate, boundParameter, innerException);
				}
			}
			try
			{
				result = (IComparable)TypeDescriptor.GetConverter(boundType).ConvertFrom(null, CultureInfo.InvariantCulture, bound);
			}
			catch (Exception innerException2)
			{
				throw new ArgumentException(Resources.ExceptionCannotConvertBound, innerException2);
			}
			return result;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RangeValidator(this.lowerBound, this.lowerBoundType, this.upperBound, this.upperBoundType, base.Negated);
		}
	}
}
