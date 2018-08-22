using Hishop.Components.Validation.Properties;
using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation.Validators
{
	public class OrCompositeValidator : Validator
	{
		private IEnumerable<Validator> validators;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return Resources.OrCompositeValidatorDefaultMessageTemplate;
			}
		}

		public OrCompositeValidator(params Validator[] validators) : this(null, validators)
		{
		}

		public OrCompositeValidator(string messageTemplate, params Validator[] validators) : base(messageTemplate, null)
		{
			this.validators = validators;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			List<ValidationResult> list = new List<ValidationResult>();
			foreach (Validator current in this.validators)
			{
				ValidationResults validationResults2 = new ValidationResults();
				current.DoValidate(objectToValidate, currentTarget, key, validationResults2);
				if (validationResults2.IsValid)
				{
					return;
				}
				list.AddRange(validationResults2);
			}
			base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key, list);
		}
	}
}
