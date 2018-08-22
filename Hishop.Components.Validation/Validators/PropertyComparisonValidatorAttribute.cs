using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class PropertyComparisonValidatorAttribute : ValueValidatorAttribute
	{
		private string propertyToCompare;

		private ComparisonOperator comparisonOperator;

		public PropertyComparisonValidatorAttribute(string propertyToCompare, ComparisonOperator comparisonOperator)
		{
			if (propertyToCompare == null)
			{
				throw new ArgumentNullException("propertyToCompare");
			}
			this.propertyToCompare = propertyToCompare;
			this.comparisonOperator = comparisonOperator;
		}

		protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
		{
			PropertyInfo property = ValidationReflectionHelper.GetProperty(ownerType, this.propertyToCompare, false);
			if (property == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionPropertyToCompareNotFound, new object[]
				{
					this.propertyToCompare,
					ownerType.FullName
				}));
			}
			return new PropertyComparisonValidator(memberValueAccessBuilder.GetPropertyValueAccess(property), this.comparisonOperator, base.Negated);
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			throw new NotImplementedException(Resources.ExceptionShouldNotCall);
		}
	}
}
