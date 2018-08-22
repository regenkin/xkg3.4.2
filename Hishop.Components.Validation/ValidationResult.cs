using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	[Serializable]
	public class ValidationResult
	{
		private string message;

		private string key;

		private string tag;

		[NonSerialized]
		private object target;

		[NonSerialized]
		private Validator validator;

		private IEnumerable<ValidationResult> nestedValidationResults;

		private static readonly IEnumerable<ValidationResult> NoNestedValidationResults = new ValidationResult[0];

		public string Key
		{
			get
			{
				return this.key;
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}
		}

		public string Tag
		{
			get
			{
				return this.tag;
			}
		}

		public object Target
		{
			get
			{
				return this.target;
			}
		}

		public Validator Validator
		{
			get
			{
				return this.validator;
			}
		}

		public IEnumerable<ValidationResult> NestedValidationResults
		{
			get
			{
				return this.nestedValidationResults;
			}
		}

		public ValidationResult(string message, object target, string key, string tag, Validator validator) : this(message, target, key, tag, validator, ValidationResult.NoNestedValidationResults)
		{
		}

		public ValidationResult(string message, object target, string key, string tag, Validator validator, IEnumerable<ValidationResult> nestedValidationResults)
		{
			this.message = message;
			this.key = key;
			this.target = target;
			this.tag = tag;
			this.validator = validator;
			this.nestedValidationResults = nestedValidationResults;
		}
	}
}
