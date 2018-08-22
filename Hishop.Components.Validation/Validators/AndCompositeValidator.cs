using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation.Validators
{
	public class AndCompositeValidator : Validator
	{
		private IEnumerable<Validator> validators;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return null;
			}
		}

		public AndCompositeValidator(params Validator[] validators) : base(null, null)
		{
			this.validators = validators;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			foreach (Validator current in this.validators)
			{
				current.DoValidate(objectToValidate, currentTarget, key, validationResults);
			}
		}
	}
}
