using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	public class NotNullValidator : ValueValidator
	{
		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.NonNullNonNegatedValidatorDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.NonNullNegatedValidatorDefaultMessageTemplate;
			}
		}

		public NotNullValidator() : this(false)
		{
		}

		public NotNullValidator(bool negated) : this(negated, null)
		{
		}

		public NotNullValidator(string messageTemplate) : this(false, messageTemplate)
		{
		}

		public NotNullValidator(bool negated, string messageTemplate) : base(messageTemplate, null, negated)
		{
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (null == objectToValidate == !base.Negated)
			{
				base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
			}
		}
	}
}
