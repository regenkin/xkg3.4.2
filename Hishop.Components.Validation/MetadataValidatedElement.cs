using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatedElement : IValidatedElement
	{
		private MemberInfo memberInfo;

		private IgnoreNullsAttribute ignoreNullsAttribute;

		private ValidatorCompositionAttribute validatorCompositionAttribute;

		private Type targetType;

		private string ruleset;

		protected string Ruleset
		{
			get
			{
				return this.ruleset;
			}
		}

		protected Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		CompositionType IValidatedElement.CompositionType
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

		string IValidatedElement.CompositionMessageTemplate
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

		string IValidatedElement.CompositionTag
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

		bool IValidatedElement.IgnoreNulls
		{
			get
			{
				return this.ignoreNullsAttribute != null;
			}
		}

		string IValidatedElement.IgnoreNullsMessageTemplate
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

		string IValidatedElement.IgnoreNullsTag
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

		MemberInfo IValidatedElement.MemberInfo
		{
			get
			{
				return this.memberInfo;
			}
		}

		Type IValidatedElement.TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		public MetadataValidatedElement(string ruleset)
		{
			this.ruleset = ruleset;
		}

		public MetadataValidatedElement(FieldInfo fieldInfo, string ruleset) : this(ruleset)
		{
			this.UpdateFlyweight(fieldInfo);
		}

		public MetadataValidatedElement(MethodInfo methodInfo, string ruleset) : this(ruleset)
		{
			this.UpdateFlyweight(methodInfo);
		}

		public MetadataValidatedElement(PropertyInfo propertyInfo, string ruleset) : this(ruleset)
		{
			this.UpdateFlyweight(propertyInfo);
		}

		public MetadataValidatedElement(Type type, string ruleset) : this(ruleset)
		{
			this.UpdateFlyweight(type);
		}

		public void UpdateFlyweight(FieldInfo fieldInfo)
		{
			this.UpdateFlyweight(fieldInfo, fieldInfo.FieldType);
		}

		public void UpdateFlyweight(MethodInfo methodInfo)
		{
			this.UpdateFlyweight(methodInfo, methodInfo.ReturnType);
		}

		public void UpdateFlyweight(PropertyInfo propertyInfo)
		{
			this.UpdateFlyweight(propertyInfo, propertyInfo.PropertyType);
		}

		public void UpdateFlyweight(Type type)
		{
			this.UpdateFlyweight(type, type);
		}

		private void UpdateFlyweight(MemberInfo memberInfo, Type targetType)
		{
			this.memberInfo = memberInfo;
			this.targetType = targetType;
			this.ignoreNullsAttribute = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(memberInfo, this.ruleset);
			this.validatorCompositionAttribute = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(memberInfo, this.ruleset);
		}

		IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
		{
			if (this.memberInfo != null)
			{
				try
				{
					object[] customAttributes = this.memberInfo.GetCustomAttributes(typeof(ValidatorAttribute), false);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						ValidatorAttribute validatorAttribute = (ValidatorAttribute)customAttributes[i];
						if (this.ruleset.Equals(validatorAttribute.Ruleset))
						{
							yield return validatorAttribute;
						}
					}
				}
				finally
				{
				}
			}
			yield break;
		}
	}
}
