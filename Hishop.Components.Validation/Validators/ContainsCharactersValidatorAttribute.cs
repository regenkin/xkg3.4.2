using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class ContainsCharactersValidatorAttribute : ValueValidatorAttribute
	{
		private string characterSet;

		private ContainsCharacters containsCharacters;

		public ContainsCharactersValidatorAttribute(string characterSet) : this(characterSet, ContainsCharacters.Any)
		{
		}

		public ContainsCharactersValidatorAttribute(string characterSet, ContainsCharacters containsCharacters)
		{
			ValidatorArgumentsValidatorHelper.ValidateContainsCharacterValidator(characterSet);
			this.characterSet = characterSet;
			this.containsCharacters = containsCharacters;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new ContainsCharactersValidator(this.characterSet, this.containsCharacters, base.Negated);
		}
	}
}
