using System;

namespace Hishop.Components.Validation.Validators
{
	internal class RelativeDateTimeGenerator
	{
		internal DateTime GenerateBoundDateTime(int bound, DateTimeUnit unit, DateTime referenceDateTime)
		{
			DateTime result;
			switch (unit)
			{
			case DateTimeUnit.Second:
				result = referenceDateTime.AddSeconds((double)bound);
				break;
			case DateTimeUnit.Minute:
				result = referenceDateTime.AddMinutes((double)bound);
				break;
			case DateTimeUnit.Hour:
				result = referenceDateTime.AddHours((double)bound);
				break;
			case DateTimeUnit.Day:
				result = referenceDateTime.AddDays((double)bound);
				break;
			case DateTimeUnit.Month:
				result = referenceDateTime.AddMonths(bound);
				break;
			case DateTimeUnit.Year:
				result = referenceDateTime.AddYears(bound);
				break;
			default:
				result = referenceDateTime;
				break;
			}
			return result;
		}
	}
}
