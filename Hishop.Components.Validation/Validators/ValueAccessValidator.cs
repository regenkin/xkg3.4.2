using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	public class ValueAccessValidator : Validator
	{
		private ValueAccess valueAccess;

		private Validator valueValidator;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return Resources.ValueValidatorDefaultMessageTemplate;
			}
		}

		public ValueAccessValidator(ValueAccess valueAccess, Validator valueValidator) : base(null, null)
		{
			if (valueAccess == null)
			{
				throw new ArgumentNullException("valueAccess");
			}
			if (valueValidator == null)
			{
				throw new ArgumentNullException("valueValidator");
			}
			this.valueAccess = valueAccess;
			this.valueValidator = valueValidator;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate == null)
			{
				string messageTemplate = base.MessageTemplate;
				base.LogValidationResult(validationResults, messageTemplate, currentTarget, key);
				return;
			}
			object objectToValidate2;
			string message;
			bool value = this.valueAccess.GetValue(objectToValidate, out objectToValidate2, out message);
			if (value)
			{
				this.valueValidator.DoValidate(objectToValidate2, objectToValidate, this.valueAccess.Key, validationResults);
				return;
			}
			base.LogValidationResult(validationResults, message, currentTarget, this.valueAccess.Key);
		}
	}
}
