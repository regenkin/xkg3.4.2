using Hishop.Components.Validation.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hishop.Components.Validation
{
	public abstract class Validator
	{
		private string messageTemplate;

		private string tag;

		protected abstract string DefaultMessageTemplate
		{
			get;
		}

		public string MessageTemplate
		{
			get
			{
				if (this.messageTemplate == null)
				{
					return this.DefaultMessageTemplate;
				}
				return this.messageTemplate;
			}
			set
			{
				this.messageTemplate = value;
			}
		}

		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}

		protected Validator(string messageTemplate, string tag)
		{
			this.messageTemplate = messageTemplate;
			this.tag = tag;
		}

		public ValidationResults Validate(object target)
		{
			ValidationResults validationResults = new ValidationResults();
			this.DoValidate(target, target, null, validationResults);
			return validationResults;
		}

		public void Validate(object target, ValidationResults validationResults)
		{
			if (validationResults == null)
			{
				throw new ArgumentNullException("validationResults");
			}
			this.DoValidate(target, target, null, validationResults);
		}

		protected internal abstract void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults);

		protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key)
		{
			validationResults.AddResult(new ValidationResult(message, target, key, this.Tag, this));
		}

		protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key, IEnumerable<ValidationResult> nestedValidationResults)
		{
			validationResults.AddResult(new ValidationResult(message, target, key, this.Tag, this, nestedValidationResults));
		}

		protected virtual string GetMessage(object objectToValidate, string key)
		{
			return string.Format(CultureInfo.CurrentCulture, this.MessageTemplate, new object[]
			{
				objectToValidate,
				key,
				this.Tag
			});
		}
	}
	public abstract class Validator<T> : Validator
	{
		protected Validator(string messageTemplate, string tag) : base(messageTemplate, tag)
		{
		}

		public ValidationResults Validate(T target)
		{
			ValidationResults validationResults = new ValidationResults();
			this.Validate(target, validationResults);
			return validationResults;
		}

		public void Validate(T target, ValidationResults validationResults)
		{
			if (validationResults == null)
			{
				throw new ArgumentNullException("validationResults");
			}
			this.DoValidate(target, target, null, validationResults);
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate != null && !(objectToValidate is T))
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidTargetType, new object[]
				{
					typeof(T).FullName,
					objectToValidate.GetType().FullName
				});
				base.LogValidationResult(validationResults, message, currentTarget, key);
				return;
			}
			this.DoValidate((T)((object)objectToValidate), currentTarget, key, validationResults);
		}

		protected abstract void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults);
	}
}
