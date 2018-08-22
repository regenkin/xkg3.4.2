using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class ValueAccessComparisonValidator : ValueValidator
	{
		private ValueAccess valueAccess;

		private ComparisonOperator comparisonOperator;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.ValueAccessComparisonValidatorNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.ValueAccessComparisonValidatorNegatedDefaultMessageTemplate;
			}
		}

		public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator) : this(valueAccess, comparisonOperator, null, null)
		{
		}

		public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, string tag) : this(valueAccess, comparisonOperator, messageTemplate, tag, false)
		{
			this.valueAccess = valueAccess;
			this.comparisonOperator = comparisonOperator;
		}

		public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, bool negated) : this(valueAccess, comparisonOperator, messageTemplate, null, negated)
		{
			this.valueAccess = valueAccess;
			this.comparisonOperator = comparisonOperator;
		}

		public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, string tag, bool negated) : base(messageTemplate, tag, negated)
		{
			if (valueAccess == null)
			{
				throw new ArgumentNullException("valueAccess");
			}
			this.valueAccess = valueAccess;
			this.comparisonOperator = comparisonOperator;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			object obj;
			string text;
			if (!this.valueAccess.GetValue(currentTarget, out obj, out text))
			{
				base.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, Resources.ValueAccessComparisonValidatorFailureToRetrieveComparand, new object[]
				{
					this.valueAccess.Key,
					text
				}), currentTarget, key);
				return;
			}
			bool flag = false;
			if (this.comparisonOperator == ComparisonOperator.Equal || this.comparisonOperator == ComparisonOperator.NotEqual)
			{
				flag = (((objectToValidate != null) ? objectToValidate.Equals(obj) : (obj == null)) ^ this.comparisonOperator == ComparisonOperator.NotEqual ^ base.Negated);
			}
			else
			{
				IComparable comparable = objectToValidate as IComparable;
				if (comparable != null && obj != null && comparable.GetType() == obj.GetType())
				{
					int num = comparable.CompareTo(obj);
					switch (this.comparisonOperator)
					{
					case ComparisonOperator.GreaterThan:
						flag = (num > 0);
						break;
					case ComparisonOperator.GreaterThanEqual:
						flag = (num >= 0);
						break;
					case ComparisonOperator.LessThan:
						flag = (num < 0);
						break;
					case ComparisonOperator.LessThanEqual:
						flag = (num <= 0);
						break;
					}
					flag ^= base.Negated;
				}
				else
				{
					flag = false;
				}
			}
			if (!flag)
			{
				base.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[]
				{
					objectToValidate,
					key,
					base.Tag,
					obj,
					this.valueAccess.Key,
					this.comparisonOperator
				}), currentTarget, key);
			}
		}
	}
}
