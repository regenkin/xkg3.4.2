using System;

namespace Hishop.Components.Validation.Validators
{
	public abstract class ValueValidator<T> : Validator<T>
	{
		private bool negated;

		public bool Negated
		{
			get
			{
				return this.negated;
			}
		}

		protected sealed override string DefaultMessageTemplate
		{
			get
			{
				if (this.negated)
				{
					return this.DefaultNegatedMessageTemplate;
				}
				return this.DefaultNonNegatedMessageTemplate;
			}
		}

		protected abstract string DefaultNonNegatedMessageTemplate
		{
			get;
		}

		protected abstract string DefaultNegatedMessageTemplate
		{
			get;
		}

		protected ValueValidator(string messageTemplate, string tag, bool negated) : base(messageTemplate, tag)
		{
			this.negated = negated;
		}
	}
	public abstract class ValueValidator : Validator
	{
		private bool negated;

		public bool Negated
		{
			get
			{
				return this.negated;
			}
		}

		protected sealed override string DefaultMessageTemplate
		{
			get
			{
				if (this.negated)
				{
					return this.DefaultNegatedMessageTemplate;
				}
				return this.DefaultNonNegatedMessageTemplate;
			}
		}

		protected abstract string DefaultNonNegatedMessageTemplate
		{
			get;
		}

		protected abstract string DefaultNegatedMessageTemplate
		{
			get;
		}

		protected ValueValidator(string messageTemplate, string tag, bool negated) : base(messageTemplate, tag)
		{
			this.negated = negated;
		}
	}
}
