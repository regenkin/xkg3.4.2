using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Hishop.Components.Validation.Validators
{
	public class RegexValidator : ValueValidator<string>
	{
		private string pattern;

		private RegexOptions options;

		private string patternResourceName;

		private Type patternResourceType;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.RegexValidatorNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.RegexValidatorNegatedDefaultMessageTemplate;
			}
		}

		public RegexValidator(string pattern) : this(pattern, RegexOptions.None)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType) : this(patternResourceName, patternResourceType, RegexOptions.None)
		{
		}

		public RegexValidator(string pattern, bool negated) : this(pattern, RegexOptions.None, negated)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, bool negated) : this(patternResourceName, patternResourceType, RegexOptions.None, negated)
		{
		}

		public RegexValidator(string pattern, RegexOptions options) : this(pattern, options, null)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, RegexOptions options) : this(patternResourceName, patternResourceType, options, null)
		{
		}

		public RegexValidator(string pattern, RegexOptions options, bool negated) : this(pattern, options, null, negated)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, RegexOptions options, bool negated) : this(patternResourceName, patternResourceType, options, null, negated)
		{
		}

		public RegexValidator(string pattern, string messageTemplate) : this(pattern, RegexOptions.None, messageTemplate)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, string messageTemplate) : this(patternResourceName, patternResourceType, RegexOptions.None, messageTemplate)
		{
		}

		public RegexValidator(string pattern, string messageTemplate, bool negated) : this(pattern, RegexOptions.None, messageTemplate, negated)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, string messageTemplate, bool negated) : this(patternResourceName, patternResourceType, RegexOptions.None, messageTemplate, negated)
		{
		}

		public RegexValidator(string pattern, RegexOptions options, string messageTemplate) : this(pattern, options, messageTemplate, false)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, RegexOptions options, string messageTemplate) : this(patternResourceName, patternResourceType, options, messageTemplate, false)
		{
		}

		public RegexValidator(string pattern, RegexOptions options, string messageTemplate, bool negated) : this(pattern, null, null, options, messageTemplate, negated)
		{
		}

		public RegexValidator(string patternResourceName, Type patternResourceType, RegexOptions options, string messageTemplate, bool negated) : this(null, patternResourceName, patternResourceType, options, messageTemplate, negated)
		{
		}

		public RegexValidator(string pattern, string patternResourceName, Type patternResourceType, RegexOptions options, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateRegexValidator(pattern, patternResourceName, patternResourceType);
			this.pattern = pattern;
			this.options = options;
			this.patternResourceName = patternResourceName;
			this.patternResourceType = patternResourceType;
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool flag = false;
			bool flag2 = objectToValidate == null;
			if (!flag2)
			{
				string text = this.GetPattern();
				Regex regex = new Regex(text, this.options);
				flag = !regex.IsMatch(objectToValidate);
			}
			if (flag2 || flag != base.Negated)
			{
				base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
			}
		}

		protected override string GetMessage(object objectToValidate, string key)
		{
			return string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[]
			{
				objectToValidate,
				key,
				base.Tag,
				this.pattern,
				this.options
			});
		}

		public string GetPattern()
		{
			if (!string.IsNullOrEmpty(this.pattern))
			{
				return this.pattern;
			}
			return ResourceLoader.LoadString(this.patternResourceType.FullName, this.patternResourceName, this.patternResourceType.Assembly);
		}
	}
}
