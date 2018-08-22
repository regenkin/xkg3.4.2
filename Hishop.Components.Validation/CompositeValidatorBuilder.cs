using Hishop.Components.Validation.Properties;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	internal class CompositeValidatorBuilder
	{
		private IValidatedElement validatedElement;

		private List<Validator> valueValidators;

		private Validator builtValidator;

		public CompositeValidatorBuilder(IValidatedElement validatedElement)
		{
			this.validatedElement = validatedElement;
			this.valueValidators = new List<Validator>();
		}

		public Validator GetValidator()
		{
			this.builtValidator = this.DoGetValidator();
			return this.builtValidator;
		}

		protected virtual Validator DoGetValidator()
		{
			Validator validator;
			if (this.valueValidators.Count == 1)
			{
				validator = this.valueValidators[0];
			}
			else if (this.validatedElement.CompositionType == CompositionType.And)
			{
				validator = new AndCompositeValidator(this.valueValidators.ToArray());
			}
			else
			{
				validator = new OrCompositeValidator(this.valueValidators.ToArray());
				validator.MessageTemplate = this.validatedElement.CompositionMessageTemplate;
				validator.Tag = this.validatedElement.CompositionTag;
			}
			Validator validator2;
			if (this.validatedElement.IgnoreNulls)
			{
				validator2 = new OrCompositeValidator(new Validator[]
				{
					new NotNullValidator(true),
					validator
				});
				validator2.MessageTemplate = ((this.validatedElement.IgnoreNullsMessageTemplate != null) ? this.validatedElement.IgnoreNullsMessageTemplate : Resources.IgnoreNullsDefaultMessageTemplate);
				validator2.Tag = this.validatedElement.IgnoreNullsTag;
			}
			else
			{
				validator2 = validator;
			}
			return validator2;
		}

		internal void AddValueValidator(Validator valueValidator)
		{
			this.valueValidators.Add(valueValidator);
		}
	}
}
