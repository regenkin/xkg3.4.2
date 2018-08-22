using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class TypeConversionValidatorAttribute : ValueValidatorAttribute
	{
		private Type targetType;

		public TypeConversionValidatorAttribute(Type targetType)
		{
			ValidatorArgumentsValidatorHelper.ValidateTypeConversionValidator(targetType);
			this.targetType = targetType;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new TypeConversionValidator(this.targetType, base.Negated);
		}
	}
}
