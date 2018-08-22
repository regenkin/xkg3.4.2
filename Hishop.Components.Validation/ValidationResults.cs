using System;
using System.Collections;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	[Serializable]
	public class ValidationResults : IEnumerable<ValidationResult>, IEnumerable
	{
		private List<ValidationResult> validationResults;

		public bool IsValid
		{
			get
			{
				return this.validationResults.Count == 0;
			}
		}

		public int Count
		{
			get
			{
				return this.validationResults.Count;
			}
		}

		public ValidationResults()
		{
			this.validationResults = new List<ValidationResult>();
		}

		public void AddResult(ValidationResult validationResult)
		{
			this.validationResults.Add(validationResult);
		}

		public void AddAllResults(IEnumerable<ValidationResult> sourceValidationResults)
		{
			this.validationResults.AddRange(sourceValidationResults);
		}

		public ValidationResults FindAll(TagFilter tagFilter, params string[] tags)
		{
			if (tags == null)
			{
				string[] array = new string[1];
				tags = array;
			}
			ValidationResults validationResults = new ValidationResults();
			foreach (ValidationResult current in ((IEnumerable<ValidationResult>)this))
			{
				bool flag = false;
				string[] array2 = tags;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if ((text == null && current.Tag == null) || (text != null && text.Equals(current.Tag)))
					{
						flag = true;
						break;
					}
				}
				if (flag ^ tagFilter == TagFilter.Ignore)
				{
					validationResults.AddResult(current);
				}
			}
			return validationResults;
		}

		IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator()
		{
			return this.validationResults.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.validationResults.GetEnumerator();
		}
	}
}
