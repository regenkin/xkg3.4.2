using Hishop.Components.Validation.Validators;
using System;

namespace Hishop.Components.Validation
{
	internal class ValueAccessValidatorBuilder : CompositeValidatorBuilder
	{
		private ValueAccess valueAccess;

		public ValueAccessValidatorBuilder(ValueAccess valueAccess, IValidatedElement validatedElement) : base(validatedElement)
		{
			this.valueAccess = valueAccess;
		}

		protected override Validator DoGetValidator()
		{
			return new ValueAccessValidator(this.valueAccess, base.DoGetValidator());
		}
	}
}
