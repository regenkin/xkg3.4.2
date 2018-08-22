using System;

namespace Hishop.Components.Validation.Validators
{
	public class PropertyValueValidator<T> : MemberAccessValidator<T>
	{
		public PropertyValueValidator(string propertyName, Validator propertyValueValidator) : base(PropertyValueValidator<T>.GetPropertyValueAccess(propertyName), propertyValueValidator)
		{
		}

		private static ValueAccess GetPropertyValueAccess(string propertyName)
		{
			return new PropertyValueAccess(ValidationReflectionHelper.GetProperty(typeof(T), propertyName, true));
		}
	}
}
