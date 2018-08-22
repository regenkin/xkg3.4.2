using System;

namespace Hishop.Components.Validation.Validators
{
	internal sealed class GenericValidatorWrapper<T> : Validator<T>
	{
		private Validator wrappedValidator;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return null;
			}
		}

		public GenericValidatorWrapper(Validator wrappedValidator) : base(null, null)
		{
			this.wrappedValidator = wrappedValidator;
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			this.wrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}
	}
}
