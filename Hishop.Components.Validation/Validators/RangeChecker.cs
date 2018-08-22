using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	internal class RangeChecker<T> where T : IComparable
	{
		private T lowerBound;

		private RangeBoundaryType lowerBoundType;

		private T upperBound;

		private RangeBoundaryType upperBoundType;

		internal T LowerBound
		{
			get
			{
				return this.lowerBound;
			}
		}

		internal T UpperBound
		{
			get
			{
				return this.upperBound;
			}
		}

		internal RangeBoundaryType LowerBoundType
		{
			get
			{
				return this.lowerBoundType;
			}
		}

		internal RangeBoundaryType UpperBoundType
		{
			get
			{
				return this.upperBoundType;
			}
		}

		public RangeChecker(T lowerBound, RangeBoundaryType lowerBoundType, T upperBound, RangeBoundaryType upperBoundType)
		{
			if (upperBound == null && upperBoundType != RangeBoundaryType.Ignore)
			{
				throw new ArgumentException(Resources.ExceptionUpperBoundNull);
			}
			if (lowerBound == null && lowerBoundType != RangeBoundaryType.Ignore)
			{
				throw new ArgumentException(Resources.ExceptionLowerBoundNull);
			}
			if (lowerBoundType != RangeBoundaryType.Ignore && upperBoundType != RangeBoundaryType.Ignore && upperBound.CompareTo(lowerBound) < 0)
			{
				throw new ArgumentException(Resources.ExceptionUpperBoundLowerThanLowerBound);
			}
			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		public bool IsInRange(T target)
		{
			if (this.lowerBoundType > RangeBoundaryType.Ignore)
			{
				int num = this.lowerBound.CompareTo(target);
				if (num > 0)
				{
					return false;
				}
				if (this.lowerBoundType == RangeBoundaryType.Exclusive && num == 0)
				{
					return false;
				}
			}
			if (this.upperBoundType > RangeBoundaryType.Ignore)
			{
				int num2 = this.upperBound.CompareTo(target);
				if (num2 < 0)
				{
					return false;
				}
				if (this.upperBoundType == RangeBoundaryType.Exclusive && num2 == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
