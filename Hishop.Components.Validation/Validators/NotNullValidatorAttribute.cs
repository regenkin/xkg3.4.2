using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class NotNullValidatorAttribute : ValueValidatorAttribute
	{
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new NotNullValidator(base.Negated);
		}
	}
}
