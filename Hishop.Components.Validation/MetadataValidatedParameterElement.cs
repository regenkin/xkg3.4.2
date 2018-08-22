using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatedParameterElement : IValidatedElement
	{
		private ParameterInfo parameterInfo;

		private IgnoreNullsAttribute ignoreNullsAttribute;

		private ValidatorCompositionAttribute validatorCompositionAttribute;

		public CompositionType CompositionType
		{
			get
			{
				if (this.validatorCompositionAttribute == null)
				{
					return CompositionType.And;
				}
				return this.validatorCompositionAttribute.CompositionType;
			}
		}

		public string CompositionMessageTemplate
		{
			get
			{
				if (this.validatorCompositionAttribute == null)
				{
					return null;
				}
				return this.validatorCompositionAttribute.GetMessageTemplate();
			}
		}

		public string CompositionTag
		{
			get
			{
				if (this.validatorCompositionAttribute == null)
				{
					return null;
				}
				return this.validatorCompositionAttribute.Tag;
			}
		}

		public bool IgnoreNulls
		{
			get
			{
				return this.ignoreNullsAttribute != null;
			}
		}

		public string IgnoreNullsMessageTemplate
		{
			get
			{
				if (this.ignoreNullsAttribute == null)
				{
					return null;
				}
				return this.ignoreNullsAttribute.GetMessageTemplate();
			}
		}

		public string IgnoreNullsTag
		{
			get
			{
				if (this.ignoreNullsAttribute == null)
				{
					return null;
				}
				return this.ignoreNullsAttribute.Tag;
			}
		}

		public MemberInfo MemberInfo
		{
			get
			{
				return null;
			}
		}

		public Type TargetType
		{
			get
			{
				return null;
			}
		}

		public IEnumerable<IValidatorDescriptor> GetValidatorDescriptors()
		{
			if (this.parameterInfo != null)
			{
				try
				{
					object[] customAttributes = this.parameterInfo.GetCustomAttributes(typeof(ValidatorAttribute), false);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						object obj = customAttributes[i];
						yield return (IValidatorDescriptor)obj;
					}
				}
				finally
				{
				}
			}
			yield break;
		}

		public void UpdateFlyweight(ParameterInfo parameterInfo)
		{
			this.parameterInfo = parameterInfo;
			this.ignoreNullsAttribute = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(parameterInfo, string.Empty);
			this.validatorCompositionAttribute = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(parameterInfo, string.Empty);
		}
	}
}
