using System;

namespace Hishop.Components.Validation.Validators
{
	public sealed class ValidatorWrapper : Validator
	{
		private Validator wrappedValidator;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return null;
			}
		}

		public ValidatorWrapper(Validator wrappedValidator) : base(null, null)
		{
			this.wrappedValidator = wrappedValidator;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			this.wrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}
	}
}
