using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public abstract class ValidatorAttribute : BaseValidationAttribute, IValidatorDescriptor
	{
		Validator IValidatorDescriptor.CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
		{
			Validator validator = this.DoCreateValidator(targetType, ownerType, memberValueAccessBuilder);
			validator.Tag = base.Tag;
			validator.MessageTemplate = base.GetMessageTemplate();
			return validator;
		}

		protected virtual Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
		{
			return this.DoCreateValidator(targetType);
		}

		protected abstract Validator DoCreateValidator(Type targetType);
	}
}
