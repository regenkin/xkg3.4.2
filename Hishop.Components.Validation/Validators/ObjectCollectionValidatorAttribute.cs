using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class ObjectCollectionValidatorAttribute : ValidatorAttribute
	{
		private Type targetType;

		private string targetRuleset;

		public ObjectCollectionValidatorAttribute(Type targetType) : this(targetType, string.Empty)
		{
		}

		public ObjectCollectionValidatorAttribute(Type targetType, string targetRuleset)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetRuleset == null)
			{
				throw new ArgumentNullException("targetRuleset");
			}
			this.targetType = targetType;
			this.targetRuleset = targetRuleset;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new ObjectCollectionValidator(this.targetType, this.targetRuleset);
		}
	}
}
