using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	internal static class ValidatorArgumentsValidatorHelper
	{
		internal static void ValidateContainsCharacterValidator(string characterSet)
		{
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
		}

		internal static void ValidateDomainValidator(object domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
		}

		internal static void ValidateEnumConversionValidator(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
		}

		internal static void ValidateRangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundaryType, IComparable upperBound, RangeBoundaryType upperBoundaryType)
		{
			if (lowerBoundaryType != RangeBoundaryType.Ignore && lowerBound == null)
			{
				throw new ArgumentNullException("lowerBound");
			}
			if (upperBoundaryType != RangeBoundaryType.Ignore && upperBound == null)
			{
				throw new ArgumentNullException("upperBound");
			}
			if (lowerBoundaryType == RangeBoundaryType.Ignore && upperBoundaryType == RangeBoundaryType.Ignore)
			{
				throw new ArgumentException(Resources.ExceptionCannotIgnoreBothBoundariesInRange, "lowerBound");
			}
			if (lowerBound != null && upperBound != null && lowerBound.GetType() != upperBound.GetType())
			{
				throw new ArgumentException(Resources.ExceptionTypeOfBoundsMustMatch, "upperBound");
			}
		}

		internal static void ValidateRegexValidator(string pattern, string patternResourceName, Type patternResourceType)
		{
			if (pattern == null && (patternResourceName == null || patternResourceType == null))
			{
				throw new ArgumentNullException("pattern");
			}
			if (pattern == null && patternResourceName == null)
			{
				throw new ArgumentNullException("patternResourceName");
			}
			if (pattern == null && patternResourceType == null)
			{
				throw new ArgumentNullException("patternResourceType");
			}
		}

		internal static void ValidateRelativeDatimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
		{
			if ((lowerBound != 0 && lowerUnit == DateTimeUnit.None && lowerBoundType != RangeBoundaryType.Ignore) || (upperBound != 0 && upperUnit == DateTimeUnit.None && upperBoundType != RangeBoundaryType.Ignore))
			{
				throw new ArgumentException(Resources.RelativeDateTimeValidatorNotValidDateTimeUnit);
			}
		}

		internal static void ValidateTypeConversionValidator(Type targetType)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
		}
	}
}
