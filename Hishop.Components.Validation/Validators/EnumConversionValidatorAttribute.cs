using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class EnumConversionValidatorAttribute : ValueValidatorAttribute
	{
		private Type enumType;

		public EnumConversionValidatorAttribute(Type enumType)
		{
			ValidatorArgumentsValidatorHelper.ValidateEnumConversionValidator(enumType);
			this.enumType = enumType;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new EnumConversionValidator(this.enumType, base.Negated);
		}
	}
}
