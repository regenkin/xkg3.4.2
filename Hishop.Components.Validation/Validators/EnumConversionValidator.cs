using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class EnumConversionValidator : ValueValidator<string>
	{
		private Type enumType;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.EnumConversionNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.EnumConversionNegatedDefaultMessageTemplate;
			}
		}

		public EnumConversionValidator(Type enumType) : this(enumType, false)
		{
		}

		public EnumConversionValidator(Type enumType, bool negated) : this(enumType, null, negated)
		{
		}

		public EnumConversionValidator(Type enumType, string messageTemplate) : this(enumType, messageTemplate, false)
		{
		}

		public EnumConversionValidator(Type enumType, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateEnumConversionValidator(enumType);
			this.enumType = enumType;
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool flag = false;
			bool flag2 = objectToValidate == null;
			if (!flag2)
			{
				flag = !Enum.IsDefined(this.enumType, objectToValidate);
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
				this.enumType.Name
			});
		}
	}
}
