using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public abstract class ValueValidatorAttribute : ValidatorAttribute
	{
		private bool negated;

		public bool Negated
		{
			get
			{
				return this.negated;
			}
			set
			{
				this.negated = value;
			}
		}
	}
}
