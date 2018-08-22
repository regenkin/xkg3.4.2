using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class ValidatorCompositionAttribute : BaseValidationAttribute
	{
		private CompositionType compositionType;

		internal CompositionType CompositionType
		{
			get
			{
				return this.compositionType;
			}
		}

		public ValidatorCompositionAttribute(CompositionType compositionType)
		{
			this.compositionType = compositionType;
		}
	}
}
